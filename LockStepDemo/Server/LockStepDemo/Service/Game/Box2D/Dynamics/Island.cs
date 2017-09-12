/*
  Box2DX Copyright (c) 2009 Ihar Kalasouski http://code.google.com/p/box2dx
  Box2D original C++ version Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.
*/

/*
Position Correction Notes
=========================
I tried the several algorithms for position correction of the 2D revolute joint.
I looked at these systems:
- simple pendulum (1m diameter sphere on massless 5m stick) with initial angular velocity of 100 rad/s.
- suspension bridge with 30 1m long planks of length 1m.
- multi-link chain with 30 1m long links.

Here are the algorithms:

Baumgarte - A fraction of the position error is added to the velocity error. There is no
separate position solver.

Pseudo Velocities - After the velocity solver and position integration,
the position error, Jacobian, and effective mass are recomputed. Then
the velocity constraints are solved with pseudo velocities and a fraction
of the position error is added to the pseudo velocity error. The pseudo
velocities are initialized to zero and there is no warm-starting. After
the position solver, the pseudo velocities are added to the positions.
This is also called the First Order World method or the Position LCP method.

Modified Nonlinear Gauss-Seidel (NGS) - Like Pseudo Velocities except the
position error is re-computed for each constraint and the positions are updated
after the constraint is solved. The radius vectors (aka Jacobians) are
re-computed too (otherwise the algorithm has horrible instability). The pseudo
velocity states are not needed because they are effectively zero at the beginning
of each iteration. Since we have the current position error, we allow the
iterations to terminate early if the error becomes smaller than b2_linearSlop.

Full NGS or just NGS - Like Modified NGS except the effective mass are re-computed
each time a constraint is solved.

Here are the results:
Baumgarte - this is the cheapest algorithm but it has some stability problems,
especially with the bridge. The chain links separate easily close to the root
and they jitter as they struggle to pull together. This is one of the most common
methods in the field. The big drawback is that the position correction artificially
affects the momentum, thus leading to instabilities and false bounce. I used a
bias factor of 0.2. A larger bias factor makes the bridge less stable, a smaller
factor makes joints and contacts more spongy.

Pseudo Velocities - the is more stable than the Baumgarte method. The bridge is
stable. However, joints still separate with large angular velocities. Drag the
simple pendulum in a circle quickly and the joint will separate. The chain separates
easily and does not recover. I used a bias factor of 0.2. A larger value lead to
the bridge collapsing when a heavy cube drops on it.

Modified NGS - this algorithm is better in some ways than Baumgarte and Pseudo
Velocities, but in other ways it is worse. The bridge and chain are much more
stable, but the simple pendulum goes unstable at high angular velocities.

Full NGS - stable in all tests. The joints display good stiffness. The bridge
still sags, but this is better than infinite forces.

Recommendations
Pseudo Velocities are not really worthwhile because the bridge and chain cannot
recover from joint separation. In other cases the benefit over Baumgarte is small.

Modified NGS is not a robust method for the revolute joint due to the violent
instability seen in the simple pendulum. Perhaps it is viable with other constraint
types, especially scalar constraints where the effective mass is a scalar.

This leaves Baumgarte and Full NGS. Baumgarte has small, but manageable instabilities
and is very fast. I don't think we can escape Baumgarte, especially in highly
demanding cases where high constraint fidelity is not needed.

Full NGS is robust and easy on the eyes. I recommend this as an option for
higher fidelity simulation and certainly for suspension bridges and long chains.
Full NGS might be a good choice for ragdolls, especially motorized ragdolls where
joint separation can be problematic. The number of NGS iterations can be reduced
for better performance without harming robustness much.

Each joint in a can be handled differently in the position solver. So I recommend
a system where the user can select the algorithm on a per joint basis. I would
probably default to the slower Full NGS and let the user select the faster
Baumgarte method in performance critical scenarios.
*/

/*
Cache Performance

The Box2D solvers are dominated by cache misses. Data structures are designed
to increase the number of cache hits. Much of misses are due to random access
to body data. The constraint structures are iterated over linearly, which leads
to few cache misses.

The _bodies are not accessed during iteration. Instead read only data, such as
the mass values are stored with the constraints. The mutable data are the constraint
impulses and the _bodies velocities/positions. The impulses are held inside the
constraint structures. The body velocities/positions are held in compact, temporary
arrays to increase the number of cache hits. Linear and angular velocity are
stored in a single array since multiple arrays lead to multiple misses.
*/

/*
2D Rotation

R = [cos(theta) -sin(theta)]
    [sin(theta) cos(theta) ]

thetaDot = omega

Let q1 = cos(theta), q2 = sin(theta).
R = [q1 -q2]
    [q2  q1]

q1Dot = -thetaDot * q2
q2Dot = thetaDot * q1

q1_new = q1_old - dt * w * q2
q2_new = q2_old + dt * w * q1
then normalize.

This might be faster than computing sin+cos.
However, we can compute sin+cos of the same angle fast.
*/

using System;
using System.Collections.Generic;
using System.Text;

using Box2DX.Common;
using Box2DX.Collision;

namespace Box2DX.Dynamics
{
	public struct Position
	{
		public Vec2 x;
		public float a;
	}

	public struct Velocity
	{
		public Vec2 v;
		public float w;
	}

	public class Island : IDisposable
	{
		public ContactListener _listener;

		public Body[] _bodies;
		public Contact[] _contacts;
		public Joint[] _joints;

		public Position[] _positions;
		public Velocity[] _velocities;

		public int _bodyCount;
		public int _jointCount;
		public int _contactCount;

		public int _bodyCapacity;
		public int _contactCapacity;
		public int _jointCapacity;

		public int _positionIterationCount;

		public Island(int bodyCapacity, int contactCapacity, int jointCapacity, ContactListener listener)
		{
			_bodyCapacity = bodyCapacity;
			_contactCapacity = contactCapacity;
			_jointCapacity = jointCapacity;
			//__bodyCount = 0;
			//_contactCount = 0;
			//_jointCount = 0;

			_listener = listener;

			_bodies = new Body[bodyCapacity];
			_contacts = new Contact[contactCapacity];
			_joints = new Joint[jointCapacity];

			_velocities = new Velocity[_bodyCapacity];
			_positions = new Position[_bodyCapacity];
		}

		public void Dispose()
		{
			// Warning: the order should reverse the constructor order.
			_positions = null;
			_velocities = null;
			_joints = null;
			_contacts = null;
			_bodies = null;
		}

		public void Clear()
		{
			_bodyCount = 0;
			_contactCount = 0;
			_jointCount = 0;
		}

		public void Solve(TimeStep step, Vec2 gravity, bool allowSleep)
		{
			// Integrate velocities and apply damping.
			for (int i = 0; i < _bodyCount; ++i)
			{
				Body b = _bodies[i];

				if (b.IsStatic())
					continue;

				// Integrate velocities.
				b._linearVelocity += step.Dt * (gravity + b._invMass * b._force);
				b._angularVelocity += step.Dt * b._invI * b._torque;

				// Reset forces.
				b._force.Set(0.0f, 0.0f);
				b._torque = 0.0f;

				// Apply damping.
				// ODE: dv/dt + c * v = 0
				// Solution: v(t) = v0 * exp(-c * t)
				// Time step: v(t + dt) = v0 * exp(-c * (t + dt)) = v0 * exp(-c * t) * exp(-c * dt) = v * exp(-c * dt)
				// v2 = exp(-c * dt) * v1
				// Taylor expansion:
				// v2 = (1.0f - c * dt) * v1
				b._linearVelocity *= Common.Math.Clamp(1.0f - step.Dt * b._linearDamping, 0.0f, 1.0f);
				b._angularVelocity *= Common.Math.Clamp(1.0f - step.Dt * b._angularDamping, 0.0f, 1.0f);
			}

			ContactSolver contactSolver = new ContactSolver(step, _contacts, _contactCount);

			// Initialize velocity constraints.
			contactSolver.InitVelocityConstraints(step);

			for (int i = 0; i < _jointCount; ++i)
			{
				_joints[i].InitVelocityConstraints(step);
			}

			// Solve velocity constraints.
			for (int i = 0; i < step.VelocityIterations; ++i)
			{
				for (int j = 0; j < _jointCount; ++j)
				{
					_joints[j].SolveVelocityConstraints(step);
				}
				contactSolver.SolveVelocityConstraints();
			}

			// Post-solve (store impulses for warm starting).
			contactSolver.FinalizeVelocityConstraints();

			// Integrate positions.
			for (int i = 0; i < _bodyCount; ++i)
			{
				Body b = _bodies[i];

				if (b.IsStatic())
					continue;

				// Check for large velocities.
				Vec2 translation = step.Dt * b._linearVelocity;
				if (Common.Vec2.Dot(translation, translation) > Settings.MaxTranslationSquared)
				{
					translation.Normalize();
					b._linearVelocity = (Settings.MaxTranslation * step.Inv_Dt) * translation;
				}

				float rotation = step.Dt * b._angularVelocity;
				if (rotation * rotation > Settings.MaxRotationSquared)
				{
					if (rotation < 0.0)
					{
						b._angularVelocity = -step.Inv_Dt * Settings.MaxRotation;
					}
					else
					{
						b._angularVelocity = step.Inv_Dt * Settings.MaxRotation;
					}
				}

				// Store positions for continuous collision.
				b._sweep.C0 = b._sweep.C;
				b._sweep.A0 = b._sweep.A;

				// Integrate
				b._sweep.C += step.Dt * b._linearVelocity;
				b._sweep.A += step.Dt * b._angularVelocity;

				// Compute new transform
				b.SynchronizeTransform();

				// Note: shapes are synchronized later.
			}

			// Iterate over constraints.
			for (int i = 0; i < step.PositionIterations; ++i)
			{
				bool contactsOkay = contactSolver.SolvePositionConstraints(Settings.ContactBaumgarte);

				bool jointsOkay = true;
				for (int j = 0; j < _jointCount; ++j)
				{
					bool jointOkay = _joints[j].SolvePositionConstraints(Settings.ContactBaumgarte);
					jointsOkay = jointsOkay && jointOkay;
				}

				if (contactsOkay && jointsOkay)
				{
					// Exit early if the position errors are small.
					break;
				}
			}

			Report(contactSolver._constraints);

			if (allowSleep)
			{
				float minSleepTime = Settings.FLT_MAX;

#if !TARGET_FLOAT32_IS_FIXED
				float linTolSqr = Settings.LinearSleepTolerance * Settings.LinearSleepTolerance;
				float angTolSqr = Settings.AngularSleepTolerance * Settings.AngularSleepTolerance;
#endif

				for (int i = 0; i < _bodyCount; ++i)
				{
					Body b = _bodies[i];
					if (b._invMass == 0.0f)
					{
						continue;
					}

					if ((b._flags & Body.BodyFlags.AllowSleep) == 0)
					{
						b._sleepTime = 0.0f;
						minSleepTime = 0.0f;
					}

					if ((b._flags & Body.BodyFlags.AllowSleep) == 0 ||
#if TARGET_FLOAT32_IS_FIXED
						Common.Math.Abs(b._angularVelocity) > Settings.AngularSleepTolerance ||
						Common.Math.Abs(b._linearVelocity.X) > Settings.LinearSleepTolerance ||
						Common.Math.Abs(b._linearVelocity.Y) > Settings.LinearSleepTolerance)
#else
 b._angularVelocity * b._angularVelocity > angTolSqr ||
						Vec2.Dot(b._linearVelocity, b._linearVelocity) > linTolSqr)
#endif
					{
						b._sleepTime = 0.0f;
						minSleepTime = 0.0f;
					}
					else
					{
						b._sleepTime += step.Dt;
						minSleepTime = Common.Math.Min(minSleepTime, b._sleepTime);
					}
				}

				if (minSleepTime >= Settings.TimeToSleep)
				{
					for (int i = 0; i < _bodyCount; ++i)
					{
						Body b = _bodies[i];
						b._flags |= Body.BodyFlags.Sleep;
						b._linearVelocity = Vec2.Zero;
						b._angularVelocity = 0.0f;
					}
				}
			}
		}

		public void SolveTOI(ref TimeStep subStep)
		{
			ContactSolver contactSolver = new ContactSolver(subStep, _contacts, _contactCount);

			// No warm starting is needed for TOI events because warm
			// starting impulses were applied in the discrete solver.

			// Warm starting for joints is off for now, but we need to
			// call this function to compute Jacobians.
			for (int i = 0; i < _jointCount; ++i)
			{
				_joints[i].InitVelocityConstraints(subStep);
			}

			// Solve velocity constraints.
			for (int i = 0; i < subStep.VelocityIterations; ++i)
			{
				contactSolver.SolveVelocityConstraints();
				for (int j = 0; j < _jointCount; ++j)
				{
					_joints[j].SolveVelocityConstraints(subStep);
				}
			}

			// Don't store the TOI contact forces for warm starting
			// because they can be quite large.

			// Integrate positions.
			for (int i = 0; i < _bodyCount; ++i)
			{
				Body b = _bodies[i];

				if (b.IsStatic())
					continue;

				// Check for large velocities.
				Vec2 translation = subStep.Dt * b._linearVelocity;
				if (Vec2.Dot(translation, translation) > Settings.MaxTranslationSquared)
				{
					translation.Normalize();
					b._linearVelocity = (Settings.MaxTranslation * subStep.Inv_Dt) * translation;
				}

				float rotation = subStep.Dt * b._angularVelocity;
				if (rotation * rotation > Settings.MaxRotationSquared)
				{
					if (rotation < 0.0)
					{
						b._angularVelocity = -subStep.Inv_Dt * Settings.MaxRotation;
					}
					else
					{
						b._angularVelocity = subStep.Inv_Dt * Settings.MaxRotation;
					}
				}

				// Store positions for continuous collision.
				b._sweep.C0 = b._sweep.C;
				b._sweep.A0 = b._sweep.A;

				// Integrate
				b._sweep.C += subStep.Dt * b._linearVelocity;
				b._sweep.A += subStep.Dt * b._angularVelocity;

				// Compute new transform
				b.SynchronizeTransform();

				// Note: shapes are synchronized later.
			}

			// Solve position constraints.
			const float k_toiBaumgarte = 0.75f;
			for (int i = 0; i < subStep.PositionIterations; ++i)
			{
				bool contactsOkay = contactSolver.SolvePositionConstraints(k_toiBaumgarte);
				bool jointsOkay = true;
				for (int j = 0; j < _jointCount; ++j)
				{
					bool jointOkay = _joints[j].SolvePositionConstraints(k_toiBaumgarte);
					jointsOkay = jointsOkay && jointOkay;
				}

				if (contactsOkay && jointsOkay)
				{
					break;
				}
			}

			Report(contactSolver._constraints);
		}

		public void Add(Body body)
		{
			Box2DXDebug.Assert(_bodyCount < _bodyCapacity);
			body._islandIndex = _bodyCount;
			_bodies[_bodyCount++] = body;
		}

		public void Add(Contact contact)
		{
			Box2DXDebug.Assert(_contactCount < _contactCapacity);
			_contacts[_contactCount++] = contact;
		}

		public void Add(Joint joint)
		{
			Box2DXDebug.Assert(_jointCount < _jointCapacity);
			_joints[_jointCount++] = joint;
		}

		public void Report(ContactConstraint[] constraints)
		{
			if (_listener == null)
			{
				return;
			}

			for (int i = 0; i < _contactCount; ++i)
			{
				Contact c = _contacts[i];
				ContactConstraint cc = constraints[i];
				ContactImpulse impulse = new ContactImpulse();
				for (int j = 0; j < cc.PointCount; ++j)
				{
					impulse.normalImpulses[j] = cc.Points[j].NormalImpulse;
					impulse.tangentImpulses[j] = cc.Points[j].TangentImpulse;
				}

				_listener.PostSolve(c, impulse);
			}
		}
	}
}
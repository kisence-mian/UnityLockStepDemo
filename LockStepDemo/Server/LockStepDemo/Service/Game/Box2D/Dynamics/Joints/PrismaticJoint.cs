/*
  Box2DX Copyright (c) 2008 Ihar Kalasouski http://code.google.com/p/box2dx
  Box2D original C++ version Copyright (c) 2006-2007 Erin Catto http://www.gphysics.com

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

// Linear constraint (point-to-line)
// d = p2 - p1 = x2 + r2 - x1 - r1
// C = dot(perp, d)
// Cdot = dot(d, cross(w1, perp)) + dot(perp, v2 + cross(w2, r2) - v1 - cross(w1, r1))
//      = -dot(perp, v1) - dot(cross(d + r1, perp), w1) + dot(perp, v2) + dot(cross(r2, perp), v2)
// J = [-perp, -cross(d + r1, perp), perp, cross(r2,perp)]
//
// Angular constraint
// C = a2 - a1 + a_initial
// Cdot = w2 - w1
// J = [0 0 -1 0 0 1]
//
// K = J * invM * JT
//
// J = [-a -s1 a s2]
//     [0  -1  0  1]
// a = perp
// s1 = cross(d + r1, a) = cross(p2 - x1, a)
// s2 = cross(r2, a) = cross(p2 - x2, a)


// Motor/Limit linear constraint
// C = dot(ax1, d)
// Cdot = = -dot(ax1, v1) - dot(cross(d + r1, ax1), w1) + dot(ax1, v2) + dot(cross(r2, ax1), v2)
// J = [-ax1 -cross(d+r1,ax1) ax1 cross(r2,ax1)]

// Block Solver
// We develop a block solver that includes the joint limit. This makes the limit stiff (inelastic) even
// when the mass has poor distribution (leading to large torques about the joint anchor points).
//
// The Jacobian has 3 rows:
// J = [-uT -s1 uT s2] // linear
//     [0   -1   0  1] // angular
//     [-vT -a1 vT a2] // limit
//
// u = perp
// v = axis
// s1 = cross(d + r1, u), s2 = cross(r2, u)
// a1 = cross(d + r1, v), a2 = cross(r2, v)

// M * (v2 - v1) = JT * df
// J * v2 = bias
//
// v2 = v1 + invM * JT * df
// J * (v1 + invM * JT * df) = bias
// K * df = bias - J * v1 = -Cdot
// K = J * invM * JT
// Cdot = J * v1 - bias
//
// Now solve for f2.
// df = f2 - f1
// K * (f2 - f1) = -Cdot
// f2 = invK * (-Cdot) + f1
//
// Clamp accumulated limit impulse.
// lower: f2(3) = max(f2(3), 0)
// upper: f2(3) = min(f2(3), 0)
//
// Solve for correct f2(1:2)
// K(1:2, 1:2) * f2(1:2) = -Cdot(1:2) - K(1:2,3) * f2(3) + K(1:2,1:3) * f1
//                       = -Cdot(1:2) - K(1:2,3) * f2(3) + K(1:2,1:2) * f1(1:2) + K(1:2,3) * f1(3)
// K(1:2, 1:2) * f2(1:2) = -Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3)) + K(1:2,1:2) * f1(1:2)
// f2(1:2) = invK(1:2,1:2) * (-Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3))) + f1(1:2)
//
// Now compute impulse to be applied:
// df = f2 - f1

using System;
using System.Collections.Generic;
using System.Text;

using Box2DX.Common;

namespace Box2DX.Dynamics
{
	using Box2DXMath = Box2DX.Common.Math;
	using SystemMath = System.Math;

	/// <summary>
	/// Prismatic joint definition. This requires defining a line of
	/// motion using an axis and an anchor point. The definition uses local
	/// anchor points and a local axis so that the initial configuration
	/// can violate the constraint slightly. The joint translation is zero
	/// when the local anchor points coincide in world space. Using local
	/// anchors and a local axis helps when saving and loading a game.
	/// </summary>
	public class PrismaticJointDef : JointDef
	{
		public PrismaticJointDef()
		{
			Type = JointType.PrismaticJoint;
			LocalAnchor1.SetZero();
			LocalAnchor2.SetZero();
			LocalAxis1.Set(1.0f, 0.0f);
			ReferenceAngle = 0.0f;
			EnableLimit = false;
			LowerTranslation = 0.0f;
			UpperTranslation = 0.0f;
			EnableMotor = false;
			MaxMotorForce = 0.0f;
			MotorSpeed = 0.0f;
		}

		/// <summary>
		/// Initialize the bodies, anchors, axis, and reference angle using the world
		/// anchor and world axis.
		/// </summary>
		public void Initialize(Body body1, Body body2, Vec2 anchor, Vec2 axis)
		{
			Body1 = body1;
			Body2 = body2;
			LocalAnchor1 = body1.GetLocalPoint(anchor);
			LocalAnchor2 = body2.GetLocalPoint(anchor);
			LocalAxis1 = body1.GetLocalVector(axis);
			ReferenceAngle = body2.GetAngle() - body1.GetAngle();
		}

		/// <summary>
		/// The local anchor point relative to body1's origin.
		/// </summary>
		public Vec2 LocalAnchor1;

		/// <summary>
		/// The local anchor point relative to body2's origin.
		/// </summary>
		public Vec2 LocalAnchor2;

		/// <summary>
		/// The local translation axis in body1.
		/// </summary>
		public Vec2 LocalAxis1;

		/// <summary>
		/// The constrained angle between the bodies: body2_angle - body1_angle.
		/// </summary>
		public float ReferenceAngle;

		/// <summary>
		/// Enable/disable the joint limit.
		/// </summary>
		public bool EnableLimit;

		/// <summary>
		/// The lower translation limit, usually in meters.
		/// </summary>
		public float LowerTranslation;

		/// <summary>
		/// The upper translation limit, usually in meters.
		/// </summary>
		public float UpperTranslation;

		/// <summary>
		/// Enable/disable the joint motor.
		/// </summary>
		public bool EnableMotor;

		/// <summary>
		/// The maximum motor torque, usually in N-m.
		/// </summary>
		public float MaxMotorForce;

		/// <summary>
		/// The desired motor speed in radians per second.
		/// </summary>
		public float MotorSpeed;
	}

	/// <summary>
	/// A prismatic joint. This joint provides one degree of freedom: translation
	/// along an axis fixed in body1. Relative rotation is prevented. You can
	/// use a joint limit to restrict the range of motion and a joint motor to
	/// drive the motion or to model joint friction.
	/// </summary>
	public class PrismaticJoint : Joint
	{
		public Vec2 _localAnchor1;
		public Vec2 _localAnchor2;
		public Vec2 _localXAxis1;
		public Vec2 _localYAxis1;
		public float _refAngle;

		public Vec2 _axis, _perp;
		public float _s1, _s2;
		public float _a1, _a2;

		public Mat33 _K;
		public Vec3 _impulse;

		public float _motorMass;			// effective mass for motor/limit translational constraint.
		public float _motorImpulse;

		public float _lowerTranslation;
		public float _upperTranslation;
		public float _maxMotorForce;
		public float _motorSpeed;

		public bool _enableLimit;
		public bool _enableMotor;
		public LimitState _limitState;

		public override Vec2 Anchor1
		{
			get { return _body1.GetWorldPoint(_localAnchor1); }
		}

		public override Vec2 Anchor2
		{
			get { return _body2.GetWorldPoint(_localAnchor2); }
		}

		public override Vec2 GetReactionForce(float inv_dt)
		{
			return inv_dt * (_impulse.X * _perp + (_motorImpulse + _impulse.Z) * _axis);
		}

		public override float GetReactionTorque(float inv_dt)
		{
			return inv_dt * _impulse.Y;
		}

		/// <summary>
		/// Get the current joint translation, usually in meters.
		/// </summary>
		public float JointTranslation
		{
			get
			{
				Body b1 = _body1;
				Body b2 = _body2;

				Vec2 p1 = b1.GetWorldPoint(_localAnchor1);
				Vec2 p2 = b2.GetWorldPoint(_localAnchor2);
				Vec2 d = p2 - p1;
				Vec2 axis = b1.GetWorldVector(_localXAxis1);

				float translation = Vec2.Dot(d, axis);
				return translation;
			}
		}

		/// <summary>
		/// Get the current joint translation speed, usually in meters per second.
		/// </summary>
		public float JointSpeed
		{
			get
			{
				Body b1 = _body1;
				Body b2 = _body2;

				Vec2 r1 = Common.Math.Mul(b1.GetXForm().R, _localAnchor1 - b1.GetLocalCenter());
				Vec2 r2 = Common.Math.Mul(b2.GetXForm().R, _localAnchor2 - b2.GetLocalCenter());
				Vec2 p1 = b1._sweep.C + r1;
				Vec2 p2 = b2._sweep.C + r2;
				Vec2 d = p2 - p1;
				Vec2 axis = b1.GetWorldVector(_localXAxis1);

				Vec2 v1 = b1._linearVelocity;
				Vec2 v2 = b2._linearVelocity;
				float w1 = b1._angularVelocity;
				float w2 = b2._angularVelocity;

				float speed = Vec2.Dot(d, Vec2.Cross(w1, axis)) + Vec2.Dot(axis, v2 + Vec2.Cross(w2, r2) - v1 - Vec2.Cross(w1, r1));
				return speed;
			}
		}

		/// <summary>
		/// Is the joint limit enabled?
		/// </summary>
		public bool IsLimitEnabled
		{
			get { return _enableLimit; }
		}

		/// <summary>
		/// Enable/disable the joint limit.
		/// </summary>
		public void EnableLimit(bool flag)
		{
			_body1.WakeUp();
			_body2.WakeUp();
			_enableLimit = flag;
		}

		/// <summary>
		/// Get the lower joint limit, usually in meters.
		/// </summary>
		public float LowerLimit
		{
			get { return _lowerTranslation; }
		}

		/// <summary>
		/// Get the upper joint limit, usually in meters.
		/// </summary>
		public float UpperLimit
		{
			get { return _upperTranslation; }
		}

		/// <summary>
		/// Set the joint limits, usually in meters.
		/// </summary>
		public void SetLimits(float lower, float upper)
		{
			Box2DXDebug.Assert(lower <= upper);
			_body1.WakeUp();
			_body2.WakeUp();
			_lowerTranslation = lower;
			_upperTranslation = upper;
		}

		/// <summary>
		/// Is the joint motor enabled?
		/// </summary>
		public bool IsMotorEnabled
		{
			get { return _enableMotor; }
		}

		/// <summary>
		/// Enable/disable the joint motor.
		/// </summary>
		public void EnableMotor(bool flag)
		{
			_body1.WakeUp();
			_body2.WakeUp();
			_enableMotor = flag;
		}

		/// <summary>
		/// Get\Set the motor speed, usually in meters per second.
		/// </summary>
		public float MotorSpeed
		{
			get { return _motorSpeed; }
			set
			{
				_body1.WakeUp();
				_body2.WakeUp();
				_motorSpeed = value;
			}
		}

		/// <summary>
		/// Set the maximum motor force, usually in N.
		/// </summary>
		public void SetMaxMotorForce(float force)
		{
			_body1.WakeUp();
			_body2.WakeUp();
			_maxMotorForce = Settings.FORCE_SCALE(1.0f) * force;
		}

		/// <summary>
		/// Get the current motor force, usually in N.
		/// </summary>
		public float MotorForce
		{
			get { return _motorImpulse; }
		}

		public PrismaticJoint(PrismaticJointDef def)
			: base(def)
		{
			_localAnchor1 = def.LocalAnchor1;
			_localAnchor2 = def.LocalAnchor2;
			_localXAxis1 = def.LocalAxis1;
			_localYAxis1 = Vec2.Cross(1.0f, _localXAxis1);
			_refAngle = def.ReferenceAngle;

			_impulse.SetZero();
			_motorMass = 0.0f;
			_motorImpulse = 0.0f;

			_lowerTranslation = def.LowerTranslation;
			_upperTranslation = def.UpperTranslation;
			_maxMotorForce = Settings.FORCE_INV_SCALE(def.MaxMotorForce);
			_motorSpeed = def.MotorSpeed;
			_enableLimit = def.EnableLimit;
			_enableMotor = def.EnableMotor;
			_limitState = LimitState.InactiveLimit;

			_axis.SetZero();
			_perp.SetZero();
		}

		internal override void InitVelocityConstraints(TimeStep step)
		{
			Body b1 = _body1;
			Body b2 = _body2;

			// You cannot create a prismatic joint between bodies that
			// both have fixed rotation.
			Box2DXDebug.Assert(b1._invI > 0.0f || b2._invI > 0.0f);

			_localCenter1 = b1.GetLocalCenter();
			_localCenter2 = b2.GetLocalCenter();

			XForm xf1 = b1.GetXForm();
			XForm xf2 = b2.GetXForm();

			// Compute the effective masses.
			Vec2 r1 = Box2DX.Common.Math.Mul(xf1.R, _localAnchor1 - _localCenter1);
			Vec2 r2 = Box2DX.Common.Math.Mul(xf2.R, _localAnchor2 - _localCenter2);
			Vec2 d = b2._sweep.C + r2 - b1._sweep.C - r1;

			_invMass1 = b1._invMass;
			_invI1 = b1._invI;
			_invMass2 = b2._invMass;
			_invI2 = b2._invI;

			// Compute motor Jacobian and effective mass.
			{
				_axis = Box2DX.Common.Math.Mul(xf1.R, _localXAxis1);
				_a1 = Vec2.Cross(d + r1, _axis);
				_a2 = Vec2.Cross(r2, _axis);

				_motorMass = _invMass1 + _invMass2 + _invI1 * _a1 * _a1 + _invI2 * _a2 * _a2;
				Box2DXDebug.Assert(_motorMass > Settings.FLT_EPSILON);
				_motorMass = 1.0f / _motorMass;
			}

			// Prismatic constraint.
			{
				_perp = Box2DX.Common.Math.Mul(xf1.R, _localYAxis1);

				_s1 = Vec2.Cross(d + r1, _perp);
				_s2 = Vec2.Cross(r2, _perp);

				float m1 = _invMass1, m2 = _invMass2;
				float i1 = _invI1, i2 = _invI2;

				float k11 = m1 + m2 + i1 * _s1 * _s1 + i2 * _s2 * _s2;
				float k12 = i1 * _s1 + i2 * _s2;
				float k13 = i1 * _s1 * _a1 + i2 * _s2 * _a2;
				float k22 = i1 + i2;
				float k23 = i1 * _a1 + i2 * _a2;
				float k33 = m1 + m2 + i1 * _a1 * _a1 + i2 * _a2 * _a2;

				_K.Col1.Set(k11, k12, k13);
				_K.Col2.Set(k12, k22, k23);
				_K.Col3.Set(k13, k23, k33);
			}

			// Compute motor and limit terms.
			if (_enableLimit)
			{
				float jointTranslation = Vec2.Dot(_axis, d);
				if (Box2DX.Common.Math.Abs(_upperTranslation - _lowerTranslation) < 2.0f * Settings.LinearSlop)
				{
					_limitState = LimitState.EqualLimits;
				}
				else if (jointTranslation <= _lowerTranslation)
				{
					if (_limitState != LimitState.AtLowerLimit)
					{
						_limitState = LimitState.AtLowerLimit;
						_impulse.Z = 0.0f;
					}
				}
				else if (jointTranslation >= _upperTranslation)
				{
					if (_limitState != LimitState.AtUpperLimit)
					{
						_limitState = LimitState.AtUpperLimit;
						_impulse.Z = 0.0f;
					}
				}
				else
				{
					_limitState = LimitState.InactiveLimit;
					_impulse.Z = 0.0f;
				}
			}
			else
			{
				_limitState = LimitState.InactiveLimit;
			}

			if (_enableMotor == false)
			{
				_motorImpulse = 0.0f;
			}

			if (step.WarmStarting)
			{
				// Account for variable time step.
				_impulse *= step.DtRatio;
				_motorImpulse *= step.DtRatio;

				Vec2 P = _impulse.X * _perp + (_motorImpulse + _impulse.Z) * _axis;
				float L1 = _impulse.X * _s1 + _impulse.Y + (_motorImpulse + _impulse.Z) * _a1;
				float L2 = _impulse.X * _s2 + _impulse.Y + (_motorImpulse + _impulse.Z) * _a2;

				b1._linearVelocity -= _invMass1 * P;
				b1._angularVelocity -= _invI1 * L1;

				b2._linearVelocity += _invMass2 * P;
				b2._angularVelocity += _invI2 * L2;
			}
			else
			{
				_impulse.SetZero();
				_motorImpulse = 0.0f;
			}
		}

		internal override void SolveVelocityConstraints(TimeStep step)
		{
			Body b1 = _body1;
			Body b2  = _body2;

			Vec2 v1 = b1._linearVelocity;
			float w1 = b1._angularVelocity;
			Vec2 v2 = b2._linearVelocity;
			float w2 = b2._angularVelocity;

			// Solve linear motor constraint.
			if (_enableMotor && _limitState != LimitState.EqualLimits)
			{
				float Cdot = Vec2.Dot(_axis, v2 - v1) + _a2 * w2 - _a1 * w1;
				float impulse = _motorMass * (_motorSpeed - Cdot);
				float oldImpulse = _motorImpulse;
				float maxImpulse = step.Dt * _maxMotorForce;
				_motorImpulse = Box2DX.Common.Math.Clamp(_motorImpulse + impulse, -maxImpulse, maxImpulse);
				impulse = _motorImpulse - oldImpulse;

				Vec2 P = impulse * _axis;
				float L1 = impulse * _a1;
				float L2 = impulse * _a2;

				v1 -= _invMass1 * P;
				w1 -= _invI1 * L1;

				v2 += _invMass2 * P;
				w2 += _invI2 * L2;
			}

			Vec2 Cdot1;
			Cdot1.X = Vec2.Dot(_perp, v2 - v1) + _s2 * w2 - _s1 * w1;
			Cdot1.Y = w2 - w1;

			if (_enableLimit && _limitState != LimitState.InactiveLimit)
			{
				// Solve prismatic and limit constraint in block form.
				float Cdot2;
				Cdot2 = Vec2.Dot(_axis, v2 - v1) + _a2 * w2 - _a1 * w1;
				Vec3 Cdot = new Vec3(Cdot1.X, Cdot1.Y, Cdot2);

				Vec3 f1 = _impulse;
				Vec3 df =  _K.Solve33(-Cdot);
				_impulse += df;

				if (_limitState ==LimitState.AtLowerLimit)
				{
					_impulse.Z = Box2DX.Common.Math.Max(_impulse.Z, 0.0f);
				}
				else if (_limitState == LimitState.AtUpperLimit)
				{
					_impulse.Z = Box2DX.Common.Math.Min(_impulse.Z, 0.0f);
				}

				// f2(1:2) = invK(1:2,1:2) * (-Cdot(1:2) - K(1:2,3) * (f2(3) - f1(3))) + f1(1:2)
				Vec2 b = -Cdot1 - (_impulse.Z - f1.Z) * new Vec2(_K.Col3.X, _K.Col3.Y);
				Vec2 f2r = _K.Solve22(b) + new Vec2(f1.X, f1.Y);
				_impulse.X = f2r.X;
				_impulse.Y = f2r.Y;

				df = _impulse - f1;

				Vec2 P = df.X * _perp + df.Z * _axis;
				float L1 = df.X * _s1 + df.Y + df.Z * _a1;
				float L2 = df.X * _s2 + df.Y + df.Z * _a2;

				v1 -= _invMass1 * P;
				w1 -= _invI1 * L1;

				v2 += _invMass2 * P;
				w2 += _invI2 * L2;
			}
			else
			{
				// Limit is inactive, just solve the prismatic constraint in block form.
				Vec2 df = _K.Solve22(-Cdot1);
				_impulse.X += df.X;
				_impulse.Y += df.Y;

				Vec2 P = df.X * _perp;
				float L1 = df.X * _s1 + df.Y;
				float L2 = df.X * _s2 + df.Y;

				v1 -= _invMass1 * P;
				w1 -= _invI1 * L1;

				v2 += _invMass2 * P;
				w2 += _invI2 * L2;
			}

			b1._linearVelocity = v1;
			b1._angularVelocity = w1;
			b2._linearVelocity = v2;
			b2._angularVelocity = w2;
		}

		internal override bool SolvePositionConstraints(float baumgarte)
		{
			Body b1 = _body1;
			Body b2 = _body2;

			Vec2 c1 = b1._sweep.C;
			float a1 = b1._sweep.A;

			Vec2 c2 = b2._sweep.C;
			float a2 = b2._sweep.A;

			// Solve linear limit constraint.
			float linearError = 0.0f, angularError = 0.0f;
			bool active = false;
			float C2 = 0.0f;

			Mat22 R1 = new Mat22(a1), R2 = new Mat22(a2);

			Vec2 r1 = Box2DX.Common.Math.Mul(R1, _localAnchor1 - _localCenter1);
			Vec2 r2 = Box2DX.Common.Math.Mul(R2, _localAnchor2 - _localCenter2);
			Vec2 d = c2 + r2 - c1 - r1;

			if (_enableLimit)
			{
				_axis = Box2DX.Common.Math.Mul(R1, _localXAxis1);

				_a1 = Vec2.Cross(d + r1, _axis);
				_a2 = Vec2.Cross(r2, _axis);

				float translation = Vec2.Dot(_axis, d);
				if (Box2DX.Common.Math.Abs(_upperTranslation - _lowerTranslation) < 2.0f * Settings.LinearSlop)
				{
					// Prevent large angular corrections
					C2 = Box2DX.Common.Math.Clamp(translation, -Settings.MaxLinearCorrection, Settings.MaxLinearCorrection);
					linearError = Box2DX.Common.Math.Abs(translation);
					active = true;
				}
				else if (translation <= _lowerTranslation)
				{
					// Prevent large linear corrections and allow some slop.
					C2 = Box2DX.Common.Math.Clamp(translation - _lowerTranslation + Settings.LinearSlop, -Settings.MaxLinearCorrection, 0.0f);
					linearError = _lowerTranslation - translation;
					active = true;
				}
				else if (translation >= _upperTranslation)
				{
					// Prevent large linear corrections and allow some slop.
					C2 = Box2DX.Common.Math.Clamp(translation - _upperTranslation - Settings.LinearSlop, 0.0f, Settings.MaxLinearCorrection);
					linearError = translation - _upperTranslation;
					active = true;
				}
			}

			_perp = Box2DX.Common.Math.Mul(R1, _localYAxis1);

			_s1 = Vec2.Cross(d + r1, _perp);
			_s2 = Vec2.Cross(r2, _perp);

			Vec3 impulse;
			Vec2 C1 = new Vec2();
			C1.X = Vec2.Dot(_perp, d);
			C1.Y = a2 - a1 - _refAngle;

			linearError = Box2DX.Common.Math.Max(linearError, Box2DX.Common.Math.Abs(C1.X));
			angularError = Box2DX.Common.Math.Abs(C1.Y);

			if (active)
			{
				float m1 = _invMass1, m2 = _invMass2;
				float i1 = _invI1, i2 = _invI2;

				float k11 = m1 + m2 + i1 * _s1 * _s1 + i2 * _s2 * _s2;
				float k12 = i1 * _s1 + i2 * _s2;
				float k13 = i1 * _s1 * _a1 + i2 * _s2 * _a2;
				float k22 = i1 + i2;
				float k23 = i1 * _a1 + i2 * _a2;
				float k33 = m1 + m2 + i1 * _a1 * _a1 + i2 * _a2 * _a2;

				_K.Col1.Set(k11, k12, k13);
				_K.Col2.Set(k12, k22, k23);
				_K.Col3.Set(k13, k23, k33);

				Vec3 C = new Vec3();
				C.X = C1.X;
				C.Y = C1.Y;
				C.Z = C2;

				impulse = _K.Solve33(-C);
			}
			else
			{
				float m1 = _invMass1, m2 = _invMass2;
				float i1 = _invI1, i2 = _invI2;

				float k11 = m1 + m2 + i1 * _s1 * _s1 + i2 * _s2 * _s2;
				float k12 = i1 * _s1 + i2 * _s2;
				float k22 = i1 + i2;

				_K.Col1.Set(k11, k12, 0.0f);
				_K.Col2.Set(k12, k22, 0.0f);

				Vec2 impulse1 = _K.Solve22(-C1);
				impulse.X = impulse1.X;
				impulse.Y = impulse1.Y;
				impulse.Z = 0.0f;
			}

			Vec2 P = impulse.X * _perp + impulse.Z * _axis;
			float L1 = impulse.X * _s1 + impulse.Y + impulse.Z * _a1;
			float L2 = impulse.X * _s2 + impulse.Y + impulse.Z * _a2;

			c1 -= _invMass1 * P;
			a1 -= _invI1 * L1;
			c2 += _invMass2 * P;
			a2 += _invI2 * L2;

			// TODO_ERIN remove need for this.
			b1._sweep.C = c1;
			b1._sweep.A = a1;
			b2._sweep.C = c2;
			b2._sweep.A = a2;
			b1.SynchronizeTransform();
			b2.SynchronizeTransform();
			
			return linearError <= Settings.LinearSlop && angularError <= Settings.AngularSlop;
		}
	}
}

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

// Gear Joint:
// C0 = (coordinate1 + ratio * coordinate2)_initial
// C = C0 - (cordinate1 + ratio * coordinate2) = 0
// Cdot = -(Cdot1 + ratio * Cdot2)
// J = -[J1 ratio * J2]
// K = J * invM * JT
//   = J1 * invM1 * J1T + ratio * ratio * J2 * invM2 * J2T
//
// Revolute:
// coordinate = rotation
// Cdot = angularVelocity
// J = [0 0 1]
// K = J * invM * JT = invI
//
// Prismatic:
// coordinate = dot(p - pg, ug)
// Cdot = dot(v + cross(w, r), ug)
// J = [ug cross(r, ug)]
// K = J * invM * JT = invMass + invI * cross(r, ug)^2

using System;
using System.Collections.Generic;
using System.Text;

using Box2DX.Common;

namespace Box2DX.Dynamics
{
	/// <summary>
	/// Gear joint definition. This definition requires two existing
	/// revolute or prismatic joints (any combination will work).
	/// The provided joints must attach a dynamic body to a static body.
	/// </summary>
	public class GearJointDef : JointDef
	{
		public GearJointDef()
		{
			Type = JointType.GearJoint;
			Joint1 = null;
			Joint2 = null;
			Ratio = 1.0f;
		}

		/// <summary>
		/// The first revolute/prismatic joint attached to the gear joint.
		/// </summary>
		public Joint Joint1;

		/// <summary>
		/// The second revolute/prismatic joint attached to the gear joint.
		/// </summary>
		public Joint Joint2;

		/// <summary>
		/// The gear ratio.
		/// @see GearJoint for explanation.
		/// </summary>
		public float Ratio;
	}

	/// <summary>
	/// A gear joint is used to connect two joints together. Either joint
	/// can be a revolute or prismatic joint. You specify a gear ratio
	/// to bind the motions together:
	/// coordinate1 + ratio * coordinate2 = constant
	/// The ratio can be negative or positive. If one joint is a revolute joint
	/// and the other joint is a prismatic joint, then the ratio will have units
	/// of length or units of 1/length.
	/// @warning The revolute and prismatic joints must be attached to
	/// fixed bodies (which must be body1 on those joints).
	/// </summary>
	public class GearJoint : Joint
	{
		public Body _ground1;
		public Body _ground2;

		// One of these is NULL.
		public RevoluteJoint _revolute1;
		public PrismaticJoint _prismatic1;

		// One of these is NULL.
		public RevoluteJoint _revolute2;
		public PrismaticJoint _prismatic2;

		public Vec2 _groundAnchor1;
		public Vec2 _groundAnchor2;

		public Vec2 _localAnchor1;
		public Vec2 _localAnchor2;

		public Jacobian _J;

		public float _constant;
		public float _ratio;

		// Effective mass
		public float _mass;

		// Impulse for accumulation/warm starting.
		public float _impulse;

		public override Vec2 Anchor1 { get { return _body1.GetWorldPoint(_localAnchor1); } }
		public override Vec2 Anchor2 { get { return _body2.GetWorldPoint(_localAnchor2); } }

		public override Vec2 GetReactionForce(float inv_dt)
		{
			// TODO_ERIN not tested
			Vec2 P = _impulse * _J.Linear2;
			return inv_dt * P;
		}

		public override float GetReactionTorque(float inv_dt)
		{
			// TODO_ERIN not tested
			Vec2 r = Common.Math.Mul(_body2.GetXForm().R, _localAnchor2 - _body2.GetLocalCenter());
			Vec2 P = _impulse * _J.Linear2;
			float L = _impulse * _J.Angular2 - Vec2.Cross(r, P);
			return inv_dt * L;
		}

		/// <summary>
		/// Get the gear ratio.
		/// </summary>
		public float Ratio { get { return _ratio; } }

		public GearJoint(GearJointDef def)
			: base(def)
		{
			JointType type1 = def.Joint1.GetType();
			JointType type2 = def.Joint2.GetType();

			Box2DXDebug.Assert(type1 == JointType.RevoluteJoint || type1 == JointType.PrismaticJoint);
			Box2DXDebug.Assert(type2 == JointType.RevoluteJoint || type2 == JointType.PrismaticJoint);
			Box2DXDebug.Assert(def.Joint1.GetBody1().IsStatic());
			Box2DXDebug.Assert(def.Joint2.GetBody1().IsStatic());

			_revolute1 = null;
			_prismatic1 = null;
			_revolute2 = null;
			_prismatic2 = null;

			float coordinate1, coordinate2;

			_ground1 = def.Joint1.GetBody1();
			_body1 = def.Joint1.GetBody2();
			if (type1 == JointType.RevoluteJoint)
			{
				_revolute1 = (RevoluteJoint)def.Joint1;
				_groundAnchor1 = _revolute1._localAnchor1;
				_localAnchor1 = _revolute1._localAnchor2;
				coordinate1 = _revolute1.JointAngle;
			}
			else
			{
				_prismatic1 = (PrismaticJoint)def.Joint1;
				_groundAnchor1 = _prismatic1._localAnchor1;
				_localAnchor1 = _prismatic1._localAnchor2;
				coordinate1 = _prismatic1.JointTranslation;
			}

			_ground2 = def.Joint2.GetBody1();
			_body2 = def.Joint2.GetBody2();
			if (type2 == JointType.RevoluteJoint)
			{
				_revolute2 = (RevoluteJoint)def.Joint2;
				_groundAnchor2 = _revolute2._localAnchor1;
				_localAnchor2 = _revolute2._localAnchor2;
				coordinate2 = _revolute2.JointAngle;
			}
			else
			{
				_prismatic2 = (PrismaticJoint)def.Joint2;
				_groundAnchor2 = _prismatic2._localAnchor1;
				_localAnchor2 = _prismatic2._localAnchor2;
				coordinate2 = _prismatic2.JointTranslation;
			}

			_ratio = def.Ratio;

			_constant = coordinate1 + _ratio * coordinate2;

			_impulse = 0.0f;
		}

		internal override void InitVelocityConstraints(TimeStep step)
		{
			Body g1 = _ground1;
			Body g2 = _ground2;
			Body b1 = _body1;
			Body b2 = _body2;

			float K = 0.0f;
			_J.SetZero();

			if (_revolute1!=null)
			{
				_J.Angular1 = -1.0f;
				K += b1._invI;
			}
			else
			{
				Vec2 ug = Common.Math.Mul(g1.GetXForm().R, _prismatic1._localXAxis1);
				Vec2 r = Common.Math.Mul(b1.GetXForm().R, _localAnchor1 - b1.GetLocalCenter());
				float crug = Vec2.Cross(r, ug);
				_J.Linear1 = -ug;
				_J.Angular1 = -crug;
				K += b1._invMass + b1._invI * crug * crug;
			}

			if (_revolute2!=null)
			{
				_J.Angular2 = -_ratio;
				K += _ratio * _ratio * b2._invI;
			}
			else
			{
				Vec2 ug = Common.Math.Mul(g2.GetXForm().R, _prismatic2._localXAxis1);
				Vec2 r = Common.Math.Mul(b2.GetXForm().R, _localAnchor2 - b2.GetLocalCenter());
				float crug = Vec2.Cross(r, ug);
				_J.Linear2 = -_ratio * ug;
				_J.Angular2 = -_ratio * crug;
				K += _ratio * _ratio * (b2._invMass + b2._invI * crug * crug);
			}

			// Compute effective mass.
			Box2DXDebug.Assert(K > 0.0f);
			_mass = 1.0f / K;

			if (step.WarmStarting)
			{
				// Warm starting.
				b1._linearVelocity += b1._invMass * _impulse * _J.Linear1;
				b1._angularVelocity += b1._invI * _impulse * _J.Angular1;
				b2._linearVelocity += b2._invMass * _impulse * _J.Linear2;
				b2._angularVelocity += b2._invI * _impulse * _J.Angular2;
			}
			else
			{
				_impulse = 0.0f;
			}
		}

		internal override void SolveVelocityConstraints(TimeStep step)
		{
			Body b1 = _body1;
			Body b2 = _body2;

			float Cdot = _J.Compute(b1._linearVelocity, b1._angularVelocity, b2._linearVelocity, b2._angularVelocity);

			float impulse = _mass * (-Cdot);
			_impulse += impulse;

			b1._linearVelocity += b1._invMass * impulse * _J.Linear1;
			b1._angularVelocity += b1._invI * impulse * _J.Angular1;
			b2._linearVelocity += b2._invMass * impulse * _J.Linear2;
			b2._angularVelocity += b2._invI * impulse * _J.Angular2;
		}

		internal override bool SolvePositionConstraints(float baumgarte)
		{
			float linearError = 0.0f;

			Body b1 = _body1;
			Body b2 = _body2;

			float coordinate1, coordinate2;
			if (_revolute1 != null)
			{
				coordinate1 = _revolute1.JointAngle;
			}
			else
			{
				coordinate1 = _prismatic1.JointTranslation;
			}

			if (_revolute2 != null)
			{
				coordinate2 = _revolute2.JointAngle;
			}
			else
			{
				coordinate2 = _prismatic2.JointTranslation;
			}

			float C = _constant - (coordinate1 + _ratio * coordinate2);

			float impulse = _mass * (-C);

			b1._sweep.C += b1._invMass * impulse * _J.Linear1;
			b1._sweep.A += b1._invI * impulse * _J.Angular1;
			b2._sweep.C += b2._invMass * impulse * _J.Linear2;
			b2._sweep.A += b2._invI * impulse * _J.Angular2;

			b1.SynchronizeTransform();
			b2.SynchronizeTransform();

			//TODO_ERIN not implemented
			return linearError < Settings.LinearSlop;
		}
	}
}

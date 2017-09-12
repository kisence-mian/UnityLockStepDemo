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

using System;
using System.Collections.Generic;
using System.Text;

using Box2DX.Common;

namespace Box2DX.Dynamics
{
	public enum JointType
	{
		UnknownJoint,
		RevoluteJoint,
		PrismaticJoint,
		DistanceJoint,
		PulleyJoint,
		MouseJoint,
		GearJoint,
		LineJoint
	}

	public enum LimitState
	{
		InactiveLimit,
		AtLowerLimit,
		AtUpperLimit,
		EqualLimits
	}

	public struct Jacobian
	{
		public Vec2 Linear1;
		public float Angular1;
		public Vec2 Linear2;
		public float Angular2;

		public void SetZero()
		{
			Linear1.SetZero(); Angular1 = 0.0f;
			Linear2.SetZero(); Angular2 = 0.0f;
		}

		public void Set(Vec2 x1, float a1, Vec2 x2, float a2)
		{
			Linear1 = x1; Angular1 = a1;
			Linear2 = x2; Angular2 = a2;
		}

		public float Compute(Vec2 x1, float a1, Vec2 x2, float a2)
		{
			return Vec2.Dot(Linear1, x1) + Angular1 * a1 + Vec2.Dot(Linear2, x2) + Angular2 * a2;
		}
	}

#warning "CAS"
	/// <summary>
	/// A joint edge is used to connect bodies and joints together
	/// in a joint graph where each body is a node and each joint
	/// is an edge. A joint edge belongs to a doubly linked list
	/// maintained in each attached body. Each joint has two joint
	/// nodes, one for each attached body.
	/// </summary>
	public class JointEdge
	{
		/// <summary>
		/// Provides quick access to the other body attached.
		/// </summary>
		public Body Other;

		/// <summary>
		/// The joint.
		/// </summary>
		public Joint Joint;

		/// <summary>
		/// The previous joint edge in the body's joint list.
		/// </summary>
		public JointEdge Prev;

		/// <summary>
		/// The next joint edge in the body's joint list.
		/// </summary>
		public JointEdge Next;
	}

#warning "CAS"
	/// <summary>
	/// Joint definitions are used to construct joints.
	/// </summary>
	public class JointDef
	{
		public JointDef()
		{
			Type = JointType.UnknownJoint;
			UserData = null;
			Body1 = null;
			Body2 = null;
			CollideConnected = false;
		}

		/// <summary>
		/// The joint type is set automatically for concrete joint types.
		/// </summary>
		public JointType Type;

		/// <summary>
		/// Use this to attach application specific data to your joints.
		/// </summary>
		public object UserData;

		/// <summary>
		/// The first attached body.
		/// </summary>
		public Body Body1;

		/// <summary>
		/// The second attached body.
		/// </summary>
		public Body Body2;

		/// <summary>
		/// Set this flag to true if the attached bodies should collide.
		/// </summary>
		public bool CollideConnected;
	}

	/// <summary>
	/// The base joint class. Joints are used to constraint two bodies together in
	/// various fashions. Some joints also feature limits and motors.
	/// </summary>
	public abstract class Joint
	{
		protected JointType _type;
		internal Joint _prev;
		internal Joint _next;
		internal JointEdge _node1 = new JointEdge();
		internal JointEdge _node2 = new JointEdge();
		internal Body _body1;
		internal Body _body2;

		internal bool _islandFlag;
		internal bool _collideConnected;

		protected object _userData;

		// Cache here per time step to reduce cache misses.
		protected Vec2 _localCenter1, _localCenter2;
		protected float _invMass1, _invI1;
		protected float _invMass2, _invI2;

		/// <summary>
		/// Get the type of the concrete joint.
		/// </summary>
		public new JointType GetType()
		{
			return _type;
		}

		/// <summary>
		/// Get the first body attached to this joint.
		/// </summary>
		/// <returns></returns>
		public Body GetBody1()
		{
			return _body1;
		}

		/// <summary>
		/// Get the second body attached to this joint.
		/// </summary>
		/// <returns></returns>
		public Body GetBody2()
		{
			return _body2;
		}

		/// <summary>
		/// Get the anchor point on body1 in world coordinates.
		/// </summary>
		/// <returns></returns>
		public abstract Vec2 Anchor1 { get; }

		/// <summary>
		/// Get the anchor point on body2 in world coordinates.
		/// </summary>
		/// <returns></returns>
		public abstract Vec2 Anchor2 { get; }

		/// <summary>
		/// Get the reaction force on body2 at the joint anchor.
		/// </summary>		
		public abstract Vec2 GetReactionForce(float inv_dt);

		/// <summary>
		/// Get the reaction torque on body2.
		/// </summary>		
		public abstract float GetReactionTorque(float inv_dt);

		/// <summary>
		/// Get the next joint the world joint list.
		/// </summary>
		/// <returns></returns>
		public Joint GetNext()
		{
			return _next;
		}

		/// <summary>
		/// Get/Set the user data pointer.
		/// </summary>
		/// <returns></returns>
		public object UserData
		{
			get { return _userData; }
			set { _userData = value; }
		}

		protected Joint(JointDef def)
		{
			_type = def.Type;
			_prev = null;
			_next = null;
			_body1 = def.Body1;
			_body2 = def.Body2;
			_collideConnected = def.CollideConnected;
			_islandFlag = false;
			_userData = def.UserData;
		}

		internal static Joint Create(JointDef def)
		{
			Joint joint = null;

			switch (def.Type)
			{
				case JointType.DistanceJoint:
					{
						joint = new DistanceJoint((DistanceJointDef)def);
					}
					break;
				case JointType.MouseJoint:
					{
						joint = new MouseJoint((MouseJointDef)def);
					}
					break;
				case JointType.PrismaticJoint:
					{
						joint = new PrismaticJoint((PrismaticJointDef)def);
					}
					break;
				case JointType.RevoluteJoint:
					{
						joint = new RevoluteJoint((RevoluteJointDef)def);
					}
					break;
				case JointType.PulleyJoint:
					{
						joint = new PulleyJoint((PulleyJointDef)def);
					}
					break;
				case JointType.GearJoint:
					{
						joint = new GearJoint((GearJointDef)def);
					}
					break;
				case JointType.LineJoint:
					{
						joint = new LineJoint((LineJointDef)def);
					}
					break;
				default:
					Box2DXDebug.Assert(false);
					break;
			}

			return joint;
		}

		internal static void Destroy(Joint joint)
		{
			joint = null;
		}

		internal abstract void InitVelocityConstraints(TimeStep step);
		internal abstract void SolveVelocityConstraints(TimeStep step);

		// This returns true if the position errors are within tolerance.
		internal abstract bool SolvePositionConstraints(float baumgarte);

		internal void ComputeXForm(ref XForm xf, Vec2 center, Vec2 localCenter, float angle)
		{
			xf.R.Set(angle);
			xf.Position = center - Box2DX.Common.Math.Mul(xf.R, localCenter);
		}
	}
}

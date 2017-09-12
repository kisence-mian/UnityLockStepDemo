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

using System;
using Box2DX.Collision;
using Box2DX.Common;

namespace Box2DX.Dynamics
{
	public delegate Contact ContactCreateFcn(Fixture fixtureA, Fixture fixtureB);
	public delegate void ContactDestroyFcn(ref Contact contact);

	public struct ContactRegister
	{
		public ContactCreateFcn CreateFcn;
		public ContactDestroyFcn DestroyFcn;
		public bool Primary;
	}

	/// <summary>
	/// A contact edge is used to connect bodies and contacts together
	/// in a contact graph where each body is a node and each contact
	/// is an edge. A contact edge belongs to a doubly linked list
	/// maintained in each attached body. Each contact has two contact
	/// nodes, one for each attached body.
	/// </summary>
	public class ContactEdge
	{
		/// <summary>
		/// Provides quick access to the other body attached.
		/// </summary>
		public Body Other;
		/// <summary>
		/// The contact.
		/// </summary>
		public Contact Contact;
		/// <summary>
		/// The previous contact edge in the body's contact list.
		/// </summary>
		public ContactEdge Prev;
		/// <summary>
		/// The next contact edge in the body's contact list.
		/// </summary>
		public ContactEdge Next;
	}

	/// <summary>
	/// The class manages contact between two shapes. A contact exists for each overlapping
	/// AABB in the broad-phase (except if filtered). Therefore a contact object may exist
	/// that has no contact points.
	/// </summary>
	public abstract class Contact
	{
		[Flags]
		public enum CollisionFlags
		{
			NonSolid = 0x0001,
			Slow = 0x0002,
			Island = 0x0004,
			Toi = 0x0008,
			Touch = 0x0010
		}

		public static ContactRegister[][] s_registers =
			new ContactRegister[(int)ShapeType.ShapeTypeCount][/*(int)ShapeType.ShapeTypeCount*/];
		public static bool s_initialized;

		public CollisionFlags _flags;

		// World pool and list pointers.
		public Contact _prev;
		public Contact _next;

		// Nodes for connecting bodies.
		public ContactEdge _nodeA;
		public ContactEdge _nodeB;

		public Fixture _fixtureA;
		public Fixture _fixtureB;
		
		public Manifold _manifold = new Manifold();

		public float _toi;

		internal delegate void CollideShapeDelegate(
			ref Manifold manifold, Shape circle1, XForm xf1, Shape circle2, XForm xf2);
		internal CollideShapeDelegate CollideShapeFunction;

		public Contact(){}

		public Contact(Fixture fA, Fixture fB)
		{
			_flags = 0;

			if (fA.IsSensor || fB.IsSensor)
			{
				_flags |= CollisionFlags.NonSolid;
			}

			_fixtureA = fA;
			_fixtureB = fB;

			_manifold.PointCount = 0;

			_prev = null;
			_next = null;

			_nodeA = new ContactEdge();
			_nodeB = new ContactEdge();
		}

		public static void AddType(ContactCreateFcn createFcn, ContactDestroyFcn destoryFcn,
					  ShapeType type1, ShapeType type2)
		{
			Box2DXDebug.Assert(ShapeType.UnknownShape < type1 && type1 < ShapeType.ShapeTypeCount);
			Box2DXDebug.Assert(ShapeType.UnknownShape < type2 && type2 < ShapeType.ShapeTypeCount);

			if (s_registers[(int)type1] == null)
				s_registers[(int)type1] = new ContactRegister[(int)ShapeType.ShapeTypeCount];

			s_registers[(int)type1][(int)type2].CreateFcn = createFcn;
			s_registers[(int)type1][(int)type2].DestroyFcn = destoryFcn;
			s_registers[(int)type1][(int)type2].Primary = true;

			if (type1 != type2)
			{
				s_registers[(int)type2][(int)type1].CreateFcn = createFcn;
				s_registers[(int)type2][(int)type1].DestroyFcn = destoryFcn;
				s_registers[(int)type2][(int)type1].Primary = false;
			}
		}

		public static void InitializeRegisters()
		{
			AddType(CircleContact.Create, CircleContact.Destroy, ShapeType.CircleShape, ShapeType.CircleShape);
			AddType(PolyAndCircleContact.Create, PolyAndCircleContact.Destroy, ShapeType.PolygonShape, ShapeType.CircleShape);
			AddType(PolygonContact.Create, PolygonContact.Destroy, ShapeType.PolygonShape, ShapeType.PolygonShape);

			AddType(EdgeAndCircleContact.Create, EdgeAndCircleContact.Destroy, ShapeType.EdgeShape, ShapeType.CircleShape);
			AddType(PolyAndEdgeContact.Create, PolyAndEdgeContact.Destroy, ShapeType.PolygonShape, ShapeType.EdgeShape);
		}

		public static Contact Create(Fixture fixtureA, Fixture fixtureB)
		{
			if (s_initialized == false)
			{
				InitializeRegisters();
				s_initialized = true;
			}

			ShapeType type1 = fixtureA.ShapeType;
			ShapeType type2 = fixtureB.ShapeType;

			Box2DXDebug.Assert(ShapeType.UnknownShape < type1 && type1 < ShapeType.ShapeTypeCount);
			Box2DXDebug.Assert(ShapeType.UnknownShape < type2 && type2 < ShapeType.ShapeTypeCount);

			ContactCreateFcn createFcn = s_registers[(int)type1][(int)type2].CreateFcn;
			if (createFcn != null)
			{
				if (s_registers[(int)type1][(int)type2].Primary)
				{
					return createFcn(fixtureA, fixtureB);
				}
				else
				{
					return createFcn(fixtureB, fixtureA);
				}
			}
			else
			{
				return null;
			}
		}

		public static void Destroy(ref Contact contact)
		{
			Box2DXDebug.Assert(s_initialized == true);

			if (contact._manifold.PointCount > 0)
			{
				contact.FixtureA.Body.WakeUp();
				contact.FixtureB.Body.WakeUp();
			}

			ShapeType typeA = contact.FixtureA.ShapeType;
			ShapeType typeB = contact.FixtureB.ShapeType;

			Box2DXDebug.Assert(ShapeType.UnknownShape < typeA && typeA < ShapeType.ShapeTypeCount);
			Box2DXDebug.Assert(ShapeType.UnknownShape < typeB && typeB < ShapeType.ShapeTypeCount);

			ContactDestroyFcn destroyFcn = s_registers[(int)typeA][(int)typeB].DestroyFcn;
			destroyFcn(ref contact);
		}

		public void Update(ContactListener listener)
		{
			Manifold oldManifold = _manifold.Clone();

			Evaluate();

			Body bodyA = _fixtureA.Body;
			Body bodyB = _fixtureB.Body;

			int oldCount = oldManifold.PointCount;
			int newCount = _manifold.PointCount;

			if (newCount == 0 && oldCount > 0)
			{
				bodyA.WakeUp();
				bodyB.WakeUp();
			}

			// Slow contacts don't generate TOI events.
			if (bodyA.IsStatic() || bodyA.IsBullet() || bodyB.IsStatic() || bodyB.IsBullet())
			{
				_flags &= ~CollisionFlags.Slow;
			}
			else
			{
				_flags |= CollisionFlags.Slow;
			}

			// Match old contact ids to new contact ids and copy the
			// stored impulses to warm start the solver.
			for (int i = 0; i < _manifold.PointCount; ++i)
			{
				ManifoldPoint mp2 = _manifold.Points[i];
				mp2.NormalImpulse = 0.0f;
				mp2.TangentImpulse = 0.0f;
				ContactID id2 = mp2.ID;

				for (int j = 0; j < oldManifold.PointCount; ++j)
				{
					ManifoldPoint mp1 = oldManifold.Points[j];

					if (mp1.ID.Key == id2.Key)
					{
						mp2.NormalImpulse = mp1.NormalImpulse;
						mp2.TangentImpulse = mp1.TangentImpulse;
						break;
					}
				}
			}

			if (oldCount == 0 && newCount > 0)
			{
				_flags |= CollisionFlags.Touch;
				if(listener!=null)
					listener.BeginContact(this);
			}

			if (oldCount > 0 && newCount == 0)
			{
				_flags &= ~CollisionFlags.Touch;
				if (listener != null)
				listener.EndContact(this);
			}

			if ((_flags & CollisionFlags.NonSolid) == 0)
			{
				if (listener != null)
					listener.PreSolve(this, oldManifold);

				// The user may have disabled contact.
				if (_manifold.PointCount == 0)
				{
					_flags &= ~CollisionFlags.Touch;
				}
			}
		}

		public void Evaluate()
		{
			Body bodyA = _fixtureA.Body;
			Body bodyB = _fixtureB.Body;

			Box2DXDebug.Assert(CollideShapeFunction!=null);

			CollideShapeFunction(ref _manifold, _fixtureA.Shape, bodyA.GetXForm(), _fixtureB.Shape, bodyB.GetXForm());
		}

		public float ComputeTOI(Sweep sweepA, Sweep sweepB)
		{
			TOIInput input = new TOIInput();
			input.SweepA = sweepA;
			input.SweepB = sweepB;
			input.SweepRadiusA = _fixtureA.ComputeSweepRadius(sweepA.LocalCenter);
			input.SweepRadiusB = _fixtureB.ComputeSweepRadius(sweepB.LocalCenter);
			input.Tolerance = Common.Settings.LinearSlop;

			return Collision.Collision.TimeOfImpact(input, _fixtureA.Shape, _fixtureB.Shape);
		}

		/// <summary>
		/// Get the contact manifold.
		/// </summary>
		public Manifold Manifold
		{
			get { return _manifold; }
		}

		/// <summary>
		/// Get the world manifold.
		/// </summary>		
		public void GetWorldManifold(out WorldManifold worldManifold)
		{
			worldManifold = new WorldManifold();

			Body bodyA = _fixtureA.Body;
			Body bodyB = _fixtureB.Body;
			Shape shapeA = _fixtureA.Shape;
			Shape shapeB = _fixtureB.Shape;

			worldManifold.Initialize(_manifold, bodyA.GetXForm(), shapeA._radius, bodyB.GetXForm(), shapeB._radius);
		}

		/// <summary>
		/// Is this contact solid?
		/// </summary>
		/// <returns>True if this contact should generate a response.</returns>
		public bool IsSolid
		{
			get { return (_flags & CollisionFlags.NonSolid) == 0; }
		}

		/// <summary>
		/// Are fixtures touching?
		/// </summary>
		public bool AreTouching
		{
			get { return (_flags & CollisionFlags.Touch) == CollisionFlags.Touch; }
		}

		/// <summary>
		/// Get the next contact in the world's contact list.
		/// </summary>
		public Contact GetNext()
		{
			return _next;
		}

		/// <summary>
		/// Get the first fixture in this contact.
		/// </summary>
		public Fixture FixtureA
		{
			get { return _fixtureA; }
		}

		/// <summary>
		/// Get the second fixture in this contact.
		/// </summary>
		public Fixture FixtureB
		{
			get { return _fixtureB; }
		}		
	}
}

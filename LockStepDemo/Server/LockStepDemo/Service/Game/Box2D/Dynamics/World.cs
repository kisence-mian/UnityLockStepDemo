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

using Box2DX.Common;
using Box2DX.Collision;

namespace Box2DX.Dynamics
{
	public struct TimeStep
	{
		public float Dt; // time step
		public float Inv_Dt; // inverse time step (0 if dt == 0).
		public float DtRatio;	// dt * inv_dt0
		public int VelocityIterations;
		public int PositionIterations;
		public bool WarmStarting;
	}

	/// <summary>
	/// The world class manages all physics entities, dynamic simulation,
	/// and asynchronous queries.
	/// </summary>
	public class World : IDisposable
	{
		internal bool _lock;

		internal BroadPhase _broadPhase;
		private ContactManager _contactManager;

		private Body _bodyList;
		private Joint _jointList;
		private Controllers.Controller _controllerList;

		private Vec2 _raycastNormal;
		private object _raycastUserData;
		private Segment _raycastSegment;
		private bool _raycastSolidShape;

		// Do not access
		internal Contact _contactList;

		private int _bodyCount;
		internal int _contactCount;
		private int _jointCount;
		private int _controllerCount;

		private Vec2 _gravity;
		/// <summary>
		/// Get\Set global gravity vector.
		/// </summary>
		public Vec2 Gravity { get { return _gravity; } set { _gravity = value; } }

		private bool _allowSleep;

		private Body _groundBody;

		private DestructionListener _destructionListener;
		private BoundaryListener _boundaryListener;
		internal ContactFilter _contactFilter;
		internal ContactListener _contactListener;
		private DebugDraw _debugDraw;

		// This is used to compute the time step ratio to
		// support a variable time step.
		private float _inv_dt0;

		// This is for debugging the solver.
		private bool _warmStarting;

		// This is for debugging the solver.
		private bool _continuousPhysics;

		/// <summary>
		/// Construct a world object.
		/// </summary>
		/// <param name="worldAABB">A bounding box that completely encompasses all your shapes.</param>
		/// <param name="gravity">The world gravity vector.</param>
		/// <param name="doSleep">Improve performance by not simulating inactive bodies.</param>
		public World(AABB worldAABB, Vec2 gravity, bool doSleep)
		{
			_destructionListener = null;
			_boundaryListener = null;
			_contactFilter = null;
			_contactListener = null;
			_debugDraw = null;

			_bodyList = null;
			_contactList = null;
			_jointList = null;

			_bodyCount = 0;
			_contactCount = 0;
			_jointCount = 0;

			_warmStarting = true;
			_continuousPhysics = true;

			_allowSleep = doSleep;
			_gravity = gravity;

			_lock = false;

			_inv_dt0 = 0.0f;

			_contactManager = new ContactManager();
			_contactManager._world = this;
			_broadPhase = new BroadPhase(worldAABB, _contactManager);

			BodyDef bd = new BodyDef();
			_groundBody = CreateBody(bd);
		}

		/// <summary>
		/// Destruct the world. All physics entities are destroyed.
		/// </summary>
		public void Dispose()
		{
			DestroyBody(_groundBody);
			if (_broadPhase is IDisposable)
				(_broadPhase as IDisposable).Dispose();
			_broadPhase = null;
		}

		/// <summary>
		/// Register a destruction listener.
		/// </summary>
		/// <param name="listener"></param>
		public void SetDestructionListener(DestructionListener listener)
		{
			_destructionListener = listener;
		}

		/// <summary>
		/// Register a broad-phase boundary listener.
		/// </summary>
		/// <param name="listener"></param>
		public void SetBoundaryListener(BoundaryListener listener)
		{
			_boundaryListener = listener;
		}

		/// <summary>
		/// Register a contact filter to provide specific control over collision.
		/// Otherwise the default filter is used (b2_defaultFilter).
		/// </summary>
		/// <param name="filter"></param>
		public void SetContactFilter(ContactFilter filter)
		{
			_contactFilter = filter;
		}

		/// <summary>
		/// Register a contact event listener
		/// </summary>
		/// <param name="listener"></param>
		public void SetContactListener(ContactListener listener)
		{
			_contactListener = listener;
		}

		/// <summary>
		/// Register a routine for debug drawing. The debug draw functions are called
		/// inside the World.Step method, so make sure your renderer is ready to
		/// consume draw commands when you call Step().
		/// </summary>
		/// <param name="debugDraw"></param>
		public void SetDebugDraw(DebugDraw debugDraw)
		{
			_debugDraw = debugDraw;
		}

		/// <summary>
		/// Create a rigid body given a definition. No reference to the definition
		/// is retained.
		/// @warning This function is locked during callbacks.
		/// </summary>
		/// <param name="def"></param>
		/// <returns></returns>
		public Body CreateBody(BodyDef def)
		{
			Box2DXDebug.Assert(_lock == false);
			if (_lock == true)
			{
				return null;
			}

			Body b = new Body(def, this);

			// Add to world doubly linked list.
			b._prev = null;
			b._next = _bodyList;
			if (_bodyList != null)
			{
				_bodyList._prev = b;
			}
			_bodyList = b;
			++_bodyCount;

			return b;
		}

		/// <summary>
		/// Destroy a rigid body given a definition. No reference to the definition
		/// is retained. This function is locked during callbacks.
		/// @warning This automatically deletes all associated shapes and joints.
		/// @warning This function is locked during callbacks.
		/// </summary>
		/// <param name="b"></param>
		public void DestroyBody(Body b)
		{
			Box2DXDebug.Assert(_bodyCount > 0);
			Box2DXDebug.Assert(_lock == false);
			if (_lock == true)
			{
				return;
			}

			// Delete the attached joints.
			JointEdge jn = null;
			if (b._jointList != null)
				jn = b._jointList;
			while (jn != null)
			{
				JointEdge jn0 = jn;
				jn = jn.Next;

				if (_destructionListener != null)
				{
					_destructionListener.SayGoodbye(jn0.Joint);
				}

				DestroyJoint(jn0.Joint);
			}

			//Detach controllers attached to this body
			Controllers.ControllerEdge ce = b._controllerList;
			while (ce != null)
			{
				Controllers.ControllerEdge ce0 = ce;
				ce = ce.nextController;

				ce0.controller.RemoveBody(b);
			}

			// Delete the attached fixtures. This destroys broad-phase
			// proxies and pairs, leading to the destruction of contacts.
			Fixture f = b._fixtureList;
			while (f != null)
			{
				Fixture f0 = f;
				f = f.Next;

				if (_destructionListener != null)
				{
					_destructionListener.SayGoodbye(f0);
				}

				f0.Destroy(_broadPhase);
			}

			// Remove world body list.
			if (b._prev != null)
			{
				b._prev._next = b._next;
			}

			if (b._next != null)
			{
				b._next._prev = b._prev;
			}

			if (b == _bodyList)
			{
				_bodyList = b._next;
			}

			--_bodyCount;
			if (b is IDisposable)
				(b as IDisposable).Dispose();
			b = null;
		}

		/// <summary>
		/// Create a joint to constrain bodies together. No reference to the definition
		/// is retained. This may cause the connected bodies to cease colliding.
		/// @warning This function is locked during callbacks.
		/// </summary>
		/// <param name="def"></param>
		/// <returns></returns>
		public Joint CreateJoint(JointDef def)
		{
			Box2DXDebug.Assert(_lock == false);

			Joint j = Joint.Create(def);

			// Connect to the world list.
			j._prev = null;
			j._next = _jointList;
			if (_jointList != null)
			{
				_jointList._prev = j;
			}
			_jointList = j;
			++_jointCount;

			// Connect to the bodies' doubly linked lists.
			j._node1.Joint = j;
			j._node1.Other = j._body2;
			j._node1.Prev = null;
			j._node1.Next = j._body1._jointList;
			if (j._body1._jointList != null)
				j._body1._jointList.Prev = j._node1;
			j._body1._jointList = j._node1;

			j._node2.Joint = j;
			j._node2.Other = j._body1;
			j._node2.Prev = null;
			j._node2.Next = j._body2._jointList;
			if (j._body2._jointList != null)
				j._body2._jointList.Prev = j._node2;
			j._body2._jointList = j._node2;

			// If the joint prevents collisions, then reset collision filtering.
			if (def.CollideConnected == false)
			{
				// Reset the proxies on the body with the minimum number of shapes.
				Body b = def.Body1._fixtureCount < def.Body2._fixtureCount ? def.Body1 : def.Body2;
				for (Fixture f = b._fixtureList; f != null; f = f.Next)
				{
					f.RefilterProxy(_broadPhase, b.GetXForm());
				}
			}

			return j;
		}

		/// <summary>
		/// Destroy a joint. This may cause the connected bodies to begin colliding.
		/// @warning This function is locked during callbacks.
		/// </summary>
		/// <param name="j"></param>
		public void DestroyJoint(Joint j)
		{
			Box2DXDebug.Assert(_lock == false);

			bool collideConnected = j._collideConnected;

			// Remove from the doubly linked list.
			if (j._prev != null)
			{
				j._prev._next = j._next;
			}

			if (j._next != null)
			{
				j._next._prev = j._prev;
			}

			if (j == _jointList)
			{
				_jointList = j._next;
			}

			// Disconnect from island graph.
			Body body1 = j._body1;
			Body body2 = j._body2;

			// Wake up connected bodies.
			body1.WakeUp();
			body2.WakeUp();

			// Remove from body 1.
			if (j._node1.Prev != null)
			{
				j._node1.Prev.Next = j._node1.Next;
			}

			if (j._node1.Next != null)
			{
				j._node1.Next.Prev = j._node1.Prev;
			}

			if (j._node1 == body1._jointList)
			{
				body1._jointList = j._node1.Next;
			}

			j._node1.Prev = null;
			j._node1.Next = null;

			// Remove from body 2
			if (j._node2.Prev != null)
			{
				j._node2.Prev.Next = j._node2.Next;
			}

			if (j._node2.Next != null)
			{
				j._node2.Next.Prev = j._node2.Prev;
			}

			if (j._node2 == body2._jointList)
			{
				body2._jointList = j._node2.Next;
			}

			j._node2.Prev = null;
			j._node2.Next = null;

			Joint.Destroy(j);

			Box2DXDebug.Assert(_jointCount > 0);
			--_jointCount;

			// If the joint prevents collisions, then reset collision filtering.
			if (collideConnected == false)
			{
				// Reset the proxies on the body with the minimum number of shapes.
				Body b = body1._fixtureCount < body2._fixtureCount ? body1 : body2;
				for (Fixture f = b._fixtureList; f != null; f = f.Next)
				{
					f.RefilterProxy(_broadPhase, b.GetXForm());
				}
			}
		}

		public Controllers.Controller AddController(Controllers.Controller def)
		{
			def._next = _controllerList;
			def._prev = null;
			if (_controllerList != null)
				_controllerList._prev = def;
			_controllerList = def;
			++_controllerCount;

			def._world = this;

			return def;
		}

		public void RemoveController(Controllers.Controller controller)
		{
			Box2DXDebug.Assert(_controllerCount > 0);
			if (controller._next != null)
				controller._next._prev = controller._prev;
			if (controller._prev != null)
				controller._prev._next = controller._next;
			if (controller == _controllerList)
				_controllerList = controller._next;
			--_controllerCount;
		}

		/// <summary>
		/// The world provides a single static ground body with no collision shapes.
		/// You can use this to simplify the creation of joints and static shapes.
		/// </summary>
		/// <returns></returns>
		public Body GetGroundBody()
		{
			return _groundBody;
		}

		/// <summary>
		/// Get the world body list. With the returned body, use Body.GetNext to get
		/// the next body in the world list. A null body indicates the end of the list.
		/// </summary>
		/// <returns>The head of the world body list.</returns>
		public Body GetBodyList()
		{
			return _bodyList;
		}

		/// <summary>
		/// Get the world joint list. With the returned joint, use Joint.GetNext to get
		/// the next joint in the world list. A null joint indicates the end of the list.
		/// </summary>
		/// <returns>The head of the world joint list.</returns>
		public Joint GetJointList()
		{
			return _jointList;
		}

		public Controllers.Controller GetControllerList()
		{
			return _controllerList;
		}

		public int GetControllerCount()
		{
			return _controllerCount;
		}

		/// <summary>
		/// Re-filter a fixture. This re-runs contact filtering on a fixture.
		/// </summary>		
		public void Refilter(Fixture fixture)
		{
			Box2DXDebug.Assert(_lock == false);
			fixture.RefilterProxy(_broadPhase, fixture.Body.GetXForm());
		}

		/// <summary>
		/// Enable/disable warm starting. For testing.
		/// </summary>		
		public void SetWarmStarting(bool flag) { _warmStarting = flag; }

		/// <summary>
		/// Enable/disable continuous physics. For testing.
		/// </summary>		
		public void SetContinuousPhysics(bool flag) { _continuousPhysics = flag; }

		/// <summary>
		/// Perform validation of internal data structures.
		/// </summary>
		public void Validate() { _broadPhase.Validate(); }

		/// <summary>
		/// Get the number of broad-phase proxies.
		/// </summary>
		public int GetProxyCount() { return _broadPhase._proxyCount; }

		/// <summary>
		/// Get the number of broad-phase pairs.
		/// </summary>
		/// <returns></returns>
		public int GetPairCount() { return _broadPhase._pairManager._pairCount; }

		/// <summary>
		/// Get the number of bodies.
		/// </summary>
		/// <returns></returns>
		public int GetBodyCount() { return _bodyCount; }

		/// <summary>
		/// Get the number joints.
		/// </summary>
		/// <returns></returns>
		public int GetJointCount() { return _jointCount; }

		/// <summary>
		/// Get the number of contacts (each may have 0 or more contact points).
		/// </summary>
		/// <returns></returns>
		public int GetContactCount() { return _contactCount; }

		/// <summary>
		/// Take a time step. This performs collision detection, integration,
		/// and constraint solution.
		/// </summary>
		/// <param name="dt">The amount of time to simulate, this should not vary.</param>
		/// <param name="iterations">For the velocity constraint solver.</param>
		/// <param name="iterations">For the positionconstraint solver.</param>
		public void Step(float dt, int velocityIterations, int positionIteration)
		{
			_lock = true;

			TimeStep step = new TimeStep();
			step.Dt = dt;
			step.VelocityIterations = velocityIterations;
			step.PositionIterations = positionIteration;
			if (dt > 0.0f)
			{
				step.Inv_Dt = 1.0f / dt;
			}
			else
			{
				step.Inv_Dt = 0.0f;
			}

			step.DtRatio = _inv_dt0 * dt;

			step.WarmStarting = _warmStarting;

			// Update contacts.
			_contactManager.Collide();

			// Integrate velocities, solve velocity constraints, and integrate positions.
			if (step.Dt > 0.0f)
			{
				Solve(step);
			}

			// Handle TOI events.
			if (_continuousPhysics && step.Dt > 0.0f)
			{
				SolveTOI(step);
			}

			// Draw debug information.
			DrawDebugData();

			_inv_dt0 = step.Inv_Dt;
			_lock = false;
		}

		/// Query the world for all shapes that potentially overlap the
		/// provided AABB. You provide a shape pointer buffer of specified
		/// size. The number of shapes found is returned.
		/// @param aabb the query box.
		/// @param shapes a user allocated shape pointer array of size maxCount (or greater).
		/// @param maxCount the capacity of the shapes array.
		/// @return the number of shapes found in aabb.
		public int Query(AABB aabb, Fixture[] fixtures, int maxCount)
		{
			//using (object[] results = new object[maxCount])
			{
				object[] results = new object[maxCount];

				int count = _broadPhase.Query(aabb, results, maxCount);

				for (int i = 0; i < count; ++i)
				{
					fixtures[i] = (Fixture)results[i];
				}

				results = null;
				return count;
			}
		}

		/// <summary>
		/// Query the world for all shapes that intersect a given segment. You provide a shap
		/// pointer buffer of specified size. The number of shapes found is returned, and the buffer
		/// is filled in order of intersection.
		/// </summary>
		/// <param name="segment">Defines the begin and end point of the ray cast, from p1 to p2.
		/// Use Segment.Extend to create (semi-)infinite rays.</param>
		/// <param name="shapes">A user allocated shape pointer array of size maxCount (or greater).</param>
		/// <param name="maxCount">The capacity of the shapes array.</param>
		/// <param name="solidShapes">Determines if shapes that the ray starts in are counted as hits.</param>
		/// <param name="userData">Passed through the worlds contact filter, with method RayCollide. This can be used to filter valid shapes.</param>
		/// <returns>The number of shapes found</returns>
		public int Raycast(Segment segment, out Fixture[] fixtures, int maxCount, bool solidShapes, object userData)
		{
#warning "PTR"
			_raycastSegment = segment;
			_raycastUserData = userData;
			_raycastSolidShape = solidShapes;

			object[] results = new object[maxCount];
			fixtures = new Fixture[maxCount];
			int count = _broadPhase.QuerySegment(segment, results, maxCount, RaycastSortKey);

			for (int i = 0; i < count; ++i)
			{
				fixtures[i] = (Fixture)results[i];
			}

			return count;
		}

		/// <summary>
		/// Performs a raycast as with Raycast, finding the first intersecting shape.
		/// </summary>
		/// <param name="segment">Defines the begin and end point of the ray cast, from p1 to p2.
		/// Use Segment.Extend to create (semi-)infinite rays.</param>
		/// <param name="lambda">Returns the hit fraction. You can use this to compute the contact point
		/// p = (1 - lambda) * segment.p1 + lambda * segment.p2.</param>
		/// <param name="normal">Returns the normal at the contact point. If there is no intersection, the normal is not set.</param>
		/// <param name="solidShapes">Determines if shapes that the ray starts in are counted as hits.</param>
		/// <param name="userData"></param>
		/// <returns>Returns the colliding shape shape, or null if not found.</returns>
		public Fixture RaycastOne(Segment segment, out float lambda, out Vec2 normal, bool solidShapes, object userData)
		{
			int maxCount = 1;
			Fixture[] fixture;
			lambda = 0.0f;
			normal = new Vec2();

			int count = Raycast(segment, out fixture, maxCount, solidShapes, userData);

			if (count == 0)
				return null;

			Box2DXDebug.Assert(count == 1);

			//Redundantly do TestSegment a second time, as the previous one's results are inaccessible

			fixture[0].TestSegment(out lambda, out normal, segment, 1);
			//We already know it returns true
			return fixture[0];
		}

		// Find islands, integrate and solve constraints, solve position constraints
		private void Solve(TimeStep step)
		{
			// Step all controlls
			for (Controllers.Controller controller = _controllerList; controller != null; controller = controller._next)
			{
				controller.Step(step);
			}

			// Size the island for the worst case.
			Island island = new Island(_bodyCount, _contactCount, _jointCount, _contactListener);

			// Clear all the island flags.
			for (Body b = _bodyList; b != null; b = b._next)
			{
				b._flags &= ~Body.BodyFlags.Island;
			}
			for (Contact c = _contactList; c != null; c = c._next)
			{
				c._flags &= ~Contact.CollisionFlags.Island;
			}
			for (Joint j = _jointList; j != null; j = j._next)
			{
				j._islandFlag = false;
			}

			// Build and simulate all awake islands.
			int stackSize = _bodyCount;
			{
				Body[] stack = new Body[stackSize];

				for (Body seed = _bodyList; seed != null; seed = seed._next)
				{
					if ((seed._flags & (Body.BodyFlags.Island | Body.BodyFlags.Sleep | Body.BodyFlags.Frozen)) != 0)
					{
						continue;
					}

					if (seed.IsStatic())
					{
						continue;
					}

					// Reset island and stack.
					island.Clear();
					int stackCount = 0;
					stack[stackCount++] = seed;
					seed._flags |= Body.BodyFlags.Island;

					// Perform a depth first search (DFS) on the constraint graph.
					while (stackCount > 0)
					{
						// Grab the next body off the stack and add it to the island.
						Body b = stack[--stackCount];
						island.Add(b);

						// Make sure the body is awake.
						b._flags &= ~Body.BodyFlags.Sleep;

						// To keep islands as small as possible, we don't
						// propagate islands across static bodies.
						if (b.IsStatic())
						{
							continue;
						}

						// Search all contacts connected to this body.
						for (ContactEdge cn = b._contactList; cn != null; cn = cn.Next)
						{
							// Has this contact already been added to an island?
							if ((cn.Contact._flags & (Contact.CollisionFlags.Island | Contact.CollisionFlags.NonSolid)) != 0)
							{
								continue;
							}

							// Is this contact touching?
							if ((cn.Contact._flags & Contact.CollisionFlags.Touch) == (Contact.CollisionFlags)0)
							{
								continue;
							}

							island.Add(cn.Contact);
							cn.Contact._flags |= Contact.CollisionFlags.Island;

							Body other = cn.Other;

							// Was the other body already added to this island?
							if ((other._flags & Body.BodyFlags.Island) != 0)
							{
								continue;
							}

							Box2DXDebug.Assert(stackCount < stackSize);
							stack[stackCount++] = other;
							other._flags |= Body.BodyFlags.Island;
						}

						// Search all joints connect to this body.
						for (JointEdge jn = b._jointList; jn != null; jn = jn.Next)
						{
							if (jn.Joint._islandFlag == true)
							{
								continue;
							}

							island.Add(jn.Joint);
							jn.Joint._islandFlag = true;

							Body other = jn.Other;
							if ((other._flags & Body.BodyFlags.Island) != 0)
							{
								continue;
							}

							Box2DXDebug.Assert(stackCount < stackSize);
							stack[stackCount++] = other;
							other._flags |= Body.BodyFlags.Island;
						}
					}

					island.Solve(step, _gravity, _allowSleep);

					// Post solve cleanup.
					for (int i = 0; i < island._bodyCount; ++i)
					{
						// Allow static bodies to participate in other islands.
						Body b = island._bodies[i];
						if (b.IsStatic())
						{
							b._flags &= ~Body.BodyFlags.Island;
						}
					}
				}

				stack = null;
			}

			// Synchronize shapes, check for out of range bodies.
			for (Body b = _bodyList; b != null; b = b.GetNext())
			{
				if ((b._flags & (Body.BodyFlags.Sleep | Body.BodyFlags.Frozen)) != 0)
				{
					continue;
				}

				if (b.IsStatic())
				{
					continue;
				}

				// Update shapes (for broad-phase). If the shapes go out of
				// the world AABB then shapes and contacts may be destroyed,
				// including contacts that are
				bool inRange = b.SynchronizeFixtures();

				// Did the body's shapes leave the world?
				if (inRange == false && _boundaryListener != null)
				{
					_boundaryListener.Violation(b);
				}
			}

			// Commit shape proxy movements to the broad-phase so that new contacts are created.
			// Also, some contacts can be destroyed.
			_broadPhase.Commit();
		}

		// Find TOI contacts and solve them.
		private void SolveTOI(TimeStep step)
		{
			// Reserve an island and a queue for TOI island solution.
			Island island = new Island(_bodyCount, Settings.MaxTOIContactsPerIsland, Settings.MaxTOIJointsPerIsland, _contactListener);

			//Simple one pass queue
			//Relies on the fact that we're only making one pass
			//through and each body can only be pushed/popped once.
			//To push: 
			//  queue[queueStart+queueSize++] = newElement;
			//To pop: 
			//	poppedElement = queue[queueStart++];
			//  --queueSize;
			int queueCapacity = _bodyCount;
			Body[] queue = new Body[queueCapacity];

			for (Body b = _bodyList; b != null; b = b._next)
			{
				b._flags &= ~Body.BodyFlags.Island;
				b._sweep.T0 = 0.0f;
			}

			for (Contact c = _contactList; c != null; c = c._next)
			{
				// Invalidate TOI
				c._flags &= ~(Contact.CollisionFlags.Toi | Contact.CollisionFlags.Island);
			}

			for (Joint j = _jointList; j != null; j = j._next)
			{
				j._islandFlag = false;
			}

			// Find TOI events and solve them.
			for (; ; )
			{
				// Find the first TOI.
				Contact minContact = null;
				float minTOI = 1.0f;

				for (Contact c = _contactList; c != null; c = c._next)
				{
					if ((int)(c._flags & (Contact.CollisionFlags.Slow | Contact.CollisionFlags.NonSolid)) == 1)
					{
						continue;
					}

					// TODO_ERIN keep a counter on the contact, only respond to M TOIs per contact.

					float toi = 1.0f;
					if ((int)(c._flags & Contact.CollisionFlags.Toi) == 1)
					{
						// This contact has a valid cached TOI.
						toi = c._toi;
					}
					else
					{
						// Compute the TOI for this contact.
						Fixture s1 = c.FixtureA;
						Fixture s2 = c.FixtureB;
						Body b1 = s1.Body;
						Body b2 = s2.Body;

						if ((b1.IsStatic() || b1.IsSleeping()) && (b2.IsStatic() || b2.IsSleeping()))
						{
							continue;
						}

						// Put the sweeps onto the same time interval.
						float t0 = b1._sweep.T0;

						if (b1._sweep.T0 < b2._sweep.T0)
						{
							t0 = b2._sweep.T0;
							b1._sweep.Advance(t0);
						}
						else if (b2._sweep.T0 < b1._sweep.T0)
						{
							t0 = b1._sweep.T0;
							b2._sweep.Advance(t0);
						}

						Box2DXDebug.Assert(t0 < 1.0f);

						// Compute the time of impact.
						toi = c.ComputeTOI(b1._sweep, b2._sweep);
						//b2TimeOfImpact(c->m_fixtureA->GetShape(), b1->m_sweep, c->m_fixtureB->GetShape(), b2->m_sweep);

						Box2DXDebug.Assert(0.0f <= toi && toi <= 1.0f);

						// If the TOI is in range ...
						if (0.0f < toi && toi < 1.0f)
						{
							// Interpolate on the actual range.
							toi = Common.Math.Min((1.0f - toi) * t0 + toi, 1.0f);
						}


						c._toi = toi;
						c._flags |= Contact.CollisionFlags.Toi;
					}

					if (Settings.FLT_EPSILON < toi && toi < minTOI)
					{
						// This is the minimum TOI found so far.
						minContact = c;
						minTOI = toi;
					}
				}

				if (minContact == null || 1.0f - 100.0f * Settings.FLT_EPSILON < minTOI)
				{
					// No more TOI events. Done!
					break;
				}

				// Advance the bodies to the TOI.
				Fixture f1 = minContact.FixtureA;
				Fixture f2 = minContact.FixtureB;
				Body b3 = f1.Body;
				Body b4 = f2.Body;
				b3.Advance(minTOI);
				b4.Advance(minTOI);

				// The TOI contact likely has some new contact points.
				minContact.Update(_contactListener);
				minContact._flags &= ~Contact.CollisionFlags.Toi;

				if ((minContact._flags & Contact.CollisionFlags.Touch) == 0)
				{
					// This shouldn't happen. Numerical error?
					//b2Assert(false);
					continue;
				}

				// Build the TOI island. We need a dynamic seed.
				Body seed = b3;
				if (seed.IsStatic())
				{
					seed = b4;
				}

				// Reset island and queue.
				island.Clear();

				int queueStart = 0; // starting index for queue
				int queueSize = 0;  // elements in queue
				queue[queueStart + queueSize++] = seed;
				seed._flags |= Body.BodyFlags.Island;

				// Perform a breadth first search (BFS) on the contact/joint graph.
				while (queueSize > 0)
				{
					// Grab the next body off the stack and add it to the island.
					Body b = queue[queueStart++];
					--queueSize;

					island.Add(b);

					// Make sure the body is awake.
					b._flags &= ~Body.BodyFlags.Sleep;

					// To keep islands as small as possible, we don't
					// propagate islands across static bodies.
					if (b.IsStatic())
					{
						continue;
					}

					// Search all contacts connected to this body.
					for (ContactEdge cEdge = b._contactList; cEdge != null; cEdge = cEdge.Next)
					{
						// Does the TOI island still have space for contacts?
						if (island._contactCount == island._contactCapacity)
						{
							continue;
						}

						// Has this contact already been added to an island? Skip slow or non-solid contacts.
						if ((int)(cEdge.Contact._flags & (Contact.CollisionFlags.Island | Contact.CollisionFlags.Slow | Contact.CollisionFlags.NonSolid)) != 0)
						{
							continue;
						}

						// Is this contact touching? For performance we are not updating this contact.
						if ((cEdge.Contact._flags & Contact.CollisionFlags.Touch) == 0)
						{
							continue;
						}

						island.Add(cEdge.Contact);
						cEdge.Contact._flags |= Contact.CollisionFlags.Island;

						// Update other body.
						Body other = cEdge.Other;

						// Was the other body already added to this island?
						if ((int)(other._flags & Body.BodyFlags.Island) == 1)
						{
							continue;
						}

						// March forward, this can do no harm since this is the min TOI.
						if (other.IsStatic() == false)
						{
							other.Advance(minTOI);
							other.WakeUp();
						}

						//Box2DXDebug.Assert(queueStart + queueSize < queueCapacity);
						queue[queueStart + queueSize] = other;
						++queueSize;
						other._flags |= Body.BodyFlags.Island;
					}

					for (JointEdge jEdge = b._jointList; jEdge != null; jEdge = jEdge.Next)
					{
						if (island._jointCount == island._jointCapacity)
						{
							continue;
						}

						if (jEdge.Joint._islandFlag == true)
						{
							continue;
						}

						island.Add(jEdge.Joint);

						jEdge.Joint._islandFlag = true;

						Body other = jEdge.Other;

						if ((int)(other._flags & Body.BodyFlags.Island) == 1)
						{
							continue;
						}

						if (!other.IsStatic())
						{
							other.Advance(minTOI);
							other.WakeUp();
						}

						//Box2DXDebug.Assert(queueStart + queueSize < queueCapacity);
						queue[queueStart + queueSize] = other;
						++queueSize;
						other._flags |= Body.BodyFlags.Island;
					}
				}

				TimeStep subStep;
				subStep.WarmStarting = false;
				subStep.Dt = (1.0f - minTOI) * step.Dt;
				subStep.Inv_Dt = 1.0f / subStep.Dt;
				subStep.DtRatio = 0.0f;
				subStep.VelocityIterations = step.VelocityIterations;
				subStep.PositionIterations = step.PositionIterations;

				island.SolveTOI(ref subStep);

				// Post solve cleanup.
				for (int i = 0; i < island._bodyCount; ++i)
				{
					// Allow bodies to participate in future TOI islands.
					Body b = island._bodies[i];
					b._flags &= ~Body.BodyFlags.Island;

					if ((int)(b._flags & (Body.BodyFlags.Sleep | Body.BodyFlags.Frozen)) == 1)
					{
						continue;
					}

					if (b.IsStatic())
					{
						continue;
					}

					// Update fixtures (for broad-phase). If the fixtures go out of
					// the world AABB then fixtures and contacts may be destroyed,
					// including contacts that are
					bool inRange = b.SynchronizeFixtures();

					// Did the body's fixtures leave the world?
					if (inRange == false && _boundaryListener != null)
					{
						_boundaryListener.Violation(b);
					}

					// Invalidate all contact TOIs associated with this body. Some of these
					// may not be in the island because they were not touching.
					for (ContactEdge cn = b._contactList; cn != null; cn = cn.Next)
					{
						cn.Contact._flags &= ~Contact.CollisionFlags.Toi;
					}
				}

				for (int i = 0; i < island._contactCount; ++i)
				{
					// Allow contacts to participate in future TOI islands.
					Contact c = island._contacts[i];
					c._flags &= ~(Contact.CollisionFlags.Toi | Contact.CollisionFlags.Island);
				}

				for (int i = 0; i < island._jointCount; ++i)
				{
					// Allow joints to participate in future TOI islands.
					Joint j = island._joints[i];
					j._islandFlag = false;
				}

				// Commit fixture proxy movements to the broad-phase so that new contacts are created.
				// Also, some contacts can be destroyed.
				_broadPhase.Commit();
			}

			queue = null;
		}

		private void DrawJoint(Joint joint)
		{
			Body b1 = joint.GetBody1();
			Body b2 = joint.GetBody2();
			XForm xf1 = b1.GetXForm();
			XForm xf2 = b2.GetXForm();
			Vec2 x1 = xf1.Position;
			Vec2 x2 = xf2.Position;
			Vec2 p1 = joint.Anchor1;
			Vec2 p2 = joint.Anchor2;

			Color color = new Color(0.5f, 0.8f, 0.8f);

			switch (joint.GetType())
			{
				case JointType.DistanceJoint:
					_debugDraw.DrawSegment(p1, p2, color);
					break;

				case JointType.PulleyJoint:
					{
						PulleyJoint pulley = (PulleyJoint)joint;
						Vec2 s1 = pulley.GroundAnchor1;
						Vec2 s2 = pulley.GroundAnchor2;
						_debugDraw.DrawSegment(s1, p1, color);
						_debugDraw.DrawSegment(s2, p2, color);
						_debugDraw.DrawSegment(s1, s2, color);
					}
					break;

				case JointType.MouseJoint:
					// don't draw this
					break;

				default:
					_debugDraw.DrawSegment(x1, p1, color);
					_debugDraw.DrawSegment(p1, p2, color);
					_debugDraw.DrawSegment(x2, p2, color);
					break;
			}
		}

		private void DrawFixture(Fixture fixture, XForm xf, Color color, bool core)
		{
#warning "the core argument is not used, the coreColor variable is also not used"
			Color coreColor = new Color(0.9f, 0.6f, 0.6f);

			switch (fixture.ShapeType)
			{
				case ShapeType.CircleShape:
					{
						CircleShape circle = (CircleShape)fixture.Shape;

						Vec2 center = Common.Math.Mul(xf, circle._position);
						float radius = circle._radius;
						Vec2 axis = xf.R.Col1;

						_debugDraw.DrawSolidCircle(center, radius, axis, color);
					}
					break;

				case ShapeType.PolygonShape:
					{
						PolygonShape poly = (PolygonShape)fixture.Shape;
						int vertexCount = poly._vertexCount;
						Vec2[] localVertices = poly._vertices;

						Box2DXDebug.Assert(vertexCount <= Settings.MaxPolygonVertices);
						Vec2[] vertices = new Vec2[Settings.MaxPolygonVertices];

						for (int i = 0; i < vertexCount; ++i)
						{
							vertices[i] = Common.Math.Mul(xf, localVertices[i]);
						}

						_debugDraw.DrawSolidPolygon(vertices, vertexCount, color);
					}
					break;

				case ShapeType.EdgeShape:
					{
						EdgeShape edge = (EdgeShape)fixture.Shape;

						_debugDraw.DrawSegment(Common.Math.Mul(xf, edge.Vertex1), Common.Math.Mul(xf, edge.Vertex2), color);
					}
					break;
			}
		}

		private void DrawDebugData()
		{
			if (_debugDraw == null)
			{
				return;
			}

			DebugDraw.DrawFlags flags = _debugDraw.Flags;

			if ((flags & DebugDraw.DrawFlags.Shape) != 0)
			{
				bool core = (flags & DebugDraw.DrawFlags.CoreShape) == DebugDraw.DrawFlags.CoreShape;

				for (Body b = _bodyList; b != null; b = b.GetNext())
				{
					XForm xf = b.GetXForm();
					for (Fixture f = b.GetFixtureList(); f != null; f = f.Next)
					{
						if (b.IsStatic())
						{
							DrawFixture(f, xf, new Color(0.5f, 0.9f, 0.5f), core);
						}
						else if (b.IsSleeping())
						{
							DrawFixture(f, xf, new Color(0.5f, 0.5f, 0.9f), core);
						}
						else
						{
							DrawFixture(f, xf, new Color(0.9f, 0.9f, 0.9f), core);
						}
					}
				}
			}

			if ((flags & DebugDraw.DrawFlags.Joint) != 0)
			{
				for (Joint j = _jointList; j != null; j = j.GetNext())
				{
					if (j.GetType() != JointType.MouseJoint)
					{
						DrawJoint(j);
					}
				}
			}

			if ((flags & DebugDraw.DrawFlags.Controller) != 0)
			{
				for (Controllers.Controller c = _controllerList; c != null; c = c.GetNext())
				{
					c.Draw(_debugDraw);
				}
			}

			if ((flags & DebugDraw.DrawFlags.Pair) != 0)
			{
				BroadPhase bp = _broadPhase;
				Vec2 invQ = new Vec2();
				invQ.Set(1.0f / bp._quantizationFactor.X, 1.0f / bp._quantizationFactor.Y);
				Color color = new Color(0.9f, 0.9f, 0.3f);

				for (int i = 0; i < PairManager.TableCapacity; ++i)
				{
					ushort index = bp._pairManager._hashTable[i];
					while (index != PairManager.NullPair)
					{
						Pair pair = bp._pairManager._pairs[index];
						Proxy p1 = bp._proxyPool[pair.ProxyId1];
						Proxy p2 = bp._proxyPool[pair.ProxyId2];

						AABB b1 = new AABB(), b2 = new AABB();
						b1.LowerBound.X = bp._worldAABB.LowerBound.X + invQ.X * bp._bounds[0][p1.LowerBounds[0]].Value;
						b1.LowerBound.Y = bp._worldAABB.LowerBound.Y + invQ.Y * bp._bounds[1][p1.LowerBounds[1]].Value;
						b1.UpperBound.X = bp._worldAABB.LowerBound.X + invQ.X * bp._bounds[0][p1.UpperBounds[0]].Value;
						b1.UpperBound.Y = bp._worldAABB.LowerBound.Y + invQ.Y * bp._bounds[1][p1.UpperBounds[1]].Value;
						b2.LowerBound.X = bp._worldAABB.LowerBound.X + invQ.X * bp._bounds[0][p2.LowerBounds[0]].Value;
						b2.LowerBound.Y = bp._worldAABB.LowerBound.Y + invQ.Y * bp._bounds[1][p2.LowerBounds[1]].Value;
						b2.UpperBound.X = bp._worldAABB.LowerBound.X + invQ.X * bp._bounds[0][p2.UpperBounds[0]].Value;
						b2.UpperBound.Y = bp._worldAABB.LowerBound.Y + invQ.Y * bp._bounds[1][p2.UpperBounds[1]].Value;

						Vec2 x1 = 0.5f * (b1.LowerBound + b1.UpperBound);
						Vec2 x2 = 0.5f * (b2.LowerBound + b2.UpperBound);

						_debugDraw.DrawSegment(x1, x2, color);

						index = pair.Next;
					}
				}
			}

			if ((flags & DebugDraw.DrawFlags.Aabb) != 0)
			{
				BroadPhase bp = _broadPhase;
				Vec2 worldLower = bp._worldAABB.LowerBound;
				Vec2 worldUpper = bp._worldAABB.UpperBound;

				Vec2 invQ = new Vec2();
				invQ.Set(1.0f / bp._quantizationFactor.X, 1.0f / bp._quantizationFactor.Y);
				Color color = new Color(0.9f, 0.3f, 0.9f);
				for (int i = 0; i < Settings.MaxProxies; ++i)
				{
					Proxy p = bp._proxyPool[i];
					if (p.IsValid == false)
					{
						continue;
					}

					AABB b = new AABB();
					b.LowerBound.X = worldLower.X + invQ.X * bp._bounds[0][p.LowerBounds[0]].Value;
					b.LowerBound.Y = worldLower.Y + invQ.Y * bp._bounds[1][p.LowerBounds[1]].Value;
					b.UpperBound.X = worldLower.X + invQ.X * bp._bounds[0][p.UpperBounds[0]].Value;
					b.UpperBound.Y = worldLower.Y + invQ.Y * bp._bounds[1][p.UpperBounds[1]].Value;

					Vec2[] vs1 = new Vec2[4];
					vs1[0].Set(b.LowerBound.X, b.LowerBound.Y);
					vs1[1].Set(b.UpperBound.X, b.LowerBound.Y);
					vs1[2].Set(b.UpperBound.X, b.UpperBound.Y);
					vs1[3].Set(b.LowerBound.X, b.UpperBound.Y);

					_debugDraw.DrawPolygon(vs1, 4, color);
				}

				Vec2[] vs = new Vec2[4];
				vs[0].Set(worldLower.X, worldLower.Y);
				vs[1].Set(worldUpper.X, worldLower.Y);
				vs[2].Set(worldUpper.X, worldUpper.Y);
				vs[3].Set(worldLower.X, worldUpper.Y);
				_debugDraw.DrawPolygon(vs, 4, new Color(0.3f, 0.9f, 0.9f));
			}

			if ((flags & DebugDraw.DrawFlags.CenterOfMass) != 0)
			{
				for (Body b = _bodyList; b != null; b = b.GetNext())
				{
					XForm xf = b.GetXForm();
					xf.Position = b.GetWorldCenter();
					_debugDraw.DrawXForm(xf);
				}
			}
		}

		//Is it safe to pass private static function pointers?
		private static float RaycastSortKey(object data)
		{
			Fixture fixture = data as Fixture;
			Box2DXDebug.Assert(fixture != null);
			Body body = fixture.Body;
			World world = body.GetWorld();

			if (world._contactFilter != null && !world._contactFilter.RayCollide(world._raycastUserData, fixture))
				return -1;

			float lambda;

			SegmentCollide collide = fixture.TestSegment(out lambda, out world._raycastNormal, world._raycastSegment, 1);

			if (world._raycastSolidShape && collide == SegmentCollide.MissCollide)
				return -1;
			if (!world._raycastSolidShape && collide != SegmentCollide.HitCollide)
				return -1;

			return lambda;
		}

		public bool InRange(AABB aabb)
		{
			return _broadPhase.InRange(aabb);
		}
	}
}

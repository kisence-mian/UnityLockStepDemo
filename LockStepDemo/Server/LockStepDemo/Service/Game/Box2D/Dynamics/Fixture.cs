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
using System.Collections.Generic;
using System.Text;
using Box2DX.Collision;
using Box2DX.Common;

namespace Box2DX.Dynamics
{
	/// <summary>
	/// This holds contact filtering data.
	/// </summary>
	public struct FilterData
	{
		/// <summary>
		/// The collision category bits. Normally you would just set one bit.
		/// </summary>
		public ushort CategoryBits;

		/// <summary>
		/// The collision mask bits. This states the categories that this
		/// shape would accept for collision.
		/// </summary>
		public ushort MaskBits;

		/// <summary>
		/// Collision groups allow a certain group of objects to never collide (negative)
		/// or always collide (positive). Zero means no collision group. Non-zero group
		/// filtering always wins against the mask bits.
		/// </summary>
		public short GroupIndex;
	}

	/// <summary>
	/// A fixture definition is used to create a fixture. This class defines an
	/// abstract fixture definition. You can reuse fixture definitions safely.
	/// </summary>
	public class FixtureDef
	{
		/// <summary>
		/// The constructor sets the default fixture definition values.
		/// </summary>	
		public FixtureDef()
		{
			Type = ShapeType.UnknownShape;
			UserData = null;
			Friction = 0.2f;
			Restitution = 0.0f;
			Density = 0.0f;
			Filter.CategoryBits = 0x0001;
			Filter.MaskBits = 0xFFFF;
			Filter.GroupIndex = 0;
			IsSensor = false;
		}

		/// <summary>
		/// Holds the shape type for down-casting.
		/// </summary>
		public ShapeType Type;

		/// <summary>
		/// Use this to store application specific fixture data.
		/// </summary>
		public object UserData;

		/// <summary>
		/// The friction coefficient, usually in the range [0,1].
		/// </summary>
		public float Friction;

		/// <summary>
		/// The restitution (elasticity) usually in the range [0,1].
		/// </summary>
		public float Restitution;

		/// <summary>
		/// The density, usually in kg/m^2.
		/// </summary>
		public float Density;

		/// <summary>
		/// A sensor shape collects contact information but never generates a collision response.
		/// </summary>
		public bool IsSensor;

		/// <summary>
		/// Contact filtering data.
		/// </summary>
		public FilterData Filter;
	}

	/// <summary>
	/// This structure is used to build a fixture with a circle shape.
	/// </summary>
	public class CircleDef : FixtureDef
	{
		public Vec2 LocalPosition;
		public float Radius;

		public CircleDef()
		{
			Type = ShapeType.CircleShape;
			LocalPosition = Vec2.Zero;
			Radius = 1.0f;
		}
	}

	/// <summary>
	/// Convex polygon. The vertices must be ordered so that the outside of
	/// the polygon is on the right side of the edges (looking along the edge
	/// from start to end).
	/// </summary>
	public class PolygonDef : FixtureDef
	{
		/// <summary>
		/// The number of polygon vertices.
		/// </summary>
		public int VertexCount;

		/// <summary>
		/// The polygon vertices in local coordinates.
		/// </summary>
		public Vec2[] Vertices = new Vec2[Settings.MaxPolygonVertices];

		public PolygonDef()
		{
			Type = ShapeType.PolygonShape;
			VertexCount = 0;
		}

		/// <summary>
		/// Build vertices to represent an axis-aligned box.
		/// </summary>
		/// <param name="hx">The half-width</param>
		/// <param name="hy">The half-height.</param>
		public void SetAsBox(float hx, float hy)
		{
			VertexCount = 4;
			Vertices[0].Set(-hx, -hy);
			Vertices[1].Set(hx, -hy);
			Vertices[2].Set(hx, hy);
			Vertices[3].Set(-hx, hy);
		}


		/// <summary>
		/// Build vertices to represent an oriented box.
		/// </summary>
		/// <param name="hx">The half-width</param>
		/// <param name="hy">The half-height.</param>
		/// <param name="center">The center of the box in local coordinates.</param>
		/// <param name="angle">The rotation of the box in local coordinates.</param>
		public void SetAsBox(float hx, float hy, Vec2 center, float angle)
		{
			SetAsBox(hx, hy);

			XForm xf = new XForm();
			xf.Position = center;
			xf.R.Set(angle);

			for (int i = 0; i < VertexCount; ++i)
			{
				Vertices[i] = Common.Math.Mul(xf, Vertices[i]);
			}
		}
	}

	/// <summary>
	/// This structure is used to build a chain of edges.
	/// </summary>
	public class EdgeDef : FixtureDef
	{
		public EdgeDef()
		{
			Type = ShapeType.EdgeShape;
		}

		/// <summary>
		/// The start vertex.
		/// </summary>
		public Vec2 Vertex1;

		/// <summary>
		/// The end vertex.
		/// </summary>
		public Vec2 Vertex2;
	}

	/// <summary>
	/// A fixture is used to attach a shape to a body for collision detection. A fixture
	/// inherits its transform from its parent. Fixtures hold additional non-geometric data
	/// such as friction, collision filters, etc.
	/// Fixtures are created via Body.CreateFixture.
	/// @warning you cannot reuse fixtures.
	/// </summary>
	public class Fixture
	{
		protected ShapeType _type;
		protected bool _isSensor;
		protected UInt16 _proxyId;

		internal Body _body;
		protected Shape _shape;
		internal Fixture _next;

		/// <summary>
		/// Contact filtering data. You must call b2World::Refilter to correct
		/// existing contacts/non-contacts.
		/// </summary>
		public FilterData Filter;

		/// <summary>
		/// Is this fixture a sensor (non-solid)?
		/// </summary>
		public bool IsSensor { get { return _isSensor; } }

		/// <summary>
		/// Get the child shape. You can modify the child shape, however you should not change the
		/// number of vertices because this will crash some collision caching mechanisms.
		/// </summary>
		public Shape Shape { get { return _shape; } }

		/// <summary>
		/// Get the type of this shape. You can use this to down cast to the concrete shape.
		/// </summary>
		public ShapeType ShapeType { get { return _type; } }

		/// <summary>
		/// Get the next fixture in the parent body's fixture list.
		/// </summary>
		public Fixture Next { get { return _next; } }

		/// <summary>
		/// Get the parent body of this fixture. This is NULL if the fixture is not attached.
		/// </summary>
		public Body Body { get { return _body; } }

		/// <summary>
		/// User data that was assigned in the fixture definition. Use this to
		/// store your application specific data.
		/// </summary>
		public object UserData;

		/// <summary>
		/// Friction coefficient, usually in the range [0,1].
		/// </summary>
		public float Friction;

		/// <summary>
		/// Restitution (elasticity) usually in the range [0,1].
		/// </summary>
		public float Restitution;

		/// <summary>
		/// Density, usually in kg/m^2.
		/// </summary>
		public float Density;

		public Fixture()
		{
			_proxyId = PairManager.NullProxy;
		}

		public void Create(BroadPhase broadPhase, Body body, XForm xf, FixtureDef def)
		{
			UserData = def.UserData;
			Friction = def.Friction;
			Restitution = def.Restitution;
			Density = def.Density;

			_body = body;
			_next = null;

			Filter = def.Filter;

			_isSensor = def.IsSensor;

			_type = def.Type;

			// Allocate and initialize the child shape.
			switch (_type)
			{
				case ShapeType.CircleShape:
					{
						CircleShape circle = new CircleShape();
						CircleDef circleDef = (CircleDef)def;
						circle._position = circleDef.LocalPosition;
						circle._radius = circleDef.Radius;
						_shape = circle;
					}
					break;

				case ShapeType.PolygonShape:
					{
						PolygonShape polygon = new PolygonShape();
						PolygonDef polygonDef = (PolygonDef)def;
						polygon.Set(polygonDef.Vertices, polygonDef.VertexCount);
						_shape = polygon;
					}
					break;

				case ShapeType.EdgeShape:
					{
						EdgeShape edge = new EdgeShape();
						EdgeDef edgeDef = (EdgeDef)def;
						edge.Set(edgeDef.Vertex1, edgeDef.Vertex2);
						_shape = edge;
					}
					break;

				default:
					Box2DXDebug.Assert(false);
					break;
			}

			// Create proxy in the broad-phase.
			AABB aabb;
			_shape.ComputeAABB(out aabb, xf);

			bool inRange = broadPhase.InRange(aabb);

			// You are creating a shape outside the world box.
			Box2DXDebug.Assert(inRange);

			if (inRange)
			{
				_proxyId = broadPhase.CreateProxy(aabb, this);
			}
			else
			{
				_proxyId = PairManager.NullProxy;
			}
		}

		public void Destroy(BroadPhase broadPhase)
		{
			// Remove proxy from the broad-phase.
			if (_proxyId != PairManager.NullProxy)
			{
				broadPhase.DestroyProxy(_proxyId);
				_proxyId = PairManager.NullProxy;
			}

			// Free the child shape.
			_shape.Dispose();
			_shape = null;
		}

		internal bool Synchronize(BroadPhase broadPhase, XForm transform1, XForm transform2)
		{
			if (_proxyId == PairManager.NullProxy)
			{
				return false;
			}

			// Compute an AABB that covers the swept shape (may miss some rotation effect).
			AABB aabb1, aabb2;
			_shape.ComputeAABB(out aabb1, transform1);
			_shape.ComputeAABB(out aabb2, transform2);

			AABB aabb = new AABB();
			aabb.Combine(aabb1, aabb2);

			if (broadPhase.InRange(aabb))
			{
				broadPhase.MoveProxy(_proxyId, aabb);
				return true;
			}
			else
			{
				return false;
			}
		}

		internal void RefilterProxy(BroadPhase broadPhase, XForm transform)
		{
			if (_proxyId == PairManager.NullProxy)
			{
				return;
			}

			broadPhase.DestroyProxy(_proxyId);

			AABB aabb;
			_shape.ComputeAABB(out aabb, transform);

			bool inRange = broadPhase.InRange(aabb);

			if (inRange)
			{
				_proxyId = broadPhase.CreateProxy(aabb, this);
			}
			else
			{
				_proxyId = PairManager.NullProxy;
			}
		}

		public virtual void Dispose()
		{
			Box2DXDebug.Assert(_proxyId == PairManager.NullProxy);
			Box2DXDebug.Assert(_shape == null);
		}

		/// <summary>
		/// Compute the mass properties of this shape using its dimensions and density.
		/// The inertia tensor is computed about the local origin, not the centroid.
		/// </summary>
		/// <param name="massData">Returns the mass data for this shape.</param>
		public void ComputeMass(out MassData massData)
		{
			_shape.ComputeMass(out massData, Density);
		}

		/// <summary>
		/// Compute the volume and centroid of this fixture intersected with a half plane.
		/// </summary>
		/// <param name="normal">Normal the surface normal.</param>
		/// <param name="offset">Offset the surface offset along normal.</param>
		/// <param name="c">Returns the centroid.</param>
		/// <returns>The total volume less than offset along normal.</returns>
		public float ComputeSubmergedArea(Vec2 normal, float offset, out Vec2 c)
		{
			return _shape.ComputeSubmergedArea(normal, offset, _body.GetXForm(), out c);
		}

		/// <summary>
		/// Test a point for containment in this fixture. This only works for convex shapes.
		/// </summary>
		/// <param name="p">A point in world coordinates.</param>
		public bool TestPoint(Vec2 p)
		{
			return _shape.TestPoint(_body.GetXForm(), p);
		}

		/// <summary>
		/// Perform a ray cast against this shape.
		/// </summary>
		/// <param name="lambda">Returns the hit fraction. You can use this to compute the contact point
		/// p = (1 - lambda) * segment.p1 + lambda * segment.p2.</param>
		/// <param name="normal">Returns the normal at the contact point. If there is no intersection, the normal
		/// is not set.</param>
		/// <param name="segment">Defines the begin and end point of the ray cast.</param>
		/// <param name="maxLambda">A number typically in the range [0,1].</param>
		public SegmentCollide TestSegment(out float lambda, out Vec2 normal, Segment segment, float maxLambda)
		{
			return _shape.TestSegment(_body.GetXForm(), out lambda, out normal, segment, maxLambda);
		}

		/// <summary>
		/// Get the maximum radius about the parent body's center of mass.
		/// </summary>
		public float ComputeSweepRadius(Vec2 pivot)
		{
			return _shape.ComputeSweepRadius(pivot);
		}
	}
}

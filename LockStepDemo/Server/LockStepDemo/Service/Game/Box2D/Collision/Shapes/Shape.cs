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

namespace Box2DX.Collision
{
	/// <summary>
	/// This holds the mass data computed for a shape.
	/// </summary>
	public struct MassData
	{
		/// <summary>
		/// The mass of the shape, usually in kilograms.
		/// </summary>
		public float Mass;

		/// <summary>
		/// The position of the shape's centroid relative to the shape's origin.
		/// </summary>
		public Vec2 Center;

		/// <summary>
		/// The rotational inertia of the shape.
		/// </summary>
		public float I;
	}

	/// <summary>
	/// The various collision shape types supported by Box2D.
	/// </summary>
	public enum ShapeType
	{
		UnknownShape = -1,
		CircleShape,
		PolygonShape,
		EdgeShape,
		ShapeTypeCount,
	}

	/// <summary>
	/// Returns code from TestSegment
	/// </summary>
	public enum SegmentCollide
	{
		StartInsideCollide = -1,
		MissCollide = 0,
		HitCollide = 1
	}

	/// <summary>
	/// A shape is used for collision detection. You can create a shape however you like.
	/// Shapes used for simulation in World are created automatically when a Fixture is created.
	/// </summary>
	public abstract class Shape : IDisposable
	{
		#region Fields

		protected ShapeType _type = ShapeType.UnknownShape;
		internal float _radius;

		#endregion Fields

		protected Shape() { }

		/// <summary>
		/// Test a point for containment in this shape. This only works for convex shapes.
		/// </summary>
		/// <param name="xf">The shape world transform.</param>
		/// <param name="p">A point in world coordinates.</param>
		/// <returns></returns>
		public abstract bool TestPoint(XForm xf, Vec2 p);

		/// <summary>
		/// Perform a ray cast against this shape.
		/// </summary>
		/// <param name="xf">The shape world transform.</param>
		/// <param name="lambda">Returns the hit fraction. You can use this to compute the contact point
		/// p = (1 - lambda) * segment.P1 + lambda * segment.P2.</param>
		/// <param name="normal"> Returns the normal at the contact point. If there is no intersection, 
		/// the normal is not set.</param>
		/// <param name="segment">Defines the begin and end point of the ray cast.</param>
		/// <param name="maxLambda">A number typically in the range [0,1].</param>
		public abstract SegmentCollide TestSegment(XForm xf, out float lambda, out Vec2 normal, Segment segment, float maxLambda);

		/// <summary>
		/// Given a transform, compute the associated axis aligned bounding box for this shape.
		/// </summary>
		/// <param name="aabb">Returns the axis aligned box.</param>
		/// <param name="xf">The world transform of the shape.</param>
		public abstract void ComputeAABB(out AABB aabb, XForm xf);

		/// <summary>
		/// Compute the mass properties of this shape using its dimensions and density.
		/// The inertia tensor is computed about the local origin, not the centroid.
		/// </summary>
		/// <param name="massData">Returns the mass data for this shape</param>
		public abstract void ComputeMass(out MassData massData, float density);

		/// <summary>
		/// Compute the volume and centroid of this shape intersected with a half plane.
		/// </summary>
		/// <param name="normal">Normal the surface normal.</param>
		/// <param name="offset">Offset the surface offset along normal.</param>
		/// <param name="xf">The shape transform.</param>
		/// <param name="c">Returns the centroid.</param>
		/// <returns>The total volume less than offset along normal.</returns>
		public abstract float ComputeSubmergedArea(Vec2 normal, float offset, XForm xf, out Vec2 c);

		/// <summary>
		/// Compute the sweep radius. This is used for conservative advancement (continuous collision detection).
		/// </summary>
		/// <param name="pivot">Pivot is the pivot point for rotation.</param>
		/// <returns>The distance of the furthest point from the pivot.</returns>
		public abstract float ComputeSweepRadius(Vec2 pivot);

		public abstract Vec2 GetVertex(int index);

		public abstract int GetSupport(Vec2 d);

		public abstract Vec2 GetSupportVertex(Vec2 d);

		public virtual void Dispose(){}
 	}
}
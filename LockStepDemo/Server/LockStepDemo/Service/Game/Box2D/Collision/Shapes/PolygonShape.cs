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

#define DEBUG

using System;
using System.Collections.Generic;
using System.Text;

using Box2DX.Common;

namespace Box2DX.Collision
{
	/// <summary>
	/// A convex polygon. It is assumed that the interior of the polygon is to the left of each edge.
	/// </summary>
	public class PolygonShape : Shape
	{
		internal Vec2 _centroid;
		internal Vec2[] _vertices = new Vec2[Settings.MaxPolygonVertices];
		internal Vec2[] _normals = new Vec2[Settings.MaxPolygonVertices];

		internal int _vertexCount;

		public int VertexCount
		{
			get { return _vertexCount; }
		}

		public Vec2[] Vertices
		{
			get { return _vertices; }
		}

		/// <summary>
		/// Copy vertices. This assumes the vertices define a convex polygon.
		/// It is assumed that the exterior is the the right of each edge.
		/// </summary>
		public void Set(Vec2[] vertices, int count)
		{
			Box2DXDebug.Assert(3 <= count && count <= Settings.MaxPolygonVertices);
			_vertexCount = count;

			int i;
			// Copy vertices.
			for (i = 0; i < _vertexCount; ++i)
			{
				_vertices[i] = vertices[i];
			}

			// Compute normals. Ensure the edges have non-zero length.
			for (i = 0; i < _vertexCount; ++i)
			{
				int i1 = i;
				int i2 = i + 1 < count ? i + 1 : 0;
				Vec2 edge = _vertices[i2] - _vertices[i1];
				Box2DXDebug.Assert(edge.LengthSquared() > Settings.FLT_EPSILON_SQUARED);
				_normals[i] = Vec2.Cross(edge, 1.0f);
				_normals[i].Normalize();
			}

#if DEBUG
			// Ensure the polygon is convex and the interior
			// is to the left of each edge.
			for (i = 0; i < _vertexCount; ++i)
			{
				int i1 = i;
				int i2 = i + 1 < count ? i + 1 : 0;
				Vec2 edge = _vertices[i2] - _vertices[i1];

				for (int j = 0; j < _vertexCount; ++j)
				{
					// Don't check vertices on the current edge.
					if (j == i1 || j == i2)
					{
						continue;
					}

					Vec2 r = _vertices[j] - _vertices[i1];

					// Your polygon is non-convex (it has an indentation) or
					// has colinear edges.
					float s = Vec2.Cross(edge, r);
					Box2DXDebug.Assert(s > 0.0f);
				}
			}
#endif

			// Compute the polygon centroid.
			_centroid = ComputeCentroid(_vertices, _vertexCount);
		}

		/// <summary>
		/// Build vertices to represent an axis-aligned box.
		/// </summary>
		/// <param name="hx">The half-width</param>
		/// <param name="hy">The half-height.</param>
		public void SetAsBox(float hx, float hy)
		{
			_vertexCount = 4;
			_vertices[0].Set(-hx, -hy);
			_vertices[1].Set(hx, -hy);
			_vertices[2].Set(hx, hy);
			_vertices[3].Set(-hx, hy);
			_normals[0].Set(0.0f, -1.0f);
			_normals[1].Set(1.0f, 0.0f);
			_normals[2].Set(0.0f, 1.0f);
			_normals[3].Set(-1.0f, 0.0f);
			_centroid = new Vec2(0);
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

			// Transform vertices and normals.
			for (int i = 0; i < _vertexCount; ++i)
			{
				_vertices[i] = Common.Math.Mul(xf, _vertices[i]);
				_normals[i] = Common.Math.Mul(xf.R, _normals[i]);
			}
		}

		public void SetAsEdge(Vec2 v1, Vec2 v2)
		{
			_vertexCount = 2;
			_vertices[0] = v1;
			_vertices[1] = v2;
			_centroid = 0.5f * (v1 + v2);
			_normals[0] = Vec2.Cross(v2 - v1, 1.0f);
			_normals[0].Normalize();
			_normals[1] = -_normals[0];
		}

		public override bool TestPoint(XForm xf, Vec2 p)
		{
			Vec2 pLocal = Common.Math.MulT(xf.R, p - xf.Position);

			int vc = _vertexCount;
			for (int i = 0; i < vc; ++i)
			{
				float dot = Vec2.Dot(_normals[i], pLocal - _vertices[i]);
				if (dot > 0.0f)
				{
					return false;
				}
			}

			return true;
		}

		public override SegmentCollide TestSegment(XForm xf, out float lambda, out Vec2 normal, Segment segment, float maxLambda)
		{
			lambda = 0f;
			normal = Vec2.Zero;

			float lower = 0.0f, upper = maxLambda;

			Vec2 p1 = Common.Math.MulT(xf.R, segment.P1 - xf.Position);
			Vec2 p2 = Common.Math.MulT(xf.R, segment.P2 - xf.Position);
			Vec2 d = p2 - p1;
			int index = -1;

			for (int i = 0; i < _vertexCount; ++i)
			{
				// p = p1 + a * d
				// dot(normal, p - v) = 0
				// dot(normal, p1 - v) + a * dot(normal, d) = 0
				float numerator = Vec2.Dot(_normals[i], _vertices[i] - p1);
				float denominator = Vec2.Dot(_normals[i], d);

				if (denominator == 0.0f)
				{
					if (numerator < 0.0f)
					{
						return SegmentCollide.MissCollide;
					}
				}
				else
				{
					// Note: we want this predicate without division:
					// lower < numerator / denominator, where denominator < 0
					// Since denominator < 0, we have to flip the inequality:
					// lower < numerator / denominator <==> denominator * lower > numerator.
					if (denominator < 0.0f && numerator < lower * denominator)
					{
						// Increase lower.
						// The segment enters this half-space.
						lower = numerator / denominator;
						index = i;
					}
					else if (denominator > 0.0f && numerator < upper * denominator)
					{
						// Decrease upper.
						// The segment exits this half-space.
						upper = numerator / denominator;
					}
				}

				if (upper < lower)
				{
					return SegmentCollide.MissCollide;
				}
			}

			Box2DXDebug.Assert(0.0f <= lower && lower <= maxLambda);

			if (index >= 0)
			{
				lambda = lower;
				normal = Common.Math.Mul(xf.R, _normals[index]);
				return SegmentCollide.HitCollide;
			}

			lambda = 0f;
			return SegmentCollide.StartInsideCollide;
		}

		public override void ComputeAABB(out AABB aabb, XForm xf)
		{
			Vec2 lower = Common.Math.Mul(xf, _vertices[0]);
			Vec2 upper = lower;

			for (int i = 1; i < _vertexCount; ++i)
			{
				Vec2 v = Common.Math.Mul(xf, _vertices[i]);
				lower = Common.Math.Min(lower, v);
				upper = Common.Math.Max(upper, v);
			}

			Vec2 r = new Vec2(_radius);
			aabb.LowerBound = lower - r;
			aabb.UpperBound = upper + r;
		}

		public override void ComputeMass(out MassData massData, float denstity)
		{
			// Polygon mass, centroid, and inertia.
			// Let rho be the polygon density in mass per unit area.
			// Then:
			// mass = rho * int(dA)
			// centroid.x = (1/mass) * rho * int(x * dA)
			// centroid.y = (1/mass) * rho * int(y * dA)
			// I = rho * int((x*x + y*y) * dA)
			//
			// We can compute these integrals by summing all the integrals
			// for each triangle of the polygon. To evaluate the integral
			// for a single triangle, we make a change of variables to
			// the (u,v) coordinates of the triangle:
			// x = x0 + e1x * u + e2x * v
			// y = y0 + e1y * u + e2y * v
			// where 0 <= u && 0 <= v && u + v <= 1.
			//
			// We integrate u from [0,1-v] and then v from [0,1].
			// We also need to use the Jacobian of the transformation:
			// D = cross(e1, e2)
			//
			// Simplification: triangle centroid = (1/3) * (p1 + p2 + p3)
			//
			// The rest of the derivation is handled by computer algebra.

			Box2DXDebug.Assert(_vertexCount >= 3);

			Vec2 center = new Vec2(0);
			float area = 0.0f;
			float I = 0.0f;

			// pRef is the reference point for forming triangles.
			// It's location doesn't change the result (except for rounding error).
			Vec2 pRef = new Vec2(0);

#if O
			// This code would put the reference point inside the polygon.
			for (int i = 0; i < vCount; ++i)
			{
				pRef += _vertices[i];
			}
			pRef *= 1.0f / count;
#endif

			const float k_inv3 = 1.0f / 3.0f;

			for (int i = 0; i < _vertexCount; ++i)
			{
				// Triangle vertices.
				Vec2 p1 = pRef;
				Vec2 p2 = _vertices[i];
				Vec2 p3 = i + 1 < _vertexCount ? _vertices[i + 1] : _vertices[0];

				Vec2 e1 = p2 - p1;
				Vec2 e2 = p3 - p1;

				float D = Vec2.Cross(e1, e2);

				float triangleArea = 0.5f * D;
				area += triangleArea;

				// Area weighted centroid
				center += triangleArea * k_inv3 * (p1 + p2 + p3);

				float px = p1.X, py = p1.Y;
				float ex1 = e1.X, ey1 = e1.Y;
				float ex2 = e2.X, ey2 = e2.Y;

				float intx2 = k_inv3 * (0.25f * (ex1 * ex1 + ex2 * ex1 + ex2 * ex2) + (px * ex1 + px * ex2)) + 0.5f * px * px;
				float inty2 = k_inv3 * (0.25f * (ey1 * ey1 + ey2 * ey1 + ey2 * ey2) + (py * ey1 + py * ey2)) + 0.5f * py * py;

				I += D * (intx2 + inty2);
			}

			// Total mass
			massData.Mass = denstity * area;

			// Center of mass
			Box2DXDebug.Assert(area > Common.Settings.FLT_EPSILON);
			center *= 1.0f / area;
			massData.Center = center;

			// Inertia tensor relative to the local origin.
			massData.I = denstity * I;
		}

		public override float ComputeSubmergedArea(Vec2 normal, float offset, XForm xf, out Vec2 c)
		{
			//Transform plane into shape co-ordinates
			Vec2 normalL = Box2DX.Common.Math.MulT(xf.R, normal);
			float offsetL = offset - Vec2.Dot(normal, xf.Position);

			float[] depths = new float[Common.Settings.MaxPolygonVertices];
			int diveCount = 0;
			int intoIndex = -1;
			int outoIndex = -1;

			bool lastSubmerged = false;
			int i;
			for (i = 0; i < _vertexCount; i++)
			{
				depths[i] = Vec2.Dot(normalL, _vertices[i]) - offsetL;
				bool isSubmerged = depths[i] < -Common.Settings.FLT_EPSILON;
				if (i > 0)
				{
					if (isSubmerged)
					{
						if (!lastSubmerged)
						{
							intoIndex = i - 1;
							diveCount++;
						}
					}
					else
					{
						if (lastSubmerged)
						{
							outoIndex = i - 1;
							diveCount++;
						}
					}
				}
				lastSubmerged = isSubmerged;
			}
			switch (diveCount)
			{
				case 0:
					if (lastSubmerged)
					{
						//Completely submerged
						MassData md;
						ComputeMass(out md, 1f);
						c = Common.Math.Mul(xf, md.Center);
						return md.Mass;
					}
					else
					{
						//Completely dry
						c = new Vec2();
						return 0;
					}
					break;
				case 1:
					if (intoIndex == -1)
					{
						intoIndex = _vertexCount - 1;
					}
					else
					{
						outoIndex = _vertexCount - 1;
					}
					break;
			}
			int intoIndex2 = (intoIndex + 1) % _vertexCount;
			int outoIndex2 = (outoIndex + 1) % _vertexCount;

			float intoLambda = (0 - depths[intoIndex]) / (depths[intoIndex2] - depths[intoIndex]);
			float outoLambda = (0 - depths[outoIndex]) / (depths[outoIndex2] - depths[outoIndex]);

			Vec2 intoVec = new Vec2(_vertices[intoIndex].X * (1 - intoLambda) + _vertices[intoIndex2].X * intoLambda,
							_vertices[intoIndex].Y * (1 - intoLambda) + _vertices[intoIndex2].Y * intoLambda);
			Vec2 outoVec = new Vec2(_vertices[outoIndex].X * (1 - outoLambda) + _vertices[outoIndex2].X * outoLambda,
							_vertices[outoIndex].Y * (1 - outoLambda) + _vertices[outoIndex2].Y * outoLambda);

			//Initialize accumulator
			float area = 0;
			Vec2 center = new Vec2(0);
			Vec2 p2 = _vertices[intoIndex2];
			Vec2 p3;

			const float k_inv3 = 1.0f / 3.0f;

			//An awkward loop from intoIndex2+1 to outIndex2
			i = intoIndex2;
			while (i != outoIndex2)
			{
				i = (i + 1) % _vertexCount;
				if (i == outoIndex2)
					p3 = outoVec;
				else
					p3 = _vertices[i];
				//Add the triangle formed by intoVec,p2,p3
				{
					Vec2 e1 = p2 - intoVec;
					Vec2 e2 = p3 - intoVec;

					float D = Vec2.Cross(e1, e2);

					float triangleArea = 0.5f * D;

					area += triangleArea;

					// Area weighted centroid
					center += triangleArea * k_inv3 * (intoVec + p2 + p3);

				}
				//
				p2 = p3;
			}

			//Normalize and transform centroid
			center *= 1.0f / area;

			c = Common.Math.Mul(xf, center);

			return area;
		}

		public override float ComputeSweepRadius(Vec2 pivot)
		{
			int vCount = _vertexCount;
			Box2DXDebug.Assert(vCount > 0);
			float sr = Vec2.DistanceSquared(_vertices[0], pivot);
			for (int i = 1; i < vCount; ++i)
			{
				sr = Common.Math.Max(sr, Vec2.DistanceSquared(_vertices[i], pivot));
			}

			return Common.Math.Sqrt(sr);
		}

		/// <summary>
		/// Get the supporting vertex index in the given direction.
		/// </summary>
		public override int GetSupport(Vec2 d)
		{
			int bestIndex = 0;
			float bestValue = Vec2.Dot(_vertices[0], d);
			for (int i = 1; i < _vertexCount; ++i)
			{
				float value = Vec2.Dot(_vertices[i], d);
				if (value > bestValue)
				{
					bestIndex = i;
					bestValue = value;
				}
			}

			return bestIndex;
		}

		public override Vec2 GetSupportVertex(Vec2 d)
		{
			int bestIndex = 0;
			float bestValue = Vec2.Dot(_vertices[0], d);
			for (int i = 1; i < _vertexCount; ++i)
			{
				float value = Vec2.Dot(_vertices[i], d);
				if (value > bestValue)
				{
					bestIndex = i;
					bestValue = value;
				}
			}

			return _vertices[bestIndex];
		}

		public override Vec2 GetVertex(int index)
		{
			Box2DXDebug.Assert(0 <= index && index < _vertexCount);
			return _vertices[index];
		}

		public static Vec2 ComputeCentroid(Vec2[] vs, int count)
		{
			Box2DXDebug.Assert(count >= 3);

			Vec2 c = new Vec2(0f);
			float area = 0f;

			// pRef is the reference point for forming triangles.
			// It's location doesn't change the result (except for rounding error).
			Vec2 pRef = new Vec2(0f);
#if O
			// This code would put the reference point inside the polygon.
			for (int i = 0; i < count; ++i)
			{
				pRef += vs[i];
			}
			pRef *= 1.0f / count;
#endif

			const float inv3 = 1.0f / 3.0f;

			for (int i = 0; i < count; ++i)
			{
				// Triangle vertices.
				Vec2 p1 = pRef;
				Vec2 p2 = vs[i];
				Vec2 p3 = i + 1 < count ? vs[i + 1] : vs[0];

				Vec2 e1 = p2 - p1;
				Vec2 e2 = p3 - p1;

				float D = Vec2.Cross(e1, e2);

				float triangleArea = 0.5f * D;
				area += triangleArea;

				// Area weighted centroid
				c += triangleArea * inv3 * (p1 + p2 + p3);
			}

			// Centroid
			Box2DXDebug.Assert(area > Common.Settings.FLT_EPSILON);
			c *= 1.0f / area;
			return c;
		}

		public PolygonShape()
		{
			_type = ShapeType.PolygonShape; 
			_radius = Settings.PolygonRadius;

			/*Box2DXDebug.Assert(def.Type == ShapeType.PolygonShape);
			_type = ShapeType.PolygonShape;
			PolygonDef poly = (PolygonDef)def;

			// Get the vertices transformed into the body frame.
			_vertexCount = poly.VertexCount;
			Box2DXDebug.Assert(3 <= _vertexCount && _vertexCount <= Settings.MaxPolygonVertices);

			// Copy vertices.
			for (int i = 0; i < _vertexCount; ++i)
			{
				_vertices[i] = poly.Vertices[i];
			}

			// Compute normals. Ensure the edges have non-zero length.
			for (int i = 0; i < _vertexCount; ++i)
			{
				int i1 = i;
				int i2 = i + 1 < _vertexCount ? i + 1 : 0;
				Vec2 edge = _vertices[i2] - _vertices[i1];
				Box2DXDebug.Assert(edge.LengthSquared() > Common.Settings.FLT_EPSILON * Common.Settings.FLT_EPSILON);
				_normals[i] = Vec2.Cross(edge, 1.0f);
				_normals[i].Normalize();
			}

#if DEBUG
			// Ensure the polygon is convex.
			for (int i = 0; i < _vertexCount; ++i)
			{
				for (int j = 0; j < _vertexCount; ++j)
				{
					// Don't check vertices on the current edge.
					if (j == i || j == (i + 1) % _vertexCount)
					{
						continue;
					}

					// Your polygon is non-convex (it has an indentation).
					// Or your polygon is too skinny.
					float s = Vec2.Dot(_normals[i], _vertices[j] - _vertices[i]);
					Box2DXDebug.Assert(s < -Settings.LinearSlop);
				}
			}

			// Ensure the polygon is counter-clockwise.
			for (int i = 1; i < _vertexCount; ++i)
			{
				float cross = Vec2.Cross(_normals[i - 1], _normals[i]);

				// Keep asinf happy.
				cross = Common.Math.Clamp(cross, -1.0f, 1.0f);

				// You have consecutive edges that are almost parallel on your polygon.
				float angle = (float)System.Math.Asin(cross);
				Box2DXDebug.Assert(angle > Settings.AngularSlop);
			}
#endif

			// Compute the polygon centroid.
			_centroid = ComputeCentroid(poly.Vertices, poly.VertexCount);

			// Compute the oriented bounding box.
			ComputeOBB(out _obb, _vertices, _vertexCount);

			// Create core polygon shape by shifting edges inward.
			// Also compute the min/max radius for CCD.
			for (int i = 0; i < _vertexCount; ++i)
			{
				int i1 = i - 1 >= 0 ? i - 1 : _vertexCount - 1;
				int i2 = i;

				Vec2 n1 = _normals[i1];
				Vec2 n2 = _normals[i2];
				Vec2 v = _vertices[i] - _centroid; ;

				Vec2 d = new Vec2();
				d.X = Vec2.Dot(n1, v) - Settings.ToiSlop;
				d.Y = Vec2.Dot(n2, v) - Settings.ToiSlop;

				// Shifting the edge inward by b2_toiSlop should
				// not cause the plane to pass the centroid.

				// Your shape has a radius/extent less than b2_toiSlop.
				Box2DXDebug.Assert(d.X >= 0.0f);
				Box2DXDebug.Assert(d.Y >= 0.0f);
				Mat22 A = new Mat22();
				A.Col1.X = n1.X; A.Col2.X = n1.Y;
				A.Col1.Y = n2.X; A.Col2.Y = n2.Y;
				_coreVertices[i] = A.Solve(d) + _centroid;
			}*/
		}

		/*// http://www.geometrictools.com/Documentation/MinimumAreaRectangle.pdf
		public static void ComputeOBB(out OBB obb, Vec2[] vs, int count)
		{
			obb = new OBB();

			Box2DXDebug.Assert(count <= Settings.MaxPolygonVertices);
			Vec2[] p = new Vec2[Settings.MaxPolygonVertices + 1];
			for (int i = 0; i < count; ++i)
			{
				p[i] = vs[i];
			}
			p[count] = p[0];

			float minArea = Common.Settings.FLT_MAX;

			for (int i = 1; i <= count; ++i)
			{
				Vec2 root = p[i - 1];
				Vec2 ux = p[i] - root;
				float length = ux.Normalize();
				Box2DXDebug.Assert(length > Common.Settings.FLT_EPSILON);
				Vec2 uy = new Vec2(-ux.Y, ux.X);
				Vec2 lower = new Vec2(Common.Settings.FLT_MAX, Common.Settings.FLT_MAX);
				Vec2 upper = new Vec2(-Common.Settings.FLT_MAX, -Common.Settings.FLT_MAX);

				for (int j = 0; j < count; ++j)
				{
					Vec2 d = p[j] - root;
					Vec2 r = new Vec2();
					r.X = Vec2.Dot(ux, d);
					r.Y = Vec2.Dot(uy, d);
					lower = Common.Math.Min(lower, r);
					upper = Common.Math.Max(upper, r);
				}

				float area = (upper.X - lower.X) * (upper.Y - lower.Y);
				if (area < 0.95f * minArea)
				{
					minArea = area;
					obb.R.Col1 = ux;
					obb.R.Col2 = uy;
					Vec2 center = 0.5f * (lower + upper);
					obb.Center = root + Common.Math.Mul(obb.R, center);
					obb.Extents = 0.5f * (upper - lower);
				}
			}

			Box2DXDebug.Assert(minArea < Common.Settings.FLT_MAX);
		}*/
	}
}
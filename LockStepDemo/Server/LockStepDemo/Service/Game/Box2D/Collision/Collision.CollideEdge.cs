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

using Box2DX.Common;

namespace Box2DX.Collision
{
	public partial class Collision
	{
		// This implements 2-sided edge vs circle collision.
		public static void CollideEdgeAndCircle(ref Manifold manifold, EdgeShape edge, XForm transformA, CircleShape circle, XForm transformB)
		{
			manifold.PointCount = 0;
			Vec2 cLocal = Common.Math.MulT(transformA, Common.Math.Mul(transformB, circle._position));
			Vec2 normal = edge._normal;
			Vec2 v1 = edge._v1;
			Vec2 v2 = edge._v2;
			float radius = edge._radius + circle._radius;

			// Barycentric coordinates
			float u1 = Vec2.Dot(cLocal - v1, v2 - v1);
			float u2 = Vec2.Dot(cLocal - v2, v1 - v2);

			if (u1 <= 0.0f)
			{
				// Behind v1
				if (Vec2.DistanceSquared(cLocal, v1) > radius * radius)
				{
					return;
				}

				manifold.PointCount = 1;
				manifold.Type = ManifoldType.FaceA;
				manifold.LocalPlaneNormal = cLocal - v1;
				manifold.LocalPlaneNormal.Normalize();
				manifold.LocalPoint = v1;
				manifold.Points[0].LocalPoint = circle._position;
				manifold.Points[0].ID.Key = 0;
			}
			else if (u2 <= 0.0f)
			{
				// Ahead of v2
				if (Vec2.DistanceSquared(cLocal, v2) > radius * radius)
				{
					return;
				}

				manifold.PointCount = 1;
				manifold.Type = ManifoldType.FaceA;
				manifold.LocalPlaneNormal = cLocal - v2;
				manifold.LocalPlaneNormal.Normalize();
				manifold.LocalPoint = v2;
				manifold.Points[0].LocalPoint = circle._position;
				manifold.Points[0].ID.Key = 0;
			}
			else
			{
				float separation = Vec2.Dot(cLocal - v1, normal);
				if (separation < -radius || radius < separation)
				{
					return;
				}
				
				manifold.PointCount = 1;
				manifold.Type = ManifoldType.FaceA;
				manifold.LocalPlaneNormal = separation < 0.0f ? -normal : normal;
				manifold.LocalPoint = 0.5f * (v1 + v2);
				manifold.Points[0].LocalPoint = circle._position;
				manifold.Points[0].ID.Key = 0;
			}
		}

		// Polygon versus 2-sided edge.
		public static void CollidePolyAndEdge(ref Manifold manifold, PolygonShape polygon, XForm transformA, EdgeShape edge, XForm transformB)
		{
			PolygonShape polygonB = new PolygonShape();
			polygonB.SetAsEdge(edge._v1, edge._v2);

			CollidePolygons(ref manifold, polygon, transformA, polygonB, transformB);
		}
	}
}
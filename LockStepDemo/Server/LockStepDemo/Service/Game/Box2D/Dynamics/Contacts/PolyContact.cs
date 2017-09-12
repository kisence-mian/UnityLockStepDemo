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

using Box2DX.Collision;
using Box2DX.Common;

namespace Box2DX.Dynamics
{
	public class PolygonContact : Contact
	{
		public PolygonContact(Fixture fixtureA, Fixture fixtureB)
			: base(fixtureA, fixtureB)
		{
			Box2DXDebug.Assert(fixtureA.ShapeType == ShapeType.PolygonShape);
			Box2DXDebug.Assert(fixtureB.ShapeType == ShapeType.PolygonShape);
			CollideShapeFunction = CollidePolygons;
		}

		private static void CollidePolygons(ref Manifold manifold, Shape shape1, XForm xf1, Shape shape2, XForm xf2)
		{
			Collision.Collision.CollidePolygons(ref manifold, (PolygonShape)shape1, xf1, (PolygonShape)shape2, xf2);
		}

		new public static Contact Create(Fixture fixtureA, Fixture fixtureB)
		{
			return new PolygonContact(fixtureA, fixtureB);
		}

		new public static void Destroy(ref Contact contact)
		{
			contact = null;
		}
	}
}
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

//r175

using System;
using System.Collections.Generic;
using System.Text;

namespace Box2DX.Common
{
	/// <summary>
	/// A 2D column vector with 3 elements.
	/// </summary>
	public struct Vec3
	{
		/// <summary>
		/// Construct using coordinates.
		/// </summary>
		public Vec3(float x, float y, float z) { X = x; Y = y; Z = z; }

		/// <summary>
		/// Set this vector to all zeros.
		/// </summary>
		public void SetZero() { X = 0.0f; Y = 0.0f; Z = 0.0f; }

		/// <summary>
		/// Set this vector to some specified coordinates.
		/// </summary>
		public void Set(float x, float y, float z) { X = x; Y = y; Z = z; }

		/// <summary>
		/// Perform the dot product on two vectors.
		/// </summary>
		public static float Dot(Vec3 a, Vec3 b)
		{
			return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
		}

		/// <summary>
		/// Perform the cross product on two vectors.
		/// </summary>
		public static Vec3 Cross(Vec3 a, Vec3 b)
		{
			return new Vec3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
		}

		/// <summary>
		/// Negate this vector.
		/// </summary>
		public static Vec3 operator -(Vec3 v)
		{
			return new Vec3(-v.X, -v.Y, -v.Z);
		}

		/// <summary>
		/// Add two vectors component-wise.
		/// </summary>
		public static Vec3 operator +(Vec3 v1, Vec3 v2)
		{
			return new Vec3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
		}

		/// <summary>
		/// Subtract two vectors component-wise.
		/// </summary>
		public static Vec3 operator -(Vec3 v1, Vec3 v2)
		{
			return new Vec3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
		}

		/// <summary>
		/// Multiply this vector by a scalar.
		/// </summary>
		public static Vec3 operator *(Vec3 v, float s)
		{
			return new Vec3(v.X * s, v.Y * s, v.Z * s);
		}

		/// <summary>
		/// Multiply this vector by a scalar.
		/// </summary>
		public static Vec3 operator *(float s, Vec3 v)
		{
			return new Vec3(v.X * s, v.Y * s, v.Z * s);
		}

		public float X, Y, Z;
	}
}
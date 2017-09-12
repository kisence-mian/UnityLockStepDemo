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

namespace Box2DX.Common
{
	/// <summary>
	/// A 2D column vector.
	/// </summary>
	public struct Vec2
	{
		public float X, Y;

		public float this[int i]
		{
			get
			{
				if (i == 0) return X;
				else if (i == 1) return Y;
				else
				{
					Box2DXDebug.Assert(false, "Incorrect Vec2 element!");
					return 0;
				}
			}
			set
			{
				if (i == 0) X = value;
				else if (i == 1) Y = value;
				else
				{
					Box2DXDebug.Assert(false, "Incorrect Vec2 element!");					
				}
			}
		}

		/// <summary>
		/// Construct using coordinates.
		/// </summary>
		public Vec2(float x)
		{
			X = x;
			Y = x;
		}

		/// <summary>
		/// Construct using coordinates.
		/// </summary>
		public Vec2(float x, float y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Set this vector to all zeros.
		/// </summary>
		public void SetZero() { X = 0.0f; Y = 0.0f; }

		/// <summary>
		/// Set this vector to some specified coordinates.
		/// </summary>
		public void Set(float x, float y) { X = x; Y = y; }

		public void Set(float xy) { X = xy; Y = xy; }

		/// <summary>
		///  Get the length of this vector (the norm).
		/// </summary>
		public float Length()
		{
			return (float)System.Math.Sqrt(X * X + Y * Y);
		}

		/// <summary>
		/// Get the length squared. For performance, use this instead of
		/// Length (if possible).
		/// </summary>
		public float LengthSquared()
		{
			return X * X + Y * Y;
		}

		/// <summary>
		/// Convert this vector into a unit vector. Returns the length.
		/// </summary>
		public float Normalize()
		{
			float length = Length();
			if (length < Settings.FLT_EPSILON)
			{
				return 0.0f;
			}
			float invLength = 1.0f / length;
			X *= invLength;
			Y *= invLength;

			return length;
		}

		/// <summary>
		/// Does this vector contain finite coordinates?
		/// </summary>
		public bool IsValid
		{
			get { return Math.IsValid(X) && Math.IsValid(Y); }
		}

		/// <summary>
		/// Negate this vector.
		/// </summary>
		public static Vec2 operator -(Vec2 v1)
		{
			Vec2 v = new Vec2();
			v.Set(-v1.X, -v1.Y);
			return v;
		}

		public static Vec2 operator +(Vec2 v1, Vec2 v2)
		{
			Vec2 v = new Vec2();
			v.Set(v1.X + v2.X, v1.Y + v2.Y);
			return v;
		}

		public static Vec2 operator -(Vec2 v1, Vec2 v2)
		{
			Vec2 v = new Vec2();
			v.Set(v1.X - v2.X, v1.Y - v2.Y);
			return v;
		}

		public static Vec2 operator *(Vec2 v1, float a)
		{
			Vec2 v = new Vec2();
			v.Set(v1.X * a, v1.Y * a);
			return v;
		}

		public static Vec2 operator *(float a, Vec2 v1)
		{
			Vec2 v = new Vec2();
			v.Set(v1.X * a, v1.Y * a);
			return v;
		}

		public static bool operator ==(Vec2 a, Vec2 b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		public static bool operator !=(Vec2 a, Vec2 b)
		{
			return a.X != b.X || a.Y != b.Y;
		}

		public static Vec2 Zero { get { return new Vec2(0, 0); } }

		/// <summary>
		/// Peform the dot product on two vectors.
		/// </summary>
		public static float Dot(Vec2 a, Vec2 b)
		{
			return a.X * b.X + a.Y * b.Y;
		}

		/// <summary>
		/// Perform the cross product on two vectors. In 2D this produces a scalar.
		/// </summary>
		public static float Cross(Vec2 a, Vec2 b)
		{
			return a.X * b.Y - a.Y * b.X;
		}

		/// <summary>
		/// Perform the cross product on a vector and a scalar. 
		/// In 2D this produces a vector.
		/// </summary>
		public static Vec2 Cross(Vec2 a, float s)
		{
			Vec2 v = new Vec2();
			v.Set(s * a.Y, -s * a.X);
			return v;
		}

		/// <summary>
		/// Perform the cross product on a scalar and a vector. 
		/// In 2D this produces a vector.
		/// </summary>
		public static Vec2 Cross(float s, Vec2 a)
		{
			Vec2 v = new Vec2();
			v.Set(-s * a.Y, s * a.X);
			return v;
		}

		public static float Distance(Vec2 a, Vec2 b)
		{
			Vec2 c = a - b;
			return c.Length();
		}

		public static float DistanceSquared(Vec2 a, Vec2 b)
		{
			Vec2 c = a - b;
			return Vec2.Dot(c, c);
		}
	}
}

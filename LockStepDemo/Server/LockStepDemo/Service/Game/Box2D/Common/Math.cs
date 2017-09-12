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
	public class Math
	{
		public static readonly ushort USHRT_MAX = 0xffff;
		public static readonly byte UCHAR_MAX = 0xff;
		public static readonly int RAND_LIMIT = 32767;

		/// <summary>
		/// This function is used to ensure that a floating point number is
		/// not a NaN or infinity.
		/// </summary>
		public static bool IsValid(float x)
		{
			return !(float.IsNaN(x) || float.IsNegativeInfinity(x) || float.IsPositiveInfinity(x));
		}

		[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
		public struct Convert
		{
			[System.Runtime.InteropServices.FieldOffset(0)]
			public float x;

			[System.Runtime.InteropServices.FieldOffset(0)]
			public int i;
		}

		/// <summary>
		/// This is a approximate yet fast inverse square-root.
		/// </summary>
		public static float InvSqrt(float x)
		{
			Convert convert = new Convert();
			convert.x = x;
			float xhalf = 0.5f * x;
			convert.i = 0x5f3759df - (convert.i >> 1);
			x = convert.x;
			x = x * (1.5f - xhalf * x * x);
			return x;
		}

		public static float Sqrt(float x)
		{
			return (float)System.Math.Sqrt(x);
		}

		private static Random s_rnd = new Random();
		/// <summary>
		/// Random number in range [-1,1]
		/// </summary>
		public static float Random()
		{
			float r = (float)(s_rnd.Next() & RAND_LIMIT);
			r /= RAND_LIMIT;
			r = 2.0f * r - 1.0f;
			return r;
		}

		/// <summary>
		/// Random floating point number in range [lo, hi]
		/// </summary>
		public static float Random(float lo, float hi)
		{
			float r = (float)(s_rnd.Next() & RAND_LIMIT);
			r /= RAND_LIMIT;
			r = (hi - lo) * r + lo;
			return r;		
		}

		/// <summary>
		/// "Next Largest Power of 2
		/// Given a binary integer value x, the next largest power of 2 can be computed by a SWAR algorithm
		/// that recursively "folds" the upper bits into the lower bits. This process yields a bit vector with
		/// the same most significant 1 as x, but all 1's below it. Adding 1 to that value yields the next
		/// largest power of 2. For a 32-bit value:"
		/// </summary>
		public static uint NextPowerOfTwo(uint x)
		{
			x |= (x >> 1);
			x |= (x >> 2);
			x |= (x >> 4);
			x |= (x >> 8);
			x |= (x >> 16);
			return x + 1;
		}

		public static bool IsPowerOfTwo(uint x)
		{
			bool result = x > 0 && (x & (x - 1)) == 0;
			return result;
		}

		public static float Abs(float a)
		{
			return a > 0.0f ? a : -a;
		}

		public static Vec2 Abs(Vec2 a)
		{
			Vec2 b = new Vec2();
			b.Set(Math.Abs(a.X), Math.Abs(a.Y));
			return b;
		}

		public static Mat22 Abs(Mat22 A)
		{
			Mat22 B = new Mat22();
			B.Set(Math.Abs(A.Col1), Math.Abs(A.Col2));
			return B;
		}

		public static float Min(float a, float b)
		{
			return a < b ? a : b;
		}

		public static int Min(int a, int b)
		{
			return a < b ? a : b;
		}

		public static Vec2 Min(Vec2 a, Vec2 b)
		{
			Vec2 c = new Vec2();
			c.X = Math.Min(a.X, b.X);
			c.Y = Math.Min(a.Y, b.Y);
			return c;
		}

		public static float Max(float a, float b)
		{
			return a > b ? a : b;
		}

		public static int Max(int a, int b)
		{
			return a > b ? a : b;
		}

		public static Vec2 Max(Vec2 a, Vec2 b)
		{
			Vec2 c = new Vec2();
			c.X = Math.Max(a.X, b.X);
			c.Y = Math.Max(a.Y, b.Y);
			return c;
		}

		public static float Clamp(float a, float low, float high)
		{
			return Math.Max(low, Math.Min(a, high));
		}

		public static int Clamp(int a, int low, int high)
		{
			return Math.Max(low, Math.Min(a, high));
		}

		public static Vec2 Clamp(Vec2 a, Vec2 low, Vec2 high)
		{
			return Math.Max(low, Math.Min(a, high));
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			T tmp = a;
			a = b;
			b = tmp;
		}

		/// <summary>
		/// Multiply a matrix times a vector. If a rotation matrix is provided,
		/// then this transforms the vector from one frame to another.
		/// </summary>
		public static Vec2 Mul(Mat22 A, Vec2 v)
		{
			Vec2 u = new Vec2();
			u.Set(A.Col1.X * v.X + A.Col2.X * v.Y, A.Col1.Y * v.X + A.Col2.Y * v.Y);
			return u;
		}

		/// <summary>
		/// Multiply a matrix transpose times a vector. If a rotation matrix is provided,
		/// then this transforms the vector from one frame to another (inverse transform).
		/// </summary>
		public static Vec2 MulT(Mat22 A, Vec2 v)
		{
			Vec2 u = new Vec2();
			u.Set(Vec2.Dot(v, A.Col1), Vec2.Dot(v, A.Col2));
			return u;
		}

		/// <summary>
		/// A * B
		/// </summary>
		public static Mat22 Mul(Mat22 A, Mat22 B)
		{
			Mat22 C = new Mat22();
			C.Set(Math.Mul(A, B.Col1), Math.Mul(A, B.Col2));
			return C;
		}

		/// <summary>
		/// A^T * B
		/// </summary>
		public static Mat22 MulT(Mat22 A, Mat22 B)
		{
			Vec2 c1 = new Vec2();
			c1.Set(Vec2.Dot(A.Col1, B.Col1), Vec2.Dot(A.Col2, B.Col1));
			Vec2 c2 = new Vec2();
			c2.Set(Vec2.Dot(A.Col1, B.Col2), Vec2.Dot(A.Col2, B.Col2));
			Mat22 C = new Mat22();
			C.Set(c1, c2);
			return C;
		}

		public static Vec2 Mul(XForm T, Vec2 v)
		{
			return T.Position + Math.Mul(T.R, v);
		}

		public static Vec2 MulT(XForm T, Vec2 v)
		{
			return Math.MulT(T.R, v - T.Position);
		}

		/// <summary>
		/// Multiply a matrix times a vector.
		/// </summary>
		public static Vec3 Mul(Mat33 A, Vec3 v)
		{
			Vec3 u = v.X * A.Col1 + v.Y * A.Col2 + v.Z * A.Col3;
			return u;
		}

		public static float Atan2(float y, float x)
		{
			return (float)System.Math.Atan2(y, x);
		}
	}
}

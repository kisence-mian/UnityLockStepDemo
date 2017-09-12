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
	/// A 2-by-2 matrix. Stored in column-major order.
	/// </summary>
	public struct Mat22
	{
		public Vec2 Col1, Col2;

		/// <summary>
		/// Construct this matrix using columns.
		/// </summary>
		public Mat22(Vec2 c1, Vec2 c2)
		{
			Col1 = c1;
			Col2 = c2;
		}

		/// <summary>
		/// Construct this matrix using scalars.
		/// </summary>
		public Mat22(float a11, float a12, float a21, float a22)
		{
			Col1.X = a11; Col1.Y = a21;
			Col2.X = a12; Col2.Y = a22;
		}

		/// <summary>
		/// Construct this matrix using an angle. 
		/// This matrix becomes an orthonormal rotation matrix.
		/// </summary>
		public Mat22(float angle)
		{
			float c = (float)System.Math.Cos(angle), s = (float)System.Math.Sin(angle);
			Col1.X = c; Col2.X = -s;
			Col1.Y = s; Col2.Y = c;
		}

		/// <summary>
		/// Initialize this matrix using columns.
		/// </summary>
		public void Set(Vec2 c1, Vec2 c2)
		{
			Col1 = c1;
			Col2 = c2;
		}

		/// <summary>
		/// Initialize this matrix using an angle.
		/// This matrix becomes an orthonormal rotation matrix.
		/// </summary>
		public void Set(float angle)
		{
			float c = (float)System.Math.Cos(angle), s = (float)System.Math.Sin(angle);
			Col1.X = c; Col2.X = -s;
			Col1.Y = s; Col2.Y = c;
		}

		/// <summary>
		/// Set this to the identity matrix.
		/// </summary>
		public void SetIdentity()
		{
			Col1.X = 1.0f; Col2.X = 0.0f;
			Col1.Y = 0.0f; Col2.Y = 1.0f;
		}

		/// <summary>
		/// Set this matrix to all zeros.
		/// </summary>
		public void SetZero()
		{
			Col1.X = 0.0f; Col2.X = 0.0f;
			Col1.Y = 0.0f; Col2.Y = 0.0f;
		}

		/// <summary>
		/// Extract the angle from this matrix (assumed to be a rotation matrix).
		/// </summary>
		public float GetAngle()
		{
			return (float)System.Math.Atan2(Col1.Y, Col1.X);
		}

		/// <summary>
		/// Compute the inverse of this matrix, such that inv(A) * A = identity.
		/// </summary>
		public Mat22 GetInverse()
		{
			float a = Col1.X, b = Col2.X, c = Col1.Y, d = Col2.Y;
			Mat22 B = new Mat22();
			float det = a * d - b * c;
			Box2DXDebug.Assert(det != 0.0f);
			det = 1.0f / det;
			B.Col1.X = det * d; B.Col2.X = -det * b;
			B.Col1.Y = -det * c; B.Col2.Y = det * a;
			return B;
		}

		/// <summary>
		/// Solve A * x = b, where b is a column vector. This is more efficient
		/// than computing the inverse in one-shot cases.
		/// </summary>
		public Vec2 Solve(Vec2 b)
		{
			float a11 = Col1.X, a12 = Col2.X, a21 = Col1.Y, a22 = Col2.Y;
			float det = a11 * a22 - a12 * a21;
			Box2DXDebug.Assert(det != 0.0f);
			det = 1.0f / det;
			Vec2 x = new Vec2();
			x.X = det * (a22 * b.X - a12 * b.Y);
			x.Y = det * (a11 * b.Y - a21 * b.X);
			return x;
		}

		public static Mat22 Identity { get { return new Mat22(1, 0, 0, 1); } }

		public static Mat22 operator +(Mat22 A, Mat22 B)
		{
			Mat22 C = new Mat22();
			C.Set(A.Col1 + B.Col1, A.Col2 + B.Col2);
			return C;
		}
	}
}

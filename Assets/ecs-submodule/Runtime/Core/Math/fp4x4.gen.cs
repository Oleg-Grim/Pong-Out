//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Runtime.CompilerServices;
using static Unity.Mathematics.math;
using Unity.Mathematics;

#pragma warning disable 0660, 0661

namespace ME.ECS
{
    [System.Serializable]
    public partial struct fp4x4 : System.IEquatable<fp4x4>, IFormattable
    {
        public fp4 c0;
        public fp4 c1;
        public fp4 c2;
        public fp4 c3;

        /// <summary>fp4x4 identity transform.</summary>
        public static readonly fp4x4 identity = new fp4x4((fp)1, (fp)0, (fp)0, (fp)0,   (fp)0, (fp)1, (fp)0, (fp)0,   (fp)0, (fp)0, (fp)1, (fp)0,   (fp)0, (fp)0, (fp)0, (fp)1);

        /// <summary>fp4x4 zero value.</summary>
        public static readonly fp4x4 zero;

        /// <summary>Constructs a fp4x4 matrix from four fp4 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp4x4(fp4 c0, fp4 c1, fp4 c2, fp4 c3)
        { 
            this.c0 = c0;
            this.c1 = c1;
            this.c2 = c2;
            this.c3 = c3;
        }

        /// <summary>Constructs a fp4x4 matrix from 16 fp values given in row-major order.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp4x4(fp m00, fp m01, fp m02, fp m03,
                     fp m10, fp m11, fp m12, fp m13,
                     fp m20, fp m21, fp m22, fp m23,
                     fp m30, fp m31, fp m32, fp m33)
        { 
            this.c0 = new fp4(m00, m10, m20, m30);
            this.c1 = new fp4(m01, m11, m21, m31);
            this.c2 = new fp4(m02, m12, m22, m32);
            this.c3 = new fp4(m03, m13, m23, m33);
        }

        /// <summary>Constructs a fp4x4 matrix from a single fp value by assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp4x4(fp v)
        {
            this.c0 = v;
            this.c1 = v;
            this.c2 = v;
            this.c3 = v;
        }

        /// <summary>Constructs a fp4x4 matrix from a single int value by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp4x4(int v)
        {
            this.c0 = (fp4)v;
            this.c1 = (fp4)v;
            this.c2 = (fp4)v;
            this.c3 = (fp4)v;
        }

        /// <summary>Constructs a fp4x4 matrix from a int4x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp4x4(int4x4 v)
        {
            this.c0 = (fp4)v.c0;
            this.c1 = (fp4)v.c1;
            this.c2 = (fp4)v.c2;
            this.c3 = (fp4)v.c3;
        }

        /// <summary>Constructs a fp4x4 matrix from a single uint value by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp4x4(uint v)
        {
            this.c0 = (fp4)v;
            this.c1 = (fp4)v;
            this.c2 = (fp4)v;
            this.c3 = (fp4)v;
        }

        /// <summary>Constructs a fp4x4 matrix from a uint4x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp4x4(uint4x4 v)
        {
            this.c0 = (fp4)v.c0;
            this.c1 = (fp4)v.c1;
            this.c2 = (fp4)v.c2;
            this.c3 = (fp4)v.c3;
        }


        /// <summary>Implicitly converts a single fp value to a fp4x4 matrix by assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp4x4(fp v) { return new fp4x4(v); }

        /// <summary>Explicitly converts a single int value to a fp4x4 matrix by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp4x4(int v) { return new fp4x4(v); }

        /// <summary>Explicitly converts a int4x4 matrix to a fp4x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp4x4(int4x4 v) { return new fp4x4(v); }

        /// <summary>Explicitly converts a single uint value to a fp4x4 matrix by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp4x4(uint v) { return new fp4x4(v); }

        /// <summary>Explicitly converts a uint4x4 matrix to a fp4x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp4x4(uint4x4 v) { return new fp4x4(v); }


        /// <summary>Returns the result of a componentwise multiplication operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator * (fp4x4 lhs, fp4x4 rhs) { return new fp4x4 (lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3); }

        /// <summary>Returns the result of a componentwise multiplication operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator * (fp4x4 lhs, fp rhs) { return new fp4x4 (lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs); }

        /// <summary>Returns the result of a componentwise multiplication operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator * (fp lhs, fp4x4 rhs) { return new fp4x4 (lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3); }


        /// <summary>Returns the result of a componentwise addition operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator + (fp4x4 lhs, fp4x4 rhs) { return new fp4x4 (lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3); }

        /// <summary>Returns the result of a componentwise addition operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator + (fp4x4 lhs, fp rhs) { return new fp4x4 (lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs); }

        /// <summary>Returns the result of a componentwise addition operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator + (fp lhs, fp4x4 rhs) { return new fp4x4 (lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3); }


        /// <summary>Returns the result of a componentwise subtraction operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator - (fp4x4 lhs, fp4x4 rhs) { return new fp4x4 (lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3); }

        /// <summary>Returns the result of a componentwise subtraction operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator - (fp4x4 lhs, fp rhs) { return new fp4x4 (lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs); }

        /// <summary>Returns the result of a componentwise subtraction operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator - (fp lhs, fp4x4 rhs) { return new fp4x4 (lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3); }


        /// <summary>Returns the result of a componentwise division operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator / (fp4x4 lhs, fp4x4 rhs) { return new fp4x4 (lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3); }

        /// <summary>Returns the result of a componentwise division operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator / (fp4x4 lhs, fp rhs) { return new fp4x4 (lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs); }

        /// <summary>Returns the result of a componentwise division operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator / (fp lhs, fp4x4 rhs) { return new fp4x4 (lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3); }


        /// <summary>Returns the result of a componentwise modulus operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator % (fp4x4 lhs, fp4x4 rhs) { return new fp4x4 (lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3); }

        /// <summary>Returns the result of a componentwise modulus operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator % (fp4x4 lhs, fp rhs) { return new fp4x4 (lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs); }

        /// <summary>Returns the result of a componentwise modulus operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator % (fp lhs, fp4x4 rhs) { return new fp4x4 (lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3); }


        /// <summary>Returns the result of a componentwise increment operation on a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator ++ (fp4x4 val) { return new fp4x4 (++val.c0, ++val.c1, ++val.c2, ++val.c3); }


        /// <summary>Returns the result of a componentwise decrement operation on a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator -- (fp4x4 val) { return new fp4x4 (--val.c0, --val.c1, --val.c2, --val.c3); }


        /// <summary>Returns the result of a componentwise less than operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator < (fp4x4 lhs, fp4x4 rhs) { return new bool4x4 (lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3); }

        /// <summary>Returns the result of a componentwise less than operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator < (fp4x4 lhs, fp rhs) { return new bool4x4 (lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs); }

        /// <summary>Returns the result of a componentwise less than operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator < (fp lhs, fp4x4 rhs) { return new bool4x4 (lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3); }


        /// <summary>Returns the result of a componentwise less or equal operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator <= (fp4x4 lhs, fp4x4 rhs) { return new bool4x4 (lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3); }

        /// <summary>Returns the result of a componentwise less or equal operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator <= (fp4x4 lhs, fp rhs) { return new bool4x4 (lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs); }

        /// <summary>Returns the result of a componentwise less or equal operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator <= (fp lhs, fp4x4 rhs) { return new bool4x4 (lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3); }


        /// <summary>Returns the result of a componentwise greater than operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator > (fp4x4 lhs, fp4x4 rhs) { return new bool4x4 (lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3); }

        /// <summary>Returns the result of a componentwise greater than operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator > (fp4x4 lhs, fp rhs) { return new bool4x4 (lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs); }

        /// <summary>Returns the result of a componentwise greater than operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator > (fp lhs, fp4x4 rhs) { return new bool4x4 (lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3); }


        /// <summary>Returns the result of a componentwise greater or equal operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator >= (fp4x4 lhs, fp4x4 rhs) { return new bool4x4 (lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3); }

        /// <summary>Returns the result of a componentwise greater or equal operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator >= (fp4x4 lhs, fp rhs) { return new bool4x4 (lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs); }

        /// <summary>Returns the result of a componentwise greater or equal operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator >= (fp lhs, fp4x4 rhs) { return new bool4x4 (lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3); }


        /// <summary>Returns the result of a componentwise unary minus operation on a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator - (fp4x4 val) { return new fp4x4 (-val.c0, -val.c1, -val.c2, -val.c3); }


        /// <summary>Returns the result of a componentwise unary plus operation on a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 operator + (fp4x4 val) { return new fp4x4 (+val.c0, +val.c1, +val.c2, +val.c3); }


        /// <summary>Returns the result of a componentwise equality operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator == (fp4x4 lhs, fp4x4 rhs) { return new bool4x4 (lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3); }

        /// <summary>Returns the result of a componentwise equality operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator == (fp4x4 lhs, fp rhs) { return new bool4x4 (lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs); }

        /// <summary>Returns the result of a componentwise equality operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator == (fp lhs, fp4x4 rhs) { return new bool4x4 (lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3); }


        /// <summary>Returns the result of a componentwise not equal operation on two fp4x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator != (fp4x4 lhs, fp4x4 rhs) { return new bool4x4 (lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3); }

        /// <summary>Returns the result of a componentwise not equal operation on a fp4x4 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator != (fp4x4 lhs, fp rhs) { return new bool4x4 (lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs); }

        /// <summary>Returns the result of a componentwise not equal operation on a fp value and a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool4x4 operator != (fp lhs, fp4x4 rhs) { return new bool4x4 (lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3); }



        /// <summary>Returns the fp4 element at a specified index.</summary>
        unsafe public ref fp4 this[int index]
        {
            get
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if ((uint)index >= 4)
                    throw new System.ArgumentException("index must be between[0...3]");
#endif
                fixed (fp4x4* array = &this) { return ref ((fp4*)array)[index]; }
            }
        }

        /// <summary>Returns true if the fp4x4 is equal to a given fp4x4, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(fp4x4 rhs) { return c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2) && c3.Equals(rhs.c3); }

        /// <summary>Returns true if the fp4x4 is equal to a given fp4x4, false otherwise.</summary>
        public override bool Equals(object o) { return Equals((fp4x4)o); }


        /// <summary>Returns a hash code for the fp4x4.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() { return (int)fpmath.hash(this); }


        /// <summary>Returns a string representation of the fp4x4.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Format("fp4x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11},  {12}, {13}, {14}, {15})", c0.x, c1.x, c2.x, c3.x, c0.y, c1.y, c2.y, c3.y, c0.z, c1.z, c2.z, c3.z, c0.w, c1.w, c2.w, c3.w);
        }

        /// <summary>Returns a string representation of the fp4x4 using a specified format and culture-specific format information.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("fp4x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7},  {8}, {9}, {10}, {11},  {12}, {13}, {14}, {15})", c0.x.ToString(format, formatProvider), c1.x.ToString(format, formatProvider), c2.x.ToString(format, formatProvider), c3.x.ToString(format, formatProvider), c0.y.ToString(format, formatProvider), c1.y.ToString(format, formatProvider), c2.y.ToString(format, formatProvider), c3.y.ToString(format, formatProvider), c0.z.ToString(format, formatProvider), c1.z.ToString(format, formatProvider), c2.z.ToString(format, formatProvider), c3.z.ToString(format, formatProvider), c0.w.ToString(format, formatProvider), c1.w.ToString(format, formatProvider), c2.w.ToString(format, formatProvider), c3.w.ToString(format, formatProvider));
        }

    }

    public static partial class fpmath
    {
        /// <summary>Returns a fp4x4 matrix constructed from four fp4 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 fp4x4(fp4 c0, fp4 c1, fp4 c2, fp4 c3) { return new fp4x4(c0, c1, c2, c3); }

        /// <summary>Returns a fp4x4 matrix constructed from from 16 fp values given in row-major order.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 fp4x4(fp m00, fp m01, fp m02, fp m03,
                                  fp m10, fp m11, fp m12, fp m13,
                                  fp m20, fp m21, fp m22, fp m23,
                                  fp m30, fp m31, fp m32, fp m33)
        {
            return new fp4x4(m00, m01, m02, m03,
                             m10, m11, m12, m13,
                             m20, m21, m22, m23,
                             m30, m31, m32, m33);
        }

        /// <summary>Returns a fp4x4 matrix constructed from a single fp value by assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 fp4x4(fp v) { return new fp4x4(v); }

        /// <summary>Returns a fp4x4 matrix constructed from a single int value by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 fp4x4(int v) { return new fp4x4(v); }

        /// <summary>Return a fp4x4 matrix constructed from a int4x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 fp4x4(int4x4 v) { return new fp4x4(v); }

        /// <summary>Returns a fp4x4 matrix constructed from a single uint value by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 fp4x4(uint v) { return new fp4x4(v); }

        /// <summary>Return a fp4x4 matrix constructed from a uint4x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 fp4x4(uint4x4 v) { return new fp4x4(v); }

        /// <summary>Return the fp4x4 transpose of a fp4x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp4x4 transpose(fp4x4 v)
        {
            return fp4x4(
                v.c0.x, v.c0.y, v.c0.z, v.c0.w,
                v.c1.x, v.c1.y, v.c1.z, v.c1.w,
                v.c2.x, v.c2.y, v.c2.z, v.c2.w,
                v.c3.x, v.c3.y, v.c3.z, v.c3.w);
        }

        /// <summary>Returns a uint hash code of a fp4x4 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint hash(fp4x4 v)
        {
            return math.csum(fpmath.asuint(v.c0) * uint4(0x68EEE0F5u, 0xBC3B0A59u, 0x816EFB5Du, 0xA24E82B7u) + 
                        fpmath.asuint(v.c1) * uint4(0x45A22087u, 0xFC104C3Bu, 0x5FFF6B19u, 0x5E6CBF3Bu) + 
                        fpmath.asuint(v.c2) * uint4(0xB546F2A5u, 0xBBCF63E7u, 0xC53F4755u, 0x6985C229u) + 
                        fpmath.asuint(v.c3) * uint4(0xE133B0B3u, 0xC3E0A3B9u, 0xFE31134Fu, 0x712A34D7u)) + 0x9D77A59Bu;
        }

        /// <summary>
        /// Returns a uint4 vector hash code of a fp4x4 vector.
        /// When multiple elements are to be hashes together, it can more efficient to calculate and combine wide hash
        /// that are only reduced to a narrow uint hash at the very end instead of at every step.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint4 hashwide(fp4x4 v)
        {
            return (fpmath.asuint(v.c0) * uint4(0x4942CA39u, 0xB40EC62Du, 0x565ED63Fu, 0x93C30C2Bu) + 
                    fpmath.asuint(v.c1) * uint4(0xDCAF0351u, 0x6E050B01u, 0x750FDBF5u, 0x7F3DD499u) + 
                    fpmath.asuint(v.c2) * uint4(0x52EAAEBBu, 0x4599C793u, 0x83B5E729u, 0xC267163Fu) + 
                    fpmath.asuint(v.c3) * uint4(0x67BC9149u, 0xAD7C5EC1u, 0x822A7D6Du, 0xB492BF15u)) + 0xD37220E3u;
        }

    }
}

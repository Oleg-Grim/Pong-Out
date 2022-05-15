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
    public partial struct fp3x2 : System.IEquatable<fp3x2>, IFormattable
    {
        public fp3 c0;
        public fp3 c1;

        /// <summary>fp3x2 zero value.</summary>
        public static readonly fp3x2 zero;

        /// <summary>Constructs a fp3x2 matrix from two fp3 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp3x2(fp3 c0, fp3 c1)
        { 
            this.c0 = c0;
            this.c1 = c1;
        }

        /// <summary>Constructs a fp3x2 matrix from 6 fp values given in row-major order.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp3x2(fp m00, fp m01,
                     fp m10, fp m11,
                     fp m20, fp m21)
        { 
            this.c0 = new fp3(m00, m10, m20);
            this.c1 = new fp3(m01, m11, m21);
        }

        /// <summary>Constructs a fp3x2 matrix from a single fp value by assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp3x2(fp v)
        {
            this.c0 = v;
            this.c1 = v;
        }

        /// <summary>Constructs a fp3x2 matrix from a single int value by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp3x2(int v)
        {
            this.c0 = (fp3)v;
            this.c1 = (fp3)v;
        }

        /// <summary>Constructs a fp3x2 matrix from a int3x2 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp3x2(int3x2 v)
        {
            this.c0 = (fp3)v.c0;
            this.c1 = (fp3)v.c1;
        }

        /// <summary>Constructs a fp3x2 matrix from a single uint value by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp3x2(uint v)
        {
            this.c0 = (fp3)v;
            this.c1 = (fp3)v;
        }

        /// <summary>Constructs a fp3x2 matrix from a uint3x2 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public fp3x2(uint3x2 v)
        {
            this.c0 = (fp3)v.c0;
            this.c1 = (fp3)v.c1;
        }


        /// <summary>Implicitly converts a single fp value to a fp3x2 matrix by assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator fp3x2(fp v) { return new fp3x2(v); }

        /// <summary>Explicitly converts a single int value to a fp3x2 matrix by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp3x2(int v) { return new fp3x2(v); }

        /// <summary>Explicitly converts a int3x2 matrix to a fp3x2 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp3x2(int3x2 v) { return new fp3x2(v); }

        /// <summary>Explicitly converts a single uint value to a fp3x2 matrix by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp3x2(uint v) { return new fp3x2(v); }

        /// <summary>Explicitly converts a uint3x2 matrix to a fp3x2 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator fp3x2(uint3x2 v) { return new fp3x2(v); }


        /// <summary>Returns the result of a componentwise multiplication operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator * (fp3x2 lhs, fp3x2 rhs) { return new fp3x2 (lhs.c0 * rhs.c0, lhs.c1 * rhs.c1); }

        /// <summary>Returns the result of a componentwise multiplication operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator * (fp3x2 lhs, fp rhs) { return new fp3x2 (lhs.c0 * rhs, lhs.c1 * rhs); }

        /// <summary>Returns the result of a componentwise multiplication operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator * (fp lhs, fp3x2 rhs) { return new fp3x2 (lhs * rhs.c0, lhs * rhs.c1); }


        /// <summary>Returns the result of a componentwise addition operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator + (fp3x2 lhs, fp3x2 rhs) { return new fp3x2 (lhs.c0 + rhs.c0, lhs.c1 + rhs.c1); }

        /// <summary>Returns the result of a componentwise addition operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator + (fp3x2 lhs, fp rhs) { return new fp3x2 (lhs.c0 + rhs, lhs.c1 + rhs); }

        /// <summary>Returns the result of a componentwise addition operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator + (fp lhs, fp3x2 rhs) { return new fp3x2 (lhs + rhs.c0, lhs + rhs.c1); }


        /// <summary>Returns the result of a componentwise subtraction operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator - (fp3x2 lhs, fp3x2 rhs) { return new fp3x2 (lhs.c0 - rhs.c0, lhs.c1 - rhs.c1); }

        /// <summary>Returns the result of a componentwise subtraction operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator - (fp3x2 lhs, fp rhs) { return new fp3x2 (lhs.c0 - rhs, lhs.c1 - rhs); }

        /// <summary>Returns the result of a componentwise subtraction operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator - (fp lhs, fp3x2 rhs) { return new fp3x2 (lhs - rhs.c0, lhs - rhs.c1); }


        /// <summary>Returns the result of a componentwise division operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator / (fp3x2 lhs, fp3x2 rhs) { return new fp3x2 (lhs.c0 / rhs.c0, lhs.c1 / rhs.c1); }

        /// <summary>Returns the result of a componentwise division operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator / (fp3x2 lhs, fp rhs) { return new fp3x2 (lhs.c0 / rhs, lhs.c1 / rhs); }

        /// <summary>Returns the result of a componentwise division operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator / (fp lhs, fp3x2 rhs) { return new fp3x2 (lhs / rhs.c0, lhs / rhs.c1); }


        /// <summary>Returns the result of a componentwise modulus operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator % (fp3x2 lhs, fp3x2 rhs) { return new fp3x2 (lhs.c0 % rhs.c0, lhs.c1 % rhs.c1); }

        /// <summary>Returns the result of a componentwise modulus operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator % (fp3x2 lhs, fp rhs) { return new fp3x2 (lhs.c0 % rhs, lhs.c1 % rhs); }

        /// <summary>Returns the result of a componentwise modulus operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator % (fp lhs, fp3x2 rhs) { return new fp3x2 (lhs % rhs.c0, lhs % rhs.c1); }


        /// <summary>Returns the result of a componentwise increment operation on a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator ++ (fp3x2 val) { return new fp3x2 (++val.c0, ++val.c1); }


        /// <summary>Returns the result of a componentwise decrement operation on a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator -- (fp3x2 val) { return new fp3x2 (--val.c0, --val.c1); }


        /// <summary>Returns the result of a componentwise less than operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator < (fp3x2 lhs, fp3x2 rhs) { return new bool3x2 (lhs.c0 < rhs.c0, lhs.c1 < rhs.c1); }

        /// <summary>Returns the result of a componentwise less than operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator < (fp3x2 lhs, fp rhs) { return new bool3x2 (lhs.c0 < rhs, lhs.c1 < rhs); }

        /// <summary>Returns the result of a componentwise less than operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator < (fp lhs, fp3x2 rhs) { return new bool3x2 (lhs < rhs.c0, lhs < rhs.c1); }


        /// <summary>Returns the result of a componentwise less or equal operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator <= (fp3x2 lhs, fp3x2 rhs) { return new bool3x2 (lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1); }

        /// <summary>Returns the result of a componentwise less or equal operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator <= (fp3x2 lhs, fp rhs) { return new bool3x2 (lhs.c0 <= rhs, lhs.c1 <= rhs); }

        /// <summary>Returns the result of a componentwise less or equal operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator <= (fp lhs, fp3x2 rhs) { return new bool3x2 (lhs <= rhs.c0, lhs <= rhs.c1); }


        /// <summary>Returns the result of a componentwise greater than operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator > (fp3x2 lhs, fp3x2 rhs) { return new bool3x2 (lhs.c0 > rhs.c0, lhs.c1 > rhs.c1); }

        /// <summary>Returns the result of a componentwise greater than operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator > (fp3x2 lhs, fp rhs) { return new bool3x2 (lhs.c0 > rhs, lhs.c1 > rhs); }

        /// <summary>Returns the result of a componentwise greater than operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator > (fp lhs, fp3x2 rhs) { return new bool3x2 (lhs > rhs.c0, lhs > rhs.c1); }


        /// <summary>Returns the result of a componentwise greater or equal operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator >= (fp3x2 lhs, fp3x2 rhs) { return new bool3x2 (lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1); }

        /// <summary>Returns the result of a componentwise greater or equal operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator >= (fp3x2 lhs, fp rhs) { return new bool3x2 (lhs.c0 >= rhs, lhs.c1 >= rhs); }

        /// <summary>Returns the result of a componentwise greater or equal operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator >= (fp lhs, fp3x2 rhs) { return new bool3x2 (lhs >= rhs.c0, lhs >= rhs.c1); }


        /// <summary>Returns the result of a componentwise unary minus operation on a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator - (fp3x2 val) { return new fp3x2 (-val.c0, -val.c1); }


        /// <summary>Returns the result of a componentwise unary plus operation on a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 operator + (fp3x2 val) { return new fp3x2 (+val.c0, +val.c1); }


        /// <summary>Returns the result of a componentwise equality operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator == (fp3x2 lhs, fp3x2 rhs) { return new bool3x2 (lhs.c0 == rhs.c0, lhs.c1 == rhs.c1); }

        /// <summary>Returns the result of a componentwise equality operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator == (fp3x2 lhs, fp rhs) { return new bool3x2 (lhs.c0 == rhs, lhs.c1 == rhs); }

        /// <summary>Returns the result of a componentwise equality operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator == (fp lhs, fp3x2 rhs) { return new bool3x2 (lhs == rhs.c0, lhs == rhs.c1); }


        /// <summary>Returns the result of a componentwise not equal operation on two fp3x2 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator != (fp3x2 lhs, fp3x2 rhs) { return new bool3x2 (lhs.c0 != rhs.c0, lhs.c1 != rhs.c1); }

        /// <summary>Returns the result of a componentwise not equal operation on a fp3x2 matrix and a fp value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator != (fp3x2 lhs, fp rhs) { return new bool3x2 (lhs.c0 != rhs, lhs.c1 != rhs); }

        /// <summary>Returns the result of a componentwise not equal operation on a fp value and a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool3x2 operator != (fp lhs, fp3x2 rhs) { return new bool3x2 (lhs != rhs.c0, lhs != rhs.c1); }



        /// <summary>Returns the fp3 element at a specified index.</summary>
        unsafe public ref fp3 this[int index]
        {
            get
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if ((uint)index >= 2)
                    throw new System.ArgumentException("index must be between[0...1]");
#endif
                fixed (fp3x2* array = &this) { return ref ((fp3*)array)[index]; }
            }
        }

        /// <summary>Returns true if the fp3x2 is equal to a given fp3x2, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(fp3x2 rhs) { return c0.Equals(rhs.c0) && c1.Equals(rhs.c1); }

        /// <summary>Returns true if the fp3x2 is equal to a given fp3x2, false otherwise.</summary>
        public override bool Equals(object o) { return Equals((fp3x2)o); }


        /// <summary>Returns a hash code for the fp3x2.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() { return (int)fpmath.hash(this); }


        /// <summary>Returns a string representation of the fp3x2.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Format("fp3x2({0}, {1},  {2}, {3},  {4}, {5})", c0.x, c1.x, c0.y, c1.y, c0.z, c1.z);
        }

        /// <summary>Returns a string representation of the fp3x2 using a specified format and culture-specific format information.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("fp3x2({0}, {1},  {2}, {3},  {4}, {5})", c0.x.ToString(format, formatProvider), c1.x.ToString(format, formatProvider), c0.y.ToString(format, formatProvider), c1.y.ToString(format, formatProvider), c0.z.ToString(format, formatProvider), c1.z.ToString(format, formatProvider));
        }

    }

    public static partial class fpmath
    {
        /// <summary>Returns a fp3x2 matrix constructed from two fp3 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 fp3x2(fp3 c0, fp3 c1) { return new fp3x2(c0, c1); }

        /// <summary>Returns a fp3x2 matrix constructed from from 6 fp values given in row-major order.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 fp3x2(fp m00, fp m01,
                                  fp m10, fp m11,
                                  fp m20, fp m21)
        {
            return new fp3x2(m00, m01,
                             m10, m11,
                             m20, m21);
        }

        /// <summary>Returns a fp3x2 matrix constructed from a single fp value by assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 fp3x2(fp v) { return new fp3x2(v); }

        /// <summary>Returns a fp3x2 matrix constructed from a single int value by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 fp3x2(int v) { return new fp3x2(v); }

        /// <summary>Return a fp3x2 matrix constructed from a int3x2 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 fp3x2(int3x2 v) { return new fp3x2(v); }

        /// <summary>Returns a fp3x2 matrix constructed from a single uint value by converting it to fp and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 fp3x2(uint v) { return new fp3x2(v); }

        /// <summary>Return a fp3x2 matrix constructed from a uint3x2 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp3x2 fp3x2(uint3x2 v) { return new fp3x2(v); }

        /// <summary>Return the fp2x3 transpose of a fp3x2 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static fp2x3 transpose(fp3x2 v)
        {
            return fp2x3(
                v.c0.x, v.c0.y, v.c0.z,
                v.c1.x, v.c1.y, v.c1.z);
        }

        /// <summary>Returns a uint hash code of a fp3x2 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint hash(fp3x2 v)
        {
            return math.csum(fpmath.asuint(v.c0) * uint3(0xD4DFF6D3u, 0xCB634F4Du, 0x9B13B92Du) + 
                        fpmath.asuint(v.c1) * uint3(0x4ABF0813u, 0x86068063u, 0xD75513F9u)) + 0x5AB3E8CDu;
        }

        /// <summary>
        /// Returns a uint3 vector hash code of a fp3x2 vector.
        /// When multiple elements are to be hashes together, it can more efficient to calculate and combine wide hash
        /// that are only reduced to a narrow uint hash at the very end instead of at every step.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint3 hashwide(fp3x2 v)
        {
            return (fpmath.asuint(v.c0) * uint3(0x676E8407u, 0xB36DE767u, 0x6FCA387Du) + 
                    fpmath.asuint(v.c1) * uint3(0xAF0F3103u, 0xE4A056C7u, 0x841D8225u)) + 0xC9393C7Du;
        }

    }
}
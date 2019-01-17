/*  QuaternionHelper.cs
 *  Handles a few Quaternion methods that Unity has and this game needed
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public static class QuaternionHelper
    {
        private const float RAD2DEG = (float)(360 / (Math.PI * 2));

        public static float LengthSquared(Quaternion q)
        {
            return q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;
        }

        private static Vector3 XYZ(Quaternion q)
        {
            return new Vector3(q.X, q.Y, q.Z);
        }

        /// <summary>
        /// Is the dot product of two quaternions within tolerance for them to be considered equal?
        /// </summary>
        private static bool IsEqualUsingDot(float dot)
        {
            // Returns false in the presence of NaN values.
            return dot > 1.0f - float.Epsilon;
        }

        /// <summary>
        /// Returns the angle in degrees between two rotations /a/ and /b/.
        /// </summary>
        public static float Angle(Quaternion a, Quaternion b)
        {
            float dot = Quaternion.Dot(a, b);

            return IsEqualUsingDot(dot) ? 0.0f : (float)(Math.Acos(Math.Min(Math.Abs(dot), 1.0F)) * 2.0F * RAD2DEG);
        }

        /// <summary>
        /// Rotates a rotation From towards To
        /// </summary>
        // source: http://codegist.net/snippet/c/quaternioncs_mmusial_c
        public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
        {
            float angle = Angle(from, to);

            if (Math.Abs(angle) < float.Epsilon)
            {
                return to;
            }

            return SlerpUnclamped(ref from, ref to, Math.Min(1.0f, maxDegreesDelta / angle));
        }

        /// <summary>
        /// Spherically interpolates between 'a' and 'b' by 't'. The parameter 't' is not clamped.
        /// </summary>
        // source: http://codegist.net/snippet/c/quaternioncs_mmusial_c
        public static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
        {
            return SlerpUnclamped(ref a, ref b, t);
        }

        private static Quaternion SlerpUnclamped(ref Quaternion a, ref Quaternion b, float t)
        {
            // if either input is zero, return the other.
            if (Math.Abs(LengthSquared(a)) < float.Epsilon)
            {
                if (Math.Abs(LengthSquared(b)) < float.Epsilon)
                {
                    return Quaternion.Identity;
                }
                return b;
            }
            else if (Math.Abs(LengthSquared(b)) < float.Epsilon)
            {
                return a;
            }


            float cosHalfAngle = a.W * b.W + Vector3.Dot(XYZ(a), XYZ(b));

            if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
            {
                // angle = 0.0f, so just return one input.
                return a;
            }
            else if (cosHalfAngle < 0.0f)
            {
                b = new Quaternion(-b.X, -b.Y, -b.Z, -b.W);

                cosHalfAngle = -cosHalfAngle;
            }

            float blendA;
            float blendB;
            if (cosHalfAngle < 0.99f)
            {
                // do proper slerp for big angles
                float halfAngle = (float)Math.Acos(cosHalfAngle);
                float sinHalfAngle = (float)Math.Sin(halfAngle);
                float oneOverSinHalfAngle = 1.0f / sinHalfAngle;
                blendA = (float)Math.Sin(halfAngle * (1.0f - t)) * oneOverSinHalfAngle;
                blendB = (float)Math.Sin(halfAngle * t) * oneOverSinHalfAngle;
            }
            else
            {
                // do lerp if angle is really small.
                blendA = 1.0f - t;
                blendB = t;
            }

            Quaternion result = new Quaternion(blendA * XYZ(a) + blendB * XYZ(b), blendA * a.W + blendB * b.W);
            if (LengthSquared(result) > 0.0f)
            {
                return Quaternion.Normalize(result);
            }
            else
            {
                return Quaternion.Identity;
            }
        }

        /// <summary>
        /// Creates a rotation with the specified Forward and Upward directions
        /// </summary>
        // source: http://codegist.net/snippet/c/quaternioncs_mmusial_c
        public static Quaternion LookRotation(Vector3 forward, Vector3 up)
        {
            forward = Vector3.Normalize(forward);
            Vector3 right = Vector3.Normalize(Vector3.Cross(up, forward));
            up = Vector3.Cross(forward, right);
            var m00 = right.X;
            var m01 = right.Y;
            var m02 = right.Z;
            var m10 = up.X;
            var m11 = up.Y;
            var m12 = up.Z;
            var m20 = forward.X;
            var m21 = forward.Y;
            var m22 = forward.Z;


            float num8 = (m00 + m11) + m22;
            var quaternion = new Quaternion();
            if (num8 > 0f)
            {
                var num = (float)Math.Sqrt(num8 + 1f);
                quaternion.W = num * 0.5f;
                num = 0.5f / num;
                quaternion.X = (m12 - m21) * num;
                quaternion.Y = (m20 - m02) * num;
                quaternion.Z = (m01 - m10) * num;
                return quaternion;
            }
            if ((m00 >= m11) && (m00 >= m22))
            {
                var num7 = (float)Math.Sqrt(((1f + m00) - m11) - m22);
                var num4 = 0.5f / num7;
                quaternion.X = 0.5f * num7;
                quaternion.Y = (m01 + m10) * num4;
                quaternion.Z = (m02 + m20) * num4;
                quaternion.W = (m12 - m21) * num4;
                return quaternion;
            }
            if (m11 > m22)
            {
                var num6 = (float)Math.Sqrt(((1f + m11) - m00) - m22);
                var num3 = 0.5f / num6;
                quaternion.X = (m10 + m01) * num3;
                quaternion.Y = 0.5f * num6;
                quaternion.Z = (m21 + m12) * num3;
                quaternion.W = (m20 - m02) * num3;
                return quaternion;
            }
            var num5 = (float)Math.Sqrt(((1f + m22) - m00) - m11);
            var num2 = 0.5f / num5;
            quaternion.X = (m20 + m02) * num2;
            quaternion.Y = (m21 + m12) * num2;
            quaternion.Z = 0.5f * num5;
            quaternion.W = (m01 - m10) * num2;

            return quaternion;
        }

        /// <summary>
        /// Returns a Vector3 of the Radian angles from the given Quaternion
        /// </summary>
        // Source: https://www.gamedev.net/forums/topic/597324-quaternion-to-euler-angles-and-back-why-is-the-rotation-changing/?do=findComment&comment=4784099
        public static Vector3 RadianAngles(Quaternion q)
        {
            float eX = (float)Math.Atan2(-2 * (q.Y * q.Z - q.W * q.X), q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z);
            float eY = (float)Math.Asin(2 * (q.X * q.Z + q.W * q.Y));
            float eZ = (float)Math.Atan2(-2 * (q.X * q.Y - q.W * q.Z), q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z);

            return new Vector3(eX, eY, eZ);
        }

        /// <summary>
        /// Returns a Vector3 of the Euler angles from the given Quaternion - between -180 & 180
        /// </summary>
        public static Vector3 EulerAngles(Quaternion q)
        {
            Vector3 radians = RadianAngles(q);

            float x = MathHelper.ToDegrees(radians.X);

            if (x > 180)
            {
                x -= 180;
            }

            float y = MathHelper.ToDegrees(radians.Y);

            if (y > 180)
            {
                y -= 180;
            }

            float z = MathHelper.ToDegrees(radians.Z);

            if (z > 180)
            {
                z -= 180;
            }

            return new Vector3(x, y, z);
        }
        
    }
}
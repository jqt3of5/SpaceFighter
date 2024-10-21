using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class MathEx
    {
        public static float Cap(float a, float max, float min)
        {
            if (a > max)
            {
                return max;
            }

            if (a < min)
            {
                return min;
            }

            return a;
        }
        
        public static float CapAbs(float a, float max, float min)
        {
            if (Math.Abs(a) > Math.Abs(max))
            {
                return max;
            }

            if (Math.Abs(a) < Math.Abs(min))
            {
                return min;
            }

            return a;
        }
        public static float MaxAbs(float a, float b)
        {
            if (Math.Abs(a) > Math.Abs(b))
            {
                return a;
            }

            if (a < 0)
            {
                return -b;
            }

            return b;
        }
        
        public static float MinAbs(float a, float b)
        {
            if (Math.Abs(a) < Math.Abs(b))
            {
                return a;
            }
            
            if (a < 0)
            {
                return -b;
            }

            return b;
        }
        
        public static Vector3 MinAbs(Vector3 a, float b)
        {
            a.x = MinAbs(a.x, b);
            a.y = MinAbs(a.y, b);
            a.z = MinAbs(a.z, b);

            return a;
        }
    }
}
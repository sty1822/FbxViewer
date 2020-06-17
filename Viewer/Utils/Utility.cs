using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Viewer.Utils
{
    class CUtility
    {
        public static Vector3 ToDegree(Vector3 radian)
        {
            return new Vector3(MathHelper.RadiansToDegrees(radian.X),
                               MathHelper.RadiansToDegrees(radian.Y),
                               MathHelper.RadiansToDegrees(radian.Z));
        }

        public static Vector3 ToRadian(Vector3 degree)
        {
            return new Vector3(MathHelper.DegreesToRadians(degree.X),
                               MathHelper.DegreesToRadians(degree.Y),
                               MathHelper.DegreesToRadians(degree.Z));
        }

        public static Vector3 ToEulerAngle(Quaternion q)
        {
            Vector3 rotate; // is radian

            // x-axis(roll)
            double sinr_cosp = 2.0 * ((q.W * q.X) + (q.Y * q.Z));
            double cosr_cosp = 1.0 - 2.0 * (q.X * q.X + q.Y * q.Y);
            rotate.X = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            // y-axis(pitch)
            double sinp = 2.0 * ((q.W * q.Y) - (q.Z * q.X));
            if (Math.Abs(sinp) >= 1)
            {
                // use 90 degree if out of range
                rotate.Y = (float)(Math.PI * 0.5) * Math.Sign(sinp);
            }
            else
                rotate.Y = (float)Math.Asin(sinp);

            // z-axis(yaw)
            double siny_cosp = 2.0 * ((q.W * q.Z) + (q.X * q.Y));
            double cosy_cosp = 1.0 - 2.0 * ((q.Y * q.Y) + (q.Z * q.Z));
            rotate.Z = (float)Math.Atan2(siny_cosp, cosy_cosp);

            return rotate;
        }
    }
}

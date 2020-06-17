using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK;

namespace Viewer.Render
{
    class CPlane
    {
        public enum DiscriminantType
        {
            Invalid,
            OnPoint,        // 평면 위의 점, 판별식의 값이 0
            FrontPoint,     // Normal의 방향, 판별식의 값이 0보다 큼
            BackPoint,      // Normal의 반대 방향, 판별식의 값이 0보다 작음
        }

        #region Public Method
        public CPlane()
        {
        }

        /// <summary>
        /// 법선 벡터와 원점으로부터 평면까지의 최단거리로 평면을 구성합니다.
        /// </summary>
        /// <param name="x">법선벡터의 X값</param>
        /// <param name="y">법선벡터의 Y값</param>
        /// <param name="z">법선벡터의 Z값</param>
        /// <param name="d">최단 거리</param>
        public CPlane(float x, float y, float z, float d)
        {
            Vector3 normal = new Vector3(x, y, z);
            if(normal == Vector3.Zero)
            {
                string msg = String.Format(
                    "평면을 구성하기 위한 Normal 값이 잘못되었습니다({0})", normal);
                MessageBox.Show(msg);
                return;
            }

            normal.Normalize();
            mNormal = normal;
            mLengthToZeroPoint = d / (float)Math.Sqrt(x * x + y * y + z * z);

        }

        /// <summary>
        /// 법선 벡터와 원점으로부터 평면까지의 최단거리로 평면을 구성합니다.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="l"></param>
        public CPlane(Vector3 n, float l)
            : this(n.X, n.Y, n.Z, l) { }

        /// <summary>
        /// 3개의 점으로 평면을 구성합니다.
        /// </summary>
        /// <param name="p0">점0</param>
        /// <param name="p1">점1</param>
        /// <param name="p2">점2</param>
        public CPlane(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            FromPoints(ref p0, ref p1, ref p2);
        }

        public CPlane(CPlane other)
        {
            mNormal = other.mNormal;
            mLengthToZeroPoint = other.mLengthToZeroPoint;
        }

        
        /// <summary>
        /// 3개의 점으로 평면을 구성합니다.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public void FromPoints(ref Vector3 p0, ref Vector3 p1, ref Vector3 p2)
        {
            Vector3 v1 = new Vector3(p1 - p0);
            Vector3 v2 = new Vector3(p2 - p0);

            v1.Normalize();
            v2.Normalize();

            Vector3.Cross(ref v1, ref v2, out mNormal);
            mNormal.Normalize();
            mLengthToZeroPoint = -Vector3.Dot(mNormal, p0);
        }

        public Vector3 Normal { get { return mNormal; } set { mNormal = value; } }

        /// <summary>
        /// 평면에서 p까지의 최단 거리
        /// </summary>
        /// <param name="p">점</param>
        /// <returns></returns>
        public float DistanceToPoint(Vector3 p)
        {
            return Math.Abs(ProcessDiscriminat(p));
        }

        public DiscriminantType Discriminant(Vector3 p)
        {
            float disc = ProcessDiscriminat(p);
            DiscriminantType t = DiscriminantType.Invalid;

            if (disc == 0)
                t = DiscriminantType.OnPoint;
            else if (disc > 0)
                t = DiscriminantType.FrontPoint;
            else if (disc < 0)
                t = DiscriminantType.BackPoint;

            return t;
        }


        #endregion

        #region Private Method

        private float ProcessDiscriminat(Vector3 point)
        {
            return Vector3.Dot(mNormal, point) + mLengthToZeroPoint;
        }
        #endregion

        #region Variable
        private Vector3 mNormal;            // 평면의 법선 벡터
        private float mLengthToZeroPoint;   // 원점에서 평면까지의 최단 거리
        #endregion
    }
}

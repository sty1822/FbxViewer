using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Viewer.Render
{
    class CCameraData
    {
        #region Public Method
        public void CalcUpRightDirection(Vector3 look2Pos)
        {
            Vector3.Normalize(ref look2Pos, out Direction);

            if(Direction.Length < 0.00001f)
            {
                // OpenTK의 Normalize()함수는 길이가 0일 경우 Nan으로 지정
                return;
            }

            // Up
            Up.Normalize();

            // Right
            Vector3.Cross(ref Up, ref Direction, out Right);
            Right.Normalize();

            // Up
            Vector3.Cross(ref Direction, ref Right, out Up);
            Up.Normalize();
        }

        /// <summary>
        /// 카메라의 가로, 세로, Fov, 종횡비 변경시에만 호출됩니다.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public Matrix4 CreateProjMatrix(float w, float h)
        {
            Aspect = w / (float)h;

            return Matrix4.CreatePerspectiveFieldOfView(FOV, Aspect, Near, Far);
        }

        /// <summary>
        /// 절두체에 사용될 8개의 모서리 정점을 설정합니다.
        /// </summary>
        public void Setyp8Points()
        {
            float farLeft = -(float)System.Math.Tan(FOV * 0.5f) * Far * Aspect;
            float farTop = (float)System.Math.Tan(FOV * 0.5f) * Far;
            float nearLeft = -(float)System.Math.Tan(FOV * 0.5f) * Near * Aspect;
            float nearTop = (float)System.Math.Tan(FOV * 0.5f) * Near;

            LeftTopFar = new Vector3(farLeft, farTop, -Far);
            LeftBottomFar = new Vector3(farLeft, -farTop, -Far);
            RightTopFar = new Vector3(-farLeft, farTop, -Far);
            RightBottomFar = new Vector3(-farLeft, -farTop, -Far);

            LeftTopNear = new Vector3(nearLeft, nearTop, -Near);
            LeftBottomNear = new Vector3(nearLeft, -nearTop, -Near);
            RightTopNear = new Vector3(-nearLeft, nearTop, -Near);
            RightBottomNear = new Vector3(-nearLeft, -nearTop, -Near);
        }
        #endregion


        #region Variable
        public Vector3 Direction;               // 카메라가 바라보는 방향(단위벡터)
        public Vector3 Right;                   // 카메라의 오른쪽 방향 벡터(단위벡터)
        public Vector3 Up;                      // 카메라의 위쪽 벡터(단위벡터)
        public float FOV = MathHelper.PiOver4;  // 카메라의 시야각(45도 초기 설정, 라디안)
        public float Aspect;                    // 카메라의 가로, 세로 비율(종횡비)
        public float Near = 1.0f;               // 카메라에서 근평면까지의 거리 값
        public float Far = 1000.0f;             // 카메라에서 원평면까지의 거리 값

        public Vector3 LeftTopFar, LeftBottomFar, RightTopFar, RightBottomFar;
        public Vector3 LeftTopNear, LeftBottomNear, RightTopNear, RightBottomNear;
        #endregion
    }
}

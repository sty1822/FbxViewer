using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Viewer.Render
{
    class CFrustum
    {
        #region Public Method
        public CFrustum(CCameraData cam)
        {
            mCamData = cam;
            mPlNear = new CPlane();
            mPlFar = new CPlane();
            mPlLeft = new CPlane();
            mPlRight = new CPlane();
            mPlTop = new CPlane();
            mPlBottom = new CPlane();
        }

        public void CalcPlanes(ref Matrix4 matView)
        {
            // todo : matView의 값이 NaN일때 예외 발생!!
            Matrix4 matInvert;
            Matrix4.Invert(ref matView, out matInvert);

            // Debug 모드에서 절두체를 렌더링하기 위해 데이터를 저장(todo : 릴리즈에서는 제외하도록)
            mLeftTopFar = Vector3.TransformPosition(mCamData.LeftTopFar, matInvert);
            mLeftBottomFar = Vector3.TransformPosition(mCamData.LeftBottomFar, matInvert);
            mRightTopFar = Vector3.TransformPosition(mCamData.RightTopFar, matInvert);
            mRightBottomFar = Vector3.TransformPosition(mCamData.RightBottomFar, matInvert);

            mLeftTopNear = Vector3.TransformPosition(mCamData.LeftTopNear, matInvert);
            mLeftBottomNear = Vector3.TransformPosition(mCamData.LeftBottomNear, matInvert);
            mRightTopNear = Vector3.TransformPosition(mCamData.RightTopNear, matInvert);
            mRightBottomNear = Vector3.TransformPosition(mCamData.RightBottomNear, matInvert);

            mPlFar.FromPoints(ref mRightTopFar, ref mLeftTopFar, ref mRightBottomFar);
            mPlNear.FromPoints(ref mLeftTopNear, ref mRightTopNear, ref mRightBottomNear);
            mPlLeft.FromPoints(ref mLeftTopFar, ref mLeftTopNear, ref mLeftBottomNear);
            mPlRight.FromPoints(ref mRightTopNear, ref mRightTopFar, ref mRightBottomFar);
            mPlTop.FromPoints(ref mLeftTopFar, ref mRightTopFar, ref mRightTopNear);
            mPlBottom.FromPoints(ref mRightBottomNear, ref mLeftBottomNear, ref mLeftBottomFar);

            // 
            plNearCenterStart = new Vector3((mCamData.LeftTopNear.X + mCamData.RightTopNear.X) * 0.5f,
                (mCamData.LeftTopNear.Y + mCamData.LeftBottomNear.Y) * 0.5f,
                mCamData.LeftTopNear.Z);
            plNearCenterStart = Vector3.TransformPosition(plNearCenterStart, matInvert);

            Matrix4 matTrans = Matrix4.CreateTranslation(mPlNear.Normal * 5.0f);
            plNearCenterEnd = Vector3.TransformPosition(plNearCenterStart, matTrans);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 생성 불가
        /// </summary>
        private CFrustum() { }
        #endregion

        #region Variable
        private Vector3 plNearCenterStart;
        private Vector3 plNearCenterEnd;
        private CCameraData mCamData;
        private CPlane mPlNear, mPlFar, mPlLeft, mPlRight, mPlTop, mPlBottom;
        private Vector3 mLeftTopFar, mLeftBottomFar, mRightTopFar, mRightBottomFar;
        private Vector3 mLeftTopNear, mLeftBottomNear, mRightTopNear, mRightBottomNear;
        #endregion
    }
}

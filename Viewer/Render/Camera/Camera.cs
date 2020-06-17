using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace Viewer.Render
{
    public class CCamera
    {

        #region Public Method
        public CCamera(CCameraDiscription disc)
        {
            Reset(disc);
        }

        public void ResizeViewport(float w, float h)
        {
            ProjMatrix = mCamData.CreateProjMatrix(w, h);
        }

        public void LoadViewMatrix()
        {
            GL.LoadMatrix(ref mViewMatrix);
        }

        public void Move(Vector3 v)
        {
            Position += v;
            LookAt += v;

            CalcViewMatrix();
        }

        public void MoveAt(Vector3 pos)
        {
            Position += (pos - LookAt);
            LookAt = pos;
            CalcViewMatrix();
        }

        public void MoveRight(float f)
        {
            float factor = (Position - LookAt).Length * f / mDefaultPos2LookAt;
            Position += (mCamData.Right * factor);
            LookAt += (mCamData.Right * factor);

            CalcViewMatrix();
        }

        public void MoveUp(float f)
        {
            float factor = (Position - LookAt).Length * f / mDefaultPos2LookAt;
            Position += (Vector3.UnitZ * factor);
            LookAt += (Vector3.UnitZ * factor);

            CalcViewMatrix();
        }

        public void MoveFront(float f)
        {
            Vector3 look2pos = new Vector3(LookAt - Position);

            float factor = look2pos.Length * f / mDefaultPos2LookAt;
            look2pos.Z = 0.0f;
            look2pos.Normalize();

            Position += (look2pos * factor);
            LookAt += (look2pos * factor);

            CalcViewMatrix();
        }
        
        public void Zoom(float f)
        {
            Vector3 look2pos = new Vector3(Position - LookAt);
            if (look2pos.Length < f + mCamData.Near)
                return;

            Position += mCamData.Direction * f * 3.0f;

            CalcViewMatrix();
        }

        public void Rotate(Vector2 angle)
        {
            // Right Vector를 축으로 회전
            Vector3 look2pos = new Vector3(Position - LookAt);
            float l = look2pos.Length;

            Matrix4 matAxisRight = new Matrix4();
            
            Matrix4.CreateFromAxisAngle(mCamData.Right, -angle.Y, out matAxisRight);
            look2pos.Normalize();
            look2pos = Vector3.TransformPosition(look2pos, matAxisRight);
            Position = (look2pos * l) + LookAt;

            CalcViewMatrix();

            // Up Vector를 축으로 회전
            look2pos = new Vector3(Position - LookAt);
            l = look2pos.Length;

            Matrix4 matAxisUp = new Matrix4();
            
            Matrix4.CreateFromAxisAngle(mCamData.Up, angle.X, out matAxisUp);
            look2pos.Normalize();
            look2pos = Vector3.TransformPosition(look2pos, matAxisUp);
            Position = (look2pos * l) + LookAt;

            //
            mCamData.Up = Vector3.UnitZ;

            if(IsParallelWithUp(look2pos))
            {
                return;
            }

            CalcViewMatrix();

        }

        public void Reset(CCameraDiscription disc)
        {
            if (disc == null)
                disc = new CCameraDiscription();

            Position = new Vector3(disc.Position);
            LookAt = new Vector3(disc.LookAt);

            mCamData = new CCameraData();
            mCamData.FOV = disc.FOV;
            mCamData.Up = Vector3.UnitZ;
            mCamData.Far = disc.Far;
            mCamData.Near = disc.Near;
            mDefaultPos2LookAt = (Position - LookAt).Length;

            mFrustum = new CFrustum(mCamData);

            CalcViewMatrix();
        }

        //
        public Vector3 Position { get; private set; }
        public Vector3 LookAt { get; private set; }
        public Matrix4 ViewMatrix { get { return mViewMatrix; } }
        public Matrix4 ProjMatrix { get; private set; }
        public float Fov { get { return mCamData.FOV; } }
        public float Far { get { return mCamData.Far; } }
        public float Near { get { return mCamData.Near; } }
        #endregion

        #region Private Method
        /// <summary>
        /// 카메라 행렬을 계산합니다.
        /// </summary>
        private void CalcViewMatrix()
        {
            mCamData.CalcUpRightDirection(new Vector3(LookAt - Position));

            mViewMatrix = Matrix4.LookAt(Position, LookAt, mCamData.Up);

            mCamData.Setyp8Points();
            mFrustum.CalcPlanes(ref mViewMatrix);

        }

        private bool IsParallelWithUp(Vector3 dir)
        {
            // 회전시 Up벡터와 방향이 평행하면 외적 값이 0이 된다.
            float diff = System.Math.Abs(Vector3.Dot(dir, mCamData.Up)) - 1.0f;
            return System.Math.Abs(diff) < float.Epsilon;
        }
        #endregion

        #region Variable
        private CCameraData mCamData;
        private float mDefaultPos2LookAt;
        private CFrustum mFrustum = null;
        private Matrix4 mViewMatrix;
        #endregion

    }
}

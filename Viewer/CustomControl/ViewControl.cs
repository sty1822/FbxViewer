using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Viewer.Utils;
using Viewer.Render;

namespace Viewer.CustomControl
{
    public partial class ViewControl : GLControl
    {
        #region Public Method
        public ViewControl()
            : base(new OpenTK.Graphics.GraphicsMode(32, 24, 8, 8))
        {
            InitializeComponent();

            MouseWheel += OnMouseWheel;

            Camera = null;

            BackgroundColor = Color.DimGray;

        }

        public void Init()
        {
            CheckVersion();

            CCameraDiscription disc = new CCameraDiscription();
            Camera = new CCamera(disc);
            mDragMouseClock.Init(OnDragMouse, 120);

            ViewControl_Resize(this, EventArgs.Empty);
        }

        public bool RenderStart()
        {
            if (!Created)
                return false;

            if (Context.IsCurrent == false)
                MakeCurrent();

            //
            SetupRC();

            return true;
        }

        public void RenderGridAndAxis()
        {
            // 카메라 공간으로 이동
            Camera.LoadViewMatrix();
            {
                GL.Enable(EnableCap.DepthTest);

                if (mShowGrid)
                    DrawGrid();

                if (mShowAxis)
                    DrawAxis();

                // 카메라의 타겟 위치에 박스 그리기
                GL.PushMatrix();
                GL.Translate(Camera.LookAt);
                CSampleMesh.RenderCube(2.0f, Color.DarkOrange);
                GL.PopMatrix();

                // 카메라의 타겟에서 높이 값만 0인 곳에 박스그리기
                GL.PushMatrix();
                GL.Translate(Camera.LookAt.X, Camera.LookAt.Y, 0f);
                CSampleMesh.RenderCube(2.0f, Color.DarkOrchid);
                GL.PopMatrix();
            }
            GL.LoadIdentity();
        }

        public void RenderEnd()
        {
            SwapBuffers();
        }

        public void ResetCamera()
        {
            Camera.Reset(null);
        }

        #endregion

        #region Private Method
        private void CheckVersion()
        {
            Version ver = new Version(GL.GetString(StringName.Version).Substring(0, 3));
            Version target = new Version(1, 5);

            if (ver < target)
            {
                throw new NotSupportedException(String.Format
                    ("OpenGL {0} is required (you have {1}).", target, ver) + Environment.NewLine);
            }

            if (!GL.GetString(StringName.Extensions).Contains("GL_EXT_framebuffer_object"))
            {
                throw new NotSupportedException(
                    "GL_EXT_framebuffer_object extension is required. Please update your drivers." + Environment.NewLine);

            }

            string trace = "";
            int maj_ver, min_ver, dep_bits, sten_bits = 0;
            GL.GetInteger(GetPName.MajorVersion, out maj_ver);
            GL.GetInteger(GetPName.MinorVersion, out min_ver);
            GL.GetInteger(GetPName.DepthBits, out dep_bits);
            GL.GetInteger(GetPName.StencilBits, out sten_bits);

            trace += String.Format("OpenGL Context Version : {0}.{1}", maj_ver, min_ver) + Environment.NewLine + Environment.NewLine;
            trace += String.Format("Depth Bits : {0}", dep_bits) + Environment.NewLine;
            trace += String.Format("Stencil Bits : {0}", sten_bits) + Environment.NewLine;

            if (dep_bits < 16)
            {
                trace += String.Format("Aborting. Need at least 16 depth bits, only {0} available.", maj_ver) + Environment.NewLine + Environment.NewLine;
            }

            if (sten_bits < 8)
            {
                trace += String.Format("Aborting. Need at least 8 Stencil bit, only {0} available.", sten_bits) + Environment.NewLine + Environment.NewLine;
            }
        }

        private void SetupViewport()
        {
            if (Context.IsCurrent == false)
                MakeCurrent();

            GL.Viewport(0, 0, Width, Height);

            Matrix4 pm = Camera.ProjMatrix;

            GL.MatrixMode(MatrixMode.Projection);

            GL.LoadMatrix(ref pm);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void SetupRC()
        {
            GL.ClearColor(BackgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit |
                     ClearBufferMask.DepthBufferBit |
                     ClearBufferMask.StencilBufferBit);

            GL.ClearStencil(0);
            GL.StencilMask(0xFF);

            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.LoadIdentity();
        }

        private void DrawGrid()
        {
            Vector3 lookAt = new Vector3((float)Math.Floor(Camera.LookAt.X),
                                         (float)Math.Floor(Camera.LookAt.Y),
                                         0.0f);

            float size = 10.0f;
            int doubleCnt = mGridCount * 2;

            float line_half_length = mGridCount * size;

            GL.Begin(PrimitiveType.Lines);
            GL.Color3(Color.DarkGray);

            for(int i = 0; i < doubleCnt; ++i)
            {
                float pos = (mGridCount - i) * size;

                if(pos + lookAt.Y == 0 && mShowAxis)
                {
                    // 축을 그릴 경우, 축 쪽은 라인을 그리지 않는다.
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(-line_half_length + lookAt.X, pos + lookAt.Y, 0);
                }
                else
                {
                    GL.Vertex3(line_half_length + lookAt.X, pos + lookAt.Y, 0);
                    GL.Vertex3(-line_half_length + lookAt.X, pos + lookAt.Y, 0);
                }

                if(pos + lookAt.X == 0 && mShowAxis)
                {
                    // 축을 그릴 경우, 축 쪽은 라인을 그리지 않는다.
                    GL.Vertex3(0, 0, 0);
                    GL.Vertex3(pos + lookAt.X, -line_half_length + lookAt.Y, 0);
                }
                else
                {
                    GL.Vertex3(pos + lookAt.X, line_half_length + lookAt.Y, 0);
                    GL.Vertex3(pos + lookAt.X, -line_half_length + lookAt.Y, 0);
                }
            }

            GL.End();
        }

        private void DrawAxis()
        {
            float lw = GL.GetFloat(GetPName.LineWidth);

            GL.LineWidth(2.0f);

            GL.Begin(PrimitiveType.Lines);

            // Right X-Axis
            GL.Color3(Color.Red);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(1000.0f, 0.0f, 0.0f);

            // Foward Y-Axis
            GL.Color3(Color.Green);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 1000.0f, 0.0f);

            // Up Z-Axis
            GL.Color3(Color.Blue);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 1000.0f);

            GL.End();

            GL.LineWidth(lw);
        }

        #endregion

        #region Variable
        public CCamera Camera { get; private set; }
        public Color BackgroundColor { get; set; }

        private int mGridCount = 64;
        private bool mShowAxis = true;
        private bool mShowGrid = true;

        private CClock mDragMouseClock = new CClock();
        private Point mPreMousePosition = new Point(0, 0);
        private Point mMousePosition = new Point(0, 0);
        private MouseButtons mDownMouseButton = MouseButtons.None;
        #endregion

        #region event
        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            float zoomFactor = 0.0f;
            if (e.Delta > 0)
                zoomFactor += 7.0f;
            else
                zoomFactor -= 7.0f;

            Camera.Zoom(zoomFactor);
        }

        private void OnDragMouse()
        {
            Point pt = new Point(mPreMousePosition.X - mMousePosition.X, mPreMousePosition.Y - mMousePosition.Y);

            switch(mDownMouseButton)
            {
                case MouseButtons.Left:
                    {
                        float scaleFactor = 1.0f;
                        // 카메라의 Up-Right 평면 기준 카메라 이동
                        Camera.MoveRight(-pt.X * scaleFactor);
                        Camera.MoveUp(-pt.Y * scaleFactor);
                    }
                    break;
                case MouseButtons.Right:
                    {
                        // 카메라 LookAt에서 Up기준 카메라 회전
                        Vector2 angle = new Vector2(pt.X, pt.Y);
                        angle *= 0.01f;
                        Camera.Rotate(angle);
                    }
                    break;
                case MouseButtons.Middle:
                    {
                        float scaleFactor = 1.0f;
                        // xz 평면 이동
                        Camera.MoveRight(-pt.X * scaleFactor);
                        Camera.MoveFront(-pt.Y * scaleFactor);
                    }
                    break;
            }

            mPreMousePosition = mMousePosition;
        }

        private void ViewControl_Resize(object sender, EventArgs e)
        {
            // 카메라의 Projection Matrix를 재계산합니다.
            if(Camera != null)
            {
                Camera.ResizeViewport(Width, Height);
                SetupViewport();
            }
        }

        private void ViewControl_MouseDown(object sender, MouseEventArgs e)
        {
            mPreMousePosition = mMousePosition = e.Location;
            mDownMouseButton = e.Button;

            mDragMouseClock.Start();
        }

        private void ViewControl_MouseLeave(object sender, EventArgs e)
        {
            mDragMouseClock.Stop();
            mDownMouseButton = MouseButtons.None;
        }

        private void ViewControl_MouseMove(object sender, MouseEventArgs e)
        {
            mMousePosition = e.Location;
        }

        private void ViewControl_MouseUp(object sender, MouseEventArgs e)
        {
            mDragMouseClock.Stop();
            mDownMouseButton = MouseButtons.None;
        }

        
        #endregion

    }
}

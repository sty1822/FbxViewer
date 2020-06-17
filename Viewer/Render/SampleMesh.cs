using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using OpenTK.Graphics.OpenGL;

namespace Viewer.Render
{
    class CSampleMesh
    {
        #region Public Method
        public static void RenderCube(float size, Color color)
        {
            RenderCube(size, color, color, color, color, color, color);
        }

        public static void RenderCube(float size, Color leftColor, Color rightColor,
                                                  Color topColor, Color bottomColor,
                                                  Color frontColor, Color rearColor)
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            float half = size * 0.5f;
            GL.Begin(PrimitiveType.Quads);

            // left x-axis(-)
            GL.Color3(leftColor);
            GL.Vertex3(-half, -half, -half);
            GL.Vertex3(-half, -half,  half);
            GL.Vertex3(-half,  half,  half);
            GL.Vertex3(-half,  half, -half);


            // right x-axis(+)
            GL.Color3(rightColor);
            GL.Vertex3( half, -half,  half);
            GL.Vertex3( half, -half, -half);
            GL.Vertex3( half,  half, -half);
            GL.Vertex3( half,  half,  half);


            // top y-axis(+)
            GL.Color3(topColor);
            GL.Vertex3( half,  half,  half);
            GL.Vertex3( half,  half, -half);
            GL.Vertex3(-half,  half, -half);
            GL.Vertex3(-half,  half,  half);


            // bottom y-axis(-)
            GL.Color3(bottomColor);
            GL.Vertex3(-half, -half, -half);
            GL.Vertex3( half, -half, -half);
            GL.Vertex3( half, -half,  half);
            GL.Vertex3(-half, -half,  half);


            // front z-axis(+)
            GL.Color3(frontColor);
            GL.Vertex3(-half, -half,  half);
            GL.Vertex3( half, -half,  half);
            GL.Vertex3( half,  half,  half);
            GL.Vertex3(-half,  half,  half);

            // rear z-axis(-)
            GL.Color3(rearColor);
            GL.Vertex3( half,  half, -half);
            GL.Vertex3( half, -half, -half);
            GL.Vertex3(-half, -half, -half);
            GL.Vertex3(-half,  half, -half);


            GL.End();
            GL.Disable(EnableCap.CullFace);
        }
        #endregion
    }
}

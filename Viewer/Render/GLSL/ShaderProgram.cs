using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenTK.Graphics.OpenGL;

namespace Viewer.Render
{
    class CShaderProgram
    {
        #region Public Method
        public CShaderProgram(CShader vs, CShader fs)
        {
            GLHandle = 0;
            mVertexhandle = 0;
            mFragmentHandle = 0;

            CreateProgramAndAttach(vs, fs);
        }
        #endregion

        #region Private Method
        private void CreateProgramAndAttach(CShader vs, CShader fs)
        {
            try
            {
                mVertexhandle = GL.CreateShader(ShaderType.VertexShader);
                mFragmentHandle = GL.CreateShader(ShaderType.FragmentShader);

                int status_code;
                string info;

                // Compile Vertex Shader
                GL.ShaderSource(mVertexhandle, vs.Code);
                GL.CompileShader(mVertexhandle);
                GL.GetShaderInfoLog(mVertexhandle, out info);
                GL.GetShader(mVertexhandle, ShaderParameter.CompileStatus, out status_code);

                if (status_code != 1)
                    throw new ApplicationException("Vertex Shader Compile Error : \r\n" + info);

                // Compile Fragment Shader
                GL.ShaderSource(mFragmentHandle, fs.Code);
                GL.CompileShader(mFragmentHandle);
                GL.GetShaderInfoLog(mFragmentHandle, out info);
                GL.GetShader(mFragmentHandle, ShaderParameter.CompileStatus, out status_code);

                if (status_code != 1)
                    throw new ApplicationException("Fragment Shader compile Error : \r\n" + info);

                // Create Program
                GLHandle = GL.CreateProgram();

                GL.AttachShader(GLHandle, mVertexhandle);
                GL.AttachShader(GLHandle, mFragmentHandle);

                GL.LinkProgram(GLHandle);

                // Shader가 Program에 링크되면 Program 내부에 바이너리 코드로 보관하기 때문에 연결을 끊고, 해제해도 됩니다.
                GL.DetachShader(GLHandle, mVertexhandle);
                GL.DetachShader(GLHandle, mFragmentHandle);

                GL.DeleteShader(mVertexhandle);
                GL.DeleteShader(mFragmentHandle);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        #endregion

        #region Variable
        public int GLHandle { get; private set; }

        private int mVertexhandle;
        private int mFragmentHandle;
        #endregion
    }
}

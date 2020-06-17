using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using OpenTK.Graphics.OpenGL;

using Viewer.Files;

namespace Viewer.Render
{
    enum SupportShader
    {
        Model,
    }

    class CShaderSystem
    {
        #region Public Method
        public bool Init()
        {
            mShaderProgram.Add(SupportShader.Model,
                CreateShader(CFileSystem.Inst.AppPath + Path.DirectorySeparatorChar + "GLSL\\Model.vs",
                CFileSystem.Inst.AppPath + Path.DirectorySeparatorChar + "GLSL\\Model.fs"));

            return true;
        }

        public bool UseProgram(SupportShader shader)
        {
            if(mShaderProgram.ContainsKey(shader) == false)
            {
                GL.UseProgram(0);
                MessageBox.Show("등록되지 않은 ShaderProgram 입니다.");
            }

            GL.UseProgram(mShaderProgram[shader].GLHandle);

            return true;
        }

        public int GetProgram(SupportShader shader)
        {
            if(mShaderProgram.ContainsKey(shader) == false)
                return 0;

            return mShaderProgram[shader].GLHandle;
        }

        public void ReleaseProgram()
        {
            GL.UseProgram(0);
        }
        #endregion

        #region Private Method
        private CShaderProgram CreateShader(string vs_filename, string fs_filename)
        {
            CShader vs = new CShader(vs_filename);
            CShader fs = new CShader(fs_filename);

            return new CShaderProgram(vs, fs);
        }
        #endregion

        #region Variable
        private Dictionary<SupportShader, CShaderProgram> mShaderProgram = new Dictionary<SupportShader, CShaderProgram>();
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using WeifenLuo.WinFormsUI.Docking;
using OpenTK;

using Viewer.Render;
using Viewer.Asset;
using Viewer.Files;

namespace Viewer.Forms
{
    public partial class ViewForm : DockContent
    {
        public ViewForm()
        {
            InitializeComponent();
        }

        public void Init()
        {
            viewer.Init();

            if (viewer.Context.IsCurrent == false)
                viewer.MakeCurrent();


            // GLSL
            mShader.Init();
            mRenderer.Init();

            // 샘플 모델 로딩
            //string filename = CFileSystem.Inst.AppPath + Path.DirectorySeparatorChar + "Sample\\scent_stand2.fbx";
            //string filename = "C:\\sample\\cube_edge.FBX";
            //string filename = "C:\\sample\\scent_stand1.FBX";
            //mImportModel = new CModel();
            //mImportModel.Init(filename);
        }

        public void Progress()
        {
            // Update
            if( mImportModel != null)
            {
                mImportModel.Progress(Matrix4.Identity);

            }


            // Render
            if(viewer.RenderStart())
            {
                // 
                viewer.RenderGridAndAxis();

                mRenderer.Render(mImportModel, viewer.Camera, mShader, false);

                viewer.RenderEnd();
            }
        }

        #region Variable
        private CShaderSystem mShader = new CShaderSystem();
        private CModelRenderer mRenderer = new CModelRenderer();
        private CModel mImportModel = null;
        #endregion


    }
}

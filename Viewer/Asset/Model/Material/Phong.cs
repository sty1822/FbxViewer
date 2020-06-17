using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Newtonsoft.Json.Linq;



namespace Viewer.Asset
{
    class CPhong : CMaterial
    {
        #region Public Method
        public override bool Init()
        {
            GL.GenBuffers(1, out mUBO);

            return true;
        }

        public override void Uniform()
        {
            
        }

        public override bool Serialize(ref object writer)
        {
            return true;
        }
        #endregion

        #region Variable
        public override MaterialType Mode
        {
            get { return MaterialType.Phong; }
        }

        public Color AmbientColor { get; set; }
        public Color SpecularColor { get; set; }
        public float Sharpness { get; set; }

        private enum Location
        {
            eLightDirection = 7,
        };

        private struct UBOMaterialData
        {
            public Vector3 SpecularAlbedo;
            public float SpecularPower;
            public static int SizeInBytes = 16;
        }
        private int mUBO = 0;

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using Newtonsoft.Json.Linq;


namespace Viewer.Asset
{
    class CDiffuse : CMaterial
    {
        #region Public Method
        public override bool Init()
        {
            return true;
        }

        public override void Uniform()
        {
            // Material Routine Function Uniform
            //int func_index = (int)CModel
        }

        public override bool Serialize(ref object writer)
        {
            if (writer is JObject)
            {
                if (base.Serialize(ref writer))
                {
                    JObject jdiff = writer as JObject;
                    jdiff.Add("Type", Mode.ToString());
                }
            }
            else
                return false;

            return true;
        }
        #endregion


        #region Variable
        public override MaterialType Mode
        {
            get { return MaterialType.Diffuse; }
        }

        #endregion
    }
}
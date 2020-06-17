using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Viewer.Render
{
    public class CCameraDiscription
    {
        #region Variable
        public Vector3 Position = new Vector3(500.0f, -500.0f, 600.0f);
        public Vector3 LookAt = new Vector3(0.0f, 0.0f, 200.0f);
        public float FOV = MathHelper.PiOver4;
        public float Near = 1.0f;
        public float Far = 10000.0f;
        #endregion
    }
    
}

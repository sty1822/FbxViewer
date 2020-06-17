using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using OpenTK;
using Newtonsoft.Json.Linq;

using Viewer.Files;
using Viewer.Utils;

namespace Viewer.Asset
{
    class CAnimation
    {
        #region Public Method
        
        public bool Deserialize(JObject janim)
        {
            if (janim == null)
                return false;

            
            if (janim["Frames"] != null)
                FrameCount = janim["Frames"].Value<int>();

            if (janim["AniPerSec"] != null)
                AnimPerSec = janim["AniPerSec"].Value<float>();

            if (janim["IsLoop"] != null)
                IsLoop = janim["IsLoop"].Value<bool>();

            foreach (JObject jbone in janim["Bones"])
            {
                CBone bone = new CBone();
                if (bone.Deserialize(jbone) == true)
                    Bones.Add(bone);
            }
                

            return true;
        }

        public CAnimation()
        {
            FrameCount = 0;
            IsLoop = true;
            AnimPerSec = 1.0f;
            Bones = new List<CBone>();
        }

        public bool Init()
        {
            AnimatedMatrix = new List<Matrix4>();

            for (int i = 0; i < Bones.Count; ++i)
                AnimatedMatrix.Add(Matrix4.Identity);

            mAnimateTime = 0.0;
            BeforeFrame = 0;

            return true;
        }

        public void Progress(Matrix4 mat)
        {
            mAnimateTime += CWorldTime.Inst.Delta * 0.001f;
            CalcAnimatedMatrix();
        }
        #endregion

        #region Private Method
        private void CalcAnimatedMatrix()
        {
            double d = AnimPerSec * mAnimateTime * FrameCount;

            double animTime = d % FrameCount;

            BeforeFrame = (int)animTime;
            int afterFrame = BeforeFrame + 1;

            if (afterFrame >= FrameCount)
                afterFrame = 0;

            float blend = System.Math.Abs((float)animTime - BeforeFrame);

            foreach(CBone bone in Bones)
            {
                Matrix4 matBone;
                if(bone.ParentIndex != -1)
                {
                    matBone = bone.GetBlendedMatrix(blend, BeforeFrame, afterFrame) * AnimatedMatrix[bone.ParentIndex];
                }
                else
                {
                    matBone = bone.GetBlendedMatrix(blend, BeforeFrame, afterFrame);
                }

                AnimatedMatrix[bone.Index] = matBone;
            }
        }
        #endregion

        #region Variable

        public List<Matrix4> AnimatedMatrix { get; private set; }
        public int BeforeFrame { get; private set; }
        public int FrameCount { get; private set; }
        public List<CBone> Bones { get; private set; }

        public bool IsLoop { get; set; }
        public float AnimPerSec { get; set; }
        
        private double mAnimateTime = 0.0;

        #endregion
    }
}

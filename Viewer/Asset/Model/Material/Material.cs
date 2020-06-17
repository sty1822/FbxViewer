using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using Newtonsoft.Json.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL;

using Viewer.Files;


namespace Viewer.Asset
{
    enum MaterialType
    {
        Diffuse,
        Phong,
    }

    abstract class CMaterial : ISerializable
    {
        public enum BlendType
        {
            None,
            Alpha,
            Addtive,
            Multiply,
            Minus,
            OneMinusColor_One,
        }

        public enum FaceModeType
        {
            Front,
            Back,
            FrontAndBack,
        }

        #region Public Method
        
        public static CMaterial Create(string matName, string linkedSubsetName)
        {
            MaterialType mode = ConvertMode(matName);
            return Create(mode, linkedSubsetName);
        }

        public static CMaterial Create(MaterialType mode, string linkedSubsetName)
        {
            CMaterial mat = Create(mode);
            mat.LinkedSubsetName = linkedSubsetName;
            mat.Init();

            return mat;
        }


        public abstract bool Init();
        public abstract void Uniform();


        public virtual bool Serialize(ref object writer)
        {
            if (writer is JObject)
            {
                JObject jmat = writer as JObject;

                jmat.Add("Blend", BlendMode.ToString());
                //jmat.Add("Texture", Texture.Keyname);
                jmat.Add("DepthTest", IsDepthTest);
                jmat.Add("Face", FaceMode.ToString());
                jmat.Add("LinkedSubset", LinkedSubsetName.ToLower());
                jmat.Add("DiffColor", CJsonConverter.ConvertColor(DiffuseColor));
            }
            else
                return false;

            return true;
        }

        public virtual bool Deserialize(object data)
        {
            JObject jmat = data as JObject;
            if (jmat == null)
                return false;

            if (jmat["Blend"] != null)
                BlendMode = ConvertBlendType(jmat["Blend"].Value<string>());

            if(jmat["Texture"] != null)
            {
                string relative_filename = jmat["Texture"].Value<string>().ToLower();
                // todo : 텍스처 로딩
                //Texture.LoadFile(CFileSystem.Inst.AppPath + Path.DirectorySeparatorChar +)

            }
            else
            {
                // todo : 빈텍스처 등록
            }

            if (jmat["DepthTest"] != null)
                IsDepthTest = jmat["DepthTest"].Value<bool>();

            if (jmat["Face"] != null)
                FaceMode = ConvertFaceModeType(jmat["Face"].Value<string>());

            if (jmat["LinkedSubset"] != null)
                LinkedSubsetName = jmat["LinkedSubset"].Value<string>().ToLower();
            else
                return false;

            if (jmat["DiffColor"] != null)
                DiffuseColor = CJsonConverter.ConvertColor(jmat["DiffColor"].Value<JArray>());


            return true;
        }

        public virtual void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, Texture.GLIndex);
        }

        public virtual void GLModeSetting()
        {
            BlendModeSetting();
            DepthModeSetting();
            FaceModeSetting();
        }

        public CMaterial()
        {
            DiffuseColor = Color.White;
            BlendMode = BlendType.None;
            IsDepthTest = true;
            FaceMode = FaceModeType.FrontAndBack;
            Texture = new CTexture();
            LinkedSubsetName = "";
        }

        public bool IsTransparent()
        {
            switch(BlendMode)
            {
                case BlendType.None:
                    return false;
                case BlendType.Alpha:
                    return true;
                case BlendType.Addtive:
                    return true;
                case BlendType.Multiply:
                    return true;
                case BlendType.Minus:
                    return true;
                case BlendType.OneMinusColor_One:
                    return true;

            }

            return false;
        }

        #endregion

        #region Private Method
        private static CMaterial Create(MaterialType mode)
        {
            CMaterial mat = null;
            switch(mode)
            {
                case MaterialType.Phong:
                    mat = new CPhong();
                    break;
                case MaterialType.Diffuse:
                    mat = new CDiffuse();
                    break;
            }

            return mat;
        }

        private static FaceModeType ConvertFaceModeType(string type)
        {
            FaceModeType mode = FaceModeType.FrontAndBack;

            if (type == FaceModeType.Front.ToString())
                mode = FaceModeType.Front;
            else if (type == FaceModeType.Back.ToString())
                mode = FaceModeType.Back;

            return mode;
        }

        private static BlendType ConvertBlendType(string type)
        {
            if (type == BlendType.None.ToString())
                return BlendType.None;
            else if (type == BlendType.Alpha.ToString())
                return BlendType.Alpha;
            else if (type == BlendType.Addtive.ToString())
                return BlendType.Addtive;
            else if (type == BlendType.Multiply.ToString())
                return BlendType.Multiply;
            else if (type == BlendType.Minus.ToString())
                return BlendType.Minus;
            else if (type == BlendType.OneMinusColor_One.ToString())
                return BlendType.OneMinusColor_One;

            return BlendType.None;
        }

        private static MaterialType ConvertMode(string mode_name)
        {
            MaterialType mode = MaterialType.Phong;

            if (mode_name == MaterialType.Phong.ToString())
                mode = MaterialType.Phong;
            else if (mode_name == MaterialType.Diffuse.ToString())
                mode = MaterialType.Diffuse;

            return mode;
        }



        private void FaceModeSetting()
        {
            switch (FaceMode)
            {
                case FaceModeType.Front:
                    {
                        GL.Enable(EnableCap.CullFace);
                        GL.CullFace(CullFaceMode.Back);
                    }
                    break;
                case FaceModeType.Back:
                    {
                        GL.Enable(EnableCap.CullFace);
                        GL.CullFace(CullFaceMode.Front);
                    }
                    break;
                case FaceModeType.FrontAndBack:
                    {
                        GL.Disable(EnableCap.CullFace);
                    }
                    break;
            }
        }

        private void DepthModeSetting()
        {
            if (IsDepthTest == true)
                GL.Enable(EnableCap.DepthTest);
            else
                GL.Disable(EnableCap.DepthTest);
        }

        private void BlendModeSetting()
        {
            switch (BlendMode)
            {
                case BlendType.None:
                    GL.Disable(EnableCap.Blend);
                    break;
                case BlendType.Alpha:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                    break;
                case BlendType.Addtive:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
                    break;
                case BlendType.Multiply:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.DstColor, BlendingFactorDest.Zero);
                    break;
                case BlendType.Minus:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.Zero, BlendingFactorDest.OneMinusSrcColor);
                    break;
                case BlendType.OneMinusColor_One:
                    GL.Enable(EnableCap.Blend);
                    GL.BlendFunc(BlendingFactorSrc.OneMinusSrcColor, BlendingFactorDest.One);
                    break;
            }
        }
        #endregion

        #region Variable
        public abstract MaterialType Mode { get; }

        public Color DiffuseColor { get; set; }
        public BlendType BlendMode { get; set; }
        public bool IsDepthTest { get; set; }
        public FaceModeType FaceMode { get; set; }
        public CTexture Texture { get; set; }
        public string LinkedSubsetName { get; set; }
        #endregion



    }
}

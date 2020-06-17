using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Newtonsoft.Json.Linq;
using OpenTK;

using Viewer.Files;
using Viewer.Utils;

namespace Viewer.Asset
{
    class CKeyFrame : ISerializable
    {
        #region Public Method
        public CKeyFrame()
        {
            LclTranslate = Vector3.Zero;
            LclRotation = Quaternion.Identity;
            LclScaling = Vector3.One;
        }

        public bool Serialize(ref object writer)
        {
            if (writer is BinaryWriter)
            {
                BinaryWriter bw = writer as BinaryWriter;

                Vector3 trans = LclTranslate;
                Vector3 radian = CUtility.ToDegree(CUtility.ToEulerAngle(LclRotation));
                Vector3 scale = LclScaling;
                CBinaryConverter.Write(bw, ref trans);
                CBinaryConverter.Write(bw, ref radian);
                CBinaryConverter.Write(bw, ref scale);
            }
            else
                return false;

            return true;
        }

        public bool Deserialize(object data)
        {
            if (data is BinaryReader)
            {
                BinaryReader br = data as BinaryReader;

                Vector3 trans = Vector3.Zero;
                Vector3 rot = Vector3.Zero;
                Vector3 scale = Vector3.One;

                CBinaryConverter.Read(br, out trans);
                CBinaryConverter.Read(br, out rot);
                CBinaryConverter.Read(br, out scale);

                LclTranslate = trans;
                Quaternion qx = Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(rot.X));
                Quaternion qy = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(rot.Y));
                Quaternion qz = Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(rot.Z));
                LclRotation = qz * qy * qx;
                LclScaling = scale;
            }
            else if (data is JObject)
            {
                JObject jkf = data as JObject;
                if (jkf == null)
                    return false;

                if (jkf["LT"] == null || jkf["LS"] == null || jkf["LR"] == null)
                    return false;

                LclTranslate = CJsonConverter.ConvertVector3(jkf["LT"].Value<JArray>());
                LclScaling = CJsonConverter.ConvertVector3(jkf["LS"].Value<JArray>());

                Vector3 rot = CJsonConverter.ConvertVector3(jkf["LR"].Value<JArray>());
                Quaternion qx = Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(rot.X));
                Quaternion qy = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(rot.Y));
                Quaternion qz = Quaternion.FromAxisAngle(Vector3.UnitZ, MathHelper.DegreesToRadians(rot.Z));
                LclRotation = qz * qy * qx;
            }
            else
                return false;

            return true;
        }
        #endregion

        #region Variable
        public Vector3 LclTranslate { get; private set; }
        public Quaternion LclRotation { get; private set; }
        public Vector3 LclScaling { get; private set; }
        #endregion
    }
}

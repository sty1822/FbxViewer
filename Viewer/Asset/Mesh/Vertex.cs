using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using OpenTK;
using Newtonsoft.Json.Linq;

using Viewer.Files;

namespace Viewer.Asset
{
    struct CVertex : ISerializable
    {
        #region Public Method
        public bool Serialize(ref object writer)
        {
            if (writer is BinaryWriter)
            {
                BinaryWriter bw = writer as BinaryWriter;

                bw.Write(BoneIndex);
                CBinaryConverter.Write(bw, ref Position);
                CBinaryConverter.Write(bw, ref Normal);
                CBinaryConverter.Write(bw, ref Texcoord);
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

                BoneIndex = br.ReadInt32();
                CBinaryConverter.Read(br, out Position);
                CBinaryConverter.Read(br, out Normal);
                CBinaryConverter.Read(br, out Texcoord);
            }
            else if (data is JObject)
            {
                JObject jv = data as JObject;

                if (jv["Pos"] != null)
                    Position = CJsonConverter.ConvertVector3(jv["Pos"].Value<JArray>());
                if (jv["Nor"] != null)
                    Normal = CJsonConverter.ConvertVector3(jv["Nor"].Value<JArray>());
                if (jv["UV"] != null)
                    Texcoord = CJsonConverter.ConvertVector2(jv["UV"].Value<JArray>());
                if (jv["BI"] != null)
                    BoneIndex = jv["BI"].Value<int>();
            }
            else
                return false;

            return true;
        }
        #endregion
        

        #region Variable
        public static int OffsetAtPosition = 0;
        public static int OffsetAtNormal = 12;
        public static int OffsetAtTexcoord = 24;
        public static int OffsetAtBoneIndex = 32;
        public static int SizeInBytes = 36;

        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 Texcoord;
        public int BoneIndex;
        #endregion
    }
}

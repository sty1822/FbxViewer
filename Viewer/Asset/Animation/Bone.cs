using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Newtonsoft.Json.Linq;
using OpenTK;

using Viewer.Files;


namespace Viewer.Asset
{
    class CBone : ISerializable
    {
        #region Public Method
        public CBone()
        {
            Index = -1;
            ParentIndex = -1;
            Name = "";
            Frames = new List<CKeyFrame>();
        }

        public Matrix4 GetBlendedMatrix(float blend, int beforeFrame, int afterFrame)
        {
            // Translate
            Vector3 bt = Frames[beforeFrame].LclTranslate;
            Vector3 at = Frames[afterFrame].LclTranslate;
            Vector3 trans = Vector3.Lerp(bt, at, blend);
            Matrix4 matAnimTrans = Matrix4.CreateTranslation(trans);

            // Rotation
            Quaternion br = Frames[beforeFrame].LclRotation;
            Quaternion ar = Frames[afterFrame].LclRotation;
            Quaternion rot = Quaternion.Slerp(br, ar, blend);
            Matrix4 matAnimRot = Matrix4.CreateFromQuaternion(rot);

            // Scaling
            Vector3 bsc = Frames[beforeFrame].LclScaling;
            Vector3 asc = Frames[afterFrame].LclScaling;
            Vector3 scale = Vector3.Lerp(bsc, asc, blend);
            Matrix4 matAnimScale = Matrix4.CreateScale(scale);

            return matAnimScale * matAnimRot * matAnimTrans;
        }

        public bool Serialize(ref object writer)
        {
            if (writer is BinaryWriter)
            {
                BinaryWriter bw = writer as BinaryWriter;

                bw.Write(Index);
                bw.Write(ParentIndex);
                bw.Write(Name);

                if (Frames != null)
                {
                    bw.Write(Frames.Count);
                    foreach (CKeyFrame kf in Frames)
                    {
                        kf.Serialize(ref writer);
                    }
                }
            }
            else
                return false;

            return true;
        }

        public bool Deserialize(object data)
        {
            if(data is BinaryReader)
            {
                BinaryReader br = data as BinaryReader;

                Index = br.ReadInt32();
                ParentIndex = br.ReadInt32();
                Name = br.ReadString().ToLower();

                int frameCount = br.ReadInt32();
                for(int i = 0; i < frameCount; ++i)
                {
                    CKeyFrame kf = new CKeyFrame();
                    if (kf.Deserialize(data) == true)
                        Frames.Add(kf);
                }
            }
            else if(data is JObject)
            {
                JObject jbone = data as JObject;
                if (jbone == null)
                    return false;

                if (jbone["Idx"] == null || jbone["PI"] == null ||
                    jbone["Name"] == null || jbone["Frames"] == null)
                    return false;

                Index = jbone["Idx"].Value<int>();
                ParentIndex = jbone["PI"].Value<int>();
                Name = jbone["Name"].Value<string>();

                foreach(JObject jkeyframe in jbone["Frames"])
                {
                    CKeyFrame kf = new CKeyFrame();
                    if (kf.Deserialize(jkeyframe) == false)
                        return false;

                    Frames.Add(kf);
                }

            }

            return true;
        }
        #endregion

        #region Variable
        public int Index { get; private set; }
        public int ParentIndex { get; private set; }
        public string Name { get; private set; }
        public List<CKeyFrame> Frames { get; private set; }

        #endregion

    }
}

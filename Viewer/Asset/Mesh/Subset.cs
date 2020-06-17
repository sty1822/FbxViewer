using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Newtonsoft.Json.Linq;

using Viewer.Files;

namespace Viewer.Asset
{
    class CSubset : ISerializable
    {
        #region Public Method
        public CSubset()
        {
            Name = "";
            IndexOffset = 0;
            TriangleCount = 0;
            Visible = true;
        }

        public bool Serialize(ref object writer)
        {
            if (writer is BinaryWriter)
            {
                BinaryWriter bw = writer as BinaryWriter;

                bw.Write(Name);
                bw.Write(IndexOffset);
                bw.Write(TriangleCount);
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

                Name = br.ReadString().ToLower();
                IndexOffset = br.ReadInt32();
                TriangleCount = br.ReadInt32();
            }
            else if (data is JObject)
            {
                JObject jsubset = data as JObject;

                if (jsubset["Name"] != null)
                    Name = jsubset["Name"].Value<string>().ToLower();

                if (jsubset["Offset"] != null)
                    IndexOffset = jsubset["Offset"].Value<int>();

                if (jsubset["TriCnt"] != null)
                    TriangleCount = jsubset["TriCnt"].Value<int>();
            }
            else
                return false;

            return true;
        }
        #endregion

        #region Variable
        public int IndexOffset { get; private set; }
        public int TriangleCount { get; private set; }
        public bool Visible { get; set; }
        public string Name { get; set; }
        #endregion

    }
}

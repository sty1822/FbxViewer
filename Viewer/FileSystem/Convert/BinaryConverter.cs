using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using OpenTK;

namespace Viewer.Files
{
    class CBinaryConverter
    {
        #region Public Method
        public static void Write(BinaryWriter bw, ref Vector2 v)
        {
            bw.Write(v.X);
            bw.Write(v.Y);
        }

        public static void Read(BinaryReader br, out Vector2 v)
        {
            v.X = br.ReadSingle();
            v.Y = br.ReadSingle();
        }

        public static void Write(BinaryWriter bw, ref Vector3 v)
        {
            bw.Write(v.X);
            bw.Write(v.Y);
            bw.Write(v.Z);
        }

        public static void Read(BinaryReader br, out Vector3 v)
        {
            v.X = br.ReadSingle();
            v.Y = br.ReadSingle();
            v.Z = br.ReadSingle();
        }

        #endregion
    }
}

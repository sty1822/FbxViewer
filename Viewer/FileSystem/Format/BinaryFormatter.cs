using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Viewer.Files
{
    class CBinaryFormatter : IFormatter
    {
        #region Public Method
        public bool SaveFile(IFileable f)
        {
            if (f == null)
                return false;

            ISerializable iser = f as ISerializable;

            if (iser == null)
                return false;

            FileStream fs = new FileStream(f.FileInfo.FullFileName, FileMode.Create, FileAccess.Write);
            object writer = new BinaryWriter(fs);

            bool success = iser.Serialize(ref writer);

            BinaryWriter bw = writer as BinaryWriter;

            bw.Close();
            fs.Close();

            return success;
        }

        public bool LoadFile(IFileable f)
        {
            if (f == null)
                return false;

            ISerializable iser = f as ISerializable;

            if (iser == null)
                return false;

            FileStream fs = new FileStream(f.FileInfo.FullFileName, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fs);

            bool success = iser.Deserialize(br);
            br.Close();
            fs.Close();

            return success;
        }

        #endregion
    }
}

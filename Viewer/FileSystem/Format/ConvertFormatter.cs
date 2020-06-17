using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Viewer.Files
{
    class CConvertFormatter : IFormatter
    {
        public CConvertFormatter()
        {
            //mConverters.Add(CFileSystem.Inst.FbxExt, new CFbxConverter());
        }

        public bool SaveFile(IFileable f)
        {
            MessageBox.Show("미구현");
            return false;
        }

        public bool LoadFile(IFileable f)
        {
            try
            {
                if(mConverters.ContainsKey(f.FileInfo.Extension) == false)
                {
                    MessageBox.Show("해당 파일을 지원하는 Converter가 없습니다. - " + f.FileInfo.Extension);
                    return false;
                }

                IConverter conv = mConverters[f.FileInfo.Extension];
                JObject jmodel = conv.Import(f.FileInfo.FullFileName);

                // Deserialize
                ISerializable iser = f as ISerializable;
                if (iser == null)
                    return false;

                if (iser.Deserialize(jmodel) == false)
                    return false;
            }
            catch(Exception e)
            {
                //f.FileInfo.error
                MessageBox.Show(e.ToString());
                return false;
            }

            return true;
        }


        #region Variable
        private Dictionary<CFileExtension, IConverter> mConverters =
                                                new Dictionary<CFileExtension, IConverter>();
        #endregion
    }
}

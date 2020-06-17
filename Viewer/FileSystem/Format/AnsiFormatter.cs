using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Viewer.Files
{
    class CAnsiFormatter : IFormatter
    {
        #region Public Method
        public bool SaveFile(IFileable f)
        {
            MessageBox.Show("미구현");
            return false;
        }

        public bool LoadFile(IFileable f)
        {
            try
            {
                StreamReader sr = new StreamReader(f.FileInfo.FullFileName);
                string text = sr.ReadToEnd();

                ISerializable iser = f as ISerializable;
                if (iser == null)
                    return false;

                if(iser.Deserialize(text) == false)
                {
                    MessageBox.Show("파일 deserialize 실패 - " + f.FileInfo.FullFileName);
                    return false;
                }
            }
            catch(Exception e)
            {
                // error log
                MessageBox.Show(e.ToString());
            }

            return true;
        }
        #endregion
    }
}

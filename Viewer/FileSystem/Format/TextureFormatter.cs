using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using FreeImageAPI;

namespace Viewer.Files
{
    class CTextureFormatter : IFormatter
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
                Bitmap bm = LoadTexture(f);
                if (bm == null)
                    return false;

                ISerializable iser = f as ISerializable;

                if (iser == null)
                    return false;

                if (iser.Deserialize(bm) == false)
                    return false;

            }
            catch(Exception e)
            {
                // f.fileinfo.error
                MessageBox.Show(e.ToString());
                return false;
            }

            return true;
        }
        #endregion

        #region Private Method
        private Bitmap LoadTexture(IFileable f)
        {
            FIBITMAP dib = new FIBITMAP();
            if (dib.IsNull == false)
                FreeImage.Unload(dib);

            FREE_IMAGE_FORMAT fif = FreeImage.GetFIFFromFilename(f.FileInfo.FullFileName);

            dib = FreeImage.Load(fif, f.FileInfo.FullFileName, FREE_IMAGE_LOAD_FLAGS.DEFAULT);

            FreeImage.FlipVertical(dib);

            if(dib.IsNull)
            {
                // f.FileInfo.error
                return null;
            }

            Bitmap bitmap = FreeImage.GetBitmap(dib);

            FreeImage.Unload(dib);

            return bitmap;
        }
        #endregion
    }
}

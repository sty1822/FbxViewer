using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using FreeImageAPI;

using Viewer.Files;

namespace Viewer.Asset
{
    class CTexture
    {
        #region Public Method
        public bool LoadFile(string filename)
        {
            Bitmap bm = LoadTexture(filename);
            if (bm == null)
            {
                MessageBox.Show("Deserialize()에 들어온 data가 Bitmap이 아닙니다.");
                return false;
            }

            GLIndex = RegistOpenGL(bm);
            Height = bm.Height;
            Width = bm.Width;
            Image = bm;

            return true;
        }

        
        public CTexture()
        {
            Image = null;
        }
        #endregion

        #region Private Method
        private int RegistOpenGL(Bitmap bm)
        {
            int tex = 0;
            //GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out tex);
            GL.BindTexture(TextureTarget.Texture2D, tex);

            BitmapData bmd = bm.LockBits(new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height),
                            ImageLockMode.ReadOnly, bm.PixelFormat);

            switch(bm.PixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb: // jpg
                    {
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
                                        bmd.Width, bmd.Height, 0,
                                        OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, bmd.Scan0);
                    }
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb: // tga
                    {
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                                        bmd.Width, bmd.Height, 0,
                                        OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmd.Scan0);
                    }
                    break;
            }

            bm.UnlockBits(bmd);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureWrapMode.Repeat);


            return tex;
        }

        private Bitmap LoadTexture(string filename)
        {
            FIBITMAP dib = new FIBITMAP();
            if (dib.IsNull == false)
                FreeImage.Unload(dib);

            FREE_IMAGE_FORMAT fif = FreeImage.GetFIFFromFilename(filename);

            dib = FreeImage.Load(fif, filename, FREE_IMAGE_LOAD_FLAGS.DEFAULT);

            FreeImage.FlipVertical(dib);

            if (dib.IsNull)
            {
                // f.FileInfo.error
                return null;
            }

            Bitmap bitmap = FreeImage.GetBitmap(dib);

            FreeImage.Unload(dib);

            return bitmap;
        }
        #endregion

        #region Variable
        public int GLIndex { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public Bitmap Image { get; private set; }

        #endregion
    }
}

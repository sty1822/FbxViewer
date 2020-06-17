using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Viewer.DesignPattern;

namespace Viewer.Files
{
    class CFileSystem : CSingletonPattern<CFileSystem>
    {
        #region Public Method
        public override bool Init()
        {
            AppPath = Path.GetDirectoryName(Application.ExecutablePath).ToLower();

            // 확장자 생성
            JsonExt = new CJsonExtension();
            JpgExt = new CJpgExtension();
            TgaExt = new CTgaExtension();
            PngExt = new CPngExtension();
            FbxExt = new CFbxExtension();
            

            AnimExt = new CAnimExtension();
            MeshExt = new CMeshExtension();
            GOExt = new CGameObjectExtension();
            ModelExt = new CModelExtension();

            // 확장자와 Formatter 매칭
            mFormatters.Add(JsonExt, new CJsonFormatter());
            mFormatters.Add(AnimExt, new CBinaryFormatter());
            mFormatters.Add(MeshExt, new CBinaryFormatter());
            mFormatters.Add(ModelExt, new CJsonFormatter());
            mFormatters.Add(GOExt, new CJsonFormatter());
            mFormatters.Add(TgaExt, new CTextureFormatter());
            mFormatters.Add(JpgExt, new CTextureFormatter());
            mFormatters.Add(PngExt, new CTextureFormatter());
            mFormatters.Add(FbxExt, new CConvertFormatter());
            

            return true;
        }

        public CFileExtension GetExt(string ext)
        {
            foreach(CFileExtension fe in mFormatters.Keys)
            {
                if (fe.Is(ext))
                    return fe;
            }

            return null;
        }

        public bool SaveFile(string filename, IFileable dest)
        {
            dest.FileInfo = CFileInfo.SetFileInfo(filename);
            
            // todo : 파일 유효성 체크

            return SaveFile(dest);
        }

        public bool LoadFile(IFileable dest)
        {
            if(dest.FileInfo == null)
            {
                MessageBox.Show("파일 로딩 실패");
                return false;
            }

            // Formatter 선택
            IFormatter formatter = mFormatters[dest.FileInfo.Extension];

            if(formatter.LoadFile(dest) == false)
            {
                // todo : file error logging
                return false;
            }

            // todo : file status loaded!!

            return true;
        }

        public bool IsSupported(CFileExtension ext)
        {
            return mFormatters.ContainsKey(ext);
        }

        public bool IsSupported(string ext)
        {
            CFileExtension fe = GetExt(ext);

            return (fe == null) ? false : true;
        }
        #endregion


        #region Private Method
        private bool SaveFile(IFileable dest)
        {
            if(dest.FileInfo == null)
            {
                MessageBox.Show("저장 실패");
                return false;
            }


            // Formatter 선택
            IFormatter formatter = mFormatters[dest.FileInfo.Extension];

            if(formatter.SaveFile(dest) == false)
            {
                // todo : file error logging
                return false;
            }

            // todo : file status 변경 saved!

            return true;
        }

        #endregion


        #region Variable
        public string AppPath { get; private set; }

        public CFileExtension JpgExt { get; private set; }
        public CFileExtension TgaExt { get; private set; }
        public CFileExtension PngExt { get; private set; }
        public CFileExtension FbxExt { get; private set; }
        public CFileExtension JsonExt { get; private set; }

        public CFileExtension AnimExt { get; private set; }
        public CFileExtension MeshExt { get; private set; }
        public CFileExtension GOExt { get; private set; }
        public CFileExtension ModelExt { get; private set; }


        private Dictionary<CFileExtension, IFormatter> mFormatters =
                                          new Dictionary<CFileExtension, IFormatter>();

        #endregion
    }
}

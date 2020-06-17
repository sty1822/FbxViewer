using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Viewer.Files
{
    class CFileInfo
    {
        public enum FileStatusType
        {
            Invalid,
            Valid,
        }
        #region Public Method
        public static CFileInfo SetFileInfo(string filename)
        {
            string lower = filename.ToLower();

            CFileInfo info = new CFileInfo();

            // 파일 정보 설정
            if(info.Setting(lower) == false)
            {
                //info.error
                //info.Status = ?
                return info;
            }

            // 경로 유효성 체크
            if(Directory.Exists(info.AbsolutePath) == false)
            {
                //info.error
                //info.Status = ?
                return info;
            }

            //info.Status = ready?

            return info;
        
        }

        public CFileInfo Clone()
        {
            CFileInfo clone = new CFileInfo();

            clone.FileNameWithoutExtension = this.FileNameWithoutExtension;
            clone.Extension = this.Extension;
            clone.RelativePath = this.RelativePath;
            clone.AbsolutePath = this.AbsolutePath;

            return clone;
        }
        #endregion

        #region Private Method
        private CFileInfo()
        {
            FileNameWithoutExtension = "";
            RelativePath = "";
            AbsolutePath = "";
        }

        private bool Setting(string filename)
        {
            // 파일 이름 설정
            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(filename);

            // 확장자 설정
            string ext = Path.GetExtension(filename);
            Extension = CFileSystem.Inst.GetExt(ext);

            // 지원하는 확장자(Formatter가 있는지) 확인
            if(Extension == null)
            {
                // error log
                return false;
            }



            return true;
        }
        #endregion

        #region Variable
        public string RelativePath { get; private set; }        // 상대 경로
        public string AbsolutePath { get; private set; }        // 절대 경로
        public string KeyName
        {
            get
            {
                return RelativePath + Path.DirectorySeparatorChar
                    + FileNameWithoutExtension + Extension;
            }
        }
        public string FullFileName
        {
            get
            {
                return AbsolutePath + Path.DirectorySeparatorChar
                    + FileNameWithoutExtension + Extension;
            }
        }
        public string FileNameWithoutExtension { get; private set; }  // 확장자와 경로를 제외한 파일 이름
        public CFileExtension Extension { get; private set; } // 확장자
        public FileStatusType Status { get; private set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace Viewer.Render
{
    class CShader
    {
        #region Public Method
        public CShader(string filename)
        {
            Code = "";
            LoadFile(filename);
        }
        
        #endregion

        #region Private Method
        private CShader() { /* 기본 생성 금지 */ }
        private bool LoadFile(string filename)
        {
            try
            {
                StreamReader sr = new StreamReader(filename);
                Code = sr.ReadToEnd();
            }
            catch(Exception e)
            {
                MessageBox.Show("파일 로드 실패");
                return false;
            }

            return true;
        }
        #endregion

        #region Variable
        public string Code { get; private set; }
        #endregion


        

        
    }
}

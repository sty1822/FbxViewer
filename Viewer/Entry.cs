using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Viewer.Files;


namespace Viewer
{
    static class Entry
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Init
            CFileSystem.Inst.Init();

            // MainForm Start
            using (CMainForm main = new CMainForm())
            {
                main.Show();

                Utils.CWorldTime.Inst.StartTime(main.Progress, 60); // 60fps

                while(main.Created)
                {
                    // 이벤트 처리
                    Application.DoEvents();
                }
            }
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;

namespace Viewer
{
    public partial class CMainForm : Form
    {
        #region Public Method
        public CMainForm()
        {
            InitializeComponent();


            // Init Forms
            ViewWindow = new Forms.ViewForm();
            ViewWindow.Text = "[View]";
            ViewWindow.BackColor = Color.DarkGray;
            ViewWindow.ShowHint = DockState.Document;

            // Show
            ViewWindow.Show(dockPanel1);
        }

        public void Progress()
        {
            ViewWindow.Progress();
        }

        #endregion

        #region Private Method
        
        #endregion

        #region Members
        public Forms.ViewForm ViewWindow { get; private set; }
        #endregion

        private void CMainForm_Load(object sender, EventArgs e)
        {
            try
            {
                ViewWindow.Init();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                this.Close();
            }
            
        }
    }
}

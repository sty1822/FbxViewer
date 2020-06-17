namespace Viewer.Forms
{
    partial class ViewForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.viewer = new Viewer.CustomControl.ViewControl();
            this.SuspendLayout();
            // 
            // viewer
            // 
            this.viewer.BackColor = System.Drawing.Color.Black;
            this.viewer.BackgroundColor = System.Drawing.Color.DimGray;
            this.viewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewer.Location = new System.Drawing.Point(0, 0);
            this.viewer.Name = "viewer";
            this.viewer.Size = new System.Drawing.Size(506, 407);
            this.viewer.TabIndex = 0;
            this.viewer.VSync = false;
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 407);
            this.CloseButton = false;
            this.ControlBox = false;
            this.Controls.Add(this.viewer);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 1024);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(256, 256);
            this.Name = "ViewForm";
            this.ShowIcon = false;
            this.Text = "FloatForm";
            this.TopMost = true;
            this.ResumeLayout(false);

        }


        public Viewer.CustomControl.ViewControl viewer;

        #endregion
    }
}
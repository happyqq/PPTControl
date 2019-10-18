namespace PPTControl
{
    partial class frmSetting
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
            this.webSetting = new System.Windows.Forms.WebBrowser();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webSetting
            // 
            this.webSetting.Location = new System.Drawing.Point(1, 2);
            this.webSetting.MinimumSize = new System.Drawing.Size(20, 20);
            this.webSetting.Name = "webSetting";
            this.webSetting.Size = new System.Drawing.Size(329, 405);
            this.webSetting.TabIndex = 0;
            this.webSetting.Url = new System.Uri("http://webapi.happyqq.cn/pptcontrol/setting/save", System.UriKind.Absolute);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(131, 418);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "关闭";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 448);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.webSetting);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webSetting;
        private System.Windows.Forms.Button btnOK;
    }
}
namespace Myco
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtEncrypted = new System.Windows.Forms.TextBox();
            this.txtPlain = new System.Windows.Forms.TextBox();
            this.btnEncryptIt = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.chbxSaveEntry = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(17, 131);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(400, 23);
            this.txtSource.TabIndex = 0;
            // 
            // txtEncrypted
            // 
            this.txtEncrypted.Location = new System.Drawing.Point(17, 180);
            this.txtEncrypted.Multiline = true;
            this.txtEncrypted.Name = "txtEncrypted";
            this.txtEncrypted.Size = new System.Drawing.Size(400, 111);
            this.txtEncrypted.TabIndex = 1;
            // 
            // txtPlain
            // 
            this.txtPlain.Location = new System.Drawing.Point(447, 180);
            this.txtPlain.Multiline = true;
            this.txtPlain.Name = "txtPlain";
            this.txtPlain.Size = new System.Drawing.Size(400, 111);
            this.txtPlain.TabIndex = 2;
            // 
            // btnEncryptIt
            // 
            this.btnEncryptIt.Location = new System.Drawing.Point(17, 309);
            this.btnEncryptIt.Name = "btnEncryptIt";
            this.btnEncryptIt.Size = new System.Drawing.Size(182, 30);
            this.btnEncryptIt.TabIndex = 3;
            this.btnEncryptIt.Text = "Encrypt It";
            this.btnEncryptIt.UseVisualStyleBackColor = true;
            this.btnEncryptIt.Click += new System.EventHandler(this.btnEncryptIt_Click);
            // 
            // btnOptions
            // 
            this.btnOptions.Location = new System.Drawing.Point(12, 597);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(182, 31);
            this.btnOptions.TabIndex = 4;
            this.btnOptions.Text = "Set Options";
            this.btnOptions.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(17, 39);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(256, 23);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(17, 21);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "password";
            // 
            // chbxSaveEntry
            // 
            this.chbxSaveEntry.AutoSize = true;
            this.chbxSaveEntry.Location = new System.Drawing.Point(447, 131);
            this.chbxSaveEntry.Name = "chbxSaveEntry";
            this.chbxSaveEntry.Size = new System.Drawing.Size(84, 19);
            this.chbxSaveEntry.TabIndex = 7;
            this.chbxSaveEntry.Text = "save entry?";
            this.chbxSaveEntry.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 640);
            this.Controls.Add(this.chbxSaveEntry);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnOptions);
            this.Controls.Add(this.btnEncryptIt);
            this.Controls.Add(this.txtPlain);
            this.Controls.Add(this.txtEncrypted);
            this.Controls.Add(this.txtSource);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtEncrypted;
        private System.Windows.Forms.TextBox txtPlain;
        private System.Windows.Forms.Button btnEncryptIt;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.CheckBox chbxSaveEntry;
    }
}


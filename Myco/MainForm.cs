namespace Myco
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Domain.Configuration;
    using Zatoichi.Common.Infrastructure.Security;
    using System;
    using System.Text;

    public partial class MainForm : Form
    {
        private readonly IEncryptor encryptor;
        private readonly IOptions<Journal> journalOptions;

        public MainForm()
        {
            if (!this.DesignMode)
            {
                using var scope = Program.Host.Services.CreateScope();
                this.encryptor = scope.ServiceProvider.GetRequiredService<IEncryptor>();
                this.journalOptions = scope.ServiceProvider.GetRequiredService<IOptions<Journal>>();
            }
            InitializeComponent();
        }

        private void btnEncryptIt_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtPassword.Text))
            {
                this.lblPassword.ForeColor = Color.Red;
                this.lblPassword.Text = "You must supply a password first!";
                return;
            }

            this.encryptor.SetPassword(this.txtPassword.Text);
            this.lblPassword.ForeColor = this.txtEncrypted.ForeColor;
            this.lblPassword.Text = "password";

            this.txtEncrypted.Text = string.Empty;
            this.txtPlain.Text = string.Empty;

            if (!string.IsNullOrWhiteSpace(this.txtSource.Text))
            {
                var encrypted = this.encryptor.EncryptString(this.txtSource.Text);
                var decrypted = this.encryptor.DecryptString(encrypted);
                this.txtEncrypted.Text = encrypted;
                this.txtPlain.Text = decrypted;

                if (this.chbxSaveEntry.Checked)
                {
                    using var fs = File.Create($"{DateTime.UtcNow.Ticks}.json");
                    var msBytes = Encoding.UTF8.GetBytes(encrypted);
                    fs.Write(msBytes, 0, msBytes.Length);
                }
            }
        }
    }
}
namespace Myco
{
    using Microsoft.AspNetCore.Hosting;
    using Myco.Domain;
    using System;
    using System.Windows.Forms;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    internal static class Program
    {

        internal static IHost Host;

        [STAThread]
        private static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = new MycoHostBuilder()
                .ConfigureContainer<MycoContainerBuilder>((b, c) =>
                {
                    c.ConfigureServices(b.Configuration);
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json");
                    config.AddJsonFile($"secrets.json", true);
                });

            Host = host.Build();
            Application.Run(new MainForm());
        }
    }
}
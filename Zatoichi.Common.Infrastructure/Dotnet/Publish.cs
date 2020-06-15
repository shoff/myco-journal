namespace Zatoichi.Common.Infrastructure.Dotnet
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using ChaosMonkey.Guards;
    using Microsoft.Extensions.Logging;

    public class Publish : BaseCmd
    {
        public Publish(ILogger logger, IProgressFactory progressFactory)
            : base(logger, progressFactory.Build())
        {
        }

        public override string Command { get; protected set; } = "publish";

        public bool NoRestore { get; set; }
        public string Manifest { get; set; }
        public bool SelfContained { get; set; }
        public bool NoBuild { get; set; }

        public override void Execute(string workingDirectory)
        {
            Guard.IsNotNullOrWhitespace(workingDirectory, nameof(workingDirectory));
            if (!Directory.Exists(workingDirectory))
            {
                throw new ApplicationException($"{workingDirectory} does not exist.");
            }

            this.processStartInfo.WorkingDirectory = workingDirectory;
            this.processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            this.processStartInfo.CreateNoWindow = true;
            this.processStartInfo.RedirectStandardOutput = true;
            this.processStartInfo.UseShellExecute = false;
            this.processStartInfo.RedirectStandardInput = true;
            this.processStartInfo.Arguments = BuildArguments(workingDirectory);

            this.process = Process.Start(this.processStartInfo);
            this.process.OutputDataReceived += OnOutputDataReceived;
            this.process.BeginOutputReadLine();

            // do not reverse order of synchronous read to end and WaitForExit or deadlock
            // Wait for the process to end.  
            this.process.WaitForExit();
        }

        protected override string BuildArguments(string workingDirectory)
        {
            var sb = new StringBuilder();
            sb.Append(base.BuildArguments(workingDirectory));
            if (this.NoBuild)
            {
                sb.Append(" --no-build");
            }

            if (this.NoRestore)
            {
                sb.Append(" -- no-restore");
            }

            return sb.ToString();
        }
    }
}
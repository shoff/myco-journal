namespace Zatoichi.Common.Infrastructure.Dotnet
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using ChaosMonkey.Guards;
    using Microsoft.Extensions.Logging;

    public abstract class BaseCmd : IDisposable
    {
        protected readonly ILogger logger;
        protected readonly ProcessStartInfo processStartInfo;
        protected readonly IProgress<CmdProgress> progress;
        protected Process process;

        protected BaseCmd(ILogger logger, IProgress<CmdProgress> progress)
        {
            this.progress = Guard.IsNotNull(progress, nameof(progress));
            this.logger = Guard.IsNotNull(logger, nameof(logger));
            this.processStartInfo = new ProcessStartInfo("dotnet")
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                Verb = "build"
            };
        }

        public abstract string Command { get; protected set; }

        public bool NoDependencies { get; set; } = false;
        public string SolutionFile { get; set; }
        public string ProjectFile { get; set; }
        public string Framework { get; set; }
        public string Output { get; set; }
        public string Configuration { get; set; } = "Release";
        public Verbosity Verbosity { get; set; } = Verbosity.Detailed;
        public string Runtime { get; set; }
        public string VersionSuffix { get; set; }
        public bool Force { get; set; }

        public void Dispose()
        {
            this.process.OutputDataReceived -= OnOutputDataReceived;
            this.process.Dispose();
        }

        public abstract void Execute(string workingDirectory);

        protected virtual string BuildArguments(string workingDirectory)
        {
            var sb = new StringBuilder();
            sb.Append($" {this.Command}");

            if (string.IsNullOrWhiteSpace(this.SolutionFile) && string.IsNullOrWhiteSpace(this.ProjectFile))
            {
                var projFiles = Directory.GetFiles(workingDirectory, "*.csproj", SearchOption.TopDirectoryOnly);
                var slnFiles = Directory.GetFiles(workingDirectory, "*.sln", SearchOption.TopDirectoryOnly);
                if ((projFiles == null || projFiles.Length == 0) && (slnFiles == null || slnFiles.Length == 0))
                {
                    throw new ApplicationException(
                        "You must either specify a .csproj, .sln file by assigning the ProjectFile or SolutionFile property, or set your working directory to one that contains the .csproj, or .sln file to build.");
                }
            }
            // prefer sln file

            if (!string.IsNullOrWhiteSpace(this.SolutionFile))
            {
                sb.Append($" {this.SolutionFile}");
            }
            else if (!string.IsNullOrWhiteSpace(this.ProjectFile))
            {
                sb.Append($" {this.ProjectFile}");
            }

            sb.Append($" -c {this.Configuration ?? "Release"} ");
            if (!string.IsNullOrWhiteSpace(this.Framework))
            {
                sb.Append($" -f {this.Framework}");
            }

            if (!string.IsNullOrWhiteSpace(this.Output))
            {
                sb.Append($" -o {this.Output}");
            }

            sb.Append($" -v {this.Verbosity.AsString()}");

            if (this.Force)
            {
                sb.Append(" --force");
            }

            if (this.NoDependencies)
            {
                sb.Append(" --no-dependencies");
            }

            return sb.ToString();
        }


        protected void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.logger.LogInformation(e.Data);
            this.progress.Report(new CmdProgress(e.Data));
        }
    }
}
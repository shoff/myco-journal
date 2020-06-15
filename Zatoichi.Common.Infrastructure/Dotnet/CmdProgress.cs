namespace Zatoichi.Common.Infrastructure.Dotnet
{
    public class CmdProgress
    {
        public CmdProgress(string message)
        {
            this.Message = message;
        }

        public string Message { get; set; }
    }
}
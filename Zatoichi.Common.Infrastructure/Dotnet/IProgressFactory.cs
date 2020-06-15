namespace Zatoichi.Common.Infrastructure.Dotnet
{
    using System;

    public interface IProgressFactory
    {
        IProgress<CmdProgress> Build();
    }
}
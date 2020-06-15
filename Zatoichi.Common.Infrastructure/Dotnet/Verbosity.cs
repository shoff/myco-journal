namespace Zatoichi.Common.Infrastructure.Dotnet
{
    public enum Verbosity
    {
        Quiet, Minimal, Normal,
        Detailed, Diagnostic
    }

    public static class VerbosityExtensions
    {
        public static string AsString(this Verbosity verbosity)
        {
            switch (verbosity)
            {
                case Verbosity.Detailed:
                    return "Detailed";

                case Verbosity.Diagnostic:
                    return "Diagnostic";

                case Verbosity.Minimal:
                    return "Minimal";

                case Verbosity.Quiet:
                    return "Quiet";

                default:
                    return "Normal";
            }
        }
    }
}
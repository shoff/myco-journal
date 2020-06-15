namespace Zatoichi.Common.Infrastructure.Validation
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentValidation;

    [ExcludeFromCodeCoverage]
    public class BaseValidatedRequest<TValidator> where TValidator : IValidator, new()
    {
        public virtual void Validate()
        {
            var val = ((IValidator) new TValidator()).Validate(this);
            if (!val.IsValid)
            {
                var stringBuilder = new StringBuilder();
                foreach (var error in val.Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }

                throw new ValidationException(stringBuilder.ToString(), val.Errors);
            }
        }

        public virtual async Task ValidateAsync(CancellationToken cancellationToken)
        {
            var val = await ((IValidator) new TValidator()).ValidateAsync(this, cancellationToken);
            if (!val.IsValid)
            {
                var stringBuilder = new StringBuilder();
                foreach (var error in val.Errors)
                {
                    stringBuilder.AppendLine(error.ErrorMessage);
                }

                throw new ValidationException(stringBuilder.ToString(), val.Errors);
            }
        }
    }
}
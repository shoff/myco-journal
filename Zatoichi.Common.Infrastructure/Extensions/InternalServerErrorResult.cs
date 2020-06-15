namespace Zatoichi.Common.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc;

    public class InternalServerErrorResult : ObjectResult
    {
        public InternalServerErrorResult(object error)
            : base(error)
        {
            this.StatusCode = 500;
        }
    }
}
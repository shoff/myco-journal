namespace Zatoichi.Common.Infrastructure.Swagger
{
    using System;
    using System.Linq;
    using Extensions.Swagger;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class UnauthorizedOperationFilter : IOperationFilter
    {
        private const string UnauthorizedStatusCode = "401";

        private static readonly OpenApiResponse UnauthorizedResponse = new OpenApiResponse
        {
            Description = "Not authorized to view this resource."
        };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var authorizationRequirements = filterDescriptors.GetPolicyRequirements();
            if (!operation.Responses.ContainsKey(UnauthorizedStatusCode) &&
                authorizationRequirements.OfType<DenyAnonymousAuthorizationRequirement>().Any())
            {
                operation.Responses.Add(UnauthorizedStatusCode, UnauthorizedResponse);
            }
        }
    }
}
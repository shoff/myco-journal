namespace Zatoichi.Common.Infrastructure.Swagger
{
    using System;
    using System.Collections.Generic;
    using Extensions.Swagger;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Authorization.Infrastructure;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ForbiddenResponseOperationFilter : IOperationFilter
    {
        private const string ForbiddenStatusCode = "403";

        private static readonly OpenApiResponse ForbiddenResponse = new OpenApiResponse
        {
            Description = "You do not have the necessary permissions to access the resource."
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
            if (!operation.Responses.ContainsKey(ForbiddenStatusCode) &&
                HasAuthorizationRequirement(authorizationRequirements))
            {
                operation.Responses.Add(ForbiddenStatusCode, ForbiddenResponse);
            }
        }

        private static bool HasAuthorizationRequirement(
            IEnumerable<IAuthorizationRequirement> authorizationRequirements)
        {
            foreach (var authorizationRequirement in authorizationRequirements)
            {
                if (authorizationRequirement is ClaimsAuthorizationRequirement ||
                    authorizationRequirement is NameAuthorizationRequirement ||
                    authorizationRequirement is RolesAuthorizationRequirement ||
                    authorizationRequirement is AssertionRequirement)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
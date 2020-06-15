namespace Zatoichi.Common.Infrastructure.Swagger
{
    using System.Linq;
    using System.Reflection;
    using ChaosMonkey.Guards;
    using Extensions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class HideDocumentFilter : IDocumentFilter
    {
        private readonly IWebHostEnvironment environment;

        public HideDocumentFilter(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            Guard.IsNotNull(swaggerDoc, nameof(swaggerDoc));
            Guard.IsNotNull(context, nameof(context));
            if (!this.environment.IsDevelopment() && !this.environment.IsLocal())
            {
                foreach (var apiDescription in context.ApiDescriptions)
                {
                    var controllerActionDescriptor = (ControllerActionDescriptor) apiDescription.ActionDescriptor;
                    // If HideInSwaggerDoc attribute on Controller level then remove Tags from SwaggerDoc.
                    if (controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes<HideAttribute>().Any())
                    {
                        var tag = swaggerDoc.Tags.FirstOrDefault(x =>
                            x.Name == controllerActionDescriptor.ControllerName);
                        if (tag != null)
                        {
                            swaggerDoc.Tags.Remove(tag);
                        }
                    }

                    if (controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes<HideAttribute>().Any() ||
                        controllerActionDescriptor.MethodInfo.GetCustomAttributes<HideAttribute>().Any())
                    {
                        var key = $"/{apiDescription.RelativePath.TrimEnd('/')}";
                        var pathItem = swaggerDoc.Paths[key];
                        if (pathItem != null)
                        {
                            swaggerDoc.Paths.Remove(key);
                        }
                    }
                }
            }
        }
    }
}
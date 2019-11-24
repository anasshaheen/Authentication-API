using ApiAuth.API.Infrastructure.Abstraction.IServices;
using ApiAuth.API.Infrastructure.Constants;
using ApiAuth.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Services
{
    public class EmailTemplateGenerator : IEmailTemplateGenerator
    {
        private readonly ILogger<EmailTemplateGenerator> _logger;
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly SignatureEmailTemplateViewModel _signature;

        public EmailTemplateGenerator(
            ILogger<EmailTemplateGenerator> logger,
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider
            )
        {
            _logger = logger;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _signature = new SignatureEmailTemplateViewModel("Anas Shaheen", "GP Taxi");
        }

        public async Task<string> RenderActionTemplate(string title, List<string> body, UrlEmailTemplateViewModel url = null)
        {
            _logger.LogInformation("Start generating the email template");
            var actionEmailTemplate = new ActionEmailTemplateViewModel
            {
                Body = body,
                Signature = _signature,
                Title = title,
                Url = url
            };

            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var writer = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(actionContext, EmailTemplates.ActionView, false);
                if (viewResult == null)
                {
                    throw new ArgumentNullException($"{EmailTemplates.ActionView} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = actionEmailTemplate
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    writer,
                    new HtmlHelperOptions()
                );
                await viewResult.View.RenderAsync(viewContext);

                _logger.LogInformation("Email template generated!");

                return writer.ToString();
            }
        }
    }
}

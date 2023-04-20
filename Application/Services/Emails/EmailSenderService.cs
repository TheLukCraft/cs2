using Application.Interfaces;
using Domain.Enums;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Application.Services.Emails
{
    public class EmailSenderService : IEmailSenderService
    {
        private const string TemplatePath = "Application.Services.Emails.Templates.{0}.cshtml";
        private readonly IFluentEmail email;
        private readonly ILogger<EmailSenderService> logger;

        public EmailSenderService(IFluentEmail email, ILogger<EmailSenderService> logger)
        {
            this.logger = logger;
            this.email = email;
        }

        public async Task<bool> Send(string to, string subject, EmailTemplate template, object model)
        {
            var result = await email.To(to)
                .Subject(subject)
                .UsingTemplateFromEmbedded(string.Format(TemplatePath, template), ToExpando(model), GetType().Assembly)
                .SendAsync();

            if (!result.Successful)
                logger.LogError("Fail to send an e-mail.\n{Errors}", string.Join(Environment.NewLine, result.ErrorMessages));

            return result.Successful;
        }

        #region HelperMethods

        private static ExpandoObject ToExpando(object model)
        {
            if (model is ExpandoObject exp)
                return exp;

            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var propertyDescription in model.GetType().GetTypeInfo().GetProperties())
            {
                var obj = propertyDescription.GetValue(model);

                if (obj != null && IsAnonymousType(obj.GetType()))
                    obj = ToExpando(obj);

                expando.Add(propertyDescription.Name, obj);
            }
            return (ExpandoObject)expando;
        }

        private static bool IsAnonymousType(Type type)
        {
            bool hasCompilerGeneratedAttribute = type.GetTypeInfo()
                .GetCustomAttributes(typeof(CompilerGeneratedAttribute), false)
                .Any();

            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }

        #endregion HelperMethods
    }
}
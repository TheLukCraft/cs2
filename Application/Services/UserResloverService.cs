using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class UserResloverService
    {
        private readonly IHttpContextAccessor context;

        public UserResloverService(IHttpContextAccessor context)
        {
            this.context = context;
        }

        public string GetUser()
        {
            return context.HttpContext.User?.Identities?.FirstOrDefault()?.Name;
        }
    }
}
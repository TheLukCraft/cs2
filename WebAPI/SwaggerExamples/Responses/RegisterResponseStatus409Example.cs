using Swashbuckle.AspNetCore.Filters;
using WebAPI.Wrappers;

namespace WebAPI.SwaggerExamples.Responses
{
    public class RegisterResponseStatus409Example : IExamplesProvider<RegisterResponseStatus409>
    {
        public RegisterResponseStatus409 GetExamples()
        {
            return new RegisterResponseStatus409
            {
                Succeeded = true,
                Message = "User already exists!"
            };
        }
    }

    public class RegisterResponseStatus409 : Response
    {
    }
}
using Swashbuckle.AspNetCore.Filters;
using WebAPI.Models;

namespace WebAPI.Swagger.Requests
{
    public class RegisterModelExample : IExamplesProvider<RegisterModel>
    {
        public RegisterModel GetExamples()
        {
            return new RegisterModel
            {
                UserName = "yourUniqueName",
                Email = "YourEmailAddress@example.com",
                Password = "Pa$$word123!",
            };
        }
    }
}
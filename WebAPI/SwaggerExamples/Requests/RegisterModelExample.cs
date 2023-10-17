using Swashbuckle.AspNetCore.Filters;
using WebAPI.Models;

namespace WebAPI.SwaggerExamples.Requests
{
    public class RegisterModelExample : IExamplesProvider<RegisterModel>
    {
        public RegisterModel GetExamples()
        {
            return new RegisterModel
            {
                UserName = "yourUniqueName",
                Email = "yourEmailAdress@example.com",
                Password = "Komenda1@"
            };
        }
    }
}
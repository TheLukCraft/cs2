using CSGO2.Contracts.Requests;
using CSGO2.Contracts.Responses;
using Refit;

namespace CSGO2.Sdk.Sample
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var cachedToken = string.Empty;
            var identityApi = RestService.For<IIdentityAPI>("https://localhost:7138/");
            var csgoApi = RestService.For<ICSGO2API>("https://localhost:7138/", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(cachedToken)
            });

            var register = await identityApi.RegisterAsync(new RegisterModel()
            {
                Email = "sdkAcc@gmail.com",
                UserName = "sdkAcc",
                Password = "Pa$$word123!"
            });

            var login = await identityApi.LoginAsync(new LoginModel()
            {
                UserName = "sdkAcc",
                Password = "Pa$$word123!"
            });

            cachedToken = login.Content.Token;

            var createdPost = await csgoApi.CreatePostAsync(new CreatePostDto
            {
                Title = "Post sdk",
                Content = "treść sdk"
            });

            var retrievedPost = await csgoApi.GetPostAsync(createdPost.Content.Data.Id);

            await csgoApi.UpdatePostAsync(new UpdatePostDto
            {
                Id = retrievedPost.Content.Data.Id,
                Content = "nowa treść sdk"
            });

            await csgoApi.DeletePostAsync(retrievedPost.Content.Data.Id);
        }
    }
}
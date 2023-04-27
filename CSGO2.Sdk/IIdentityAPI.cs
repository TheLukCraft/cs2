using CSGO2.Contracts;
using CSGO2.Contracts.Requests;
using CSGO2.Contracts.Responses;
using Refit;

namespace CSGO2.Sdk
{
    public interface IIdentityAPI
    {
        [Post("/identity/register")]
        Task<ApiResponse<Response>> RegisterAsync([Body] RegisterModel register);

        [Post("/identity/registerAdmin")]
        Task<ApiResponse<Response>> RegisterAdminAsync([Body] RegisterModel register);

        [Post("/identity/login")]
        Task<ApiResponse<AuthSuccessResponse>> LoginAsync([Body] LoginModel login);
    }
}
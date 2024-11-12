using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace UsersInfrastructure.HttpClients.Handler
{
    public class AuthorizationHeaderHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationHeader.Split(" ").Last());
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}

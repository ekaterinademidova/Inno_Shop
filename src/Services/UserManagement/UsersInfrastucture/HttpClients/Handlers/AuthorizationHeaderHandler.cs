using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace UsersInfrastructure.HttpClients.Handler
{
    public class AuthorizationHeaderHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // get the Authorization header from the current request
            var authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                // set the Authorization header for requesting another microservice
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationHeader.Split(" ").Last());
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}

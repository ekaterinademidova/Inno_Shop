using Microsoft.AspNetCore.Mvc;
using UsersApplication.Users.Commands.ResetPassword;

namespace UsersAPI.Endpoints.Users
{
    public record TokenParam(Guid Token);
    public record PasswordParam(string Password);
    public record ResetPasswordRequest(Guid Token, string NewPassword);
    public record ResetPasswordResponse(bool IsSuccess);
    public class ResetPassword : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/users/reset-password", ([AsParameters] Guid token, ISender sender) =>
            {
                return Results.Ok();
            })
            .WithName("ResetPassword GET")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Reset Password GET")
            .WithDescription("Reset Password GET");

            app.MapPost("/users/reset-password", async (
                [AsParameters] TokenParam token, 
                [FromBody] PasswordParam password, 
                ISender sender) =>
            {
                var request = new ResetPasswordRequest(token.Token, password.Password);

                var command = request.Adapt<ResetPasswordCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<ResetPasswordResponse>();

                return Results.Ok(response);
            })
            .WithName("ResetPassword POST")
            .Produces<ResetPasswordResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Reset Password POST")
            .WithDescription("Reset Password POST");
        }
    }
}

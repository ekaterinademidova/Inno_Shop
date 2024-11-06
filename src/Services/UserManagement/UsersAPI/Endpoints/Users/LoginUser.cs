using UsersApplication.ValueObjects;
using UsersApplication.Users.Commands.LoginUser;

namespace UsersAPI.Endpoints.Users
{
    public record LoginUserRequest(string Email, string Password);
    public record LoginUserResponse(JwtToken Token);
    public class LoginUser : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("users/login", async (LoginUserRequest request, ISender sender) =>
            {
                var command = request.Adapt<LoginUserCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<LoginUserResponse>();
                return Results.Ok(response);
            })
            .WithName("LoginUser")
            .Produces<LoginUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Login User")
            .WithDescription("Login User");
        }
    }
}

using UsersApplication.Users.Commands.SeekPasswordReset;

namespace UsersAPI.Endpoints.Users
{
    public record SeekPasswordResetRequest(string Email);
    public record SeekPasswordResetResponse(bool IsSuccess);
    public class SeekPasswordReset : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/users/seek-password-reset", async (SeekPasswordResetRequest request, ISender sender) =>
            {
                var command = request.Adapt<SeekPasswordResetCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<SeekPasswordResetResponse>();

                return Results.Ok(response);
            })
            .WithName("SeekPasswordReset")
            .Produces<SeekPasswordResetResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Seek Password Reset")
            .WithDescription("Seek Password Reset");
        }
    }
}

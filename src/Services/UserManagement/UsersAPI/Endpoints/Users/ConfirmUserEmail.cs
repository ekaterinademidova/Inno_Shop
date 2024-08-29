using UsersApplication.Users.Commands.ConfirmUserEmail;

namespace UsersAPI.Endpoints.Users
{
    public record ConfirmUserEmailRequest(Guid Token);
    public record ConfirmUserEmailResponse(bool IsSuccess);
    public class ConfirmUserEmail : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/users/confirm-email", async ([AsParameters] ConfirmUserEmailRequest request, ISender sender) =>
            {
                var command = request.Adapt<ConfirmUserEmailCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<ConfirmUserEmailResponse>();

                return Results.Ok(response);

            })
            .WithName("ConfirmUserEmail")
            .Produces<ConfirmUserEmailResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Confirm User Email")
            .WithDescription("Confirm User Email");
        }
    }
}

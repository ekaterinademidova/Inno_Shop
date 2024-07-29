using UsersApplication.Users.Commands.DeleteUser;

namespace UsersAPI.Endpoints
{
    // Accepts the order ID as a parameter
    // Maps the request to a DeleteOrderCommand
    // Uses MediatR to send the command to the corresponding handler
    // Returns a success or not found response based on the outcome

    //public record DeleteUserRequest(Guid Id);
    public record DeleteUserResponse(bool IsSuccess);
    public class DeleteUser : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("users/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteUserCommand(id));
                var response = result.Adapt<DeleteUserResponse>();
                return Results.Ok(response);
            })
            .WithName("DeleteUser")
            .Produces<DeleteUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete User")
            .WithDescription("Delete User");
        }
    }
}

namespace UsersAPI.Users.UpdateUser
{
    public record UpdateUserRequest(Guid Id, string Name);
    public record UpdateUserResponse(bool IsSuccess);

    public class UpdateUserEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/users",
                async (UpdateUserRequest request, ISender sender) =>
                {
                    var command = request.Adapt<UpdateUserCommand>();
                    var result = await sender.Send(command);
                    var response = result.Adapt<UpdateUserResponse>();

                    return Results.Ok(response);
                })
            .WithName("UpdateUser")
            .Produces<UpdateUserResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update User")
            .WithDescription("Update User");
        }
    }
}

using UsersApplication.Users.Commands.CreateUser;

namespace UsersAPI.Endpoints.Users
{
    // Accepts a CreateUserRequest object
    // Maps the request to a CreateUserCommand
    // Uses MediatR to send the command to the corresponding handler
    // Returns a response with the created user's ID

    public record CreateUserRequest(UserDto User);
    public record CreateUserResponse(Guid Id);
    public class CreateUser : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/users", async (CreateUserRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateUserCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateUserResponse>();

                return Results.Created($"/users/{response.Id}", response);

            })
            //.RequireAuthorization() 
            .WithName("CreateUser")
            .Produces<CreateUserResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create User")
            .WithDescription("Create User");
        }
    }
}

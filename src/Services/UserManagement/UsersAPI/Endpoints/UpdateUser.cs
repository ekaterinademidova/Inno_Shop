﻿using UsersApplication.Users.Commands.UpdateUser;

namespace UsersAPI.Endpoints
{
    // Accepts a UpdateUserRequest object
    // Maps the request to a UpdateUserCommand
    // Uses MediatR to send the command to the corresponding handler
    // Returns a success or error response based on the outcome

    public record UpdateUserRequest(UserDto User);
    public record UpdateUserResponse(bool IsSuccess);
    public class UpdateUser : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/users", async (UpdateUserRequest request, ISender sender) =>
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
﻿using UsersApplication.Users.Queries.GetUserById;

namespace UsersAPI.Endpoints.Users
{
    //public record GetUserByIdRequest();
    public record GetUserByIdResponse(UserDto User);
    public class GetUserById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/users/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetUserByIdQuery(id));
                var response = result.Adapt<GetUserByIdResponse>();

                return Results.Ok(response);
            })
            .WithName("GetUserById")
            .Produces<GetUserByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get User By Id")
            .WithDescription("Get User By Id");
        }
    }
}

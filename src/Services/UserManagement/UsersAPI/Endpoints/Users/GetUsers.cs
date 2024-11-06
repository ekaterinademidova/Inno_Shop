using BuildingBlocks.Pagination;
using UsersApplication.Users.Queries.GetUsers;

namespace UsersAPI.Endpoints.Users
{
    // Accepts pagination parameters
    // Constructs a GetUsersQuery with these parameters
    // Retrieves the data and returns it in a paginated format

    //public record GetUsersRequest(PaginationRequest PaginationRequest);
    public record GetUsersResponse(PaginatedResult<UserDto> Users);
    public class GetUsers : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/users", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetUsersQuery(request));
                var response = result.Adapt<GetUsersResponse>();

                return Results.Ok(response);
            })
            .WithName("GetUsers")
            .Produces<GetUsersResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Users")
            .WithDescription("Get Users");
        }
    }
}

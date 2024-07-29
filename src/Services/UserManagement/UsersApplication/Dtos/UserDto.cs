namespace UsersApplication.Dtos
{
    public record UserDto(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string Password,
        UserRole Role,
        UserStatus Status,
        List<ProductDto> CreatedProducts);
}

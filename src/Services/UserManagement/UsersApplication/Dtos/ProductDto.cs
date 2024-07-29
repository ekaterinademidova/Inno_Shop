﻿namespace UsersApplication.Dtos
{
    public record ProductDto(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        int Quantity,
        Guid CreatedByUserId );
}

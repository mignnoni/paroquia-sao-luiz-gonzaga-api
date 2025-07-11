using System;
using BuildingBlocks.Domain.ValueObjects;

namespace BuildingBlocks.Application;

public class AddressRequest
{
    public string? Country { get; set; } = string.Empty;
    public string? State { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? District { get; set; } = string.Empty;
    public string? Street { get; set; } = string.Empty;
    public string? Number { get; set; } = string.Empty;
    public string? ZipCode { get; set; } = string.Empty;
    public string? Complement { get; set; } = string.Empty;

    public Address ToAddress()
    {
        return Address.Create(
            Country ?? string.Empty,
            State ?? string.Empty,
            City ?? string.Empty,
            District ?? string.Empty,
            Street ?? string.Empty,
            Number ?? string.Empty,
            ZipCode ?? string.Empty,
            Complement ?? string.Empty);
    }

}

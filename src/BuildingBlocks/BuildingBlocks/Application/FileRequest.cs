namespace BuildingBlocks.Application;

public sealed record FileRequest(
    string Name,
    string ContentType,
    string Extension,
    Stream FileStream);

using System.Threading.RateLimiting;
using Ardalis.Result.AspNetCore;
using BuildingBlocks.Infrastructure.RabbitMQInfra;
using BuildingBlocks.Persistence.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Modules.IdentityProvider.Infrastructure;
using Modules.Notification.Infrastructure;
using Modules.ParishManagement.Application.PendingMembers.AddPendingMember;
using Modules.ParishManagement.Application.PendingMembers.GetPendingMembers;
using Modules.ParishManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureOptions<ConnectionStringSetup>()
    .AddRabbitMQ()
    .AddIdentityProviderModule(builder.Configuration)
    .AddParishManagementModule()
    .AddNotificationModule();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/pendingMembers", async (AddPendingMemberCommand command, ISender sender) =>
{
    var result = await sender.Send(command);

    return Results.Ok(result.ToMinimalApiResult());
});

app.MapGet("/pendingMembers", async ([FromQuery] int pageIndex, [FromQuery] int pageSize, ISender sender) =>
{
    var result = await sender.Send(new GetPendingMembersQuery(pageIndex, pageSize));
    return Results.Ok(result.ToMinimalApiResult());
});

app.Run();

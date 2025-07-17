using System;
using BuildingBlocks.Domain;

namespace Modules.ParishManagement.Domain.Members;

public sealed record MemberId(Guid Value) : IEntityId;
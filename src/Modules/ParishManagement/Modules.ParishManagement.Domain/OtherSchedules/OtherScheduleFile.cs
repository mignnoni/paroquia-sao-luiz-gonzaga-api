using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.ValueObjects;

namespace Modules.ParishManagement.Domain.OtherSchedules;

public class OtherScheduleFile : Entity<Guid>
{
    private OtherScheduleFile(Guid id, OtherScheduleId otherScheduleId, UploadInfo uploadInfo) : base(id)
    {
        OtherScheduleId = otherScheduleId;
        UploadInfo = uploadInfo;
    }

    // EF Core
    private OtherScheduleFile() { }

    public OtherScheduleId OtherScheduleId { get; private set; }
    public UploadInfo UploadInfo { get; private set; }

    public static OtherScheduleFile Create(Guid id, OtherScheduleId otherScheduleId, UploadInfo uploadInfo)
    {
        return new OtherScheduleFile(id, otherScheduleId, uploadInfo);
    }
}

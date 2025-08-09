using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Modules.ParishManagement.Infrastructure.Services.AWS;

public class S3ServiceOptions
{
    public string BucketName { get; set; }
}

public class S3ServiceOptionsSetup(
    IConfiguration configuration) : IConfigureOptions<S3ServiceOptions>
{
    private const string SectionName = "S3ServiceOptions";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(S3ServiceOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}


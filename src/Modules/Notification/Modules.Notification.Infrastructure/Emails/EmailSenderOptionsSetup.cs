using BuildingBlocks.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Modules.Notification.Infrastructure.Emails;

public class EmailSenderOptionsSetup(
    IConfiguration _configuration) : IConfigureOptions<EmailSenderOptions>
{
    public void Configure(EmailSenderOptions options)
    {
        _configuration.GetSection(EmailSenderOptions.SectionName).Bind(options);

        options.Port = SettingsUtils.GetEnvironmentValueOrDefault("EMAIL_PORT", options.Port);
        options.Host = SettingsUtils.GetEnvironmentValueOrDefault("EMAIL_HOST", options.Host);
        options.FromName = SettingsUtils.GetEnvironmentValueOrDefault("EMAIL_FROM_NAME", options.FromName);
        options.FromEmail = SettingsUtils.GetEnvironmentValueOrDefault("EMAIL_FROM_ADDRESS", options.FromEmail);
        options.ApiKey = SettingsUtils.GetEnvironmentValueOrDefault("EMAIL_API_KEY", options.ApiKey);
        options.BaseUrlFront = SettingsUtils.GetEnvironmentValueOrDefault("FRONT_BASE_URL", options.BaseUrlFront);
    }
}

public class EmailSenderOptions
{
    public const string SectionName = "EmailOptions";

    public int Port { get; set; }
    public string Host { get; set; }
    public string FromName { get; set; }
    public string FromEmail { get; set; }
    public string ApiKey { get; set; }
    public string BaseUrlFront { get; set; }
}

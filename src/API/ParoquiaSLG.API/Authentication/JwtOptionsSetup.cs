using BuildingBlocks.Infrastructure;
using Microsoft.Extensions.Options;

namespace ParoquiaSLG.API.Authentication
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private const string SectionName = "Jwt";
        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);

            options.SecretKey = GetEnvironmentValueOrDefault("JWT_KEY", options.SecretKey);
            options.Issuer = GetEnvironmentValueOrDefault("JWT_ISSUER", options.Issuer);
            options.Audience = GetEnvironmentValueOrDefault("JWT_AUDIENCE", options.Audience);
        }

        private static string GetEnvironmentValueOrDefault(string key, string defaultValue)
            => Environment.GetEnvironmentVariable(key) ?? defaultValue;
    }
}

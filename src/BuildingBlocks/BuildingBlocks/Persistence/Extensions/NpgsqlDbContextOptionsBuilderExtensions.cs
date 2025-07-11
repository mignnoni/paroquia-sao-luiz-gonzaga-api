using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace BuildingBlocks.Persistence.Extensions;

public static class NpgsqlDbContextOptionsBuilderExtensions
{
    public static NpgsqlDbContextOptionsBuilder WithMigrationHistoryTableInSchema(
        this NpgsqlDbContextOptionsBuilder dbContextOptionsBuilder,
        string schema) =>
        dbContextOptionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", schema);
}

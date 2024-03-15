using FluentMigrator;
using FluentMigrator.Snowflake;

namespace WebApplication10.Migrations;

[Migration(1)]
public class M0000_InitialMigration : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("Id").AsInt64().Identity().NotNullable().PrimaryKey()
            .WithColumn("Username").AsString().Unique().NotNullable()
            .WithColumn("HashedPassword").AsString().NotNullable()
            .WithColumn("AccessToken").AsString().Nullable()
            .WithColumn("RefreshToken").AsString().Nullable();
    }

    public override void Down()
    {
        Delete.Table("users");
    }
}
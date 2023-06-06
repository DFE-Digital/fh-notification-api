using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.Notification.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateIntialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SentNotifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TokenValues",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentNotificationId = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokenValues_SentNotifications_SentNotificationId",
                        column: x => x.SentNotificationId,
                        principalTable: "SentNotifications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenValues_SentNotificationId",
                table: "TokenValues",
                column: "SentNotificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokenValues");

            migrationBuilder.DropTable(
                name: "SentNotifications");
        }
    }
}

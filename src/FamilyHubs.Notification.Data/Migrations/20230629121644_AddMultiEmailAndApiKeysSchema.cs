using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyHubs.Notification.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiEmailAndApiKeysSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientEmail",
                table: "SentNotifications");

            migrationBuilder.AddColumn<int>(
                name: "ApiKeyType",
                table: "SentNotifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Notified",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentNotificationId = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notified", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notified_SentNotifications_SentNotificationId",
                        column: x => x.SentNotificationId,
                        principalTable: "SentNotifications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notified_SentNotificationId",
                table: "Notified",
                column: "SentNotificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notified");

            migrationBuilder.DropColumn(
                name: "ApiKeyType",
                table: "SentNotifications");

            migrationBuilder.AddColumn<string>(
                name: "RecipientEmail",
                table: "SentNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

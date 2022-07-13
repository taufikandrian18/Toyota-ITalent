using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class pushNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PushNotificationCategories",
                columns: table => new
                {
                    Guid = table.Column<string>(unicode: false, maxLength: 64, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushNotificationCategories", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "PushNotifications",
                columns: table => new
                {
                    Guid = table.Column<string>(unicode: false, maxLength: 64, nullable: false),
                    SenderEmployeeId = table.Column<string>(maxLength: 64, nullable: true),
                    NotificationCategoryId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    SendDate = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushNotifications", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Push_Category",
                        column: x => x.NotificationCategoryId,
                        principalTable: "PushNotificationCategories",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Push_Employee_Sender",
                        column: x => x.SenderEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PushNotificationRecipients",
                columns: table => new
                {
                    Guid = table.Column<string>(unicode: false, maxLength: 64, nullable: false),
                    RecipientEmployeeId = table.Column<string>(maxLength: 64, nullable: true),
                    NotificationId = table.Column<string>(nullable: true),
                    ReadDate = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushNotificationRecipients", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Push",
                        column: x => x.NotificationId,
                        principalTable: "PushNotifications",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Push_Employee_Receiver",
                        column: x => x.RecipientEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PushNotificationRecipients_NotificationId",
                table: "PushNotificationRecipients",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PushNotificationRecipients_RecipientEmployeeId",
                table: "PushNotificationRecipients",
                column: "RecipientEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PushNotifications_NotificationCategoryId",
                table: "PushNotifications",
                column: "NotificationCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PushNotifications_SenderEmployeeId",
                table: "PushNotifications",
                column: "SenderEmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PushNotificationRecipients");

            migrationBuilder.DropTable(
                name: "PushNotifications");

            migrationBuilder.DropTable(
                name: "PushNotificationCategories");
        }
    }
}

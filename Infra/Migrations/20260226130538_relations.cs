using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantSubscriptions_SubscriptionPlans_SubscriptionPlanId",
                table: "TenantSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_PlanFeatures_SubscriptionPlanId_FeatureId",
                table: "PlanFeatures");

            migrationBuilder.RenameColumn(
                name: "IsEnabledGlobal",
                table: "Features",
                newName: "IsEnabledForSpecificTenant");

            migrationBuilder.CreateTable(
                name: "TenantFeature",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    FeatureId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantFeature", x => new { x.TenantId, x.FeatureId });
                    table.ForeignKey(
                        name: "FK_TenantFeature_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenantFeature_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantSubscriptions_TenantId",
                table: "TenantSubscriptions",
                column: "TenantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TenantFeature_FeatureId",
                table: "TenantFeature",
                column: "FeatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSubscriptions_SubscriptionPlans_SubscriptionPlanId",
                table: "TenantSubscriptions",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TenantSubscriptions_SubscriptionPlans_SubscriptionPlanId",
                table: "TenantSubscriptions");

            migrationBuilder.DropTable(
                name: "TenantFeature");

            migrationBuilder.DropIndex(
                name: "IX_TenantSubscriptions_TenantId",
                table: "TenantSubscriptions");

            migrationBuilder.RenameColumn(
                name: "IsEnabledForSpecificTenant",
                table: "Features",
                newName: "IsEnabledGlobal");

            migrationBuilder.CreateIndex(
                name: "IX_PlanFeatures_SubscriptionPlanId_FeatureId",
                table: "PlanFeatures",
                columns: ["SubscriptionPlanId", "FeatureId"],
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TenantSubscriptions_SubscriptionPlans_SubscriptionPlanId",
                table: "TenantSubscriptions",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

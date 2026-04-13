using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class SeedPlansFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Features",
                columns: new[] { "Id", "CreatedAt", "Description", "FeatureCode", "FeatureName", "IsEnabledGlobal" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "FEAT_AI_INSIGHTS", "AI Insights", true },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "FEAT_ADV_REPORTS", "Advanced Reporting", true },
                    { new Guid("f3333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "FEAT_MULTI_USER", "MultiUser", true }
                });

            migrationBuilder.InsertData(
                table: "PlanFeatures",
                columns: new[] { "FeatureId", "SubscriptionPlanId", "IsEnabledForPlan" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("c3333333-3333-3333-3333-333333333333"), true },
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("d4444444-4444-4444-4444-444444444444"), true },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), new Guid("d4444444-4444-4444-4444-444444444444"), true },
                    { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("e5555555-5555-5555-5555-555555555555"), true },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), new Guid("e5555555-5555-5555-5555-555555555555"), true },
                    { new Guid("f3333333-3333-3333-3333-333333333333"), new Guid("e5555555-5555-5555-5555-555555555555"), true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumns: new[] { "FeatureId", "SubscriptionPlanId" },
                keyValues: new object[] { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("c3333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumns: new[] { "FeatureId", "SubscriptionPlanId" },
                keyValues: new object[] { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("d4444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumns: new[] { "FeatureId", "SubscriptionPlanId" },
                keyValues: new object[] { new Guid("b2222222-2222-2222-2222-222222222222"), new Guid("d4444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumns: new[] { "FeatureId", "SubscriptionPlanId" },
                keyValues: new object[] { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("e5555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumns: new[] { "FeatureId", "SubscriptionPlanId" },
                keyValues: new object[] { new Guid("b2222222-2222-2222-2222-222222222222"), new Guid("e5555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "PlanFeatures",
                keyColumns: new[] { "FeatureId", "SubscriptionPlanId" },
                keyValues: new object[] { new Guid("f3333333-3333-3333-3333-333333333333"), new Guid("e5555555-5555-5555-5555-555555555555") });

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: new Guid("f3333333-3333-3333-3333-333333333333"));
        }
    }
}
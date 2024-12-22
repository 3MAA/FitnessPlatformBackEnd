using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessPlatform.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.CreateTable(
                name: "NutritionalPlan",
                columns: table => new
                {
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    PlanType = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    MealType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    FoodStuff = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Grammage = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    FoodStuffCalories = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    MealCalories = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    RegistrationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Nutritio__755C22B727179788", x => x.PlanId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    UserPassword = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    UserWeight = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    UserHeight = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4C20CDA3AF", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    WorkoutId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutDescription = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    WorkoutType = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DifficultyLevel = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    WorkoutDuration = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    CaloriesBurned = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    ContentPath = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Workouts__E1C42A01CE1317B5", x => x.WorkoutId);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    DiscountId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    GrantDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Discount__E43F6D96989FDC9D", x => x.DiscountId);
                    table.ForeignKey(
                        name: "FK__Discounts__UserI__4316F928",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Objectives",
                columns: table => new
                {
                    ObjectiveId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    ObjectiveType = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TargetValue = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Progress = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Deadline = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Objectiv__8C5633AD6112FF2E", x => x.ObjectiveId);
                    table.ForeignKey(
                        name: "FK__Objective__UserI__3D5E1FD2",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    SubscriptionType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    Term = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Subscrip__9A2B249DFA8EF3C6", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK__Subscript__UserI__403A8C7D",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlan",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: true),
                    PlanPrice = table.Column<decimal>(type: "decimal(8,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__Subscript__PlanI__45F365D3",
                        column: x => x.PlanId,
                        principalTable: "NutritionalPlan",
                        principalColumn: "PlanId");
                    table.ForeignKey(
                        name: "FK__Subscript__Subsc__44FF419A",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "SubscriptionId");
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionWorkout",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: true),
                    WorkoutId = table.Column<int>(type: "int", nullable: true),
                    WorkoutPrice = table.Column<decimal>(type: "decimal(8,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK__Subscript__Subsc__47DBAE45",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "SubscriptionId");
                    table.ForeignKey(
                        name: "FK__Subscript__Worko__48CFD27E",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "WorkoutId");
                });*/

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_UserId",
                table: "Discounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Objectives_UserId",
                table: "Objectives",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlan_PlanId",
                table: "SubscriptionPlan",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlan_SubscriptionId",
                table: "SubscriptionPlan",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionWorkout_SubscriptionId",
                table: "SubscriptionWorkout",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionWorkout_WorkoutId",
                table: "SubscriptionWorkout",
                column: "WorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Objectives");

            migrationBuilder.DropTable(
                name: "SubscriptionPlan");

            migrationBuilder.DropTable(
                name: "SubscriptionWorkout");

            migrationBuilder.DropTable(
                name: "NutritionalPlan");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

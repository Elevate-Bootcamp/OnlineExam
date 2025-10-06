using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineExam.Migrations
{
    /// <inheritdoc />
    public partial class editDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "SelectedChoiceIds",
                table: "UserAnswers");

            migrationBuilder.AddColumn<int>(
                name: "AttemptNumber",
                table: "UserExamAttempts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "UserExamAttempts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choices_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSelectedChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAnswerId = table.Column<int>(type: "int", nullable: false),
                    ChoiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSelectedChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSelectedChoices_Choices_ChoiceId",
                        column: x => x.ChoiceId,
                        principalTable: "Choices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSelectedChoices_UserAnswers_UserAnswerId",
                        column: x => x.UserAnswerId,
                        principalTable: "UserAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Choices_QuestionId",
                table: "Choices",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedChoices_ChoiceId",
                table: "UserSelectedChoices",
                column: "ChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSelectedChoices_UserAnswerId",
                table: "UserSelectedChoices",
                column: "UserAnswerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_UserExamAttempts_QuestionId",
                table: "UserAnswers",
                column: "QuestionId",
                principalTable: "UserExamAttempts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_UserExamAttempts_QuestionId",
                table: "UserAnswers");

            migrationBuilder.DropTable(
                name: "UserSelectedChoices");

            migrationBuilder.DropTable(
                name: "Choices");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "AttemptNumber",
                table: "UserExamAttempts");

            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "UserExamAttempts");

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "UserAnswers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SelectedChoiceIds",
                table: "UserAnswers",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

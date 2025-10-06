using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineExam.Migrations
{
    /// <inheritdoc />
    public partial class FixUserAnswerRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_UserExamAttempts_QuestionId",
                table: "UserAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers");

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
    }
}

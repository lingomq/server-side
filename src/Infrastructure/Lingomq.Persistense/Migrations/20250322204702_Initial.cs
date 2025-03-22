using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lingomq.Persistense.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_credentials",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    password_salt = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_credentials", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    x = table.Column<int>(type: "integer", nullable: false),
                    y = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "word_infos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    word = table.Column<string>(type: "text", nullable: false),
                    transcription = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    language_value = table.Column<string>(type: "text", nullable: false),
                    language_code = table.Column<string>(type: "text", nullable: false),
                    language_sub_code = table.Column<string>(type: "text", nullable: false),
                    thematics_category = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_word_infos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "authorization_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    auth_type = table.Column<int>(type: "integer", nullable: false),
                    auth_value = table.Column<string>(type: "text", nullable: false),
                    user_credentials_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authorization_type", x => x.id);
                    table.ForeignKey(
                        name: "fk_authorization_type_user_credentials_user_credentials_id",
                        column: x => x.user_credentials_id,
                        principalTable: "user_credentials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    credentials_id = table.Column<int>(type: "integer", nullable: false),
                    nickname = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    role_weight = table.Column<int>(type: "integer", nullable: false),
                    role_name = table.Column<string>(type: "text", nullable: false),
                    image_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_users_user_credentials_credentials_id",
                        column: x => x.credentials_id,
                        principalTable: "user_credentials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_user_images_image_id",
                        column: x => x.image_id,
                        principalTable: "user_images",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "word_info_word_info",
                columns: table => new
                {
                    translations_id = table.Column<Guid>(type: "uuid", nullable: false),
                    word_info_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_word_info_word_info", x => new { x.translations_id, x.word_info_id });
                    table.ForeignKey(
                        name: "fk_word_info_word_info_word_infos_translations_id",
                        column: x => x.translations_id,
                        principalTable: "word_infos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_word_info_word_info_word_infos_word_info_id",
                        column: x => x.word_info_id,
                        principalTable: "word_infos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_authorization_type_user_credentials_id",
                table: "authorization_type",
                column: "user_credentials_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_credentials_id",
                table: "users",
                column: "credentials_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_image_id",
                table: "users",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "ix_word_info_word_info_word_info_id",
                table: "word_info_word_info",
                column: "word_info_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "authorization_type");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "word_info_word_info");

            migrationBuilder.DropTable(
                name: "user_credentials");

            migrationBuilder.DropTable(
                name: "user_images");

            migrationBuilder.DropTable(
                name: "word_infos");
        }
    }
}

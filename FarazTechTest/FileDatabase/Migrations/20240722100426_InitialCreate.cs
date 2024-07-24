using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FileDatabase.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "folders",
                columns: table => new
                {
                    folderid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    parentfolderid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("folders_pkey", x => x.folderid);
                    table.ForeignKey(
                        name: "folders_parentfolderid_fkey",
                        column: x => x.parentfolderid,
                        principalTable: "folders",
                        principalColumn: "folderid");
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    fileid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    folderid = table.Column<int>(type: "integer", nullable: false),
                    filedata = table.Column<byte[]>(type: "bytea", nullable: false),
                    filetype = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    uploadeddatetime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("files_pkey", x => x.fileid);
                    table.ForeignKey(
                        name: "files_folderid_fkey",
                        column: x => x.folderid,
                        principalTable: "folders",
                        principalColumn: "folderid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_files_folderid",
                table: "files",
                column: "folderid");

            migrationBuilder.CreateIndex(
                name: "IX_folders_parentfolderid",
                table: "folders",
                column: "parentfolderid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "folders");
        }
    }
}

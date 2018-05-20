using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Blog_Web.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrator",
                columns: table => new
                {
                    Admin_ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Admin_Email = table.Column<string>(nullable: false),
                    Admin_Img = table.Column<string>(nullable: true),
                    Admin_Name = table.Column<string>(maxLength: 50, nullable: false),
                    Admin_Password = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrator", x => x.Admin_ID);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Contact_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(nullable: false),
                    Message = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Contact_Id);
                });

            migrationBuilder.CreateTable(
                name: "Tally",
                columns: table => new
                {
                    Tally_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Tally_Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tally", x => x.Tally_Id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Blog_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Admin_Id = table.Column<int>(nullable: false),
                    Blog_Context = table.Column<string>(nullable: false),
                    Blog_Digest = table.Column<string>(maxLength: 100, nullable: false),
                    Blog_Img = table.Column<string>(nullable: false),
                    Blog_Time = table.Column<DateTime>(nullable: false),
                    Blog_Title = table.Column<string>(maxLength: 50, nullable: false),
                    Tally_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Blog_Id);
                    table.ForeignKey(
                        name: "FK_Blog_Administrator_Admin_Id",
                        column: x => x.Admin_Id,
                        principalTable: "Administrator",
                        principalColumn: "Admin_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Blog_Tally_Tally_Id",
                        column: x => x.Tally_Id,
                        principalTable: "Tally",
                        principalColumn: "Tally_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Comment_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Blog_Id = table.Column<int>(nullable: false),
                    Comment_Context = table.Column<string>(maxLength: 50, nullable: false),
                    Comment_Time = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Visitor = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Comment_Id);
                    table.ForeignKey(
                        name: "FK_Comment_Blog_Blog_Id",
                        column: x => x.Blog_Id,
                        principalTable: "Blog",
                        principalColumn: "Blog_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_Admin_Id",
                table: "Blog",
                column: "Admin_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Blog_Tally_Id",
                table: "Blog",
                column: "Tally_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Blog_Id",
                table: "Comment",
                column: "Blog_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "Administrator");

            migrationBuilder.DropTable(
                name: "Tally");
        }
    }
}

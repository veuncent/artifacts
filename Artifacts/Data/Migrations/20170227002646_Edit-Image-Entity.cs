using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Artifacts.Data.Migrations
{
    public partial class EditImageEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_BlogPostImages_BannerImageId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_BlogPostImages_ThumbnailId",
                table: "BlogPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogPostImages",
                table: "BlogPostImages");

            migrationBuilder.DropColumn(
                name: "AzureUrl",
                table: "BlogPostImages");

            migrationBuilder.RenameTable(
                name: "BlogPostImages",
                newName: "BlogPostImage");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogPostImage",
                table: "BlogPostImage",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BlogPostImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostImages", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_BlogPostImage_BannerImageId",
                table: "BlogPosts",
                column: "BannerImageId",
                principalTable: "BlogPostImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_BlogPostImage_ThumbnailId",
                table: "BlogPosts",
                column: "ThumbnailId",
                principalTable: "BlogPostImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_BlogPostImage_BannerImageId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_BlogPostImage_ThumbnailId",
                table: "BlogPosts");

            migrationBuilder.DropTable(
                name: "BlogPostImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogPostImage",
                table: "BlogPostImage");

            migrationBuilder.RenameTable(
                name: "BlogPostImage",
                newName: "BlogPostImages");

            migrationBuilder.AddColumn<string>(
                name: "AzureUrl",
                table: "BlogPostImages",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogPostImages",
                table: "BlogPostImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_BlogPostImages_BannerImageId",
                table: "BlogPosts",
                column: "BannerImageId",
                principalTable: "BlogPostImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_BlogPostImages_ThumbnailId",
                table: "BlogPosts",
                column: "ThumbnailId",
                principalTable: "BlogPostImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

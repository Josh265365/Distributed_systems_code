﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFameworkLab.Migrations
{
    public partial class AddedCountryToAdress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Addresses");
        }
    }
}

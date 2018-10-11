using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class updateItemTable_GarmentEPO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "POId",
                table: "GarmentExternalPurchaseOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PONo",
                table: "GarmentExternalPurchaseOrderItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "POId",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "PONo",
                table: "GarmentExternalPurchaseOrderItems");
        }
    }
}

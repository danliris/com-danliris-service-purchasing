using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class Update_Garment_EPOItem_add_SmallUom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceBeforeTax",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.AddColumn<double>(
                name: "SmallQuantity",
                table: "GarmentExternalPurchaseOrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "SmallUomId",
                table: "GarmentExternalPurchaseOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SmallUomUnit",
                table: "GarmentExternalPurchaseOrderItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmallQuantity",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "SmallUomId",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "SmallUomUnit",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.AddColumn<double>(
                name: "PriceBeforeTax",
                table: "GarmentExternalPurchaseOrderItems",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

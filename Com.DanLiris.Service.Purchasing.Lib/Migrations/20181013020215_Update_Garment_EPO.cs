using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class Update_Garment_EPO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "GarmentExternalPurchaseOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOverBudget",
                table: "GarmentExternalPurchaseOrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "PricePerDealUnit",
                table: "GarmentExternalPurchaseOrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "IsOverBudget",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "PricePerDealUnit",
                table: "GarmentExternalPurchaseOrderItems");
        }
    }
}

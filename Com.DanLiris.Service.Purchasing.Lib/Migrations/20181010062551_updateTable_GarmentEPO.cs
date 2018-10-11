using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class updateTable_GarmentEPO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PRNoReference",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "GarmentExternalPurchaseOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOverBudget",
                table: "GarmentExternalPurchaseOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "BudgetPrice",
                table: "GarmentExternalPurchaseOrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DealQuantity",
                table: "GarmentExternalPurchaseOrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PO_SerialNumber",
                table: "GarmentExternalPurchaseOrderItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceBeforeTax",
                table: "GarmentExternalPurchaseOrderItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "IsOverBudget",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "BudgetPrice",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "DealQuantity",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "PO_SerialNumber",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.DropColumn(
                name: "PriceBeforeTax",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.AddColumn<string>(
                name: "PRNoReference",
                table: "GarmentExternalPurchaseOrderItems",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "GarmentExternalPurchaseOrderItems",
                nullable: false,
                defaultValue: 0f);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class Update_Garment_EPO_add_IsIncomeTax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PaymentDueDays",
                table: "GarmentExternalPurchaseOrders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "GarmentExternalPurchaseOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIncomeTax",
                table: "GarmentExternalPurchaseOrders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "GarmentExternalPurchaseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Article",
                table: "GarmentExternalPurchaseOrderItems",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "IsIncomeTax",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Article",
                table: "GarmentExternalPurchaseOrderItems");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentDueDays",
                table: "GarmentExternalPurchaseOrders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class Modify_Column_on_Table_GarmentDeliveryOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalQuantity",
                table: "GarmentDeliveryOrders");

            migrationBuilder.DropColumn(
                name: "POId",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "PONo",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "PaymentDueDays",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "GarmentDeliveryOrderDetails");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "GarmentDeliveryOrderDetails");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "GarmentDeliveryOrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "GarmentDeliveryOrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "GarmentDeliveryOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxId",
                table: "GarmentDeliveryOrderItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxName",
                table: "GarmentDeliveryOrderItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncomeTaxRate",
                table: "GarmentDeliveryOrderItems",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseIncomeTax",
                table: "GarmentDeliveryOrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "UseVat",
                table: "GarmentDeliveryOrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "POItemId",
                table: "GarmentDeliveryOrderDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<int>(
                name: "POId",
                table: "GarmentDeliveryOrderDetails",
                type: "int",
                maxLength: 255,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ReceiptQuantity",
                table: "GarmentDeliveryOrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "IncomeTaxId",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "IncomeTaxName",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "IncomeTaxRate",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "UseIncomeTax",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "UseVat",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "POId",
                table: "GarmentDeliveryOrderDetails");

            migrationBuilder.DropColumn(
                name: "ReceiptQuantity",
                table: "GarmentDeliveryOrderDetails");

            migrationBuilder.AddColumn<double>(
                name: "TotalQuantity",
                table: "GarmentDeliveryOrders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "POId",
                table: "GarmentDeliveryOrderItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PONo",
                table: "GarmentDeliveryOrderItems",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentDueDays",
                table: "GarmentDeliveryOrderItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "POItemId",
                table: "GarmentDeliveryOrderDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "GarmentDeliveryOrderDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyId",
                table: "GarmentDeliveryOrderDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "GarmentDeliveryOrderDetails",
                nullable: false,
                defaultValue: false);
        }
    }
}

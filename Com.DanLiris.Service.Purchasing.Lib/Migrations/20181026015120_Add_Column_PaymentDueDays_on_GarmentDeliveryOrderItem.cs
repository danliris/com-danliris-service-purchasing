using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class Add_Column_PaymentDueDays_on_GarmentDeliveryOrderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "IncomeTaxRate",
                table: "GarmentDeliveryOrderItems",
                type: "float",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IncomeTaxId",
                table: "GarmentDeliveryOrderItems",
                type: "int",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CurrencyId",
                table: "GarmentDeliveryOrderItems",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "PaymentDueDays",
                table: "GarmentDeliveryOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "GarmentDeliveryOrderDetails",
                type: "bigint",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDueDays",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.AlterColumn<string>(
                name: "IncomeTaxRate",
                table: "GarmentDeliveryOrderItems",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "IncomeTaxId",
                table: "GarmentDeliveryOrderItems",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "GarmentDeliveryOrderItems",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "GarmentDeliveryOrderDetails",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 255);
        }
    }
}

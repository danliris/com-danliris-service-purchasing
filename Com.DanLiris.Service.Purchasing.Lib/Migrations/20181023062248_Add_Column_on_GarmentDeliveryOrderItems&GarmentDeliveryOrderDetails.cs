using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class Add_Column_on_GarmentDeliveryOrderItemsGarmentDeliveryOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "POId",
                table: "GarmentDeliveryOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PONo",
                table: "GarmentDeliveryOrderItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentDueDays",
                table: "GarmentDeliveryOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "GarmentDeliveryOrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentType",
                table: "GarmentDeliveryOrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RONo",
                table: "GarmentDeliveryOrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "PaymentMethod",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropColumn(
                name: "RONo",
                table: "GarmentDeliveryOrderDetails");
        }
    }
}

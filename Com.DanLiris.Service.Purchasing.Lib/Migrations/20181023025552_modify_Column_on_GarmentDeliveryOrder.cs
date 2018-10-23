using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class modify_Column_on_GarmentDeliveryOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AlterColumn<long>(
                name: "SupplierId",
                table: "GarmentDeliveryOrders",
                type: "bigint",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarmentDeliveryOrderDetails_GarmentDeliveryOrderItems_GarmentDOItemId",
                table: "GarmentDeliveryOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_GarmentDeliveryOrderItems_GarmentDeliveryOrders_GarmentDOId",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentDeliveryOrderItems_GarmentDOId",
                table: "GarmentDeliveryOrderItems");

            migrationBuilder.DropIndex(
                name: "IX_GarmentDeliveryOrderDetails_GarmentDOItemId",
                table: "GarmentDeliveryOrderDetails");

            migrationBuilder.AlterColumn<string>(
                name: "SupplierId",
                table: "GarmentDeliveryOrders",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<long>(
                name: "DOId",
                table: "GarmentDeliveryOrderItems",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DOItemId",
                table: "GarmentDeliveryOrderDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GarmentDeliveryOrderItems_DOId",
                table: "GarmentDeliveryOrderItems",
                column: "DOId");

            migrationBuilder.CreateIndex(
                name: "IX_GarmentDeliveryOrderDetails_DOItemId",
                table: "GarmentDeliveryOrderDetails",
                column: "DOItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentDeliveryOrderDetails_GarmentDeliveryOrderItems_DOItemId",
                table: "GarmentDeliveryOrderDetails",
                column: "DOItemId",
                principalTable: "GarmentDeliveryOrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GarmentDeliveryOrderItems_GarmentDeliveryOrders_DOId",
                table: "GarmentDeliveryOrderItems",
                column: "DOId",
                principalTable: "GarmentDeliveryOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

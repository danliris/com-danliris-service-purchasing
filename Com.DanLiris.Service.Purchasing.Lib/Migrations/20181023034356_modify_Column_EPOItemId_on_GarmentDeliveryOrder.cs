using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class modify_Column_EPOItemId_on_GarmentDeliveryOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPODetailId",
                table: "GarmentDeliveryOrderDetails");

            migrationBuilder.AddColumn<long>(
                name: "EPOItemId",
                table: "GarmentDeliveryOrderDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPOItemId",
                table: "GarmentDeliveryOrderDetails");

            migrationBuilder.AddColumn<long>(
                name: "EPODetailId",
                table: "GarmentDeliveryOrderDetails",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

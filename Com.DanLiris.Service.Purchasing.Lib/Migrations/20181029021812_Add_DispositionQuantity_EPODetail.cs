using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class Add_DispositionQuantity_EPODetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDueDays",
                table: "PurchasingDispositions");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PaymentDueDate",
                table: "PurchasingDispositions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<double>(
                name: "DispositionQuantity",
                table: "ExternalPurchaseOrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDueDate",
                table: "PurchasingDispositions");

            migrationBuilder.DropColumn(
                name: "DispositionQuantity",
                table: "ExternalPurchaseOrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "PaymentDueDays",
                table: "PurchasingDispositions",
                nullable: false,
                defaultValue: 0);
        }
    }
}

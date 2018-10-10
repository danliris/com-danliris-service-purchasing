using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class addStandardQuality_GarmentEPO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DarkPerspiration",
                table: "GarmentExternalPurchaseOrders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DryRubbing",
                table: "GarmentExternalPurchaseOrders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LightMedPerspiration",
                table: "GarmentExternalPurchaseOrders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PieceLength",
                table: "GarmentExternalPurchaseOrders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QualityStandardType",
                table: "GarmentExternalPurchaseOrders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Shrinkage",
                table: "GarmentExternalPurchaseOrders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Washing",
                table: "GarmentExternalPurchaseOrders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WetRubbing",
                table: "GarmentExternalPurchaseOrders",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DarkPerspiration",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "DryRubbing",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "LightMedPerspiration",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "PieceLength",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "QualityStandardType",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Shrinkage",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Washing",
                table: "GarmentExternalPurchaseOrders");

            migrationBuilder.DropColumn(
                name: "WetRubbing",
                table: "GarmentExternalPurchaseOrders");
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class changePricetoPricePerDealUnitGDeliveryOrderNonPO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "GarmentDeliveryOrderNonPOItems",
                newName: "PricePerDealUnit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PricePerDealUnit",
                table: "GarmentDeliveryOrderNonPOItems",
                newName: "Price");
        }
    }
}

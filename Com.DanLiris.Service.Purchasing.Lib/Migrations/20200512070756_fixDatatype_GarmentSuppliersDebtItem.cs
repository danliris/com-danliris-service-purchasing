using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Com.DanLiris.Service.Purchasing.Lib.Migrations
{
    public partial class fixDatatype_GarmentSuppliersDebtItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "DOId",
                table: "GarmentSupplierBalanceDebtItems",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DOId",
                table: "GarmentSupplierBalanceDebtItems",
                nullable: true,
                oldClrType: typeof(long));
        }
    }
}

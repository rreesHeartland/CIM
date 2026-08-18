using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeartlandCIM.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Current_Cycle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalibrationInstruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MM = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Area = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PointType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InstrumentType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Low = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    High = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    U_of_M = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tolerance = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModelNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Cal_Frequency = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Last_Cal_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Next_Cal_Date = table.Column<DateTime>(type: "date", nullable: true),
                    AreaStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AccessRequirement = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tool_Requirement = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DrainTanks = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Removed_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Removed_Tech = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Install_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Install_Tech = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Feedback_Verified_Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Feedback_Verified_Tech = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Detail_PicPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Wide_PicPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationInstruments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LogTimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Technician_Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Action_Taken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Archived_PicPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Image_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentLog", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaStatus_Title",
                table: "AreaStatus",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationInstruments_Area",
                table: "CalibrationInstruments",
                column: "Area");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationInstruments_Title",
                table: "CalibrationInstruments",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentLog_ItemID",
                table: "InstrumentLog",
                column: "ItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaStatus");

            migrationBuilder.DropTable(
                name: "CalibrationInstruments");

            migrationBuilder.DropTable(
                name: "InstrumentLog");
        }
    }
}

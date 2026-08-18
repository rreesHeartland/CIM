/* ============================================================
   Heartland Calibrated Instruments Manager - Database Schema
   Target: Microsoft SQL Server
   Run this against a database named HeartlandCIM (create it first):
       CREATE DATABASE HeartlandCIM;
       GO
   ============================================================ */

IF OBJECT_ID('dbo.InstrumentLog', 'U') IS NOT NULL DROP TABLE dbo.InstrumentLog;
IF OBJECT_ID('dbo.CalibrationInstruments', 'U') IS NOT NULL DROP TABLE dbo.CalibrationInstruments;
IF OBJECT_ID('dbo.AreaStatus', 'U') IS NOT NULL DROP TABLE dbo.AreaStatus;
GO

/* ---------- AreaStatus ---------- */
CREATE TABLE dbo.AreaStatus (
    Id            INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title         NVARCHAR(255)  NOT NULL,
    Status        NVARCHAR(50)   NOT NULL DEFAULT('Closed'),
    Current_Cycle NVARCHAR(100)  NULL,
    Created       DATETIME2      NOT NULL DEFAULT(SYSUTCDATETIME()),
    Modified      DATETIME2      NOT NULL DEFAULT(SYSUTCDATETIME())
);
GO
CREATE INDEX IX_AreaStatus_Title ON dbo.AreaStatus(Title);
GO

/* ---------- CalibrationInstruments ---------- */
CREATE TABLE dbo.CalibrationInstruments (
    Id                     INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Title                  NVARCHAR(255)  NOT NULL,
    MM                     NVARCHAR(255)  NULL,
    Area                   NVARCHAR(255)  NOT NULL,
    Description            NVARCHAR(255)  NULL,
    PointType              NVARCHAR(50)   NULL,   -- 'CP' or 'CCP'
    InstrumentType         NVARCHAR(255)  NULL,
    Low                    NVARCHAR(50)   NULL,
    Mid                    NVARCHAR(50)   NULL,
    High                   NVARCHAR(50)   NULL,
    U_of_M                 NVARCHAR(100)  NULL,
    Tolerance              NVARCHAR(100)  NULL,
    Manufacturer           NVARCHAR(255)  NULL,
    ModelNumber            NVARCHAR(255)  NULL,
    SerialNumber           NVARCHAR(255)  NULL,
    Cal_Frequency          NVARCHAR(100)  NULL,
    Last_Cal_Date          DATETIME2      NULL,
    Next_Cal_Date          DATE           NULL,
    AreaStatus             NVARCHAR(50)   NULL,   -- 'Open' or 'Closed'
    AccessRequirement      NVARCHAR(100)  NULL,   -- Ground Level / Ladder Required / Scissor Lift / Crawl under
    Tool_Requirement       NVARCHAR(255)  NULL,
    DrainTanks             NVARCHAR(255)  NULL,   -- non-null => tanks must be drained
    Removed_Time           DATETIME2      NULL,
    Removed_Tech           NVARCHAR(255)  NULL,
    Install_Time           DATETIME2      NULL,
    Install_Tech           NVARCHAR(255)  NULL,
    Feedback_Verified_Time DATETIME2      NULL,
    Feedback_Verified_Tech NVARCHAR(255)  NULL,
    Detail_PicPath         NVARCHAR(500)  NULL,
    Wide_PicPath           NVARCHAR(500)  NULL,
    Created                DATETIME2      NOT NULL DEFAULT(SYSUTCDATETIME()),
    Modified               DATETIME2      NOT NULL DEFAULT(SYSUTCDATETIME())
);
GO
CREATE INDEX IX_CalibrationInstruments_Area  ON dbo.CalibrationInstruments(Area);
CREATE INDEX IX_CalibrationInstruments_Title ON dbo.CalibrationInstruments(Title);
GO

/* ---------- InstrumentLog ---------- */
CREATE TABLE dbo.InstrumentLog (
    Id               INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ItemID           NVARCHAR(255)  NOT NULL,   -- references CalibrationInstruments.Title
    LogTimeStamp     DATETIME2      NOT NULL DEFAULT(SYSUTCDATETIME()),
    Technician_Name  NVARCHAR(255)  NULL,
    Action_Taken     NVARCHAR(500)  NULL,
    Archived_PicPath NVARCHAR(500)  NULL,
    Image_Type       NVARCHAR(50)   NULL,
    Created          DATETIME2      NOT NULL DEFAULT(SYSUTCDATETIME())
);
GO
CREATE INDEX IX_InstrumentLog_ItemID ON dbo.InstrumentLog(ItemID);
GO

/* Note: The instrument workflow "Status" (Not Started / Removed / Installed /
   Verified) is intentionally NOT stored as a column. It is always computed in
   the application from Removed_Time / Install_Time / Feedback_Verified_Time. */

/* ============================================================
   Heartland Calibrated Instruments Manager - Sample Seed Data
   Run AFTER schema.sql against the HeartlandCIM database.
   ============================================================ */

/* ---------- Areas ---------- */
SET IDENTITY_INSERT dbo.AreaStatus OFF;

INSERT INTO dbo.AreaStatus (Title, Status, Current_Cycle)
VALUES
 (N'Boiler House',   N'Open',   N'Spring 2026'),
 (N'Process Area 1', N'Open',   N'Spring 2026'),
 (N'Tank Farm',      N'Closed', N'Spring 2026'),
 (N'Utilities',      N'Closed', N'Spring 2026');
GO

/* ---------- Instruments ---------- */
INSERT INTO dbo.CalibrationInstruments
 (Title, MM, Area, Description, PointType, InstrumentType, Low, Mid, High, U_of_M,
  Tolerance, Manufacturer, ModelNumber, SerialNumber, Cal_Frequency,
  Last_Cal_Date, Next_Cal_Date, AreaStatus, AccessRequirement, Tool_Requirement, DrainTanks)
VALUES
 (N'TT.1010', N'MM-1010', N'Boiler House', N'Steam header temperature transmitter', N'CCP',
  N'Temperature Transmitter', N'0', N'250', N'500', N'°F', N'+/- 1%',
  N'Rosemount', N'3144P', N'SN-TT1010', N'6 Months',
  '2025-10-15', '2026-04-15', N'Open', N'Ground Level', N'Calibrated multimeter', NULL),

 (N'PT.1020', N'MM-1020', N'Boiler House', N'Boiler drum pressure transmitter', N'CCP',
  N'Pressure Transmitter', N'0', N'150', N'300', N'PSI', N'+/- 0.5%',
  N'Rosemount', N'3051S', N'SN-PT1020', N'6 Months',
  '2025-10-01', '2026-04-01', N'Open', N'Scissor Lift', N'Pressure calibrator', N'Drain boiler drum before removal'),

 (N'FT.1030', N'MM-1030', N'Boiler House', N'Feedwater flow transmitter', N'CP',
  N'Flow Transmitter', N'0', N'500', N'1000', N'GPM', N'+/- 1%',
  N'Endress+Hauser', N'Promag 400', N'SN-FT1030', N'12 Months',
  '2025-05-20', '2026-05-20', N'Open', N'Ladder Required', N'Laptop with config software', NULL),

 (N'LT.2010', N'MM-2010', N'Process Area 1', N'Reactor level transmitter', N'CCP',
  N'Level Transmitter', N'0', N'50', N'100', N'%', N'+/- 0.5%',
  N'Siemens', N'SITRANS LR250', N'SN-LT2010', N'6 Months',
  '2025-09-30', '2026-03-30', N'Open', N'Crawl under', N'Radar test target', N'Empty reactor vessel first'),

 (N'TT.2020', N'MM-2020', N'Process Area 1', N'Jacket temperature RTD', N'CP',
  N'RTD', N'-40', N'100', N'200', N'°C', N'+/- 1%',
  N'WIKA', N'TR10', N'SN-TT2020', N'12 Months',
  '2025-06-10', '2026-06-10', N'Open', N'Ground Level', N'Dry block calibrator', NULL),

 (N'PT.3010', N'MM-3010', N'Tank Farm', N'Storage tank pressure transmitter', N'CP',
  N'Pressure Transmitter', N'0', N'25', N'50', N'PSI', N'+/- 1%',
  N'Yokogawa', N'EJA530E', N'SN-PT3010', N'12 Months',
  '2025-04-01', '2026-04-01', N'Closed', N'Ladder Required', N'Pressure calibrator', NULL);
GO

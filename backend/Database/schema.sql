/* =========================================================
   Home Healthcare Management System - Database Schema
   Target: SQL Server
   ========================================================= */

IF DB_ID('HomeHealthcareDb') IS NULL
BEGIN
    CREATE DATABASE HomeHealthcareDb;
END
GO

USE HomeHealthcareDb;
GO

-- ---------------------------------------------------------
-- Users (Admin, Caregiver, Patient login accounts)
-- ---------------------------------------------------------
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
CREATE TABLE dbo.Users (
    UserId          INT IDENTITY(1,1) PRIMARY KEY,
    FullName        NVARCHAR(150)   NOT NULL,
    Email           NVARCHAR(150)   NOT NULL UNIQUE,
    PasswordHash    NVARCHAR(300)   NOT NULL,
    Role            NVARCHAR(20)    NOT NULL CHECK (Role IN ('Admin','Caregiver','Patient')),
    CreatedAt       DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- ---------------------------------------------------------
-- Patients
-- ---------------------------------------------------------
IF OBJECT_ID('dbo.Patients', 'U') IS NOT NULL DROP TABLE dbo.Patients;
CREATE TABLE dbo.Patients (
    PatientId       INT IDENTITY(1,1) PRIMARY KEY,
    UserId          INT NOT NULL REFERENCES dbo.Users(UserId) ON DELETE CASCADE,
    FullName        NVARCHAR(150) NOT NULL,
    Email           NVARCHAR(150) NOT NULL,
    Phone           NVARCHAR(30)  NULL,
    Address         NVARCHAR(300) NULL,
    DateOfBirth     DATE          NULL,
    MedicalHistory  NVARCHAR(MAX) NULL,
    CreatedAt       DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- ---------------------------------------------------------
-- Caregivers
-- ---------------------------------------------------------
IF OBJECT_ID('dbo.Caregivers', 'U') IS NOT NULL DROP TABLE dbo.Caregivers;
CREATE TABLE dbo.Caregivers (
    CaregiverId     INT IDENTITY(1,1) PRIMARY KEY,
    UserId          INT NOT NULL REFERENCES dbo.Users(UserId) ON DELETE CASCADE,
    FullName        NVARCHAR(150) NOT NULL,
    Email           NVARCHAR(150) NOT NULL,
    Phone           NVARCHAR(30)  NULL,
    Specialization  NVARCHAR(150) NULL,
    IsAvailable     BIT           NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- ---------------------------------------------------------
-- Appointments (visit scheduling)
-- ---------------------------------------------------------
IF OBJECT_ID('dbo.Appointments', 'U') IS NOT NULL DROP TABLE dbo.Appointments;
CREATE TABLE dbo.Appointments (
    AppointmentId   INT IDENTITY(1,1) PRIMARY KEY,
    PatientId       INT NOT NULL REFERENCES dbo.Patients(PatientId) ON DELETE CASCADE,
    CaregiverId     INT NOT NULL REFERENCES dbo.Caregivers(CaregiverId),
    ScheduledDate   DATE          NOT NULL,
    TimeSlot        NVARCHAR(30)  NOT NULL,   -- e.g. "09:00 AM - 10:00 AM"
    Status          NVARCHAR(20)  NOT NULL DEFAULT 'Scheduled' CHECK (Status IN ('Scheduled','Completed','Cancelled')),
    VisitNotes      NVARCHAR(MAX) NULL,
    CreatedAt       DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME(),

    -- Prevent double-booking the same caregiver for the same date/time slot
    CONSTRAINT UQ_Caregiver_Slot UNIQUE (CaregiverId, ScheduledDate, TimeSlot)
);
GO

-- ---------------------------------------------------------
-- Care Plans
-- ---------------------------------------------------------
IF OBJECT_ID('dbo.CarePlans', 'U') IS NOT NULL DROP TABLE dbo.CarePlans;
CREATE TABLE dbo.CarePlans (
    CarePlanId      INT IDENTITY(1,1) PRIMARY KEY,
    PatientId       INT NOT NULL REFERENCES dbo.Patients(PatientId) ON DELETE CASCADE,
    CaregiverId     INT NOT NULL REFERENCES dbo.Caregivers(CaregiverId),
    Title           NVARCHAR(200) NOT NULL,
    Description     NVARCHAR(MAX) NULL,
    Medications     NVARCHAR(MAX) NULL,
    Goals           NVARCHAR(MAX) NULL,
    StartDate       DATE          NOT NULL,
    EndDate         DATE          NULL,
    Status          NVARCHAR(20)  NOT NULL DEFAULT 'Active' CHECK (Status IN ('Active','Completed')),
    CreatedAt       DATETIME2     NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- ---------------------------------------------------------
-- Helpful indexes
-- ---------------------------------------------------------
CREATE INDEX IX_Appointments_PatientId ON dbo.Appointments(PatientId);
CREATE INDEX IX_Appointments_CaregiverId ON dbo.Appointments(CaregiverId);
CREATE INDEX IX_CarePlans_PatientId ON dbo.CarePlans(PatientId);
GO

PRINT 'HomeHealthcareDb schema created successfully.';

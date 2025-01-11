DECLARE
    @JobOffers TABLE
               (
                   Id               UNIQUEIDENTIFIER,
                   OfferSummary     NVARCHAR(100),
                   JobDescription   NVARCHAR(1000),

                   Location_City    NVARCHAR(100),
                   Location_Country NVARCHAR(100),

                   PublishedAt      DATETIMEOFFSET,
                   PublishedUntil   DATETIMEOFFSET,

                   RemoteWork       INT
               );

DECLARE @ContractDetails TABLE
                         (
                             Id                      INT,
                             JobOfferId              UNIQUEIDENTIFIER,
                             EmploymentType          INT,

                             SalaryRange_IsPublished BIT,
                             SalaryRange_From        DECIMAL(18, 2),
                             SalaryRange_To          DECIMAL(18, 2),

                             TimeNumerator           INT,
                             TimeDenominator         INT
                         );

DECLARE @JobOfferRequirements TABLE
                              (
                                  JobOfferId      UNIQUEIDENTIFIER,
                                  ExperienceLevel INT
                              );

DECLARE @Skills TABLE
                (
                    Id                     INT,
                    RequirementsJobOfferId UNIQUEIDENTIFIER,
                    Label                  NVARCHAR(100),
                    Level                  INT
                );

INSERT INTO @JobOffers
VALUES ('D0C85350-E31E-4D62-BBF3-FDC554877D92',
        '.NET Developer',
        'Doing dotnetty stuff',
        'Gdańsk',
        'Poland',
        '2021-01-01 00:00:00',
        NULL,
        1);

INSERT INTO @ContractDetails
VALUES (3,
        'D0C85350-E31E-4D62-BBF3-FDC554877D92',
        1,
        1,
        500,
        600,
        8,
        8);

INSERT INTO @JobOfferRequirements
VALUES ('D0C85350-E31E-4D62-BBF3-FDC554877D92',
        2);

INSERT INTO @Skills
VALUES (1,
        'D0C85350-E31E-4D62-BBF3-FDC554877D92',
        'C#',
        2);

MERGE INTO dbo.JobOffers AS target
USING @JobOffers AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE
    SET OfferSummary     = source.OfferSummary,
        JobDescription   = source.JobDescription,
        Location_City    = source.Location_City,
        Location_Country = source.Location_Country,
        PublishedAt      = source.PublishedAt,
        PublishedUntil   = source.PublishedUntil,
        RemoteWork       = source.RemoteWork
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, OfferSummary, JobDescription, Location_City, Location_Country, PublishedAt, PublishedUntil, RemoteWork)
    VALUES (source.Id, source.OfferSummary, source.JobDescription, source.Location_City, source.Location_Country,
            source.PublishedAt, source.PublishedUntil, source.RemoteWork);

SET IDENTITY_INSERT dbo.JobOfferContractsDetails ON;

MERGE INTO dbo.JobOfferContractsDetails AS target
USING @ContractDetails AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE
    SET EmploymentType          = source.EmploymentType,
        SalaryRange_IsPublished = source.SalaryRange_IsPublished,
        SalaryRange_From        = source.SalaryRange_From,
        SalaryRange_To          = source.SalaryRange_To,
        TimeNumerator           = source.TimeNumerator,
        TimeDenominator         = source.TimeDenominator
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, JobOfferId, EmploymentType, SalaryRange_IsPublished, SalaryRange_From, SalaryRange_To, TimeNumerator,
            TimeDenominator)
    VALUES (source.Id, source.JobOfferId, source.EmploymentType, source.SalaryRange_IsPublished,
            source.SalaryRange_From, source.SalaryRange_To, source.TimeNumerator, source.TimeDenominator);

SET IDENTITY_INSERT dbo.JobOfferContractsDetails OFF;

MERGE INTO dbo.JobOfferRequirements AS target
USING @JobOfferRequirements AS source
ON target.JobOfferId = source.JobOfferId
WHEN MATCHED THEN
    UPDATE
    SET ExperienceLevel = source.ExperienceLevel
WHEN NOT MATCHED BY TARGET THEN
    INSERT (JobOfferId, ExperienceLevel)
    VALUES (source.JobOfferId, source.ExperienceLevel);

SET IDENTITY_INSERT dbo.Skills ON;

MERGE INTO dbo.Skills AS target
USING @Skills AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE
    SET Label = source.Label,
        Level = source.Level
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, RequirementsJobOfferId, Label, Level)
    VALUES (source.Id, source.RequirementsJobOfferId, source.Label, source.Level);

SET IDENTITY_INSERT dbo.Skills OFF;
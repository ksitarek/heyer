DECLARE
    @PublishedJobOffers
    TABLE
    (
        Id               UNIQUEIDENTIFIER,

        CompanyId        UNIQUEIDENTIFIER,
        CompanyName      NVARCHAR(100),

        OfferSummary     NVARCHAR(100),
        JobDescription   NVARCHAR(1000),
        RemoteWork       INT,

        Location_City    NVARCHAR(100),
        Location_Country NVARCHAR(100),

        PublishedAt      DATETIMEOFFSET,
        PublishedUntil   DATETIMEOFFSET,

        ExperienceLevel  INT
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

DECLARE @Skills TABLE
                (
                    Id         INT,
                    JobOfferId UNIQUEIDENTIFIER,
                    SkillLabel NVARCHAR(100),
                    SkillLevel INT
                );

INSERT INTO @PublishedJobOffers
VALUES ('3E99C748-9DC3-49DD-97E1-9233A166802F',
        '0692183B-CE56-432D-88B5-B59280A678C5',
        'ACME Corporation 06',
        'DevOps Engineer',
        'Doing devops stuff',
        1,
        'Warsaw',
        'Poland',
        '2021-01-01 00:00:00',
        NULL,
        3),
       ('D0C85350-E31E-4D62-BBF3-FDC554877D92',
        'A62C048C-8E0F-41E2-84D4-BD061F9DDE97',
        'ACME Corporation A6',
        '.NET Developer',
        'Doing dotnetty stuff',
        2,
        N'Gdańsk',
        'Poland',
        '2021-01-01 00:00:00',
        NULL,
        2);

INSERT INTO @ContractDetails
VALUES (1,
        '3E99C748-9DC3-49DD-97E1-9233A166802F',
        1,
        1,
        1000,
        2000,
        8,
        8),
       (2,
        '3E99C748-9DC3-49DD-97E1-9233A166802F',
        2,
        1,
        2000,
        3000,
        8,
        8),
       (3,
        'D0C85350-E31E-4D62-BBF3-FDC554877D92',
        1,
        1,
        500,
        600,
        8,
        8);

INSERT INTO @Skills
VALUES (1,
        '3E99C748-9DC3-49DD-97E1-9233A166802F',
        'Python',
        2),
       (2,
        '3E99C748-9DC3-49DD-97E1-9233A166802F',
        'SQL',
        3),
       (3,
        'D0C85350-E31E-4D62-BBF3-FDC554877D92',
        'C#',
        2);

MERGE INTO job_board.JobOffers AS target
USING @PublishedJobOffers AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE
    SET CompanyDetails_CompanyId     = source.CompanyId,
        CompanyDetails_Name          = source.CompanyName,
        OfferSummary                 = source.OfferSummary,
        JobDescription               = source.JobDescription,
        RemoteWork                   = source.RemoteWork,
        Location_City                = source.Location_City,
        Location_Country             = source.Location_Country,
        PublishedAt                  = source.PublishedAt,
        PublishedUntil               = source.PublishedUntil,
        Requirements_ExperienceLevel = source.ExperienceLevel
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, CompanyDetails_CompanyId, CompanyDetails_Name, OfferSummary, JobDescription, RemoteWork, Location_City,
            Location_Country, Requirements_ExperienceLevel,
            PublishedAt, PublishedUntil)
    VALUES (source.Id, source.CompanyId, source.CompanyName, source.OfferSummary, source.JobDescription,
            source.RemoteWork, source.Location_City, source.Location_Country, source.ExperienceLevel,
            source.PublishedAt, source.PublishedUntil);

SET IDENTITY_INSERT job_board.JobOfferContractsDetails ON;
MERGE INTO job_board.JobOfferContractsDetails AS target
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
    INSERT (Id, PublishedJobOfferId, EmploymentType, SalaryRange_IsPublished, SalaryRange_From, SalaryRange_To,
            TimeNumerator,
            TimeDenominator)
    VALUES (source.Id, source.JobOfferId, source.EmploymentType, source.SalaryRange_IsPublished,
            source.SalaryRange_From,
            source.SalaryRange_To, source.TimeNumerator, source.TimeDenominator);

SET IDENTITY_INSERT job_board.JobOfferContractsDetails OFF;

SET IDENTITY_INSERT job_board.Skills ON;

MERGE INTO job_board.Skills AS target
USING @Skills AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE
    SET Label = source.SkillLabel,
        Level = source.SkillLevel
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, RequirementsPublishedJobOfferId, Label, Level)
    VALUES (source.Id, source.JobOfferId, source.SkillLabel, source.SkillLevel);

SET IDENTITY_INSERT job_board.Skills OFF;
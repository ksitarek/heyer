CREATE TABLE job_board.InboxMessages
(
    Id          UNIQUEIDENTIFIER NOT NULL,
    CreatedAt   DATETIME2        NOT NULL,
    Data        NVARCHAR(MAX)    NOT NULL,
    ProcessedAt DATETIME2,
    Type        NVARCHAR(MAX)    NOT NULL,

    CONSTRAINT PK_InboxMessages PRIMARY KEY (Id)
)
GO

CREATE TABLE job_board.JobOffers
(
    Id                           UNIQUEIDENTIFIER NOT NULL,
    CompanyDetails_CompanyId     UNIQUEIDENTIFIER NOT NULL,
    CompanyDetails_Name          NVARCHAR(MAX)    NOT NULL,
    JobDescription               NVARCHAR(MAX)    NOT NULL,
    Location_City                NVARCHAR(MAX)    NOT NULL,
    Location_Country             NVARCHAR(MAX)    NOT NULL,
    OfferSummary                 NVARCHAR(100)    NOT NULL,
    PublishedAt                  DATETIMEOFFSET   NOT NULL,
    PublishedUntil               DATETIMEOFFSET,
    RemoteWork                   INT              NOT NULL,
    Requirements_ExperienceLevel INT              NOT NULL,

    CONSTRAINT PK_JobOffers PRIMARY KEY (Id)
)
GO

CREATE TABLE job_board.JobOfferContractsDetails
(
    Id                      INT IDENTITY,
    PublishedJobOfferId     UNIQUEIDENTIFIER NOT NULL,
    EmploymentType          INT              NOT NULL,
    SalaryRange_IsPublished BIT              NOT NULL,
    SalaryRange_From        DECIMAL(18, 2)   NOT NULL,
    SalaryRange_To          DECIMAL(18, 2)   NOT NULL,
    TimeNumerator           INT              NOT NULL,
    TimeDenominator         INT              NOT NULL,

    CONSTRAINT PK_JobOfferContractsDetails PRIMARY KEY (PublishedJobOfferId, Id),

    CONSTRAINT FK_JobOfferContractsDetails_JobOffers_PublishedJobOfferId
        FOREIGN KEY (PublishedJobOfferId)
            REFERENCES job_board.JobOffers (Id)
)
GO

CREATE TABLE job_board.OutboxMessages
(
    Id          UNIQUEIDENTIFIER NOT NULL,
    CreatedAt   DATETIME2        NOT NULL,
    Data        NVARCHAR(MAX)    NOT NULL,
    ProcessedAt DATETIME2,
    Type        NVARCHAR(MAX)    NOT NULL,

    CONSTRAINT PK_OutboxMessages PRIMARY KEY (Id)
)
GO

CREATE TABLE job_board.Skills
(
    Id                              INT IDENTITY,
    RequirementsPublishedJobOfferId UNIQUEIDENTIFIER NOT NULL,
    Label                           NVARCHAR(MAX)    NOT NULL,
    Level                           INT              NOT NULL,

    CONSTRAINT PK_Skills PRIMARY KEY (RequirementsPublishedJobOfferId, Id),

    CONSTRAINT FK_Skills_JobOffers_RequirementsPublishedJobOfferId
        FOREIGN KEY (RequirementsPublishedJobOfferId)
            REFERENCES job_board.JobOffers (Id)
)
GO


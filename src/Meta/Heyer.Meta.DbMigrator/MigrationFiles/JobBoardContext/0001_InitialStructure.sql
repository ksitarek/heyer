CREATE TABLE job_board.Candidates
(
    Id                     UNIQUEIDENTIFIER NOT NULL,
    Email                  NVARCHAR(MAX)    NOT NULL,
    FirstName              NVARCHAR(50)     NOT NULL,
    IncludeInCandidatePool BIT              NOT NULL,
    LastName               NVARCHAR(50)     NOT NULL,
    ResumeKey              NVARCHAR(MAX)    NOT NULL,

    CONSTRAINT PK_Candidates PRIMARY KEY (Id)
)
GO

CREATE TABLE job_board.InboxMessages
(
    Id          UNIQUEIDENTIFIER NOT NULL,
    CreatedAt   DATETIME2        NOT NULL,
    Data        NVARCHAR(MAX)    NOT NULL,
    ProcessedAt DATETIME2,
    Type        NVARCHAR(MAX)    NOT NULL,

    CONSTRAINT PK_InboxMessages PRIMARY KEY (Id),
)
GO

CREATE TABLE job_board.JobOffers
(
    Id               UNIQUEIDENTIFIER NOT NULL,
    JobDescription   NVARCHAR(MAX)    NOT NULL,
    Location_City    NVARCHAR(100),
    Location_Country NVARCHAR(100),
    OfferSummary     NVARCHAR(100)    NOT NULL,
    PublishedAt      DATETIMEOFFSET,
    PublishedUntil   DATETIMEOFFSET,
    RemoteWork       INT              NOT NULL,

    CONSTRAINT PK_JobOffers PRIMARY KEY (Id)
)
GO

CREATE TABLE job_board.JobOfferCandidates
(
    Id         INT IDENTITY,
    Guid       UNIQUEIDENTIFIER NOT NULL,
    JobOfferId UNIQUEIDENTIFIER NOT NULL,

    CONSTRAINT PK_JobOfferCandidates PRIMARY KEY (JobOfferId, Id),

    CONSTRAINT FK_JobOfferCandidates_JobOffers_JobOfferId
        FOREIGN KEY (JobOfferId)
            REFERENCES job_board.JobOffers (Id)
            ON DELETE CASCADE
)
GO

CREATE TABLE job_board.JobOfferContractsDetails
(
    Id                      INT IDENTITY,
    JobOfferId              UNIQUEIDENTIFIER NOT NULL,
    EmploymentType          INT              NOT NULL,
    SalaryRange_IsPublished BIT              NOT NULL,
    SalaryRange_From        DECIMAL(18, 2)   NOT NULL,
    SalaryRange_To          DECIMAL(18, 2)   NOT NULL,
    TimeNumerator           INT              NOT NULL,
    TimeDenominator         INT              NOT NULL,

    CONSTRAINT PK_JobOfferContractsDetails PRIMARY KEY (JobOfferId, Id),

    CONSTRAINT FK_JobOfferContractsDetails_JobOffers_JobOfferId
        FOREIGN KEY (JobOfferId)
            REFERENCES job_board.JobOffers (Id)
            ON DELETE CASCADE,
)
GO

CREATE TABLE job_board.JobOfferRequirements
(
    JobOfferId      UNIQUEIDENTIFIER NOT NULL,
    ExperienceLevel INT              NOT NULL,

    CONSTRAINT PK_JobOfferRequirements PRIMARY KEY (JobOfferId),

    CONSTRAINT FK_JobOfferRequirements_JobOffers_JobOfferId
        FOREIGN KEY (JobOfferId) REFERENCES job_board.JobOffers (Id)
            ON DELETE CASCADE
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
    Id                     INT IDENTITY,
    RequirementsJobOfferId UNIQUEIDENTIFIER NOT NULL,
    Label                  NVARCHAR(MAX)    NOT NULL,
    Level                  INT              NOT NULL,

    CONSTRAINT PK_Skills PRIMARY KEY (RequirementsJobOfferId, Id),

    CONSTRAINT FK_Skills_JobOfferRequirements_RequirementsJobOfferId
        FOREIGN KEY (RequirementsJobOfferId)
            REFERENCES job_board.JobOfferRequirements (JobOfferId)
            ON DELETE CASCADE
)
GO
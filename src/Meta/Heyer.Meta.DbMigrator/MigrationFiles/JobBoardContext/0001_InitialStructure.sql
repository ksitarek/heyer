-- Table: job_board.InboxMessages
CREATE TABLE "job_board"."InboxMessages"
(
    "Id"          UUID      NOT NULL PRIMARY KEY,
    "CreatedAt"   TIMESTAMP NOT NULL,
    "Data"        TEXT      NOT NULL,
    "ProcessedAt" TIMESTAMP,
    "Type"        TEXT      NOT NULL
);

-- Table: job_board.JobOffers
CREATE TABLE "job_board"."JobOffers"
(
    "Id"                           UUID         NOT NULL PRIMARY KEY,
    "CompanyDetails_CompanyId"     UUID         NOT NULL,
    "CompanyDetails_Name"          TEXT         NOT NULL,
    "JobDescription"               TEXT         NOT NULL,
    "Location_City"                TEXT         NOT NULL,
    "Location_Country"             TEXT         NOT NULL,
    "OfferSummary"                 VARCHAR(100) NOT NULL,
    "PublishedAt"                  TIMESTAMPTZ  NOT NULL,
    "PublishedUntil"               TIMESTAMPTZ,
    "RemoteWork"                   INT          NOT NULL,
    "Requirements_ExperienceLevel" INT          NOT NULL
);

-- Table: job_board.JobOfferContractsDetails
CREATE TABLE "job_board"."JobOfferContractsDetails"
(
    "Id"                      SERIAL,
    "PublishedJobOfferId"     UUID           NOT NULL,
    "EmploymentType"          INT            NOT NULL,
    "SalaryRange_IsPublished" BOOLEAN        NOT NULL,
    "SalaryRange_From"        NUMERIC(18, 2) NOT NULL,
    "SalaryRange_To"          NUMERIC(18, 2) NOT NULL,
    "TimeNumerator"           INT            NOT NULL,
    "TimeDenominator"         INT            NOT NULL,
    PRIMARY KEY ("PublishedJobOfferId", "Id"),
    CONSTRAINT FK_JobOfferContractsDetails_JobOffers_PublishedJobOfferId FOREIGN KEY ("PublishedJobOfferId")
        REFERENCES "job_board"."JobOffers" ("Id")
);

-- Table: job_board.OutboxMessages
CREATE TABLE "job_board"."OutboxMessages"
(
    "Id"          UUID      NOT NULL PRIMARY KEY,
    "CreatedAt"   TIMESTAMP NOT NULL,
    "Data"        TEXT      NOT NULL,
    "ProcessedAt" TIMESTAMP,
    "Type"        TEXT      NOT NULL
);

-- Table: job_board.Skills
CREATE TABLE "job_board"."Skills"
(
    "Id"                              SERIAL,
    "RequirementsPublishedJobOfferId" UUID NOT NULL,
    "Label"                           TEXT NOT NULL,
    "Level"                           INT  NOT NULL,
    PRIMARY KEY ("RequirementsPublishedJobOfferId", "Id"),
    CONSTRAINT FK_Skills_JobOffers_RequirementsPublishedJobOfferId FOREIGN KEY ("RequirementsPublishedJobOfferId")
        REFERENCES "job_board"."JobOffers" ("Id")
);
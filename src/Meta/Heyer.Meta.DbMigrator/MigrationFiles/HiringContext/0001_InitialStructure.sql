-- Table: Candidates
CREATE TABLE "Candidates"
(
    "Id"                     UUID        NOT NULL PRIMARY KEY,
    "Email"                  TEXT        NOT NULL,
    "FirstName"              VARCHAR(50) NOT NULL,
    "IncludeInCandidatePool" BOOLEAN     NOT NULL,
    "LastName"               VARCHAR(50) NOT NULL,
    "ResumeKey"              TEXT        NOT NULL
);

-- Table: InboxMessages
CREATE TABLE "InboxMessages"
(
    "Id"          UUID      NOT NULL PRIMARY KEY,
    "CreatedAt"   TIMESTAMP NOT NULL,
    "Data"        TEXT      NOT NULL,
    "ProcessedAt" TIMESTAMP,
    "Type"        TEXT      NOT NULL
);

-- Table: JobOffers
CREATE TABLE "JobOffers"
(
    "Id"               UUID         NOT NULL PRIMARY KEY,
    "JobDescription"   TEXT         NOT NULL,
    "Location_City"    VARCHAR(100),
    "Location_Country" VARCHAR(100),
    "OfferSummary"     VARCHAR(100) NOT NULL,
    "PublishedAt"      TIMESTAMPTZ,
    "PublishedUntil"   TIMESTAMPTZ,
    "RemoteWork"       INT          NOT NULL
);

-- Table: JobOfferCandidates
CREATE TABLE "JobOfferCandidates"
(
    "Id"         SERIAL,
    "Guid"       UUID NOT NULL,
    "JobOfferId" UUID NOT NULL,
    PRIMARY KEY ("JobOfferId", "Id"),
    CONSTRAINT FK_JobOfferCandidates_JobOffers_JobOfferId FOREIGN KEY ("JobOfferId")
        REFERENCES "JobOffers" ("Id")
        ON DELETE CASCADE
);

-- Table: JobOfferContractsDetails
CREATE TABLE "JobOfferContractsDetails"
(
    "Id"                      SERIAL,
    "JobOfferId"              UUID           NOT NULL,
    "EmploymentType"          INT            NOT NULL,
    "SalaryRange_IsPublished" BOOLEAN        NOT NULL,
    "SalaryRange_From"        NUMERIC(18, 2) NOT NULL,
    "SalaryRange_To"          NUMERIC(18, 2) NOT NULL,
    "TimeNumerator"           INT            NOT NULL,
    "TimeDenominator"         INT            NOT NULL,
    PRIMARY KEY ("JobOfferId", "Id"),
    CONSTRAINT FK_JobOfferContractsDetails_JobOffers_JobOfferId FOREIGN KEY ("JobOfferId")
        REFERENCES "JobOffers" ("Id")
        ON DELETE CASCADE
);

-- Table: JobOfferRequirements
CREATE TABLE "JobOfferRequirements"
(
    "JobOfferId"      UUID NOT NULL PRIMARY KEY,
    "ExperienceLevel" INT  NOT NULL,
    CONSTRAINT FK_JobOfferRequirements_JobOffers_JobOfferId FOREIGN KEY ("JobOfferId")
        REFERENCES "JobOffers" ("Id")
        ON DELETE CASCADE
);

-- Table: OutboxMessages
CREATE TABLE "OutboxMessages"
(
    "Id"          UUID      NOT NULL PRIMARY KEY,
    "CreatedAt"   TIMESTAMP NOT NULL,
    "Data"        TEXT      NOT NULL,
    "ProcessedAt" TIMESTAMP,
    "Type"        TEXT      NOT NULL
);

-- Table: Skills
CREATE TABLE "Skills"
(
    "Id"                     SERIAL,
    "RequirementsJobOfferId" UUID NOT NULL,
    "Label"                  TEXT NOT NULL,
    "Level"                  INT  NOT NULL,
    PRIMARY KEY ("RequirementsJobOfferId", "Id"),
    CONSTRAINT FK_Skills_JobOfferRequirements_RequirementsJobOfferId FOREIGN KEY ("RequirementsJobOfferId")
        REFERENCES "JobOfferRequirements" ("JobOfferId")
        ON DELETE CASCADE
);
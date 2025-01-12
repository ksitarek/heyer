-- Temporary tables for PostgreSQL
CREATE TEMP TABLE "t_JobOffers" (
                                  "Id" UUID PRIMARY KEY,
                                  "OfferSummary" VARCHAR(100),
                                  "JobDescription" VARCHAR(1000),
                                  "Location_City" VARCHAR(100),
                                  "Location_Country" VARCHAR(100),
                                  "PublishedAt" TIMESTAMPTZ,
                                  "PublishedUntil" TIMESTAMPTZ,
                                  "RemoteWork" INT
);

CREATE TEMP TABLE "t_ContractDetails" (
                                        "Id" SERIAL PRIMARY KEY,
                                        "JobOfferId" UUID,
                                        "EmploymentType" INT,
                                        "SalaryRange_IsPublished" BOOLEAN,
                                        "SalaryRange_From" NUMERIC(18, 2),
                                        "SalaryRange_To" NUMERIC(18, 2),
                                        "TimeNumerator" INT,
                                        "TimeDenominator" INT
);

CREATE TEMP TABLE "t_JobOfferRequirements" (
                                             "JobOfferId" UUID PRIMARY KEY,
                                             "ExperienceLevel" INT
);

CREATE TEMP TABLE "t_Skills" (
                               "Id" SERIAL PRIMARY KEY,
                               "RequirementsJobOfferId" UUID,
                               "Label" VARCHAR(100),
                               "Level" INT
);

-- Insert data into temporary tables
INSERT INTO "t_JobOffers" VALUES
    ('D0C85350-E31E-4D62-BBF3-FDC554877D92', '.NET Developer', 'Doing dotnetty stuff', 'Gdańsk', 'Poland', '2021-01-01 00:00:00+00', NULL, 1);

INSERT INTO "t_ContractDetails" VALUES
    (3, 'D0C85350-E31E-4D62-BBF3-FDC554877D92', 1, TRUE, 500, 600, 8, 8);

INSERT INTO "t_JobOfferRequirements" VALUES
    ('D0C85350-E31E-4D62-BBF3-FDC554877D92', 2);

INSERT INTO "t_Skills" VALUES
    (1, 'D0C85350-E31E-4D62-BBF3-FDC554877D92', 'C#', 2);

-- Merge equivalent in PostgreSQL using INSERT ON CONFLICT
-- JobOffers table
INSERT INTO public."JobOffers" ("Id", "OfferSummary", "JobDescription", "Location_City", "Location_Country",
                                "PublishedAt", "PublishedUntil", "RemoteWork")
SELECT "Id",
       "OfferSummary",
       "JobDescription",
       "Location_City",
       "Location_Country",
       "PublishedAt",
       "PublishedUntil",
       "RemoteWork"
FROM "t_JobOffers"
ON CONFLICT ("Id") DO UPDATE
    SET "OfferSummary"     = EXCLUDED."OfferSummary",
        "JobDescription"   = EXCLUDED."JobDescription",
        "Location_City"    = EXCLUDED."Location_City",
        "Location_Country" = EXCLUDED."Location_Country",
        "PublishedAt"      = EXCLUDED."PublishedAt",
        "PublishedUntil"   = EXCLUDED."PublishedUntil",
        "RemoteWork"       = EXCLUDED."RemoteWork";

-- JobOfferContractsDetails table
INSERT INTO public."JobOfferContractsDetails" ("Id", "JobOfferId", "EmploymentType", "SalaryRange_IsPublished",
                                               "SalaryRange_From", "SalaryRange_To", "TimeNumerator", "TimeDenominator")
SELECT "Id",
       "JobOfferId",
       "EmploymentType",
       "SalaryRange_IsPublished",
       "SalaryRange_From",
       "SalaryRange_To",
       "TimeNumerator",
       "TimeDenominator"
FROM "t_ContractDetails"
ON CONFLICT ("JobOfferId", "Id") DO UPDATE
    SET "EmploymentType"          = EXCLUDED."EmploymentType",
        "SalaryRange_IsPublished" = EXCLUDED."SalaryRange_IsPublished",
        "SalaryRange_From"        = EXCLUDED."SalaryRange_From",
        "SalaryRange_To"          = EXCLUDED."SalaryRange_To",
        "TimeNumerator"           = EXCLUDED."TimeNumerator",
        "TimeDenominator"         = EXCLUDED."TimeDenominator";

-- JobOfferRequirements table
INSERT INTO public."JobOfferRequirements" ("JobOfferId", "ExperienceLevel")
SELECT "JobOfferId", "ExperienceLevel"
FROM "t_JobOfferRequirements"
ON CONFLICT ("JobOfferId") DO UPDATE
    SET "ExperienceLevel" = EXCLUDED."ExperienceLevel";

-- Skills table
INSERT INTO public."Skills" ("Id", "RequirementsJobOfferId", "Label", "Level")
SELECT "Id", "RequirementsJobOfferId", "Label", "Level"
FROM "t_Skills"
ON CONFLICT ("RequirementsJobOfferId", "Id") DO UPDATE
    SET "Label" = EXCLUDED."Label",
        "Level" = EXCLUDED."Level";

-- Drop temporary tables
DROP TABLE "t_JobOffers";
DROP TABLE "t_ContractDetails";
DROP TABLE "t_JobOfferRequirements";
DROP TABLE "t_Skills";
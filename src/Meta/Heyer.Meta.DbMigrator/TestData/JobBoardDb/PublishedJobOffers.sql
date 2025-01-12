-- Temporary tables for PostgreSQL
CREATE TEMP TABLE "t_PublishedJobOffers" (
                                           "Id" UUID PRIMARY KEY,
                                           "CompanyId" UUID,
                                           "CompanyName" VARCHAR(100),
                                           "OfferSummary" VARCHAR(100),
                                           "JobDescription" VARCHAR(1000),
                                           "RemoteWork" INT,
                                           "Location_City" VARCHAR(100),
                                           "Location_Country" VARCHAR(100),
                                           "PublishedAt" TIMESTAMPTZ,
                                           "PublishedUntil" TIMESTAMPTZ,
                                           "ExperienceLevel" INT
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

CREATE TEMP TABLE "t_Skills" (
                               "Id" SERIAL PRIMARY KEY,
                               "JobOfferId" UUID,
                               "SkillLabel" VARCHAR(100),
                               "SkillLevel" INT
);

-- Insert data into temporary tables
INSERT INTO "t_PublishedJobOffers" VALUES
                                     ('3E99C748-9DC3-49DD-97E1-9233A166802F', '0692183B-CE56-432D-88B5-B59280A678C5', 'ACME Corporation 06', 'DevOps Engineer', 'Doing devops stuff', 1, 'Warsaw', 'Poland', '2021-01-01 00:00:00+00', NULL, 3),
                                     ('D0C85350-E31E-4D62-BBF3-FDC554877D92', 'A62C048C-8E0F-41E2-84D4-BD061F9DDE97', 'ACME Corporation A6', '.NET Developer', 'Doing dotnetty stuff', 2, 'Gdańsk', 'Poland', '2021-01-01 00:00:00+00', NULL, 2);

INSERT INTO "t_ContractDetails" VALUES
                                  (1, '3E99C748-9DC3-49DD-97E1-9233A166802F', 1, TRUE, 1000, 2000, 8, 8),
                                  (2, '3E99C748-9DC3-49DD-97E1-9233A166802F', 2, TRUE, 2000, 3000, 8, 8),
                                  (3, 'D0C85350-E31E-4D62-BBF3-FDC554877D92', 1, TRUE, 500, 600, 8, 8);

INSERT INTO "t_Skills" VALUES
                         (1, '3E99C748-9DC3-49DD-97E1-9233A166802F', 'Python', 2),
                         (2, '3E99C748-9DC3-49DD-97E1-9233A166802F', 'SQL', 3),
                         (3, 'D0C85350-E31E-4D62-BBF3-FDC554877D92', 'C#', 2);

-- Merge equivalent for JobOffers
INSERT INTO "job_board"."JobOffers" ("Id", "CompanyDetails_CompanyId", "CompanyDetails_Name", "OfferSummary", "JobDescription", "RemoteWork", "Location_City", "Location_Country", "PublishedAt", "PublishedUntil", "Requirements_ExperienceLevel")
SELECT "Id", "CompanyId", "CompanyName", "OfferSummary", "JobDescription", "RemoteWork", "Location_City", "Location_Country", "PublishedAt", "PublishedUntil", "ExperienceLevel"
FROM "t_PublishedJobOffers"
ON CONFLICT ("Id") DO UPDATE
    SET "CompanyDetails_CompanyId" = EXCLUDED."CompanyDetails_CompanyId",
        "CompanyDetails_Name" = EXCLUDED."CompanyDetails_Name",
        "OfferSummary" = EXCLUDED."OfferSummary",
        "JobDescription" = EXCLUDED."JobDescription",
        "RemoteWork" = EXCLUDED."RemoteWork",
        "Location_City" = EXCLUDED."Location_City",
        "Location_Country" = EXCLUDED."Location_Country",
        "PublishedAt" = EXCLUDED."PublishedAt",
        "PublishedUntil" = EXCLUDED."PublishedUntil",
        "Requirements_ExperienceLevel" = EXCLUDED."Requirements_ExperienceLevel";

-- Merge equivalent for JobOfferContractsDetails
INSERT INTO "job_board"."JobOfferContractsDetails" ("Id", "PublishedJobOfferId", "EmploymentType", "SalaryRange_IsPublished", "SalaryRange_From", "SalaryRange_To", "TimeNumerator", "TimeDenominator")
SELECT "Id", "JobOfferId", "EmploymentType", "SalaryRange_IsPublished", "SalaryRange_From", "SalaryRange_To", "TimeNumerator", "TimeDenominator"
FROM "t_ContractDetails"
ON CONFLICT ("PublishedJobOfferId", "Id") DO UPDATE
    SET "EmploymentType" = EXCLUDED."EmploymentType",
        "SalaryRange_IsPublished" = EXCLUDED."SalaryRange_IsPublished",
        "SalaryRange_From" = EXCLUDED."SalaryRange_From",
        "SalaryRange_To" = EXCLUDED."SalaryRange_To",
        "TimeNumerator" = EXCLUDED."TimeNumerator",
        "TimeDenominator" = EXCLUDED."TimeDenominator";

-- Merge equivalent for Skills
INSERT INTO "job_board"."Skills" ("Id", "RequirementsPublishedJobOfferId", "Label", "Level")
SELECT "Id", "JobOfferId", "SkillLabel", "SkillLevel"
FROM "t_Skills"
ON CONFLICT ("RequirementsPublishedJobOfferId", "Id") DO UPDATE
    SET "Label" = EXCLUDED."Label",
        "Level" = EXCLUDED."Level";

-- Drop temporary tables
DROP TABLE "t_PublishedJobOffers";
DROP TABLE "t_ContractDetails";
DROP TABLE "t_Skills";
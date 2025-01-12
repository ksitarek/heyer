-- Table: public.StorageRegistryEntries
CREATE TABLE public."StorageRegistryEntries"
(
    "Key"         VARCHAR(450) NOT NULL PRIMARY KEY,
    "ContentType" TEXT         NOT NULL,
    "CreatedAt"   TIMESTAMP    NOT NULL,
    "FileName"    TEXT         NOT NULL,
    "Preserve"    BOOLEAN      NOT NULL,
    "Size"        BIGINT       NOT NULL
);
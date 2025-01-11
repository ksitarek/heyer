CREATE TABLE dbo.StorageRegistryEntries
(
    [Key]       NVARCHAR(450) NOT NULL,
    ContentType NVARCHAR(MAX) NOT NULL,
    CreatedAt   DATETIME2     NOT NULL,
    FileName    NVARCHAR(MAX) NOT NULL,
    Preserve    BIT           NOT NULL,
    Size        BIGINT        NOT NULL,

    CONSTRAINT PK_StorageRegistryEntries PRIMARY KEY ([Key])
)
GO
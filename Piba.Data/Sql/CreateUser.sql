DECLARE @email VARCHAR(MAX) = '{youruser}';

INSERT INTO
    [Users] (
        Id,
        SecurityStamp,
        UserName,
        Email,
        NormalizedEmail,
        AccessFailedCount,
        EmailConfirmed,
        PhoneNumberConfirmed,
        TwoFactorEnabled,
        LockoutEnabled
    )
VALUES
    (
        NEWID(),
        NEWID(),
        @email,
        @email,
        UPPER(@email),
        0,
        0,
        0,
        0,
        0
    );
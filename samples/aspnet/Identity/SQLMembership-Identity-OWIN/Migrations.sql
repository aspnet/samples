IF OBJECT_ID('AspNetUserRoles', 'U') IS NOT NULL
BEGIN
DROP TABLE AspNetUserRoles;
END

IF OBJECT_ID('AspNetUserClaims', 'U') IS NOT NULL
BEGIN
DROP TABLE AspNetUserClaims;
END

IF OBJECT_ID('AspNetUserLogins', 'U') IS NOT NULL
BEGIN
DROP TABLE AspNetUserLogins;
END

IF OBJECT_ID('AspNetRoles', 'U') IS NOT NULL
BEGIN
DROP TABLE AspNetRoles;
END

IF OBJECT_ID('AspNetUsers', 'U') IS NOT NULL
BEGIN
DROP TABLE AspNetUsers;
END

CREATE TABLE [dbo].[AspNetUsers] (
    [Id]            NVARCHAR (128) NOT NULL,
    [UserName]      NVARCHAR (MAX) NULL,
    [PasswordHash]  NVARCHAR (MAX) NULL,
    [SecurityStamp] NVARCHAR (MAX) NULL,
    [Discriminator] NVARCHAR (128) NOT NULL,
    [ApplicationId]                          UNIQUEIDENTIFIER NOT NULL,
    [LegacyPasswordHash]  NVARCHAR (MAX) NULL,
    [LoweredUserName]  NVARCHAR (256)   NOT NULL,
    [MobileAlias]      NVARCHAR (16)    DEFAULT (NULL) NULL,
    [IsAnonymous]      BIT              DEFAULT ((0)) NOT NULL,
    [LastActivityDate] DATETIME2         NOT NULL,
    [MobilePIN]                              NVARCHAR (16)    NULL,
    [Email]                                  NVARCHAR (256)   NULL,
    [LoweredEmail]                           NVARCHAR (256)   NULL,
    [PasswordQuestion]                       NVARCHAR (256)   NULL,
    [PasswordAnswer]                         NVARCHAR (128)   NULL,
    [IsApproved]                             BIT              NOT NULL,
    [IsLockedOut]                            BIT              NOT NULL,
    [CreateDate]                             DATETIME2	         NOT NULL,
    [LastLoginDate]                          DATETIME2         NOT NULL,
    [LastPasswordChangedDate]                DATETIME2         NOT NULL,
    [LastLockoutDate]                        DATETIME2         NOT NULL,
    [FailedPasswordAttemptCount]             INT              NOT NULL,
    [FailedPasswordAttemptWindowStart]       DATETIME2         NOT NULL,
    [FailedPasswordAnswerAttemptCount]       INT              NOT NULL,
    [FailedPasswordAnswerAttemptWindowStart] DATETIME2         NOT NULL,
    [Comment]                                NTEXT            NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId]),
);

INSERT INTO AspNetUsers(Id,UserName,PasswordHash,Discriminator,SecurityStamp,
ApplicationId,LoweredUserName,MobileAlias,IsAnonymous,LastActivityDate,LegacyPasswordHash,
MobilePIN,Email,LoweredEmail,PasswordQuestion,PasswordAnswer,IsApproved,IsLockedOut,CreateDate,
LastLoginDate,LastPasswordChangedDate,LastLockoutDate,FailedPasswordAttemptCount,
FailedPasswordAnswerAttemptWindowStart,FailedPasswordAnswerAttemptCount,FailedPasswordAttemptWindowStart,Comment)
SELECT aspnet_Users.UserId,aspnet_Users.UserName,(aspnet_Membership.Password+'|'+CAST(aspnet_Membership.PasswordFormat as varchar)+'|'+aspnet_Membership.PasswordSalt),'User',NewID(),aspnet_Users.ApplicationId,aspnet_Users.LoweredUserName,
aspnet_Users.MobileAlias,aspnet_Users.IsAnonymous,aspnet_Users.LastActivityDate,aspnet_Membership.Password,
aspnet_Membership.MobilePIN,aspnet_Membership.Email,aspnet_Membership.LoweredEmail,aspnet_Membership.PasswordQuestion,aspnet_Membership.PasswordAnswer,
aspnet_Membership.IsApproved,aspnet_Membership.IsLockedOut,aspnet_Membership.CreateDate,aspnet_Membership.LastLoginDate,aspnet_Membership.LastPasswordChangedDate,
aspnet_Membership.LastLockoutDate,aspnet_Membership.FailedPasswordAttemptCount, aspnet_Membership.FailedPasswordAnswerAttemptWindowStart,
aspnet_Membership.FailedPasswordAnswerAttemptCount,aspnet_Membership.FailedPasswordAttemptWindowStart,aspnet_Membership.Comment
FROM aspnet_Users
LEFT OUTER JOIN aspnet_Membership ON aspnet_Membership.ApplicationId = aspnet_Users.ApplicationId 
AND aspnet_Users.UserId = aspnet_Membership.UserId;

CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY NONCLUSTERED ([Id] ASC),
);

INSERT INTO AspNetRoles(Id,Name)
SELECT RoleId,RoleName
FROM aspnet_Roles;

CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

INSERT INTO AspNetUserRoles(UserId,RoleId)
SELECT UserId,RoleId
FROM aspnet_UsersInRoles;

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    [User_Id]    NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_User_Id]
    ON [dbo].[AspNetUserClaims]([User_Id] ASC);

CREATE TABLE [dbo].[AspNetUserLogins] (
    [UserId]        NVARCHAR (128) NOT NULL,
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [ProviderKey] ASC),
    CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserLogins]([UserId] ASC);

/****** Object: Table [dbo].[AspNetRoles] Script Date: 11/14/2013 1:56:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID('dbo.AspNetUserRoles', 'U') IS NOT NULL
	DROP TABLE [dbo].[AspNetUserRoles]
GO
IF OBJECT_ID('dbo.AspNetUserLogins', 'U') IS NOT NULL
	DROP TABLE [dbo].[AspNetUserLogins]
GO
IF OBJECT_ID('dbo.AspNetUserClaims', 'U') IS NOT NULL
	DROP TABLE [dbo].[AspNetUserClaims]
GO
IF OBJECT_ID('dbo.AspNetRoles', 'U') IS NOT NULL
	DROP TABLE [dbo].[AspNetRoles]
GO
IF OBJECT_ID('dbo.AspNetUsers', 'U') IS NOT NULL
	DROP TABLE [dbo].[AspNetUsers]
GO

CREATE TABLE [dbo].[AspNetUsers] (
    [AccessFailedCount]    INT            NOT NULL,
    [Email]                NVARCHAR (MAX) NULL,
    [EmailConfirmed]       BIT            DEFAULT ((0)) NULL,
    [Id]                   NVARCHAR (128) NOT NULL,
    [LockoutEnabled]       BIT            DEFAULT ((0)) NULL,
    [LockoutEndDateUtc]           DATETIME2 (7)  NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            DEFAULT ((0)) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [TwoFactorEnabled]     BIT            DEFAULT ((0)) NULL,
    [UserName]             NVARCHAR (MAX) NULL,
	[CreateDate]                              DATETIME       NULL,
    [ConfirmationToken]                       NVARCHAR (128) NULL,
    [IsConfirmed]                             BIT            DEFAULT ((0)) NULL,
    [LastPasswordFailureDate]                 DATETIME       NULL,
    [PasswordFailuresSinceLastSuccess]        INT            DEFAULT ((0)) NULL,
	[PasswordChangedDate]                     DATETIME       NULL,
    [PasswordVerificationToken]               NVARCHAR (128) NULL,
    [PasswordVerificationTokenExpirationDate] DATETIME       NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO
CREATE NONCLUSTERED INDEX [IX_RoleId]
    ON [dbo].[AspNetUserRoles]([RoleId] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_UserId]
    ON [dbo].[AspNetUserRoles]([UserId] ASC);
GO
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
GO
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    [UserId]    NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO
CREATE NONCLUSTERED INDEX [IX_User_Id]
    ON [dbo].[AspNetUserClaims]([UserId] ASC);
GO

INSERT INTO AspNetUsers(Id, UserName, PasswordHash, SecurityStamp,
CreateDate, ConfirmationToken, IsConfirmed, LastPasswordFailureDate, PasswordFailuresSinceLastSuccess,
PasswordChangedDate, PasswordVerificationToken, PasswordVerificationTokenExpirationDate,AccessFailedCount,LockoutEndDateUtc)
SELECT UserProfile.UserId, UserProfile.UserName, webpages_Membership.Password, 
webpages_Membership.PasswordSalt,  CreateDate, 
ConfirmationToken, IsConfirmed, LastPasswordFailureDate, PasswordFailuresSinceLastSuccess,
PasswordChangedDate, PasswordVerificationToken, PasswordVerificationTokenExpirationDate,0,'1/1/1977'
FROM UserProfile
LEFT OUTER JOIN webpages_Membership ON UserProfile.UserId = webpages_Membership.UserId
GO

INSERT INTO AspNetRoles(Id, Name)
SELECT RoleId, RoleName
FROM webpages_Roles
GO

INSERT INTO AspNetUserRoles(UserId, RoleId)
SELECT UserId, RoleId
FROM webpages_UsersInRoles
GO

INSERT INTO AspNetUserLogins(UserId, LoginProvider, ProviderKey)
SELECT UserId, Provider, ProviderUserId
FROM webpages_OAuthMembership
GO
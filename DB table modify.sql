USE [TEST]
GO

/****** Object:  Table [dbo].[ProviderUser]    Script Date: 9/1/2024 10:17:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProviderUser](
	[ProviderID] [int] IDENTITY(1,1) NOT NULL,
	[ProviderName] [varchar](250) NULL,
	[Email] [varchar](255) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[RoleId] [int] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[FirstName] [varchar](150) NULL,
	[LastName] [varchar](150) NULL,
	[PhoneNumber] [varchar](100) NULL,
	[Address] [varchar](max) NULL,
	[PostalCode] [varchar](10) NULL,
	[MemberSince] [datetime] NULL,
	[ProfilePicture] [varchar](250) NULL,
	[Passport] [varchar](250) NULL,
	[EmiratesId] [varchar](250) NULL,
	[DrivingLicense] [varchar](250) NULL,
	[isVerified] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProviderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProviderUser] ADD  DEFAULT ((0)) FOR [IsActive]
GO

ALTER TABLE [dbo].[ProviderUser] ADD  DEFAULT ((0)) FOR [isVerified]
GO

ALTER TABLE [dbo].[ProviderUser]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO



USE [TEST]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActionLog]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActionLog](
	[LogID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[ActionTimestamp] [datetime] NOT NULL,
	[ActionType] [varchar](100) NOT NULL,
	[ActionDetails] [varchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdminDashboard]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminDashboard](
	[AdminDashboardID] [int] NOT NULL,
	[DashboardItemID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminDashboardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdminUsers]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminUsers](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[UserName] [varchar](250) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[RoleId] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChatMessage]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatMessage](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[SenderID] [int] NOT NULL,
	[ReceiverID] [int] NOT NULL,
	[MessageType] [varchar](50) NOT NULL,
	[MessageContent] [varchar](max) NOT NULL,
	[MessageTimestamp] [datetime] NOT NULL,
	[IsRead] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[MerchantID] [int] NULL,
 CONSTRAINT [PK_ChatMessage] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [varchar](250) NOT NULL,
	[Email] [varchar](250) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[RoleId] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[FirstName] [varchar](150) NULL,
	[LastName] [varchar](150) NULL,
	[PhoneNumber] [varchar](100) NULL,
	[Address] [varchar](max) NULL,
	[PostalCode] [varchar](10) NULL,
	[ProfilePicture] [varchar](max) NULL,
	[MemberSince] [datetime] NULL,
	[DOB] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DashboardItems]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DashboardItems](
	[DashboardItemID] [int] NOT NULL,
	[DashboardItemType] [varchar](100) NOT NULL,
	[DashboardItemName] [varchar](255) NOT NULL,
	[Description] [varchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[DashboardItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Document]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document](
	[DocumentID] [int] IDENTITY(1,1) NOT NULL,
	[UploadedBy] [int] NOT NULL,
	[DocumentName] [varchar](255) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[MerchantID] [int] NULL,
	[MerchantUserID] [int] NULL,
 CONSTRAINT [PK_Document] PRIMARY KEY CLUSTERED 
(
	[DocumentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[GroupID] [int] NOT NULL,
	[GroupName] [varchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoginHistory]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoginHistory](
	[LogID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[LoginTimestamp] [datetime] NOT NULL,
	[LogoutTimestamp] [datetime] NULL,
	[IPAddress] [varchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menu]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menu](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[MenuName] [varchar](250) NOT NULL,
	[MenuUrl] [varchar](250) NOT NULL,
	[Description] [varchar](250) NULL,
	[MenuIcon] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MenuPermissions]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuPermissions](
	[MenuPermissionID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[UserID] [int] NULL,
	[GroupID] [int] NULL,
	[MenuID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
	[CreatedDate] [datetime2](3) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime2](3) NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MenuPermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Merchant]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Merchant](
	[MerchantID] [int] NOT NULL,
	[CompanyName] [varchar](255) NOT NULL,
	[ContactInfo] [varchar](255) NOT NULL,
	[RegistrationMethod] [varchar](100) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CompanyRegistrationNumber] [varchar](255) NOT NULL,
	[TradingLicense] [varchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[EmiratesId] [varchar](255) NULL,
	[Deactivate] [bit] NULL,
	[MerchantLocation] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MerchantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MerchantDashboard]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantDashboard](
	[MerchantDashboardID] [int] NOT NULL,
	[DashboardItemID] [int] NOT NULL,
	[MerchantID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MerchantDashboardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MerchantRating]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantRating](
	[RatingID] [int] NOT NULL,
	[RatedByUserID] [int] NOT NULL,
	[RatedMerchantID] [int] NOT NULL,
	[RatingValue] [int] NOT NULL,
	[Comments] [varchar](max) NULL,
	[RatedDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RatingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MerchantUser]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantUser](
	[MerchantUserID] [int] NOT NULL,
	[Username] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[PasswordHash] [varchar](255) NOT NULL,
	[MerchantID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[GroupID] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[IsAdmin] [bit] NULL,
	[lastlogindate] [datetime] NULL,
	[rfu1] [varchar](255) NULL,
	[rfu2] [varchar](255) NULL,
	[rfu3] [varchar](255) NULL,
	[rfu4] [varchar](255) NULL,
	[rfu5] [varchar](255) NULL,
	[rfu6] [varchar](255) NULL,
	[rfu7] [varchar](255) NULL,
	[rfu8] [varchar](255) NULL,
	[rfu9] [varchar](255) NULL,
	[rfu10] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[MerchantUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MerchantUserDashboard]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantUserDashboard](
	[MerchantUserDashboardID] [int] NOT NULL,
	[DashboardItemID] [int] NOT NULL,
	[MerchantUserID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MerchantUserDashboardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MerchantUserRating]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MerchantUserRating](
	[RatingID] [int] NOT NULL,
	[RatedByUserID] [int] NOT NULL,
	[RatedMerchantUserID] [int] NOT NULL,
	[RatingValue] [int] NOT NULL,
	[Comments] [varchar](max) NULL,
	[RatedDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RatingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentHistory]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PAYMENTTYPE] [varchar](50) NULL,
	[AMOUNT] [varchar](50) NULL,
	[PAYERID] [int] NULL,
	[MERCHANTID] [int] NULL,
	[ISPAYMENTSUCCESS] [int] NULL,
	[SERVICEID] [int] NULL,
	[PAYMENTDATETIME] [datetime] NULL,
 CONSTRAINT [PK_PaymentHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[PermissionId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NULL,
	[MenuId] [int] NULL,
	[CanCreate] [bit] NULL,
	[CanRead] [bit] NULL,
	[CanUpdate] [bit] NULL,
	[CanDelete] [bit] NULL,
	[CanView] [bit] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Policies]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Policies](
	[PolicyID] [int] NOT NULL,
	[PolicyName] [varchar](255) NOT NULL,
	[PolicyContent] [varchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PolicyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProviderUser]    Script Date: 7/29/2024 10:41:36 PM ******/
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
PRIMARY KEY CLUSTERED 
(
	[ProviderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestForDisCountToMerchant]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestForDisCountToMerchant](
	[RFDTM] [int] IDENTITY(1,1) NOT NULL,
	[SID] [int] NOT NULL,
	[MID] [int] NOT NULL,
	[UID] [int] NOT NULL,
	[IsResponseSent] [int] NULL,
	[RequestDateTime] [datetime] NULL,
 CONSTRAINT [PK_RequestForDisCountToMerchant] PRIMARY KEY CLUSTERED 
(
	[RFDTM] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestForDisCountToUser]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestForDisCountToUser](
	[SID] [int] NOT NULL,
	[MID] [int] NOT NULL,
	[FINALPRICE] [int] NOT NULL,
	[UID] [int] NOT NULL,
	[RFDFU] [int] IDENTITY(1,1) NOT NULL,
	[ResponseDateTime] [datetime] NULL,
	[IsMerchantSelected] [int] NULL,
	[IsPaymentDone] [int] NULL,
 CONSTRAINT [PK_RequestForDisCountToUser] PRIMARY KEY CLUSTERED 
(
	[RFDFU] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](255) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[ServiceID] [int] NOT NULL,
	[CategoryID] [int] NULL,
	[MerchantID] [int] NULL,
	[ServiceName] [varchar](100) NOT NULL,
	[Description] [text] NULL,
	[ServicePrice] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SERVICE_DOCUMENTS]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SERVICE_DOCUMENTS](
	[SERVICE_DOCUMENT_ID] [int] IDENTITY(1,1) NOT NULL,
	[SERVICE_ID] [int] NULL,
	[DOCUMENT_DETAIL] [text] NULL,
 CONSTRAINT [PK_SERVICE_DOCUMENTS] PRIMARY KEY CLUSTERED 
(
	[SERVICE_DOCUMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceCategory]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceCategory](
	[CategoryID] [int] NOT NULL,
	[CategoryName] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubMenu]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubMenu](
	[SubMenuId] [int] IDENTITY(1,1) NOT NULL,
	[SubMenuName] [varchar](250) NOT NULL,
	[SubMenuUrl] [varchar](250) NOT NULL,
	[Description] [varchar](250) NULL,
	[MenuId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SubMenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubscribeChannel]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubscribeChannel](
	[SubscriptionChannelId] [int] IDENTITY(1,1) NOT NULL,
	[SubscriptionChannelType] [varchar](255) NULL,
 CONSTRAINT [PK_SubscribeChannel] PRIMARY KEY CLUSTERED 
(
	[SubscriptionChannelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SuperUserDashboard]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuperUserDashboard](
	[SuperUserDashboardID] [int] NOT NULL,
	[DashboardItemID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SuperUserDashboardID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UploadedFile]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UploadedFile](
	[UFID] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](max) NULL,
	[ContentType] [varchar](max) NULL,
	[FileSize] [bigint] NULL,
	[FolderName] [varchar](max) NULL,
	[Status] [varchar](50) NULL,
	[UserId] [int] NULL,
	[DocumentAddedDate] [datetime] NULL,
	[DocumentModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_UploadedFiles] PRIMARY KEY CLUSTERED 
(
	[UFID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserGroupPermissions]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserGroupPermissions](
	[UserGroupPermissionID] [int] NOT NULL,
	[GroupID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserGroupPermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRolePermissions]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRolePermissions](
	[UserRolePermissionID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
	[PermissionID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[CanCreate] [bit] NOT NULL,
	[CanRead] [bit] NOT NULL,
	[CanUpdate] [bit] NOT NULL,
	[CanDelete] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserRolePermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] NOT NULL,
	[Username] [varchar](255) NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[PasswordHash] [varchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[RoleID] [int] NOT NULL,
	[GroupID] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[PhoneNumber] [varchar](50) NULL,
	[SubscriptionChannelId] [int] NULL,
	[lastlogindate] [datetime] NULL,
	[rfu1] [varchar](255) NULL,
	[rfu2] [varchar](255) NULL,
	[rfu3] [varchar](255) NULL,
	[rfu4] [varchar](255) NULL,
	[rfu5] [varchar](255) NULL,
	[rfu6] [varchar](255) NULL,
	[rfu7] [varchar](255) NULL,
	[rfu8] [varchar](255) NULL,
	[rfu9] [varchar](255) NULL,
	[rfu10] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserSubscriptionChannel]    Script Date: 7/29/2024 10:41:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSubscriptionChannel](
	[SubscriptionId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[SubscriptionType] [int] NULL,
 CONSTRAINT [PK_UserSubscriptionChannel] PRIMARY KEY CLUSTERED 
(
	[SubscriptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AdminUsers] ON 
GO
INSERT [dbo].[AdminUsers] ([UserId], [Email], [UserName], [Password], [RoleId], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy]) VALUES (1, N'admin@admin.com', N'Admin', N'12345', 3, CAST(N'2024-06-21T16:42:31.767' AS DateTime), 0, CAST(N'2024-06-21T16:42:31.767' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[AdminUsers] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 
GO
INSERT [dbo].[Customer] ([CustomerId], [CustomerName], [Email], [Password], [RoleId], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [FirstName], [LastName], [PhoneNumber], [Address], [PostalCode], [ProfilePicture], [MemberSince], [DOB]) VALUES (1, N'test', N'test@test.com', N'9nPVSNL3F7h3seBWnuckDA==', 1, CAST(N'2024-07-09T20:34:26.587' AS DateTime), 1, NULL, NULL, N'zain', N'maqbool', N'+971000000', N'test address', N'000000', N'D:\TempTesting\AFFZ_MVC\ProfilePictures\Profile_1.jpg', NULL, CAST(N'2024-04-07' AS Date))
GO
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Menu] ON 
GO
INSERT [dbo].[Menu] ([MenuId], [MenuName], [MenuUrl], [Description], [MenuIcon]) VALUES (1, N'Dashboard', N'/UserDashboard', N'User Dashboard', N'feather-grid')
GO
INSERT [dbo].[Menu] ([MenuId], [MenuName], [MenuUrl], [Description], [MenuIcon]) VALUES (2, N'Bookings', N'/UserBookings', N'User Bookings', N'feather-smartphone')
GO
INSERT [dbo].[Menu] ([MenuId], [MenuName], [MenuUrl], [Description], [MenuIcon]) VALUES (3, N'Reviews', N'/UserReviews', N'User Reviews', N'feather-star')
GO
INSERT [dbo].[Menu] ([MenuId], [MenuName], [MenuUrl], [Description], [MenuIcon]) VALUES (4, N'Chats', N'/UserChats', N'User Chats', N'feather-message-circle')
GO
SET IDENTITY_INSERT [dbo].[Menu] OFF
GO
INSERT [dbo].[Merchant] ([MerchantID], [CompanyName], [ContactInfo], [RegistrationMethod], [IsActive], [CompanyRegistrationNumber], [TradingLicense], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [EmiratesId], [Deactivate], [MerchantLocation]) VALUES (1, N'placeholder
Ahlan Typing & Document Copying ', N'06-5730319', N'Direct', 1, N'123456', N'1', CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, N'8741975606895078', 0, N'Sharjah')
GO
INSERT [dbo].[Merchant] ([MerchantID], [CompanyName], [ContactInfo], [RegistrationMethod], [IsActive], [CompanyRegistrationNumber], [TradingLicense], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [EmiratesId], [Deactivate], [MerchantLocation]) VALUES (2, N'Al Ahli Business Center', N'06-5730319', N'Direct', 1, N'123456', N'1', CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, N'8741975606895078', 0, N'Sharjah')
GO
INSERT [dbo].[Merchant] ([MerchantID], [CompanyName], [ContactInfo], [RegistrationMethod], [IsActive], [CompanyRegistrationNumber], [TradingLicense], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [EmiratesId], [Deactivate], [MerchantLocation]) VALUES (4, N'Al Bayan Documents Typing and Copying', N'06-5738685', N'Direct', 1, N'123456', N'1', CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, N'8741975606895078', 0, N'Al QasmiyaCity Sharjah')
GO
INSERT [dbo].[Merchant] ([MerchantID], [CompanyName], [ContactInfo], [RegistrationMethod], [IsActive], [CompanyRegistrationNumber], [TradingLicense], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [EmiratesId], [Deactivate], [MerchantLocation]) VALUES (5, N'Al Bidaya Management Consultancy', N'06-5638728', N'Direct', 1, N'123456', N'1', CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, N'8741975606895078', 0, N'Al QasmiyaCity Sharjah')
GO
INSERT [dbo].[Merchant] ([MerchantID], [CompanyName], [ContactInfo], [RegistrationMethod], [IsActive], [CompanyRegistrationNumber], [TradingLicense], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [EmiratesId], [Deactivate], [MerchantLocation]) VALUES (6, N'Al Burkan Typing and Document Copying', N'06-56212166', N'Direct', 1, N'123456', N'1', CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, N'8741975606895078', 0, N'Zahra HospCity Sharjah')
GO
INSERT [dbo].[Merchant] ([MerchantID], [CompanyName], [ContactInfo], [RegistrationMethod], [IsActive], [CompanyRegistrationNumber], [TradingLicense], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [EmiratesId], [Deactivate], [MerchantLocation]) VALUES (7, N'Al Durah', N'06-5746225', N'Direct', 1, N'123456', N'1', CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, N'8741975606895078', 0, N'Zahra HospCity Sharjah')
GO
INSERT [dbo].[Merchant] ([MerchantID], [CompanyName], [ContactInfo], [RegistrationMethod], [IsActive], [CompanyRegistrationNumber], [TradingLicense], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [EmiratesId], [Deactivate], [MerchantLocation]) VALUES (8, N'Al Falak Businessmen Services', N' ', N'Direct', 1, N'123456', N'1', CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, N'8741975606895078', 0, N'Al Istaqlal StreetCity Sharjah')
GO
INSERT [dbo].[Merchant] ([MerchantID], [CompanyName], [ContactInfo], [RegistrationMethod], [IsActive], [CompanyRegistrationNumber], [TradingLicense], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [EmiratesId], [Deactivate], [MerchantLocation]) VALUES (9, N'Al Fouz Typing Centre', N'06-5668394', N'Direct', 1, N'123456', N'1', CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, N'8741975606895078', 0, N'Near Marthoma Church Bhnd Labour Office Sharjah')
GO
INSERT [dbo].[Merchant] ([MerchantID], [CompanyName], [ContactInfo], [RegistrationMethod], [IsActive], [CompanyRegistrationNumber], [TradingLicense], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [EmiratesId], [Deactivate], [MerchantLocation]) VALUES (10, N'Al Jameel Typing and Doc Copying', N'06-5635837', N'Direct', 1, N'123456', N'1', CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, CAST(N'2024-06-07T10:13:17.457' AS DateTime), 1, N'8741975606895078', 0, N'Al Zahra RdCity Sharjah')
GO
SET IDENTITY_INSERT [dbo].[PaymentHistory] ON 
GO
INSERT [dbo].[PaymentHistory] ([ID], [PAYMENTTYPE], [AMOUNT], [PAYERID], [MERCHANTID], [ISPAYMENTSUCCESS], [SERVICEID], [PAYMENTDATETIME]) VALUES (1, N'COD', N'900.00', 1, 1, 1, 49, CAST(N'2024-07-13T16:03:23.953' AS DateTime))
GO
INSERT [dbo].[PaymentHistory] ([ID], [PAYMENTTYPE], [AMOUNT], [PAYERID], [MERCHANTID], [ISPAYMENTSUCCESS], [SERVICEID], [PAYMENTDATETIME]) VALUES (2, N'COD', N'200.00', 1, 1, 1, 1, CAST(N'2024-07-28T22:29:30.887' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[PaymentHistory] OFF
GO
SET IDENTITY_INSERT [dbo].[Permissions] ON 
GO
INSERT [dbo].[Permissions] ([PermissionId], [RoleId], [MenuId], [CanCreate], [CanRead], [CanUpdate], [CanDelete], [CanView], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy]) VALUES (1, 1, 1, 1, 1, 1, 1, 1, CAST(N'2024-06-21T20:03:15.723' AS DateTime), 0, CAST(N'2024-06-21T20:03:15.723' AS DateTime), 0)
GO
INSERT [dbo].[Permissions] ([PermissionId], [RoleId], [MenuId], [CanCreate], [CanRead], [CanUpdate], [CanDelete], [CanView], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy]) VALUES (3, 1, 2, 1, 1, 1, 1, 0, CAST(N'2024-06-22T16:32:37.460' AS DateTime), 1, CAST(N'2024-06-22T16:32:37.460' AS DateTime), NULL)
GO
INSERT [dbo].[Permissions] ([PermissionId], [RoleId], [MenuId], [CanCreate], [CanRead], [CanUpdate], [CanDelete], [CanView], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy]) VALUES (4, 1, 3, 0, 0, 0, 0, 1, CAST(N'2024-06-22T16:32:37.460' AS DateTime), 1, CAST(N'2024-06-22T16:32:37.460' AS DateTime), NULL)
GO
INSERT [dbo].[Permissions] ([PermissionId], [RoleId], [MenuId], [CanCreate], [CanRead], [CanUpdate], [CanDelete], [CanView], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy]) VALUES (5, 1, 4, 0, 0, 0, 0, 1, CAST(N'2024-06-22T16:32:37.460' AS DateTime), 1, CAST(N'2024-06-22T16:32:37.460' AS DateTime), NULL)
GO
SET IDENTITY_INSERT [dbo].[Permissions] OFF
GO
SET IDENTITY_INSERT [dbo].[ProviderUser] ON 
GO
INSERT [dbo].[ProviderUser] ([ProviderID], [ProviderName], [Email], [Password], [RoleId], [IsActive], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy], [FirstName], [LastName], [PhoneNumber], [Address], [PostalCode], [MemberSince], [ProfilePicture]) VALUES (2, NULL, N'test@test.com', N'9nPVSNL3F7h3seBWnuckDA==', 5, 0, CAST(N'2024-07-10T23:24:02.987' AS DateTime), 1, NULL, NULL, NULL, NULL, N'+9710000000', NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ProviderUser] OFF
GO
SET IDENTITY_INSERT [dbo].[RequestForDisCountToMerchant] ON 
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (1, 1, 1, 1, 1, CAST(N'2024-06-22T20:54:02.690' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (2, 13, 4, 13, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (3, 8, 2, 8, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (4, 32, 7, 32, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (5, 44, 9, 44, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (6, 8, 2, 8, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (7, 44, 9, 44, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (8, 2, 1, 2, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (9, 2, 1, 1, 1, CAST(N'2024-06-24T13:03:51.490' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (10, 32, 7, 32, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (11, 26, 6, 26, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (12, 14, 4, 14, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (13, 26, 6, 26, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (14, 44, 9, 44, 0, CAST(N'2024-06-22T17:17:17.097' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (15, 3, 1, 1, 1, CAST(N'2024-06-22T23:54:15.700' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (16, 37, 8, 37, 0, CAST(N'2024-06-24T13:03:13.390' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (17, 53, 1, 1, 1, CAST(N'2024-07-01T12:25:10.970' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (18, 37, 8, 37, 0, CAST(N'2024-07-05T23:22:38.787' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (19, 1, 1, 1, 1, CAST(N'2024-07-05T23:23:23.223' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (20, 1, 1, 1, 1, CAST(N'2024-07-05T23:36:50.537' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (21, 49, 1, 1, 1, CAST(N'2024-07-13T14:45:37.733' AS DateTime))
GO
INSERT [dbo].[RequestForDisCountToMerchant] ([RFDTM], [SID], [MID], [UID], [IsResponseSent], [RequestDateTime]) VALUES (22, 1, 1, 1, 1, CAST(N'2024-07-28T22:16:18.743' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[RequestForDisCountToMerchant] OFF
GO
SET IDENTITY_INSERT [dbo].[RequestForDisCountToUser] ON 
GO
INSERT [dbo].[RequestForDisCountToUser] ([SID], [MID], [FINALPRICE], [UID], [RFDFU], [ResponseDateTime], [IsMerchantSelected], [IsPaymentDone]) VALUES (1, 1, 300, 1, 1, CAST(N'2024-06-22T22:14:32.093' AS DateTime), 0, 0)
GO
INSERT [dbo].[RequestForDisCountToUser] ([SID], [MID], [FINALPRICE], [UID], [RFDFU], [ResponseDateTime], [IsMerchantSelected], [IsPaymentDone]) VALUES (3, 1, 1000, 1, 2, CAST(N'2024-06-22T23:54:15.737' AS DateTime), 0, 0)
GO
INSERT [dbo].[RequestForDisCountToUser] ([SID], [MID], [FINALPRICE], [UID], [RFDFU], [ResponseDateTime], [IsMerchantSelected], [IsPaymentDone]) VALUES (2, 1, 150, 1, 3, CAST(N'2024-06-24T13:03:51.510' AS DateTime), 0, 0)
GO
INSERT [dbo].[RequestForDisCountToUser] ([SID], [MID], [FINALPRICE], [UID], [RFDFU], [ResponseDateTime], [IsMerchantSelected], [IsPaymentDone]) VALUES (53, 1, 150, 1, 4, CAST(N'2024-07-01T13:28:59.527' AS DateTime), 1, 0)
GO
INSERT [dbo].[RequestForDisCountToUser] ([SID], [MID], [FINALPRICE], [UID], [RFDFU], [ResponseDateTime], [IsMerchantSelected], [IsPaymentDone]) VALUES (1, 1, 325, 1, 6, CAST(N'2024-07-05T23:23:38.840' AS DateTime), 1, 0)
GO
INSERT [dbo].[RequestForDisCountToUser] ([SID], [MID], [FINALPRICE], [UID], [RFDFU], [ResponseDateTime], [IsMerchantSelected], [IsPaymentDone]) VALUES (1, 1, 320, 1, 7, CAST(N'2024-07-05T23:37:19.400' AS DateTime), 1, 1)
GO
INSERT [dbo].[RequestForDisCountToUser] ([SID], [MID], [FINALPRICE], [UID], [RFDFU], [ResponseDateTime], [IsMerchantSelected], [IsPaymentDone]) VALUES (49, 1, 900, 1, 8, CAST(N'2024-07-13T16:13:26.077' AS DateTime), 1, 1)
GO
INSERT [dbo].[RequestForDisCountToUser] ([SID], [MID], [FINALPRICE], [UID], [RFDFU], [ResponseDateTime], [IsMerchantSelected], [IsPaymentDone]) VALUES (1, 1, 200, 1, 9, CAST(N'2024-07-28T22:29:31.297' AS DateTime), 1, 1)
GO
SET IDENTITY_INSERT [dbo].[RequestForDisCountToUser] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([RoleId], [RoleName], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy]) VALUES (1, N'Customers', CAST(N'2024-06-21T16:42:31.767' AS DateTime), 0, CAST(N'2024-06-21T16:42:31.767' AS DateTime), 0)
GO
INSERT [dbo].[Roles] ([RoleId], [RoleName], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy]) VALUES (3, N'Admin', CAST(N'2024-06-21T16:42:31.767' AS DateTime), 0, CAST(N'2024-06-21T16:42:31.767' AS DateTime), NULL)
GO
INSERT [dbo].[Roles] ([RoleId], [RoleName], [CreatedDate], [CreatedBy], [ModifyDate], [ModifiedBy]) VALUES (5, N'Merchant', CAST(N'2024-06-21T00:00:00.000' AS DateTime), 0, CAST(N'2024-06-21T00:00:00.000' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (1, 1, 1, N'1MonthVisa', N'1MonthVisa', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (2, 1, 1, N'2MonthVisa', N'2MonthVisa', 650)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (3, 1, 1, N'3MonthVisa', N'3MonthVisa', 1050)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (4, 1, 1, N'New Job Offer Letter', N'New Job Offer Letter', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (5, 6, 1, N'Renew Work Permit', N'Renew Work Permit', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (6, 6, 1, N'New Work Permit', N'New Work Permit', 500)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (7, 1, 2, N'1MonthVisa', N'1MonthVisa', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (8, 1, 2, N'2MonthVisa', N'2MonthVisa', 650)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (9, 1, 2, N'3MonthVisa', N'3MonthVisa', 1050)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (10, 1, 2, N'New Job Offer Letter', N'New Job Offer Letter', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (11, 6, 2, N'Renew Work Permit', N'Renew Work Permit', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (12, 6, 2, N'New Work Permit', N'New Work Permit', 500)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (13, 1, 4, N'1MonthVisa', N'1MonthVisa', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (14, 1, 4, N'2MonthVisa', N'2MonthVisa', 650)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (15, 1, 4, N'3MonthVisa', N'3MonthVisa', 1050)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (16, 1, 4, N'New Job Offer Letter', N'New Job Offer Letter', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (17, 6, 4, N'Renew Work Permit', N'Renew Work Permit', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (18, 6, 4, N'New Work Permit', N'New Work Permit', 500)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (19, 1, 5, N'1MonthVisa', N'1MonthVisa', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (20, 1, 5, N'2MonthVisa', N'2MonthVisa', 650)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (21, 1, 5, N'3MonthVisa', N'3MonthVisa', 1050)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (22, 1, 5, N'New Job Offer Letter', N'New Job Offer Letter', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (23, 6, 5, N'Renew Work Permit', N'Renew Work Permit', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (24, 6, 5, N'New Work Permit', N'New Work Permit', 500)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (25, 1, 6, N'1MonthVisa', N'1MonthVisa', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (26, 1, 6, N'2MonthVisa', N'2MonthVisa', 650)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (27, 1, 6, N'3MonthVisa', N'3MonthVisa', 1050)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (28, 1, 6, N'New Job Offer Letter', N'New Job Offer Letter', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (29, 6, 6, N'Renew Work Permit', N'Renew Work Permit', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (30, 6, 6, N'New Work Permit', N'New Work Permit', 500)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (31, 1, 7, N'1MonthVisa', N'1MonthVisa', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (32, 1, 7, N'2MonthVisa', N'2MonthVisa', 650)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (33, 1, 7, N'3MonthVisa', N'3MonthVisa', 1050)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (34, 1, 7, N'New Job Offer Letter', N'New Job Offer Letter', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (35, 6, 7, N'Renew Work Permit', N'Renew Work Permit', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (36, 6, 7, N'New Work Permit', N'New Work Permit', 500)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (37, 1, 8, N'1MonthVisa', N'1MonthVisa', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (38, 1, 8, N'2MonthVisa', N'2MonthVisa', 650)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (39, 1, 8, N'3MonthVisa', N'3MonthVisa', 1050)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (40, 1, 8, N'New Job Offer Letter', N'New Job Offer Letter', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (41, 6, 8, N'Renew Work Permit', N'Renew Work Permit', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (42, 6, 8, N'New Work Permit', N'New Work Permit', 500)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (43, 1, 9, N'1MonthVisa', N'1MonthVisa', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (44, 1, 9, N'2MonthVisa', N'2MonthVisa', 650)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (45, 1, 9, N'3MonthVisa', N'3MonthVisa', 1050)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (46, 1, 9, N'New Job Offer Letter', N'New Job Offer Letter', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (47, 6, 9, N'Renew Work Permit', N'Renew Work Permit', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (48, 6, 9, N'New Work Permit', N'New Work Permit', 500)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (49, 1, 1, N'Investor Visa Processing', N'Investor Visa Processing', 1000)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (50, 1, 10, N'House Maid Visa Processing', N'House Maid Visa Processing', 1000)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (51, 1, 10, N'Visa Cancellation Processing', N'Visa Cancellation Processing', 1000)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (52, 1, 10, N'Visa Stamping Processing', N'Visa Stamping Processing', 1000)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (53, 1, 1, N'Transit Visa Processing', N'Transit Visa Processing', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (54, 3, 1, N'EmiratesId Processing', N'EmiratesId Processing', 375)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (55, 3, 4, N'EmiratesId Processing', N'EmiratesId Processing', 375)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (56, 3, 5, N'EmiratesId Processing', N'EmiratesId Processing', 375)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (57, 3, 9, N'EmiratesId Processing', N'EmiratesId Processing', 375)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (58, 5, 1, N'Company Startup', N'Company Startup', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (59, 3, 1, N'Local Sponsorship Arrangement Trade License Amendment', N'Local Sponsorship Arrangement Trade License Amendment', 190)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (60, 6, 1, N'Office Arrangement', N'Office Arrangement', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (61, 6, 1, N'Company Stamp', N'Company Stamp', 100)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (62, 6, 1, N'Dubai Court', N'Dubai Court', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (63, 4, 1, N'Compnay Labour and Tasheel works', N'Compnay Labour and Tasheel works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (64, 1, 1, N'Family Visa New and Renewal', N'Family Visa New and Renewal', 982)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (65, 3, 1, N'Family under Wife Sponsorship', N'Family under Wife Sponsorship', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (66, 6, 1, N'Entry Permit Inside Entry Permit Out Side ', N'Entry Permit Inside Entry Permit Out Side ', 785)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (67, 1, 1, N'Chagne Status ', N'Chagne Status ', 335)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (68, 1, 1, N'Visa Stamping', N'Visa Stamping', 987)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (69, 1, 1, N'Visa Cancellation Inside', N'Visa Cancellation Inside', 157)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (70, 1, 1, N'Visa Cancellation Outside', N'Visa Cancellation Outside', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (71, 6, 1, N'Retun Permit', N'Retun Permit', 444)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (72, 4, 1, N'Passport Renewal Appointment', N'Passport Renewal Appointment', 744)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (73, 4, 1, N'Passport Renewal Travel Report', N'Passport Renewal Travel Report', 130)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (74, 4, 1, N'Health Insurance', N'Health Insurance', 140)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (75, 3, 1, N'Police Clearanc Certificate', N'Police Clearanc Certificate', 586)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (76, 3, 1, N'Normal Translation', N'Normal Translation', 687)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (77, 3, 1, N'Legal Translation.', N'Legal Translation.', 788)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (78, 3, 1, N'Birth Certificate Attestation', N'Birth Certificate Attestation', 258)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (79, 3, 1, N'Marriage Certificate Attestation', N'Marriage Certificate Attestation', 753)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (80, 3, 1, N'Education Certificates Attestation Jebel Ali Port Pass', N'Education Certificates Attestation Jebel Ali Port Pass', 167)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (81, 3, 1, N'Vehicle Insurance', N'Vehicle Insurance', 943)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (82, 3, 1, N'Driving License Renewal', N'Driving License Renewal', 349)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (83, 3, 1, N'Company Bank Account', N'Company Bank Account', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (84, 3, 1, N'Tenancy Contract Typing Visit Visa Services', N'Tenancy Contract Typing Visit Visa Services', 456)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (85, 3, 1, N'PRO Services', N'PRO Services', 964)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (86, 3, 1, N'RTA Letters and Online Services', N'RTA Letters and Online Services', 746)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (87, 3, 1, N'Update Mobile Number On Emirates ID', N'Update Mobile Number On Emirates ID', 550)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (88, 3, 1, N'Letter Typing', N'Letter Typing', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (89, 3, 1, N'Photocopying A3,A4', N'Photocopying A3,A4', 124)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (90, 3, 1, N'Document Scanning', N'Document Scanning', 457)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (91, 3, 1, N'Lamination', N'Lamination', 958)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (92, 3, 1, N'Company Immigration and Amer Works', N'Company Immigration and Amer Works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (93, 5, 2, N'Company Startup', N'Company Startup', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (94, 3, 2, N'Local Sponsorship Arrangement Trade License Amendment', N'Local Sponsorship Arrangement Trade License Amendment', 190)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (95, 6, 2, N'Office Arrangement', N'Office Arrangement', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (96, 6, 2, N'Company Stamp', N'Company Stamp', 100)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (97, 6, 2, N'Dubai Court', N'Dubai Court', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (98, 4, 2, N'Compnay Labour and Tasheel works', N'Compnay Labour and Tasheel works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (99, 1, 2, N'Family Visa New and Renewal', N'Family Visa New and Renewal', 982)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (100, 3, 2, N'Family under Wife Sponsorship', N'Family under Wife Sponsorship', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (101, 6, 2, N'Entry Permit Inside Entry Permit Out Side ', N'Entry Permit Inside Entry Permit Out Side ', 785)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (102, 1, 2, N'Chagne Status ', N'Chagne Status ', 335)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (103, 1, 2, N'Visa Stamping', N'Visa Stamping', 987)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (104, 1, 2, N'Visa Cancellation Inside', N'Visa Cancellation Inside', 157)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (105, 1, 2, N'Visa Cancellation Outside', N'Visa Cancellation Outside', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (106, 6, 2, N'Retun Permit', N'Retun Permit', 444)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (107, 4, 2, N'Passport Renewal Appointment', N'Passport Renewal Appointment', 744)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (108, 4, 2, N'Passport Renewal Travel Report', N'Passport Renewal Travel Report', 130)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (109, 4, 2, N'Health Insurance', N'Health Insurance', 140)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (110, 3, 2, N'Police Clearanc Certificate', N'Police Clearanc Certificate', 586)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (111, 3, 2, N'Normal Translation', N'Normal Translation', 687)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (112, 3, 2, N'Legal Translation.', N'Legal Translation.', 788)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (113, 3, 2, N'Birth Certificate Attestation', N'Birth Certificate Attestation', 258)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (114, 3, 2, N'Marriage Certificate Attestation', N'Marriage Certificate Attestation', 753)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (115, 3, 2, N'Education Certificates Attestation Jebel Ali Port Pass', N'Education Certificates Attestation Jebel Ali Port Pass', 167)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (116, 3, 2, N'Vehicle Insurance', N'Vehicle Insurance', 943)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (117, 3, 2, N'Driving License Renewal', N'Driving License Renewal', 349)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (118, 3, 2, N'Company Bank Account', N'Company Bank Account', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (119, 3, 2, N'Tenancy Contract Typing Visit Visa Services', N'Tenancy Contract Typing Visit Visa Services', 456)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (120, 3, 2, N'PRO Services', N'PRO Services', 964)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (121, 3, 2, N'RTA Letters and Online Services', N'RTA Letters and Online Services', 746)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (122, 3, 2, N'Update Mobile Number On Emirates ID', N'Update Mobile Number On Emirates ID', 550)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (123, 3, 2, N'Letter Typing', N'Letter Typing', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (124, 3, 2, N'Photocopying A3,A4', N'Photocopying A3,A4', 124)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (125, 3, 2, N'Document Scanning', N'Document Scanning', 457)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (126, 3, 2, N'Lamination', N'Lamination', 958)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (127, 3, 2, N'Company Immigration and Amer Works', N'Company Immigration and Amer Works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (128, 5, 4, N'Company Startup', N'Company Startup', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (129, 3, 4, N'Local Sponsorship Arrangement Trade License Amendment', N'Local Sponsorship Arrangement Trade License Amendment', 190)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (130, 6, 4, N'Office Arrangement', N'Office Arrangement', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (131, 6, 4, N'Company Stamp', N'Company Stamp', 100)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (132, 6, 4, N'Dubai Court', N'Dubai Court', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (133, 4, 4, N'Compnay Labour and Tasheel works', N'Compnay Labour and Tasheel works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (134, 1, 4, N'Family Visa New and Renewal', N'Family Visa New and Renewal', 982)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (135, 3, 4, N'Family under Wife Sponsorship', N'Family under Wife Sponsorship', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (136, 6, 4, N'Entry Permit Inside Entry Permit Out Side ', N'Entry Permit Inside Entry Permit Out Side ', 785)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (137, 1, 4, N'Chagne Status ', N'Chagne Status ', 335)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (138, 1, 4, N'Visa Stamping', N'Visa Stamping', 987)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (139, 1, 4, N'Visa Cancellation Inside', N'Visa Cancellation Inside', 157)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (140, 1, 4, N'Visa Cancellation Outside', N'Visa Cancellation Outside', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (141, 6, 4, N'Retun Permit', N'Retun Permit', 444)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (142, 4, 4, N'Passport Renewal Appointment', N'Passport Renewal Appointment', 744)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (143, 4, 4, N'Passport Renewal Travel Report', N'Passport Renewal Travel Report', 130)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (144, 4, 4, N'Health Insurance', N'Health Insurance', 140)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (145, 3, 4, N'Police Clearanc Certificate', N'Police Clearanc Certificate', 586)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (146, 3, 4, N'Normal Translation', N'Normal Translation', 687)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (147, 3, 4, N'Legal Translation.', N'Legal Translation.', 788)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (148, 3, 4, N'Birth Certificate Attestation', N'Birth Certificate Attestation', 258)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (149, 3, 4, N'Marriage Certificate Attestation', N'Marriage Certificate Attestation', 753)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (150, 3, 4, N'Education Certificates Attestation Jebel Ali Port Pass', N'Education Certificates Attestation Jebel Ali Port Pass', 167)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (151, 3, 4, N'Vehicle Insurance', N'Vehicle Insurance', 943)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (152, 3, 4, N'Driving License Renewal', N'Driving License Renewal', 349)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (153, 3, 4, N'Company Bank Account', N'Company Bank Account', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (154, 3, 4, N'Tenancy Contract Typing Visit Visa Services', N'Tenancy Contract Typing Visit Visa Services', 456)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (155, 3, 4, N'PRO Services', N'PRO Services', 964)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (156, 3, 4, N'RTA Letters and Online Services', N'RTA Letters and Online Services', 746)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (157, 3, 4, N'Update Mobile Number On Emirates ID', N'Update Mobile Number On Emirates ID', 550)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (158, 3, 4, N'Letter Typing', N'Letter Typing', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (159, 3, 4, N'Photocopying A3,A4', N'Photocopying A3,A4', 124)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (160, 3, 4, N'Document Scanning', N'Document Scanning', 457)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (161, 3, 4, N'Lamination', N'Lamination', 958)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (162, 3, 4, N'Company Immigration and Amer Works', N'Company Immigration and Amer Works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (163, 5, 5, N'Company Startup', N'Company Startup', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (164, 3, 5, N'Local Sponsorship Arrangement Trade License Amendment', N'Local Sponsorship Arrangement Trade License Amendment', 190)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (165, 6, 5, N'Office Arrangement', N'Office Arrangement', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (166, 6, 5, N'Company Stamp', N'Company Stamp', 100)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (167, 6, 5, N'Dubai Court', N'Dubai Court', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (168, 4, 5, N'Compnay Labour and Tasheel works', N'Compnay Labour and Tasheel works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (169, 1, 5, N'Family Visa New and Renewal', N'Family Visa New and Renewal', 982)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (170, 3, 5, N'Family under Wife Sponsorship', N'Family under Wife Sponsorship', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (171, 6, 5, N'Entry Permit Inside Entry Permit Out Side ', N'Entry Permit Inside Entry Permit Out Side ', 785)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (172, 1, 5, N'Chagne Status ', N'Chagne Status ', 335)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (173, 1, 5, N'Visa Stamping', N'Visa Stamping', 987)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (174, 1, 5, N'Visa Cancellation Inside', N'Visa Cancellation Inside', 157)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (175, 1, 5, N'Visa Cancellation Outside', N'Visa Cancellation Outside', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (176, 6, 5, N'Retun Permit', N'Retun Permit', 444)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (177, 4, 5, N'Passport Renewal Appointment', N'Passport Renewal Appointment', 744)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (178, 4, 5, N'Passport Renewal Travel Report', N'Passport Renewal Travel Report', 130)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (179, 4, 5, N'Health Insurance', N'Health Insurance', 140)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (180, 3, 5, N'Police Clearanc Certificate', N'Police Clearanc Certificate', 586)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (181, 3, 5, N'Normal Translation', N'Normal Translation', 687)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (182, 3, 5, N'Legal Translation.', N'Legal Translation.', 788)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (183, 3, 5, N'Birth Certificate Attestation', N'Birth Certificate Attestation', 258)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (184, 3, 5, N'Marriage Certificate Attestation', N'Marriage Certificate Attestation', 753)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (185, 3, 5, N'Education Certificates Attestation Jebel Ali Port Pass', N'Education Certificates Attestation Jebel Ali Port Pass', 167)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (186, 3, 5, N'Vehicle Insurance', N'Vehicle Insurance', 943)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (187, 3, 5, N'Driving License Renewal', N'Driving License Renewal', 349)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (188, 3, 5, N'Company Bank Account', N'Company Bank Account', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (189, 3, 5, N'Tenancy Contract Typing Visit Visa Services', N'Tenancy Contract Typing Visit Visa Services', 456)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (190, 3, 5, N'PRO Services', N'PRO Services', 964)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (191, 3, 5, N'RTA Letters and Online Services', N'RTA Letters and Online Services', 746)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (192, 3, 5, N'Update Mobile Number On Emirates ID', N'Update Mobile Number On Emirates ID', 550)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (193, 3, 5, N'Letter Typing', N'Letter Typing', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (194, 3, 5, N'Photocopying A3,A4', N'Photocopying A3,A4', 124)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (195, 3, 5, N'Document Scanning', N'Document Scanning', 457)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (196, 3, 5, N'Lamination', N'Lamination', 958)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (197, 3, 5, N'Company Immigration and Amer Works', N'Company Immigration and Amer Works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (198, 5, 6, N'Company Startup', N'Company Startup', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (199, 3, 6, N'Local Sponsorship Arrangement Trade License Amendment', N'Local Sponsorship Arrangement Trade License Amendment', 190)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (200, 6, 6, N'Office Arrangement', N'Office Arrangement', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (201, 6, 6, N'Company Stamp', N'Company Stamp', 100)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (202, 6, 6, N'Dubai Court', N'Dubai Court', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (203, 4, 6, N'Compnay Labour and Tasheel works', N'Compnay Labour and Tasheel works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (204, 1, 6, N'Family Visa New and Renewal', N'Family Visa New and Renewal', 982)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (205, 3, 6, N'Family under Wife Sponsorship', N'Family under Wife Sponsorship', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (206, 6, 6, N'Entry Permit Inside Entry Permit Out Side ', N'Entry Permit Inside Entry Permit Out Side ', 785)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (207, 1, 6, N'Chagne Status ', N'Chagne Status ', 335)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (208, 1, 6, N'Visa Stamping', N'Visa Stamping', 987)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (209, 1, 6, N'Visa Cancellation Inside', N'Visa Cancellation Inside', 157)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (210, 1, 6, N'Visa Cancellation Outside', N'Visa Cancellation Outside', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (211, 6, 6, N'Retun Permit', N'Retun Permit', 444)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (212, 4, 6, N'Passport Renewal Appointment', N'Passport Renewal Appointment', 744)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (213, 4, 6, N'Passport Renewal Travel Report', N'Passport Renewal Travel Report', 130)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (214, 4, 6, N'Health Insurance', N'Health Insurance', 140)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (215, 3, 6, N'Police Clearanc Certificate', N'Police Clearanc Certificate', 586)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (216, 3, 6, N'Normal Translation', N'Normal Translation', 687)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (217, 3, 6, N'Legal Translation.', N'Legal Translation.', 788)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (218, 3, 6, N'Birth Certificate Attestation', N'Birth Certificate Attestation', 258)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (219, 3, 6, N'Marriage Certificate Attestation', N'Marriage Certificate Attestation', 753)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (220, 3, 6, N'Education Certificates Attestation Jebel Ali Port Pass', N'Education Certificates Attestation Jebel Ali Port Pass', 167)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (221, 3, 6, N'Vehicle Insurance', N'Vehicle Insurance', 943)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (222, 3, 6, N'Driving License Renewal', N'Driving License Renewal', 349)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (223, 3, 6, N'Company Bank Account', N'Company Bank Account', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (224, 3, 6, N'Tenancy Contract Typing Visit Visa Services', N'Tenancy Contract Typing Visit Visa Services', 456)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (225, 3, 6, N'PRO Services', N'PRO Services', 964)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (226, 3, 6, N'RTA Letters and Online Services', N'RTA Letters and Online Services', 746)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (227, 3, 6, N'Update Mobile Number On Emirates ID', N'Update Mobile Number On Emirates ID', 550)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (228, 3, 6, N'Letter Typing', N'Letter Typing', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (229, 3, 6, N'Photocopying A3,A4', N'Photocopying A3,A4', 124)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (230, 3, 6, N'Document Scanning', N'Document Scanning', 457)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (231, 3, 6, N'Lamination', N'Lamination', 958)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (232, 3, 6, N'Company Immigration and Amer Works', N'Company Immigration and Amer Works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (233, 5, 7, N'Company Startup', N'Company Startup', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (234, 3, 7, N'Local Sponsorship Arrangement Trade License Amendment', N'Local Sponsorship Arrangement Trade License Amendment', 190)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (235, 6, 7, N'Office Arrangement', N'Office Arrangement', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (236, 6, 7, N'Company Stamp', N'Company Stamp', 100)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (237, 6, 7, N'Dubai Court', N'Dubai Court', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (238, 4, 7, N'Compnay Labour and Tasheel works', N'Compnay Labour and Tasheel works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (239, 1, 7, N'Family Visa New and Renewal', N'Family Visa New and Renewal', 982)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (240, 3, 7, N'Family under Wife Sponsorship', N'Family under Wife Sponsorship', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (241, 6, 7, N'Entry Permit Inside Entry Permit Out Side ', N'Entry Permit Inside Entry Permit Out Side ', 785)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (242, 1, 7, N'Chagne Status ', N'Chagne Status ', 335)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (243, 1, 7, N'Visa Stamping', N'Visa Stamping', 987)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (244, 1, 7, N'Visa Cancellation Inside', N'Visa Cancellation Inside', 157)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (245, 1, 7, N'Visa Cancellation Outside', N'Visa Cancellation Outside', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (246, 6, 7, N'Retun Permit', N'Retun Permit', 444)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (247, 4, 7, N'Passport Renewal Appointment', N'Passport Renewal Appointment', 744)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (248, 4, 7, N'Passport Renewal Travel Report', N'Passport Renewal Travel Report', 130)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (249, 4, 7, N'Health Insurance', N'Health Insurance', 140)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (250, 3, 7, N'Police Clearanc Certificate', N'Police Clearanc Certificate', 586)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (251, 3, 7, N'Normal Translation', N'Normal Translation', 687)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (252, 3, 7, N'Legal Translation.', N'Legal Translation.', 788)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (253, 3, 7, N'Birth Certificate Attestation', N'Birth Certificate Attestation', 258)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (254, 3, 7, N'Marriage Certificate Attestation', N'Marriage Certificate Attestation', 753)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (255, 3, 7, N'Education Certificates Attestation Jebel Ali Port Pass', N'Education Certificates Attestation Jebel Ali Port Pass', 167)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (256, 3, 7, N'Vehicle Insurance', N'Vehicle Insurance', 943)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (257, 3, 7, N'Driving License Renewal', N'Driving License Renewal', 349)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (258, 3, 7, N'Company Bank Account', N'Company Bank Account', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (259, 3, 7, N'Tenancy Contract Typing Visit Visa Services', N'Tenancy Contract Typing Visit Visa Services', 456)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (260, 3, 7, N'PRO Services', N'PRO Services', 964)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (261, 3, 7, N'RTA Letters and Online Services', N'RTA Letters and Online Services', 746)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (262, 3, 7, N'Update Mobile Number On Emirates ID', N'Update Mobile Number On Emirates ID', 550)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (263, 3, 7, N'Letter Typing', N'Letter Typing', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (264, 3, 7, N'Photocopying A3,A4', N'Photocopying A3,A4', 124)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (265, 3, 7, N'Document Scanning', N'Document Scanning', 457)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (266, 3, 7, N'Lamination', N'Lamination', 958)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (267, 3, 7, N'Company Immigration and Amer Works', N'Company Immigration and Amer Works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (268, 5, 8, N'Company Startup', N'Company Startup', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (269, 3, 8, N'Local Sponsorship Arrangement Trade License Amendment', N'Local Sponsorship Arrangement Trade License Amendment', 190)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (270, 6, 8, N'Office Arrangement', N'Office Arrangement', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (271, 6, 8, N'Company Stamp', N'Company Stamp', 100)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (272, 6, 8, N'Dubai Court', N'Dubai Court', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (273, 4, 8, N'Compnay Labour and Tasheel works', N'Compnay Labour and Tasheel works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (274, 1, 8, N'Family Visa New and Renewal', N'Family Visa New and Renewal', 982)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (275, 3, 8, N'Family under Wife Sponsorship', N'Family under Wife Sponsorship', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (276, 6, 8, N'Entry Permit Inside Entry Permit Out Side ', N'Entry Permit Inside Entry Permit Out Side ', 785)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (277, 1, 8, N'Chagne Status ', N'Chagne Status ', 335)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (278, 1, 8, N'Visa Stamping', N'Visa Stamping', 987)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (279, 1, 8, N'Visa Cancellation Inside', N'Visa Cancellation Inside', 157)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (280, 1, 8, N'Visa Cancellation Outside', N'Visa Cancellation Outside', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (281, 6, 8, N'Retun Permit', N'Retun Permit', 444)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (282, 4, 8, N'Passport Renewal Appointment', N'Passport Renewal Appointment', 744)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (283, 4, 8, N'Passport Renewal Travel Report', N'Passport Renewal Travel Report', 130)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (284, 4, 8, N'Health Insurance', N'Health Insurance', 140)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (285, 3, 8, N'Police Clearanc Certificate', N'Police Clearanc Certificate', 586)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (286, 3, 8, N'Normal Translation', N'Normal Translation', 687)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (287, 3, 8, N'Legal Translation.', N'Legal Translation.', 788)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (288, 3, 8, N'Birth Certificate Attestation', N'Birth Certificate Attestation', 258)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (289, 3, 8, N'Marriage Certificate Attestation', N'Marriage Certificate Attestation', 753)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (290, 3, 8, N'Education Certificates Attestation Jebel Ali Port Pass', N'Education Certificates Attestation Jebel Ali Port Pass', 167)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (291, 3, 8, N'Vehicle Insurance', N'Vehicle Insurance', 943)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (292, 3, 8, N'Driving License Renewal', N'Driving License Renewal', 349)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (293, 3, 8, N'Company Bank Account', N'Company Bank Account', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (294, 3, 8, N'Tenancy Contract Typing Visit Visa Services', N'Tenancy Contract Typing Visit Visa Services', 456)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (295, 3, 8, N'PRO Services', N'PRO Services', 964)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (296, 3, 8, N'RTA Letters and Online Services', N'RTA Letters and Online Services', 746)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (297, 3, 8, N'Update Mobile Number On Emirates ID', N'Update Mobile Number On Emirates ID', 550)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (298, 3, 8, N'Letter Typing', N'Letter Typing', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (299, 3, 8, N'Photocopying A3,A4', N'Photocopying A3,A4', 124)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (300, 3, 8, N'Document Scanning', N'Document Scanning', 457)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (301, 3, 8, N'Lamination', N'Lamination', 958)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (302, 3, 8, N'Company Immigration and Amer Works', N'Company Immigration and Amer Works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (303, 5, 9, N'Company Startup', N'Company Startup', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (304, 3, 9, N'Local Sponsorship Arrangement Trade License Amendment', N'Local Sponsorship Arrangement Trade License Amendment', 190)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (305, 6, 9, N'Office Arrangement', N'Office Arrangement', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (306, 6, 9, N'Company Stamp', N'Company Stamp', 100)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (307, 6, 9, N'Dubai Court', N'Dubai Court', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (308, 4, 9, N'Compnay Labour and Tasheel works', N'Compnay Labour and Tasheel works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (309, 1, 9, N'Family Visa New and Renewal', N'Family Visa New and Renewal', 982)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (310, 3, 9, N'Family under Wife Sponsorship', N'Family under Wife Sponsorship', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (311, 6, 9, N'Entry Permit Inside Entry Permit Out Side ', N'Entry Permit Inside Entry Permit Out Side ', 785)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (312, 1, 9, N'Chagne Status ', N'Chagne Status ', 335)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (313, 1, 9, N'Visa Stamping', N'Visa Stamping', 987)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (314, 1, 9, N'Visa Cancellation Inside', N'Visa Cancellation Inside', 157)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (315, 1, 9, N'Visa Cancellation Outside', N'Visa Cancellation Outside', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (316, 6, 9, N'Retun Permit', N'Retun Permit', 444)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (317, 4, 9, N'Passport Renewal Appointment', N'Passport Renewal Appointment', 744)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (318, 4, 9, N'Passport Renewal Travel Report', N'Passport Renewal Travel Report', 130)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (319, 4, 9, N'Health Insurance', N'Health Insurance', 140)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (320, 3, 9, N'Police Clearanc Certificate', N'Police Clearanc Certificate', 586)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (321, 3, 9, N'Normal Translation', N'Normal Translation', 687)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (322, 3, 9, N'Legal Translation.', N'Legal Translation.', 788)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (323, 3, 9, N'Birth Certificate Attestation', N'Birth Certificate Attestation', 258)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (324, 3, 9, N'Marriage Certificate Attestation', N'Marriage Certificate Attestation', 753)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (325, 3, 9, N'Education Certificates Attestation Jebel Ali Port Pass', N'Education Certificates Attestation Jebel Ali Port Pass', 167)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (326, 3, 9, N'Vehicle Insurance', N'Vehicle Insurance', 943)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (327, 3, 9, N'Driving License Renewal', N'Driving License Renewal', 349)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (328, 3, 9, N'Company Bank Account', N'Company Bank Account', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (329, 3, 9, N'Tenancy Contract Typing Visit Visa Services', N'Tenancy Contract Typing Visit Visa Services', 456)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (330, 3, 9, N'PRO Services', N'PRO Services', 964)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (331, 3, 9, N'RTA Letters and Online Services', N'RTA Letters and Online Services', 746)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (332, 3, 9, N'Update Mobile Number On Emirates ID', N'Update Mobile Number On Emirates ID', 550)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (333, 3, 9, N'Letter Typing', N'Letter Typing', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (334, 3, 9, N'Photocopying A3,A4', N'Photocopying A3,A4', 124)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (335, 3, 9, N'Document Scanning', N'Document Scanning', 457)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (336, 3, 9, N'Lamination', N'Lamination', 958)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (337, 3, 9, N'Company Immigration and Amer Works', N'Company Immigration and Amer Works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (338, 5, 10, N'Company Startup', N'Company Startup', 150)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (339, 3, 10, N'Local Sponsorship Arrangement Trade License Amendment', N'Local Sponsorship Arrangement Trade License Amendment', 190)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (340, 6, 10, N'Office Arrangement', N'Office Arrangement', 350)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (341, 6, 10, N'Company Stamp', N'Company Stamp', 100)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (342, 6, 10, N'Dubai Court', N'Dubai Court', 200)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (343, 4, 10, N'Compnay Labour and Tasheel works', N'Compnay Labour and Tasheel works', 880)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (344, 1, 10, N'Family Visa New and Renewal', N'Family Visa New and Renewal', 982)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (345, 3, 10, N'Family under Wife Sponsorship', N'Family under Wife Sponsorship', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (346, 6, 10, N'Entry Permit Inside Entry Permit Out Side ', N'Entry Permit Inside Entry Permit Out Side ', 785)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (347, 1, 10, N'Chagne Status ', N'Chagne Status ', 335)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (348, 1, 10, N'Visa Stamping', N'Visa Stamping', 987)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (349, 1, 10, N'Visa Cancellation Inside', N'Visa Cancellation Inside', 157)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (350, 1, 10, N'Visa Cancellation Outside', N'Visa Cancellation Outside', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (351, 6, 10, N'Retun Permit', N'Retun Permit', 444)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (352, 4, 10, N'Passport Renewal Appointment', N'Passport Renewal Appointment', 744)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (353, 4, 10, N'Passport Renewal Travel Report', N'Passport Renewal Travel Report', 130)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (354, 4, 10, N'Health Insurance', N'Health Insurance', 140)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (355, 3, 10, N'Police Clearanc Certificate', N'Police Clearanc Certificate', 586)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (356, 3, 10, N'Normal Translation', N'Normal Translation', 687)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (357, 3, 10, N'Legal Translation.', N'Legal Translation.', 788)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (358, 3, 10, N'Birth Certificate Attestation', N'Birth Certificate Attestation', 258)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (359, 3, 10, N'Marriage Certificate Attestation', N'Marriage Certificate Attestation', 753)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (360, 3, 10, N'Education Certificates Attestation Jebel Ali Port Pass', N'Education Certificates Attestation Jebel Ali Port Pass', 167)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (361, 3, 10, N'Vehicle Insurance', N'Vehicle Insurance', 943)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (362, 3, 10, N'Driving License Renewal', N'Driving License Renewal', 349)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (363, 3, 10, N'Company Bank Account', N'Company Bank Account', 654)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (364, 3, 10, N'Tenancy Contract Typing Visit Visa Services', N'Tenancy Contract Typing Visit Visa Services', 456)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (365, 3, 10, N'PRO Services', N'PRO Services', 964)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (366, 3, 10, N'RTA Letters and Online Services', N'RTA Letters and Online Services', 746)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (367, 3, 10, N'Update Mobile Number On Emirates ID', N'Update Mobile Number On Emirates ID', 550)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (368, 3, 10, N'Letter Typing', N'Letter Typing', 754)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (369, 3, 10, N'Photocopying A3,A4', N'Photocopying A3,A4', 124)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (370, 3, 10, N'Document Scanning', N'Document Scanning', 457)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (371, 3, 10, N'Lamination', N'Lamination', 958)
GO
INSERT [dbo].[Service] ([ServiceID], [CategoryID], [MerchantID], [ServiceName], [Description], [ServicePrice]) VALUES (372, 3, 10, N'Company Immigration and Amer Works', N'Company Immigration and Amer Works', 880)
GO
INSERT [dbo].[ServiceCategory] ([CategoryID], [CategoryName]) VALUES (1, N'Visa')
GO
INSERT [dbo].[ServiceCategory] ([CategoryID], [CategoryName]) VALUES (2, N'Emirates ID')
GO
INSERT [dbo].[ServiceCategory] ([CategoryID], [CategoryName]) VALUES (3, N'Documents')
GO
INSERT [dbo].[ServiceCategory] ([CategoryID], [CategoryName]) VALUES (4, N'Pro Services')
GO
INSERT [dbo].[ServiceCategory] ([CategoryID], [CategoryName]) VALUES (5, N'Business')
GO
INSERT [dbo].[ServiceCategory] ([CategoryID], [CategoryName]) VALUES (6, N'Others')
GO
SET IDENTITY_INSERT [dbo].[UploadedFile] ON 
GO
INSERT [dbo].[UploadedFile] ([UFID], [FileName], [ContentType], [FileSize], [FolderName], [Status], [UserId], [DocumentAddedDate], [DocumentModifiedDate]) VALUES (4, N'FRM HR N 0883 Leave Form (2) (1).pdf', N'application/pdf', 128948, N'Documents_1', N'Pending', 1, CAST(N'2024-07-01T17:35:08.833' AS DateTime), CAST(N'2024-07-01T17:35:08.840' AS DateTime))
GO
INSERT [dbo].[UploadedFile] ([UFID], [FileName], [ContentType], [FileSize], [FolderName], [Status], [UserId], [DocumentAddedDate], [DocumentModifiedDate]) VALUES (5, N'INSTA_PAY_UserManual 1.pdf', N'application/pdf', 865451, N'Documents_1', N'Pending', 1, CAST(N'2024-07-01T17:37:10.720' AS DateTime), CAST(N'2024-07-01T17:37:10.720' AS DateTime))
GO
INSERT [dbo].[UploadedFile] ([UFID], [FileName], [ContentType], [FileSize], [FolderName], [Status], [UserId], [DocumentAddedDate], [DocumentModifiedDate]) VALUES (6, N'INSTA_PAY_UserManual 1.pdf', N'application/pdf', 865451, N'Documents_1', N'Pending', 1, CAST(N'2024-07-01T17:39:36.387' AS DateTime), CAST(N'2024-07-01T17:39:36.387' AS DateTime))
GO
INSERT [dbo].[UploadedFile] ([UFID], [FileName], [ContentType], [FileSize], [FolderName], [Status], [UserId], [DocumentAddedDate], [DocumentModifiedDate]) VALUES (7, N'INSTA_PAY_UserManual 1.pdf', N'application/pdf', 865451, N'Documents_1', N'Pending', 1, CAST(N'2024-07-01T17:42:30.077' AS DateTime), CAST(N'2024-07-01T17:42:30.077' AS DateTime))
GO
INSERT [dbo].[UploadedFile] ([UFID], [FileName], [ContentType], [FileSize], [FolderName], [Status], [UserId], [DocumentAddedDate], [DocumentModifiedDate]) VALUES (8, N'what is devops(1).png', N'image/png', 366654, N'Documents_1', N'Pending', 0, CAST(N'2024-07-01T17:56:48.990' AS DateTime), CAST(N'2024-07-01T17:56:48.990' AS DateTime))
GO
INSERT [dbo].[UploadedFile] ([UFID], [FileName], [ContentType], [FileSize], [FolderName], [Status], [UserId], [DocumentAddedDate], [DocumentModifiedDate]) VALUES (9, N'what is devops.png', N'image/png', 366654, N'Documents_1', N'Pending', 0, CAST(N'2024-07-01T17:56:49.093' AS DateTime), CAST(N'2024-07-01T17:56:49.093' AS DateTime))
GO
INSERT [dbo].[UploadedFile] ([UFID], [FileName], [ContentType], [FileSize], [FolderName], [Status], [UserId], [DocumentAddedDate], [DocumentModifiedDate]) VALUES (10, N'what is devops(1).png', N'image/png', 366654, N'Documents_1', N'Pending', 0, CAST(N'2024-07-01T18:00:30.977' AS DateTime), CAST(N'2024-07-01T18:00:30.977' AS DateTime))
GO
INSERT [dbo].[UploadedFile] ([UFID], [FileName], [ContentType], [FileSize], [FolderName], [Status], [UserId], [DocumentAddedDate], [DocumentModifiedDate]) VALUES (11, N'what is devops.png', N'image/png', 366654, N'Documents_1', N'Pending', 0, CAST(N'2024-07-01T18:00:31.017' AS DateTime), CAST(N'2024-07-01T18:00:31.017' AS DateTime))
GO
INSERT [dbo].[UploadedFile] ([UFID], [FileName], [ContentType], [FileSize], [FolderName], [Status], [UserId], [DocumentAddedDate], [DocumentModifiedDate]) VALUES (12, N'what is devops(1).png', N'image/png', 366654, N'Documents_1', N'Pending', 1, CAST(N'2024-07-01T18:02:06.853' AS DateTime), CAST(N'2024-07-01T18:02:06.853' AS DateTime))
GO
INSERT [dbo].[UploadedFile] ([UFID], [FileName], [ContentType], [FileSize], [FolderName], [Status], [UserId], [DocumentAddedDate], [DocumentModifiedDate]) VALUES (13, N'what is devops.png', N'image/png', 366654, N'Documents_1', N'Pending', 1, CAST(N'2024-07-01T18:02:06.987' AS DateTime), CAST(N'2024-07-01T18:02:06.987' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[UploadedFile] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__AdminUse__A9D105340D054614]    Script Date: 7/29/2024 10:41:36 PM ******/
ALTER TABLE [dbo].[AdminUsers] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Customer__A9D10534AC2133AF]    Script Date: 7/29/2024 10:41:36 PM ******/
ALTER TABLE [dbo].[Customer] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Merchant__A9D10534EDD25F95]    Script Date: 7/29/2024 10:41:36 PM ******/
ALTER TABLE [dbo].[ProviderUser] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DashboardItems] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[MenuPermissions] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[MenuPermissions] ADD  DEFAULT (sysutcdatetime()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[Merchant] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[MerchantUser] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Permissions] ADD  DEFAULT ((0)) FOR [CanView]
GO
ALTER TABLE [dbo].[ProviderUser] ADD  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserRolePermissions] ADD  DEFAULT ((0)) FOR [CanCreate]
GO
ALTER TABLE [dbo].[UserRolePermissions] ADD  DEFAULT ((0)) FOR [CanRead]
GO
ALTER TABLE [dbo].[UserRolePermissions] ADD  DEFAULT ((0)) FOR [CanUpdate]
GO
ALTER TABLE [dbo].[UserRolePermissions] ADD  DEFAULT ((0)) FOR [CanDelete]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ActionLog]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[AdminDashboard]  WITH CHECK ADD FOREIGN KEY([DashboardItemID])
REFERENCES [dbo].[DashboardItems] ([DashboardItemID])
GO
ALTER TABLE [dbo].[AdminUsers]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[ChatMessage]  WITH CHECK ADD  CONSTRAINT [FK_ChatMessage_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ChatMessage] CHECK CONSTRAINT [FK_ChatMessage_CreatedBy]
GO
ALTER TABLE [dbo].[ChatMessage]  WITH CHECK ADD  CONSTRAINT [FK_ChatMessage_Merchant] FOREIGN KEY([MerchantID])
REFERENCES [dbo].[Merchant] ([MerchantID])
GO
ALTER TABLE [dbo].[ChatMessage] CHECK CONSTRAINT [FK_ChatMessage_Merchant]
GO
ALTER TABLE [dbo].[ChatMessage]  WITH CHECK ADD  CONSTRAINT [FK_ChatMessage_ModifiedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ChatMessage] CHECK CONSTRAINT [FK_ChatMessage_ModifiedBy]
GO
ALTER TABLE [dbo].[ChatMessage]  WITH CHECK ADD  CONSTRAINT [FK_ChatMessage_Receiver] FOREIGN KEY([ReceiverID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ChatMessage] CHECK CONSTRAINT [FK_ChatMessage_Receiver]
GO
ALTER TABLE [dbo].[ChatMessage]  WITH CHECK ADD  CONSTRAINT [FK_ChatMessage_Sender] FOREIGN KEY([SenderID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ChatMessage] CHECK CONSTRAINT [FK_ChatMessage_Sender]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_CreatedBy]
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_Merchant] FOREIGN KEY([MerchantID])
REFERENCES [dbo].[Merchant] ([MerchantID])
GO
ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_Merchant]
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_MerchantUser] FOREIGN KEY([MerchantUserID])
REFERENCES [dbo].[MerchantUser] ([MerchantUserID])
GO
ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_MerchantUser]
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_ModifiedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_ModifiedBy]
GO
ALTER TABLE [dbo].[Document]  WITH CHECK ADD  CONSTRAINT [FK_Document_UploadedBy] FOREIGN KEY([UploadedBy])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Document] CHECK CONSTRAINT [FK_Document_UploadedBy]
GO
ALTER TABLE [dbo].[LoginHistory]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[MenuPermissions]  WITH CHECK ADD  CONSTRAINT [FK_MenuPermissions_Groups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[MenuPermissions] CHECK CONSTRAINT [FK_MenuPermissions_Groups]
GO
ALTER TABLE [dbo].[MenuPermissions]  WITH CHECK ADD  CONSTRAINT [FK_MenuPermissions_Menus] FOREIGN KEY([MenuID])
REFERENCES [dbo].[Menu] ([MenuId])
GO
ALTER TABLE [dbo].[MenuPermissions] CHECK CONSTRAINT [FK_MenuPermissions_Menus]
GO
ALTER TABLE [dbo].[MenuPermissions]  WITH CHECK ADD  CONSTRAINT [FK_MenuPermissions_Permissions] FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([PermissionId])
GO
ALTER TABLE [dbo].[MenuPermissions] CHECK CONSTRAINT [FK_MenuPermissions_Permissions]
GO
ALTER TABLE [dbo].[MenuPermissions]  WITH CHECK ADD  CONSTRAINT [FK_MenuPermissions_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[MenuPermissions] CHECK CONSTRAINT [FK_MenuPermissions_Roles]
GO
ALTER TABLE [dbo].[MenuPermissions]  WITH CHECK ADD  CONSTRAINT [FK_MenuPermissions_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[MenuPermissions] CHECK CONSTRAINT [FK_MenuPermissions_Users]
GO
ALTER TABLE [dbo].[MerchantDashboard]  WITH CHECK ADD FOREIGN KEY([DashboardItemID])
REFERENCES [dbo].[DashboardItems] ([DashboardItemID])
GO
ALTER TABLE [dbo].[MerchantDashboard]  WITH CHECK ADD FOREIGN KEY([MerchantID])
REFERENCES [dbo].[Merchant] ([MerchantID])
GO
ALTER TABLE [dbo].[MerchantRating]  WITH CHECK ADD FOREIGN KEY([RatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[MerchantRating]  WITH CHECK ADD FOREIGN KEY([RatedMerchantID])
REFERENCES [dbo].[Merchant] ([MerchantID])
GO
ALTER TABLE [dbo].[MerchantUser]  WITH CHECK ADD FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[MerchantUser]  WITH CHECK ADD FOREIGN KEY([MerchantID])
REFERENCES [dbo].[Merchant] ([MerchantID])
GO
ALTER TABLE [dbo].[MerchantUser]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[MerchantUserDashboard]  WITH CHECK ADD FOREIGN KEY([DashboardItemID])
REFERENCES [dbo].[DashboardItems] ([DashboardItemID])
GO
ALTER TABLE [dbo].[MerchantUserDashboard]  WITH CHECK ADD FOREIGN KEY([MerchantUserID])
REFERENCES [dbo].[MerchantUser] ([MerchantUserID])
GO
ALTER TABLE [dbo].[MerchantUserRating]  WITH CHECK ADD FOREIGN KEY([RatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[MerchantUserRating]  WITH CHECK ADD FOREIGN KEY([RatedMerchantUserID])
REFERENCES [dbo].[MerchantUser] ([MerchantUserID])
GO
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menu] ([MenuId])
GO
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[ProviderUser]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK_Category_Service] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[ServiceCategory] ([CategoryID])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK_Category_Service]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK_Merchant_Service] FOREIGN KEY([MerchantID])
REFERENCES [dbo].[Merchant] ([MerchantID])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK_Merchant_Service]
GO
ALTER TABLE [dbo].[SubMenu]  WITH CHECK ADD FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menu] ([MenuId])
GO
ALTER TABLE [dbo].[SuperUserDashboard]  WITH CHECK ADD FOREIGN KEY([DashboardItemID])
REFERENCES [dbo].[DashboardItems] ([DashboardItemID])
GO
ALTER TABLE [dbo].[UserGroupPermissions]  WITH CHECK ADD FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[UserGroupPermissions]  WITH CHECK ADD FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([PermissionId])
GO
ALTER TABLE [dbo].[UserRolePermissions]  WITH CHECK ADD FOREIGN KEY([PermissionID])
REFERENCES [dbo].[Permissions] ([PermissionId])
GO
ALTER TABLE [dbo].[UserRolePermissions]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Groups] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Groups] ([GroupID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Groups]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO

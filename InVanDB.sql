USE [master]
GO
/****** Object:  Database [InVanDB]    Script Date: 05-05-2022 16:31:00 ******/
CREATE DATABASE [InVanDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'InVanDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\InVanDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'InVanDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\InVanDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [InVanDB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [InVanDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [InVanDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [InVanDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [InVanDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [InVanDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [InVanDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [InVanDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [InVanDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [InVanDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [InVanDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [InVanDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [InVanDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [InVanDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [InVanDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [InVanDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [InVanDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [InVanDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [InVanDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [InVanDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [InVanDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [InVanDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [InVanDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [InVanDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [InVanDB] SET RECOVERY FULL 
GO
ALTER DATABASE [InVanDB] SET  MULTI_USER 
GO
ALTER DATABASE [InVanDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [InVanDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [InVanDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [InVanDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [InVanDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [InVanDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'InVanDB', N'ON'
GO
ALTER DATABASE [InVanDB] SET QUERY_STORE = OFF
GO
USE [InVanDB]
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branch](
	[BranchId] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationID] [int] NULL,
	[BranchName] [varchar](max) NULL,
	[Abbreviation] [varchar](max) NULL,
	[ContactPerson] [varchar](max) NULL,
	[ContactNo] [varchar](max) NULL,
	[StartDate] [date] NULL,
	[CountryID] [int] NULL,
	[StateID] [int] NULL,
	[CityID] [int] NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED 
(
	[BranchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CityMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CityMaster](
	[CityID] [int] IDENTITY(1,1) NOT NULL,
	[StateID] [int] NOT NULL,
	[CityName] [varchar](70) NULL,
	[CityCode] [int] NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CityMaster] PRIMARY KEY CLUSTERED 
(
	[CityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ClientMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientMaster](
	[ClientID] [int] IDENTITY(1,1) NOT NULL,
	[ClientName] [varchar](70) NULL,
	[ClientEmailId] [varchar](30) NULL,
	[ContactPersonName] [varchar](70) NULL,
	[Designation] [varchar](50) NULL,
	[Department] [varchar](50) NULL,
	[ContactNo] [varchar](15) NULL,
	[BankName] [varchar](70) NULL,
	[BankBranchName] [varchar](70) NULL,
	[BankAccountNo] [varchar](70) NULL,
	[IFSCCode] [varchar](20) NULL,
	[GSTINNo] [varchar](20) NULL,
	[BlackList] [int] NULL,
	[ShipToAddress] [varchar](max) NULL,
	[ShipToCountry] [int] NULL,
	[ShipToState] [int] NULL,
	[ShipToCity] [int] NULL,
	[ShipPinCode] [varchar](20) NULL,
	[ShipStateCode] [varchar](20) NULL,
	[BillToAddress] [varchar](max) NULL,
	[BillToCountry] [int] NULL,
	[BillToState] [int] NULL,
	[BillToCity] [int] NULL,
	[BillPinCode] [varchar](20) NULL,
	[BillStateCode] [varchar](20) NULL,
	[PanNo] [varchar](50) NULL,
	[FSSAI] [varchar](50) NULL,
	[CountryCode] [varchar](10) NULL,
	[TypeOfCompany] [varchar](50) NULL,
	[IsDeleted] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ClientMaster] PRIMARY KEY CLUSTERED 
(
	[ClientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[COAMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[COAMaster](
	[COAID] [int] IDENTITY(1,1) NOT NULL,
	[COANo] [nvarchar](30) NULL,
	[WorkOrderID] [int] NULL,
	[LabID] [int] NULL,
	[COADate] [date] NULL,
	[ItemCategoryID] [int] NULL,
	[ItemID] [int] NULL,
	[ItemCode] [nvarchar](50) NULL,
	[BatchNo] [nvarchar](50) NULL,
	[PONumber] [nvarchar](50) NULL,
	[ManufecturingDate] [date] NULL,
	[BestBefore] [date] NULL,
	[Weight] [float] NULL,
	[PackingSize] [float] NULL,
	[Acidity] [float] NULL,
	[Salt] [float] NULL,
	[PH] [float] NULL,
	[BRIX] [varchar](max) NULL,
	[Consistency] [bit] NULL,
	[Ecoli] [bit] NULL,
	[YestAndMold] [bit] NULL,
	[ArobicCount] [float] NULL,
	[Remarks] [varchar](max) NULL,
	[Status] [varchar](20) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_COAMaster] PRIMARY KEY CLUSTERED 
(
	[COAID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[Name] [nvarchar](100) NULL,
	[Company_ID] [int] IDENTITY(1,1) NOT NULL,
	[ContactNo] [numeric](18, 0) NULL,
	[Address] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[Company_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CountryMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountryMaster](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [varchar](50) NULL,
	[CountryCode] [int] NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CountryMaster] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CurrencyMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CurrencyMaster](
	[CurrencyID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NOT NULL,
	[CurrencyName] [varchar](20) NULL,
	[CurrencyPrice] [float] NULL,
	[ndianCurrencyPrice] [float] NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_CurrencyMaster] PRIMARY KEY CLUSTERED 
(
	[CurrencyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DebitNote]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DebitNote](
	[DebitNoteId] [int] IDENTITY(1,1) NOT NULL,
	[RejectionNoteId] [int] NULL,
	[GRNId] [int] NULL,
	[SupplierId] [int] NULL,
	[DebitNoteNo] [varchar](50) NULL,
	[Date] [datetime] NULL,
	[Grandtotal] [decimal](18, 2) NULL,
	[GeneralRemarks] [varchar](50) NULL,
	[Status] [bit] NULL,
	[CheckedBy] [int] NULL,
	[ApprovedBy] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedby] [int] NULL,
 CONSTRAINT [PK_DebitNote] PRIMARY KEY CLUSTERED 
(
	[DebitNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DebitNoteDetails]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DebitNoteDetails](
	[DebitNoteDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[DebitNoteId] [int] NULL,
	[ItemId] [int] NULL,
	[UnitId] [int] NULL,
	[Quantity] [decimal](18, 2) NULL,
	[Rate] [decimal](18, 2) NULL,
	[Discount] [decimal](18, 2) NULL,
	[SGST] [decimal](18, 2) NULL,
	[CGST] [decimal](18, 2) NULL,
	[IGST] [decimal](18, 2) NULL,
	[Value] [decimal](18, 2) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedby] [int] NULL,
 CONSTRAINT [PK_DebitNoteDetails] PRIMARY KEY CLUSTERED 
(
	[DebitNoteDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[DepartmentID] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [varchar](50) NULL,
	[BranchId] [int] NULL,
	[ContactNo] [varchar](50) NULL,
	[ContactPerson] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedby] [int] NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[DepartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DesignationMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DesignationMaster](
	[DesignationID] [int] IDENTITY(1,1) NOT NULL,
	[DesignationName] [varchar](100) NULL,
	[Description] [varchar](250) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_DesignationMaster] PRIMARY KEY CLUSTERED 
(
	[DesignationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeMaster](
	[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeName] [varchar](200) NULL,
	[OrganisationGroupId] [int] NULL,
	[OrganisationId] [int] NULL,
	[BranchID] [int] NULL,
	[DepartmentID] [int] NULL,
	[DesignationID] [int] NULL,
	[EmployeeMobileNo] [varchar](12) NULL,
	[EmployeeBirthDate] [date] NULL,
	[EmployeeJoingDate] [date] NULL,
	[EmployeeGender] [varchar](6) NULL,
	[CountryID] [int] NULL,
	[StateID] [int] NULL,
	[CityID] [int] NULL,
	[EmployeeAddress] [varchar](max) NULL,
	[PinNumber] [varchar](10) NULL,
	[EmployeePic] [varchar](max) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EmployeeMaster] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinishGoodSeries]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinishGoodSeries](
	[FGSID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](50) NULL,
	[PackageSize] [int] NULL,
	[MfgDate] [date] NULL,
	[NoOfCartonBox] [int] NULL,
	[QuantityInKG] [float] NULL,
	[BatchNo] [varchar](50) NULL,
	[PONumber] [varchar](50) NULL,
	[Packaging] [nchar](10) NULL,
	[Sealing] [nchar](10) NULL,
	[Labeling] [nchar](10) NULL,
	[QCCheck] [nchar](10) NULL,
	[Remarks] [nchar](10) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_FinishGoodSeries] PRIMARY KEY CLUSTERED 
(
	[FGSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GSTMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GSTMaster](
	[TaxID] [int] IDENTITY(1,1) NOT NULL,
	[TaxName] [varchar](50) NULL,
	[TaxRate] [float] NULL,
	[Description] [varchar](250) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_GSTMaster] PRIMARY KEY CLUSTERED 
(
	[TaxID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InquiryMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InquiryMaster](
	[InquiryID] [int] IDENTITY(1,1) NOT NULL,
	[InquiryStatusID] [int] NULL,
	[CompanyName] [varchar](70) NULL,
	[ContactPersonName] [varchar](70) NULL,
	[DateOfInquiry] [date] NULL,
	[ClientEmail] [varchar](50) NULL,
	[ClientAddress] [varchar](max) NULL,
	[ClientCountry] [int] NULL,
	[ClientState] [int] NULL,
	[ClientCity] [int] NULL,
	[ClientZipCode] [varchar](50) NULL,
	[ItemCategoryID] [int] NULL,
	[ItemID] [int] NULL,
	[ItemCode] [nvarchar](50) NULL,
	[ItemQuantity] [float] NULL,
	[QuotedPrice] [float] NULL,
	[ExpectedPrice] [float] NULL,
	[CloserPrice] [float] NULL,
	[PONumber] [nvarchar](50) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InquiryMaster] PRIMARY KEY CLUSTERED 
(
	[InquiryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceMaster](
	[InvoiceId] [int] IDENTITY(1,1) NOT NULL,
	[DANo] [varchar](50) NULL,
	[DADate] [datetime] NULL,
	[InvoiceNo] [varchar](50) NULL,
	[InvoiceDate] [datetime] NULL,
	[InvoiceType] [varchar](50) NULL,
	[DateOfIssue] [varchar](50) NULL,
	[DateOfRemoval] [varchar](50) NULL,
	[TimeOfIssue] [varchar](50) NULL,
	[TimeOfRemoval] [varchar](50) NULL,
	[NotificationNo] [varchar](50) NULL,
	[CheckedById] [int] NULL,
	[ApprovedById] [int] NULL,
	[ApprovalStatus] [int] NULL,
	[ApprovalRemarks] [varchar](max) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedById] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedById] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_InvoiceMaster] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InwardNote]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InwardNote](
	[InwardNoteID] [int] IDENTITY(1,1) NOT NULL,
	[InwardNumber] [varchar](50) NULL,
	[InwardDate] [date] NULL,
	[PurchaseOrderId] [int] NULL,
	[PONumber] [varchar](50) NULL,
	[PODate] [date] NULL,
	[LocationID] [int] NULL,
	[BillingAddress] [varchar](500) NULL,
	[ShippingAddress] [varchar](500) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedby] [int] NULL,
 CONSTRAINT [PK_InwardNote] PRIMARY KEY CLUSTERED 
(
	[InwardNoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IssueNote]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IssueNote](
	[IssueNoteId] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentId] [int] NULL,
	[IssueNoteNo] [varchar](50) NULL,
	[IsAgainstBOM] [int] NULL,
	[SalesOrderId] [int] NULL,
	[IssueNoteDate] [datetime] NULL,
	[PONumber] [varchar](50) NULL,
	[WorkOrderNumber] [varchar](50) NULL,
	[BatchNo] [varchar](50) NULL,
	[VendorName] [varchar](500) NULL,
	[Remarks] [varchar](max) NULL,
	[PreparedBy] [int] NULL,
	[ApprovedBy] [int] NULL,
	[CheckedBy] [int] NULL,
	[Status] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedby] [int] NULL,
 CONSTRAINT [PK_IssueNote] PRIMARY KEY CLUSTERED 
(
	[IssueNoteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IssueNoteDetails]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IssueNoteDetails](
	[IssueNoteDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[IssueNoteId] [int] NULL,
	[ItemId] [int] NULL,
	[QuantityRequested] [float] NULL,
	[QuantityIssued] [float] NULL,
	[CurrentStock] [float] NULL,
	[UnitId] [int] NULL,
	[Description] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedby] [int] NULL,
 CONSTRAINT [PK_IssueNoteDetails] PRIMARY KEY CLUSTERED 
(
	[IssueNoteDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemCategoryMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemCategoryMaster](
	[ItemCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ItemCategoryName] [varchar](70) NULL,
	[Description] [varchar](150) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ItemCategoryMaster] PRIMARY KEY CLUSTERED 
(
	[ItemCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ItemMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemMaster](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ItemCategoryID] [int] NULL,
	[ItemName] [varchar](70) NULL,
	[ItemCode] [nvarchar](50) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ItemMaster] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LabMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabMaster](
	[LabID] [int] IDENTITY(1,1) NOT NULL,
	[LocationID] [int] NULL,
	[FGSID] [int] NULL,
	[LabName] [varchar](100) NULL,
	[LabReport] [varchar](max) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_LabMaster] PRIMARY KEY CLUSTERED 
(
	[LabID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocationMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationMaster](
	[LocationID] [int] IDENTITY(1,1) NOT NULL,
	[LocationName] [varchar](50) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_LocationMaster] PRIMARY KEY CLUSTERED 
(
	[LocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NULL,
	[Thread] [varchar](255) NULL,
	[Level] [varchar](50) NULL,
	[Logger] [varchar](255) NULL,
	[Message] [varchar](4000) NULL,
	[Exception] [varchar](2000) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MachineMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MachineMaster](
	[MachineID] [int] IDENTITY(1,1) NOT NULL,
	[MachineName] [varchar](50) NULL,
	[InstallationDate] [date] NULL,
	[LocationID] [int] NULL,
	[Description] [varchar](500) NULL,
	[ManufacturerName] [varchar](70) NULL,
	[ContactPersonName] [varchar](70) NULL,
	[ContactPersonEmail] [varchar](30) NULL,
	[ContactPersonMobileNo] [varchar](15) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_AssetMachineMaster] PRIMARY KEY CLUSTERED 
(
	[MachineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganisationGroups]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganisationGroups](
	[OrganisationGroupId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Abbreviation] [varchar](50) NULL,
	[Logo] [varchar](max) NULL,
	[StartDate] [date] NULL,
	[Description] [varchar](100) NULL,
	[Details] [varchar](500) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_OrganisationGroups] PRIMARY KEY CLUSTERED 
(
	[OrganisationGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organisations]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organisations](
	[OrganisationId] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationGroupId] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[Abbreviation] [varchar](50) NULL,
	[Logo] [varchar](100) NULL,
	[ContactPerson] [varchar](50) NULL,
	[ContactNo] [varchar](50) NULL,
	[Address] [varchar](100) NULL,
	[Range] [varchar](50) NULL,
	[Division] [varchar](50) NULL,
	[Commisionerate] [varchar](50) NULL,
	[Email] [varchar](50) NULL,
	[Website] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[StateId] [int] NULL,
	[StateCode] [int] NULL,
	[Country] [varchar](50) NULL,
	[PANNo] [varchar](50) NULL,
	[CINNo] [varchar](50) NULL,
	[GSTINNo] [varchar](50) NULL,
	[Description] [varchar](100) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Organisations] PRIMARY KEY CLUSTERED 
(
	[OrganisationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProcessMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProcessMaster](
	[ProcessID] [int] IDENTITY(1,1) NOT NULL,
	[ProcessName] [varchar](50) NULL,
	[ProcessStep] [varchar](max) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ProcessMaster] PRIMARY KEY CLUSTERED 
(
	[ProcessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrder](
	[PurchaseOrderId] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationId] [int] NULL,
	[BranchId] [int] NULL,
	[SupplierId] [int] NULL,
	[PODate] [date] NULL,
	[PONumber] [varchar](50) NULL,
	[DespatchMode] [nvarchar](50) NULL,
	[TransportationMode] [varchar](50) NULL,
	[DeliveryDate] [datetime] NULL,
	[WithComparison] [bit] NULL,
	[Freight] [nvarchar](50) NULL,
	[ComparisionId] [int] NULL,
	[Total] [numeric](18, 2) NULL,
	[Tax] [numeric](18, 2) NULL,
	[OtherCharges] [numeric](18, 2) NOT NULL,
	[GrandTotal] [numeric](18, 2) NULL,
	[PaymentsTerms] [varchar](max) NULL,
	[PaymentTermsDesc] [varchar](max) NULL,
	[DeliveryTerms] [varchar](max) NULL,
	[TransitInsurance] [varchar](max) NULL,
	[PackingAndForwarding] [varchar](max) NULL,
	[TestToBeOffered] [varchar](max) NULL,
	[SupervisionTerms] [varchar](max) NULL,
	[Warranty] [varchar](max) NULL,
	[DeliveryTermsDesc] [varchar](max) NULL,
	[Remarks] [varchar](max) NULL,
	[ApprovalStatus] [int] NULL,
	[ApprovalRemarks] [varchar](max) NULL,
	[Status] [bit] NULL,
	[ApprovedById] [int] NULL,
	[PreparedById] [int] NULL,
	[CheckedById] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[FreightRs] [numeric](18, 2) NULL,
	[FreightGSTPercent] [numeric](18, 2) NULL,
	[CheckedByDate] [datetime] NULL,
	[ApprovedByDate] [datetime] NULL,
	[QuatationNo] [varchar](50) NULL,
 CONSTRAINT [PK_PurchaseOrder] PRIMARY KEY CLUSTERED 
(
	[PurchaseOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseOrderAmendment]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrderAmendment](
	[PurchaseOrderAmendmentId] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseOrderId] [int] NULL,
	[SupplierId] [int] NULL,
	[PONo] [varchar](50) NULL,
	[POAmendNo] [varchar](50) NULL,
	[POAmendDate] [datetime] NULL,
	[PODate] [datetime] NULL,
	[Total] [numeric](18, 2) NULL,
	[PaymentsTerms] [varchar](max) NULL,
	[DeliveryTerms] [nvarchar](max) NULL,
	[TransitInsurance] [varchar](max) NULL,
	[PackingAndForwarding] [varchar](max) NULL,
	[TestToBeOffered] [varchar](max) NULL,
	[SupervisionTerms] [varchar](max) NULL,
	[Warranty] [varchar](max) NULL,
	[ApprovalStatus] [int] NULL,
	[ApprovalRemarks] [varchar](max) NULL,
	[Remarks] [varchar](max) NULL,
	[ApprovedById] [int] NULL,
	[PreparedById] [int] NULL,
	[CheckedById] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[CheckedByDate] [datetime] NULL,
	[ApprovedByDate] [datetime] NULL,
 CONSTRAINT [PK_PurchaseOrderAmendment] PRIMARY KEY CLUSTERED 
(
	[PurchaseOrderAmendmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseOrderAmendmentDetails]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrderAmendmentDetails](
	[PurchaseOrderAmendmentDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[PurchaseOrderAmendmentId] [int] NULL,
	[ItemId] [int] NULL,
	[Quantity] [numeric](18, 2) NULL,
	[AmendQuantity] [numeric](18, 2) NULL,
	[UnitId] [int] NULL,
	[Rate] [numeric](18, 2) NULL,
	[AmendRate] [numeric](18, 2) NULL,
	[Discount] [numeric](18, 2) NULL,
	[CGST] [numeric](18, 2) NULL,
	[SGST] [numeric](18, 2) NULL,
	[IGST] [numeric](18, 2) NULL,
	[AmendDISC] [numeric](18, 2) NULL,
	[AmendCgst] [numeric](18, 2) NULL,
	[AmendSgst] [numeric](18, 2) NULL,
	[AmendIgst] [numeric](18, 2) NULL,
	[Value] [numeric](18, 2) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
 CONSTRAINT [PK_PurchaseOrderAmendmentDetails] PRIMARY KEY CLUSTERED 
(
	[PurchaseOrderAmendmentDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QCProductionSpecificationMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QCProductionSpecificationMaster](
	[QCProductionSpecificationID] [int] IDENTITY(1,1) NOT NULL,
	[ProductionSpecification] [varchar](500) NULL,
	[ItemCategoryID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[ItemCode] [nvarchar](50) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_QCProductionSpecificationMaster] PRIMARY KEY CLUSTERED 
(
	[QCProductionSpecificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QCProductioObservationMaste]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QCProductioObservationMaste](
	[QCProductionObservationID] [int] IDENTITY(1,1) NOT NULL,
	[QCProductionSpecificationID] [int] NOT NULL,
	[ProductionObservation] [varchar](500) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_QCProductioObservationMaste] PRIMARY KEY CLUSTERED 
(
	[QCProductionObservationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QCProductioObservationMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QCProductioObservationMaster](
	[QCProductionObservationID] [int] IDENTITY(1,1) NOT NULL,
	[QCProductionSpecificationID] [int] NOT NULL,
	[ProductionObservation] [varchar](500) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_QCProductioObservationMaster] PRIMARY KEY CLUSTERED 
(
	[QCProductionObservationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecipeMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecipeMaster](
	[RecipeID] [int] IDENTITY(1,1) NOT NULL,
	[RecipeName] [varchar](70) NULL,
	[Description] [varchar](max) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RecipeMaster] PRIMARY KEY CLUSTERED 
(
	[RecipeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RejectionDataSheetMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RejectionDataSheetMaster](
	[RejectionID] [int] IDENTITY(1,1) NOT NULL,
	[RejectionDate] [date] NULL,
	[SupplierID] [int] NOT NULL,
	[PONumber] [varchar](50) NULL,
	[DateOfReceving] [nchar](10) NULL,
	[NameOfMaterial] [nchar](10) NULL,
	[ManufecturingDate] [nchar](10) NULL,
	[BestBefore] [nchar](10) NULL,
	[ItemCategoryID] [int] NULL,
	[ItemID] [int] NULL,
	[ItemCode] [nvarchar](50) NULL,
	[TotalRecevingQuantiy] [float] NULL,
	[TotalRejectedQuantity] [float] NULL,
	[PostRejectedQuantity] [float] NULL,
	[ReasonForRejection] [varchar](max) NULL,
	[CurrectiveAction] [varchar](max) NULL,
	[Remarks] [varchar](max) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RejectionDataSheetMaster] PRIMARY KEY CLUSTERED 
(
	[RejectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestForQuotation]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestForQuotation](
	[RequestForQuotationId] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationId] [int] NULL,
	[BranchId] [int] NULL,
	[RFQNO] [varchar](50) NULL,
	[SupplierId] [int] NULL,
	[Date] [datetime] NULL,
	[RFQYear] [varchar](50) NULL,
	[ItemId] [varchar](max) NULL,
	[QuantityS] [varchar](max) NULL,
	[SOIDS] [varchar](max) NULL,
	[PRIDS] [varchar](max) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RequestForQuotation] PRIMARY KEY CLUSTERED 
(
	[RequestForQuotationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleRights]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleRights](
	[RoleRightId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NULL,
	[ScreenId] [int] NULL,
	[LastModifiedUserId] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[CheckedRole] [bit] NULL,
	[ApprovedRole] [bit] NULL,
	[PreparedRole] [bit] NULL,
	[AddRight] [bit] NULL,
	[UpdateRight] [bit] NULL,
	[DeleteRight] [bit] NULL,
	[ViewScreen] [bit] NULL,
 CONSTRAINT [PK_RoleRight_T] PRIMARY KEY CLUSTERED 
(
	[RoleRightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedby] [int] NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SalesOrder]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesOrder](
	[SalesOrderId] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationId] [int] NULL,
	[BranchId] [int] NULL,
	[SONo] [varchar](50) NULL,
	[SODate] [date] NULL,
	[ClientId] [int] NULL,
	[DispatchTo] [varchar](max) NULL,
	[ServiceType] [varchar](50) NULL,
	[PONo] [varchar](50) NULL,
	[PODate] [date] NULL,
	[TransportationMode] [varchar](50) NULL,
	[QuatationRef] [varchar](50) NULL,
	[DelivaryDate] [datetime] NULL,
	[SOIncharge] [int] NULL,
	[OtherCharges] [numeric](18, 2) NULL,
	[GrandTotal] [numeric](18, 2) NULL,
	[InspectionTerms] [varchar](max) NULL,
	[InspectionTermsDesc] [varchar](max) NULL,
	[PaymentTerms] [varchar](max) NULL,
	[PaymentTermsDesc] [varchar](max) NULL,
	[DeliveryTerms] [varchar](max) NULL,
	[DeliveryTermsDesc] [varchar](max) NULL,
	[DespatchMode] [nvarchar](50) NULL,
	[Freight] [nvarchar](50) NULL,
	[Payment] [nvarchar](50) NULL,
	[TaxId] [int] NULL,
	[TransitInsurance] [varchar](200) NULL,
	[PackingAndForwarding] [varchar](200) NULL,
	[TestToBeOffered] [varchar](200) NULL,
	[SupervisionTerms] [varchar](200) NULL,
	[SOYear] [varchar](50) NULL,
	[ApprovalStatus] [int] NULL,
	[Approvalremarks] [varchar](max) NULL,
	[ApprovedById] [int] NULL,
	[CheckedById] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedById] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedById] [int] NULL,
	[FreightRs] [numeric](18, 2) NULL,
	[FreightGSTPercent] [numeric](18, 2) NULL,
	[IsOpen] [bit] NULL,
	[SpecialRemarks] [varchar](max) NULL,
 CONSTRAINT [PK_SalesOrder] PRIMARY KEY CLUSTERED 
(
	[SalesOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScreenName]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScreenName](
	[ScreenId] [int] IDENTITY(1,1) NOT NULL,
	[DisplayName] [nvarchar](100) NULL,
	[ScreenName] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_RoleRight_ScreenName] PRIMARY KEY CLUSTERED 
(
	[ScreenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SOItem]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SOItem](
	[SOItemId] [int] IDENTITY(1,1) NOT NULL,
	[SalesOrderId] [int] NULL,
	[ItemId] [int] NULL,
	[Quantity] [numeric](18, 2) NULL,
	[UnitId] [int] NULL,
	[Rate] [numeric](18, 2) NULL,
	[IsSchedule] [varchar](50) NULL,
	[Discount] [numeric](18, 2) NULL,
	[TaxId] [int] NULL,
	[Value] [numeric](18, 2) NULL,
	[DeliveryDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedById] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedById] [int] NULL,
	[Remarks] [varchar](max) NULL,
	[JoNo] [varchar](50) NULL,
 CONSTRAINT [PK_SOItem] PRIMARY KEY CLUSTERED 
(
	[SOItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SOItemSchedule]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SOItemSchedule](
	[SOItemScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[SOItemId] [int] NULL,
	[ScheduleQuantity] [numeric](18, 2) NULL,
	[ScheduleDate] [datetime] NULL,
	[ActualDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedById] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedById] [int] NULL,
 CONSTRAINT [PK_SOItemSchedule] PRIMARY KEY CLUSTERED 
(
	[SOItemScheduleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StateMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StateMaster](
	[StateID] [int] IDENTITY(1,1) NOT NULL,
	[CountryID] [int] NOT NULL,
	[StateName] [varchar](70) NULL,
	[StateCode] [int] NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_StateMaster] PRIMARY KEY CLUSTERED 
(
	[StateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[Status] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedById] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedById] [int] NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockAdjustment_M]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockAdjustment_M](
	[StockAdjustmentMId] [int] IDENTITY(1,1) NOT NULL,
	[AdjustmentVoucherNo] [varchar](50) NULL,
	[AdjustmentVoucherDate] [datetime] NULL,
	[AdjustmentVoucherWEF] [datetime] NULL,
	[CheckedBy] [int] NULL,
	[ApprovedBy] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedby] [int] NULL,
 CONSTRAINT [PK_StockAdjustment_M] PRIMARY KEY CLUSTERED 
(
	[StockAdjustmentMId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockAdjustment_T]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockAdjustment_T](
	[StockAdjustmentTId] [int] IDENTITY(1,1) NOT NULL,
	[StockAdjustmentMId] [int] NULL,
	[ItemId] [int] NULL,
	[UnitId] [int] NULL,
	[Bookstock] [float] NULL,
	[Physicalstock] [float] NULL,
	[Reason] [varchar](50) NULL,
	[ShortExcessId] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModificationDate] [datetime] NULL,
	[LastModificationBy] [int] NULL,
 CONSTRAINT [PK_StockAdjustment_T] PRIMARY KEY CLUSTERED 
(
	[StockAdjustmentTId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockMaster](
	[StockID] [int] IDENTITY(1,1) NOT NULL,
	[StorageItemMappingID] [int] NOT NULL,
	[ItemCategoryID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[ItemName] [varchar](200) NULL,
	[StockQuantity] [float] NULL,
	[StockMinLeval] [float] NULL,
	[UnitID] [int] NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_StockMaster] PRIMARY KEY CLUSTERED 
(
	[StockID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockTransfer_M]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockTransfer_M](
	[StockTransferMId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentType] [varchar](10) NULL,
	[SalesOrderId] [int] NULL,
	[TransferNo] [varchar](20) NULL,
	[TransferType] [varchar](10) NULL,
	[Date] [datetime] NULL,
	[FromSite] [int] NULL,
	[ToSite] [int] NULL,
	[PreparedBy] [int] NULL,
	[ApprovedBy] [int] NULL,
	[AuthorisedBy] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedby] [int] NULL,
 CONSTRAINT [PK_StockTransfer] PRIMARY KEY CLUSTERED 
(
	[StockTransferMId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockTransfer_T]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockTransfer_T](
	[StockTransferTId] [int] IDENTITY(1,1) NOT NULL,
	[StockTransferMId] [int] NULL,
	[ItemId] [int] NULL,
	[TransferQuantity] [float] NULL,
	[RequiredQuantity] [float] NULL,
	[Remarks] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedby] [int] NULL,
 CONSTRAINT [PK_StockTransfer_T] PRIMARY KEY CLUSTERED 
(
	[StockTransferTId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StorageItemMapping]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageItemMapping](
	[StorageItemMappingID] [int] IDENTITY(1,1) NOT NULL,
	[StorageLocationID] [int] NULL,
	[ItemID] [int] NULL,
	[Quantity] [float] NULL,
	[UnitID] [int] NULL,
	[RowNumber] [varchar](50) NULL,
	[Per] [nvarchar](50) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_StorageItemMapping] PRIMARY KEY CLUSTERED 
(
	[StorageItemMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StorageLocationMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StorageLocationMaster](
	[StorageLocationID] [int] IDENTITY(1,1) NOT NULL,
	[LocationID] [int] NOT NULL,
	[StorageLocationName] [varchar](70) NULL,
	[StorageDescription] [varchar](max) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_StorageLocationMaster] PRIMARY KEY CLUSTERED 
(
	[StorageLocationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplierMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierMaster](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[SupplierName] [varchar](150) NULL,
	[SupplierCode] [nvarchar](50) NULL,
	[SupplierAddress] [varchar](250) NULL,
	[SupplierEmailID] [varchar](30) NULL,
	[ContactPersonName] [varchar](150) NULL,
	[CountryID] [int] NULL,
	[StateID] [int] NULL,
	[CityID] [int] NULL,
	[Pincode] [nvarchar](10) NULL,
	[StateCode] [nvarchar](10) NULL,
	[BankName] [varchar](200) NULL,
	[BankBranch] [varchar](200) NULL,
	[BankAccountNo] [nvarchar](100) NULL,
	[IFSCCode] [nvarchar](100) NULL,
	[GSTINNumber] [nvarchar](100) NULL,
	[PANNumber] [nvarchar](100) NULL,
	[PaymentTerms] [varchar](max) NULL,
	[DeliveryTerms] [varchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsDeleted] [int] NULL,
 CONSTRAINT [PK_SupplierMaster] PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TermsAndConditionMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TermsAndConditionMaster](
	[TermsConditionID] [int] IDENTITY(1,1) NOT NULL,
	[TermName] [varchar](50) NULL,
	[TermDescription] [varchar](max) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_TermsAndConditionMaster] PRIMARY KEY CLUSTERED 
(
	[TermsConditionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UnitMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UnitMaster](
	[UnitID] [int] IDENTITY(1,1) NOT NULL,
	[UnitName] [varchar](70) NULL,
	[UnitCode] [varchar](20) NULL,
	[Description] [varchar](250) NULL,
	[IsDeleted] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_UnitMaster] PRIMARY KEY CLUSTERED 
(
	[UnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationId] [int] NULL,
	[BranchId] [int] NULL,
	[EmployeeId] [int] NULL,
	[Username] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[RoleId] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [varchar](50) NULL,
	[LastModifiedBy] [int] NULL,
	[LastModifiedDate] [varchar](50) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrder]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrder](
	[WorkOrderId] [int] IDENTITY(1,1) NOT NULL,
	[OrganisationId] [int] NULL,
	[BranchId] [int] NULL,
	[SupplierId] [int] NULL,
	[SONo] [varchar](50) NULL,
	[INNo] [varchar](50) NULL,
	[WODate] [datetime] NULL,
	[WONo] [varchar](50) NULL,
	[DespatchMode] [nvarchar](50) NULL,
	[TransportationMode] [varchar](50) NULL,
	[Freight] [nvarchar](50) NULL,
	[Total] [numeric](18, 2) NULL,
	[Tax] [numeric](18, 2) NULL,
	[OtherCharges] [numeric](18, 2) NULL,
	[GrandTotal] [numeric](18, 2) NULL,
	[PaymentsTerms] [varchar](max) NULL,
	[PaymentTermsDesc] [varchar](max) NULL,
	[DeliveryTerms] [varchar](max) NULL,
	[DeliveryTermsDesc] [varchar](max) NULL,
	[Remarks] [varchar](max) NULL,
	[ApprovedById] [int] NULL,
	[PreparedById] [int] NULL,
	[CheckedById] [int] NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
	[FreightRs] [numeric](18, 2) NULL,
	[FreightGSTPercent] [numeric](18, 2) NULL,
 CONSTRAINT [PK_WorkOrder] PRIMARY KEY CLUSTERED 
(
	[WorkOrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrderDetails]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderDetails](
	[WorkOrderDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderId] [int] NULL,
	[ItemType] [varchar](10) NULL,
	[ItemId] [int] NULL,
	[ItemName] [varchar](max) NULL,
	[Quantity] [numeric](18, 2) NULL,
	[UnitId] [int] NULL,
	[Rate] [numeric](18, 2) NULL,
	[Discount] [numeric](18, 2) NULL,
	[CGST] [numeric](18, 2) NULL,
	[SGST] [numeric](18, 2) NULL,
	[IGST] [numeric](18, 2) NULL,
	[Value] [numeric](18, 2) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[LastModifiedDate] [datetime] NULL,
	[LastModifiedBy] [int] NULL,
 CONSTRAINT [PK_WorkOrderDetails] PRIMARY KEY CLUSTERED 
(
	[WorkOrderDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[YearMaster]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[YearMaster](
	[YearID] [int] IDENTITY(1,1) NOT NULL,
	[YearName] [varchar](9) NULL,
PRIMARY KEY CLUSTERED 
(
	[YearID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Branch]  WITH CHECK ADD  CONSTRAINT [FK_Branch_ToOrganisations] FOREIGN KEY([OrganisationID])
REFERENCES [dbo].[Organisations] ([OrganisationId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Branch] CHECK CONSTRAINT [FK_Branch_ToOrganisations]
GO
ALTER TABLE [dbo].[CityMaster]  WITH CHECK ADD  CONSTRAINT [FK_CityMaster_StateMaster] FOREIGN KEY([StateID])
REFERENCES [dbo].[StateMaster] ([StateID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CityMaster] CHECK CONSTRAINT [FK_CityMaster_StateMaster]
GO
ALTER TABLE [dbo].[CurrencyMaster]  WITH CHECK ADD  CONSTRAINT [FK_CurrencyMaster_ToCountryMaster] FOREIGN KEY([CountryID])
REFERENCES [dbo].[CountryMaster] ([CountryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CurrencyMaster] CHECK CONSTRAINT [FK_CurrencyMaster_ToCountryMaster]
GO
ALTER TABLE [dbo].[Department]  WITH CHECK ADD  CONSTRAINT [FK_Department_ToBranch] FOREIGN KEY([BranchId])
REFERENCES [dbo].[Branch] ([BranchId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_Department_ToBranch]
GO
ALTER TABLE [dbo].[EmployeeMaster]  WITH CHECK ADD  CONSTRAINT [FK_EmployeeMaster_EmployeeMaster] FOREIGN KEY([DesignationID])
REFERENCES [dbo].[DesignationMaster] ([DesignationID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeMaster] CHECK CONSTRAINT [FK_EmployeeMaster_EmployeeMaster]
GO
ALTER TABLE [dbo].[InquiryMaster]  WITH CHECK ADD  CONSTRAINT [FK_InquiryMaster_ToItemCategoryMaster] FOREIGN KEY([ItemCategoryID])
REFERENCES [dbo].[ItemCategoryMaster] ([ItemCategoryID])
GO
ALTER TABLE [dbo].[InquiryMaster] CHECK CONSTRAINT [FK_InquiryMaster_ToItemCategoryMaster]
GO
ALTER TABLE [dbo].[InquiryMaster]  WITH CHECK ADD  CONSTRAINT [FK_InquiryMaster_ToItemMaster] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ItemMaster] ([ItemID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InquiryMaster] CHECK CONSTRAINT [FK_InquiryMaster_ToItemMaster]
GO
ALTER TABLE [dbo].[ItemMaster]  WITH CHECK ADD  CONSTRAINT [FK_ItemMaster_ToItemCategoryMaster] FOREIGN KEY([ItemCategoryID])
REFERENCES [dbo].[ItemCategoryMaster] ([ItemCategoryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ItemMaster] CHECK CONSTRAINT [FK_ItemMaster_ToItemCategoryMaster]
GO
ALTER TABLE [dbo].[LabMaster]  WITH CHECK ADD  CONSTRAINT [FK_LabMaster_ToFinishGoodSeries] FOREIGN KEY([FGSID])
REFERENCES [dbo].[FinishGoodSeries] ([FGSID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LabMaster] CHECK CONSTRAINT [FK_LabMaster_ToFinishGoodSeries]
GO
ALTER TABLE [dbo].[LabMaster]  WITH CHECK ADD  CONSTRAINT [FK_LabMaster_ToLocationMaster] FOREIGN KEY([LocationID])
REFERENCES [dbo].[LocationMaster] ([LocationID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LabMaster] CHECK CONSTRAINT [FK_LabMaster_ToLocationMaster]
GO
ALTER TABLE [dbo].[MachineMaster]  WITH CHECK ADD  CONSTRAINT [FK_MachineMaster_ToLocationMaster] FOREIGN KEY([LocationID])
REFERENCES [dbo].[LocationMaster] ([LocationID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MachineMaster] CHECK CONSTRAINT [FK_MachineMaster_ToLocationMaster]
GO
ALTER TABLE [dbo].[Organisations]  WITH CHECK ADD  CONSTRAINT [FK_Organisations_ToOrganisationGroups] FOREIGN KEY([OrganisationGroupId])
REFERENCES [dbo].[OrganisationGroups] ([OrganisationGroupId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Organisations] CHECK CONSTRAINT [FK_Organisations_ToOrganisationGroups]
GO
ALTER TABLE [dbo].[QCProductionSpecificationMaster]  WITH CHECK ADD  CONSTRAINT [FK_QCProductionSpecificationMaster_ToItemCategoryMaster] FOREIGN KEY([ItemCategoryID])
REFERENCES [dbo].[ItemCategoryMaster] ([ItemCategoryID])
GO
ALTER TABLE [dbo].[QCProductionSpecificationMaster] CHECK CONSTRAINT [FK_QCProductionSpecificationMaster_ToItemCategoryMaster]
GO
ALTER TABLE [dbo].[QCProductionSpecificationMaster]  WITH CHECK ADD  CONSTRAINT [FK_QCProductionSpecificationMaster_ToItemMaster] FOREIGN KEY([ItemID])
REFERENCES [dbo].[ItemMaster] ([ItemID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[QCProductionSpecificationMaster] CHECK CONSTRAINT [FK_QCProductionSpecificationMaster_ToItemMaster]
GO
ALTER TABLE [dbo].[QCProductioObservationMaste]  WITH CHECK ADD  CONSTRAINT [FK_QCProductioObservationMaste_ToQCProductionSpecificationMaster] FOREIGN KEY([QCProductionSpecificationID])
REFERENCES [dbo].[QCProductionSpecificationMaster] ([QCProductionSpecificationID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[QCProductioObservationMaste] CHECK CONSTRAINT [FK_QCProductioObservationMaste_ToQCProductionSpecificationMaster]
GO
ALTER TABLE [dbo].[QCProductioObservationMaster]  WITH CHECK ADD  CONSTRAINT [FK_QCProductioObservationMaster_ToQCProductionSpecificationMaster] FOREIGN KEY([QCProductionSpecificationID])
REFERENCES [dbo].[QCProductionSpecificationMaster] ([QCProductionSpecificationID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[QCProductioObservationMaster] CHECK CONSTRAINT [FK_QCProductioObservationMaster_ToQCProductionSpecificationMaster]
GO
ALTER TABLE [dbo].[RejectionDataSheetMaster]  WITH CHECK ADD  CONSTRAINT [FK_RejectionDataSheetMaster_ToSupplierMaster] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[SupplierMaster] ([SupplierID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RejectionDataSheetMaster] CHECK CONSTRAINT [FK_RejectionDataSheetMaster_ToSupplierMaster]
GO
ALTER TABLE [dbo].[RoleRights]  WITH CHECK ADD  CONSTRAINT [FK_RoleRights_ToRoles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleRights] CHECK CONSTRAINT [FK_RoleRights_ToRoles]
GO
ALTER TABLE [dbo].[RoleRights]  WITH CHECK ADD  CONSTRAINT [FK_RoleRights_ToScreenName] FOREIGN KEY([ScreenId])
REFERENCES [dbo].[ScreenName] ([ScreenId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RoleRights] CHECK CONSTRAINT [FK_RoleRights_ToScreenName]
GO
ALTER TABLE [dbo].[StateMaster]  WITH CHECK ADD  CONSTRAINT [FK_StateMaster_CountryMaster] FOREIGN KEY([CountryID])
REFERENCES [dbo].[CountryMaster] ([CountryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StateMaster] CHECK CONSTRAINT [FK_StateMaster_CountryMaster]
GO
ALTER TABLE [dbo].[StorageLocationMaster]  WITH CHECK ADD  CONSTRAINT [FK_StorageLocationMaster_ToLocationMaster] FOREIGN KEY([LocationID])
REFERENCES [dbo].[LocationMaster] ([LocationID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StorageLocationMaster] CHECK CONSTRAINT [FK_StorageLocationMaster_ToLocationMaster]
GO
/****** Object:  StoredProcedure [dbo].[usp_tbl_OrganisationGroups_M_Insert]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[usp_tbl_OrganisationGroups_M_Insert]
(
	@OrganisationGroupId int = NULL,	
	@Name varchar(200) = NULL,
	@Logo varchar(200) = NULL,	
	@StartDate date = NULL,
	@Description varchar(100) = NULL,
	@Details varchar(500) = NULL,
	@IsDeleted int = NULL,    
	@CreatedBy int = NULL,    
	@CreatedDate date = NULL  
)
AS
--SET NOCOUNT ON
Declare @OrganisationGroupCount int
	set @OrganisationGroupCount = (select COUNT(@OrganisationGroupId) from OrganisationGroups where Name = @Name and IsDeleted = 0)
	If(@OrganisationGroupCount = 0)
Begin
	
	INSERT INTO [OrganisationGroups]
	(		
		[Name],
		Logo,	
		StartDate,
		Description,
		Details,
		IsDeleted,    
		CreatedBy,    
		CreatedDate   
	)
	VALUES
	(
	
		@Name ,
		@Logo ,	
		@StartDate ,
		@Description ,
		@Details ,
		@IsDeleted,    
		@CreatedBy ,    
		@CreatedDate   
	)	
End








GO
/****** Object:  StoredProcedure [dbo].[usp_tbl_Unit_Delete]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Farheen
-- Create date: 22 April 2022
-- Description:	Update unit
-- =============================================
CREATE PROCEDURE [dbo].[usp_tbl_Unit_Delete]
@UnitID int,
@LastModifiedBy int,
@LastModifiedDate datetime
AS
BEGIN

Update UnitMaster set LastModifiedBy=@LastModifiedBy,LastModifiedDate=@LastModifiedDate,IsDeleted=1
where UnitID=@UnitID

END
GO
/****** Object:  StoredProcedure [dbo].[usp_tbl_Unit_GetAll]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Farheen
-- Create date: 22 April 2022
-- Description:	Get all unit names
-- =============================================
CREATE PROCEDURE [dbo].[usp_tbl_Unit_GetAll]
AS
BEGIN
	Select UnitID, UnitName , UnitCode,Description from UnitMaster where IsDeleted=0;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_tbl_Unit_GetByID]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Farheen
-- Create date: 22 April 2022
-- Description:	Get unit by id
-- =============================================
CREATE PROCEDURE [dbo].[usp_tbl_Unit_GetByID]
@UnitID int
AS
BEGIN
	Select UnitID, UnitName, UnitCode,Description from UnitMaster where IsDeleted=0 and UnitID=@UnitID;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_tbl_Unit_Insert]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Farheen
-- Create date: 22 April 2022
-- Description:	Insert unit
-- =============================================
CREATE PROCEDURE [dbo].[usp_tbl_Unit_Insert]
@UnitName varchar(70),
@UnitCode varchar(70),
@Description varchar(250),
@CreatedBy int,
@CreatedDate datetime
AS
BEGIN
Insert into UnitMaster (UnitName,UnitCode,Description,IsDeleted,CreatedBy,CreatedDate)
values
(@UnitName,@UnitCode,@Description,0,@CreatedBy,@CreatedDate)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_tbl_Unit_Update]    Script Date: 05-05-2022 16:31:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Farheen
-- Create date: 22 April 2022
-- Description:	Update unit
-- =============================================
CREATE PROCEDURE [dbo].[usp_tbl_Unit_Update]
@UnitID int,
@UnitName varchar(70),
@UnitCode varchar(70),
@Description varchar(250),
@LastModifiedBy int,
@LastModifiedDate datetime
AS
BEGIN

Update UnitMaster set UnitName=@UnitName, UnitCode=@UnitCode, Description=@Description,
LastModifiedBy=@LastModifiedBy,LastModifiedDate=@LastModifiedDate
where IsDeleted=0 and UnitID=@UnitID

END
GO
USE [master]
GO
ALTER DATABASE [InVanDB] SET  READ_WRITE 
GO

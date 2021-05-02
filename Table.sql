USE [Z0001]
GO

/****** Object:  Table [dbo].[Z001]    Script Date: 27-04-2021 09:52:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Z001](
	[ZF01] [int] IDENTITY(1,1) NOT NULL,
	[ZF02] [varchar](50) NULL,
	[ZF03] [varchar](50) NULL,
	[ZF04] [varchar](500) NULL,
	[ZF05] [varchar](50) NULL,
	[ZF06] [varchar](50) NULL,
	[ZF07] [varchar](50) NULL,
	[ZF08] [varchar](50) NULL,
	[ZF09] [varchar](50) NULL,
	[ZF10] [varchar](20) NULL,
	[ZF11] [varchar](10) NULL,
	[ZF12] [varchar](10) NULL,
	[ZF13] [varchar](10) NULL,
	[ZF14] [varchar](10) NULL,
	[ZF15] [datetime] NULL,
	[ZF16] [datetime] NULL,
 CONSTRAINT [PK_Z001] PRIMARY KEY CLUSTERED 
(
	[ZF01] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Z001] ADD  CONSTRAINT [DF_Z001_ZF15]  DEFAULT (getdate()) FOR [ZF15]
GO

ALTER TABLE [dbo].[Z001] ADD  CONSTRAINT [DF_Z001_ZF16]  DEFAULT (getdate()) FOR [ZF16]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Item Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Z001', @level2type=N'COLUMN',@level2name=N'ZF02'
GO



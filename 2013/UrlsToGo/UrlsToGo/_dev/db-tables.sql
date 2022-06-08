USE [HomeDev]
GO

/****** Object:  Table [dbo].[tb_urls]    Script Date: 2013-06-05 11:51:02 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tb_urls](
	[urlID] [int] IDENTITY(1,1) NOT NULL,
	[urlHitCount] [int] NOT NULL,
	[addedDate] [datetime] NOT NULL,
	[urlLastHitDate] [datetime] NOT NULL,
	[urlShortCode] [varchar](32) NOT NULL,
	[urlFull] [varchar](512) NOT NULL,
 CONSTRAINT [PK_tb_urls] PRIMARY KEY CLUSTERED 
(
	[urlID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tb_urls] ADD  CONSTRAINT [DF_tb_urls_dateAdded]  DEFAULT (getutcdate()) FOR [addedDate]
GO
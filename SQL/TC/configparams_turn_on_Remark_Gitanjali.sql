/****** Script for SelectTopNRows command from SSMS  ******/
--INSERT INTO [trums].[dbo].[SupplierConfigsParams] ([SupplierConfigsId],[Key],[Value])
--VALUES (19, 'Remark', '*TC*Created by Tripchamp')


/****** Script for SelectTopNRows command from SSMS  ******/
	DECLARE @accessfaresHH int = 12
	DECLARE @skybird int = 18
	DECLARE @gitanjali int = 19
	DECLARE @transam int = 30
	DECLARE @consolid int = 31


INSERT INTO [dbo].[SupplierConfigsParams] ([SupplierConfigsId],[Key],[Value])
VALUES (@consolid, N'clientId', N'"1A2B3C4D"'),(@consolid, N'originatorIP', N'192.168.11.181')

--VALUES (@skybird, 'StoreFareQuoteType', 'NoStoreFareQuote') -- 'BSSSettings', 'StoreFareQuote')


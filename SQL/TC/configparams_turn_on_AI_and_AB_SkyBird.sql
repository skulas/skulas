/****** Script for SelectTopNRows command from SSMS  ******/
DECLARE @accessfaresHH int = 12
DECLARE @skybird int = 18
DECLARE @gitanjali int = 19


--INSERT INTO [SupplierConfigsParams] ([SupplierConfigsId],[Key],[Value])
--VALUES (@skybird, 'Remark', '$$AI:AN:15126777283')

--INSERT INTO [SupplierConfigsParams] ([SupplierConfigsId],[Key],[Value])
--VALUES (@skybird, 'Remark', '$$AB:NA:1010 Land Creek Cove Ste 150 Austin TX')

--INSERT INTO [SupplierConfigsParams] ([SupplierConfigsId],[Key],[Value])
--VALUES (@skybird, '3Docs', 'true')

--INSERT INTO [SupplierConfigsParams] ([SupplierConfigsId],[Key],[Value])
--VALUES (@skybird, 'APX', 'skybird')

INSERT INTO [SupplierConfigsParams] ([SupplierConfigsId],[Key],[Value])
VALUES (@skybird, 'StoreFareQuoteType', 'NoStoreFareQuote')

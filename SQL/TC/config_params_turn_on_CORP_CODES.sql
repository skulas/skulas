/****** Script for SelectTopNRows command from SSMS  ******/

-- USE [trums]

DECLARE @accessfaresHH int = 12
DECLARE @skybird int = 18
DECLARE @gitanjali int = 19


INSERT INTO [dbo].[SupplierConfigsParams] ([SupplierConfigsId],[Key],[Value])
VALUES (@gitanjali, 'SupplierSpecificCorporateCodesHyphenDelimited', '16092-621312-690726')

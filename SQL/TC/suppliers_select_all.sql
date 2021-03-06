/****** Script for SelectTopNRows command from SSMS  ******/

-- USE [trums]

	DECLARE @akbar int = 20
	DECLARE @akbarINTL int = 27



SELECT TOP 1000 [Id]
      ,[ParentId]
      ,[SupplierTypeConfigId]
      ,[IsEnabled]
      ,[ProductType]
      ,[Name]
      ,[Code]
      ,[TestAccountId]
      ,[TestUsername]
      ,[TestPassword]
      ,[TestUrl]
      ,[LiveAccountId]
      ,[LiveUsername]
      ,[LivePassword]
      ,[LiveUrl]
  FROM [dbo].[SupplierConfigs]
   where isEnabled = 1 -- and ProductType = 1
  -- where ProductType = 2
  -- where [name] like '%trans%'
  --where [Code] in ('V3.0w', 'V3.7', 'V8 - Live', 'V1 - Live', 'V11', 'V10', 'V2 - Live')
  --where id in (@akbar, @akbarINTL)
/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [Id]
      ,[SupplierConfigsId]
      ,[Key]
      ,[Value]
  FROM [trums].[dbo].[SupplierConfigsParams]
  WHERE [Key] like '3Docs'
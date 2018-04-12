/****** Script for SelectTopNRows command from SSMS  ******/
DECLARE @accessfaresHH int = 12
DECLARE @skybird int = 18
DECLARE @gitanjali int = 19
DECLARE @transam int = 30
DECLARE @consolid int = 31

SELECT TOP 1000 [Id]
      ,[SupplierConfigsId]
      ,[Key]
      ,[Value]
  FROM [dbo].[SupplierConfigsParams]
   Where SupplierConfigsId = @skybird
  --Where [Key] like 'clientId'
/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[SupplierConfigsId]
      ,[Key]
      ,[Value]
  FROM [dbo].[SupplierConfigsParams]
  where [SupplierConfigsId] = 28



delete  FROM [dbo].[SupplierConfigsParams]
  where [SupplierConfigsId] = 28

update [dbo].[SupplierConfigsParams]
set [Value] = N'http://data.zumata.com '
where [key]=N'StaticDataUrl' AND [SupplierConfigsId]=28


  Insert into [SupplierConfigsParams]
  values (28,N'StaticDataUrl',  N'http://staging-data.zumata.com')

  Insert into [SupplierConfigsParams]
  values (33,N'StaticDataUrl',  N'http://staging-data.zumata.com')
    
/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[CompanyId]
      ,[PhoneNumber]
      ,[Email]
      ,[Name]
      ,[Role_Id]
  FROM [TCData].[dbo].[LPUsers]
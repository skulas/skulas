/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[CompanyName]
      ,[Approved]
      ,[TravelManagementCompany]
      ,[MonthlyInternationalSpend]
      ,[MonthlyDomesticSpend]
  FROM [TCData].[dbo].[LPCompanies]
/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Date]
      ,[UserID]
      ,[ProcessToken]
      ,[Logger]
      ,[Message]
      ,[Exception]
      ,[Env]
  FROM [TCData].[dbo].[UserTracing]
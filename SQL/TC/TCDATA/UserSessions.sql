/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [UserName]
      ,[SessionId]
      ,[IP]
      ,[Item]
  FROM [TCData].[dbo].[UserSessionInfo]


  select count(SessionId)

  FROM [TCData].[dbo].[UserSessionInfo]

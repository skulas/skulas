/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [id]
      ,[Name]
      ,[Parent]
      ,[Customer]
      ,[ApiKey]
      ,[KeyRoleName]
      ,[Rules]
      ,[MarkUp]
      ,[Status]
  FROM [dbo].[WhiteLabel]

  update [WhiteLabel]  set [Name]='Vacationchamp' where [Name]='Tripchamp'
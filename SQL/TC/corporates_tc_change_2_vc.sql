/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [id]
      ,[Name]
      ,[ContactPerson]
      ,[adress]
      ,[PricePlan]
      ,[PaymentMethod]
      ,[Package]
      ,[Discount]
      ,[smtplInfo]
      ,[BillingEmail]
      ,[AdminEmail]
      ,[ErrEmail]
      ,[LogoURL]
      ,[ParentCustomer]
      ,[Template]
  FROM [dbo].[Corporates]

  update Corporates  set [Name]='Vacationchamp' where [Name]='Tripchamp'
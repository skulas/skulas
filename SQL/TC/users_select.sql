/****** Script for SelectTopNRows command from SSMS  ******/

DECLARE @ezeuid varchar(75) = 'D540A1A5-CA0D-4405-B5B6-A6234C965500'
DECLARE @ccadminid varchar(75) = 'C4330EBE-F340-4D2A-8D1D-37CF0F6E410B'


SELECT TOP (1000) [userId]
      ,[Customer]
      ,[StartDate]
      ,[DBirth]
      ,[Title]
      ,[FirstName]
      ,[MiddleName]
      ,[LastName]
      ,[Address1]
      ,[Address2]
      ,[City]
      ,[State]
      ,[Country]
      ,[Zip]
      ,[TimeZone]
      ,[TimeZonePlus]
      ,[WeekendsSMS]
      ,[WeekendEmails]
      ,[Email]
      ,[Phone]
      ,[APIUser]
      ,[LastLogin]
      ,[PhoneCountryCode]
      ,[PhoneNumber]
      ,[HomeAirportCode]
      ,[EmergencyContactFullName]
      ,[EmergencyContactPhoneCountryCode]
      ,[EmergencyContactPhoneNumber]
      ,[PassportCountryOfIssue]
      ,[PassportNumber]
      ,[PassportIssueDate]
      ,[PassortExpirationDate]
      ,[RedressNumber]
      ,[TrustedTravelerNumber]
      ,[FavoriteSupplier]
      ,[Currency]
  FROM [dbo].[Users]
  where userId = @ccadminid
  --Where FirstName like 'ruise'
  --where customer != 0
  --where APIUser != NULL
    order by StartDate desc

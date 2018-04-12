

--USE [trums]


	DECLARE @airportCode varchar(3) = 'HKG'
	DECLARE @airportName varchar(75) = 'International Airport'


  UPDATE [dbo].[Airports]
 SET [Name] = @airportName  --,  LiveUrl = @liveURL
 --SET LivePassword = 'apitest',
 -- LiveUrl = @testURL
 
  WHERE [code] = @airportCode

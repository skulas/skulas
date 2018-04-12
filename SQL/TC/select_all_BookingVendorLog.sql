/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [id]
      ,[BookingID]
      ,[BookingRequest]
      ,[BookingResponse]
      ,[Vendor]
      ,[Request]
      ,[product]
      ,[Messages]
      ,[errors]
      ,[BookTime]
  FROM [trums].[dbo].[BookingVendorLog]
  Order by BookTime

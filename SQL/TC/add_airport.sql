/****** Script for SelectTopNRows command from SSMS  ******/

USE [trums]

--INSERT INTO [dbo].[SupplierConfigsParams] ([SupplierConfigsId],[Key],[Value])
--VALUES (19, 'Remark', '*TC*Created by Tripchamp')

INSERT INTO [dbo].[Airports] ([code],[WorldAreaCode],[Country],[CounntryAb],[Name],[City],[GMT],[tzcode],[longtitude],[latitude],[worldCodeUrl],[tl],[AirportUrl])
--VALUES ('XNB',941,'United Arab Emirates','AE','Dubai','Dubai','+4.0','Asia/Dubai')
VALUES ('DXB',971,'United Arab Emirates','AE','International Airport Dubai','Dubai','+4.0','Asia/Dubai','55.3657° E','25.2532° N','united-arab-emirates/dubai-2003.html','+971 4 216 2525','http://www.dubaiairport.com')



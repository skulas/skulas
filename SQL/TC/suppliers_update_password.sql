

-- USE [trums]


	DECLARE @akbar int = 20
	DECLARE @akbarINTL int = 27

	DECLARE @testURL varchar(75) = 'http://test.benzyinfotech.com:8013/WPFlights.svc'
	DECLARE @testUsername varchar(30) = 'apitest'
	DECLARE @testPassword varchar(30) = 'apitest'

	DECLARE @liveURL varchar(75) = 'http://wc.akbartravelsonline.com/WPFlights.svc'
	DECLARE @liveUsername varchar(30) = 'tripchamp'
	DECLARE @livePassword varchar(30) = 'summer79'

  UPDATE [dbo].[SupplierConfigs]
	SET [LiveUsername] = @liveUsername, [LivePassword] = @livePassword, [LiveUrl] = @liveURL, 
	[TestUsername] = @testUsername, [TestPassword] = @testPassword, [TestUrl] = @testURL
 --SET LivePassword = 'apitest',
 -- LiveUrl = @testURL
 
  WHERE Id in (@akbarINTL,@akbar)


  -- ----------------

  	DECLARE @name varchar(10) = 'Zumata'
	DECLARE @productType int = 2
	DECLARE @isEnabled int = 1
	DECLARE @testURL varchar(75) = 'https://test-api-v3.zumata.com'
	DECLARE @testAccountId varchar(30) = 'fce58cb1-1fa1-c3e3-463d-121af67fa98c'
	DECLARE @code varchar(3) = 'V21'
	DECLARE @supplierTypeConfigId int = 28


	DECLARE @liveURL varchar(75) = 'https://api-v3.zumata.com'
	DECLARE @liveAccountId varchar(90) = 'db60b2bd-b790-710c-0da8-ed4a358dd1de'
	
	insert into [dbo].[SupplierConfigs] ([SupplierTypeConfigId], [IsEnabled], [ProductType],[Name],[Code],[TestAccountId],[TestUrl],[LiveAccountId],[LiveUrl])
	values (@supplierTypeConfigId, @isEnabled, @productType, @name, @code, @testAccountId, @testURL, @liveAccountId, @liveURL)


  -- ----------------


  SET IDENTITY_INSERT [SupplierTypeConfigs] ON

 	DECLARE @name varchar(10) = 'Zumata'
	DECLARE @productType int = 2
	DECLARE @supplierTypeConfigId int = 28
	DECLARE @asemblyName varchar(20) = 'Zumata'
	DECLARE	@typeName varchar(20) = 'Zumata.ZumataVendor'
	
	insert into [dbo].[SupplierTypeConfigs] ([Id],[ProductType],[Name],[AssemblyName],[TypeName])
	values (@supplierTypeConfigId, @productType, @name, @asemblyName, @typeName)

SET IDENTITY_INSERT [SupplierTypeConfigs] OFF

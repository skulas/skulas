

-- USE [trums]


	DECLARE @consolid int = 29 -- 31 in dev!!

	-- DECLARE @testURL varchar(75) = 'http://test.benzyinfotech.com:8013/WPFlights.svc'
	DECLARE @testUsername varchar(30) = 'tripchamp'
	DECLARE @testPassword varchar(30) = 'nFqbnbWi'

	-- DECLARE @liveURL varchar(75) = 'http://wc.akbartravelsonline.com/WPFlights.svc'
	DECLARE @liveUsername varchar(30) = 'tripchamp'
	DECLARE @livePassword varchar(30) = 'nFqbnbWi'


  UPDATE [dbo].[SupplierConfigs]
	SET [LiveUsername] = @liveUsername, [LivePassword] = @livePassword, 
	[TestUsername] = @testUsername, [TestPassword] = @testPassword

	--SET [LiveUsername] = @liveUsername, [LivePassword] = @livePassword, [LiveUrl] = @liveURL, 
	--[TestUsername] = @testUsername, [TestPassword] = @testPassword, [TestUrl] = @testURL
 --SET LivePassword = 'apitest',
 -- LiveUrl = @testURL
 
  WHERE Id in (@consolid)

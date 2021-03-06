-- Disable all but AccessFares

-- USE [trums]

	DECLARE @mistyfly int = 1
	DECLARE @LBF int = 2
	DECLARE @riya int = 3
	DECLARE @WorldTravelHH int = 5
	DECLARE @StatemanHH int = 6
	DECLARE @ATEHH int = 7
	DECLARE @BirdHH int = 8
	DECLARE @InternalAmadeusHH int = 9
	DECLARE @VirtuosoHH int = 10
	DECLARE @accessfaresHH int = 12
	DECLARE @skybird int = 18
	DECLARE @gitanjali int = 19
	DECLARE @akbar int = 20
	DECLARE @akbarINTL int = 27

	-------------
	-- HOTELS
	DECLARE @SimplyTravel int = 13
	DECLARE @HotUsa int = 14
	DECLARE @Travco int = 17
	DECLARE @TouricoHotels int = 22
	DECLARE @HotelBeds int = 23
	DECLARE @ResorTime int = 24

  UPDATE [dbo].[SupplierConfigs]
  SET IsEnabled = 1
  where name = 'Zumata'

  UPDATE [dbo].[SupplierConfigs]
  SET IsEnabled = 0
 --WHERE Id = @skybird
  -- WHERE Id <> 3
   -- WHERE not Id in (1,2,3,4,13,14,15,16,17,19,20,22,23,24,27,28) -- (@WorldTravelHH,@StatemanHH,@ATEHH,@BirdHH, @InternalAmadeusHH, @VirtuosoHH) 
   -- WHERE Id in (@akbar, @akbarINTL, @TouricoHotels, @HotelBeds, @ResorTime, @Travco, @HotUsa, @SimplyTravel) --(1,2,3,4,13,14,16,17,19,20,22,23,24,27,28) --(1,2,3,4,13,14,15,16,17,19,20,22,23,24,27,28)
   where Name in ('HitchHikerVendor_WebRates','HitchHikerVendor_BIRD','HitchHikerVendor_Virtuoso','Simply Travel','Travco','TouricoHomeRentals','TouricoHotels','ResorTime','TouricoCars')

  --		  (1,2,3,4,11,19,20)
  -- 


  select * from [dbo].[SupplierConfigs]
  where isenabled = 0

 UPDATE [dbo].[SupplierConfigs]
 -- SET TestUrl = 'https://api-v3.zumata.com'
 Set TestAccountId = 'db60b2bd-b790-710c-0da8-ed4a35'
  where name = 'Zumata'


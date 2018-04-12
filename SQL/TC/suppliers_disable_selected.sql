
	USE [trums]

	DECLARE @mistyfly int = 1
	DECLARE @LBF int = 2
	DECLARE @LBF_hotel int = 15
	DECLARE @riya int = 3
	DECLARE @accessfaresHH int = 12
	DECLARE @skybird int = 18
	DECLARE @gitanjali int = 19
	DECLARE @akbar int = 20
	DECLARE @akbarINTL int = 27


  UPDATE [dbo].[SupplierConfigs]
  SET IsEnabled = 0
  -- WHERE Id <> 3
   WHERE Id in (@LBF,@riya, @LBF_hotel) 

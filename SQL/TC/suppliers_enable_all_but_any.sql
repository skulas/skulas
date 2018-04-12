-- Disable all but AccessFares

select * from [SupplierConfigs] where IsEnabled = 1


USE [trums]

  --UPDATE [dbo].[SupplierConfigs]
  --SET IsEnabled = 1
  --WHERE not Id in (23,24,26)

  	DECLARE @internalAmadeusAgentHH int = 9
	DECLARE @webFaresHH int = 4
	DECLARE @accessfaresHH int = 12
	DECLARE @skybird int = 18
	DECLARE @gitanjali int = 19

	DECLARE @zumata int = 28


UPDATE [dbo].[SupplierConfigs]
  SET IsEnabled = 0
  
UPDATE [dbo].[SupplierConfigs]
  SET IsEnabled = 1
  where [SupplierTypeConfigId] = @zumata

  --WHERE not Id in (@webFaresHH, @internalAmadeusAgentHH)
  -- where [Code] in ('V1 - Live', 'V2 - Live', 'V3.0w', 'V3.7', 'V3.11', 'V8 - Live', 'V10', 'V11', 'V16', 'V17')  

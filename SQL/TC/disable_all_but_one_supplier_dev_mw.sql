-- Disable all but AccessFares
DECLARE @accessfaresHH int = 12
DECLARE @skybird int = 18
DECLARE @gitanjali int = 19
DECLARE @Akbar int = 20
DECLARE @transam int = 30
DECLARE @consolid int = 31


  UPDATE [dev-middleware].[dbo].[SupplierConfigs]
  SET IsEnabled = 0
  WHERE Id <> 20 --Akbar-20 AccessFares-12
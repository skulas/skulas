UPDATE [trums].[dbo].[SupplierConfigs]
SET IsEnabled=0 /*1*/
WHERE Code in ('V3.1','V3.2','V3.3','V3.4','V3.5','V3.6','V3.8')

DECLARE @PCC VARCHAR(10) = 'TRIPCHAMP'
DECLARE	@webRates int = 4

UPDATE [dbo].[SupplierConfigs]
SET [TestAccountId]=@PCC,[LiveAccountId]=@PCC
WHERE [Id] = @webRates

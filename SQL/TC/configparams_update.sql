-- Disable all but AccessFares
use [dev-middleware]

DECLARE @accessfaresHH int = 12
DECLARE @skybird int = 18
DECLARE @gitanjali int = 19
DECLARE @transam int = 30

UPDATE [dbo].[SupplierConfigsParams]
SET [value] = 'TRS1026'
WHERE [SupplierConfigsId] = @transam and [key] like 'clientId'
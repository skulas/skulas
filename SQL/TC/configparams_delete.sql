-- use [trums]

/****** Script for SelectTopNRows command from SSMS  ******/
DECLARE @accessfaresHH int = 12
DECLARE @skybird int = 18
DECLARE @gitanjali int = 19
DECLARE @transam int = 30


DELETE FROM [dbo].[SupplierConfigsParams]
WHERE SupplierConfigsId = @skybird AND id in (67)  -- [KEY] = 'TicketingDateAddMonths'
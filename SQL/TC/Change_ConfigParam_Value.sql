-- Edit a param
  UPDATE [trums].[dbo].[SupplierConfigsParams]
  SET [Value] = '2'
  WHERE [SupplierConfigsId] = 19 and [Key] = 'StoreFareQuoteType'
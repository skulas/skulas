/****** Script for SelectTopNRows command from SSMS  ******/
USE [trums]

SELECT TOP 1000 [SourceType]
      ,[TargetType]
      ,[SourceFieldName]
      ,[TargetFieldName]
      ,[SourceDLL]
      ,[TargetDLL]
  FROM [dbo].[NormalizerFieldMapping]

-- Drop table [dbo].[NormalizerFieldMapping]
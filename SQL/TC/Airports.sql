/****** Script for SelectTopNRows command from SSMS  ******/


USE [trums]

SELECT TOP (1000) [code]
      ,[WorldAreaCode]
      ,[Country]
      ,[CounntryAb]
      ,[Name]
      ,[AirportUrl]
      ,[City]
      ,[AirportGuiseUrl]
      ,[GMT]
      ,[tl]
      ,[email]
      ,[longtitude]
      ,[latitude]
      ,[worldCodeUrl]
      ,[Region]
      ,[tzcode]
  FROM [dbo].[Airports]
  where code in ('ruh', 'auh') --, 'bom','dxb','xnb','hkg')

  


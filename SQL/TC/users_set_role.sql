/****** Script for SelectTopNRows command from SSMS  ******/
DECLARE @ezeuid varchar(75) = 'D540A1A5-CA0D-4405-B5B6-A6234C965500'
DECLARE @ccadminid varchar(75) = 'C4330EBE-F340-4D2A-8D1D-37CF0F6E410B'

DECLARE @tcAdminRoleId varchar(75) = 'EA234AE2-23FE-14A4-D3AB-12B4E1354A67'
DECLARE @adminRoleId varchar(75) = '55A7466B-9DF2-4E34-B2DC-1DA6389FCE66'
DECLARE @basicUser varchar(75) = 'E486DA03-211E-4F65-82F0-B051C0317D2A'


-- update aspnet_UsersInRoles set RoleId = @tcAdminRoleId where userid = @ccadminid

SELECT TOP (1000) [UserId]
      ,[RoleId]
  FROM [dbo].[aspnet_UsersInRoles]
  where UserId = @ccadminid

  -- Admin RoleId 55A7466B-9DF2-4E34-B2DC-1DA6389FCE66

  -- Basic User: E486DA03-211E-4F65-82F0-B051C0317D2A

 
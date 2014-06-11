
/****** Object:  View [dbo].[V_UserPermission]    Script Date: 11/18/2010 12:00:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[V_UserRole]
AS
SELECT     dbo.[User].UserID, dbo.Role.RoleID, dbo.Role.Title, dbo.[User].UserName, dbo.[User].Email, dbo.[User].Name, dbo.[User].Nickname
FROM         dbo.[User] INNER JOIN
                      dbo.UserRole ON dbo.[User].UserID = dbo.UserRole.UserID INNER JOIN
                      dbo.Role ON dbo.UserRole.RoleID = dbo.Role.RoleID

GO



CREATE VIEW [dbo].[V_UserPermission]
AS
SELECT     dbo.[User].UserID, dbo.[User].UserName, dbo.[User].Email, dbo.[User].Nickname, dbo.Permission.Title, dbo.Permission.Url, dbo.Permission.Flag, 
                      dbo.Permission.PermissionType, dbo.Permission.ParentID, dbo.Permission.PermissionStatus, dbo.Permission.DisplayOrder, dbo.Permission.PermissionID, 
                      dbo.[User].TenantID
FROM         dbo.Permission INNER JOIN
                      dbo.RolePermission ON dbo.Permission.PermissionID = dbo.RolePermission.PermissionID INNER JOIN
                      dbo.[User] INNER JOIN
                      dbo.UserRole ON dbo.[User].UserID = dbo.UserRole.UserID ON dbo.RolePermission.RoleID = dbo.UserRole.RoleID

GO
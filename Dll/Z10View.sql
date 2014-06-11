

CREATE VIEW [dbo].[V_DepotProduct]
AS
SELECT     dbo.Z10Depot.DepotID, dbo.Z10DepotProduct.DepotProductID, dbo.Z01Product.ProductID, dbo.Z01Product.Title AS ProductTitle, dbo.Z10Depot.Title AS DepotTitle, 
                      dbo.Z10Depot.Code, dbo.Z10Depot.[Content], dbo.Z10Depot.Manager, dbo.Z10DepotProduct.InSum, dbo.Z10DepotProduct.OutSum, dbo.Z10DepotProduct.StockSum, 
                      dbo.Z10DepotProduct.CountAlarm, dbo.Z01Product.PriceList, dbo.Z01Product.PriceStock, dbo.Z01Product.PriceSelling, dbo.Z01Product.PriceSellOff1, 
                      dbo.Z01Product.PriceSellOff2, dbo.Z01Product.PriceSellOff3, dbo.Z01Product.UnitID
FROM         dbo.Z01Product RIGHT OUTER JOIN
                      dbo.Z10DepotProduct ON dbo.Z01Product.ProductID = dbo.Z10DepotProduct.ProductID LEFT OUTER JOIN
                      dbo.Z10Depot ON dbo.Z10DepotProduct.DepotID = dbo.Z10Depot.DepotID

GO


CREATE VIEW [dbo].[V_DepotFlow]
AS
SELECT     dbo.Z10Depot.Title AS DepotTitle, dbo.Z01Product.Title AS ProductTitle, dbo.Z10DepotFlow.Count, dbo.Z10DepotFlow.TenantID, dbo.Z10DepotFlow.DepotID, 
                      dbo.Z10DepotFlow.ProductID, dbo.Z10DepotFlow.CreateDate, dbo.Z10DepotFlow.FlowType, dbo.Z10DepotFlow.FlowID, dbo.Z10DepotFlow.OrderID
FROM         dbo.Z10DepotFlow LEFT OUTER JOIN
                      dbo.Z10Depot ON dbo.Z10DepotFlow.DepotID = dbo.Z10Depot.DepotID LEFT OUTER JOIN
                      dbo.Z01Product ON dbo.Z10DepotFlow.ProductID = dbo.Z01Product.ProductID

GO

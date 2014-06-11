--删除订单

create proc DeleteOrder  (@orderid bigint)
as

delete from Z10OrderItem where OrderID=@orderid;
delete from  Z10DepotFlow where OrderID=@orderid;
delete from  Z10DepotProductDetail where OrderID=@orderid;
delete from Z10Order where OrderID=@orderid;
--delete from  Z01FinancialFlow where OrderID=@orderid;
go




---删除产品
declare @pid  bigint
set @pid=81
--delete from Z10DepotProduct where ProductID=@pid
--delete from Z10DepotProductDetail where ProductID=@pid
delete from Z01Product where ProductID=@pid
delete from Z01ProductInCategory where ProductID = @pid

----删除库存中某一个项目，并更新数量
select * from Z10DepotProduct where ProductID=50301
select * from Z10DepotProductDetail where ProductID=50301
delete from Z10DepotProductDetail where DepotProductID=10324
update Z10DepotProduct set StockSum=2, InSum=2 where ProductID=50301





/*
---重置数据
truncate table [dbo].[Z01FinancialFlow];
truncate table [dbo].[Z01Customer];
truncate table [dbo].[Z01CustomerFlow];
truncate table [dbo].[Z01CustomerFlowItem]
truncate table [dbo].[Z01CustomerFlowItemDetail]
truncate table [dbo].[Z01CustomerFlowItemExtend]
truncate table [dbo].[Z01CustomerGoods]
truncate table [dbo].[Z01CustomerInCategory]
truncate table [dbo].[Z01CustomerPerson]
truncate table [dbo].[Z01Depot]
truncate table [dbo].[Z01DepotFlow]
truncate table [dbo].[Z01DepotFlowItem]
truncate table [dbo].[Z01DepotFlowItemBookCode]
truncate table [dbo].[Z01DepotFlowItemBoxCode]
truncate table [dbo].[Z01DepotFlowItemDetail]
truncate table [dbo].[Z01DepotFlowItemExtend]
truncate table [dbo].[Z01DepotGoods]
truncate table [dbo].[Z01FinancialCategory]
truncate table [dbo].[Z01FinancialFlow]
truncate table [dbo].[Z01PaperTemplate]
truncate table [dbo].[Z01SlipActionHistory]
truncate table [dbo].[Z10Depot]
truncate table [dbo].[Z10DepotFlow]
truncate table [dbo].[Z10DepotProduct]
truncate table [dbo].[Z10DepotProductDetail]
truncate table [dbo].[Z10Order]
truncate table [dbo].[Z10OrderItem]

--如果需要删除商品
truncate table [dbo].[Z01ProductInCategory]
truncate table [dbo].[Z01Product]
truncate table [dbo].[Z01Brand]



delete from [User] where userid>1;


*/
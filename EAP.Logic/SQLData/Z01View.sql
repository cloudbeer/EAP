

CREATE VIEW [dbo].[V_FinancialFlow]
AS
SELECT     dbo.Z01Bank.Title AS BankTitle, dbo.Z01FinancialCategory.Title AS CategoryTitle, dbo.Z01FinancialFlow.*
FROM         dbo.Z01FinancialFlow LEFT OUTER JOIN
                      dbo.Z01Bank ON dbo.Z01FinancialFlow.BankID = dbo.Z01Bank.BankID LEFT OUTER JOIN
                      dbo.Z01FinancialCategory ON dbo.Z01FinancialFlow.CategoryID = dbo.Z01FinancialCategory.CategoryID

GO

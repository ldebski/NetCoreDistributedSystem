CREATE DATABASE TransfersDataBase
GO

IF OBJECT_ID('TransfersDataBase.dbo.Tranfer', 'u') IS NOT NULL 
  DROP TABLE TransfersDataBase.dbo.Tranfer;

GO

CREATE TABLE TransfersDataBase.dbo.Tranfer (
    TransferID int NOT NULL UNIQUE,
	CashAmount int,
	SenderID int,
	ReceiverID int,
	Status char(10)
);

GO
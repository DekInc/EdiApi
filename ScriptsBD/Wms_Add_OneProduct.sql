BEGIN
DECLARE @MaxTransaccionId INT;
SELECT @MaxTransaccionId = ISNULL(MAX(a.Id), 0) + 1 FROM EdiDb.dbo.EdiRepSent a
SELECT @MaxTransaccionId;
END
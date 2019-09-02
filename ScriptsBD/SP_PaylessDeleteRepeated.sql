USE EdiDB
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID('SP_PaylessDeleteRepeated', 'P') IS NOT NULL
	DROP PROC SP_PaylessDeleteRepeated
GO
CREATE PROCEDURE dbo.SP_PaylessDeleteRepeated 
AS
BEGIN
	DECLARE @Barcode VARCHAR(11)
	DECLARE @IdFirst INT
	DECLARE @Count INT

	DECLARE ProdCur CURSOR FOR
	SELECT DISTINCT	--TOP 10000
		D.Barcode
	FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D 
	ORDER BY D.Barcode

	OPEN ProdCur

	FETCH NEXT FROM ProdCur
	INTO @Barcode

	WHILE @@FETCH_STATUS = 0
	BEGIN
		SELECT TOP 1 @IdFirst = D.Id
		FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D
		WHERE D.Barcode = @Barcode

		SELECT @Count = COUNT(*)
		FROM EdiDB.dbo.PAYLESS_ProdPrioriDet D
		WHERE D.Barcode = @Barcode
		IF (@Count > 1)
		BEGIN
			--PRINT CONVERT(VARCHAR(32), @IdFirst	) + ' - ' + @Barcode + ' - ' +  CONVERT(VARCHAR(32), @Count)
			DELETE FROM EdiDB.dbo.PAYLESS_ProdPrioriDet
			WHERE Id IN (
				SELECT Id 
				FROM EdiDB.dbo.PAYLESS_ProdPrioriDet
				WHERE Barcode = @Barcode
				AND Id != @IdFirst
			)
			--PRINT @@ROWCOUNT
		END

		FETCH NEXT FROM ProdCur
		INTO @Barcode
	END
	CLOSE ProdCur
	DEALLOCATE ProdCur
END
GO

EXEC SP_PaylessDeleteRepeated
--25011
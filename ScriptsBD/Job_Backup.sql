USE websilce;
GO
declare @fecha varchar(MAX)
declare @archivo varchar(MAX)
set @fecha = CONVERT(Varchar(max), GETDATE(),102)+'_'+SUBSTRING(CONVERT(varchar(10), getdate(),108),1,2)+SUBSTRING(CONVERT(varchar(10), getdate(),108),4,2)
set @archivo ='C:\CompartidaSilceAdmin\websilce_' + @fecha + '.bak'
BACKUP DATABASE websilce
TO DISK = @archivo
   WITH FORMAT,
      MEDIANAME = 'D_SQLServerBackups',
      NAME = 'Full Backup of websilce - Hilmer';
GO
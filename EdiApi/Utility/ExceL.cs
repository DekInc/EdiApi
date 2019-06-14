using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdiApi.Utility {
    public class ExceL {
        public IWorkbook ExcelWorkBook;
        public ISheet CurrentSheet;
        public int CurrentRow = 0;
        public int CurrentCol = 0;
        private IRow CurrentIRow;
        private ICell CurrentCell;
        private readonly short NormalHeight = 320;

        public ExceL() {
            ExcelWorkBook = new HSSFWorkbook();
            CurrentRow = 0;
        }
        public void CreateSheet(string SheetName) {
            CurrentSheet = ExcelWorkBook.CreateSheet(SheetName);
        }
        public void CreateRow() {
            CurrentIRow = CurrentSheet.CreateRow(CurrentRow);
            CurrentIRow.Height = NormalHeight;
        }
        public void CreateRow(int Cr) {
            CurrentRow = Cr;
            CurrentIRow = CurrentSheet.CreateRow(CurrentRow);
            CurrentIRow.Height = NormalHeight;
        }
        public void SetRow(int Cr) {
            CurrentRow = Cr;
            CurrentIRow = CurrentSheet.GetRow(CurrentRow);
        }
        public void CreateCell(CellType TypeO, FillPattern FillBackPat, short FillBackColor) {
            ICellStyle StyleO = ExcelWorkBook.CreateCellStyle();
            StyleO.FillForegroundColor = FillBackColor;
            StyleO.FillPattern = FillBackPat;
            CurrentCell = CurrentIRow.CreateCell(CurrentCol, TypeO);
            CurrentCell.CellStyle = StyleO;
        }
        public void CreateCell(CellType TypeO) {
            CurrentCell = CurrentIRow.CreateCell(CurrentCol);
        }
        public void CreateCell(int Cc, CellType TypeO) {
            CurrentCol = Cc;
            CurrentCell = CurrentIRow.CreateCell(CurrentCol);
        }
        public void SetCell(int Cc) {
            CurrentCol = Cc;
            CurrentCell = CurrentIRow.GetCell(CurrentCol);
        }
        public void SetCellValue(object Val) {
            switch (Val.GetType().Name) {
                case "String":
                    CurrentCell.SetCellValue(Convert.ToString(Val));
                    break;
                case "Double":
                    CurrentCell.SetCellValue(Convert.ToDouble(Val));
                    break;
                case "Boolean":
                    CurrentCell.SetCellValue(Convert.ToBoolean(Val));
                    break;
                case "DateTime":
                    CurrentCell.SetCellValue(Convert.ToDateTime(Val));
                    break;
                case "Int32":
                    CurrentCell.SetCellValue(Convert.ToInt32(Val));
                    break;
                default:
                    break;
            }
        }
    }
}

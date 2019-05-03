using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EdiViewer.Utility {
    public class ExceL {
        public IWorkbook ExcelWorkBook;
        private ISheet CurrentSheet;
        public int CurrentRow = 0;
        public int CurrentCol = 0;
        private IRow CurrentIRow;
        private ICell CurrentCell;
        private short NormalHeight = 320;

        public ExceL() {
            ExcelWorkBook = new XSSFWorkbook();
            CurrentRow = 0;
        }
        public void CreateSheet(string SheetName) {
            CurrentSheet = ExcelWorkBook.CreateSheet(SheetName);
        }
        public void CreateRow() {
            CurrentIRow = CurrentSheet.CreateRow(CurrentRow);
            CurrentIRow.Height = NormalHeight;
        }
        public void CreateCell(CellType TypeO, FillPattern FillBackPat, short FillBackColor) {
            ICellStyle StyleO = ExcelWorkBook.CreateCellStyle();            
            //StyleO.FillForegroundColor = FillBackColorO;
            StyleO.FillBackgroundColor = FillBackColor;
            StyleO.FillPattern = FillBackPat;
            CurrentCell = CurrentIRow.CreateCell(CurrentCol, TypeO);
            CurrentCell.CellStyle = StyleO;
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
                default:
                    break;
            }            
        }        
    }
}

﻿using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace MBS_Reborn.Excel
{
    public class WriteExcel
    {
        public static void writeExcel(string x)
        {
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
            excel.DisplayAlerts = false;
            excel.Visible = false;
            Workbook wb;
            Worksheet ws;
            wb = excel.Workbooks.Open(x);
            ws = (Worksheet)wb.Worksheets[1];
            Range cellRange = ws.Range["B1:ZZ1"];
            cellRange.set_Value(XlRangeValueDataType.xlRangeValueDefault, Main.names);
            wb.Close();
        }
    }
}
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using ExlInterop = Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using NUnit.Framework;
using System.Reflection;

namespace Nimble.Automation.Accelerators
{
    /// <summary>
    /// @Author - Pavan Parmar
    /// </summary>
    public static class ExcelOperations
    {
        public static string sheetName;
        
        public static void WriteExcelFile(string testDataFolderPath, string dataFile, string sheetname, int rowIndex, int columnIndex, string val)
        {
            DataTable dt = new DataTable();
            //tblExlData holds only data rows of excel sheet
            DataTable tblExcelData = new DataTable();
            string exlFileName = string.Empty;
            string exlDataSource = string.Empty;
            try
            {
                string fileName = dataFile + ".xlsx";
                string TestDataFolder = testDataFolderPath;
                string[] drTestdataFiles = Directory.GetFiles(TestDataFolder).
                    Where(x => x.EndsWith(".xls") || x.EndsWith(".xlsx")).ToArray();
                foreach (string tstDataFile in drTestdataFiles)
                {
                    exlFileName = tstDataFile.Substring(tstDataFile.LastIndexOf("\\") + 1);
                    if (fileName.Equals(exlFileName))
                    {
                        exlDataSource = Path.Combine(TestDataFolder, exlFileName);

                        break;
                    }
                }
                ExcelOperations.WriteDataToExcelUsingInterop(exlDataSource, sheetname, rowIndex, columnIndex, val);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable ReadExcelUsingOLEDB(string exlDataSource)
        {
            OleDbConnection ocon; OleDbDataAdapter oda;
            DataTable tblExcelSchema;
            string sheetName = string.Empty;
            //dt holds both the empty and data rows of excel sheet
            DataTable dt = new DataTable();

            try
            {
                string ExcelConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + exlDataSource + ";Extended Properties='Excel 12.0 Xml;HDR=Yes'";
                ocon = new OleDbConnection(ExcelConn);
                ocon.Open();
                tblExcelSchema = ocon.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                sheetName = tblExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                oda = new OleDbDataAdapter("select * from [" + sheetName + "]", ocon);
                dt.TableName = "TestData";
                oda.Fill(dt);
                ocon.Close();
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return dt;
        }

        public static DataTable ReadExcelUsingInterop(string exlDataSource, string sheetname)
        {
            ExlInterop.Application exlApp = new ExlInterop.Application();
            ExlInterop.Workbook exlWb = null;
            ExlInterop.Worksheet exlSheet;

            int rCnt = 0; int cCnt = 0;
            object cellValue = null;
            string colValue = string.Empty;

            //dt holds both the empty and data rows of excel sheet
            DataTable dt = new DataTable();

            try
            {
                exlWb = exlApp.Workbooks.Open(exlDataSource);

                int numSheets = exlWb.Sheets.Count;
                for (int sheetNum = 1; sheetNum < numSheets + 1; sheetNum++)
                {
                    exlSheet = (ExlInterop.Worksheet)exlWb.Sheets[sheetNum];
                    string strWorksheetName = exlSheet.Name;

                    exlWb.RefreshAll();
                    if (strWorksheetName.Equals(sheetname))
                    {
                        ExlInterop.Range range = exlSheet.UsedRange;
                        //                ExlInterop.Range range = exlSheet.UsedRange.SpecialCells(
                        //                               ExlInterop.XlCellType.xlCellTypeVisible,
                        //                               Type.Missing);
                        for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                        {
                            DataRow drow = dt.NewRow();
                            for (cCnt = 1; cCnt <= range.Columns.Count; cCnt++)
                            {
                                cellValue = (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                                colValue = cellValue == null ? string.Empty : Convert.ToString(cellValue);
                                //Adding Header Row to DataTable
                                if (rCnt == 1)
                                {
                                    dt.Columns.Add(colValue);
                                }
                                else
                                {
                                    drow[cCnt - 1] = colValue;
                                }
                            }
                            if (rCnt > 1)
                            {
                                dt.Rows.Add(drow);
                                dt.AcceptChanges();
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                exlWb.Close();
                exlApp.Quit();
                releaseProcessObject(exlWb);
                releaseProcessObject(exlApp);
            }
            return dt;
        }

        private static void releaseProcessObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        public static void WriteDataToExcelUsingInterop(string exlDataSource, string sheetName, int rowIndex, int columnIndex, string val)
        {

            ExlInterop.Application exlApp = new ExlInterop.Application();
            exlApp.Visible = true;
            ExlInterop.Workbook exlWb = null;
            ExlInterop.Worksheet exlSheet;
            try
            {
                exlWb = exlApp.Workbooks.Open(exlDataSource, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, false, false);
                exlSheet = (Microsoft.Office.Interop.Excel.Worksheet)exlWb.Worksheets[sheetName];
                exlSheet.Cells[rowIndex, columnIndex] = val;
                exlWb.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                exlWb.Close();
                exlApp.Quit();
                releaseProcessObject(exlWb);
                releaseProcessObject(exlApp);
            }
        }

        public static string GetCellValueFromExcel(string TestCaseId, string ColumnName)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ReadExcelFile("TestData", "TestData");

                //To get complete row from excel based on test case id
                var rowValue = (from myRow in dt.AsEnumerable()
                                where myRow.Field<string>(0) == TestCaseId
                                select myRow).Single();

                int indexval = dt.AsEnumerable().Select(row => row.Field<string>("TCId") == TestCaseId).ToList().FindIndex(col => col);
                string value = dt.Rows[indexval][ColumnName].ToString();

                return value;
            }
             catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                return "";
            }
        }
        public static string date = DateTime.Now.ToString("yyyyMMddHHmmss");
        public static Excel.Workbook wb;
        public static Excel.Worksheet sh;
        public static DataTable ReadExcelFile(string dataFile, string sheetname, bool isOLEDB = false)
        {
            DataTable tblExcelData = new DataTable();
            string exlFileName = string.Empty;
            try
            {
                string fileName = dataFile + ".xlsx";
                string testDataFolder = Directory.GetCurrentDirectory();// AppDomain.CurrentDomain.BaseDirectory.ToString()+"TestData" ;
                string[] drTestdataFiles = Directory.GetFiles(testDataFolder).
                    Where(x => x.EndsWith(".xls") || x.EndsWith(".xlsx")).ToArray();

                foreach (string tstDataFile in drTestdataFiles)
                {
                    exlFileName = tstDataFile.Substring(tstDataFile.LastIndexOf("\\") + 1);
                    if (fileName.Equals(exlFileName))
                    {
                        string exlDataSource = Path.Combine(testDataFolder, exlFileName);
                        if (isOLEDB)
                        {
                            tblExcelData = ReadExcelUsingOLEDB(exlDataSource);
                        }
                        else
                        {
                            tblExcelData = ReadExcelUsingInterop(exlDataSource, sheetname);
                        }
                        break;
                    }
                }
                tblExcelData.TableName = "TestData";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString() + "Stack Trace:" + ex.StackTrace.ToString());
                //throw ex;
            }
            return tblExcelData;
        }

        public static void sample() {

            
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Excel.Workbook wb = excel.Workbooks.Open("C:\\Users\\E004111\\Desktop\\NewFunnel_Testcases_Status.xlsx");
            Excel.Worksheet sh = wb.Sheets.Add();
            sh.Name = date;
            sh.Cells[1, "A"].Value2 = "DateANDTime";
            sh.Cells[1, "B"].Value2 = "Regression1";
            sh.Cells[1, "C"].Value2 = "Regression2";
            sh.Cells[1, "D"].Value2 = "Regression3";
            sh.Cells[1, "E"].Value2 = "Regression4";
            wb.Save();
            wb.Close(true);
            excel.Quit();   
        }
        public static void IterateRows()
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Excel.Workbook wb = excel.Workbooks.Open("C:\\Users\\E004111\\Desktop\\NewFunnel_Testcases_Status.xlsx");
            Excel.Worksheet worksheet = wb.ActiveSheet;
            //Get the used Range
            Excel.Range usedRange = worksheet.UsedRange;
            object misValue = Missing.Value;
            //Iterate the rows in the used range
            foreach (Excel.Range row in usedRange.Rows)
            {
                //Do something with the row.

                //Ex. Iterate through the row's data and put in a string array
                String[] rowData = new String[row.Columns.Count];
                for (int i = 2; i < row.Columns.Count; i++)
                {
                    int lastUsedRow = worksheet.Cells.SpecialCells(ExlInterop.XlCellType.xlCellTypeLastCell, misValue).Row;
                    rowData[i] = row.Cells[1, i + 1].Value2.ToString();

                        worksheet.Rows.Cells[i, "B"].Value2 = TestContext.CurrentContext.Test.Name;

                    break;
                }
            }
        }
        public static void AddRow()
        {
        //    int i = 4;

        //    Excel.Application excel = new Excel.Application();
        //    excel.Visible = true;
        //    Excel.Workbook wb = excel.Workbooks.Open("C:\\Users\\E004111\\Desktop\\NewFunnel_Testcases_Status.xlsx");
        //    Excel.Worksheet sh = wb.ActiveSheet;


        //   foreach (DataRow row in sh.Rows)
        //   foreach (DataColumn column in sh.Columns)
        //    {
                
        //            sh.Cells[2, "B"].Value2 = TestContext.CurrentContext.Test.Name;
        //        if (row[column] != null)
        //            Console.WriteLine(row[column]);

            }


        }
    }


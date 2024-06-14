using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Represents a csv file
    /// </summary>
    public class ExcelFile
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly FileInfo fileInfo;
        private readonly string delimiter;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="filePath">path to the excel file</param>
        public ExcelFile(string filePath):this(filePath, ",")
        {
            log.LogMethodEntry(filePath);
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="filePath">path to the excel file</param>
        /// <param name="delimiter">delimiter</param>
        public ExcelFile(string filePath, string delimiter)
        {
            log.LogMethodEntry(filePath);
            fileInfo = new FileInfo(filePath);
            this.delimiter = delimiter;
            log.LogMethodExit();
        }

        /// <summary>
        /// Reads the Sheet from the file
        /// </summary>
        /// <returns></returns>
        public Sheet Read(string dateTimeFormat = null)
        {
            /*log.LogMethodEntry();
            Sheet sheet = new Sheet();
            try
            {
                using (Stream s = fileInfo.OpenRead())
                {
                    TextFieldParser textFieldParser = new TextFieldParser(s);
                    textFieldParser.TextFieldType = FieldType.Delimited;
                    textFieldParser.SetDelimiters(delimiter);
                    int noOfHeaderColumns = 0;
                    while (textFieldParser.EndOfData == false)
                    {
                        Row row = new Row();
                        string[] contents = textFieldParser.ReadFields();
                        if(noOfHeaderColumns == 0)
                        {
                            noOfHeaderColumns = contents.Length;
                        }
                        for(int i = 0; i < contents.Length; i++)
                        {
                            if(contents[i].Contains("39897962412"))
                            {
                                contents[i] = "39897962412";
                            }
                            row.AddCell(new Cell(contents[i].Trim()));
                        }
                        if(noOfHeaderColumns - contents.Length > 0)
                        {
                            for (int i = 0; i < (noOfHeaderColumns - contents.Length); i++)
                            {
                                row.AddCell(new Cell(string.Empty));
                            }
                        }
                        sheet.AddRow(row);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while reading the excel file", ex);
                throw new Exception("Error occured while reading the excel file", ex);
            }
            log.LogMethodExit(sheet);
            return sheet;*/

            log.LogMethodEntry(dateTimeFormat);
            Sheet sheet = new Sheet();
            if (fileInfo.Exists == false)
            {
                log.LogMethodExit(null, "Throwing exception - Csv file not exists.");
                throw new Exception("Csv file not exists.");
            }
            
            MSExcel.Application application = new MSExcel.Application
            {
                Visible = false
            };

            try
            {
                MSExcel.Workbook workBook = application.Workbooks.Open(fileInfo.FullName);
                MSExcel.Worksheet workSheet = workBook.Sheets[1];
                workSheet.Columns.ClearFormats();
                workSheet.Rows.ClearFormats();
                object[,] range = (object[,])workSheet.UsedRange.Value2;
                int noOfRows = range.GetLength(0);
                int noOfColumns = range.GetLength(1);
                log.LogVariableState("Max Row no:", noOfRows);
                log.LogVariableState("Max Column no:", noOfColumns);
                for (int i = 1; i <= range.GetLength(0); i++)
                {
                    Row row = new Row();
                    for (int j = 1; j <= range.GetLength(1); j++)
                    {
                        Cell cell = null;
                        if (range[i, j] != null)
                        {
                            if (range[i, j] is DateTime)
                            {
                                if(string.IsNullOrWhiteSpace(dateTimeFormat) == false)
                                {
                                    cell = new Cell(((DateTime)range[i, j]).ToString(dateTimeFormat));
                                }
                                else
                                {
                                    cell = new Cell(((DateTime)range[i, j]).ToString());
                                }
                            }
                            else
                            {
                                cell = new Cell(range[i, j].ToString());
                            }
                        }
                        else
                        {
                            cell = new Cell(string.Empty);
                        }
                        row.AddCell(cell);
                    }
                    sheet.AddRow(row);
                }
                workBook.Close(false, fileInfo.Name, Missing.Value);
                application.DisplayAlerts = false;
            }
            catch(Exception ex)
            {
                log.Error("Error occured while reading the excel file", ex);
                throw new Exception("Error occured while reading the excel file", ex);
            }
            finally
            {
                application.Quit();
                KillExcel(application);
            }
            log.LogMethodExit(sheet);
            return sheet;
        }

        ///// <summary>
        ///// writes the sheet to the file
        ///// </summary>
        ///// <param name="sheet"></param>
        //public void Write(Sheet sheet)
        //{
        //    log.LogMethodEntry(sheet, delimiter);
        //    if (fileInfo.Exists)
        //    {
        //        fileInfo.Delete();
        //    }
        //    using (Stream stream = fileInfo.Create())
        //    {
        //        using (System.IO.StreamWriter streamWriter = new StreamWriter(stream))
        //        {
        //            WriteSheetToFile(sheet, streamWriter);
        //        }
        //    }
        //    log.LogMethodExit();
        //}

        //private void WriteSheetToFile(Sheet sheet, StreamWriter streamWriter)
        //{
        //    log.LogMethodEntry(sheet, streamWriter);
        //    for (int i = 0; i < sheet.Rows.Count; i++)
        //    {
        //        string joiner = string.Empty;
        //        for (int j = 0; j < sheet.Rows[i].Cells.Count; j++)
        //        {
        //            string value = sheet.Rows[i].Cells[j].Value;
        //            if (value.Contains(delimiter))
        //            {
        //                value = string.Format("\"{0}\"", value);
        //            }
        //            streamWriter.Write(joiner + value);
        //            joiner = delimiter;
        //        }
        //        streamWriter.WriteLine();
        //    }
        //    log.LogMethodExit();
        //}

        /// <summary>
        /// Saves the data to the excel using grid logic
        /// </summary>
        public void Write(Sheet sheet, string sheetName, string fileName)
        {
            log.LogMethodEntry();
            DataTable dataTable = new DataTable();
            foreach(Cell c in sheet.RowList[0].CellList)
            {
                dataTable.Columns.Add(c.Value);
            }
            if (sheet.RowList.Count > 1)
            {
                for (int i = 1; i < sheet.RowList.Count; i++)
                {
                    for (int j = 0; j < sheet.RowList[i].Cells.Count; j++)
                    {
                        dataTable.Rows.Add();
                        dataTable.Rows[i-1][j] = sheet.RowList[i].CellList[j].Value;
                    }
                }
            }
            //dataTable.Rows.Add(sheet.RowList);            

            using (Form frm = new Form())
            {
                DataGridView dgv = new DataGridView();
                frm.Controls.Add(dgv);
                dgv.Visible = true;
                dgv.DataSource = dataTable;

                dgv.RowHeadersVisible = false;
                dgv.SelectAll();
                DataObject dataObj = dgv.GetClipboardContent();
                if (dataObj != null)
                {
                    Clipboard.SetDataObject(dataObj);
                }

                MSExcel.Application objApp;
                objApp = new MSExcel.Application();
                if (objApp == null)
                {
                    log.LogMethodExit("objApp == null");
                    return;
                }

                MSExcel._Workbook objBook;

                MSExcel.Workbooks objBooks;
                MSExcel.Sheets objSheets;
                MSExcel._Worksheet objSheet;
                MSExcel.Range range;

                try
                {
                    // Instantiate Excel and start a new workbook.
                    objApp = new MSExcel.Application();                    
                    objBooks = objApp.Workbooks;
                    objBook = objBooks.Add(Missing.Value);
                    objBook.Title = fileName;
                    objSheets = objBook.Worksheets;
                    objSheet = (MSExcel._Worksheet)objSheets.get_Item(1);
                    objSheet.Name = sheetName;

                    //Get the range where the starting cell has the address
                    //m_sStartingCell and its dimensions are m_iNumRows x m_iNumCols.
                    range = objSheet.get_Range("A1", Missing.Value);

                    range = range.get_Resize(1, sheet.RowList[0].CellList.Count);

                    
                    string[,] raSet = new string[1, sheet.RowList[0].CellList.Count];
                    for (int iCol = 0; iCol < dgv.Columns.Count; iCol++)
                    {
                        //Put the row and column address in the cell.
                        raSet[0, iCol] = dgv.Columns[iCol].HeaderText;
                    }
                    //Set the range value to the array.
                    range.set_Value(Missing.Value, raSet);
                    range.Font.Bold = true;
                    range.Columns.AutoFit();
                    MSExcel.Range CR = (MSExcel.Range)objSheet.Cells[2, 1];
                    CR.Select();
                    objSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);

                    var firstCell = objSheet.get_Range("A1", "A1");
                    firstCell.Select();
                    //Return control of Excel to the user.
                    objApp.DisplayAlerts = false;
                    objApp.ActiveWorkbook.SaveAs(fileName);
                    objApp.Visible = true;
                    objApp.UserControl = true;                    
                    Clipboard.Clear();
                }
                catch (Exception theException)
                {
                    String errorMessage = "";                   
                    errorMessage = String.Concat(errorMessage, theException.Message);
                    errorMessage = String.Concat(errorMessage, " Line: ");
                    errorMessage = String.Concat(errorMessage, theException.Source);
                                       
                    log.Error(theException);
                    throw theException;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="ProcessId"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int ProcessId);
        private static void KillExcel(MSExcel.Application theApp)
        {
            log.LogMethodEntry();
            int id = 0;
            IntPtr intptr = new IntPtr(theApp.Hwnd);
            System.Diagnostics.Process p = null;
            try
            {
                GetWindowThreadProcessId(intptr, out id);
                p = System.Diagnostics.Process.GetProcessById(id);
                if (p != null)
                {
                    p.Kill();
                    p.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("KillExcel:" + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}

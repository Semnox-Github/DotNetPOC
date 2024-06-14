using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Represents a csv file
    /// </summary>
    public class CsvFile
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly FileInfo fileInfo;
        private readonly string delimiter;
        private readonly Encoding encoding;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="filePath">path to the excel file</param>
        public CsvFile(string filePath):this(filePath, ",", Encoding.Default)
        {
            log.LogMethodEntry(filePath);
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="filePath">path to the excel file</param>
        /// <param name="delimiter">delimiter</param>
        /// <param name="encoding"></param>
        public CsvFile(string filePath, string delimiter, Encoding encoding)
        {
            log.LogMethodEntry(filePath);
            fileInfo = new FileInfo(filePath);
            this.delimiter = delimiter;
            this.encoding = encoding;
            log.LogMethodExit();
        }

        /// <summary>
        /// Reads the Sheet from the file
        /// </summary>
        /// <returns></returns>
        public Sheet Read(string dateTimeFormat = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            try
            {
                using (StreamReader s = new StreamReader(fileInfo.FullName, encoding, true))
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
                            row.AddCell(new Cell(contents[i].Replace("\u00a0", string.Empty).Trim()));
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
            return sheet;

            /*log.LogMethodEntry(dateTimeFormat);
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
                for (int i = 1; i <= noOfRows; i++)
                {
                    Row row = new Row();
                    for (int j = 1; j <= noOfColumns; j++)
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
                                cell = new Cell(range[i, j].ToString().Trim());
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
            return sheet;*/
        }

        private string RemoveControlCharacters(string input)
        {
            return new string(input.Where(c => !char.IsControl(c)).ToArray());
        }

        /// <summary>
        /// writes the sheet to the file
        /// </summary>
        /// <param name="sheet"></param>
        public void Write(Sheet sheet)
        {
            log.LogMethodEntry(sheet, delimiter);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            using (Stream stream = fileInfo.Create())
            {
                using (System.IO.StreamWriter streamWriter = new StreamWriter(stream))
                {
                    WriteSheetToFile(sheet, streamWriter);
                }
            }
            log.LogMethodExit();
        }

        private void WriteSheetToFile(Sheet sheet, StreamWriter streamWriter)
        {
            log.LogMethodEntry(sheet, streamWriter);
            for (int i = 0; i < sheet.Rows.Count; i++)
            {
                string joiner = string.Empty;
                for (int j = 0; j < sheet.Rows[i].Cells.Count; j++)
                {
                    string value = sheet.Rows[i].Cells[j].Value;
                    if (value.Contains(delimiter))
                    {
                        value = string.Format("\"{0}\"", value);
                    }
                    streamWriter.Write(joiner + value);
                    joiner = delimiter;
                }
                streamWriter.WriteLine();
            }
            log.LogMethodExit();
        }

        /*/// <summary>
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
        }*/
    }
}

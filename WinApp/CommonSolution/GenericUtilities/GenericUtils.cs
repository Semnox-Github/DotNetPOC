//Copyright 2020 Semnox

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//http://www.apache.org/licenses/LICENSE-2.0
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
//Included modified Apache NPOI, NPOI.OOXML, NPOI.OpenXml4Net, NPOI.OpenXmlFormats dll's to read/write Excel and Word files 
/******************************************************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - GenericUtils.cs 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *******************************************************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha           Added logger methods.
 *2.70.2        15-Oct-2019            Guru S A          Waiver phase 2 changes
 *2.90           3-Aug-2020            Laster menezes    Included MergeFiles, TotalPDFPageCount, GetFileCountByExtention methods
 *2.140.0       14-Sep-2021            Guru S A          Waiver mapping UI enhancements
 *              25-Nov-2021            Mathew Ninan      Added method to convert Hex string to Base64 format string 
 ******************************************************************************************************************************/
using GenCode128;
using iTextSharp.text.pdf;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using QRCoder;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    public class GenericUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GetColorCode method
        /// </summary>
        /// <param name="colorString">String</param>
        /// <returns>returns color code  in RGB Format</returns>
        /// 
        public static string GetColorCode(string colorString)
        {
            log.LogMethodEntry(colorString);
            if (colorString.CompareTo("") == 0)
            {
                log.LogMethodExit();
                return "";
            }
            try
            {
                System.Drawing.Color convertedColor;
                if (colorString.Contains(","))
                {
                    log.LogMethodExit(colorString);
                    return colorString;
                }
                else
                {
                    convertedColor = System.Drawing.ColorTranslator.FromHtml(colorString);
                    string returnValue = convertedColor.R + "," + convertedColor.G + "," + convertedColor.B;
                    log.LogMethodExit(returnValue);
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
                return "";
            }

        }

        public static bool WithInHoursRange(double rangeStartHour, double rangeEndHour)
        {
            log.LogMethodEntry(rangeStartHour, rangeEndHour);
            Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();
            try
            {
                DateTime currentDay = DateTime.Today;
                DateTime dateValueToCheck = DateTime.Now;
                DateTime startTime = currentDay.AddHours(rangeStartHour);
                DateTime endTime = currentDay.AddHours(rangeEndHour);
                if (rangeEndHour < rangeStartHour && rangeStartHour < 12)
                {
                    throw new Exception(utilities.MessageUtils.getMessage(571));
                }

                if (rangeEndHour < rangeStartHour)
                {
                    endTime = DateTime.Today.AddDays(1).AddHours(rangeEndHour);
                }
                else
                {
                    endTime = DateTime.Today.AddHours(Convert.ToDouble(rangeEndHour));
                }
                if (endTime.CompareTo(startTime) <= 0)
                {
                    throw new Exception(utilities.MessageUtils.getMessage(571));
                }

                if (startTime.CompareTo(dateValueToCheck) > 0 || endTime.CompareTo(dateValueToCheck) < 0)
                {
                    log.LogMethodExit(false);
                    return false;
                }
                else
                {
                    log.LogMethodExit(true);
                    return true;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(utilities.MessageUtils.getMessage(1245) + ex.Message);
            }
        }

        public static double RoundOffFunction(double Amount, double RoundOffAmountTo, int RoundingPrecision, string RoundingType)
        {
            log.LogMethodEntry(Amount, RoundOffAmountTo, RoundingPrecision, RoundingType);
            double returnValue;
            switch (RoundingType)
            {
                case "ROUND":
                    {
                        returnValue = Math.Round(Amount / RoundOffAmountTo, RoundingPrecision, MidpointRounding.AwayFromZero) * RoundOffAmountTo;
                        log.LogMethodExit(returnValue);
                        return returnValue;
                    }
                case "FLOOR":
                    {
                        returnValue = ((int)(Amount * Math.Pow(10, RoundingPrecision) / RoundOffAmountTo) * RoundOffAmountTo) / Math.Pow(10, RoundingPrecision);
                        log.LogMethodExit(returnValue);
                        return returnValue;
                    }
                case "CEILING":
                    {
                        double lclAmount = Amount * Math.Pow(10, RoundingPrecision);
                        if (lclAmount % RoundOffAmountTo == 0)
                        {
                            log.LogMethodExit(Amount);
                            return Amount;
                        }
                        else
                        {
                            returnValue = (lclAmount + RoundOffAmountTo - (lclAmount % RoundOffAmountTo)) / Math.Pow(10, RoundingPrecision);
                            log.LogMethodExit(returnValue);
                            return returnValue;
                        }
                    }
                default:
                    log.LogMethodExit(Amount);
                    return Amount;
            }
        }

        /// <summary>
        /// Gets the file from the server folder
        /// </summary>
        /// <param name="Utilities"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public byte[] GetFileFromServer(Semnox.Core.Utilities.Utilities Utilities, string filePath)
        {
            log.LogMethodEntry(Utilities, filePath);
            byte[] bytes = new byte[0];
            try
            {
                using (SqlCommand cmdImage = Utilities.getCommand())
                {
                    cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";
                    cmdImage.Parameters.AddWithValue("@FileName", filePath);
                    try
                    {
                        object obj = cmdImage.ExecuteScalar();
                        if (obj != null)
                        {
                            bytes = obj as byte[];
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error getting the file from server" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit(bytes);
            return bytes;
        }

        /// <summary>
        /// Gets the file from the server folder
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public byte[] GetFileFromServer(ExecutionContext executionContext, string filePath)
        {
            log.LogMethodEntry(filePath);
            byte[] bytes = new byte[0];
            ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction();
            try
            {
                dBTransaction.BeginTransaction();
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                object obj = dataAccessHandler.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                             new SqlParameter[] { new SqlParameter("@FileName", filePath) },
                                                             dBTransaction.SQLTrx);

                if (obj != null)
                {
                    bytes = obj as byte[];
                }
                dBTransaction.EndTransaction();
                dBTransaction.Dispose();
            }
            catch (Exception ex)
            {
                if (dBTransaction != null)
                {
                    dBTransaction.Dispose();
                }
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(bytes);
            return bytes;
        }
        public void WriteFileToServer(byte[] outputBytes, string OutputFile, Semnox.Core.Utilities.Utilities Utilities = null)
        {
            log.LogMethodEntry(outputBytes, OutputFile, Utilities);
            if (Utilities == null)
                Utilities = new Semnox.Core.Utilities.Utilities();
            try
            {
                using (SqlCommand commandPDF = Utilities.getCommand())
                {
                    commandPDF.CommandText = "exec SaveBinaryDataToFile @Image, @FileName";
                    commandPDF.Parameters.AddWithValue("@Image", outputBytes);
                    commandPDF.Parameters.AddWithValue("@FileName", OutputFile);
                    commandPDF.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit("Throwing Exception" + ex.Message);
                throw new Exception(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get list of date between given start and end date
        /// </summary>
        /// <param name="startDate">Start Date</param>
        /// <param name="endDate">End Date</param>
        /// <param name="betweenDate">Between Date</param>
        /// <returns>List of DateTime</returns>
        public static List<DateTime> GetBetweenDates(DateTime startDate, DateTime endDate, DateTime betweenDate)
        {
            log.LogMethodEntry(startDate, endDate, betweenDate);
            List<DateTime> betweenDates = new List<DateTime>();
            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (date.Day == betweenDate.Day && date.Month == betweenDate.Month)
                {
                    betweenDates.Add(new DateTime(date.Year, date.Month, date.Day));
                    date = date.AddYears(1).AddDays(-1);
                }
            }
            log.LogMethodExit(betweenDates);
            return betweenDates;
        }
        /// <summary>
        /// Download File From site db
        /// </summary>
        /// <param name="file"></param>
        /// <returns> downloaded file name with payth</returns>
        public string DownloadFileFromDB(string file, Utilities.Utilities utilities)
        {
            if (!(File.Exists(Path.GetTempPath() + file)))
            {
                GenericUtils genericUtils = new GenericUtils();
                try
                {
                    string fileNameWithPath = Utilities.ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "IMAGE_DIRECTORY") + "\\" + file;
                    byte[] bytes = GetFileFromServer(utilities, fileNameWithPath);
                    File.WriteAllBytes(Path.GetTempPath() + file, bytes);
                    if (!(File.Exists(Path.GetTempPath() + file)))
                    {
                        throw new Exception(utilities.MessageUtils.getMessage(1508));
                    }
                    bytes = null;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new Exception(ex.Message);
                }
            }

            string filename = Path.GetTempPath() + file;
            return filename;
        }


        /// <summary>
        ///  Download File From site db
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="file"></param>
        /// <returns>downloaded file name with path</returns>
        public string DownloadFileFromDB(ExecutionContext executionContext, string file)
        {
            log.LogMethodEntry(executionContext, file);
            if (!(File.Exists(Path.GetTempPath() + file)))
            {
                GenericUtils genericUtils = new GenericUtils();
                try
                {
                    string fileNameWithPath = Utilities.ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + file;
                    byte[] bytes = GetFileFromServer(executionContext, fileNameWithPath);
                    File.WriteAllBytes(Path.GetTempPath() + file, bytes);
                    if (!(File.Exists(Path.GetTempPath() + file)))
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 1508));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new Exception(ex.Message);
                }
            }

            string filename = Path.GetTempPath() + file;
            return filename;
        }
        /// <summary>
        /// Download File From site db
        /// </summary>
        /// <param name="file"></param>
        /// <returns> downloaded file name with payth</returns>
        public string DownloadFileFromDBInBase64Format(ExecutionContext executionContext, string file)
        {
            log.LogMethodEntry(file);
            string base64Content = string.Empty;
            try
            {
                string fileNameWithPath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + file;
                byte[] bytes = GetFileFromServer(executionContext, fileNameWithPath);
                if (bytes != null)
                {
                    base64Content = Convert.ToBase64String(bytes);
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2405));//'Error getting the file from server'
                }
            }
            catch (Exception ex)
            {
                string message = MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 2435);
                message = message + ". ";
                if ((file.IndexOfAny(Path.GetInvalidFileNameChars()) < 0) == false)
                {
                    message = message + MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 2435); //Waiver file name contains invalid characters.
                }
                message = message + ex.Message;
                log.Error(message);
                throw new Exception(message);
            }
            log.LogMethodExit();
            return base64Content;
        }

        /// <summary>
        /// Download File From site db
        /// </summary>
        /// <param name="file"></param>
        /// <returns> downloaded file name with payth</returns>
        public byte[] DownloadFileFromDBAsByteArray(ExecutionContext executionContext, string file)
        {
            log.LogMethodEntry(file);
            byte[] content = null;
            try
            {
                string fileNameWithPath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + file;
                content = GetFileFromServer(executionContext, fileNameWithPath);
                if (content != null)
                {
                    return content;
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2405));//'Error getting the file from server'
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// MergeFiles
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="files"></param>
        /// <param name="destFileName"></param>
        /// <param name="fileDirectory"></param>
        /// <param name="FileExtention"></param>
        /// <returns></returns>
        public bool MergeFiles(ExecutionContext executionContext, FileInfo[] files, string destFileName, string fileDirectory, string FileExtention)
        {
            log.LogMethodEntry(files, destFileName, fileDirectory, FileExtention);
            try
            {
                int filesCount = GetFileCountByExtention(files, FileExtention);
                if (filesCount > 0)
                {
                    if (FileExtention.ToUpper() == ".PDF")
                    {
                        PdfReader reader = null;
                        iTextSharp.text.Document sourceDocument = null;
                        PdfCopy pdfCopyProvider = null;
                        PdfImportedPage importedPage;
                        string outputPdfPath = fileDirectory + destFileName + ".PDF";

                        sourceDocument = new iTextSharp.text.Document();
                        pdfCopyProvider = new PdfCopy(sourceDocument, new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

                        List<string> fileList = new List<string>();
                        foreach (FileInfo fileInfo in files)
                        {
                            if (fileInfo.Extension.ToUpper() == ".PDF")
                            {
                                fileList.Add(fileInfo.FullName);
                            }
                        }
                        sourceDocument.Open();
                        if (fileList.Count() > 0)
                        {
                            foreach (string fileName in fileList)
                            {
                                int pages = TotalPDFPageCount(fileName);
                                reader = new PdfReader(fileName);
                                //Add pages in new file  
                                for (int i = 1; i <= pages; i++)
                                {
                                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                                    pdfCopyProvider.AddPage(importedPage);
                                }
                                reader.Close();
                            }
                        }
                        sourceDocument.Close();
                    }

                    if (FileExtention.ToUpper() == ".XLSX")
                    {
                        string outputWorkbookPath = fileDirectory + destFileName + ".XLSX";
                        List<string> fileList = new List<string>();
                        foreach (FileInfo fileInfo in files)
                        {
                            if (fileInfo.Extension.ToUpper() == ".XLSX")
                            {
                                fileList.Add(fileInfo.FullName);
                            }
                        }

                        if (fileList.Count() > 0)
                        {
                            XSSFWorkbook destinationWorkbook = new XSSFWorkbook();
                            foreach (string fileName in fileList)
                            {
                                XSSFWorkbook workbookSource = new XSSFWorkbook(new FileStream(fileName, FileMode.Open));
                                for (int i = 0; i < workbookSource.NumberOfSheets; i++)
                                {
                                    ISheet sheet = workbookSource.GetSheetAt(i);
                                    sheet.CopyTo(destinationWorkbook, sheet.SheetName, true, true);
                                }
                            }
                            destinationWorkbook.Write(new FileStream(outputWorkbookPath, FileMode.Create, FileAccess.ReadWrite));
                        }
                    }

                    if (FileExtention.ToUpper() == ".CSV")
                    {
                        string outputCSVFilePath = fileDirectory + destFileName + ".CSV";
                        List<string> fileList = new List<string>();
                        foreach (FileInfo fileInfo in files)
                        {
                            if (fileInfo.Extension.ToUpper() == ".CSV")
                            {
                                fileList.Add(fileInfo.FullName);
                            }
                        }
                        StreamWriter fileDest = new StreamWriter(outputCSVFilePath, true);

                        foreach (string fileName in fileList)
                        {
                            string[] lines = File.ReadAllLines(fileName);
                            foreach (string line in lines)
                            {
                                fileDest.WriteLine(line);
                            }
                        }
                        fileDest.Close();
                    }
                }
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(false);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2736, FileExtention));//'Error while merging the &1 files'
            }
        }

        /// <summary>
        /// TotalPDFPageCount
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public int TotalPDFPageCount(string file)
        {
            log.LogMethodEntry(file);
            PdfReader pdfReader = new PdfReader(file);
            int numberOfPages = pdfReader.NumberOfPages;
            log.LogMethodExit(numberOfPages);
            return numberOfPages;
        }

        /// <summary>
        /// GetFileCountByExtention
        /// </summary>
        /// <param name="files"></param>
        /// <param name="fileExtention"></param>
        /// <returns></returns>
        public int GetFileCountByExtention(FileInfo[] files, string fileExtention)
        {
            log.LogMethodEntry(files, fileExtention);
            int filesCount = 0;
            try
            {
                if (files != null)
                {
                    if (!string.IsNullOrEmpty(fileExtention))
                    {
                        filesCount = 0;
                        foreach (FileInfo fileInfo in files)
                        {
                            if (fileInfo.Extension.ToUpper() == fileExtention)
                            {
                                filesCount++;
                            }
                        }
                        return filesCount;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(filesCount);
            return filesCount;
        }
        /// <summary>
        /// ConvertBase64StringToImage
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static System.Drawing.Image ConvertBase64StringToImage(string base64String)
        {
            log.LogMethodEntry();
            System.Drawing.Image result = null;
            if (string.IsNullOrWhiteSpace(base64String) == false)
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(base64String);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        result = System.Drawing.Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while converting base 64 string to image", ex);
                }
            }
            log.LogMethodExit();
            return result;
        }
        /// <summary>
        /// ConvertStringToInt
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static int ConvertStringToInt(ExecutionContext executionContext, string inputValue)
        {
            log.LogMethodEntry(inputValue);
            int result = 0;
            if (string.IsNullOrWhiteSpace(inputValue) || (Int32.TryParse(inputValue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result) == false))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 648));//Please enter a valid number
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GenerateQRCodeB64ForString
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static string GenerateQRCodeB64ForString(string stringValue)
        {
            log.LogMethodEntry();
            string qrCodeB64 = string.Empty;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(stringValue, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            if (qrCode != null)
            {
                Image image = qrCode.GetGraphic(1);
                if (image != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        qrCodeB64 = System.Convert.ToBase64String(stream.ToArray());
                    }
                }
            }
            log.LogMethodExit(qrCodeB64);
            return qrCodeB64;
        }

        /// <summary>
        /// GenerateBarCodeB64ForString
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="barcodeWeight"></param>
        /// <returns></returns>
        public static string GenerateBarCodeB64ForString(string stringValue, int barcodeWeight=1)
        {
            log.LogMethodEntry(stringValue, barcodeWeight);
            string barCodeB64 = string.Empty;
            Image image = Code128Rendering.MakeBarcodeImage(stringValue, barcodeWeight, true);
            if (image != null)
            {
                using (var stream = new MemoryStream())
                {
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    barCodeB64 = System.Convert.ToBase64String(stream.ToArray());
                }
            }
            log.LogMethodExit(barCodeB64);
            return barCodeB64;
        }

	/// <summary>
        /// Convert Image to byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public byte[] ConvertToByteArray(System.Drawing.Image image)
        {
            log.LogMethodEntry();
            log.LogVariableState("image", image);

            if (image == null)
            {
                log.LogMethodExit("image == null");
                return null;
            }
            else
            {
                ImageConverter converter = new ImageConverter();
                Byte[] imageByteArray = (byte[])converter.ConvertTo(image, typeof(byte[]));
                log.LogVariableState("imageByteArray", imageByteArray);
                log.LogMethodExit();
                return imageByteArray;
            }
        }

        /// <summary>
        /// Convert Hex value string to Base64 string
        /// </summary>
        /// <param name="inputHex">Hex value string</param>
        /// <returns>Base 64 string</returns>
        public static string HexStringToBytearray(string inputHex)
        {
            log.LogMethodEntry(inputHex);
            var resultantArray = new byte[inputHex.Length / 2];
            for (var i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = System.Convert.ToByte(inputHex.Substring(i * 2, 2), 16);
            }
            string base64String = Convert.ToBase64String(resultantArray);
            log.LogMethodExit(base64String);
            return base64String;
        }
    }
}

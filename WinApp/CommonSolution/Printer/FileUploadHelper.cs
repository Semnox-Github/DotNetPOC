/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - the FileUploadHelper class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         02-Apr-2020   Girish Kundar        Modified: Receipt template import - read from file instead from path
 *                                                          Issue fix WMS - import template 
 *2.140.0      23-Nov-2021   Abhishek             Modified: added logic to upload and download maintenance requests file  
 *2.150.0      04-Nov-2022   Abhishek             Modified: Upload DPL file
 ********************************************************************************************/

using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Semnox.Parafait.Printer
{
    public class FileUploadHelper
    {
        public enum FileType
        {
            BLANKACTIVITY,//Place holder 
            PROMOFILELIST,
            PROMOFILE,
            PROMOFILE_ALL_B64,
			MAINTENANCEREQUESTS
        }

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Core.Utilities.Utilities utils = new Semnox.Core.Utilities.Utilities();
        HttpPostedFile httpPostedFile;
        private ExecutionContext executionContext;
        string sharedFolderPath = string.Empty;
        string entityName = string.Empty;

        public FileUploadHelper(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public void FileUpload()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                string entityName = httpRequest.Form["EntityName"].ToString();
                string entityId = httpRequest.Form["EntityId"].ToString();
                string filePath = string.Empty;
                foreach (string file in httpRequest.Files)
                {
                    httpPostedFile = httpRequest.Files[file];
                    if (entityName.ToUpper().ToString() == "WAIVERSETDETAILS" || entityName.ToUpper().ToString() == "CUSTOMERDETAILS"
					|| entityName.ToUpper().ToString() == "MAINTENANCEREQUESTS")
                    {
                        sharedFolderPath = utils.getParafaitDefaults("IMAGE_DIRECTORY");
                        filePath = sharedFolderPath + '\\' + httpPostedFile.FileName;
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            httpPostedFile.SaveAs(filePath);
                        }
                        else
                        {
                            httpPostedFile.SaveAs(filePath);
                        }
                    }
                    else if (entityName.ToUpper().ToString() == "CONTENTMEDIA")
                    {
                        sharedFolderPath = utils.getParafaitDefaults("UPLOAD_DIRECTORY");
                        filePath = sharedFolderPath + '\\' + httpPostedFile.FileName;
                        if (File.Exists(filePath))
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1812));
                        }
                        else
                        {
                            httpPostedFile.SaveAs(filePath);
                        }
                    }
                    else if (entityName.ToUpper().ToString() == "RECEIPTTEMPLATE")
                    {
                        string content = string.Empty;
                        if (httpPostedFile.FileName.Contains(".txt | *.txt") || httpPostedFile.FileName.EndsWith(".txt"))
                        {
                            using (StreamReader reader = new StreamReader(httpPostedFile.InputStream))
                            {
                                content = reader.ReadToEnd();
                            }
                            ReceiptPrintTemplateHeaderBL receiptPrintTemplateHeaderBL = new ReceiptPrintTemplateHeaderBL(executionContext);
                            receiptPrintTemplateHeaderBL.ExecuteImportQuery(content);
                        }
                    }
                    else if (entityName.ToUpper().ToString() == "DPLFILE")
                    {
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
                        searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DPL_FILE_FOLDER_PATH"));
                        searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "DPL_FILE_FOLDER"));
                        List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
                        if (lookupValuesListDTO != null && lookupValuesListDTO.Count > 0)
                        {
                            sharedFolderPath = lookupValuesListDTO[0].Description;
                        }
                        else
                        {
                            string errorMessage = MessageContainerList.GetMessage(executionContext, 1411);
                            throw new ValidationException(errorMessage);
                        }
                        filePath = sharedFolderPath + '\\' + httpPostedFile.FileName;
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            httpPostedFile.SaveAs(filePath);
                        }
                        else
                        {
                            httpPostedFile.SaveAs(filePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        public List<String> DownloadImagesAsBase64Strings(FileUploadHelper.FileType fileType, string fileName = null)
        {
            List<String> imageList = new List<string>();
            try
            {
                switch (fileType)
                {
                    case FileUploadHelper.FileType.PROMOFILELIST:
                        {
                            sharedFolderPath = utils.getParafaitDefaults("IMAGE_DIRECTORY");
                            sharedFolderPath = sharedFolderPath + '\\' + "AppPromotions";
                            try
                            {
                                foreach (string filePath in Directory.GetFiles(sharedFolderPath, "*.*"))
                                {
                                    if (filePath.Contains(".bmp") || filePath.Contains(".jpeg") || filePath.Contains(".png"))
                                    {
                                        imageList.Add(Path.GetFileName(filePath));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Debug(ex.Message);
                                throw;
                            }
                        }
                        break;
                    case FileUploadHelper.FileType.PROMOFILE:
                        {
                            byte[] buffer;
                            sharedFolderPath = utils.getParafaitDefaults("IMAGE_DIRECTORY");
                            String fullFileName = sharedFolderPath + '\\' + "AppPromotions" + '\\' + JsonConvert.DeserializeObject(fileName);
                            using (FileStream fileStream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read))
                            {
                                try
                                {
                                    int length = (int)fileStream.Length;  // get file length
                                    buffer = new byte[length];            // create buffer
                                    int count;                            // actual number of bytes read
                                    int sum = 0;                          // total number of bytes read

                                    // read until Read method returns 0 (end of the stream has been reached)
                                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                                        sum += count;  // sum is a buffer offset for next reading
                                }
                                finally
                                {
                                    fileStream.Close();
                                }
                            }

                            String base64 = Convert.ToBase64String(buffer);
                            imageList.Add(base64);
                        }
                        break;
                    case FileUploadHelper.FileType.PROMOFILE_ALL_B64:
                        {
                            byte[] buffer;
                            sharedFolderPath = utils.getParafaitDefaults("IMAGE_DIRECTORY");
                            sharedFolderPath = sharedFolderPath + '\\' + "AppPromotions";
                            try
                            {
                                foreach (string filePath in Directory.GetFiles(sharedFolderPath, "*.*"))
                                {
                                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                                    {
                                        try
                                        {
                                            int length = (int)fileStream.Length;  // get file length
                                            buffer = new byte[length];            // create buffer
                                            int count;                            // actual number of bytes read
                                            int sum = 0;                          // total number of bytes read

                                            // read until Read method returns 0 (end of the stream has been reached)
                                            while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                                                sum += count;  // sum is a buffer offset for next reading
                                        }
                                        finally
                                        {
                                            fileStream.Close();
                                        }
                                    }

                                    String base64 = Convert.ToBase64String(buffer);
                                    imageList.Add(base64);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Debug(ex.Message);
                                throw;
                            }
                        }
                        break;
						   case FileUploadHelper.FileType.MAINTENANCEREQUESTS:
                        {
                            byte[] buffer;
                            sharedFolderPath = utils.getParafaitDefaults("IMAGE_DIRECTORY");
                            String fullFileName = sharedFolderPath + '\\' + fileName;
                            using (FileStream fileStream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read))
                            {
                                try
                                {
                                    int length = (int)fileStream.Length;  // get file length
                                    buffer = new byte[length];            // create buffer
                                    int count;                            // actual number of bytes read
                                    int sum = 0;                          // total number of bytes read

                                    // read until Read method returns 0 (end of the stream has been reached)
                                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                                        sum += count;  // sum is a buffer offset for next reading
                                }
                                finally
                                {
                                    fileStream.Close();
                                }
                            }
                            String base64 = Convert.ToBase64String(buffer);
                            imageList.Add(base64);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }

            return imageList;
        }
    }
}

//using (Stream source = File.OpenRead(filePath))
//{
//    byte[] buffer = new byte[2048];
//    int bytesRead;
//    while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
//    {
//        //dest.Write(buffer, 0, bytesRead);
//    }
//}

//string contentType = httpPostedFile.ContentType;
//Stream fileStream = httpPostedFile.InputStream;
//fileData = new byte[fileStream.Length];
//int iBytesRead = fileStream.Read(fileData, 0, (int)fileData.Length);

//using (Stream fs = httpPostedFile.InputStream)
//{
//    using (BinaryReader br = new BinaryReader(fs))
//    {
//        byte[] bytes = br.ReadBytes((Int32)fs.Length);
//    }
//}

//using (var binaryReader = new BinaryReader(httpRequest.Files[0].InputStream))
//{
//    fileData = binaryReader.ReadBytes(httpRequest.ContentLength);
//}


//int filelength = httpPostedFile.ContentLength;
//byte[] imagebytes = new byte[filelength];
//httpPostedFile.InputStream.Read(imagebytes, 0, filelength);

//Stream fs1 = httpPostedFile.InputStream;
//BinaryReader br = new BinaryReader(fs1);
//byte[] bytes = br.ReadBytes((Int32)fs1.Length);

//MemoryStream ms = new MemoryStream();
//httpRequest.Files[0].InputStream.CopyTo(ms);
//byte[] fileContent = ms.ToArray();
//kioskSetupDTOs.Image = fileContent;



//if (formData != null)
//{
//KioskSetupDTO kioskSetupDTOs = JsonConvert.DeserializeObject<KioskSetupDTO>(formData);
//if (kioskSetupDTOs != null)
//{
//    byte[] fileData = null;

//    int filelength = httpPostedFile.ContentLength;
//    byte[] imagebytes = new byte[filelength];
//    httpPostedFile.InputStream.Read(imagebytes, 0, filelength);

//    Stream fs = httpPostedFile.InputStream;
//    BinaryReader br = new BinaryReader(fs);
//    byte[] bytes = br.ReadBytes((Int32)fs.Length);

//    using (var binaryReader = new BinaryReader(httpRequest.Files[0].InputStream))
//    {
//        fileData = binaryReader.ReadBytes(httpRequest.ContentLength);
//    }

//    MemoryStream ms = new MemoryStream();
//    httpRequest.Files[0].InputStream.CopyTo(ms);
//    byte[] fileContent = ms.ToArray();
//    kioskSetupDTOs.Image = fileContent;
//    KioskSetupList kioskSetupList = new KioskSetupList(kioskSetupDTOs, executionContext);
//    kioskSetupList.SaveUpdateKioskSetupsList();
//}
//}

//if (httpRequest.Headers["EntityName"] != null)
//{
//    if (httpRequest.Headers.AllKeys.Where(key => key == "EntityName").Count() > 0)
//    {
//        entityName = httpRequest.Headers.GetValues("EntityName")[0];
//    }
//}
//if (httpRequest.Headers["EntityId"] != null)
//{
//    if (httpRequest.Headers.AllKeys.Where(key => key == "EntityId").Count() > 0)
//    {
//        entityName = httpRequest.Headers.GetValues("EntityId")[0];
//    }
//}
/********************************************************************************************
 * Project Name - CustomerSigned Waiver BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        26-Sep-2019      Deeksha           Created for waiver phase 2
 *2.70.2        17-Dec-2019      Jinto Thomas      Added parameter executioncontext for languagebl declararion with id 
 *2.100         19-Oct-2020      Guru S A          Enabling minor signature option for waiver
 ********************************************************************************************/
using iTextSharp.text;
using iTextSharp.text.pdf;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Languages;
//using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Utilities = Semnox.Core.Utilities.Utilities;

namespace Semnox.Parafait.Customer.Waivers
{
    public class CustomerSignedWaiverBL
    {
        private CustomerSignedWaiverDTO customerSignedWaiverDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Constructor with the customerSignedWaiver id as the parameter
        /// Would fetch the customerSignedWaiver object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public CustomerSignedWaiverBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            this.executionContext = executionContext;
            CustomerSignedWaiverDataHandler customerSignedWaiverDataHandler = new CustomerSignedWaiverDataHandler(sqlTransaction);
            customerSignedWaiverDTO = customerSignedWaiverDataHandler.GetCustomerSignedWaiverDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates customerSignedWaiversBL object using the customerSignedWaiverDTO
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="customerSignedWaiverDTO">customerSignedWaiverDTO object</param>
        public CustomerSignedWaiverBL(ExecutionContext executionContext, CustomerSignedWaiverDTO customerSignedWaiverDTO)
        {
            log.LogMethodEntry(executionContext, customerSignedWaiverDTO);
            this.executionContext = executionContext;
            this.customerSignedWaiverDTO = customerSignedWaiverDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// get CustomerSignedWaiverDTO Object
        /// </summary>
        public CustomerSignedWaiverDTO GetCustomerSignedWaiverDTO
        {
            get { return customerSignedWaiverDTO; }
        }

        public List<ValidationError> Validate()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (customerSignedWaiverDTO == null)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2297, (MessageContainerList.GetMessage(executionContext, "customerSignedWaiverDTO"))); //Cannot proceed customerSignedWaiverDTO record is Empty.
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"), MessageContainerList.GetMessage(executionContext, "customerSignedWaiverDTO"), errorMessage));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Saves the CustomerSignedWaiver
        /// Checks if the CustomerSignedWaiverId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerSignedWaiverDataHandler customerSignedWaiverDataHandler = new CustomerSignedWaiverDataHandler(sqlTransaction);
            bool callRoamingData = false;
            Validate();
            if (customerSignedWaiverDTO.CustomerSignedWaiverId < 0)
            {
                customerSignedWaiverDTO = customerSignedWaiverDataHandler.InsertCustomerSignedWaiver(customerSignedWaiverDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                customerSignedWaiverDTO.AcceptChanges();
                callRoamingData = true; 
            }
            else
            {
                if (customerSignedWaiverDTO.IsChanged)
                {
                    customerSignedWaiverDTO = customerSignedWaiverDataHandler.UpdateCustomerSignedWaiver(customerSignedWaiverDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerSignedWaiverDTO.AcceptChanges();
                    callRoamingData = true;
                }
            }
            if (customerSignedWaiverDTO.CustomerContentForWaiverDTOList != null && customerSignedWaiverDTO.CustomerContentForWaiverDTOList.Count > 0)
            {
                CreateWaiverSignaturePDF();
            }
            if (callRoamingData) 
            {
                CreateRoamingData(sqlTransaction);
            }
            log.LogMethodExit();
        }


        public void CreateWaiverSignaturePDF()
        {
            log.LogMethodEntry(customerSignedWaiverDTO.SignedWaiverFileName);
            if (customerSignedWaiverDTO != null && string.IsNullOrEmpty(customerSignedWaiverDTO.SignedWaiverFileName) == false)
            {// bool retStatus = false;
                string dateTimeFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT");
                string justDateFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
                string workingFolder = Path.GetTempPath();
                string outputFileName = Path.Combine(workingFolder, customerSignedWaiverDTO.SignedWaiverFileName);
                string filename = string.Empty;
                //Step 1: Create a Document-Object
                using (Document document = new Document())
                {
                    try
                    {
                        //Step 2: we create a writer that listens to the document
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputFileName, FileMode.Create));

                        //Step 3: Open the document
                        document.Open();

                        PdfContentByte cb = writer.DirectContent;

                        //The current file path
                        filename = Path.Combine(workingFolder, customerSignedWaiverDTO.WaiverFileName); // Path.GetTempPath() + customerSignedWaiverDTO.WaiverFileName;
                        if (File.Exists(filename) == false)
                        {
                            GenericUtils genericUtils = new GenericUtils();
                            filename = genericUtils.DownloadFileFromDB(executionContext, customerSignedWaiverDTO.WaiverFileName);
                        }
                        // we create a reader for the document
                        PdfReader reader = new PdfReader(filename);

                        for (int pageNumber = 1; pageNumber < reader.NumberOfPages + 1; pageNumber++)
                        {
                            document.SetPageSize(reader.GetPageSizeWithRotation(1));
                            document.NewPage();

                            //Insert to Destination on the first page
                            if (pageNumber == 1)
                            {
                                Chunk fileRef = new Chunk(" ");
                                fileRef.SetLocalDestination(filename);
                                document.Add(fileRef);
                            }

                            PdfImportedPage page = writer.GetImportedPage(reader, pageNumber);
                            int rotation = reader.GetPageRotation(pageNumber);
                            if (rotation == 90 || rotation == 270)
                            {
                                cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(pageNumber).Height);
                            }
                            else
                            {
                                cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                            }
                        }

                        // Add a new page to the pdf file
                        document.NewPage();

                        //Creating iTextSharp Table from the DataTable data
                        PdfPTable pdfTable = new PdfPTable(8);
                        pdfTable.WidthPercentage = 100f;
                        pdfTable.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.GRAY;

                        pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfTable.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;

                        pdfTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                        string fontFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
                        BaseFont baseFont = baseFont = BaseFont.CreateFont(fontFolderPath + "\\micross.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                        //Font support if chinese PRC language is selected.

                        if (executionContext != null)
                        {
                            int languageId = -1;
                            if (executionContext.GetLanguageId() > -1)
                            {
                                languageId = executionContext.GetLanguageId();
                            }
                            else
                            {
                                languageId = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "DEFAULT_LANGUAGE", -1));
                            }
                            Languages.Languages languages = new Languages.Languages(executionContext, languageId);
                            if (languages.GetLanguagesDTO != null)
                            {
                                if (languages.GetLanguagesDTO.LanguageName.Equals("Chinese (PRC)"))
                                {
                                    baseFont = BaseFont.CreateFont(fontFolderPath + "\\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                                }
                            }
                        }

                        iTextSharp.text.Font font_jpn = new iTextSharp.text.Font(baseFont);

                        iTextSharp.text.Font trxHeaderFnt = new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);
                        document.Add(new Paragraph(MessageContainerList.GetMessage(executionContext, "Customer Details"), trxHeaderFnt));
                        document.Add(new Paragraph("\n"));

                        //Insert Customer Name and Mobile number
                        iTextSharp.text.Font cusDetailsFnt = new iTextSharp.text.Font(baseFont, 12, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);

                        if (customerSignedWaiverDTO.CustomerContentForWaiverDTOList != null)
                        {
                            foreach (CustomerContentForWaiverDTO customerInfo in customerSignedWaiverDTO.CustomerContentForWaiverDTOList)
                            {
                                if ((customerInfo.CustomerId == customerSignedWaiverDTO.SignedFor) == false)
                                {
                                    continue; //Skip name print if customer is not the signed for
                                }
                                if (!string.IsNullOrEmpty(customerInfo.CustomerName))
                                    document.Add(new Paragraph(MessageContainerList.GetMessage(executionContext, "Customer Name") + ": " + customerInfo.CustomerName, cusDetailsFnt));
                                if (customerInfo.CustomerDOB != null)
                                    document.Add(new Paragraph(MessageContainerList.GetMessage(executionContext, "Date Of Birth") + ": " + ((DateTime)customerInfo.CustomerDOB).ToString(justDateFormat, CultureInfo.InvariantCulture), cusDetailsFnt));
                                document.Add(new Paragraph("\n"));
                                if (!string.IsNullOrEmpty(customerInfo.PhoneNumber))
                                    document.Add(new Paragraph(MessageContainerList.GetMessage(executionContext, "Phone") + ": " + customerInfo.PhoneNumber, cusDetailsFnt));
                                if (!string.IsNullOrEmpty(customerInfo.EmailId))
                                    document.Add(new Paragraph(MessageContainerList.GetMessage(executionContext, "Email") + ": " + customerInfo.EmailId, cusDetailsFnt));

                                if (customerInfo.WaiverCustomAttributeList != null && customerInfo.WaiverCustomAttributeList.Any())
                                {
                                    string headerColumn = string.Empty;
                                    //if (lookupValuesDTOList != null)

                                    // string[] parts = lookupValuesDTOList[0].Description.Split('|');


                                    int length = 0;
                                    if (string.IsNullOrEmpty(customerInfo.Attribute1Name) == false && string.IsNullOrEmpty(customerInfo.Attribute2Name) == false)
                                    {
                                        length = 2;
                                    }
                                    else if (string.IsNullOrEmpty(customerInfo.Attribute1Name) == false || string.IsNullOrEmpty(customerInfo.Attribute2Name) == false)
                                    {
                                        length = 1;
                                    }

                                    PdfPTable pdfCustomerTable = new PdfPTable(length);
                                    pdfCustomerTable.WidthPercentage = 50f;
                                    pdfCustomerTable.DefaultCell.BackgroundColor = iTextSharp.text.BaseColor.GRAY;

                                    pdfCustomerTable.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfCustomerTable.DefaultCell.VerticalAlignment = Element.ALIGN_TOP;

                                    pdfCustomerTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                                    iTextSharp.text.Font headerCustomerTextFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

                                    PdfPCell cell = new PdfPCell();
                                    if (length > 0 && length <= 2)
                                    {
                                        headerColumn = (string.IsNullOrEmpty(customerInfo.Attribute1Name) ? customerInfo.Attribute2Name : customerInfo.Attribute1Name);
                                        cell = new PdfPCell(new Phrase(headerColumn, headerCustomerTextFont));
                                        cell.BackgroundColor = new iTextSharp.text.BaseColor(Color.Gray);
                                        pdfCustomerTable.AddCell(cell);
                                    }
                                    if (length == 2)
                                    {
                                        headerColumn = customerInfo.Attribute2Name;
                                        cell = new PdfPCell(new Phrase(headerColumn, headerCustomerTextFont));
                                        cell.BackgroundColor = new iTextSharp.text.BaseColor(Color.Gray);
                                        pdfCustomerTable.AddCell(cell);
                                    }

                                    foreach (KeyValuePair<string, string> customAttributeKey in customerInfo.WaiverCustomAttributeList)
                                    {
                                        if (customAttributeKey.Key != null)
                                        {
                                            cell = new PdfPCell(new Phrase(customAttributeKey.Key, headerCustomerTextFont));
                                            pdfCustomerTable.AddCell(cell);
                                        }
                                        if (customAttributeKey.Value != null)
                                        {
                                            cell = new PdfPCell(new Phrase(customAttributeKey.Value, headerCustomerTextFont));
                                            pdfCustomerTable.AddCell(cell);
                                        }
                                    }
                                    document.Add(pdfCustomerTable);

                                }

                                document.Add(new Paragraph("\n"));
                            }
                        }




                        //Adding Header row
                        // iTextSharp.text.Font headerTextFont = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE);
                        //string trxId = string.Empty;
                        //if (datagrid != null)
                        //{

                        //}

                        //Adding DataRow
                        //iTextSharp.text.Font cellTextFont = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                        //iTextSharp.text.Font lastRowFnt = new iTextSharp.text.Font(baseFont, 8, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.WHITE);


                        document.Add(pdfTable);
                        document.Add(new Paragraph("\n"));

                        //add signature
                        document.Add(new Paragraph("\n"));

                        if (customerSignedWaiverDTO.WaiverSignedImageList != null && customerSignedWaiverDTO.WaiverSignedImageList.Any())
                        {
                            //int signatureCount = customerSignedWaiverDTO.WaiverSignedImageList.Count;
                            for (int i = 0; i < customerSignedWaiverDTO.WaiverSignedImageList.Count; i++)
                            {
                                if ((customerSignedWaiverDTO.WaiverSignedImageList[i].CustomerId == customerSignedWaiverDTO.SignedFor
                                         || customerSignedWaiverDTO.WaiverSignedImageList[i].CustomerId == customerSignedWaiverDTO.GuardianId) == false)
                                {
                                    continue; //Skip name print if customer is not the signed for or guardian
                                }

                                System.Drawing.Image signature = customerSignedWaiverDTO.WaiverSignedImageList[i].SignatureImage;
                                iTextSharp.text.Image pdfsignature = iTextSharp.text.Image.GetInstance(signature, System.Drawing.Imaging.ImageFormat.Jpeg);
                                pdfsignature.Alignment = iTextSharp.text.Image.ALIGN_RIGHT;
                                document.Add(pdfsignature);
                                document.Add(new Paragraph("\n"));

                                iTextSharp.text.Font cusSigHeaderTextFnt = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

                                Paragraph cusSigPara;
                                Paragraph cusSigDate; 
                                if (customerSignedWaiverDTO.WaiverSignedImageList[i].CustomerId == customerSignedWaiverDTO.GuardianId
                                    && customerSignedWaiverDTO.WaiverSignedImageList[i].CustomerId != customerSignedWaiverDTO.SignedFor)
                                {
                                    cusSigPara = new Paragraph(new Phrase(MessageContainerList.GetMessage(executionContext, "Guardian: ") + customerSignedWaiverDTO.WaiverSignedImageList[i].CustomerName, font_jpn));
                                }
                                else
                                {
                                    cusSigPara = new Paragraph(new Phrase(MessageContainerList.GetMessage(executionContext, "Participant: ") + customerSignedWaiverDTO.WaiverSignedImageList[i].CustomerName, font_jpn));
                                } 
                                cusSigDate = new Paragraph(new Phrase(MessageContainerList.GetMessage(executionContext, "Date") + ": " + (string.IsNullOrEmpty(dateTimeFormat) ? DateTime.Now.ToString() : DateTime.Now.ToString(dateTimeFormat)) + " (" +TimeZoneInfo.Local.DisplayName +") ", font_jpn));
                                cusSigPara.Alignment = Element.ALIGN_RIGHT;
                                document.Add(cusSigPara);
                                cusSigDate.Alignment = Element.ALIGN_RIGHT;
                                document.Add(cusSigDate);

                                document.Add(new Paragraph("\n"));
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        log.Error(e);
                        throw;
                    }
                    finally
                    {
                        document.Close();
                    }
                }


                try
                {
                    string InputFile = outputFileName;
                    string OutputFile = Path.Combine(Path.GetTempPath(), "Enc.pdf");

                    using (Stream output = new FileStream(OutputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        PdfReader pdfReader = new PdfReader(InputFile);
                        PdfEncryptor.Encrypt(pdfReader, output, true, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WAIVER_ENCRYPTION_KEY"), ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WAIVER_ENCRYPTION_KEY"), PdfWriter.ALLOW_SCREENREADERS);
                        try
                        {
                            pdfReader.Dispose();
                        }
                        catch { }
                    }

                    using (System.IO.FileStream fs = new System.IO.FileStream(OutputFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        System.IO.MemoryStream ms = new System.IO.MemoryStream();
                        fs.CopyTo(ms);
                        byte[] bytes = ms.GetBuffer();

                        string OutputEncFile = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY") + "\\" + customerSignedWaiverDTO.SignedWaiverFileName;
                        GenericUtils genericUtils = new GenericUtils();
                        genericUtils.WriteFileToServer(bytes, OutputEncFile);

                        try
                        {
                            bytes = null;
                            ms.Dispose();
                        }
                        catch { }
                    }


                    try
                    {
                        File.Delete(OutputFile);
                    }
                    catch (Exception ex) { log.Error(ex.Message); }
                }
                catch (Exception ex)
                {
                    log.Error("Error Encrypting the file" + ex.Message);
                    throw;
                }

                try
                {
                    File.Delete(outputFileName);
                }
                catch (Exception ex)
                {
                    log.Error("Error in file deletion." + ex.Message);
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// GetDecryptedWaiverFile
        /// </summary>
        /// <returns>string</returns>
        public string GetDecryptedWaiverFile(int siteId)
        {
            log.LogMethodEntry(siteId);
            string decrptedFileWithPath = string.Empty;
            string password = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WAIVER_ENCRYPTION_KEY");
            try
            {
                string inputFile = this.customerSignedWaiverDTO.SignedWaiverFileName;
                if (string.IsNullOrEmpty(inputFile) == false)
                {
                    log.LogVariableState("IsCorporate", executionContext.GetIsCorporate());
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ON_DEMAND_ROAMING")
                        && executionContext.GetIsCorporate() == false)
                    {
                        RemotingClient remotingClient = new RemotingClient();
                        remotingClient.DownloadServerFiles(siteId, inputFile);
                    }
                    GenericUtils genericUtils = new GenericUtils();
                    string filename = genericUtils.DownloadFileFromDB(executionContext, inputFile);
                    decrptedFileWithPath = Path.GetTempPath() + this.customerSignedWaiverDTO.SignedWaiverFileName;
                    log.Info("Initialize the PdfReader object");
                    PdfReader reader = new PdfReader(filename, new System.Text.ASCIIEncoding().GetBytes(password));

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfStamper stamper = new PdfStamper(reader, memoryStream);
                        stamper.Close();
                        reader.Close();
                        File.WriteAllBytes(decrptedFileWithPath, memoryStream.ToArray());
                    }
                    reader.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
            log.LogMethodExit(decrptedFileWithPath);
            return decrptedFileWithPath;
        }

        /// <summary>
        /// GetDecryptedWaiverFile
        /// </summary>
        /// <returns>string</returns>
        public string GetDecryptedSignedWaiverFileInBase64Format(Core.Utilities.Utilities utilities)
        {
            log.LogMethodEntry();
            string signedFileContent = string.Empty;
            string password = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WAIVER_ENCRYPTION_KEY");
            try
            {
                string inputFile = this.customerSignedWaiverDTO.SignedWaiverFileName;
                if (string.IsNullOrEmpty(inputFile) == false)
                {
                    log.LogVariableState("IsCorporate", executionContext.GetIsCorporate());
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "ENABLE_ON_DEMAND_ROAMING") &&
                        executionContext.GetIsCorporate() == false)
                    {
                        RemotingClient remotingClient = new RemotingClient();
                        remotingClient.DownloadServerFiles(utilities.ParafaitEnv.SiteId, inputFile);
                    }
                    GenericUtils genericUtils = new GenericUtils();
                    //string encryptedFileContent = genericUtils.DownloadFileFromDBInBase64Format(executionContext, inputFile);
                    byte[] content = genericUtils.DownloadFileFromDBAsByteArray(executionContext, inputFile);
                    log.Info("Initialize the PdfReader object");
                    PdfReader reader = new PdfReader(content, new System.Text.ASCIIEncoding().GetBytes(password));

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PdfStamper stamper = new PdfStamper(reader, memoryStream);
                        stamper.Close();
                        reader.Close();
                        signedFileContent = Convert.ToBase64String(memoryStream.ToArray());
                    }
                    reader.Dispose();
                    this.customerSignedWaiverDTO.SignedWaiverFileContentInBase64Format = signedFileContent;
                    this.customerSignedWaiverDTO.AcceptChanges();
                    log.LogVariableState("signedFileContent", signedFileContent);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
            log.LogMethodExit();
            return signedFileContent;
        }
        ///// <summary>
        ///// can deactive
        ///// </summary>
        ///// <param name="sqlTrx"></param>
        ///// <returns></returns>
        //public bool CanDeactivate(Utilities utilities, SqlTransaction sqlTrx = null)
        //{
        //    log.LogMethodEntry();
        //    bool canDeactivate = true;
        //    if (customerSignedWaiverDTO != null && customerSignedWaiverDTO.CustomerSignedWaiverId > -1)
        //    {
        //        LookupValuesList serverTimeObject = new LookupValuesList(executionContext);

        //        Type type = Type.GetType("Semnox.Parafait.Transaction.TransactionListBL,Transaction");
        //        object transactionListBL = null;
        //        if (type != null)
        //        {
        //            ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType() });
        //            transactionListBL = constructorN.Invoke(new object[] { executionContext });
        //        }
        //        else
        //            throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "TransactionListBL"));

        //        List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
        //        searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_SIGNED_WAIVER_ID, customerSignedWaiverDTO.CustomerSignedWaiverId.ToString()));
        //        searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS_NOT_IN, "CANCELLED"));
        //        searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, serverTimeObject.GetServerDateTime().Date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
        //        searchParam.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
        //        if (transactionListBL != null)
        //        {
        //            List<TransactionDTO> transactionDTOList = (List<TransactionDTO>)type.GetMethod("GetTransactionDTOList").Invoke(transactionListBL, new object[] { searchParam, utilities, sqlTrx, 0,10,false,false,false});
        //            log.LogVariableState("transactionDTOList", transactionDTOList);
        //            if (transactionDTOList != null && transactionDTOList.Any())
        //            {
        //                canDeactivate = false;
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "TransactionListBL"));
        //        }
        //    }
        //    log.LogMethodExit(canDeactivate);
        //    return canDeactivate;
        //}

        private void CreateRoamingData(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CustomerSignedWaiverDataHandler customerSignedWaiverDataHandler = new CustomerSignedWaiverDataHandler(sqlTransaction);
            CustomerSignedWaiverDTO updatedCustomerSignedWaiverDTO = customerSignedWaiverDataHandler.GetCustomerSignedWaiverDTO(customerSignedWaiverDTO.CustomerSignedWaiverId);
            DBSynchLogService dBSynchLogService = new DBSynchLogService(executionContext, "CustomerSignedWaiver", updatedCustomerSignedWaiverDTO.Guid, updatedCustomerSignedWaiverDTO.SiteId);
            dBSynchLogService.CreateRoamingDataForCustomer(sqlTransaction);
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of CustomerSignedWaiver
    /// </summary>
    public class CustomerSignedWaiverListBL
    {

        private List<CustomerSignedWaiverDTO> customerSignedWaiverList;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public CustomerSignedWaiverListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.customerSignedWaiverList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="customerSignedWaiverList"></param>
        /// <param name="executionContext"></param>
        public CustomerSignedWaiverListBL(ExecutionContext executionContext, List<CustomerSignedWaiverDTO> customerSignedWaiverList)
        {
            log.LogMethodEntry(customerSignedWaiverList, executionContext);
            this.executionContext = executionContext;
            this.customerSignedWaiverList = customerSignedWaiverList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Save or update records with inner collections
        /// </summary>
        public void SaveCustomerSignedWaiver(SqlTransaction sqlTrx = null)
        {
            try
            {
                log.LogMethodEntry(sqlTrx);
                if (customerSignedWaiverList != null)
                {
                    foreach (CustomerSignedWaiverDTO customerSignedWaiverDTO in customerSignedWaiverList)
                    {
                        CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(executionContext, customerSignedWaiverDTO);
                        customerSignedWaiverBL.Save(sqlTrx);
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns the Customer rSigned Waiver  List
        /// </summary>
        public List<CustomerSignedWaiverDTO> GetAllCustomerSignedWaiverList(List<KeyValuePair<CustomerSignedWaiverDTO.SearchByCSWParameters, string>> searchParameters,
                                               bool loadSignedWaiverFileContent = false, Utilities utilities = null,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadSignedWaiverFileContent);
            CustomerSignedWaiverDataHandler customerSignedWaiverDataHandler = new CustomerSignedWaiverDataHandler(sqlTransaction);
            customerSignedWaiverList = customerSignedWaiverDataHandler.GetCustomerSignedWaiverDTOList(searchParameters);
            if (loadSignedWaiverFileContent && utilities != null)
            {
                if (customerSignedWaiverList != null && customerSignedWaiverList.Any())
                {
                    for (int i = 0; i < customerSignedWaiverList.Count; i++)
                    {
                        CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(executionContext, customerSignedWaiverList[i]);
                        customerSignedWaiverBL.GetDecryptedSignedWaiverFileInBase64Format(utilities);
                    }
                }
            }
            log.LogMethodExit(customerSignedWaiverList);
            return customerSignedWaiverList;
        }
        /// <summary>
        /// Get All Customer Signed Waiver List
        /// </summary>
        /// <param name="customerIdList"></param>
        /// <param name="activeRecordsOnly"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<CustomerSignedWaiverDTO> GetAllCustomerSignedWaiverList(List<int> customerIdList, bool activeRecordsOnly = true, bool loadSignedWaiverFileContent = false, Utilities utilities = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerIdList);
            //List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList = null;
            CustomerSignedWaiverDataHandler customerSignedWaiverDataHandler = new CustomerSignedWaiverDataHandler(sqlTransaction);
            customerSignedWaiverList = customerSignedWaiverDataHandler.GetCustomerSignedWaiverDTOList(customerIdList, activeRecordsOnly);
            if (loadSignedWaiverFileContent && utilities != null)
            {
                if (customerSignedWaiverList != null && customerSignedWaiverList.Any())
                {
                    for (int i = 0; i < customerSignedWaiverList.Count; i++)
                    {
                        CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(executionContext, customerSignedWaiverList[i]);
                        customerSignedWaiverBL.GetDecryptedSignedWaiverFileInBase64Format(utilities);
                    }
                }
            }
            log.LogMethodExit(customerSignedWaiverList);
            return customerSignedWaiverList;
        }
    }
}

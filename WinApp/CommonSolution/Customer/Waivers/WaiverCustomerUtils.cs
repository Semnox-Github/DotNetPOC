/********************************************************************************************
 * Project Name - Semnox.Parafait.Customer.Waivers 
 * Description  - WaiverCustomerUtils
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        01-Oct-2019      Guru S A       Created for waiver phase 2 changes
 *2.140.0       14-Sep-2021      Guru S A       Waiver mapping UI enhancements
 ********************************************************************************************/

using iTextSharp.text;
using iTextSharp.text.pdf;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Customer.Waivers
{
    public class WaiverCustomerUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string WAIVER_SETUP_LOOKUP_NAME = "WAIVER_SETUP";
        public static string MAX_COUNT_FOR_POS_MAP_UI_CUST_SEARCH = "MAX_COUNT_FOR_POS_MAP_UI_CUST_SEARCH";

        public static void HasValidWaiverSetup(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            WaiverSetContainer waiverSetContainer = null;
            try
            {
                waiverSetContainer = WaiverSetContainer.GetInstance;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;//Unexpected error while getting waiver file details. Please check the setup
            }
            List<WaiverSetDTO> waiverSetDTOList = waiverSetContainer.GetWaiverSetDTOList(executionContext.GetSiteId());
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (waiverSetDTOList != null && waiverSetDTOList.Any())
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SIGN_WAIVER_WITHOUT_CUSTOMER_REGISTRATION", false) == false)
                {
                    string bDateConfigValue = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BIRTH_DATE");
                    string emailConfigValue = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "EMAIL");
                    if (bDateConfigValue != "M" && bDateConfigValue != "O" && bDateConfigValue != "N")
                    {
                        validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"),
                                                                   MessageContainerList.GetMessage(executionContext, "Setup"),
                                                                   MessageContainerList.GetMessage(executionContext, 2314, MessageContainerList.GetMessage(executionContext, "BIRTH_DATE"))));
                        //,'Waiver signature requires &1 setup as mandatory or optional'
                    }
                    //if (emailConfigValue != "M" && emailConfigValue != "O")
                    //{
                    //    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"),
                    //                                                MessageContainerList.GetMessage(executionContext, "Setup"),
                    //                                                MessageContainerList.GetMessage(executionContext, 2314, MessageContainerList.GetMessage(executionContext, "EMAIL"))));
                    //    //,'Waiver signature requires &1 setup as mandatory or optional'
                    //}
                    if (bDateConfigValue != "N" && ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "IGNORE_CUSTOMER_BIRTH_YEAR", false) == true)
                    {
                        validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"),
                                                                   MessageContainerList.GetMessage(executionContext, "Setup"),
                                                                  MessageContainerList.GetMessage(executionContext, 2315)));//,'Waiver signature needs complete date of birth'
                    }
                }
                else
                {
                    if (CustomerListBL.GetGuestCustomerId(executionContext) == -1)
                    {
                        validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Waiver"),
                                            MessageContainerList.GetMessage(executionContext, "Setup"),
                                           MessageContainerList.GetMessage(executionContext, 2313)));//,'Guest customer setup for waiver is not defined'
                    }
                }

                if (validationErrorList != null && validationErrorList.Any())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Waiver setup validation"), validationErrorList);
                }
            }
            log.LogMethodExit();
        }


        public static List<LookupValuesDTO> GetWaiverCustomerAttributeLookup(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            List<LookupValuesDTO> custAttributesInWaiverLookUpValueDTOList = null;
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "SHOW_CUSTOM_ATTRIBUTES_IN_WAIVER_SCREEN"))
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CUSTOM_ATTRIBUTES_IN_WAIVER"));
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                custAttributesInWaiverLookUpValueDTOList = lookupValuesList.GetAllLookupValues(searchParam);
            }
            log.LogMethodExit(custAttributesInWaiverLookUpValueDTOList);
            return custAttributesInWaiverLookUpValueDTOList;
        }

        /// <summary>
        /// Create Consolidated PDF
        /// </summary>
        /// <param name="consolidatedFile"></param>
        /// <param name="attachFiles"></param>
        public static void CreateConsolidatedPDF(string consolidatedFile, List<string> attachFiles)
        {
            log.LogMethodEntry(consolidatedFile, attachFiles);
            if (attachFiles != null && attachFiles.Count > 0)
            {
                try
                {
                    string workingFolder = Path.GetTempPath();
                    string destinationFile = Path.Combine(workingFolder, consolidatedFile);
                    int f = 0;
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(attachFiles[f]);
                    // we retrieve the total number of pages
                    int n = reader.NumberOfPages;
                    //Console.WriteLine("There are " + n + " pages in the original file.");
                    // step 1: creation of a document-object
                    Document document = new Document(reader.GetPageSizeWithRotation(1));
                    // step 2: we create a writer that listens to the document
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationFile, FileMode.Create));
                    // step 3: we open the document
                    document.Open();
                    PdfContentByte cb = writer.DirectContent;
                    PdfImportedPage page;
                    int rotation;
                    // step 4: we add content
                    while (f < attachFiles.Count)
                    {
                        int i = 0;
                        while (i < n)
                        {
                            i++;
                            document.SetPageSize(reader.GetPageSizeWithRotation(i));
                            document.NewPage();
                            page = writer.GetImportedPage(reader, i);
                            rotation = reader.GetPageRotation(i);
                            if (rotation == 90 || rotation == 270)
                            {
                                cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                            }
                            else
                            {
                                cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                            }
                            //Console.WriteLine("Processed page " + i);
                        }
                        f++;
                        if (f < attachFiles.Count)
                        {
                            reader = new PdfReader(attachFiles[f]);
                            // we retrieve the total number of pages
                            n = reader.NumberOfPages;
                            //Console.WriteLine("There are " + n + " pages in the original file.");
                        }
                    }

                    // step 5: we close the document
                    document.Close();
                    try
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    try
                    {
                        document.Dispose();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw;
                }
                finally
                {
                    for (int i = 0; i < attachFiles.Count; i++)
                    {
                        try
                        {
                            FileInfo file = new FileInfo(attachFiles[i]);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Write File To Server
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="utilities"></param>
        public static void WriteFileToServer(string fileName, Core.Utilities.Utilities utilities)
        {
            log.LogMethodEntry(fileName);
            try
            {
                string inputFile = Path.GetTempPath() + fileName;
                log.LogVariableState("inputFile", inputFile);
                using (System.IO.FileStream fs = new System.IO.FileStream(inputFile, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    fs.CopyTo(ms);
                    byte[] bytes = ms.GetBuffer();

                    string OutputFile = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "IMAGE_DIRECTORY") + "\\" + fileName;
                    GenericUtils genericUtils = new GenericUtils();
                    genericUtils.WriteFileToServer(bytes, OutputFile);

                    try
                    {
                        ms.Dispose();
                    }
                    catch { }
                }
                try
                {
                    File.Delete(inputFile);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }

            log.LogMethodExit();
        }


        public static string StripNonAlphaNumericExceptUnderScore(string docName)
        {
            log.LogMethodEntry();
            char[] arr = docName.ToCharArray();
            string finalString = string.Empty;
            for (int i = 0; i < arr.Count(); i++)
            {
                if (char.IsLetterOrDigit(arr[i]) || arr[i] == '_')
                {
                    finalString = finalString + arr[i];
                }
            }
            log.LogMethodExit(finalString);
            return finalString;
        }

        public static List<LookupValuesDTO> GetWaiverSetupLookupValues(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> param = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            param.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, WAIVER_SETUP_LOOKUP_NAME)); 
            param.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(param);
            log.LogMethodExit();
            return lookupValuesDTOList;
        }
    }
}

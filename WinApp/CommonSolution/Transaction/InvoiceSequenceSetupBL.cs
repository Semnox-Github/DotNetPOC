/********************************************************************************************
 * Project Name - Transaction
 * Description  - Business logic for InvoiceSequenceSetup
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        23-Apr-2019   Mushahid Faizan   Added SQLTransaction in SaveUpdateInvoiceSequenceSetupList()
 *2.90        11-May-2020   Girish Kundar         Modified : Changes as part of the REST API  
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business logic for InvoiceSequenceSetup class.
    /// </summary>
    public class InvoiceSequenceSetupBL
    {
        private InvoiceSequenceSetupDTO invoiceSequenceSetupDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private InvoiceSequenceSetupBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the id as the parameter
        /// Would fetch the InvoiceSequenceSetup object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sqlTransaction"></param>
        public InvoiceSequenceSetupBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            InvoiceSequenceSetupDataHandler invoiceSequenceSetupDatahandler = new InvoiceSequenceSetupDataHandler(sqlTransaction);
            invoiceSequenceSetupDTO = invoiceSequenceSetupDatahandler.GetInvoiceSequenceSetupDTO(id);
            if (invoiceSequenceSetupDTO == null)
            {
                string message = " Record Not found with id" + id;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="invoiceSequenceSetupDTO">Parameter of the type InvoiceSequenceSetupDTO</param>
        public InvoiceSequenceSetupBL(ExecutionContext executionContext, InvoiceSequenceSetupDTO invoiceSequenceSetupDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, invoiceSequenceSetupDTO);
            this.invoiceSequenceSetupDTO = invoiceSequenceSetupDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the InvoiceSequenceSetup
        /// InvoiceSequenceSetup will be inserted if InvoiceSequenceSetupId is -1
        /// else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            InvoiceSequenceSetupDataHandler invoiceSequenceSetupDatahandler = new InvoiceSequenceSetupDataHandler(sqlTransaction);
            if (invoiceSequenceSetupDTO.InvoiceSequenceSetupId == -1)
            {
                ValidateInvoiceSequenceSetup();
                invoiceSequenceSetupDTO = invoiceSequenceSetupDatahandler.InsertInvoiceSequenceSetup(invoiceSequenceSetupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                invoiceSequenceSetupDTO.AcceptChanges(); 
            }
            else
            {
                if (invoiceSequenceSetupDTO.IsChanged == true)
                {
                    ValidateInvoiceSequenceSetup();
                    invoiceSequenceSetupDTO = invoiceSequenceSetupDatahandler.UpdateInvoiceSequenceSetup(invoiceSequenceSetupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    invoiceSequenceSetupDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private void ValidateInvoiceSequenceSetup()
        {
            log.LogMethodEntry();
            InvoiceSequenceSetupListBL invoiceSequenceSetupListBL = new InvoiceSequenceSetupListBL(executionContext);
            List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.ISACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.PREFIX, invoiceSequenceSetupDTO.Prefix));
            List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOList = invoiceSequenceSetupListBL.GetAllInvoiceSequenceSetupList(searchParameters);
            if (invoiceSequenceSetupDTOList != null && invoiceSequenceSetupDTO.InvoiceSequenceSetupId < 0)
            {
                if (invoiceSequenceSetupDTOList[0].InvoiceTypeId != invoiceSequenceSetupDTO.InvoiceTypeId)
                {
                    throw new InvalidPrefixException();
                }

                invoiceSequenceSetupDTOList = invoiceSequenceSetupDTOList.OrderByDescending(x => x.SeriesEndNumber).ToList();
                if (invoiceSequenceSetupDTOList[0].Prefix == invoiceSequenceSetupDTO.Prefix)
                {
                    invoiceSequenceSetupDTO.SeriesStartNumber = invoiceSequenceSetupDTOList[0].SeriesEndNumber + 1;
                }
            }
            else if (invoiceSequenceSetupDTO.InvoiceSequenceSetupId < 0)
            {
                invoiceSequenceSetupDTO.SeriesStartNumber = 1;
            }

            InvoiceSequenceSetupDataHandler invoiceSequenceSetupDataHandler = new InvoiceSequenceSetupDataHandler(null);
            bool validSetup = invoiceSequenceSetupDataHandler.ValidateInvoiceSequenceSetup(invoiceSequenceSetupDTO.SeriesStartNumber, invoiceSequenceSetupDTO.SeriesEndNumber, invoiceSequenceSetupDTO.Prefix, invoiceSequenceSetupDTO.InvoiceSequenceSetupId);
            if (!validSetup)
            {
                throw new InvalidSeriesNumberException();
            }

            TimeSpan difference = invoiceSequenceSetupDTO.ApprovedDate - invoiceSequenceSetupDTO.ResolutionDate;
            double days = difference.TotalDays;
            if (days > 10 || invoiceSequenceSetupDTO.ResolutionDate > invoiceSequenceSetupDTO.ApprovedDate)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2470));//"Resolution Date should not be greater than approved date and it should be within ten days of approved date"
            }

            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SYSTEM_AUTHORIZATION"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> LookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (LookUpValueList != null)
            {
                for (int i = 0; i < LookUpValueList.Count; i++)
                {
                    if (LookUpValueList[i].LookupValue == "SYSTEM_AUTHORIZATION_NUMBER" && !string.IsNullOrEmpty(LookUpValueList[i].Description))
                    {
                        invoiceSequenceSetupDTOList = new List<InvoiceSequenceSetupDTO>();
                        searchParameters = new List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.RESOLUTION_NUMBER, invoiceSequenceSetupDTO.ResolutionNumber));
                        invoiceSequenceSetupDTOList = invoiceSequenceSetupListBL.GetAllInvoiceSequenceSetupList(searchParameters);
                        if (invoiceSequenceSetupDTOList != null && invoiceSequenceSetupDTO.InvoiceSequenceSetupId < 0)
                        {
                            throw new InvalidResolutionNumberException();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Credit invoice sequence that is currently used
        /// </summary>
        public string GetSequenceNumber(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (invoiceSequenceSetupDTO != null && invoiceSequenceSetupDTO.IsActive == true)
            {
                if (invoiceSequenceSetupDTO.CurrentValue == null)
                {
                    //Checks if the series is series if used within the initial expiry days 
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INITIAL_EXPIRY_DATE"));

                    List<LookupValuesDTO> LookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                    if (LookUpValueList != null)
                    {
                        DateTime endDate = invoiceSequenceSetupDTO.ApprovedDate.AddDays(Convert.ToDouble(LookUpValueList[0].LookupValue));
                        if (ServerDateTime.Now.Date > endDate)
                        {
                            log.LogMethodExit();
                            throw new SeriesExpiredException();
                        }
                    }
                }
                if (invoiceSequenceSetupDTO.ExpiryDate < ServerDateTime.Now)
                {
                    log.LogMethodExit();
                    throw new SeriesExpiredException();
                }
                if (invoiceSequenceSetupDTO.CurrentValue != null && invoiceSequenceSetupDTO.CurrentValue >= invoiceSequenceSetupDTO.SeriesEndNumber)
                {
                    log.LogMethodExit();
                    throw new SeriesEndNumberExceededException();
                }
                else
                {
                    // return SequenceNumber;
                    string SequenceNumber = invoiceSequenceSetupDTO.Prefix + (invoiceSequenceSetupDTO.CurrentValue == null ? (invoiceSequenceSetupDTO.SeriesStartNumber) : invoiceSequenceSetupDTO.CurrentValue + 1).ToString().PadLeft(invoiceSequenceSetupDTO.SeriesEndNumber.ToString().Length, '0');
                    invoiceSequenceSetupDTO.CurrentValue = invoiceSequenceSetupDTO.CurrentValue == null ? (invoiceSequenceSetupDTO.SeriesStartNumber) : invoiceSequenceSetupDTO.CurrentValue + 1;
                    Save(sqlTransaction);
                    log.LogMethodExit();
                    return SequenceNumber;
                }
            }
            else
            {
                log.LogMethodExit();
                throw new SeriesExpiredException();
            }
        }


        /// <summary>
        /// Validates the invoice sequence Series End Number on POS_Load
        /// </summary>
        /// <returns></returns>
        public double ValidateInvoiceSequenceSeriesNumber()
        {
            log.LogMethodEntry();
            double value = Convert.ToDouble((100 * (invoiceSequenceSetupDTO.CurrentValue - invoiceSequenceSetupDTO.SeriesStartNumber)) / (invoiceSequenceSetupDTO.SeriesEndNumber - invoiceSequenceSetupDTO.SeriesStartNumber));

            log.LogMethodExit(value);
            return value;
        }

        /// <summary>
        /// Validates the invoice sequence Expiry Date on POS_Load
        /// </summary>
        /// <returns></returns>
        public double ValidateInvoiceSequenceExpiryDate()
        {
            log.LogMethodEntry();
            var value = ((invoiceSequenceSetupDTO.ExpiryDate - invoiceSequenceSetupDTO.ApprovedDate).TotalDays);
            var percentage = (ServerDateTime.Now - invoiceSequenceSetupDTO.ExpiryDate).TotalDays * 100 / value;
            log.LogMethodExit(percentage);
            return percentage;
        }

        ///<summary>
        ///Fetches the corresponding series details
        ///for transaction number
        ///</summary>
        public string FetchSeriesInfo(string trxNo)
        {
            log.LogMethodEntry(trxNo);
            string str = "";
            if ((trxNo).StartsWith(invoiceSequenceSetupDTO.Prefix))
            {
                str = trxNo.Remove(0, (invoiceSequenceSetupDTO.Prefix).Length);
                int value;
                if (int.TryParse(str, out value))
                {
                    if (value >= invoiceSequenceSetupDTO.SeriesStartNumber && value <= invoiceSequenceSetupDTO.SeriesEndNumber)
                    {
                        return str;
                    }
                    else
                        str = "";

                }
                else
                    str = "";
            }
            log.LogMethodExit(str);
            return str;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public InvoiceSequenceSetupDTO InvoiceSequenceSetupDTO
        {
            get
            {
                return invoiceSequenceSetupDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of Invoice Sequence Setup 
    /// </summary>
    public class InvoiceSequenceSetupListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOList;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public InvoiceSequenceSetupListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.invoiceSequenceSetupDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="invoiceSequenceSetupDTOs"></param>
        public InvoiceSequenceSetupListBL(ExecutionContext executionContext, List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOs)
        {
            log.LogMethodEntry(executionContext, invoiceSequenceSetupDTOs);
            this.executionContext = executionContext;
            this.invoiceSequenceSetupDTOList = invoiceSequenceSetupDTOs;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Invoice Sequence Setup list
        /// </summary>
        public List<InvoiceSequenceSetupDTO> GetAllInvoiceSequenceSetupList(List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InvoiceSequenceSetupDataHandler invoiceSequenceSetupDataHandler = new InvoiceSequenceSetupDataHandler(sqlTransaction);
            List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOList = invoiceSequenceSetupDataHandler.GetAllInvoiceSequenceSetupList(searchParameters);
            log.LogMethodExit(invoiceSequenceSetupDTOList);
            return invoiceSequenceSetupDTOList;
        }

        /// <summary>
        /// This method should be used to Save and Update the Invoice Sequence Setup details for Web Management Studio.
        /// </summary>
        public void SaveUpdateInvoiceSequenceSetupList()
        {
            log.LogMethodEntry();
            if (invoiceSequenceSetupDTOList != null && invoiceSequenceSetupDTOList.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (InvoiceSequenceSetupDTO invoiceSequenceSetupDTO in invoiceSequenceSetupDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            InvoiceSequenceSetupBL invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(executionContext, invoiceSequenceSetupDTO);
                            invoiceSequenceSetupBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw new Exception(ex.Message, ex);
                        }
                    }
                }
                log.LogMethodExit();
            }
        }
    }

    /// <summary>
    /// Represents Invalid Prefix error that occur during invoice sequence setup. 
    /// </summary>
    public class InvalidPrefixException : Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InvalidPrefixException()
        {

        }
        /// <summary>
        /// Initializes a new instance of InvalidPrefixException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidPrefixException(string message)
        : base(message)
        {
        }
    }

    /// <summary>
    /// Represents InvalidResolutionNumber error that occur during invoice sequence setup. 
    /// </summary>
    public class InvalidResolutionNumberException : Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InvalidResolutionNumberException()
        {

        }
        /// <summary>
        /// Initializes a new instance of InvalidResolutionNumberException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidResolutionNumberException(string message)
        : base(message)
        {
        }
    }

    /// <summary>
    /// Represents InvalidSeriesNumber error that occur during invoice sequence setup. 
    /// </summary>
    public class InvalidSeriesNumberException : Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InvalidSeriesNumberException()
        {

        }
        /// <summary>
        /// Initializes a new instance of InvalidSeriesNumberException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidSeriesNumberException(string message)
        : base(message)
        {
        }
    }
}

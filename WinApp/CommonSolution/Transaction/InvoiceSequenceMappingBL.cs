/********************************************************************************************
 * Project Name - Invoice Sequence Mapping BL
 * Description  - Business Logic for Invoice Sequence Mapping
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.60        13-May-2019   Mushahid Faizan       Added SaveUpdateSequenceMappingList() and InvoiceSequenceMappingBL, InvoiceSequenceMappingListBL constructor.
 *2.90        11-May-2020   Girish Kundar         Modified : Changes as part of the REST API  
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business logic for InvoiceSequenceMapping class.
    /// </summary>
    public class InvoiceSequenceMappingBL
    {
        private InvoiceSequenceMappingDTO invoiceSequenceMappingDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext ;
        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private InvoiceSequenceMappingBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="invoiceSequenceMappingDTO"></param>
        public InvoiceSequenceMappingBL(ExecutionContext executionContext, InvoiceSequenceMappingDTO invoiceSequenceMappingDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, invoiceSequenceMappingDTO);
            this.invoiceSequenceMappingDTO = invoiceSequenceMappingDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the id as the parameter
        /// Would fetch the InvoiceSequenceMapping object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sqlTransaction"></param>
        public InvoiceSequenceMappingBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id, executionContext, sqlTransaction);
            InvoiceSequenceMappingDataHandler invoiceSequenceMappingDatahandler = new InvoiceSequenceMappingDataHandler(sqlTransaction);
            invoiceSequenceMappingDTO = invoiceSequenceMappingDatahandler.GetInvoiceSequenceMappingDTO(id);
            if (invoiceSequenceMappingDTO == null)
            {
                string message = " Record Not found with id" + id;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }       
        /// <summary>
        /// Saves the InvoiceSequenceMapping
        /// InvoiceSequenceMapping will be inserted if InvoiceSequenceMappingId is -1
        /// else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);            
            InvoiceSequenceMappingDataHandler invoiceSequenceMappingDatahandler = new InvoiceSequenceMappingDataHandler(sqlTransaction);
            if (invoiceSequenceMappingDTO.Id < 0)
            {
                ValidateMappingDetails();
                invoiceSequenceMappingDTO = invoiceSequenceMappingDatahandler.InsertInvoiceSequenceMapping(invoiceSequenceMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                invoiceSequenceMappingDTO.AcceptChanges();
            }
            else
            {
                if (invoiceSequenceMappingDTO.IsChanged == true)
                {
                    ValidateMappingDetails();
                    invoiceSequenceMappingDTO = invoiceSequenceMappingDatahandler.UpdateInvoiceSequenceMapping(invoiceSequenceMappingDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    invoiceSequenceMappingDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validation Before Saving the data
        /// </summary>
        private void ValidateMappingDetails()
        {
            log.LogMethodEntry();
            InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(executionContext);

            //Validates to check if the sequence selected has an existing mapping
            List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_SETUP_ID, invoiceSequenceMappingDTO.InvoiceSequenceSetupId.ToString()));
            List<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOList = invoiceSequenceMappingListBL.GetAllInvoiceSequenceMappingList(searchParameters);
            if (invoiceSequenceMappingDTOList != null && invoiceSequenceMappingDTO.Id == -1)
            {
                throw new InvalidMappingException();
            }

            searchParameters = new List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>>();
            invoiceSequenceMappingDTOList = new List<InvoiceSequenceMappingDTO>();

            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.EFFECTIVE_DATE, invoiceSequenceMappingDTO.EffectiveDate.ToString("yyyy-MM-dd HH:mm:ss")));
            InvoiceSequenceSetupBL invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(executionContext, invoiceSequenceMappingDTO.InvoiceSequenceSetupId);
            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.INVOICE_TYPE_ID, invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.InvoiceTypeId.ToString()));
            searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.ISACTIVE, "1"));
            invoiceSequenceMappingDTOList = invoiceSequenceMappingListBL.GetAllInvoiceSequenceMappingList(searchParameters);

            //Validates to check if mapping exists the selected effective date
            if (invoiceSequenceMappingDTOList != null && invoiceSequenceMappingDTOList[0].Id != invoiceSequenceMappingDTO.Id)
            {
                throw new InvalidMappingException();
            }

            //Validates to ensure that series will not be expired for the selected effective date
            if (invoiceSequenceSetupBL.InvoiceSequenceSetupDTO == null || invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.ExpiryDate < ServerDateTime.Now
                || invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.ExpiryDate < invoiceSequenceMappingDTO.EffectiveDate)
            {
                if (invoiceSequenceMappingDTOList[0].Id != invoiceSequenceMappingDTO.Id)
                    throw new SeriesExpiredException();
            }

            if(invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.ApprovedDate.Date > invoiceSequenceMappingDTO.EffectiveDate.Date)
            {
                throw new Exception();
            }

            //Checks if the series is series if used within the initial expiry days 
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INITIAL_EXPIRY_DATE"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));

            List<LookupValuesDTO> LookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (LookUpValueList != null)
            {
                DateTime endDate = invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.ApprovedDate.AddDays(Convert.ToDouble(LookUpValueList[0].LookupValue));
                if (invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.CurrentValue == null && invoiceSequenceMappingDTO.EffectiveDate > endDate)
                {
                    throw new SeriesExpiredException();
                }
            }

            if (!invoiceSequenceSetupBL.InvoiceSequenceSetupDTO.IsActive)
            {
                throw new InvalidInvoiceSequenceException();
            }
            log.LogMethodExit();
        }

        ///<summary>
        ///Gets the DTO
        ///</summary>
        public InvoiceSequenceMappingDTO InvoiceSequenceMappingDTO
        {
            get
            {
                return invoiceSequenceMappingDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of Invoice Sequence Mapping 
    /// </summary>
    public class InvoiceSequenceMappingListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public InvoiceSequenceMappingListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.invoiceSequenceMappingDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="invoiceSequenceMappingDTOList"></param>
        /// <param name="executionContext"></param>
        public InvoiceSequenceMappingListBL(ExecutionContext executionContext, List<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOList)
        {
            log.LogMethodEntry(executionContext, invoiceSequenceMappingDTOList);
            this.invoiceSequenceMappingDTOList = invoiceSequenceMappingDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Invoice Sequence Setup list
        /// </summary>
        public List<InvoiceSequenceMappingDTO> GetAllInvoiceSequenceMappingList(List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InvoiceSequenceMappingDataHandler invoiceSequenceMappingDataHandler = new InvoiceSequenceMappingDataHandler(sqlTransaction);
            List<InvoiceSequenceMappingDTO> invoiceSequenceMappingDTOList = invoiceSequenceMappingDataHandler.GetAllInvoiceSequenceMappingList(searchParameters);
            log.LogMethodExit(invoiceSequenceMappingDTOList);
            return invoiceSequenceMappingDTOList;
        }


        /// <summary>
        /// Save and Updated the Invoice Sequence Mapping list details
        /// </summary>
        public void Save() 
        {
            log.LogMethodEntry();
            try
            {
                if (invoiceSequenceMappingDTOList != null && invoiceSequenceMappingDTOList.Count != 0)
                {
                    foreach (InvoiceSequenceMappingDTO invoiceSequenceMappingDTO in invoiceSequenceMappingDTOList)
                    {
                        InvoiceSequenceMappingBL invoiceSequenceMappingBL = new InvoiceSequenceMappingBL(executionContext, invoiceSequenceMappingDTO);
                        invoiceSequenceMappingBL.Save();
                    }
                }
            }
            catch (InvalidMappingException ex)
            {
                log.Error(ex.Message);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1330)); /// There exists a mapping for the selected values
            }
            catch (InvalidInvoiceSequenceException ex)
            {
                log.Error(ex.Message);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1331)); /// Please select a valid invoice sequence
            }
            catch (SeriesExpiredException ex)
            {
                log.Error(ex.Message);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1332)); /// The selected invoice series is expired
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(MessageContainerList.GetMessage(executionContext, 718)); /// There was an error while saving. Please retry.
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Represents Invalid Mapping error that occur during application execution. 
    /// </summary>
    public class InvalidMappingException : Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InvalidMappingException()
        {

        }
        /// <summary>
        /// Initializes a new instance of InvalidMappingException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidMappingException(string message)
        : base(message)
        {
        }
    }

    /// <summary>
    /// Represents Invalid Mapping error that occur during application execution. 
    /// </summary>
    public class InvalidInvoiceSequenceException : Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public InvalidInvoiceSequenceException()
        {

        }
        /// <summary>
        /// Initializes a new instance of InvalidInvoiceSequenceException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public InvalidInvoiceSequenceException(string message)
        : base(message)
        {
        }
    }

    /// <summary>
    /// Represents Invalid Mapping error that occur during application execution. 
    /// </summary>
    public class SeriesExpiredException : Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SeriesExpiredException()
        {

        }
        /// <summary>
        /// Initializes a new instance of InvalidInvoiceSequenceException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public SeriesExpiredException(string message)
        : base(message)
        {
        }
    }

    /// <summary>
    /// Represents SeriesEndNumber Exceeded error that occur during application execution. 
    /// </summary>
    public class SeriesEndNumberExceededException : Exception
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SeriesEndNumberExceededException()
        {

        }
        /// <summary>
        /// Initializes a new instance of SeriesEndNumberExceededException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public SeriesEndNumberExceededException(string message)
        : base(message)
        {
        }
    }
}

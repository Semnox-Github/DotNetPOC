/********************************************************************************************
 * Project Name - Receipt Print Template Details BL
 * Description  - Business logic to handle Receipt Template details
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2018      Mathew Ninan   Created 
 *2.70.2        18-Jul-2019      Deeksha        Modifications as per 3 tier standard.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Reflection;

using Semnox.Core.Utilities;
namespace Semnox.Parafait.Printer
{
    /// <summary>
    /// BL class for ReceiptPrintTemplate
    /// </summary>
    public class ReceiptPrintTemplateBL
    {
        private ReceiptPrintTemplateDTO receiptPrintTemplateDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ReceiptPrintTemplateBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public ReceiptPrintTemplateBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            receiptPrintTemplateDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Id as the parameter
        /// Would fetch the Receipt Print Template object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        ///  <param name="SqlTransaction">SqlTransaction</param>
        public ReceiptPrintTemplateBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction=null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ReceiptPrintTemplateDataHandler receiptPrintTemplateDataHandler = new ReceiptPrintTemplateDataHandler(sqlTransaction);
            receiptPrintTemplateDTO = receiptPrintTemplateDataHandler.GetReceiptPrintTemplate(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ReceiptPrintTemplateBL object using the receiptPrintTemplateDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="receiptPrintTemplateDTO">receiptPrintTemplateDTO object</param>
        public ReceiptPrintTemplateBL(ExecutionContext executionContext, ReceiptPrintTemplateDTO receiptPrintTemplateDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, receiptPrintTemplateDTO);
            this.receiptPrintTemplateDTO = receiptPrintTemplateDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Receipt Print Template
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ReceiptPrintTemplateDataHandler receiptPrintTemplateDataHandler = new ReceiptPrintTemplateDataHandler(sqlTransaction);
            if (receiptPrintTemplateDTO.ID < 0)
            {
                receiptPrintTemplateDTO = receiptPrintTemplateDataHandler.InsertReceiptPrintTemplate(receiptPrintTemplateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                receiptPrintTemplateDTO.AcceptChanges();
            }
            else
            {
                if (receiptPrintTemplateDTO.IsChanged)
                {
                    receiptPrintTemplateDTO = receiptPrintTemplateDataHandler.UpdateReceiptPrintTemplate(receiptPrintTemplateDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    receiptPrintTemplateDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Convert each Col information to rows
        /// Total of 5 rows. E.g. Col1Data, Col1Alignment etc.
        /// </summary>
        /// <returns>List ReceiptColumnData object</returns>
        public List<ReceiptColumnData> GetReceiptDTOColumnData()
        {
            log.LogMethodEntry();
            List<ReceiptColumnData> receiptColumnDataList = new List<ReceiptColumnData>();
            for (int j = 1; j <= 5; j++)
            {
                ReceiptColumnData receiptColumnData = new ReceiptColumnData();
                receiptColumnData.Sequence = j;
                foreach (PropertyInfo receiptTemplateDTOProperties in receiptPrintTemplateDTO.GetType().GetProperties())
                {
                    if (receiptTemplateDTOProperties.Name.Equals("Col" + j.ToString() + "Data"))
                        receiptColumnData.Data = receiptTemplateDTOProperties.GetValue(receiptPrintTemplateDTO, null).ToString();
                    if (receiptTemplateDTOProperties.Name.Equals("Col" + j.ToString() + "Alignment"))
                        receiptColumnData.Alignment = receiptTemplateDTOProperties.GetValue(receiptPrintTemplateDTO, null).ToString();
                }
                receiptColumnDataList.Add(receiptColumnData);
            }
            receiptColumnDataList.OrderBy(x => x.Sequence).ToList();
            log.LogMethodExit(receiptColumnDataList);
            return receiptColumnDataList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ReceiptPrintTemplateDTO ReceiptPrintTemplateDTO
        {
            get
            {
                return receiptPrintTemplateDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of Receipt Print Templates
    /// </summary>
    public class ReceiptPrintTemplateListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ReceiptPrintTemplateListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Printer list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Printer list</returns>
        public List<ReceiptPrintTemplateDTO> GetReceiptPrintTemplateDTOList(List<KeyValuePair<ReceiptPrintTemplateDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ReceiptPrintTemplateDataHandler receiptPrintTemplateDataHandler = new ReceiptPrintTemplateDataHandler(sqlTransaction);
            List<ReceiptPrintTemplateDTO> returnValue = receiptPrintTemplateDataHandler.GetReceiptPrintTemplateList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }

    /// <summary>
    /// Convert Columns Col1Data, Col1Alignment etc
    /// to rows. Have maximum 5 rows
    /// </summary>
    public class ReceiptColumnData
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int sequence;
        private string data;
        private string alignment;
        public ReceiptColumnData()
        {
            log.LogMethodEntry();
            sequence = -1;
            data = string.Empty;
            alignment = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Prameterized constructor
        /// </summary>
        /// <param name="sequence">sequence</param>
        /// <param name="data">data</param>
        /// <param name="alignment">alignment</param>
        public ReceiptColumnData(int sequence, string data, string alignment)
        {
            log.LogMethodEntry(sequence, data, alignment);
            this.sequence = sequence;
            this.data = data;
            this.alignment = alignment;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method for Sequence
        /// </summary>
        public int Sequence
        {
            get { return sequence; }
            set { sequence = value; }
        }

        /// <summary>
        /// Get/Set method for data
        /// </summary>
        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        /// <summary>
        /// Get/Set method for Alignment
        /// </summary>
        public string Alignment
        {
            get { return alignment; }
            set { alignment = value; }
        }
    }
}

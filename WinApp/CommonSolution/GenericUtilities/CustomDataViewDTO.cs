/********************************************************************************************
 * Project Name - CustomDataView DTO
 * Description  - Data object of CustomDataView
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
*2.140.4      24-Feb-2023   Abhishek                Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// This is the CustomDataViewDTO data object class. This acts as data holder for the CustomDataView business object
    /// </summary>
    public class CustomDataViewDTO
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by CustomDataSetID field
            /// </summary>
            CUSTOM_DATA_SET_ID,

            /// <summary>
            /// Search by APPLICABILITY field
            /// </summary>
            APPLICABILITY,
            
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME,

            /// <summary>
            /// Search by TYPE field
            /// </summary>
            TYPE,

            /// <summary>
            /// Search by VALUECHAR field
            /// </summary>
            VALUECHAR
        }

        private int customDataSetId;
        private string applicability;
        private string attributeName;
        private string type;
        private string valueChar;
        private string customDataText;
        private decimal? customDataNumber;
        private DateTime? customDataDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomDataViewDTO()
        {
            log.LogMethodEntry();
            customDataSetId = -1;
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomDataViewDTO(int customDataSetId, string applicability, string attributeName, string type, string valueChar, string customDataText,
                                 decimal? customDataNumber, DateTime? customDataDate)
            :this()
        {
            log.LogMethodEntry(customDataSetId, applicability, attributeName, type, valueChar, customDataText, customDataNumber, customDataDate);
            this.customDataSetId = customDataSetId;
            this.applicability = applicability;
            this.attributeName = attributeName;
            this.type = type;
            this.valueChar = valueChar;
            this.customDataText = customDataText;
            this.customDataNumber = customDataNumber;
            this.customDataDate = customDataDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the CustomDataSetId field
        /// </summary>
        public int CustomDataSetId
        {
            get {  return customDataSetId;} set { customDataSetId = value; }
        }

        /// <summary>
        /// Get/Set method of the Applicability field
        /// </summary>
        public string Applicability
        {
            get { return applicability; }
            set { applicability = value; }
        }

        /// <summary>
        /// Get/Set method of the AttributeName field
        /// </summary>
        public string AttributeName
        {
            get { return attributeName; }
            set { attributeName = value; }
        }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Get/Set method of the ValueChar field
        /// </summary>
        public string ValueChar
        {
            get { return valueChar; }
            set { valueChar = value; }
        }


        /// <summary>
        /// Get/Set method of the CustomDataText field
        /// </summary>
        public string CustomDataText
        {
            get { return customDataText; }
            set { customDataText = value; }
        }

        /// <summary>
        /// Get/Set method of the CustomDataNumber field
        /// </summary>
        public decimal? CustomDataNumber
        {
            get { return customDataNumber; }
            set { customDataNumber = value; }
        }

        /// <summary>
        /// Get/Set method of the CustomDataDate field
        /// </summary>
        public DateTime? CustomDataDate
        {
            get { return customDataDate; }
            set { customDataDate = value; }
        }
    }
}

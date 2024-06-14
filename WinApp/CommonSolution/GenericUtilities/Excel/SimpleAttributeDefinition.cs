/********************************************************************************************
 * Project Name - SimpleAttributeDefinition
 * Description  - Data object of SimpleAttributeDefinition
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.60.2      12-Jun-2019      Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.2        10-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's.
 **********************************************************************************************/
using System;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Simple Attribute Definition class
    /// </summary>
    [Serializable]
    public class SimpleAttributeDefinition : AttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Name of the csv header column
        /// </summary>
        protected string displayName;
        /// <summary>
        /// converts the object from/to string representation
        /// </summary>
        protected ValueConverter valueConverter;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="displayName"></param>
        /// <param name="valueConverter"></param>
        public SimpleAttributeDefinition(string fieldName, string displayName, ValueConverter valueConverter) : base(fieldName)
        {
            log.LogMethodEntry(fieldName, displayName, valueConverter);
            this.valueConverter = valueConverter;
            this.displayName = displayName;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SimpleAttributeDefinition() : base()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of valueConverter field
        /// </summary>
        public ValueConverter ValueConverter
        {
            get
            {
                return valueConverter;
            }

            set
            {
                valueConverter = value;
            }
        }

        /// <summary>
        /// Get/Set method of displayName field
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return displayName;
            }

            set
            {
                displayName = value;
            }
        }

        /// <summary>
        /// Build the header row
        /// </summary>
        /// <param name="headerRow"></param>
        public override void BuildHeaderRow(Row headerRow)
        {
            log.LogMethodEntry(headerRow);
            headerRow.AddCell(new Cell(displayName));
            log.LogMethodExit();
        }

        /// <summary>
        /// Deserializes string to a field value
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="row"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public override object Deserialize(Row headerRow, Row row, ref int currentIndex)
        {
            log.LogMethodEntry(headerRow ,row , currentIndex);
            object result = null;
            if (displayName == headerRow[currentIndex].Value)
            {
                result = valueConverter.FromString(row[currentIndex].Value);
                currentIndex++;
            }
            else
            {
                log.Error("Invalid header definition");
                log.LogMethodExit(null , "Throwing Exception");
                throw new Exception("Invalid header definition. please check the template");
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// serializes a field to a cell value
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        public override void Serialize(Row row, object value)
        {
            log.LogMethodEntry(row, value);
            row.AddCell(new Cell(valueConverter.ToString(value)));
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the header row
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="currentIndex"></param>
        public override void ValidateHeaderRow(Row headerRow, ref int currentIndex)
        {
            log.LogMethodEntry(headerRow, currentIndex);
            if (displayName != headerRow[currentIndex].Value)
            {
                log.Error("Invalid header column");
                log.LogMethodExit(null, "Throwing Exception");
                throw new Exception("Invalid header column '" + headerRow[currentIndex].Value + "' expected column '" + displayName + "'");
            }
            currentIndex++;
        }
    }
}

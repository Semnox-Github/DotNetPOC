/********************************************************************************************
 * Project Name - AttributeDefinition
 * Description  - Data object of AttributeDefinition
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.60.2      12-Jun-2019      Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.2        10-Aug-2019      Girish kundar      Modified :Added Logger methods and Removed Unused namespace's.
 **********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Attribute definition
    /// </summary>
    [XmlInclude(typeof(SimpleAttributeDefinition))]
    [XmlInclude(typeof(ComplexAttributeDefinition))]
    [XmlInclude(typeof(ListAttributeDefinition))]
    [Serializable]
    public abstract class AttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// field name
        /// </summary>
        protected string fieldName;

        /// <summary>
        /// Configures the attribute definition
        /// </summary>
        /// <param name="templateObject"></param>
        public virtual void Configure(object templateObject)
        {
            log.LogMethodEntry("Method - Configure(templateObject)");
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public AttributeDefinition()
        {
            log.LogMethodEntry("Empty Constructor - AttributeDefinition(templateObject)");
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="fieldName"></param>
        public AttributeDefinition(string fieldName)
        {
            log.LogMethodEntry(fieldName);
            this.fieldName = fieldName;
            log.LogMethodExit();
        }

        /// <summary>
        /// builds header row
        /// </summary>
        /// <param name="headerRow"></param>
        public abstract void BuildHeaderRow(Row headerRow);

        /// <summary>
        /// Get Method for the fieldName field
        /// </summary>
        public string FieldName
        {
            get
            {
                return fieldName;
            }

            set
            {
                fieldName = value;
            }
        }

        /// <summary>
        /// Validates the header row
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="currentIndex"></param>
        public abstract void ValidateHeaderRow(Row headerRow, ref int currentIndex);


        /// <summary>
        /// Get Method for displayNameField
        /// </summary>
        public abstract string DisplayName
        {
            get; set;
        }

        /// <summary>
        /// Generic overloaded function deserializes entire sheet and returns the list of object of 
        /// type specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public List<T> Deserialize<T>(Sheet sheet) where T : class
        {
            log.LogMethodEntry();
            if (sheet == null || sheet.Rows.Count == 0)
            {
                log.Error("Error occurred at Deserialize() method");
                throw new Exception("Invalid sheet. Sheet is empty.");
            }
            int currentIndex = 0;
            Row headerRow = sheet[0];
            try
            {
                ValidateHeaderRow(headerRow, ref currentIndex);
            }
            catch (Exception ex)
            {
                Row templateHeaderRow = new Row();
                BuildHeaderRow(templateHeaderRow);
                throw new Exception(ex.Message +
                                    Environment.NewLine +
                                    "Columns in the file: " +
                                    Environment.NewLine +
                                    string.Join(", ", headerRow.Cells.Select(x => x.Value)) +
                                    Environment.NewLine +
                                    "Expected columns: " +
                                    Environment.NewLine +
                                    string.Join(", ", templateHeaderRow.Cells.Select(x => x.Value)), ex);
            }
            List<T> result = new List<T>();
            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                currentIndex = 0;
                try
                {
                    T instance = Deserialize<T>(headerRow, sheet.Rows[i], ref currentIndex);
                    result.Add(instance);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred at Deserialize() method",ex);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw new Exception("row: " + (i + 1) + " column: " + (currentIndex + 1) + " (" + headerRow[currentIndex].Value + ")" +
                                        Environment.NewLine + ex.Message, ex);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Generic Overloaded function. returns the object of the type specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="headerRow"></param>
        /// <param name="row"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public T Deserialize<T>(Row headerRow, Row row, ref int currentIndex) where T : class
        {
            log.LogMethodEntry(headerRow, row,  currentIndex);
            object deserializedObject = Deserialize(headerRow, row, ref currentIndex);
            T instance = deserializedObject as T;
            if (instance == null)
            {
                log.Error("Error occurred : Unable to type cast the deserialized object to " + typeof(T).Name);
                throw new Exception("Unable to type cast the deserialized object to " + typeof(T).Name);
            }
            log.LogMethodExit();
            return instance;
        }

        /// <summary>
        /// Deserializes the row
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="row"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public abstract object Deserialize(Row headerRow, Row row, ref int currentIndex);

        /// <summary>
        /// Serializes the row
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        public abstract void Serialize(Row row, object value);


        /// <summary>
        /// Returns the serialized sheet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="attributeDefinition"></param>
        /// <returns></returns>
        public Sheet GetSheet<T>(List<T> list)
        {
            log.LogMethodEntry(list);
            Sheet sheet = new Sheet();
            Row headerRow = new Row();
            BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);
            foreach (var item in list)
            {
                Row row = new Row();
                sheet.AddRow(row);
                Serialize(row, item);
            }
            log.LogMethodExit();
            return sheet;
        }

    }
}

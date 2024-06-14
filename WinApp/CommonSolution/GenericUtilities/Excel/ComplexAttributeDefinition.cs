/********************************************************************************************
 * Project Name - ComplexAttributeDefinition
 * Description  - Data object of ComplexAttributeDefinition
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
using System.Collections.Generic;
using System.Reflection;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Complex Attribute Definition class
    /// </summary>
    [Serializable]
    public class ComplexAttributeDefinition : AttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// child property definition
        /// </summary>
        protected List<AttributeDefinition> attributeDefinitionList;

        /// <summary>
        /// type of the complex class
        /// </summary>
        protected Type classType;

        /// <summary>
        /// display header rows
        /// </summary>
        protected bool displayHeaderRows = true;

        /// <summary>
        /// whether the definition is configured
        /// </summary>
        protected bool isConfigured = false;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="type"></param>
        /// <param name="attributeDefinitionList"></param>
        public ComplexAttributeDefinition(string fieldName, Type type, List<AttributeDefinition> attributeDefinitionList) : base(fieldName)
        {
            log.LogMethodEntry(fieldName, type, attributeDefinitionList);
            this.attributeDefinitionList = attributeDefinitionList;
            this.classType = type;
            log.LogMethodExit();
        }
        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="type"></param>
        public ComplexAttributeDefinition(string fieldName, Type type) : this(fieldName, type, new List<AttributeDefinition>())
        {
            log.LogMethodEntry(fieldName, type);
            log.LogMethodExit();
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public ComplexAttributeDefinition()
        {
            log.LogMethodEntry();
            attributeDefinitionList = new List<AttributeDefinition>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Add child property attribute definition
        /// </summary>
        /// <param name="attributeDefinition"></param>
        public void AddAttributeDefinition(AttributeDefinition attributeDefinition)
        {
            log.LogMethodEntry(attributeDefinition);
            attributeDefinitionList.Add(attributeDefinition);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method for display name field
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return attributeDefinitionList[0].DisplayName;
            }

            set
            {
                //ignore
            }
        }
        /// <summary>
        /// Configure complex attibute definition
        /// </summary>
        /// <param name="templateObject"></param>
        public override void Configure(object templateObject)
        {
            log.LogMethodEntry(templateObject);
            if (templateObject != null)
            {
                foreach (var attributeDefinition in attributeDefinitionList)
                {
                    if (!string.IsNullOrEmpty(attributeDefinition.FieldName))
                    {
                        object fieldValue = templateObject.GetType().GetProperty(attributeDefinition.FieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).GetValue(templateObject);
                        attributeDefinition.Configure(fieldValue);
                    }
                }
            }
            else
            {
                SetDisplayHeaderRows(false);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// set display header rows
        /// </summary>
        /// <param name="value"></param>
        protected void SetDisplayHeaderRows(bool value)
        {
            log.LogMethodEntry(value);
            if (value == false && isConfigured == false)
            {
                displayHeaderRows = value;
            }
            else if (value)
            {
                displayHeaderRows = value;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// build header row
        /// </summary>
        /// <param name="headerRow"></param>
        public override void BuildHeaderRow(Row headerRow)
        {
            log.LogMethodEntry(headerRow);
            if (displayHeaderRows)
            {
                foreach (var attributeDefinitionItem in attributeDefinitionList)
                {
                    attributeDefinitionItem.BuildHeaderRow(headerRow);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// deserializes the row
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="row"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public override object Deserialize(Row headerRow, Row row, ref int currentIndex)
        {
            log.LogMethodEntry(headerRow , row, currentIndex);
            object result = null;
            if (headerRow.Cells.Count > currentIndex &&
                DisplayName == headerRow[currentIndex].Value)
            {
                result = classType.GetConstructor(new Type[] { }).Invoke(new object[] { });
                int startIndex = currentIndex;
                foreach (var attributeDefinition in attributeDefinitionList)
                {
                    object value = attributeDefinition.Deserialize(headerRow, row, ref currentIndex);
                    PropertyInfo propertyInfo = classType.GetProperty(attributeDefinition.FieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    if (propertyInfo == null)
                    {
                        log.Error("propertyInfo is null");
                        throw new Exception(classType.Name + " doesn't contain a property named " + attributeDefinition.FieldName);
                    }
                    propertyInfo.SetValue(result, value);
                }
                int endIndex = currentIndex;
                if (startIndex < endIndex)
                {
                    bool foundData = false;
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        if (string.IsNullOrWhiteSpace(row[i].Value) == false)
                        {
                            foundData = true;
                        }
                    }
                    if (foundData == false)
                    {
                        result = null;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// serialize the value
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        public override void Serialize(Row row, object value)
        {
            log.LogMethodEntry(row ,value);
            if (value != null)
            {
                foreach (var attributeDefinition in attributeDefinitionList)
                {
                    PropertyInfo propertyInfo = classType.GetProperty(attributeDefinition.FieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    attributeDefinition.Serialize(row, propertyInfo.GetValue(value));
                }
            }
            else if (displayHeaderRows)
            {
                foreach (var attributeDefinition in attributeDefinitionList)
                {
                    PropertyInfo propertyInfo = classType.GetProperty(attributeDefinition.FieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    attributeDefinition.Serialize(row, null);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validates the header row
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="currentIndex"></param>
        public override void ValidateHeaderRow(Row headerRow, ref int currentIndex)
        {
            log.LogMethodEntry(headerRow , currentIndex);
            if (headerRow.Cells.Count > currentIndex)
            {
                foreach (var attributeDefinition in attributeDefinitionList)
                {
                    attributeDefinition.ValidateHeaderRow(headerRow, ref currentIndex);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method for ClassType field
        /// Converts to string and creates from type 
        /// Used for xml serialization
        /// </summary>
        public string ClassTypeString
        {
            get
            {
                return classType.FullName + ", " + classType.Assembly.GetName().Name;
            }
            set
            {
                classType = Type.GetType(value);
            }
        }

        /// <summary>
        /// Get/Set method for the attributeDefinitionList field
        /// </summary>
        public List<AttributeDefinition> AttributeDefinitionList
        {
            get
            {
                return attributeDefinitionList;
            }
            set
            {
                attributeDefinitionList = value;
            }
        }
    }
}

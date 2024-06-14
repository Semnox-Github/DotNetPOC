/********************************************************************************************
 * Project Name - ListAttributeDefinition
 * Description  - Data object of ListAttributeDefinition
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
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// List Attribute Definition class
    /// </summary>
    [Serializable]
    public class ListAttributeDefinition : AttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Attribute definition of the list item
        /// </summary>
        protected AttributeDefinition attributeDefinition;
        /// <summary>
        /// Type of the list item
        /// </summary>
        protected Type classType;
        /// <summary>
        /// No of items in the list
        /// </summary>
        protected int itemCount;
        /// <summary>
        /// Whether the object is configured using header row
        /// </summary>
        protected bool isConfigured = false;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ListAttributeDefinition() : base()
        {
            log.LogMethodEntry();
            itemCount = 1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="classType"></param>
        /// <param name="attributeDefinition"></param>
        public ListAttributeDefinition(string fieldName, Type classType, AttributeDefinition attributeDefinition) : base(fieldName)
        {
            log.LogMethodEntry(fieldName, classType, attributeDefinition);
            this.attributeDefinition = attributeDefinition;
            this.classType = classType;
            itemCount = 1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set Method for itemCount Field
        /// </summary>
        public int ItemCount
        {
            get
            {
                return itemCount;
            }
            set
            {
                itemCount = value;
            }
        }

        /// <summary>
        /// Get/Set Method for attribute definition field
        /// </summary>
        public AttributeDefinition AttributeDefinition
        {
            get
            {
                return attributeDefinition;
            }

            set
            {
                attributeDefinition = value;
            }
        }
        /// <summary>
        /// Get/Set Method for classType Field
        /// </summary>
        [XmlIgnore]
        public Type ClassType
        {
            get
            {
                return classType;
            }

            set
            {
                classType = value;
            }
        }
        /// <summary>
        /// Get/Set Method for classType
        /// Used for serialization
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
        /// Get/Set Method for displayNameField
        /// </summary>
        public override string DisplayName
        {
            get
            {
                return attributeDefinition == null ? "" : attributeDefinition.DisplayName;
            }

            set
            {
                //ignore
            }
        }

        /// <summary>
        /// Configures the item count
        /// </summary>
        /// <param name="templateObject"></param>
        public override void Configure(object templateObject)
        {
            log.LogMethodEntry(templateObject);
            if (templateObject != null)
            {
                IList list = templateObject as IList;
                if (list != null && list.Count > 0)
                {
                    SetItemCount(list.Count);
                }
                else
                {
                    SetItemCount(0);
                }
                foreach (var listItem in list)
                {
                    attributeDefinition.Configure(listItem);
                }
            }
            else
            {
                SetItemCount(0);
            }
            isConfigured = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Sets the item count
        /// </summary>
        /// <param name="value"></param>
        protected void SetItemCount(int value)
        {
            log.LogMethodEntry(value);
            if (value < itemCount && isConfigured == false)
            {
                itemCount = value;
            }
            else if (value > itemCount)
            {
                itemCount = value;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the header row
        /// </summary>
        /// <param name="headerRow"></param>
        public override void BuildHeaderRow(Row headerRow)
        {
            log.LogMethodEntry(headerRow);
            for (int i = 0; i < itemCount; i++)
            {
                attributeDefinition.BuildHeaderRow(headerRow);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Deserializes the row to a list of classType objects
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="row"></param>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        public override object Deserialize(Row headerRow, Row row, ref int currentIndex)
        {
            log.LogMethodEntry(headerRow , row , currentIndex);
            object result = null;
            Type listType = typeof(List<>).MakeGenericType(classType);
            IList list = (IList)Activator.CreateInstance(listType);

            while (headerRow.Cells.Count > currentIndex && DisplayName == headerRow[currentIndex].Value)
            {
                object listItem = null;
                listItem = attributeDefinition.Deserialize(headerRow, row, ref currentIndex);
                if (listItem != null)
                {
                    list.Add(listItem);
                }
            }
            if (list.Count > 0)
            {
                result = list;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Serializes a list of classType objectcs to excelrow
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        public override void Serialize(Row row, object value)
        {
            log.LogMethodEntry(row, value);
            List<object> list = new List<object>();
            if (value != null && value is IList)
            {
                foreach (var item in (value as IList))
                {
                    list.Add(item);
                }
            }
            for (int i = 0; i < itemCount; i++)
            {
                if (list != null && list.Count > i)
                {
                    attributeDefinition.Serialize(row, list[i]);
                }
                else
                {
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
            while (headerRow.Cells.Count > currentIndex && DisplayName == headerRow[currentIndex].Value)
            {
                attributeDefinition.ValidateHeaderRow(headerRow, ref currentIndex);
            }
            log.LogMethodExit();
        }
    }
}

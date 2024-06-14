/********************************************************************************************
 * Project Name - ValueConverter
 * Description  - Data object of ValueConverter
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.60.2      12-Jun-2019      Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.2        10-Aug-2019      Girish kundar       Modified : Removed Unused namespace's.
 **********************************************************************************************/
using System;
using System.Xml.Serialization;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// abstract class for value converters
    /// </summary>
    [XmlInclude(typeof(BooleanValueConverter))]
    [XmlInclude(typeof(DateTimeValueConverter))]
    [XmlInclude(typeof(DecimalValueConverter))]
    [XmlInclude(typeof(DoubleValueConverter))]
    [XmlInclude(typeof(EnumValueConverter))]
    [XmlInclude(typeof(ForeignKeyValueConverter))]
    [XmlInclude(typeof(IntValueConverter))]
    [XmlInclude(typeof(NullableIntValueConverter))]
    [XmlInclude(typeof(NullableDateTimeValueConverter))]
    [XmlInclude(typeof(NullableDoubleValueConverter))]
    [XmlInclude(typeof(StringValueConverter))]
    [XmlInclude(typeof(NullableDecimalValueConverter))]
    [XmlInclude(typeof(NullableBooleanValueConverter))]
    [Serializable]
    public abstract class ValueConverter
    {
        /// <summary>
        /// converts the object to string representation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract string ToString(object value);
        /// <summary>
        /// creates the object from string representation
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public abstract object FromString(string stringValue);
    }
}

/********************************************************************************************
 * Project Name - GenericAsset                                                                         
 * Description  - Data object of StringValueConverter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70       30-Oct-2019   Rakesh Kumar    Created  
 *2.80       10-May-2020   Girish Kundar   Modified: REST API Changes merge from WMS  
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    class AssetTrueValueConverter : ValueConverter
    {
        /// <summary>
        /// Returns the string value from string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            string result = string.Empty;
            if (stringValue.ToString() == "True")
            {
                result = "Y";
            }
            else if (stringValue.ToString() == "False")
            {
                result = "N";
            }
            else
            {
                throw new Exception("Invalid parameter please pass valid value: " + stringValue == null ? "null" : stringValue);
            }
            return result;
        }

        /// <summary>
        /// Converts string value to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            string result = string.Empty;
            if ((bool)value)
            {
                result="True";
            }
            else if (!(bool)value)
            {
                result="False";
            }
            else
            {
                throw new Exception("Invalid parameter please pass valid value: " + value == null ? "null" : value.ToString());
            }
            return result;
        }
    }
}

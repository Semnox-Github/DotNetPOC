/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - Value converter class for ReaderTypes
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       16-Mar-2019   Muhammed Mehraj  Created    
 *2.70.2       12-Aug-2019   Deeksha          Adde logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Game
{
    class ReaderTypeValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected Dictionary<int, string> readerTypesModeIDList;
        protected Dictionary<int, string> readerTypesModeNameList;
        /// <summary>
        /// Paramertized constructor
        /// </summary>
        public ReaderTypeValueConverter()
        {
            log.LogMethodEntry();
            readerTypesModeIDList = new Dictionary<int, string>();
            readerTypesModeNameList= new Dictionary<int, string>();
            Dictionary<int, string> values = new Dictionary<int, string>
                        {
                            {-1, "Default" },
                            {0, "2 Line" },
                            {1, "Color ISM" },
                            { 2, "Wifi" }
                        };
            foreach (var item in values)
            {
                readerTypesModeIDList.Add(item.Key, item.Value);
                readerTypesModeNameList.Add(item.Key, item.Value);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts readertype name to id
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int readertype = 0;
            if (readerTypesModeIDList != null && !string.IsNullOrWhiteSpace(stringValue))
            {
                readertype = readerTypesModeIDList.FirstOrDefault(x => x.Value == stringValue).Key;
            }
            else
            {
                readertype = readerTypesModeIDList.FirstOrDefault().Key;
            }
            log.LogMethodExit(readertype);
            return readertype;
        }
        /// <summary>
        /// Converts readertype id to name
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string readername = string.Empty;
            if (readerTypesModeNameList != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                readername = readerTypesModeNameList.FirstOrDefault(m => m.Key == Convert.ToInt32(value)).Value;
            }
            log.LogMethodExit(readername);
            return readername;

        }
    }
}

/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Helper class to create search parameters for data handlers.  
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By              Remarks          
 *********************************************************************************************
 2.140.0      17-Nov-2021       Lakshminarayana            Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Helper class to create search parameters for data handlers.
    /// </summary>
    public class SearchParameterList<T> : List<KeyValuePair<T, string>> where T: struct, IConvertible
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Add(T searchParameter, string value)
        {
            log.LogMethodEntry(searchParameter, value);
            if (string.IsNullOrWhiteSpace(value))
            {
                log.LogMethodExit("value is empty");
                return;
            }
            Add(new KeyValuePair<T, string>(searchParameter, value));
            log.LogMethodExit();
        }

        public void Add(T searchParameter, int? value) 
        {
            log.LogMethodEntry(searchParameter, value);
            if (value.HasValue == false)
            {
                log.LogMethodExit("value is empty");
                return;
            }
            Add(new KeyValuePair<T, string>(searchParameter, value.Value.ToString()));
            log.LogMethodExit();
        }

        public void Add(T searchParameter, int value, bool foreignKeyColumn = true)
        {
            if(foreignKeyColumn && value <= -1)
            {
                log.LogMethodExit("value <= -1");
                return;
            }
            Add(new KeyValuePair<T, string>(searchParameter, value.ToString()));
            log.LogMethodExit();
        }

        public void Add(T searchParameter, DateTime? value)
        {
            if (value.HasValue == false)
            {
                log.LogMethodExit("value.HasValue == false");
                return;
            }
            Add(new KeyValuePair<T, string>(searchParameter, value.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            log.LogMethodExit();
        }

        public void Add(T searchParameter, decimal? value)
        {
            if (value.HasValue == false)
            {
                log.LogMethodExit("value.HasValue == false");
                return;
            }
            Add(new KeyValuePair<T, string>(searchParameter, value.Value.ToString()));
            log.LogMethodExit();
        }

        public void Add(T searchParameter, DateTime value)
        {
            if (value == DateTime.MinValue || value == DateTime.MaxValue)
            {
                log.LogMethodExit("value == DateTime.MinValue || value == DateTime.MaxValue");
                return;
            }
            Add(new KeyValuePair<T, string>(searchParameter, value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            log.LogMethodExit();
        }

        public void Add(T searchParameter, bool value, 
                        bool includeOnlyIfTrue = false, 
                        string trueValue = "1", string falseValue= "0")
        {
            if (value == false && includeOnlyIfTrue)
            {
                log.LogMethodExit("value == false && includeOnlyIfTrue");
                return;
            }
            Add(new KeyValuePair<T, string>(searchParameter, value? trueValue : falseValue));
            log.LogMethodExit();
        }
    }
}

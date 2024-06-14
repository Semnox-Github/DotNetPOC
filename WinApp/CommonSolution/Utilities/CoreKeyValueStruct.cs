/********************************************************************************************
 * Project Name - UsersDTO
 * Description  - UserValueStruct object of user
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        30-Jun-2016   Rakshith          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Summary description for CoreKeyValueStruct
    /// </summary>
    ///
    public class CoreKeyValueStruct
    {
        private string _key;
        private string _val;

        /// <summary>
        /// Get/Set method of the Key field
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }
        /// <summary>
        /// Get/Set method of the Value field
        /// </summary>
        public string Value
        {
            get { return _val; }
            set { _val = value; }
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        public CoreKeyValueStruct()
        {
        }
        /// <summary>
        ///  Constructor with
        /// </summary> parameter
        public CoreKeyValueStruct(string key, string value)
        {
            _key = key;
            _val = value;
        }
    }
}

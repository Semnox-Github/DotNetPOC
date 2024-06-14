/********************************************************************************************
 * Project Name - ClientValidationStruct Program
 * Description  - Data object of the ClientValidationStruct
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        09-May-2016   Rakshith           Created   
 ---------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Semnox.Parafait.ClientApp
{
    /// <summary>
    /// Summary description for ClientValidationStruct
    /// </summary>
    public class ClientValidationStruct
    {
            private string _key;
            private string _val;

            /// <summary>
            /// default Constructor
            /// </summary>
            public ClientValidationStruct()
            {
            }
            /// <summary>
            /// Parameterized Constructor
            /// </summary>
            public ClientValidationStruct(string key, string value)
            {
                _key = key;
                _val = value;
            }

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
           
         
    }
}
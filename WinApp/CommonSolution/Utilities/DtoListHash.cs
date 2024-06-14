/********************************************************************************************
 * Project Name - Utilities  Class
 * Description  - DtoListHash class to get the Hash values from DTO
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Lakshmi Narayan             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class DtoListHash : ValueObject
    {
        private static readonly ConcurrentDictionary<string, List<PropertyInfo>> typeProperyInfoMap = new ConcurrentDictionary<string, List<PropertyInfo>>(); 
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string value;

        public DtoListHash(IEnumerable<object> dtoList)
        {
            log.LogMethodEntry(dtoList);
            string serializedString = JsonConvert.SerializeObject(dtoList);
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(serializedString));
            ByteArray byteArray = new ByteArray(hash);
            value = byteArray.ToString();
            log.LogMethodExit();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return value;
        }

        public string Value
        {
            get
            {
                return value;
            }
        }

        public static implicit operator string(DtoListHash hash)  {  return hash.value;  }
    }
}

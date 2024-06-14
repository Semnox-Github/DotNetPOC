using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Semnox.CommonAPI.Helpers
{
    public class ExceptionSerializer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public static string Serialize(Exception ex)
        {
            log.LogMethodEntry(ex);
            string result;
            Exception tempEx = null;
            log.Debug("Caught exception of type " + ex.GetType().ToString());
            if (ex.GetType() == typeof(System.Data.SqlClient.SqlException))
            {
                log.Debug("Stripping sql exception " + ex);
                tempEx = new Exception("Something went wrong. Please try after sometime");
            }
            else
            {
                log.Debug("not a sql exception");
                tempEx = new Exception(ex.Message);
            }
            result = JsonConvert.SerializeObject(tempEx);
            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["DEBUG_MODE"]) ||  ConfigurationManager.AppSettings["DEBUG_MODE"].ToLower() != "true")
            {
                JObject jObject = (JObject)JsonConvert.DeserializeObject(result);
                jObject["StackTraceString"] = string.Empty;
                result = JsonConvert.SerializeObject(jObject);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
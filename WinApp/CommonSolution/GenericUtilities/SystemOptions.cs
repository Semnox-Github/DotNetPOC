using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    class SystemOptions
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<KeyValuePair<string, string>> OptionsList = new List<KeyValuePair<string, string>>();

         Semnox.Core.Utilities.Utilities  _utilities;
        public SystemOptions( Semnox.Core.Utilities.Utilities  Utilities)
        {
            log.LogMethodEntry(Utilities);
            _utilities = new Semnox.Core.Utilities.Utilities();

            DataTable dtSysOptions = _utilities.executeDataTable("select OptionName, OptionValue from SystemOptions");

            foreach (DataRow dr in dtSysOptions.Rows)
            {
                string value;
                if (dr["OptionValue"] == DBNull.Value)
                    value = "";
                else
                {
                    byte[] data = EncryptionAES.Decrypt(dr["OptionValue"] as byte[], StaticUtils.getKey("SYS"));
                    value = Encoding.UTF8.GetString(data);
                }
                KeyValuePair<string, string> kv = new KeyValuePair<string, string>(dr["OptionName"].ToString(), value);
                OptionsList.Add(kv);
            }
            log.LogMethodExit(null);
        }

        public string GetOptionValue(string Option)
        {
            log.LogMethodEntry(Option);
            string returnvalue= (OptionsList.Find(delegate (KeyValuePair<string, string> kv) { return (kv.Key == Option); }).Value);
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        public void UpdateOptionValue(string Option, string Value)
        {
            log.LogMethodEntry(Option, Value);
            object value;
            if (string.IsNullOrEmpty(Value))
                value = DBNull.Value;
            else
                value = EncryptionAES.Encrypt(Encoding.UTF8.GetBytes(Option), StaticUtils.getKey("SYS"));
            log.LogVariableState("@name", Option);
            log.LogVariableState("@value", value);
            if (_utilities.executeNonQuery("update systemOptions set OptionValue = @value where OptionName = @name",
                                            new SqlParameter("@name", Option),
                                            new SqlParameter("@value", value)) == 0)
            {
                _utilities.executeNonQuery("insert into SystemOptions (OptionName, OptionValue) values (@name, @value)",
                                            new SqlParameter("@name", Option),
                                            new SqlParameter("@value", value));
                log.LogVariableState("@name", Option);
                log.LogVariableState("@value", value);
            }
            log.LogMethodExit(null);
        }
    }
}

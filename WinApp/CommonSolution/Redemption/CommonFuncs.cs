/********************************************************************************************
 * Class Name - RedemptionUtils                                                                         
 * Description - Common Functions
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Semnox.Parafait.Redemption
{
    public class CommonFuncs
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Utilities Utilities;
        public static ParafaitEnv ParafaitEnv;
        public static void setSiteIdFilter(BindingSource bs)
        {
            log.LogMethodEntry();
            try
            {
                //if (Utilities.ParafaitEnv.IsCorporate) // corporate db
                if (ParafaitEnv.IsCorporate) // corporate db
                {
                    if (bs.Filter == null || bs.Filter == "")
                        bs.Filter = "site_id = " + ParafaitEnv.SiteId.ToString();
                    else
                        bs.Filter = "(" + bs.Filter + ") and site_id = " + ParafaitEnv.SiteId.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing setSiteIdFilter()" + ex.Message);
                MessageBox.Show(ex.Message, "Binding Source: " + bs.DataMember);
            }
            log.LogMethodExit();
        }

        public static object getSiteid()
        {
            log.LogMethodEntry();
            if (ParafaitEnv.IsCorporate)
            {
                log.LogMethodExit(ParafaitEnv.SiteId);
                return ParafaitEnv.SiteId;
            }
            else
            {
                log.LogMethodExit();
                return DBNull.Value;
            }
            //return -1;
        }

        public static void displayInfo(string info) // display in info labl of toolstrip
        {
            log.LogMethodEntry(info);
            try
            {
                Form f = Application.OpenForms["ParentMDI"];
                if (f != null)
                {
                    Control[] ctrls = f.Controls.Find("fillBy3ToolStrip", true);
                    if (ctrls.Length > 0)
                    {
                        ToolStrip st = ctrls[0] as ToolStrip;
                        ToolStripItem lbl = st.Items["toolStripLabelMessage"];
                        lbl.Text = info;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing displayInfo()" + ex.Message);
            }
          log.LogMethodExit(DBNull.Value);
        }

        public string getNextSeqNo(string sequenceName, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(sequenceName, SQLTrx);
            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = @"declare @value varchar(20)
                                exec GetNextSeqValue N'" + sequenceName + "', @value out, -1 "
                                + " select @value";
            try
            {
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    log.LogMethodExit(o);
                    return (o.ToString());
                }
                else
                {
                    log.LogMethodExit("-1");
                    return "-1";
                }
            }
            catch
            {
                log.LogMethodExit("-1");
                return "-1";
            }
        }

    }
}

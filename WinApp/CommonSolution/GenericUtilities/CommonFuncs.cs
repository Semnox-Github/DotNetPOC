/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - CommonFuncs 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Modified logger methods.
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    public class CommonFuncs
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Semnox.Core.Utilities.Utilities Utilities;
        public CommonFuncs(Semnox.Core.Utilities.Utilities ParafaitUtilities)
        {
            log.LogMethodEntry(ParafaitUtilities);

            Utilities = ParafaitUtilities;

            log.LogMethodExit(null);
        }

        public bool checkForExceptionProduct(string cardNumber, int productId)
        {
            log.LogMethodEntry(cardNumber, productId);

            if (Utilities.executeScalar(@"select top 1 1 
                                        where not exists
                                            (select 1 from
                                            CardTypeRule ct
                                            where product_id = @ProductId
                                            and ct.active = 'Y')
                                        OR Exists(
                                            select 1 from
                                            CardTypeRule ct, cards c left outer join customers cu on c.customer_id = cu.customer_id
                                            where c.Card_Number = @CardNumber
                                            and c.valid_flag = 'Y'
                                            and ct.active = 'Y'
                                            and ct.disAllowed = 'N'
                                            and ct.product_id = @ProductId
                                            --and isnull(ct.cardTypeId, -1) = isnull(c.cardTypeId, -1)
                                            and isnull(ct.membershipId, -1) = ISNULL(cu.membershipId, -1)
                                        ) ",
                            new SqlParameter("@CardNumber", cardNumber),
                            new SqlParameter("@ProductId", productId)) == null)
            {
                log.LogVariableState("@CardNumber", cardNumber);
                log.LogVariableState("@ProductId", productId);

                log.LogMethodExit(false);
                return false;
            }

            else
            {
                log.LogVariableState("@CardNumber", cardNumber);
                log.LogVariableState("@ProductId", productId);

                log.LogMethodExit(true);
                return true;
            }
        }

        //public bool RecordAttendance(string CardNumber, ref string Username, int AttendanceRoleId = -1, int ApproverId = -1, string Status = null, int MachineId = -1, int POSMachineId = -1)
        //{
        //    log.LogMethodEntry(CardNumber, Username, AttendanceRoleId, ApproverId, Status, MachineId, POSMachineId);

        //    object UserId = Utilities.executeScalar(@"select User_id 
        //                                                from users 
        //                                                where exists (select 1
        //                                                                from UserIdentificationTags ut 
        //                                                          where ut.UserId = users.user_id 
        //                                                          and ut.ActiveFlag = 1
        //                                                          and getdate() between isnull(ut.StartDate, getdate()) and isnull(ut.EndDate, getdate())
        //                                                                and CardNumber = @CardNumber)",
        //                                                new SqlParameter("@CardNumber", CardNumber));

        //    log.LogVariableState("@CardNumber", CardNumber);

        //    if (UserId == null)
        //    {
        //        log.LogVariableState("Username ", Username);
        //        log.LogMethodExit(false);
        //        return false;
        //    }
        //    else
        //    {
        //        bool Value = RecordAttendance((int)UserId, Utilities.getServerTime(), "CARD", ref Username, AttendanceRoleId, ApproverId, Status, MachineId, POSMachineId);

        //        log.LogVariableState("Username ", Username);
        //        log.LogMethodExit(Value);
        //        return Value;
        //    }           
        //}

        //public bool RecordAttendance(int UserId, ref string Username)
        //{
        //    log.LogMethodEntry("UserId", "Username");

        //    log.LogVariableState("Username ", Username);

        //    bool returnValueNew = RecordAttendance((int)UserId, Utilities.getServerTime(), "FINGERPRINT", ref Username);
        //    log.LogMethodExit(returnValueNew);
        //    return returnValueNew;
        //}

        //public bool RecordAttendance(int UserId, DateTime LogTime, string Mode, ref string Username, int AttendanceRoleId = -1, int ApproverId = -1, string Status = null, int MachineId = -1, int POSMachineId = -1)
        //{
        //    log.LogMethodEntry("UserId", LogTime, Mode, "Username", AttendanceRoleId, ApproverId, Status, "MachineId", "POSMachineId");

        //    DataTable dtUser = Utilities.executeDataTable(@"select EmpNumber, Username 
        //                                                    from users 
        //                                                    where user_id = @UserId",
        //                                                    new SqlParameter("@UserId", UserId));

        //    log.LogVariableState("@UserId", UserId);

        //    if (dtUser.Rows.Count == 0 || dtUser.Rows[0][0] == DBNull.Value || string.IsNullOrEmpty(dtUser.Rows[0][0].ToString()))
        //    {
        //        log.LogVariableState("Username ", Username);
        //        log.LogMethodExit(false);
        //        return false;
        //    }

        //    int userNumber = Convert.ToInt32(dtUser.Rows[0][0]);

        //    DataSet dsAttendanceLog = new DataSet();
        //    DataTable dtAttendanceLog = dsAttendanceLog.Tables.Add();
        //    dtAttendanceLog.Columns.Add("CompanyID", typeof(String));
        //    dtAttendanceLog.Columns.Add("ReaderID", typeof(Int32));
        //    dtAttendanceLog.Columns.Add("CardID", typeof(String));
        //    dtAttendanceLog.Columns.Add("UserNumber", typeof(Int32));
        //    dtAttendanceLog.Columns.Add("Type", typeof(String));
        //    dtAttendanceLog.Columns.Add("Mode", typeof(String));
        //    dtAttendanceLog.Columns.Add("Offset", typeof(Int32));
        //    dtAttendanceLog.Columns.Add("TimeStamp", typeof(DateTime));
        //    dtAttendanceLog.Columns.Add("AttendanceRoleId", typeof(Int32));
        //    dtAttendanceLog.Columns.Add("AttendanceRoleApproverId", typeof(Int32));
        //    dtAttendanceLog.Columns.Add("Status", typeof(String));
        //    dtAttendanceLog.Columns.Add("MachineId", typeof(Int32));
        //    dtAttendanceLog.Columns.Add("POSMachineId", typeof(Int32));

        //    DateTime now = LogTime;
        //    //now = now.AddSeconds(-now.Second).AddMilliseconds(-now.Millisecond); Commented for Gamtime changes to include seconds in attendancelog on 14-Nov-2015
        //    dtAttendanceLog.Rows.Add(null, null, "", userNumber, "", Mode, 0, now, AttendanceRoleId, ApproverId, Status == null ? DBNull.Value : (object) Status, MachineId, POSMachineId);

        //    try
        //    {
        //        AttendanceUtils attUtils = new AttendanceUtils(Utilities);
        //        Boolean isValid = attUtils.uploadAttendenceLog(dsAttendanceLog);

        //        if (isValid)
        //        {
        //            Username = dtUser.Rows[0][1].ToString();
        //            log.LogVariableState("Username ", Username);
        //            log.LogMethodExit(true);
        //            return true;
        //        }
        //        else
        //        {
        //            log.LogVariableState("Username ", Username);
        //            log.LogMethodExit(false);
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occured when creating new AttendanceUtils", ex);
        //        Username = ex.Message;
        //        log.LogMethodExit(false);
        //        return false;
        //    }
        //}

        public void setupDataGridProperties(DataGridView dgv)
        {
            log.LogMethodEntry(dgv);
            log.LogMethodExit(null);
        }

        public int getLastTrxNoForPOS(int POSMachineId, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(POSMachineId, sqlTrx);
            // assume business day to be from 6am to 6am. get first trx id of the business day
            string CommandText = "select isnull(max(case isnumeric(Trx_No) when 1 then cast(trx_no as int) else 0 end), '0') " +
                                "from trx_header WITH (INDEX(trx_date)) " +
                                "where trxDate > " +
                                    "(select case when GETDATE() < DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) " +
                                            "then DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 1, GETDATE()))) " +
                                            "else DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) end) " +
                                "and (POSMachineId = @POSId or @POSId = -1)";
            try
            {
                int no = Convert.ToInt32(Utilities.executeScalar(CommandText, sqlTrx, new SqlParameter("@POSId", POSMachineId)));

                log.LogVariableState("@POSId", POSMachineId);

                log.LogMethodExit(no);
                return no;
            }
            catch (Exception ex)
            {
                log.Error("Unable to Execute query on POSId ", ex);
                log.LogMethodExit(0);
                return 0;
            }
        }

        public int getTodaysFirstTrxId(SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(sqlTrx);

            // assume business day to be from 6am to 6am. get first trx id of the business day
            string CommandText = "select isnull(min(TrxId), (select isnull(max(trxId), 0) + 1 from trx_header)) " +
                                "from trx_header  WITH (INDEX(trx_date))  " +
                                "where trxDate > " +
                                    "(select case when GETDATE() < DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) " +
                                            "then DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 1, GETDATE()))) " +
                                            "else DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) end)";

            int returnValue = Convert.ToInt32(Utilities.executeScalar(CommandText, sqlTrx));

            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public string getNextTrxNo(int POSMachineId, int orderTypeGroupId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(POSMachineId, orderTypeGroupId, SQLTrx);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = @"declare @value varchar(20)
                                exec GetNextSeqValue N'Transaction', @value out, " + POSMachineId.ToString() + ", " + orderTypeGroupId.ToString() + @"
                                select @value";
            try
            {
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    log.LogMethodExit(o.ToString());
                    return (o.ToString());
                }
                else
                {
                    log.LogMethodExit("");
                    return "-1";
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to execute the query! ", ex);
                log.LogMethodExit("-1");
                return "-1";
            }
        }

        public string getNextBillNo(int POSMachineId, int orderTypeGroupId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(POSMachineId, orderTypeGroupId, SQLTrx);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = @"declare @value varchar(20)
                                exec GetNextSeqValue N'BillTransaction', @value out, " + POSMachineId.ToString() + ", " + orderTypeGroupId.ToString() + @"
                                select @value";
            try
            {
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    log.LogMethodExit(o.ToString());
                    return (o.ToString());
                }
                else
                {
                    log.LogMethodExit("-1");
                    return "-1";
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to execute the query! ", ex);
                log.LogMethodExit("-1");
                return "-1";
            }
        }
        //Begin 29-Feb-2016 - function to get next Refund specific transaction number
        public string getNextCreditTrxNo(int POSMachineId, int orderTypeGroupId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(POSMachineId, orderTypeGroupId, SQLTrx);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = @"declare @value varchar(20)
                                exec GetNextSeqValue N'RefundTransaction', @value out, " + POSMachineId.ToString() + ", " + orderTypeGroupId.ToString() + @"
                                select @value";
            try
            {
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    log.LogMethodExit(o.ToString());
                    return (o.ToString());
                }
                else
                {
                    log.LogMethodExit("");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to execute query on Refund Transaction sequence! ", ex);
                log.LogMethodExit("");
                return string.Empty;
            }
        }
        //End 29-Feb-2016 - function to get next Refund specific transaction number

        public string getNextReverseTrxNo(int POSMachineId, int orderTypeGroupId, bool fullReversal, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(POSMachineId, orderTypeGroupId, SQLTrx);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            if (fullReversal)
                cmd.CommandText = @"declare @value varchar(20)
                                exec GetNextSeqValue N'VoidTransaction', @value out, " + POSMachineId.ToString() + ", " + orderTypeGroupId.ToString() + @"
                                select @value";
            else
                cmd.CommandText = @"declare @value varchar(20)
                                exec GetNextSeqValue N'ReturnTransaction', @value out, " + POSMachineId.ToString() + ", " + orderTypeGroupId.ToString() + @"
                                select @value";
            try
            {
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    log.LogMethodExit(o.ToString());
                    return (o.ToString());
                }
                else
                {
                    log.LogMethodExit("");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to execute query on Reverse Transaction sequence! ", ex);
                log.LogMethodExit("");
                return string.Empty;
            }
        }

        public string GetNextRedemptionOrderNo(int POSMachineId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(POSMachineId, SQLTrx);

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = @"declare @value varchar(20)
                                exec GetNextSeqValue N'RedemptionOrder', @value out, " + POSMachineId.ToString() + @"
                                select @value";
            try
            {
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    log.LogMethodExit(o.ToString());
                    return (o.ToString());
                }
                else
                {
                    log.LogMethodExit("-1");
                    return "-1";
                }
            }
            catch (Exception ex)
            {
                log.Error("Unable to execute query on RedemptionOrder! ", ex);
                log.LogMethodExit("-1");
                return "-1";
            }
        }

        public double RoundOff(double Amount, int RoundOffAmountTo, int RoundingPrecision, string RoundingType)
        {
            log.LogMethodEntry(Amount, RoundOffAmountTo, RoundingPrecision, RoundingType);

            switch (RoundingType)
            {
                case "ROUND":
                    {
                        double returnValue = Math.Round(Amount / RoundOffAmountTo, RoundingPrecision, MidpointRounding.AwayFromZero) * RoundOffAmountTo;

                        log.LogMethodExit(returnValue);
                        return returnValue;
                    }
                case "FLOOR":
                    {
                        double returnValue = ((int)(Amount * Math.Pow(10, RoundingPrecision) / RoundOffAmountTo) * RoundOffAmountTo) / Math.Pow(10, RoundingPrecision);

                        log.LogMethodExit(returnValue);
                        return returnValue;
                    }
                case "CEILING":
                    {
                        double lclAmount = Amount * Math.Pow(10, RoundingPrecision);
                        if (lclAmount % RoundOffAmountTo == 0)
                        {
                            log.LogMethodExit(Amount);
                            return Amount;
                        }
                        else
                        {
                            double value = (lclAmount + RoundOffAmountTo - (lclAmount % RoundOffAmountTo)) / Math.Pow(10, RoundingPrecision);
                            log.LogMethodExit(value);
                            return value;
                        }
                    }
                default:
                    {
                        log.LogMethodExit(Amount);
                        return Amount;
                    }
            }
        }

        public void CloneObject(object opSource, object opTarget)
        {
            log.LogMethodEntry(opSource, opTarget);

            //grab the type and create a new instance of that type
            Type opSourceType = opSource.GetType();

            //grab the properties
            System.Reflection.FieldInfo[] opPropertyInfo = opSourceType.GetFields();// (System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);

            //iterate over the properties and if it has a 'set' method assign it from the source TO the target
            foreach (System.Reflection.FieldInfo item in opPropertyInfo)
            {
                //if (item.CanWrite)
                {
                    //value types can simply be 'set'
                    if (item.FieldType.IsValueType || item.FieldType.IsEnum || item.FieldType.Equals(typeof(System.String)))
                    {
                        item.SetValue(opTarget, item.GetValue(opSource));
                    }
                    //object/complex types need to recursively call this method until the end of the tree is reached
                }
            }

            log.LogMethodExit();
        }
    }
}

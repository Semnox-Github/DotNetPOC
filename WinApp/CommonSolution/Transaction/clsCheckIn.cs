/********************************************************************************************
 * Project Name - Checkin
 * Description  - Business Logic to create and save Check-ins
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad Created 
 *2.70        20-Jun-2019      Mathew Ninan   Modified Check-in to consider Line for each Trx line
 *2.80        20-May-2020      Girish Kundar  Modified : Phase -1 changes for REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction
{
    public class clsCheckIn
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const string StandardTableType = "Standard";
        public const string PoolTableType = "Pool / Snooker";
        public const string KaraokeTableType = "Karaoke";
        public const string SnKInterface = "SnK";
        public const string KMTronicInterface = "KMTronic";

        public DateTime CheckInTime;
        public string PhotoFileName = "";
        public string Remarks;
        public object Finger_Print;
        public Array FP_Template;
        public Card card;
        public int CheckInTrxId;
        public int TrxLineId;
        public int CheckInFacilityId;
        public int TableId = -1;
        public int AvailableUnits;
        public int AllowedTimeInMinutes;
        public string AutoCheckOut;
        public string FacilityName;
        string PhotoDirectory;
        public int CheckedInUnits;
        public decimal EffectivePrice;
        public string TableNumber = "";
        public bool UserTime = false;

        Utilities Utilities;

        public clsCheckIn(Utilities pUtilities)
        {
            log.LogMethodEntry(pUtilities);
            Utilities = pUtilities;
            PhotoDirectory = Utilities.ParafaitEnv.CheckInPhotoDirectory;
            log.LogMethodExit(null);
        }
        
        public class checkInDetails
        {
            public int CheckInDetailId;
            public int CheckInId;
            public string Name;
            public string VehicleNumber;
            public string VehicleModel;
            public string VehicleColor;
            public DateTime DateOfBirth = DateTime.MinValue;
            public decimal Age;
            public string SpecialNeeds;
            public string Allergies;
            public string Remarks;
            public DateTime CheckOutTime;
            public int CheckOutTrxId;
            public DateTime last_update_date;
            public string last_updated_user;
            public Card detailCard;
            public int CheckInTrxLineId;
            public int CheckInTrxId;
            public int TrxLineId;
        }

        public CustomerDTO customerDTO;
        public List<checkInDetails> checkInDetailsList = new List<checkInDetails>();

        public bool SaveCheckIn(float cardFaceValue, SqlTransaction SQLTrx, ref string message)
        {
            log.LogMethodEntry(cardFaceValue, SQLTrx, message);
            try
            {
                if ((card == null) || customerDTO.Id == -1)
                {
                    CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDTO);
                    customerBL.Save(SQLTrx);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred when saving the customerDTO details", ex);
                message = "Check-In Cust Save: " + ex.Message;

                log.LogVariableState("Message ", message);
                log.LogMethodExit(false);
                return false;
            }

            SqlCommand cmd = Utilities.getCommand(SQLTrx);

            cmd.CommandText = "insert into CheckIns (customer_id, CheckInTime, PhotoFileName, " +
                                                "CardId, CheckInFacilityId, TableId, " +
                                                "last_update_date, last_updated_user, CheckInTrxId, AllowedTimeInMinutes, TrxLineId) " +
                                        "Values (@customer_id, getdate(), @PhotoFileName, " +
                                                "@CardId, @CheckInFacilityId, @TableId, " +
                                                "getdate(), @last_updated_user, @CheckInTrxId, @AllowedTimeInMinutes, @TrxLineId); select @@identity ";


            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@customer_id", customerDTO.Id);
            cmd.Parameters.AddWithValue("@CheckInTrxId", CheckInTrxId);
            cmd.Parameters.AddWithValue("@TrxLineId", TrxLineId);
            cmd.Parameters.AddWithValue("@CheckInFacilityId", CheckInFacilityId);

            log.LogVariableState("@customer_id", customerDTO.Id);
            log.LogVariableState("@CheckInTrxId", CheckInTrxId);
            log.LogVariableState("@TrxLineId", TrxLineId);
            log.LogVariableState("@CheckInFacilityId", CheckInFacilityId);

            if (TableId >= 0)
            {
                cmd.Parameters.AddWithValue("@TableId", TableId);
                log.LogVariableState("@TableId", TableId);
            }
            else
            {
                cmd.Parameters.AddWithValue("@TableId", DBNull.Value);
                log.LogVariableState("@TableId", DBNull.Value);
            }

            if (card != null)
            {
                cmd.Parameters.AddWithValue("@CardId", card.card_id);
                log.LogVariableState("@CardId", card.card_id);
            }

            else
            {
                cmd.Parameters.AddWithValue("@CardId", DBNull.Value);
                log.LogVariableState("@CardId", DBNull.Value);
            }

            cmd.Parameters.AddWithValue("@last_updated_user", Utilities.ParafaitEnv.Username);
            cmd.Parameters.AddWithValue("@PhotoFileName", PhotoFileName);
            cmd.Parameters.AddWithValue("@AllowedTimeInMinutes", AllowedTimeInMinutes);

            log.LogVariableState("@last_updated_user", Utilities.ParafaitEnv.Username);
            log.LogVariableState("@PhotoFileName", PhotoFileName);
            log.LogVariableState("@AllowedTimeInMinutes", AllowedTimeInMinutes);

            try
            {
                int CheckInId = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = "Insert into CheckInDetails (Name, DateOfBirth, Age, SpecialNeeds, Allergies, Remarks, " +
                                                                "VehicleNumber, VehicleModel, VehicleColor, " +
                                                                "last_update_date, last_updated_user, CheckInId, " +
                                                                "CheckOutTime, CheckOutTrxId, CardId, TrxLineId, CheckInTrxId, CheckInTrxLineId) " +
                                                      "Values (@Name, @DateOfBirth, @Age, @SpecialNeeds, @Allergies, @Remarks, " +
                                                                "@VehicleNumber, @VehicleModel, @VehicleColor, " +
                                                                "getdate(), @last_updated_user, @CheckInId, " +
                                                                "case when @AutoCheckOut = 'Y' then dateadd(MINUTE, @AllowedTimeInMinutes, getdate()) else null end, " +
                                                                "case when @AutoCheckOut = 'Y' then @CheckOutTrxId else null end, " +
                                                                "@CardId, case when @AutoCheckOut = 'Y' then @TrxLineId else null end, " +
                                                                "@CheckInTrxId, @CheckInTrxLineId)";
                for (int i = 0; i < checkInDetailsList.Count; i++)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@AutoCheckOut", AutoCheckOut);
                    cmd.Parameters.AddWithValue("@AllowedTimeInMinutes", AllowedTimeInMinutes);
                    cmd.Parameters.AddWithValue("@CheckOutTrxId", CheckInTrxId);
                    cmd.Parameters.AddWithValue("@TrxLineId", TrxLineId);
                    cmd.Parameters.AddWithValue("@Name", checkInDetailsList[i].Name);
                    //Check in Trx details
                    cmd.Parameters.AddWithValue("@CheckInTrxId", checkInDetailsList[i].CheckInTrxId);
                    cmd.Parameters.AddWithValue("@CheckInTrxLineId", checkInDetailsList[i].CheckInTrxLineId);

                    log.LogVariableState("@AutoCheckOut", AutoCheckOut);
                    log.LogVariableState("@AllowedTimeInMinutes", AllowedTimeInMinutes);
                    log.LogVariableState("@CheckOutTrxId", CheckInTrxId);
                    log.LogVariableState("@TrxLineId", TrxLineId);
                    log.LogVariableState("@Name", checkInDetailsList[i].Name);
                    log.LogVariableState("@CheckInTrxId", checkInDetailsList[i].CheckInTrxId);
                    log.LogVariableState("@CheckInTrxLineId", checkInDetailsList[i].CheckInTrxLineId);


                    if (checkInDetailsList[i].DateOfBirth != DateTime.MinValue)
                    {
                        cmd.Parameters.AddWithValue("@DateOfBirth", checkInDetailsList[i].DateOfBirth);
                        log.LogVariableState("@DateOfBirth", checkInDetailsList[i].DateOfBirth);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
                        log.LogVariableState("@DateOfBirth", DBNull.Value);
                    }

                    if (checkInDetailsList[i].Age != 0)
                    {
                        cmd.Parameters.AddWithValue("@Age", checkInDetailsList[i].Age);
                        log.LogVariableState("@Age", checkInDetailsList[i].Age);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Age", DBNull.Value);
                        log.LogVariableState("@Age", DBNull.Value);
                    }

                    if (checkInDetailsList[i].SpecialNeeds == null)
                    {
                        cmd.Parameters.AddWithValue("@SpecialNeeds", DBNull.Value);
                        log.LogVariableState("@SpecialNeeds", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SpecialNeeds", checkInDetailsList[i].SpecialNeeds);
                        log.LogVariableState("@SpecialNeeds", DBNull.Value);
                    }

                    if (checkInDetailsList[i].Allergies == null)
                    {
                        cmd.Parameters.AddWithValue("@Allergies", DBNull.Value);
                        log.LogVariableState("@Allergies", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Allergies", checkInDetailsList[i].Allergies);
                        log.LogVariableState("@Allergies", checkInDetailsList[i].Allergies);
                    }

                    if (checkInDetailsList[i].Remarks == null)
                    {
                        cmd.Parameters.AddWithValue("@Remarks", DBNull.Value);
                        log.LogVariableState("@Remarks", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Remarks", checkInDetailsList[i].Remarks);
                        log.LogVariableState("@Remarks", checkInDetailsList[i].Remarks);
                    }

                    if (checkInDetailsList[i].VehicleNumber == null)
                    {
                        cmd.Parameters.AddWithValue("@VehicleNumber", DBNull.Value);
                        log.LogVariableState("@VehicleNumber", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@VehicleNumber", checkInDetailsList[i].VehicleNumber);
                        log.LogVariableState("@VehicleNumber", checkInDetailsList[i].VehicleNumber);
                    }

                    if (checkInDetailsList[i].VehicleModel == null)
                    {
                        cmd.Parameters.AddWithValue("@VehicleModel", DBNull.Value);
                        log.LogVariableState("@VehicleModel", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@VehicleModel", checkInDetailsList[i].VehicleModel);
                        log.LogVariableState("@VehicleModel", checkInDetailsList[i].VehicleModel);
                    }

                    if (checkInDetailsList[i].VehicleColor == null)
                    {
                        cmd.Parameters.AddWithValue("@VehicleColor", DBNull.Value);
                        log.LogVariableState("@VehicleColor", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@VehicleColor", checkInDetailsList[i].VehicleColor);
                        log.LogVariableState("@VehicleColor", checkInDetailsList[i].VehicleColor);
                    }

                    cmd.Parameters.AddWithValue("@last_updated_user", Utilities.ParafaitEnv.Username);
                    cmd.Parameters.AddWithValue("@CheckInId", CheckInId);

                    log.LogVariableState("@last_updated_user", Utilities.ParafaitEnv.Username);
                    log.LogVariableState("@CheckInId", CheckInId);

                    if (checkInDetailsList[i].detailCard != null)
                    {
                        if (checkInDetailsList[i].detailCard.CardStatus == "NEW")
                        {
                            if (Utilities.ParafaitEnv.CHECKIN_DETAILS_RFID_TAG == "0")
                            {
                                if (cardFaceValue == 0)
                                    checkInDetailsList[i].detailCard.face_value = Utilities.ParafaitEnv.CardFaceValue;
                                else
                                    checkInDetailsList[i].detailCard.face_value = cardFaceValue;
                            }
                            else
                            {
                                try
                                {
                                    checkInDetailsList[i].detailCard.face_value = (float)Convert.ToDecimal(Utilities.ParafaitEnv.WRIST_BAND_FACE_VALUE);
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Unable to get the check in time! ", ex);
                                    checkInDetailsList[i].detailCard.face_value = 0;
                                }
                            }

                            checkInDetailsList[i].detailCard.createCard(SQLTrx);
                        }
                        cmd.Parameters.AddWithValue("@CardId", checkInDetailsList[i].detailCard.card_id);
                        log.LogVariableState("@CardId", checkInDetailsList[i].detailCard.card_id);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CardId", DBNull.Value);
                        log.LogVariableState("@CardId", DBNull.Value);
                    }

                    cmd.ExecuteNonQuery();
                }

                FacilityDTO facilityDTO = new FacilityBL(Utilities.ExecutionContext, CheckInFacilityId,false,false, SQLTrx).FacilityDTO;
                //int capacity = getCapacity(CheckInFacilityId, SQLTrx);
                int capacity = facilityDTO.Capacity == null ? 0 : Convert.ToInt32(facilityDTO.Capacity);
                if (capacity > 0)
                {
                    if (capacity < getTotalCheckedIn(CheckInFacilityId, SQLTrx))
                    {
                        message = Utilities.MessageUtils.getMessage(11);
                        log.LogVariableState("Message ", message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }

                try
                {
                    ExternalInterfaces();
                }
                catch (Exception ex)
                {
                    log.Error("Unable to call ExternalInterfaces()", ex);
                    message = "External Interface: " + ex.Message;
                }

                log.LogVariableState("Message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured when inserting details into CheckInDetails", ex);
                message = "Check-In Save:" + ex.Message;
                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }

        void ExternalInterfaces()
        {
            log.LogMethodEntry();

            if (TableId >= 0)
            {
                DataTable dt = Utilities.executeDataTable(@"select * from FacilityTables where tableId = @tableId", new SqlParameter("@tableId", TableId));
                //Semnox.Parafait.Transaction.ExternalInterfaces.SwitchOn(dt.Rows[0]);
            }

            log.LogMethodExit(null);
        }

        public int getTotalCheckedIn(int pCheckInFacilityId, SqlTransaction pSQLTrx)
        {
            log.LogMethodEntry(pCheckInFacilityId, pSQLTrx);

            SqlCommand cmd = Utilities.getCommand(pSQLTrx);
            cmd.CommandText = "select count(*) from CheckIns h, checkInDetails d " +
                                "where h.checkInId = d.CheckInId " +
                                "and CheckInFacilityId = @CheckInFacilityId " +
                                "and (CheckOutTime is null or CheckOutTime > getdate())";
            cmd.Parameters.AddWithValue("@CheckInFacilityId", pCheckInFacilityId);

            log.LogVariableState("@CheckInFacilityId", pCheckInFacilityId);

            int returnValue = Convert.ToInt32(cmd.ExecuteScalar());

            log.LogMethodExit(returnValue);
            return returnValue;
        }

        //public int getCapacity(int pCheckInFacilityId, SqlTransaction pSQLTrx)
        //{
        //    log.LogMethodEntry(pCheckInFacilityId, pSQLTrx);

        //    SqlCommand cmd = Utilities.getCommand(pSQLTrx);
        //    cmd.CommandText = "select isnull(capacity, 0), facilityName from CheckInFacility " +
        //                        "where FacilityId = @CheckInFacilityId ";
        //    cmd.Parameters.AddWithValue("@CheckInFacilityId", pCheckInFacilityId);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    da.Fill(dt);

        //    FacilityName = dt.Rows[0][1].ToString();

        //    int returnValue = Convert.ToInt32(dt.Rows[0][0]);
        //    log.LogMethodExit(returnValue);
        //    return returnValue;
        //}

        public decimal getCheckInPrice(int ProductId, object availableUnits, decimal userPrice)
        {
            log.LogMethodEntry(ProductId, availableUnits, userPrice);

                decimal Price = Convert.ToDecimal(Utilities.executeScalar("select isnull(Price, 0) from Products where product_Id = @ProductId",new SqlParameter("@ProductId", ProductId)));

                log.LogVariableState("@ProductId", ProductId);
                log.LogVariableState("'Price",Price);

                if (userPrice > 0)
                {
                    EffectivePrice = userPrice;
                    if (Price != 0)
                        AllowedTimeInMinutes = Convert.ToInt32(userPrice / Price);
                    UserTime = true;
                }
                else
                    EffectivePrice = Price;

            if (availableUnits != DBNull.Value && Convert.ToInt32(availableUnits).Equals(0))
            {
                log.LogMethodExit(EffectivePrice);
                return EffectivePrice;
            }
            else
            {
                EffectivePrice = EffectivePrice * CheckedInUnits;

                log.LogMethodExit(EffectivePrice);
                return (EffectivePrice);
            }
        }
    }    
}

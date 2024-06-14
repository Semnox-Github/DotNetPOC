/********************************************************************************************
 * Project Name - Customer.Accounts
 * Description  - Class for  of CreditPlusType      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 *2.110.0       07-Apr-2021   Nitin Pai      Added changes for user defined Credit Plus Types - License enhancement
 *2.120.0       05-Mar-2021   Girish kundar  Modified : Added Virtual Point credit plus type as part of virtual arcade changes
 *2.120.0       04-May-2021   Mushahid Faizan   Modified : GetuserDefinedCreditPlusTypes() - for WMS issue fixes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Credit plus types
    /// </summary>
    public enum CreditPlusType
    {
        /// <summary>
        /// Card Balance
        /// </summary>
        CARD_BALANCE,
        /// <summary>
        /// Loyalty Points
        /// </summary>
        LOYALTY_POINT,
        /// <summary>
        /// Ticket
        /// </summary>
        TICKET,
        /// <summary>
        /// Game Play Credit
        /// </summary>
        GAME_PLAY_CREDIT,
        /// <summary>
        /// Counter Item
        /// </summary>
        COUNTER_ITEM,
        /// <summary>
        /// Game Play Bonus
        /// </summary>
        GAME_PLAY_BONUS,
        /// <summary>
        /// Time
        /// </summary>
        TIME,
        /// <summary>
        /// Fixed Term
        /// </summary>
        /// FIXED_TERM,
        /// <summary>
        /// One Time
        /// </summary>
        /// ONE_TIME,
        /// <summary>
        ///  virual points
        /// </summary>
        VIRTUAL_POINT,
        /// <summary>
        ///  USER_DEFINED_TYPE_D
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_D,
        /// <summary>
        ///  USER_DEFINED_TYPE_E
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_E,
        /// <summary>
        ///  USER_DEFINED_TYPE_F
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_F,
        /// <summary>
        ///  USER_DEFINED_TYPE_H
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_H,
        /// <summary>
        ///  USER_DEFINED_TYPE_I
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_I,
        /// <summary>
        ///  USER_DEFINED_TYPE_J
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_J,
        /// <summary>
        ///  USER_DEFINED_TYPE_K
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_K,
        /// <summary>
        ///  USER_DEFINED_TYPE_N
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_N,
        /// <summary>
        ///  USER_DEFINED_TYPE_O
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_O,
        /// <summary>
        ///  USER_DEFINED_TYPE_Q
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_Q,
        /// <summary>
        ///  USER_DEFINED_TYPE_R
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_R,
        /// <summary>
        ///  USER_DEFINED_TYPE_S
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_S,
        /// <summary>
        ///  USER_DEFINED_TYPE_U
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_U,
        /// <summary>
        ///  USER_DEFINED_TYPE_W
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_W,
        /// <summary>
        ///  USER_DEFINED_TYPE_X
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_X,
        /// <summary>
        ///  USER_DEFINED_TYPE_Y
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_Y,
        /// <summary>
        ///  USER_DEFINED_TYPE_Z
        /// </summary>
        USER_DEFINED_LICENSE_TYPE_Z,
    }
    /// <summary>
    /// Converts CreditPlusType from/to string
    /// </summary>
    public class CreditPlusTypeConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ConcurrentDictionary<string, string> attributeDictionary = new ConcurrentDictionary<string, string>();
        /// <summary>
        /// Converts CreditPlusType from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CreditPlusType FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value.ToUpper())
            {
                case "A":
                    {
                        return CreditPlusType.CARD_BALANCE;
                    }
                case "L":
                    {
                        return CreditPlusType.LOYALTY_POINT;
                    }
                case "T":
                    {
                        return CreditPlusType.TICKET;
                    }
                case "G":
                    {
                        return CreditPlusType.GAME_PLAY_CREDIT;
                    }
                case "P":
                    {
                        return CreditPlusType.COUNTER_ITEM;
                    }
                case "B":
                    {
                        return CreditPlusType.GAME_PLAY_BONUS;
                    }
                case "M":
                    {
                        return CreditPlusType.TIME;
                    }
                case "V":
                    {
                        return CreditPlusType.VIRTUAL_POINT;
                    }
                case "D":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_D;
                    }
                case "E":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_E;
                    }
                case "F":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_F;
                    }
                case "H":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_H;
                    }
                case "I":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_I;
                    }
                case "J":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_J;
                    }
                case "K":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_K;
                    }
                case "N":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_N;
                    }
                case "O":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_O;
                    }
                case "Q":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_Q;
                    }
                case "R":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_R;
                    }
                case "S":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_S;
                    }
                case "U":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_U;
                    }
                case "W":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_W;
                    }
                case "X":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_X;
                    }
                case "Y":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_Y;
                    }
                case "Z":
                    {
                        return CreditPlusType.USER_DEFINED_LICENSE_TYPE_Z;
                    }
                default:
                    {
                        log.Error("Error :Not a valid credit plus type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid credit plus type");
                    }
            }
        }


        /// <summary>
        /// Converts CreditPlusType to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(CreditPlusType value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case CreditPlusType.CARD_BALANCE:
                    {
                        return "A";
                    }
                case CreditPlusType.LOYALTY_POINT:
                    {
                        return "L";
                    }
                case CreditPlusType.TICKET:
                    {
                        return "T";
                    }
                case CreditPlusType.GAME_PLAY_CREDIT:
                    {
                        return "G";
                    }
                case CreditPlusType.COUNTER_ITEM:
                    {
                        return "P";
                    }
                case CreditPlusType.GAME_PLAY_BONUS:
                    {
                        return "B";
                    }
                case CreditPlusType.TIME:
                    {
                        return "M";
                    }
                case CreditPlusType.VIRTUAL_POINT:
                    {
                        return "V";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_D:
                    {
                        return "D";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_E:
                    {
                        return "E";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_F:
                    {
                        return "F";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_H:
                    {
                        return "H";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_I:
                    {
                        return "I";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_J:
                    {
                        return "J";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_K:
                    {
                        return "K";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_N:
                    {
                        return "N";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_O:
                    {
                        return "O";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_Q:
                    {
                        return "Q";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_R:
                    {
                        return "R";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_S:
                    {
                        return "S";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_U:
                    {
                        return "U";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_W:
                    {
                        return "W";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_X:
                    {
                        return "X";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_Y:
                    {
                        return "Y";
                    }
                case CreditPlusType.USER_DEFINED_LICENSE_TYPE_Z:
                    {
                        return "Z";
                    }
                default:
                    {
                        log.Error("Error :Not a valid credit plus type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid credit plus type");
                    }
            }
        }

        /// <summary>
        /// Converts CreditPlusType to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(string value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case "A":
                    {
                        return "CARD_BALANCE";
                    }
                case "L":
                    {
                        return "LOYALTY_POINT";
                    }
                case "T":
                    {
                        return "TICKET";
                    }
                case "G":
                    {
                        return "GAME_PLAY_CREDIT";
                    }
                case "P":
                    {
                        return "COUNTER_ITEM";
                    }
                case "B":
                    {
                        return "GAME_PLAY_BONUS";
                    }
                case "M":
                    {
                        return "TIME";
                    }
                case "V":
                    {
                        return "VIRTUAL_POINT";
                    }
                default:
                    {
                        String attributeValue = GetCreditPlusType(value);
                        if (string.IsNullOrEmpty(attributeValue))
                        {
                            log.Error("Error :Not a valid credit plus type ");
                            log.LogMethodExit(null, "Throwing Exception");
                            throw new ArgumentException("Not a valid credit plus type");
                        }
                        return attributeValue;
                    }
            }
        }


        public static string GetCreditPlusType(String creditPlusType)
        {
            log.LogMethodEntry(creditPlusType);
            string result = "";
            if (attributeDictionary == null || !attributeDictionary.ContainsKey(creditPlusType))
            {

                string query = @"SELECT * FROM LoyaltyAttributes";
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { }, null);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    String key = "";
                    String value = "";
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DataRow dataRow = dataTable.Rows[i];
                        key = dataRow["CreditPlusType"].ToString();
                        value = dataRow["Attribute"].ToString();
                        if (!attributeDictionary.ContainsKey(key))
                        {
                            attributeDictionary.TryAdd(key, value);
                        }
                    }
                }
                log.LogMethodExit(result);
            }

            return attributeDictionary[creditPlusType];

        }

        public static List<KeyValuePair<string, string>> GetuserDefinedCreditPlusTypes()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
            if (attributeDictionary == null || attributeDictionary.Count == 0)
            {
                string query = @"SELECT * FROM LoyaltyAttributes";
                DataAccessHandler dataAccessHandler = new DataAccessHandler();
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { }, null);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    String key = "";
                    String value = "";
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DataRow dataRow = dataTable.Rows[i];
                        key = dataRow["CreditPlusType"].ToString();
                        value = dataRow["Attribute"].ToString();
                        if (!attributeDictionary.ContainsKey(key))
                        {
                            attributeDictionary.TryAdd(key, value);
                        }
                    }
                }
            }

            List<String> cpTypeKeys = (List<String>)attributeDictionary.Keys.ToList();
            foreach (string key in cpTypeKeys)
            {
                if (IsUserDefinedCreditPlusType(key))
                {
                    result.Add(new KeyValuePair<string, string>(key, ToString(key)));
                }
            }

            log.LogMethodExit(result);
            return result;
        }

        public static bool IsUserDefinedCreditPlusType(String creditPlusType)
        {
            switch (creditPlusType.ToUpper())
            {
                case "C":
                case "D":
                case "E":
                case "F":
                case "H":
                case "I":
                case "J":
                case "K":
                case "N":
                case "O":
                case "Q":
                case "R":
                case "S":
                case "U":
                case "W":
                case "X":
                case "Y":
                case "Z":
                    {
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }
    }
}


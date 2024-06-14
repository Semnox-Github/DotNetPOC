/********************************************************************************************
 * Project Name - UserAccountDatahandler
 * Description  - UserAccountDatahandler object of user
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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using Semnox.Core.Utilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// class UserAccountDatahandler
    /// </summary>
    public class UserAccountDatahandler
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        Utilities utilities;
        ExecutionContext machineUserContext;

        /// <summary>
        /// Default constructor of UserDataHandler class
        /// </summary>
        public UserAccountDatahandler()
        {
            log.Debug("Starts-UserAccountDatahandler() default constructor.");

            // for password changes using security object from parafaitUtils
            string connstring = ConfigurationManager.ConnectionStrings["ParafaitConnectionString"].ConnectionString;
            utilities = new Utilities(connstring);

            machineUserContext = ExecutionContext.GetExecutionContext();
            dataAccessHandler = new DataAccessHandler();

            log.Debug("Ends-UserAccountDatahandler() default constructor.");
        }


        /// <summary>
        /// Updates the user record
        /// </summary>
        /// <param name="userId">int type parameter</param>
        /// <param name="binaryHash">bute[] parameter</param>
        /// <param name="salt">stting type parameter</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateUserPassword(int userId, byte[] binaryHash, string salt)
        {
            log.Debug("Starts-UpdateUserPassword(userDTO, userId, siteId) Method.");

            string updatePasswordQuery = @"Update users set  
                                                                passwordHash=@passwordHash  ,
                                                                passwordSalt =@passwordSalt  ,
                                                                LastUpdatedBy=@LastUpdatedBy, 
                                                                LastUpdatedDate=getdate(),
                                                                PasswordChangeOnNextLogin=@PasswordChangeOnNextLogin 
                                                        where user_id=@user_id";

            List<SqlParameter> updatePasswordParameters = new List<SqlParameter>();

            updatePasswordParameters.Add(new SqlParameter("@passwordHash", binaryHash));
            updatePasswordParameters.Add(new SqlParameter("@passwordSalt", salt));
            updatePasswordParameters.Add(new SqlParameter("@LastUpdatedBy", machineUserContext.GetUserId()));
            updatePasswordParameters.Add(new SqlParameter("@PasswordChangeOnNextLogin", true));
            updatePasswordParameters.Add(new SqlParameter("@user_id", userId));

            int rowsUpdated = dataAccessHandler.executeUpdateQuery(updatePasswordQuery, updatePasswordParameters.ToArray());
            log.Debug("Ends-UpdateUser(userDTO, userId, siteId) Method.");
            return rowsUpdated;
        }

        

        /// <summary>
        /// GetCardBalance
        /// </summary>
        /// <param name="cardNumber">string type</param>
        /// <returns>returns double value</returns>
        public double GetCardBalance(string cardNumber)
        {
            log.Debug("Starts- GetCardBalance(string cardNumber) Method .");

            double credits = 0;
            string selectUserQuery = @"select case when isnull(ExpiryDate, getdate()) >= GETDATE() then credits else 0 end, card_id 
                                        from cards 
                                        where card_number = @card_number and valid_flag = 'Y'";
            SqlParameter[] selectUserParameters = new SqlParameter[1];
            selectUserParameters[0] = new SqlParameter("@card_number", cardNumber);
            DataTable card = dataAccessHandler.executeSelectQuery(selectUserQuery, selectUserParameters);

            if (card.Rows.Count == 1)
                credits = Convert.ToDouble(card.Rows[0][0]);
           log.Debug("Ends- GetCardBalance(string cardNumber)  Method .");
              
            return credits;

        }

    }
}


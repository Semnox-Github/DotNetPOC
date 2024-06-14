//using Semnox.Core.Profile;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Please don't use this datahandler. This data handler is created to keep the backward compatibility
    /// </summary>
    public class CustomerBackwardCompatibityDataHandler
    {
         DataAccessHandler dataAccessHandler;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        /// <summary>
        /// Default constructor of  CustomersDatahandler class
        /// </summary>
        public CustomerBackwardCompatibityDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new  DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Customer Record.
        /// </summary>
        /// <param name="customerDTO">CustomerDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(CustomerDTO customerDTO, string userId, int siteId)
        {
            log.LogMethodEntry("customerDTO", userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@customer_id", customerDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userName", customerDTO.UserName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customer_name", customerDTO.FirstName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@middle_name", customerDTO.MiddleName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@last_name", customerDTO.LastName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@email", customerDTO.Email));
            parameters.Add(dataAccessHandler.GetSQLParameter("@birth_date", customerDTO.DateOfBirth));
            parameters.Add(dataAccessHandler.GetSQLParameter("@anniversary", customerDTO.Anniversary));
            AddressDTO addressDTO = customerDTO.LatestAddressDTO;
            parameters.Add(dataAccessHandler.GetSQLParameter("@address1", addressDTO.Line1));
            parameters.Add(dataAccessHandler.GetSQLParameter("@address2", string.IsNullOrWhiteSpace(addressDTO.Line2) ? (object)DBNull.Value : addressDTO.Line2));
            parameters.Add(dataAccessHandler.GetSQLParameter("@address3", string.IsNullOrWhiteSpace(addressDTO.Line3) ? (object)DBNull.Value : addressDTO.Line3));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contact_phone1", customerDTO.PhoneNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@contact_phone2", customerDTO.SecondaryPhoneNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@city", addressDTO.City));
            parameters.Add(dataAccessHandler.GetSQLParameter("@state", addressDTO.StateId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@country", addressDTO.CountryId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pin", addressDTO.PostalCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@gender", customerDTO.Gender));
            parameters.Add(dataAccessHandler.GetSQLParameter("@passWord", customerDTO.Password));
            parameters.Add(dataAccessHandler.GetSQLParameter("@photoFileName", customerDTO.PhotoURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notes", customerDTO.Notes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Company", customerDTO.Company));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Designation", customerDTO.Designation));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Unique_ID", customerDTO.UniqueIdentifier));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FBUserId", customerDTO.FBUserId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@FBAccessToken", customerDTO.FBAccessToken));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TWAccessToken", customerDTO.TWAccessToken));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TWAccessSecret", customerDTO.TWAccessSecret));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RightHanded", customerDTO.RightHanded ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TeamUser", customerDTO.TeamUser ? "Y" : "N"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IDProofFileName", customerDTO.IdProofFileURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Title", customerDTO.Title));
            parameters.Add(dataAccessHandler.GetSQLParameter("@WeChatAccessToken", customerDTO.WeChatAccessToken));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastLoginTime", customerDTO.LastLoginTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxCode", customerDTO.TaxCode));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Updates the customer record
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public int UpdateCustomer(CustomerDTO customerDTO, string userId, int siteId)
        {
            log.LogMethodEntry("customerDTO");
            int rowsUpdated = 0;
            if (customerDTO.ProfileDTO != null)
            {
                string query = @"update customers set 
                                            userName=@userName,
                                            customer_name=@customer_name,
                                            middle_name=@middle_name,
                                            last_name=@last_name,
                                            email=@email,
                                            birth_date=@birth_date,
                                            anniversary=@anniversary,
                                            address1=@address1,
                                            address2=@address2,
                                            address3=@address3,
                                            contact_phone1=@contact_phone1,
                                            contact_phone2=@contact_phone2,
                                            city=@city,
                                            state=(SELECT TOP 1 State FROM State WHERE StateId=@state),
                                            country=(SELECT TOP 1 CountryName FROM Country WHERE CountryId=@country),
                                            pin=@pin,
                                            gender=@gender,
                                            passWord=@passWord,
                                            photoFileName =@photoFileName,
                                            notes=@notes, 
                                            Company=@Company,
                                            Designation=@Designation,
                                            Unique_ID=@Unique_ID,
                                            FBUserId=@FBUserId,
                                            FBAccessToken=@FBAccessToken,
                                            TWAccessToken=@TWAccessToken,
                                            TWAccessSecret=@TWAccessSecret,
                                            RightHanded=@RightHanded,
                                            TeamUser=@TeamUser,
                                            IDProofFileName=@IDProofFileName,
                                            Title=@Title,
                                            WeChatAccessToken=@WeChatAccessToken,
                                            TaxCode=@TaxCode,
                                            LastLoginTime=@LastLoginTime
                                            where customer_id= @customer_id";
                try
                {
                    rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(customerDTO, userId, siteId).ToArray(), sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("", ex);
                    log.LogMethodExit(null, "throwing exception");
                    throw ex;
                }
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }
    }
}

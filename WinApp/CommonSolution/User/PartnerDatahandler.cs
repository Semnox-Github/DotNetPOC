/********************************************************************************************
 * Project Name - Partners Data Handler
 * Description  - Data handler of the Partners Datahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00          10-May-2016   Rakshith          Created 
 *2.70.2        15-Jul-2019   Girish Kundar     Modified : Added GetSQLParameter(),SQL Injection Fix,Missed Who columns
 *2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query
  *3.0          03-Nov-2020   Mushahid Faizan   Modified IsActive null as true value.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    ///PartnersDatahandler - Handles insert, update and select of  PartnersDTO type objects
    /// </summary>
    public class PartnersDatahandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Partners AS p ";

        /// <summary>
        /// Default constructor of  PartnersDatahandler class
        /// </summary>
        public PartnersDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<PartnersDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PartnersDTO.SearchByParameters, string>
        {
               {PartnersDTO.SearchByParameters.CUSTOMER_ID, "p.Customer_Id"},
               {PartnersDTO.SearchByParameters.ACTIVE, "p.Active"}  ,
               {PartnersDTO.SearchByParameters.MASTER_ENTITY_ID, "p.MasterEntityId"}  ,
               {PartnersDTO.SearchByParameters.PARTNER_ID, "p.PartnerId"},
               {PartnersDTO.SearchByParameters.SITE_ID, "p.site_id"}
        };


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Partners Record.
        /// </summary>
        /// <param name="partnersDTO">PartnersDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(PartnersDTO partnersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(partnersDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@partnerId", partnersDTO.PartnerId, true);
            ParametersHelper.ParameterHelper(parameters, "@customer_Id", partnersDTO.Customer_Id, true);
            ParametersHelper.ParameterHelper(parameters, "@partnerName", string.IsNullOrEmpty(partnersDTO.PartnerName) ? DBNull.Value : (object)partnersDTO.PartnerName);
            ParametersHelper.ParameterHelper(parameters, "@remarks", string.IsNullOrEmpty(partnersDTO.Remarks) ? DBNull.Value : (object)partnersDTO.Remarks);
            ParametersHelper.ParameterHelper(parameters, "@active", partnersDTO.Active);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedUser", loginId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@site_id", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", partnersDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the PartnersDTO Items record to the database
        /// </summary>
        /// <param name="PartnersDTO">PartnersDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns PartnersDTO</returns>
        public PartnersDTO InsertPartner(PartnersDTO partnersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(partnersDTO, loginId, siteId);
            string insertPartnersQuery = @"insert into Partners 
                                                        (  
                                                            PartnerName,
                                                            Remarks,
                                                            Customer_Id,
                                                            CreatedBy,
                                                            CreationDate,
                                                            LastUpdatedUser,
                                                            LastUpdatedDate,
                                                            Active,
                                                            site_id,
                                                            Guid,
                                                            MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                            @partnerName,
                                                            @remarks,
                                                            @customer_Id,
                                                            @createdBy,
                                                            GetDate() ,
                                                            @lastUpdatedUser,
                                                            GetDate() ,
                                                            @active,
                                                            @site_id,
                                                            NEWID(),
                                                            @masterEntityId
                                                         )SELECT  * from Partners where PartnerId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertPartnersQuery, BuildSQLParameters(partnersDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPartnersDTO(partnersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting partnersDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(partnersDTO);
            return partnersDTO;

        }

        /// <summary>
        /// update the PartnersDTO record to the database
        /// </summary>
        /// <param name="PartnersDTO">PartnersDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns PartnersDTO</returns>
        public PartnersDTO UpdatePartner(PartnersDTO partnersDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(partnersDTO, loginId, siteId);
            string updatePatnerDTOQuery = @"update Partners 
                                                          set 
                                                            PartnerName=@partnerName,
                                                            Remarks=@remarks,
                                                            Customer_Id=@customer_Id,
                                                            LastUpdatedUser=@lastUpdatedUser,
                                                            LastUpdatedDate=GetDate(),
                                                            Active=@active,
                                                            -- site_id=@site_id,
                                                            MasterEntityId= @masterEntityId
                                                            where PartnerId = @partnerId
                                           SELECT  * from Partners where PartnerId = @partnerId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updatePatnerDTOQuery, BuildSQLParameters(partnersDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPartnersDTO(partnersDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating partnersDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(partnersDTO);
            return partnersDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="partnersDTO">PartnersDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshPartnersDTO(PartnersDTO partnersDTO, DataTable dt)
        {
            log.LogMethodEntry(partnersDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                partnersDTO.PartnerId = Convert.ToInt32(dt.Rows[0]["PartnerId"]);
                partnersDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                partnersDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                partnersDTO.LastUpdatedUser = dataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedUser"]);
                partnersDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                partnersDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                partnersDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// return the record from the database
        /// Convert the datarow to PartnersDTO object
        /// </summary>
        /// <returns>return the PartnersDTO object</returns>
        private PartnersDTO GetPartnersDTO(DataRow partnerDataRow)
        {
            log.LogMethodEntry(partnerDataRow);
            PartnersDTO partnersDTO = new PartnersDTO(
                                                    partnerDataRow["PartnerId"] == DBNull.Value ? -1 : Convert.ToInt32(partnerDataRow["PartnerId"]),
                                                    partnerDataRow["PartnerName"] == DBNull.Value ? string.Empty : Convert.ToString(partnerDataRow["PartnerName"]),
                                                    partnerDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(partnerDataRow["Remarks"]),
                                                    partnerDataRow["Customer_Id"] == DBNull.Value ? -1 : Convert.ToInt32(partnerDataRow["Customer_Id"]),
                                                    partnerDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(partnerDataRow["CreatedBy"]),
                                                    partnerDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(partnerDataRow["CreationDate"]),
                                                    partnerDataRow["LastUpdatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(partnerDataRow["LastUpdatedUser"]),
                                                    partnerDataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(partnerDataRow["LastUpdatedDate"]),
                                                    partnerDataRow["Active"] == DBNull.Value ? true : Convert.ToBoolean(partnerDataRow["active"]),
                                                    partnerDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(partnerDataRow["site_id"]),
                                                    partnerDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(partnerDataRow["Guid"]),
                                                    partnerDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(partnerDataRow["SynchStatus"]),
                                                    partnerDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(partnerDataRow["MasterEntityId"])
                                                 );
            log.LogMethodExit(partnersDTO);
            return partnersDTO;

        }


        /// <summary>
        /// return the record from the database based on  partnerId
        /// </summary>
        /// <returns>return the PartnersDTO object</returns>
        /// or null
        public PartnersDTO GetPartnersDTO(int partnerId)
        {
            log.LogMethodEntry(partnerId);
            string CmsBannerItemsRequestQuery = SELECT_QUERY + "   where PartnerId = @partnerId ";
            PartnersDTO partnersDTO = null;
            SqlParameter[] partnetDTOParameters = new SqlParameter[1];
            partnetDTOParameters[0] = new SqlParameter("@partnerId", partnerId);

            DataTable dtPartnersDTO = dataAccessHandler.executeSelectQuery(CmsBannerItemsRequestQuery, partnetDTOParameters, sqlTransaction);
            if (dtPartnersDTO.Rows.Count > 0)
            {
                DataRow partnersDTORow = dtPartnersDTO.Rows[0];
                partnersDTO = GetPartnersDTO(partnersDTORow);

            }
            log.LogMethodExit(partnersDTO);
            return partnersDTO;
        }

        /// <summary>
        /// Gets the PartnersDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of Generic PartnersDTO matching the search criteria</returns>
        public List<PartnersDTO> GetAllpartnersList(List<KeyValuePair<PartnersDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            int count = 0;
            string selectPartentsDTOQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PartnersDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key == PartnersDTO.SearchByParameters.ACTIVE)
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1) =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key.Equals(PartnersDTO.SearchByParameters.CUSTOMER_ID)
                            || searchParameter.Key.Equals(PartnersDTO.SearchByParameters.PARTNER_ID)
                            || searchParameter.Key.Equals(PartnersDTO.SearchByParameters.MASTER_ENTITY_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " =  " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PartnersDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like  " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                    selectPartentsDTOQuery = selectPartentsDTOQuery + query;
            }
            DataTable dtPartentsDTO = dataAccessHandler.executeSelectQuery(selectPartentsDTOQuery, parameters.ToArray(), sqlTransaction);

            List<PartnersDTO> PartnersDTOList = new List<PartnersDTO>();
            if (dtPartentsDTO.Rows.Count > 0)
            {
                foreach (DataRow PartnersDTORow in dtPartentsDTO.Rows)
                {
                    PartnersDTO partnersDTO = GetPartnersDTO(PartnersDTORow);
                    PartnersDTOList.Add(partnersDTO);
                }

            }
            log.LogMethodExit(PartnersDTOList);
            return PartnersDTOList;
        }

        /// <summary>
        /// Delete the record from the database based on  partnerId
        /// </summary>
        /// <returns>return the int </returns>
        public int DeletePartner(int partnerId)
        {
            log.LogMethodEntry(partnerId);
            string partnerDTOQuery = @"delete  
                                           from Partners
                                           where PartnerId = @partnerId";

            SqlParameter[] partnerDTOParameters = new SqlParameter[1];
            partnerDTOParameters[0] = new SqlParameter("@partnerId", partnerId);

            int deleteStatus = dataAccessHandler.executeUpdateQuery(partnerDTOQuery, partnerDTOParameters, sqlTransaction);
            log.LogMethodExit(deleteStatus);
            return deleteStatus;

        }
    }
}

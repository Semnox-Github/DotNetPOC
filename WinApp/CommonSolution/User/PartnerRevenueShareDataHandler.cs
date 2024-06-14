/********************************************************************************************
 * Project Name - PartnerRevenueShare Data Handler
 * Description  - Data handler of the PartnerRevenueShare handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        13-May-2019   Jagan Mohana Rao        Created 
 *2.70.2        11-Dec-2019   Jinto Thomas        Removed siteid from update query
 *2.90       21-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.User
{
    public class PartnerRevenueShareDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private static readonly Dictionary<PartnerRevenueShareDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PartnerRevenueShareDTO.SearchByParameters, string>
               {
                    {PartnerRevenueShareDTO.SearchByParameters.MACHINE_GROUP_ID, "MachineGroupId"},
                    {PartnerRevenueShareDTO.SearchByParameters.PARTNER_ID, "PartnerId"},
                    {PartnerRevenueShareDTO.SearchByParameters.PARTNER_ID_LIST, "PartnerId"},
                    {PartnerRevenueShareDTO.SearchByParameters.POS_TYPE_ID, "POSTypeId"},
                    {PartnerRevenueShareDTO.SearchByParameters.SITE_ID,"site_id"},
                    {PartnerRevenueShareDTO.SearchByParameters.ISACTIVE,"IsActive"}
               };
        /// <summary>
        /// Default constructor of MachineGroupsDataHandler class
        /// </summary>
        public PartnerRevenueShareDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating MachineGroups Record.
        /// </summary>
        /// <param name="partnerRevenueShareDTO">PartnerRevenueShareDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(PartnerRevenueShareDTO partnerRevenueShareDTO, string userId, int siteId)
        {
            log.LogMethodEntry(partnerRevenueShareDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@partnerRevenueShareId", partnerRevenueShareDTO.PartnerRevenueShareId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@partnerId", partnerRevenueShareDTO.PartnerId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineGroupId", partnerRevenueShareDTO.MachineGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@revenueSharePercentage", partnerRevenueShareDTO.RevenueSharePercentage, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@minimumGuarantee", partnerRevenueShareDTO.MinimumGuarantee, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@posTypeId", partnerRevenueShareDTO.POSTypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", partnerRevenueShareDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@agentGroupId", partnerRevenueShareDTO.AgentGroupId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", partnerRevenueShareDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", partnerRevenueShareDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the PartnerRevenueShare record to the database
        /// </summary>
        /// <param name="partnerRevenueShareDTO">PartnerRevenueShareDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertPartnerRevenueShare(PartnerRevenueShareDTO partnerRevenueShareDTO, string userId, int siteId)
        {
            log.LogMethodEntry(partnerRevenueShareDTO, userId, siteId);
            int idOfRowInserted;
            string insertPartnerRevenueShareQuery = @"insert into PartnerRevenueShare 
                                                        (
                                                        PartnerId,
                                                        MachineGroupId,
                                                        RevenueSharePercentage,
                                                        MinimumGuarantee,
                                                        POSTypeId,
                                                        Guid,
                                                        site_id,
                                                        SynchStatus,
                                                        AgentGroupId,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @partnerId,
                                                        @machineGroupId,
                                                        @revenueSharePercentage,
                                                        @minimumGuarantee,
                                                        @posTypeId,
                                                        NewId(),
                                                        @site_id,
                                                        @synchStatus,
                                                        @agentGroupId,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        @isActive
                                                        )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(insertPartnerRevenueShareQuery, GetSQLParameters(partnerRevenueShareDTO, userId, siteId).ToArray(), null);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }
        /// <summary>
        /// Updates the PartnerRevenueShare record
        /// </summary>
        /// <param name="partnerRevenueShareDTO">PartnerRevenueShareDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdatePartnerRevenueShare(PartnerRevenueShareDTO partnerRevenueShareDTO, string userId, int siteId)
        {
            log.LogMethodEntry(partnerRevenueShareDTO, userId, siteId);
            int rowsUpdated;
            string updatePartnerRevenueShareQuery = @"update PartnerRevenueShare 
                                         set PartnerId=@partnerId,
                                             MachineGroupId=@machineGroupId,
                                             RevenueSharePercentage=@revenueSharePercentage,
                                             MinimumGuarantee=@minimumGuarantee,
                                             POSTypeId=@posTypeId,
                                             -- site_id=@site_id,
                                             SynchStatus = @synchStatus,
                                             AgentGroupId=@agentGroupId,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastUpdateDate = Getdate(),
                                             IsActive = @isActive
                                       where PartnerRevenueShareId = @partnerRevenueShareId";
            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(updatePartnerRevenueShareQuery, GetSQLParameters(partnerRevenueShareDTO, userId, siteId).ToArray(), null);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Delete the record from the database based on  partnerRevenueShareId
        /// </summary>
        /// <returns>return the int </returns>
        public int DeletePartnerRevenueShare(int partnerRevenueShareId)
        {
            log.LogMethodEntry(partnerRevenueShareId);
            try
            {
                string partnerRevenueShareQuery = @"DELETE  
                                           from PartnerRevenueShare
                                           where PartnerRevenueShareId = @partnerRevenueShareId";

                SqlParameter[] partnerRevenueShareParameters = new SqlParameter[1];
                partnerRevenueShareParameters[0] = new SqlParameter("@partnerRevenueShareId", partnerRevenueShareId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(partnerRevenueShareQuery, partnerRevenueShareParameters);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
        /// <summary>
        /// Converts the Data row object to PartnerRevenueShareDTO class type
        /// </summary>
        /// <param name="partnerRevenueShareDataRow">PartnerRevenueShareDTO DataRow</param>
        /// <returns>Returns PartnerRevenueShareDTO</returns>
        private PartnerRevenueShareDTO GetPartnerRevenueShareDTO(DataRow partnerRevenueShareDataRow)
        {
            log.LogMethodEntry(partnerRevenueShareDataRow);
            PartnerRevenueShareDTO partnerRevenueShareDataObject = new PartnerRevenueShareDTO(Convert.ToInt32(partnerRevenueShareDataRow["PartnerRevenueShareId"]),
                                            partnerRevenueShareDataRow["PartnerId"] == DBNull.Value ? -1 : Convert.ToInt32(partnerRevenueShareDataRow["PartnerId"]),
                                            partnerRevenueShareDataRow["MachineGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(partnerRevenueShareDataRow["MachineGroupId"]),
                                            partnerRevenueShareDataRow["RevenueSharePercentage"] == DBNull.Value ? -1 : Convert.ToDouble(partnerRevenueShareDataRow["RevenueSharePercentage"]),
                                            partnerRevenueShareDataRow["MinimumGuarantee"] == DBNull.Value ? -1 : Convert.ToDouble(partnerRevenueShareDataRow["MinimumGuarantee"]),
                                            partnerRevenueShareDataRow["POSTypeId"] == DBNull.Value ? -1 : Convert.ToInt32(partnerRevenueShareDataRow["POSTypeId"]),
                                            partnerRevenueShareDataRow["Guid"].ToString(),
                                            partnerRevenueShareDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(partnerRevenueShareDataRow["site_id"]),
                                            partnerRevenueShareDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(partnerRevenueShareDataRow["SynchStatus"]),
                                            partnerRevenueShareDataRow["AgentGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(partnerRevenueShareDataRow["AgentGroupId"]),
                                            partnerRevenueShareDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(partnerRevenueShareDataRow["MasterEntityId"]),
                                            partnerRevenueShareDataRow["CreatedBy"].ToString(),
                                            partnerRevenueShareDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(partnerRevenueShareDataRow["CreationDate"]),
                                            partnerRevenueShareDataRow["LastUpdatedBy"].ToString(),
                                            partnerRevenueShareDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(partnerRevenueShareDataRow["LastUpdateDate"]),
                                            partnerRevenueShareDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(partnerRevenueShareDataRow["IsActive"])
                                            );
            log.LogMethodExit(partnerRevenueShareDataObject);
            return partnerRevenueShareDataObject;
        }

        /// <summary>
        /// Gets the PartnerRevenueShare data of passed machineGroupId id
        /// </summary>
        /// <param name="partnerRevenueShareId">integer type parameter</param>
        /// <returns>Returns PartnerRevenueShareDTO</returns>
        public PartnerRevenueShareDTO GetPartnerRevenueShare(int partnerRevenueShareId)
        {
            log.LogMethodEntry(partnerRevenueShareId);
            string selectPartnerRevenueShareQuery = @"select * from PartnerRevenueShare where PartnerRevenueShareId = @partnerRevenueShareId";
            SqlParameter[] selectPartnerRevenueShareParameters = new SqlParameter[1];
            selectPartnerRevenueShareParameters[0] = new SqlParameter("@partnerRevenueShareId", partnerRevenueShareId);
            DataTable partnerRevenueShare = dataAccessHandler.executeSelectQuery(selectPartnerRevenueShareQuery, selectPartnerRevenueShareParameters);
            if (partnerRevenueShare.Rows.Count > 0)
            {
                DataRow partnerRevenueShareRow = partnerRevenueShare.Rows[0];
                PartnerRevenueShareDTO partnerRevenueShareDataObject = GetPartnerRevenueShareDTO(partnerRevenueShareRow);
                log.LogMethodExit(partnerRevenueShareDataObject);
                return partnerRevenueShareDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Gets the PartnerRevenueShareDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of PartnerRevenueShareDTO matching the search criteria</returns>
        public List<PartnerRevenueShareDTO> GetPartnerRevenueShareList(List<KeyValuePair<PartnerRevenueShareDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectPartnerRevenueShareQuery = @"select * from PartnerRevenueShare";
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<PartnerRevenueShareDTO> partnerRevenueShareList = new List<PartnerRevenueShareDTO>();

            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<PartnerRevenueShareDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";
                        if (searchParameter.Key.Equals(PartnerRevenueShareDTO.SearchByParameters.MACHINE_GROUP_ID) ||
                                searchParameter.Key.Equals(PartnerRevenueShareDTO.SearchByParameters.PARTNER_ID) ||
                                searchParameter.Key.Equals(PartnerRevenueShareDTO.SearchByParameters.POS_TYPE_ID))
                        {
                            //query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value);

                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(PartnerRevenueShareDTO.SearchByParameters.SITE_ID))
                        {
                            //query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + " =-1)");

                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(PartnerRevenueShareDTO.SearchByParameters.PARTNER_ID_LIST))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PartnerRevenueShareDTO.SearchByParameters.ISACTIVE) // column to be added
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            //query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "'%" + searchParameter.Value + "%'");
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        log.LogMethodExit();
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                if (searchParameters.Count > 0)
                    selectPartnerRevenueShareQuery = selectPartnerRevenueShareQuery + query;
            }

            DataTable partnerRevenueShareData = dataAccessHandler.executeSelectQuery(selectPartnerRevenueShareQuery, parameters.ToArray(), sqlTransaction);
            if (partnerRevenueShareData.Rows.Count > 0)
            {
                foreach (DataRow partnerRevenueShareDataRow in partnerRevenueShareData.Rows)
                {
                    PartnerRevenueShareDTO partnerRevenueShareDataObject = GetPartnerRevenueShareDTO(partnerRevenueShareDataRow);
                    partnerRevenueShareList.Add(partnerRevenueShareDataObject);
                }
                log.LogMethodExit(partnerRevenueShareList);
                return partnerRevenueShareList;
            }
            else
            {
                log.LogMethodExit();
                return partnerRevenueShareList;
            }
        }

        /// <summary>
        /// Converts the Data row object to PartnerRevenueDTODefination class type
        /// </summary>
        /// <param name="partnerRevenueShareDataRow">PartnerRevenueDTODefination DataRow</param>
        /// <returns>Returns PartnerRevenueShareDTO</returns>
        private PartnerRevenueDTODefination GetPartnerRevenueShareDTODefinition(DataRow partnerRevenueShareDataRow)
        {
            log.LogMethodEntry(partnerRevenueShareDataRow);

            PartnerRevenueDTODefination partnerRevenueShareDataObject = new PartnerRevenueDTODefination(Convert.ToString(partnerRevenueShareDataRow["Month"]),
                                            partnerRevenueShareDataRow["Partner"] == DBNull.Value ? string.Empty : Convert.ToString(partnerRevenueShareDataRow["Partner"]),
                                            partnerRevenueShareDataRow["Machine Group / POS Counter"] == DBNull.Value ? string.Empty : Convert.ToString(partnerRevenueShareDataRow["Machine Group / POS Counter"]),
                                            partnerRevenueShareDataRow["Agent Group Name"] == DBNull.Value ? string.Empty : Convert.ToString(partnerRevenueShareDataRow["Agent Group Name"]),
                                            partnerRevenueShareDataRow["total_amount"] == DBNull.Value ? string.Empty : Convert.ToString(partnerRevenueShareDataRow["total_amount"]),
                                            partnerRevenueShareDataRow["revenue_share_%"] == DBNull.Value ? string.Empty : Convert.ToString(partnerRevenueShareDataRow["revenue_share_%"]),
                                            partnerRevenueShareDataRow["share_amount"] == DBNull.Value ? string.Empty : Convert.ToString(partnerRevenueShareDataRow["share_amount"]),
                                            partnerRevenueShareDataRow["min_guarantee"] == DBNull.Value ? string.Empty : Convert.ToString(partnerRevenueShareDataRow["min_guarantee"]),
                                            partnerRevenueShareDataRow["final_amount"] == DBNull.Value ? string.Empty : Convert.ToString(partnerRevenueShareDataRow["final_amount"])

                                            );
            log.LogMethodExit(partnerRevenueShareDataObject);
            return partnerRevenueShareDataObject;
        }

        /// <summary>
        /// This method is used for
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="transactions"></param>
        /// <param name="gamePlays"></param>
        /// <returns></returns>
        public List<PartnerRevenueDTODefination> GetPartnerRevenueTable(DateTime fromdate, DateTime todate, string transactions, List<string> gamePlays)
        {
            log.LogMethodEntry(fromdate, todate);
            string sumCols = "";

            for (int i = 0; i < gamePlays.Count; i++)
            {
                sumCols += gamePlays[i] + " + ";
            }
            sumCols = "(" + sumCols + "0)";

            string TrxCols = "";
            if (transactions == "All")
                TrxCols = "sum(net_amount * (CashRatio + CreditCardRatio))";
            else
                TrxCols = "sum(net_amount * GameCardRatio)";

            string command =
                    "select month, partnername partner, groupname \"Machine Group / POS Counter\",  AgentGroupname \"Agent Group Name\", total_amount, " +
                    "RevenueSharePercentage \"revenue_share_%\", RevenueSharePercentage*total_amount/100.0 share_amount, " +
                    "minimumGuarantee min_guarantee, " +
                    "case when minimumGuarantee > RevenueSharePercentage*total_amount/100.0 then minimumGuarantee else RevenueSharePercentage*total_amount/100.0 end final_amount " +
                    "from ( " +
                    "select top 1000 DATENAME(MONTH, play_date) + '-' + DATENAME(year, play_date) Month, " +
                    "p.partnerName, isnull(mg.groupName, ' - All -') groupname, ag.GroupName AgentGroupname, " +
                    "SUM( " + sumCols + ") AS total_amount, " +
                    "prs.RevenueSharePercentage, prs.minimumGuarantee, " +
                    "left(CONVERT(VARCHAR(10), play_date, 102), 7) sort " +
                    " from gameplay g, partners p, partnerRevenueShare prs " +
                                    "inner join AgentGroups ag on ag.AgentGroupId=prs.AgentGroupId " +
                                    "left outer join machineGroups mg on prs.machinegroupid = mg.machineGroupId " +
                                    "left outer join machineGroupMachines mgm on mgm.machineGroupId = mg.machineGroupId " +
                    "where play_date >= @fromdate " +
                    "and play_date < @todate " +
                    "and prs.POSTypeId is null " +
                    "and credits+courtesy+bonus+time > 0 " +
                    "and p.partnerId = prs.partnerId " +
                    "and (mg.groupname is null or mgm.machineId = g.machine_id) " +
                    "group by DATENAME(MONTH, play_date) + '-' + DATENAME(year, play_date), " +
                            "left(CONVERT(VARCHAR(10), play_date, 102), 7), " +
                            "p.partnerName,ag.GroupName, mg.groupName, " +
                            "prs.RevenueSharePercentage, prs.minimumGuarantee " +
                    "order by sort) query1 " +
                    "union all " +
                    "select month, partnerName, POS_Counter,AgentGroupname   \"Agent Group Name\", total_amount, " +
                    "RevenueSharePercentage \"revenue_share_%\", RevenueSharePercentage*total_amount/100.0 share_amount, " +
                    "minimumGuarantee min_guarantee, " +
                    "case when minimumGuarantee > RevenueSharePercentage*total_amount/100.0 then minimumGuarantee else RevenueSharePercentage*total_amount/100.0 end final_amount " +
                    "from ( " +
                    "select top 1000 DATENAME(MONTH, trxDate) + '-' + DATENAME(year, trxDate) Month, " +
                    "p.partnerName, POS_Counter,ag.GroupName AgentGroupname, " +
                    TrxCols + " AS total_amount, " +
                    @"prs.RevenueSharePercentage, prs.minimumGuarantee, 
                                    left(CONVERT(VARCHAR(10), trxDate, 102), 7) sort 
                    from TransactionView v, partners p, partnerRevenueShare prs
                    inner join AgentGroups ag on ag.AgentGroupId=prs.AgentGroupId
					inner join AgentGroupAgents aga on aga.AgentGroupId=ag.AgentGroupId
					inner join Agents a on a.AgentId=aga.AgentId
                    where trxdate >= @fromdate and trxdate <@todate 
                    and v.POSTypeId = prs.POSTypeId 
                    and a.user_id =  v.user_id  and p.PartnerId = prs.PartnerId 
                    group by DATENAME(MONTH, trxDate) + '-' + DATENAME(year, trxDate), 
                            left(CONVERT(VARCHAR(10), trxDate, 102), 7), 
                            p.partnerName, POS_Counter, ag.GroupName,
                            prs.RevenueSharePercentage, prs.minimumGuarantee 
                    order by sort) query2 ";


            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@fromdate", fromdate));
            parameters.Add(new SqlParameter("@todate", todate));
            DataTable dtmonth = new DataTable();
            dtmonth = dataAccessHandler.executeSelectQuery(command, parameters.ToArray());

            List<PartnerRevenueDTODefination> partnerRevenueShareList = new List<PartnerRevenueDTODefination>();
            if (dtmonth.Rows.Count > 0)
            {
                // Calculating the Grand Total for all columns
                DataRow row = dtmonth.NewRow();
                row["Month"] = "GrandTotal";
                row["total_amount"] = Convert.ToDecimal(dtmonth.Compute("Sum(total_amount)", "total_amount > 0"));
                row["share_amount"] = dtmonth.Compute("Sum(share_amount)", "share_amount > 0");
                row["final_amount"] = dtmonth.Compute("Sum(final_amount)", "final_amount > 0");
                dtmonth.Rows.Add(row);

                foreach (DataRow partnerRevenueShareDataRow in dtmonth.Rows)
                {
                    PartnerRevenueDTODefination partnerRevenueShareDataObject = GetPartnerRevenueShareDTODefinition(partnerRevenueShareDataRow);
                    partnerRevenueShareList.Add(partnerRevenueShareDataObject);
                }
            }
            log.LogMethodExit(partnerRevenueShareList);
            return partnerRevenueShareList;
        }
    }
}
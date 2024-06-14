/********************************************************************************************
 * Project Name - Transaction
 * Description  - BookingCheckList Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.2        12-Nov-2019   Jinto Thomas            Created for waiver phase2 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    class BookingCheckListDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT bcl.*,  u.username as EventHostName, taskgroup.TaskGroupName as CheckListTaskGroupName 
                                                FROM BookingCheckList as bcl 
                                                     left outer join users u on u.user_id = bcl.EventHostUserId
                                                     left outer join Maint_TaskGroups taskgroup on bcl.ChecklistTaskGroupId = taskgroup.MaintTaskGroupId ";

        /// <summary>
        /// Dictionary for searching Parameters for the BookingCheckList object.
        /// </summary>
        private static readonly Dictionary<BookingCheckListDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<BookingCheckListDTO.SearchByParameters, string>
        {
            { BookingCheckListDTO.SearchByParameters.ID,"bcl.bookingCheckListId"},
            { BookingCheckListDTO.SearchByParameters.BOOKING_ID,"bcl.bookingId"},
            { BookingCheckListDTO.SearchByParameters.EVENT_HOST_USER_ID,"bcl.eventHostUserId"},
            { BookingCheckListDTO.SearchByParameters.CHECKLIST_TASK_GROUP_USER_ID,"bcl.checklistTaskGroupId"},
            { BookingCheckListDTO.SearchByParameters.IS_ACTIVE,"bcl.isActive"},
            { BookingCheckListDTO.SearchByParameters.SITE_ID,"bcl.site_Id"},
            { BookingCheckListDTO.SearchByParameters.MASTER_ENTITY_ID,"bcl.masterEntityId"},
        };

        /// <summary>
        /// Parameterized Constructor for BookingCheckListDataHandler.
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public BookingCheckListDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating BookingCheckList Record.
        /// </summary>
        /// <param name="bookingCheckListDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(BookingCheckListDTO bookingCheckListDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(bookingCheckListDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@BookingCheckListId", bookingCheckListDTO.BookingCheckListId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BookingId", bookingCheckListDTO.BookingId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@EventHostUserId", bookingCheckListDTO.EventHostUserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ChecklistTaskGroupId", bookingCheckListDTO.ChecklistTaskGroupId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", bookingCheckListDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", bookingCheckListDTO.SiteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", bookingCheckListDTO.MasterEntityId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId)); 
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to BookingCheckListDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns>BookingCheckListDTO</returns>
        private BookingCheckListDTO GetBookingCheckListDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            BookingCheckListDTO bookingCheckListDTO = new BookingCheckListDTO(dataRow["BookingCheckListId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BookingCheckListId"]),
                                                         dataRow["BookingId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BookingId"]),
                                                         dataRow["EventHostUserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["EventHostUserId"]),
                                                         dataRow["ChecklistTaskGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ChecklistTaskGroupId"]),
                                                         dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                         dataRow["EventHostName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EventHostName"]),
                                                         dataRow["CheckListTaskGroupName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CheckListTaskGroupName"])
                                                         );
            log.LogMethodExit(bookingCheckListDTO);
            return bookingCheckListDTO;
        }

        /// <summary>
        /// Gets the bookingCheckListDetails data of passed BookingCheckListId 
        /// </summary>
        /// <param name="bookingCheckListId">integer type parameter</param>
        /// <returns>Returns BookingCheckListDTO</returns>
        public BookingCheckListDTO GetBookingCheckListDTO(int bookingCheckListId)
        {
            log.LogMethodEntry(bookingCheckListId);
            BookingCheckListDTO result = null;
            string query = SELECT_QUERY + @" WHERE bcl.BookingCheckListId = @BookingCheckListId";
            SqlParameter parameter = new SqlParameter("@BookingCheckListId", bookingCheckListId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetBookingCheckListDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Updates the record to the CheckInDetail Table.
        /// </summary>
        /// <param name="bookingCheckListDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public BookingCheckListDTO Update(BookingCheckListDTO bookingCheckListDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(bookingCheckListDTO, loginId, siteId);
            string query = @"UPDATE BookingCheckList
                                   SET
                                    BookingId = @BookingId
                                   ,EventHostUserId = @EventHostUserId
                                   ,ChecklistTaskGroupId = @ChecklistTaskGroupId
                                   ,IsActive = @IsActive 
                                   ,MasterEntityId = @MasterEntityId 
                                   ,LastUpdatedBy = @LastUpdatedBy
                                   ,LastUpdateDate = getdate() 
                                where BookingCheckListId = @BookingCheckListId
                                SELECT * FROM BookingCheckList WHERE BookingCheckListId = @BookingCheckListId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(bookingCheckListDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshBookingCheckListDTO(bookingCheckListDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Updating BookingCheckListDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(bookingCheckListDTO);
            return bookingCheckListDTO;
        }

        /// <summary>
        ///  Inserts the record to the CheckInDetail Table.
        /// </summary>
        /// <param name="bookingCheckListDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public BookingCheckListDTO Insert(BookingCheckListDTO bookingCheckListDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(bookingCheckListDTO, loginId, siteId);
            string query = @"INSERT INTO BookingCheckList
                               (BookingId
                               ,EventHostUserId
                               ,ChecklistTaskGroupId
                               ,IsActive
                               ,Guid
                               ,site_id 
                               ,MasterEntityId
                               ,CreatedBy
                               ,CreationDate
                               ,LastUpdatedBy
                               ,LastUpdateDate )
                         VALUES
                               (@BookingId
                               ,@EventHostUserId
                               ,@ChecklistTaskGroupId
                               ,@IsActive
                               ,NEWID()
                               ,@SiteId 
                               ,@MasterEntityId
                               ,@CreatedBy
                               ,GETDATE()
                               ,@LastUpdatedBy
                               ,GETDATE() )
                        SELECT * FROM BookingCheckList WHERE BookingCheckListId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(bookingCheckListDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshBookingCheckListDTO(bookingCheckListDTO, dt, loginId, siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while inserting BookingCheckListDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(bookingCheckListDTO);
            return bookingCheckListDTO;
        }

        private void RefreshBookingCheckListDTO(BookingCheckListDTO bookingCheckListDTO, DataTable dt, string loginId, int siteId)
        {
            log.LogMethodEntry(bookingCheckListDTO, dt, loginId, siteId);
            if (dt.Rows.Count > 0)
            {
                bookingCheckListDTO.BookingCheckListId = Convert.ToInt32(dt.Rows[0]["BookingCheckListId"]);
                bookingCheckListDTO.LastUpdateDate = Convert.ToDateTime(dt.Rows[0]["LastUpdateDate"]);
                bookingCheckListDTO.CreationDate = Convert.ToDateTime(dt.Rows[0]["CreationDate"]);
                bookingCheckListDTO.Guid = Convert.ToString(dt.Rows[0]["Guid"]);
                bookingCheckListDTO.LastUpdatedBy = Convert.ToString(dt.Rows[0]["LastUpdatedBy"]);
                bookingCheckListDTO.CreatedBy = Convert.ToString(dt.Rows[0]["CreatedBy"]);
                bookingCheckListDTO.SiteId = siteId;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of BookingCheckListDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<BookingCheckListDTO> GetAllBookingCheckListDTOList(List<KeyValuePair<BookingCheckListDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<BookingCheckListDTO> bookingCheckListDTOList = new List<BookingCheckListDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<BookingCheckListDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == BookingCheckListDTO.SearchByParameters.ID
                            || searchParameter.Key == BookingCheckListDTO.SearchByParameters.BOOKING_ID
                            || searchParameter.Key == BookingCheckListDTO.SearchByParameters.EVENT_HOST_USER_ID
                            || searchParameter.Key == BookingCheckListDTO.SearchByParameters.CHECKLIST_TASK_GROUP_USER_ID
                            || searchParameter.Key == BookingCheckListDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == BookingCheckListDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == BookingCheckListDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    BookingCheckListDTO bookingCheckListDTO = GetBookingCheckListDTO(dataRow);
                    bookingCheckListDTOList.Add(bookingCheckListDTO);
                }
            }
            log.LogMethodExit(bookingCheckListDTOList);
            return bookingCheckListDTOList;
        }

        ///// <summary>
        ///// Gets the List of CheckInDetail DTO
        ///// </summary>
        ///// <param name="bookingCheckList"></param>
        ///// <returns>checkInDetailDTOList</returns>
        //public List<BookingCheckListDTO> GetAllBookingCheckListDTOList(List<int> bookingCheckList)
        //{
        //    log.LogMethodEntry(bookingCheckList);
        //    string query = SELECT_QUERY + " INNER JOIN @bookingCheckList List ON bcl.bookingId = List.Id ";
        //    DataTable dataTable = dataAccessHandler.BatchSelect(query, "@bookingCheckList", bookingCheckList, null, sqlTransaction);
        //    List<BookingCheckListDTO> bookingCheckListDTOList = GetBookingCheckListDTOList(dataTable);
        //    log.LogMethodExit(bookingCheckListDTOList);
        //    return bookingCheckListDTOList;
        //}
        //private List<BookingCheckListDTO> GetBookingCheckListDTOList(DataTable dataTable)
        //{
        //    log.LogMethodEntry(dataTable);
        //    List<BookingCheckListDTO> bookingCheckListDTOList = new List<BookingCheckListDTO>();
        //    if (dataTable.Rows.Count > 0)
        //    {
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            BookingCheckListDTO checkInDetailDTO = GetBookingCheckListDTO(dataRow);
        //            bookingCheckListDTOList.Add(checkInDetailDTO);
        //        }
        //    }
        //    log.LogMethodExit(bookingCheckListDTOList);
        //    return bookingCheckListDTOList;
        //}

    }
}

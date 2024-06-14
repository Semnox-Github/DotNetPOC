/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Data handler Maintenance Images
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
 *2.150.3     21-Mar-2022     Abhishek       Created 
 ********************************************************************************************/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Linq;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// MaintenanceImages Data Handler - Handles insert, update and select of image objects
    /// </summary>
    public class MaintenanceImagesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM Images AS img ";

        /// <summary>
        /// Dictionary for searching Parameters for the Images object.
        /// </summary>

        private static readonly Dictionary<MaintenanceImagesDTO.SearchByImagesParameters, string> DBSearchParameters = new Dictionary<MaintenanceImagesDTO.SearchByImagesParameters, string>
        {
                {MaintenanceImagesDTO.SearchByImagesParameters.IMAGE_ID, "img.ImageId"},
                {MaintenanceImagesDTO.SearchByImagesParameters.MAINT_CHECK_LIST_DETAIL_ID, "img.MaintChklstdetId"},
                {MaintenanceImagesDTO.SearchByImagesParameters.IMAGE_TYPE, "img.ImageType"},
                {MaintenanceImagesDTO.SearchByImagesParameters.IMAGE, "img.ImageFileName"},
                {MaintenanceImagesDTO.SearchByImagesParameters.IS_ACTIVE, "img.IsActive"},
                {MaintenanceImagesDTO.SearchByImagesParameters.MASTER_ENTITY_ID,"img.MasterEntityId"},
                {MaintenanceImagesDTO.SearchByImagesParameters.SITE_ID, "img.site_id"}
        };

        /// <summary>
        /// Parameterized Constructor for MaintenanceImagesDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public MaintenanceImagesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the images record to the database
        /// </summary>
        /// <param name="maintenanceImagesDTO">MaintenanceImagesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted MaintenanceImagesDTO</returns>
        public MaintenanceImagesDTO Insert(MaintenanceImagesDTO maintenanceImagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(maintenanceImagesDTO, loginId, siteId);
            string insertImagesQuery = @"insert into Images 
                                                        ( 
                                                        MaintChklstdetId,
                                                        ImageType,
                                                        ImageFileName,
                                                        MasterEntityId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id
                                                        ) 
                                                     values 
                                                        (
                                                         @maintCheckListDetailId,
                                                         @imageType,
                                                         @imageFileName,
                                                         @masterEntityId,
                                                         @isActive,
                                                         @CreatedBy,
                                                         Getdate(),                                                         
                                                         @CreatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @site_id
                                                         )SELECT * FROM Images WHERE ImageId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertImagesQuery, GetSQLParameters(maintenanceImagesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshImagesDTO(maintenanceImagesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting MaintenanceImagesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(maintenanceImagesDTO);
            return maintenanceImagesDTO;
        }


        /// <summary>
        /// Updates the Images record
        /// </summary>
        /// <param name="maintenanceImagesDTO">MaintenanceImagesDTO type parameter</param>
        /// <param name="loginId">Login Id</param>
        /// <param name="siteId">Site Id</param>
        /// <returns>Returns the MaintenanceImagesDTO</returns>
        public MaintenanceImagesDTO Update(MaintenanceImagesDTO maintenanceImagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(maintenanceImagesDTO, loginId, siteId);
            string updateImagesQuery = @"update Images 
                                          set MaintChklstdetId = @maintCheckListDetailId,
                                              ImageType = @imageType,
                                              ImageFileName = @imageFileName,
                                              MasterEntityId = @masterEntityId,
                                              IsActive = @isActive, 
                                              LastUpdatedBy = @lastUpdatedBy, 
                                              LastupdatedDate = Getdate()
                                              --site_id = @site_id                                           
                                              where  ImageId = @imageId
                                              SELECT* FROM Images WHERE ImageId = @imageId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateImagesQuery, GetSQLParameters(maintenanceImagesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshImagesDTO(maintenanceImagesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating MaintenanceImagesDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(maintenanceImagesDTO);
            return maintenanceImagesDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="maintenanceImagesDTO">MaintenanceImagesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshImagesDTO(MaintenanceImagesDTO maintenanceImagesDTO, DataTable dt)
        {
            log.LogMethodEntry(maintenanceImagesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                maintenanceImagesDTO.ImageId = Convert.ToInt32(dt.Rows[0]["ImageId"]);
                maintenanceImagesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                maintenanceImagesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                maintenanceImagesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                maintenanceImagesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                maintenanceImagesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                maintenanceImagesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Images Record.
        /// </summary>
        /// <param name="maintenanceImagesDTO">MaintenanceImagesDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(MaintenanceImagesDTO maintenanceImagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(maintenanceImagesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@imageId", maintenanceImagesDTO.ImageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@maintCheckListDetailId", maintenanceImagesDTO.MaintCheckListDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@imageType", maintenanceImagesDTO.ImageType,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@imageFileName", maintenanceImagesDTO.ImageFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", maintenanceImagesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", maintenanceImagesDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Converts the Data row object to MaintenanceImagesDTO class type
        /// </summary>
        /// <param name="imagesDataRow">Images DataRow</param>
        /// <returns>Returns ImagesDTO</returns>
        private MaintenanceImagesDTO GetMaintenanceImagesDTO(DataRow imagesDataRow)
        {
            log.LogMethodEntry(imagesDataRow);
            MaintenanceImagesDTO assetTypeDTO = new MaintenanceImagesDTO(imagesDataRow["ImageId"] == DBNull.Value ? -1 : Convert.ToInt32(imagesDataRow["ImageId"]),
                                            imagesDataRow["MaintChklstdetId"] == DBNull.Value ? -1 : Convert.ToInt32(imagesDataRow["MaintChklstdetId"]),
                                            imagesDataRow["ImageType"] == DBNull.Value ? -1 : Convert.ToInt32(imagesDataRow["ImageType"]),
                                            imagesDataRow["ImageFileName"].ToString(),
                                            imagesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(imagesDataRow["MasterEntityId"]),
                                            imagesDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(imagesDataRow["IsActive"]),
                                            imagesDataRow["CreatedBy"].ToString(),
                                            imagesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(imagesDataRow["CreationDate"]),
                                            imagesDataRow["LastUpdatedBy"].ToString(),
                                            imagesDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(imagesDataRow["LastupdatedDate"]),
                                            imagesDataRow["Guid"].ToString(),
                                            imagesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(imagesDataRow["site_id"]),
                                            imagesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(imagesDataRow["SynchStatus"])
                                            );
            log.LogMethodExit(assetTypeDTO);
            return assetTypeDTO;
        }

        /// <summary>
        /// Gets the Images data of passed image Id
        /// </summary>
        /// <param name="imageId">integer type parameter</param>
        /// <returns>Returns MaintenanceImagesDTO</returns>
        public MaintenanceImagesDTO GetMaintenanceImages(int imageId)
        {
            log.LogMethodEntry(imageId);
            MaintenanceImagesDTO result = null;
            string query = SELECT_QUERY + " WHERE img.ImageId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", imageId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetMaintenanceImagesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of MaintenanceImagesDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of MaintenanceImagesDTO</returns>
        public List<MaintenanceImagesDTO> GetMaintenanceImagesDTOList(List<KeyValuePair<MaintenanceImagesDTO.SearchByImagesParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<MaintenanceImagesDTO> maintenanceImagesDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<MaintenanceImagesDTO.SearchByImagesParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == MaintenanceImagesDTO.SearchByImagesParameters.IMAGE_ID
                            || searchParameter.Key == MaintenanceImagesDTO.SearchByImagesParameters.MAINT_CHECK_LIST_DETAIL_ID
                             || searchParameter.Key == MaintenanceImagesDTO.SearchByImagesParameters.IMAGE_TYPE
                            || searchParameter.Key == MaintenanceImagesDTO.SearchByImagesParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MaintenanceImagesDTO.SearchByImagesParameters.IMAGE)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == MaintenanceImagesDTO.SearchByImagesParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == MaintenanceImagesDTO.SearchByImagesParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                maintenanceImagesDTOList = new List<MaintenanceImagesDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    MaintenanceImagesDTO maintenanceImagesDTO = GetMaintenanceImagesDTO(dataRow);
                    maintenanceImagesDTOList.Add(maintenanceImagesDTO);
                }
            }
            log.LogMethodExit(maintenanceImagesDTOList);
            return maintenanceImagesDTOList;
        }

        /// <summary>
        /// Gets the MaintenanceImagesDTO List for maintChklstdet Id List
        /// </summary>
        /// <param name="maintChklstdetIdList">integer list parameter</param>
        /// <returns>Returns List of MaintenanceImagesDTOList</returns>
        public List<MaintenanceImagesDTO> GetMaintenanceImagesDTOList(List<int> maintChklstdetIdList, bool activeRecords)
        {
            log.LogMethodEntry(maintChklstdetIdList, activeRecords);
            List<MaintenanceImagesDTO> list = new List<MaintenanceImagesDTO>();
            string query = @"SELECT Images.*
                            FROM Images, @maintChklstdetIdList List
                            WHERE MaintChklstdetId = List.id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@maintChklstdetIdList", maintChklstdetIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetMaintenanceImagesDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}

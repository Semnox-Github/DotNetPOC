/********************************************************************************************
 * Project Name - Media Data Handler
 * Description  - Data handler of the media class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Feb-2017   Raghuveera     Created
 *2.70.2        31-Jul-2019   Dakshakh raj   Modified : added GetSQLParameters(),
 *                                                    SQL injection Issue Fix
 *2.70.2       06-Dec-2019   Jinto Thomas            Removed siteid from update query                                                     
 *2.100.0      12-Aug-2020   Mushahid Faizan     Modified : default isActive value to true.
 *2.110.0      27-Nov-2020   Prajwal S           Modified : Added GetMedia(int mediaId).
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Media Data Handler - Handles insert, update and select of media objects
    /// </summary>
    public class MediaDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM Media AS me";

        /// <summary>
        /// Dictionary for searching Parameters for the Media object.
        /// </summary>
        private static readonly Dictionary<MediaDTO.SearchByMediaParameters, string> DBSearchParameters = new Dictionary<MediaDTO.SearchByMediaParameters, string>
            {
                {MediaDTO.SearchByMediaParameters.NAME, "me.Name"},
                {MediaDTO.SearchByMediaParameters.MEDIA_ID, "me.MediaId"},
                {MediaDTO.SearchByMediaParameters.TYPE_ID, "me.Type"},
                {MediaDTO.SearchByMediaParameters.FILE_NAME, "me.File_Name"},
                {MediaDTO.SearchByMediaParameters.IS_ACTIVE, "me.Active_Flag"},
                {MediaDTO.SearchByMediaParameters.MASTER_ENTITY_ID,"me.MasterEntityId"},
                {MediaDTO.SearchByMediaParameters.SITE_ID, "me.site_id"}
            };

        /// <summary>
        /// Default constructor of MediaDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public MediaDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating mediaDTO parameters Record.
        /// </summary>
        /// <param name="mediaDTO">mediaDTO</param>
        /// <param name="loginId">loginId</param>
        /// <param name="siteId">siteId</param>
        /// <returns>  Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(MediaDTO mediaDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(mediaDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@mediaId", mediaDTO.MediaId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@name", string.IsNullOrEmpty(mediaDTO.Name) ? DBNull.Value : (object)mediaDTO.Name));
            parameters.Add(dataAccessHandler.GetSQLParameter("@type", mediaDTO.TypeId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@fileName", string.IsNullOrEmpty(mediaDTO.FileName) ? DBNull.Value : (object)mediaDTO.FileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", (mediaDTO.IsActive == true ? "Y" : "N")));
            parameters.Add(dataAccessHandler.GetSQLParameter("@description", string.IsNullOrEmpty(mediaDTO.Description) ? DBNull.Value : (object)mediaDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sizeXinPixels", mediaDTO.SizeXinPixels == null ? DBNull.Value : (object)mediaDTO.SizeXinPixels));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sizeYinPixels", mediaDTO.SizeYinPixels == null ? DBNull.Value : (object)mediaDTO.SizeYinPixels));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", mediaDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the media record to the database
        /// </summary>
        /// <param name="media">MediaDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Media DTO</returns>
        public MediaDTO InsertMedia(MediaDTO media, string loginId, int siteId)
        {
            log.LogMethodEntry(media, loginId, siteId);
            string insertMediaQuery = @"insert into Media 
                                                        ( 
                                                         Name,
                                                         Type,
                                                         File_Name,
                                                         Active_Flag,
                                                         Description,
                                                         site_id,
                                                         guid,
                                                         last_updated_user,
                                                         last_updated_date,
                                                         CreatedUser,
                                                         CreationDate,
                                                         SizeXinPixels,
                                                         SizeYinPixels,
                                                         MasterEntityId
                                                        ) 
                                                values 
                                                        (
                                                         @name,
                                                         @type,
                                                         @fileName,
                                                         @isActive,
                                                         @description,
                                                         @siteId,
                                                         NewId(),
                                                         @lastUpdatedBy,
                                                         getdate(),
                                                         @createdBy,
                                                         getdate(),
                                                         @sizeXinPixels,
                                                         @sizeYinPixels,
                                                         @masterEntityId
                                                        )SELECT * FROM Media WHERE MediaId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertMediaQuery, GetSQLParameters(media, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMediaDTO(media, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting media", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(media);
            return media;
        }

        /// <summary>
        /// Updates the Media record
        /// </summary>
        /// <param name="media">MediaDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Media DTO</returns>
        public MediaDTO UpdateMedia(MediaDTO media, string loginId, int siteId)
        {
            log.LogMethodEntry(media, loginId, siteId);
            string updateMediaQuery = @"update Media 
                                         set Name=@name,
                                             Type=@type,
                                             File_Name=@fileName,
                                             Active_Flag=@isActive,
                                             Description=@description,
                                             --site_id=@siteId,
                                             last_updated_user=@lastUpdatedBy,
                                             last_updated_date=getdate(),
                                             SizeXinPixels =@sizeXinPixels,
                                             SizeYinPixels = @sizeYinPixels,
                                             MasterEntityId = @masterEntityId                                         
                                       where MediaId = @mediaId
                                       SELECT* FROM Media WHERE MediaId = @mediaId";
            try
            {
                if(string.Equals(media.IsActive, "N") && GetMediaReferenceCount(media.MediaId) > 0)
            {
                throw new ForeignKeyException("Cannot Inactivate records for which matching detail data exists.");
            }
                DataTable dt = dataAccessHandler.executeSelectQuery(updateMediaQuery, GetSQLParameters(media, loginId, siteId).ToArray(), sqlTransaction);
                RefreshMediaDTO(media, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating MediaDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(media);
            return media;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="mediaDTO">mediaDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshMediaDTO(MediaDTO mediaDTO, DataTable dt)
        {
            log.LogMethodEntry(mediaDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                mediaDTO.MediaId = Convert.ToInt32(dt.Rows[0]["MediaId"]);
                mediaDTO.LastUpdateDate = dataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["last_updated_date"]);
                mediaDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                mediaDTO.Guid = dataRow["guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["guid"]);
                mediaDTO.LastUpdatedUser = dataRow["last_updated_user"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["last_updated_user"]);
                mediaDTO.CreatedBy = dataRow["CreatedUser"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedUser"]);
                mediaDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether media is in use.
        /// <param name="id">Media Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        private int GetMediaReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT COUNT(*) AS ReferenceCount
                             FROM ScreenZoneContentMap
                             WHERE (((ContentType='IMAGE' OR ContentType='VIDEO' OR ContentType='AUDIO') AND ContentID = @MediaId) OR (BackImage = @MediaId)) AND Active_Flag = 'Y'";
            SqlParameter parameter = new SqlParameter("@MediaId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if(dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        /// <summary>
        /// Converts the Data row object to MediaDTO class type
        /// </summary>
        /// <param name="mediaDataRow">Media DataRow</param>
        /// <returns>Returns Media</returns>
        private MediaDTO GetMediaDTO(DataRow mediaDataRow)
        {
            log.LogMethodEntry(mediaDataRow);
            MediaDTO mediaDataObject = new MediaDTO(Convert.ToInt32(mediaDataRow["MediaId"]),
                                            mediaDataRow["Name"].ToString(),
                                            mediaDataRow["Type"] == DBNull.Value ? -1 : Convert.ToInt32(mediaDataRow["Type"]),
                                            mediaDataRow["File_Name"].ToString(),                                            
                                            mediaDataRow["Description"].ToString(),
                                            mediaDataRow["SizeXinPixels"] == DBNull.Value ? (int?)null : Convert.ToInt32(mediaDataRow["SizeXinPixels"]),
                                            mediaDataRow["SizeYinPixels"] == DBNull.Value ? (int?)null : Convert.ToInt32(mediaDataRow["SizeYinPixels"]),
                                            mediaDataRow["Active_Flag"] == DBNull.Value ? true : (mediaDataRow["Active_Flag"].ToString()=="Y"? true: false), 
                                            mediaDataRow["CreatedUser"].ToString(),
                                            mediaDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(mediaDataRow["CreationDate"]),
                                            mediaDataRow["last_updated_user"].ToString(),
                                            mediaDataRow["last_updated_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(mediaDataRow["last_updated_date"]),
                                            mediaDataRow["Guid"].ToString(),
                                            mediaDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(mediaDataRow["site_id"]),
                                            mediaDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(mediaDataRow["SynchStatus"]),
                                            mediaDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(mediaDataRow["MasterEntityId"])
                                            );
            log.LogMethodExit(mediaDataObject);
            return mediaDataObject;
        }

        /// <summary>
        /// Gets the Media data of passed media media Group Id
        /// </summary>
        /// <param name="mediaId">integer type parameter</param>
        /// <returns>Returns MediaDTO</returns>
        public MediaDTO GetMedia(int mediaId)
        {
            log.LogMethodEntry(mediaId);
            string selectMediaQuery = SELECT_QUERY + @" WHERE me.MediaId = @mediaId";
            SqlParameter[] selectMediaParameters = new SqlParameter[1];
            selectMediaParameters[0] = new SqlParameter("@mediaId", mediaId);
            DataTable media = dataAccessHandler.executeSelectQuery(selectMediaQuery, selectMediaParameters, sqlTransaction);
            if (media.Rows.Count > 0)
            {
                DataRow mediaRow = media.Rows[0];
                MediaDTO mediaDataObject = GetMediaDTO(mediaRow);
                log.LogMethodExit(mediaDataObject);
                return mediaDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the Media data of passed Contents Guid
        /// </summary>
        /// <param name="contentGuid">integer type parameter</param>
        /// <returns>Returns MediaDTO</returns>
        public MediaDTO GetMediaByGuid(string contentGuid)
        {
            log.LogMethodEntry(contentGuid);
            string selectMediaQuery = SELECT_QUERY + @" WHERE me.guid = @contentGuid";
            SqlParameter[] selectMediaParameters = new SqlParameter[1];
            selectMediaParameters[0] = new SqlParameter("@contentGuid", contentGuid);
            DataTable media = dataAccessHandler.executeSelectQuery(selectMediaQuery, selectMediaParameters);
            if (media.Rows.Count > 0)
            {
                DataRow mediaRow = media.Rows[0];
                MediaDTO mediaDataObject = GetMediaDTO(mediaRow);
                log.LogMethodExit(mediaDataObject);
                return mediaDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of MediaDTO matching the search criteria</returns>
        public List<MediaDTO> GetMediaList(List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<MediaDTO> mediaList = null;
            List <SqlParameter> parameters = new List<SqlParameter>();
            string selectMediaQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<MediaDTO.SearchByMediaParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == MediaDTO.SearchByMediaParameters.MEDIA_ID 
                            || searchParameter.Key == MediaDTO.SearchByMediaParameters.MASTER_ENTITY_ID 
                            || searchParameter.Key == MediaDTO.SearchByMediaParameters.TYPE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MediaDTO.SearchByMediaParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == MediaDTO.SearchByMediaParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                selectMediaQuery = selectMediaQuery + query;
            }
            DataTable mediaData = dataAccessHandler.executeSelectQuery(selectMediaQuery, parameters.ToArray(), sqlTransaction);
            if (mediaData.Rows.Count > 0)
            {
                mediaList = new List<MediaDTO>();
                foreach (DataRow mediaDataRow in mediaData.Rows)
                {
                    MediaDTO mediaDataObject = GetMediaDTO(mediaDataRow);
                    mediaList.Add(mediaDataObject);
                }
               
            }
            log.LogMethodExit(mediaList);
            return mediaList;
        }        
    }

    
}

/********************************************************************************************
 * Project Name - Site                                                                       
 * Description  - Site Detail Data Handler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar     Created : Urban Piper changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Site
{
    public class SiteDetailBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SiteDetailDTO siteDetailDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        private SiteDetailBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="parameterSiteDetailDTO">parameterSiteDetailDTO</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public SiteDetailBL(ExecutionContext executionContext, SiteDetailDTO parameterSiteDetailDTO, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterSiteDetailDTO, sqlTransaction);

            if (parameterSiteDetailDTO.SiteDetailId > -1)
            {
                LoadSiteDetailDTO(parameterSiteDetailDTO.SiteDetailId, sqlTransaction);//added sql
                ThrowIfUserDTOIsNull(parameterSiteDetailDTO.SiteDetailId);
                Update(parameterSiteDetailDTO);
            }
            else
            {
                Validate(sqlTransaction);
                siteDetailDTO = new SiteDetailDTO(-1, parameterSiteDetailDTO.ParentSiteId, parameterSiteDetailDTO.DeliveryChannelId, parameterSiteDetailDTO.OnlineChannelStartHour,
                                                       parameterSiteDetailDTO.OnlineChannelEndHour, parameterSiteDetailDTO.OrderDeliveryType, parameterSiteDetailDTO.ZipCodes,
                                                       parameterSiteDetailDTO.IsActive);
            }
            log.LogMethodExit();
        }
        private void ThrowIfUserDTOIsNull(int id)
        {
            log.LogMethodEntry();
            if (siteDetailDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "SiteDetails", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }
        private void LoadSiteDetailDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            SiteDetailDataHandler siteDetailDataHandler = new SiteDetailDataHandler(sqlTransaction);
            siteDetailDTO = siteDetailDataHandler.GetSiteDetailDTO(id);
            ThrowIfUserDTOIsNull(id);
            log.LogMethodExit();
        }

        private void Update(SiteDetailDTO parameterSiteDetailDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterSiteDetailDTO);
            siteDetailDTO.DeliveryChannelId = parameterSiteDetailDTO.DeliveryChannelId;
            siteDetailDTO.SiteDetailId = parameterSiteDetailDTO.SiteDetailId;
            siteDetailDTO.ParentSiteId = parameterSiteDetailDTO.ParentSiteId;
            siteDetailDTO.OnlineChannelStartHour = parameterSiteDetailDTO.OnlineChannelStartHour;
            siteDetailDTO.OnlineChannelEndHour = parameterSiteDetailDTO.OnlineChannelEndHour;
            siteDetailDTO.OrderDeliveryType = parameterSiteDetailDTO.OrderDeliveryType;
            siteDetailDTO.ZipCodes = parameterSiteDetailDTO.ZipCodes;
            siteDetailDTO.IsActive = parameterSiteDetailDTO.IsActive;
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            // Validation code here 
            // return validation exceptions
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public SiteDetailBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id);
            LoadSiteDetailDTO(id, sqlTransaction);
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            SaveImpl(sqlTransaction, true);
            log.LogMethodExit();
        }
        private void SaveImpl(SqlTransaction sqlTransaction, bool updateWhoColumns)
        {
            log.LogMethodEntry(sqlTransaction, updateWhoColumns);
            SiteDetailDataHandler siteDetailDataHandler = new SiteDetailDataHandler(sqlTransaction);
            if (siteDetailDTO.SiteDetailId < 0)
            {
                siteDetailDTO = siteDetailDataHandler.Insert(siteDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                siteDetailDTO.AcceptChanges();
            }
            else
            {
                if (siteDetailDTO.IsChanged)
                {
                    if (updateWhoColumns)
                    {
                        siteDetailDTO = siteDetailDataHandler.Update(siteDetailDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    }
                    else
                    {
                        siteDetailDTO = siteDetailDataHandler.UpdateSiteDetailDTO(siteDetailDTO);
                    }
                    siteDetailDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get SiteDetailDTO Object
        /// </summary>
        public SiteDetailDTO SiteDetailDTO
        {
            get
            {
                SiteDetailDTO result = new SiteDetailDTO(siteDetailDTO);
                return result;
            }
        }

    }

    /// <summary>
    /// SiteDetailListBL list class for order details
    /// </summary>
    public class SiteDetailListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<SiteDetailDTO> siteDetailDTOList;

        /// <summary>
        /// default constructor
        /// </summary>
        public SiteDetailListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public SiteDetailListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="siteDetailDTOList"></param>
        public SiteDetailListBL(ExecutionContext executionContext, List<SiteDetailDTO> siteDetailDTOList)
        {
            log.LogMethodEntry(executionContext, siteDetailDTOList);
            this.siteDetailDTOList = siteDetailDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<SiteDetailDTO> GetSiteDetails(List<KeyValuePair<SiteDetailDTO.SearchByParameters, string>> searchParameters,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            SiteDetailDataHandler siteDetailDataHandler = new SiteDetailDataHandler(sqlTransaction);
            List<SiteDetailDTO> siteDetailDTOList = siteDetailDataHandler.GetSiteDetails(searchParameters);
            log.LogMethodExit(siteDetailDTOList);
            return siteDetailDTOList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<SiteDetailDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<SiteDetailDTO> savedSiteDetailDTOList = new List<SiteDetailDTO>();

            try
            {
                if (siteDetailDTOList != null && siteDetailDTOList.Any())
                {
                    foreach (SiteDetailDTO siteDetailDTO in siteDetailDTOList)
                    {
                        SiteDetailBL siteDetailBL = new SiteDetailBL(executionContext, siteDetailDTO);
                        siteDetailBL.Save(sqlTransaction);
                        savedSiteDetailDTOList.Add(siteDetailBL.SiteDetailDTO);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                log.Error(sqlEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                if (sqlEx.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                if (sqlEx.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
                else
                {
                    throw;
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }

            log.LogMethodExit(savedSiteDetailDTOList);
            return savedSiteDetailDTOList;
        }

        /// <summary>
        /// Gets the SiteDetailDTO List for siteIdList
        /// </summary>
        /// <param name="siteIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of SiteDetailDTO</returns>
        public List<SiteDetailDTO> GetSiteDetailDTOList(List<int> siteIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteIdList, activeRecords, sqlTransaction);
            SiteDetailDataHandler siteDetailDataHandler = new SiteDetailDataHandler(sqlTransaction);
            List<SiteDetailDTO> siteDetailDTOList = siteDetailDataHandler.GetSiteDetailDTOList(siteIdList, activeRecords);
            log.LogMethodExit(siteDetailDTOList);
            return siteDetailDTOList;
        }
    }
}

/********************************************************************************************
 * Project Name - kiosk setup
 * Description  - Bussiness logic of kiosk
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.60        18-Mar-2016   Jagan Mohana      Created 
              23-Apr-2019   Mushahid Faizan   Modified : SaveUpdateKioskSetupsList() method & Added LogMethodEntry/Exit, removed Unused Namespaces.
*2.70.2         26-Jul-2019   Dakshakh raj      Modified : Log method entries/exits, Save method.
              29-Jul-2019   Mushahid Faizan Added Delete in Save() Method for Hard - Deletion.
*2.80      03-Apr-2020   Mushahid Faizan    Added Delete Method for Web Management Studio.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Core.GenericUtilities
{
    public class KioskSetupBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KioskSetupDTO kioskSetupDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="KioskSetupDTO">KioskSetupDTO</param>
        public KioskSetupBL(ExecutionContext executionContext, KioskSetupDTO kioskSetupDTO)
        {
            log.LogMethodEntry(executionContext, kioskSetupDTO);
            this.executionContext = executionContext;
            this.kioskSetupDTO = kioskSetupDTO;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Saves the koisk setups
        /// Checks if the id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            KioskSetupDataHandler kioskSetupDataHandler = new KioskSetupDataHandler(sqlTransaction);
           //if (kioskSetupDTO.Active)
            //{
                if (kioskSetupDTO.Id < 0)
                {
                    kioskSetupDTO = kioskSetupDataHandler.InsertKioskSetup(kioskSetupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    kioskSetupDTO.AcceptChanges();
                }
                else
                {
                    if (kioskSetupDTO.IsChanged)
                    {
                        kioskSetupDTO = kioskSetupDataHandler.UpdateKioskSetup(kioskSetupDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        kioskSetupDTO.AcceptChanges();
                    }
                }
            //}
            //else
            //{
            //    if (kioskSetupDTO.Id >= 0)
            //    {
            //        kioskSetupDataHandler.Delete(kioskSetupDTO.Id);
            //    }
            //}
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the Kiosk Setup records from database based on Id
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            try
            {
                KioskSetupDataHandler kioskSetupDataHandler = new KioskSetupDataHandler(sqlTransaction);
                log.LogMethodExit();
                kioskSetupDataHandler.Delete(id);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

    }

    /// <summary>
    /// Manages the list of KioskSetup List
    /// </summary>
    public class KioskSetupList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<KioskSetupDTO> kioskSetupDTOList;
        private KioskSetupDTO kioskSetupDTO;

        /// <summary>
        /// Parameterized Constructor having executionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public KioskSetupList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.kioskSetupDTOList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="kioskSetupDTOList">kioskSetupDTOList</param>
        public KioskSetupList(ExecutionContext executionContext, List<KioskSetupDTO> kioskSetupDTOList)
        {
            log.LogMethodEntry(executionContext, kioskSetupDTOList);
            this.kioskSetupDTOList = kioskSetupDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Returns the KioskSetup list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<KioskSetupDTO> GetAllKioskSetupsList(List<KeyValuePair<KioskSetupDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            KioskSetupDataHandler kioskSetupDataHandler = new KioskSetupDataHandler(sqlTransaction);
            List<KioskSetupDTO> kioskSetupDTOList = kioskSetupDataHandler.GetAllKioskSetup(searchParameters);
            log.LogMethodExit(kioskSetupDTOList);
            return kioskSetupDTOList;

        }

        /// <summary>
        ///  This method should be used to Save and Update the Kiosk Setup details for Web Management Studio.
        /// </summary>
        public void SaveUpdateKioskSetupsList()
        {
            log.LogMethodEntry();
            try
            {
                if (kioskSetupDTOList != null)
                {
                    foreach (KioskSetupDTO KioskSetupDTO in kioskSetupDTOList)
                    {
                        KioskSetupBL kioskSetupBL = new KioskSetupBL(executionContext, KioskSetupDTO);
                        kioskSetupBL.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the kioskSetupDTOList based on Id
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (kioskSetupDTOList != null && kioskSetupDTOList.Count > 0)
            {
                foreach (KioskSetupDTO KioskSetupDTO in kioskSetupDTOList)
                {
                    if (KioskSetupDTO.IsChanged)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();

                                KioskSetupBL kioskSetupBL = new KioskSetupBL(executionContext, KioskSetupDTO);
                                kioskSetupBL.Delete(KioskSetupDTO.Id, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (SqlException sqlEx)
                            {
                                log.Error(sqlEx);
                                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                                if (sqlEx.Number == 547)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                                }
                                else
                                {
                                    throw;
                                }
                            }
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
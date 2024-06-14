/********************************************************************************************
 * Project Name - POS 
 * Description  - LocalPOSTypeUseCases class to get the data  from local DB 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0          27-Nov-2020      Vikas Dwivedi             Created : POS UI Redesign with REST API
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Semnox.Parafait.POS
{
    public class LocalPOSTypeUseCases : LocalUseCases, IPOSTypeUseCases
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalPOSTypeUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<POSTypeDTO>> GetPOSTypes(List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> parameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<POSTypeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext);
                List<POSTypeDTO> pOSTypeDTOList = pOSTypeListBL.GetPOSTypeDTOList(parameters, sqlTransaction);
                log.LogMethodExit(pOSTypeDTOList);
                return pOSTypeDTOList;
            });
        }

        public async Task<string> SavePOSTypes(List<POSTypeDTO> pOSTypeDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(pOSTypeDTOList);
                    if (pOSTypeDTOList == null)
                    {
                        throw new ValidationException("pOSTypeDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (POSTypeDTO pOSTypeDTO in pOSTypeDTOList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                POSTypeBL pOSTypeBL = new POSTypeBL(executionContext, pOSTypeDTO);
                                pOSTypeBL.Save(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw valEx;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw new Exception(ex.Message, ex);
                            }
                        }
                        /// Implementation Logic - For API
                        //POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext, pOSTypeDTOList);
                        //pOSTypeListBL.Save();
                        result = "Success";
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> DeletePOSTypes(List<POSTypeDTO> pOSTypeDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(pOSTypeDTOList);
                    if (pOSTypeDTOList == null)
                    {
                        throw new ValidationException("pOSTypeDTOList is Empty");
                    }

                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        foreach (POSTypeDTO pOSTypeDTO in pOSTypeDTOList)
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                POSTypeBL pOSTypeBL = new POSTypeBL(executionContext, pOSTypeDTO);
                                pOSTypeBL.DeletePOSTypes(pOSTypeDTO, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw valEx;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw new Exception(ex.Message, ex);
                            }
                        }
                        /// Implementation Logic - For API
                        //POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext, pOSTypeDTOList);
                        //pOSTypeListBL.DeletePOSTypesList();
                        result = "Success";
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }
    }
}

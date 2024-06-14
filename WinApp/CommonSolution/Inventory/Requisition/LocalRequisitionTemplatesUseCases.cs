/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalRequisitionTemplatesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      11-Dec-2020       Mushahid Faizan                 Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Inventory.Requisition
{
    public class LocalRequisitionTemplatesUseCases : LocalUseCases, IRequisitionTemplatesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        public LocalRequisitionTemplatesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<RequisitionTemplatesDTO>> GetRequisitionTemplates(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> parameters,
                                                                                 bool loadChildRecords = false, bool activeChildRecords = true, 
                                                                                 int currentPage = 0, int pageSize = 0, 
                                                                                 SqlTransaction sqlTransaction = null)
        {
            return await Task<List<RequisitionTemplatesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RequisitionTemplateList requisitionTemplateListBL = new RequisitionTemplateList(executionContext);
                int siteId = GetSiteId();
                List<RequisitionTemplatesDTO> requisitionTemplatesDTOList = requisitionTemplateListBL.GetAllRequisitionTemplates(parameters, loadChildRecords, activeChildRecords, currentPage, pageSize, sqlTransaction);
                log.LogMethodExit(requisitionTemplatesDTOList);
                return requisitionTemplatesDTOList;
            });
        }

        public async Task<int> GetRequisitionTemplateCount(List<KeyValuePair<RequisitionTemplatesDTO.SearchByReqTemplatesTypeParameters, string>> parameters,
                                                                                 SqlTransaction sqlTransaction = null)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RequisitionTemplateList requisitionTemplateListBL = new RequisitionTemplateList(executionContext);
                int siteId = GetSiteId();
                int requisitionTemplateCount = requisitionTemplateListBL.GetRequisitionTemplatesCount(parameters);
                log.LogMethodExit(requisitionTemplateCount);
                return requisitionTemplateCount;
            });
        }

        public async Task<string> SaveRequisitionTemplates(List<RequisitionTemplatesDTO> requisitionTemplatesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(requisitionTemplatesDTOList);
                if (requisitionTemplatesDTOList == null)
                {
                    throw new ValidationException("requisitionDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RequisitionTemplatesDTO requisitionTemplatesDTO in requisitionTemplatesDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            RequisitionTemplatesBL requisitionTemplatesBL = new RequisitionTemplatesBL(executionContext, requisitionTemplatesDTO);//note
                            requisitionTemplatesBL.Save(parafaitDBTrx.SQLTrx);
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
                }
                result = "Success";
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

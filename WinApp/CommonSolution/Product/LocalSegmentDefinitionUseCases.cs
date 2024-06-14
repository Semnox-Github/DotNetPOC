/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalSegmentDefinitionUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class LocalSegmentDefinitionUseCases : ISegmentDefinitionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalSegmentDefinitionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<SegmentDefinitionDTO>> GetSegmentDefinitions(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>
                          searchParameters, bool buildChildRecords, bool loadActiveChild, int currentPage = 0, int pageSize = 0)
        {
            return await Task<List<SegmentDefinitionDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                SegmentDefinitionList segmentDefinitionListBL = new SegmentDefinitionList(executionContext);
                List<SegmentDefinitionDTO> segmentDefinitionDTOList = segmentDefinitionListBL.GetAllSegmentDefinitionsDTOList(searchParameters, buildChildRecords, loadActiveChild, currentPage, pageSize);
                log.LogMethodExit(segmentDefinitionDTOList);
                return segmentDefinitionDTOList;
            });
        }

        public async Task<int> GetSegmentCount(List<KeyValuePair<SegmentDefinitionDTO.SearchBySegmentDefinitionParameters, string>>
                          searchParameters)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                SegmentDefinitionList segmentDefinitionListBL = new SegmentDefinitionList(executionContext);
                int segmentCount = segmentDefinitionListBL.GetSegmentDefinitionCount(searchParameters);
                log.LogMethodExit(segmentCount);
                return segmentCount;
            });
        }

        public async Task<string> SaveSegmentDefinitions(List<SegmentDefinitionDTO> segmentDefinitionDTOList)
        {
            log.LogMethodEntry("segmentDefinitionDTOList");
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                if (segmentDefinitionDTOList == null)
                {
                    throw new ValidationException("segmentDefinitionDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (SegmentDefinitionDTO segmentDefinitionDTO in segmentDefinitionDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            SegmentDefinition segmentDefinitionBL = new SegmentDefinition(executionContext, segmentDefinitionDTO);
                            segmentDefinitionBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }

                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
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
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

    }
}

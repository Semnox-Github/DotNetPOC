/********************************************************************************************
* Project Name - User
* Description  - LocalEntityOverrideDatesUseCases class
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    08-Apr-2021      B Mahesh Pai        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class LocalEntityOverrideDatesUseCases : IEntityOverrideDatesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalEntityOverrideDatesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<EntityOverrideDatesDTO>> GetEntityOverrideDates(List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<EntityOverrideDatesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                EntityOverrideList entityOverrideDatesList = new EntityOverrideList(executionContext);
                List<EntityOverrideDatesDTO> entityOverrideDatesDTOList = entityOverrideDatesList.GetAllEntityOverrideList(searchParameters, sqlTransaction);

                log.LogMethodExit(entityOverrideDatesDTOList);
                return entityOverrideDatesDTOList;
            });
        }
        public async Task<string> SaveEntityOverrideDates(List<EntityOverrideDatesDTO> entityOverrideDatesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(entityOverrideDatesDTOList);
                if (entityOverrideDatesDTOList == null)
                {
                    throw new ValidationException("entityOverrideDatesDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        EntityOverrideList entityOverrideList = new EntityOverrideList(executionContext, entityOverrideDatesDTOList);
                        entityOverrideList.SaveUpdateEntityOverrideDatesList(-1, parafaitDBTrx.SQLTrx);
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
                        throw ex;
                    }
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<string> Delete(List<EntityOverrideDatesDTO> entityOverrideDatesDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(entityOverrideDatesDTOList);
                    EntityOverrideList entityOverrideList = new EntityOverrideList(executionContext, entityOverrideDatesDTOList);
                    entityOverrideList.DeleteEntityOverrideDateList();
                    result = "Success";
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
    }
}

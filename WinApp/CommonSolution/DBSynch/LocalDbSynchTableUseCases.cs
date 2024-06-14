/********************************************************************************************
* Project Name - DBSynch
* Description  - LocalDbSynchTableUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    05-May-2021       Roshan Devadiga            Created
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DBSynch
{
    public class LocalDbSynchTableUseCases:IDbSynchTableUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalDbSynchTableUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
       public async Task<List<DBSynchTableDTO>> GetDBSynchs(List<KeyValuePair<DBSynchTableDTO.SearchByParameters, string>> searchParameters)

        {
            return await Task<List<DBSynchTableDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                DBSynchList dBSynchList = new DBSynchList(executionContext);
                List<DBSynchTableDTO> dBSynchDTOList = dBSynchList.GetAllDBSynchList(searchParameters);

                log.LogMethodExit(dBSynchDTOList);
                return dBSynchDTOList;
            });
        }
        public async Task<string> SaveDBSynchs(List<DBSynchTableDTO> dBSynchDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(dBSynchDTOList);
                    if (dBSynchDTOList == null)
                    {
                        throw new ValidationException("dBSynchDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            DBSynchList dBSynchList = new DBSynchList(executionContext, dBSynchDTOList);
                            dBSynchList.SaveUpdateDbSynchList();
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
        public async Task<string> Delete(List<DBSynchTableDTO> dBSynchDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(dBSynchDTOList);
                    DBSynchList dBSynchList = new DBSynchList(executionContext, dBSynchDTOList);
                    dBSynchList.DeleteDbSynchList();
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

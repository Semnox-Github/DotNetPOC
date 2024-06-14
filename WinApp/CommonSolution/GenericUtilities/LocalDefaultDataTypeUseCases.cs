/********************************************************************************************
 * Project Name - DefaultDataType
 * Description  - LocalDefaultDataTypeUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         10-May-2021       B Mahesh Pai       Created
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
    class LocalDefaultDataTypeUseCases:IDefaultDataTypeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalDefaultDataTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<DefaultDataTypeDTO>> GetDefaultDataTypes(List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<DefaultDataTypeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                DefaultDataTypeListBL defaultDataTypeListBL = new DefaultDataTypeListBL(executionContext);
                List<DefaultDataTypeDTO> defaultDataTypeDTO = defaultDataTypeListBL.GetDefaultDataTypeValues(searchParameters, sqlTransaction);

                log.LogMethodExit(defaultDataTypeDTO);
                return defaultDataTypeDTO;
            });
        }
        public async Task<string> SaveDefaultDataTypes(List<DefaultDataTypeDTO> defaultDataTypeDTO)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(defaultDataTypeDTO);
                    if (defaultDataTypeDTO == null)
                    {
                        throw new ValidationException("defaultDataTypeDTO is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            DefaultDataTypeListBL defaultDataTypeListBL = new DefaultDataTypeListBL(executionContext, defaultDataTypeDTO);
                            defaultDataTypeListBL.Save();
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
    }
}

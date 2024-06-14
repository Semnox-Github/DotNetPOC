/********************************************************************************************
* Project Name - DigitalSignage
* Description  - LocalSignagePatternUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.140.00    23-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{

    /// <summary>
    /// Implementation of SignagePattern use-cases
    /// </summary>
    public class LocalSignagePatternUseCases:ISignagePatternUseCases
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalSignagePatternUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
       public async Task<List<SignagePatternDTO>> GetSignagePatterns(List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchParameters)                 
        {
            return await Task<List<SignagePatternDTO>>.Factory.StartNew(() =>
            {
            log.LogMethodEntry(searchParameters);

                SignagePatternListBL signagePatternListBL = new SignagePatternListBL(executionContext);
                List<SignagePatternDTO> signagePatternDTOList = signagePatternListBL.GetSignagePatternDTOList(searchParameters);

            log.LogMethodExit(signagePatternDTOList);
            return signagePatternDTOList;
            });
        }
        public async Task<string> SaveSignagePatterns(List<SignagePatternDTO> signagePatternDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(signagePatternDTOList);
                    if (signagePatternDTOList == null)
                    {
                        throw new ValidationException("signagePatternDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            SignagePatternListBL signagePatternListBL = new SignagePatternListBL(executionContext, signagePatternDTOList);
                            signagePatternListBL.Save();
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

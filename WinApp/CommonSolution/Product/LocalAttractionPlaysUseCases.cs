/********************************************************************************************
 * Project Name - Product
 * Description  - LocalAttractionPlaysUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021    Roshan Devadiga        Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public class LocalAttractionPlaysUseCases: IAttractionPlaysUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalAttractionPlaysUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<AttractionPlaysDTO>> GetAttractionPlays(List<KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>>
                         searchParameters)
        {
            return await Task<List<AttractionPlaysDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AttractionPlaysBLList attractionPlaysBLList = new AttractionPlaysBLList(executionContext);
                List<AttractionPlaysDTO> attractionPlaysDTOList = attractionPlaysBLList.GetAttractionPlaysDTOList(searchParameters);

                log.LogMethodExit(attractionPlaysDTOList);
                return attractionPlaysDTOList;
            });
        }
        public async Task<string> SaveAttractionPlays(List<AttractionPlaysDTO> attractionPlaysDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(attractionPlaysDTOList);
                if (attractionPlaysDTOList == null)
                {
                    throw new ValidationException("attractionPlaysDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (AttractionPlaysDTO attractionPlaysDTO in attractionPlaysDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            AttractionPlaysBL attractionPlaysBL = new AttractionPlaysBL(executionContext, attractionPlaysDTO);
                            attractionPlaysBL.Save(parafaitDBTrx.SQLTrx);
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
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> Delete(List<AttractionPlaysDTO> attractionPlaysDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(attractionPlaysDTOList);
                    AttractionPlaysBLList attractionPlaysBLList = new AttractionPlaysBLList(executionContext, attractionPlaysDTOList);
                    attractionPlaysBLList.DeleteAttractionPlaysList();
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

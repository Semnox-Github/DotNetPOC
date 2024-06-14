/********************************************************************************************
 * Project Name - Country
 * Description  - LocalsCountryUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         11-May-2021       Roshan Devadiga       
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class LocalCountryUseCases:ICountryUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalCountryUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<CountryDTO>> GetCountries(CountryParams countryParams)

        {
            return await Task<List<CountryDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(countryParams);

                CountryList countryList = new CountryList();
                List<CountryDTO> countryDTOList = countryList.GetCountryList(countryParams);

                log.LogMethodExit(countryDTOList);
                return countryDTOList;
            });
        }
        public async Task<string> SaveCountries(List<CountryDTO> countryDTOList)
    
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(countryDTOList);
                    if (countryDTOList == null)
                    {
                        throw new ValidationException("countryDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            CountryDTOList countryList = new CountryDTOList(executionContext, countryDTOList);
                            countryList.SaveUpdateCountryList();
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
        public async Task<String> Delete(List<CountryDTO> countryDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(countryDTOList);
                    CountryDTOList countryList = new CountryDTOList(executionContext, countryDTOList);
                    countryList.DeleteCountryList();
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

        public async Task<CountryContainerDTOCollection> GetCountryContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<CountryContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    CountryContainerList.Rebuild(siteId);
                }
                List<CountryContainerDTO> countryContainerList = CountryContainerList.GetCountryContainerDTOList(siteId);
                CountryContainerDTOCollection result = new CountryContainerDTOCollection(countryContainerList);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}

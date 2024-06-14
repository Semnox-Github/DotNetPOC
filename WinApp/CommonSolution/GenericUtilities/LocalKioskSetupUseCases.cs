using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class LocalKioskSetupUseCases:IKioskSetupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalKioskSetupUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalKioskSetupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<KioskSetupDTO>> GetKioskSetups(List<KeyValuePair<KioskSetupDTO.SearchByParameters, string>> searchParameters)
        {
            return await Task<List<KioskSetupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                KioskSetupList kioskSetupList = new KioskSetupList(executionContext);
                List<KioskSetupDTO> kioskSetupDTODTOList = kioskSetupList.GetAllKioskSetupsList(searchParameters);

                log.LogMethodExit(kioskSetupDTODTOList);
                return kioskSetupDTODTOList;
            });
        }
        public async Task<string> SaveKioskSetups(List<KioskSetupDTO> kioskSetupDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(kioskSetupDTOList);
                    if (kioskSetupDTOList == null)
                    {
                        throw new ValidationException("kioskSetupDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            KioskSetupList kioskSetupList = new KioskSetupList(executionContext, kioskSetupDTOList);
                            kioskSetupList.SaveUpdateKioskSetupsList();
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
        public async Task<string> Delete(List<KioskSetupDTO> kioskSetupDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(kioskSetupDTOList);
                    KioskSetupList kioskSetupList = new KioskSetupList(executionContext, kioskSetupDTOList);
                    kioskSetupList.Delete();
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

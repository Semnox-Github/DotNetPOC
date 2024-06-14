/********************************************************************************************
 * Project Name - Inventory
 * Description  - LocalVendorUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Vendor
{
    public class LocalVendorUseCases : IVendorUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalVendorUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<VendorDTO>> GetVendors(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>
                          searchParameters, int currentPage = 0, int pageSize = 0)
        {
            return await Task<List<VendorDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                VendorList vendorListBL = new VendorList(executionContext);
                List<VendorDTO> vendorDTOList = vendorListBL.GetAllVendors(searchParameters, currentPage, pageSize);
                log.LogMethodExit(vendorDTOList);
                return vendorDTOList;
            });
        }

        public async Task<int> GetVendorCount(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>
                          searchParameters)
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);
                VendorList vendorListBL = new VendorList(executionContext);
                int vendorCount = vendorListBL.GetVendorCount(searchParameters);
                log.LogMethodExit(vendorCount);
                return vendorCount;
            });
        }

        public async Task<string> SaveVendors(List<VendorDTO> vendorDTOList)
        {
            log.LogMethodEntry("vendorDTOList");
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                if (vendorDTOList == null)
                {
                    throw new ValidationException("vendorDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (VendorDTO vendorDTO in vendorDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Vendor vendorBL = new Vendor(executionContext, vendorDTO);
                            vendorBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }

                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw ;
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
                            throw ;
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

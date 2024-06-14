/********************************************************************************************
 * Project Name - CheckOutPrices BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date                    Modified By                 Remarks          
 *************************************************************************************************************
 *2.50        08-Feb-2019           Indrajeet Kumar               Created 
 *2.70        11-July-2019          Muhammed Mehraj Modified      Added DeleteCheckOutPricesList() method
                                                                  Added DeleteCheckOutPrices method
 *2.100.0     14-Aug-2020           Girish Kundar                 Modified : 3 Tier changes as part of phase -3 Rest API changes
 **************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    public class CheckOutPricesBL
    {
        private CheckOutPricesDTO checkOutPricesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        private  CheckOutPricesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized Constructor 
        /// </summary>
        /// <param name="checkOutPricesDTO"></param>
        /// <param name="executionContext"></param>
        public CheckOutPricesBL(ExecutionContext executionContext, CheckOutPricesDTO checkOutPricesDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(checkOutPricesDTO, executionContext);
            this.checkOutPricesDTO = checkOutPricesDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// save and updates the record 
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CheckOutPricesDataHandler checkOutPricesDataHandler = new CheckOutPricesDataHandler(sqlTransaction);
            Validate(sqlTransaction);
            if (checkOutPricesDTO.Id < 0)
            {
                checkOutPricesDTO = checkOutPricesDataHandler.Insert(checkOutPricesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                checkOutPricesDTO.AcceptChanges();
            }
            else
            {
                if (checkOutPricesDTO.Id > 0 && checkOutPricesDTO.IsChanged)
                {
                    checkOutPricesDTO = checkOutPricesDataHandler.Update(checkOutPricesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    checkOutPricesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        private void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete DeleteCheckOutPrices object by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                CheckOutPricesDataHandler checkOutPricesDataHandler = new CheckOutPricesDataHandler(sqlTransaction);
                checkOutPricesDataHandler.DeleteCheckOutPrices(checkOutPricesDTO.Id);
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        public CheckOutPricesDTO CheckOutPricesDTO {  get { return checkOutPricesDTO; } }
    }


    public class CheckOutPricesBLList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<CheckOutPricesDTO> checkOutPricesList;

        /// <summary>
        /// Parameterized Constructor 
        /// </summary>
        public CheckOutPricesBLList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="checkOutPricesList"></param>
        /// <param name="executionContext"></param>
        public CheckOutPricesBLList(ExecutionContext executionContext, List<CheckOutPricesDTO> checkOutPricesList)
            :this(executionContext)
        {
            log.LogMethodEntry(checkOutPricesList, executionContext);
            this.checkOutPricesList = checkOutPricesList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Latest CheckOutPrices list with respect to search Parameters
        /// </summary>
        /// <param name="searchParemeters"></param>
        /// <returns></returns>
        public List<CheckOutPricesDTO> GetAllCheckOutPricesList(List<KeyValuePair<CheckOutPricesDTO.SearchByParameters, string>> searchParemeters,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParemeters);
            CheckOutPricesDataHandler checkOutPricesDataHandler = new CheckOutPricesDataHandler(sqlTransaction);
            List<CheckOutPricesDTO> checkOutPricesDTOList =  checkOutPricesDataHandler.GetAllCheckOutPricesList(searchParemeters);
            log.LogMethodExit(checkOutPricesDTOList);
            return checkOutPricesDTOList;
        }
        /// <summary>
        ///  Save and Update Method For CheckOutPrices
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (checkOutPricesList != null && checkOutPricesList.Any())
                {
                    foreach (CheckOutPricesDTO checkOutPricesDTO in checkOutPricesList)
                    {
                        CheckOutPricesBL checkOutPricesBLObj = new CheckOutPricesBL(executionContext, checkOutPricesDTO);
                        checkOutPricesBLObj.Save(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (SqlException ex)
            {
                log.Error(ex);
                if (ex.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
                else if (ex.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                log.LogMethodExit(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete CheckOutPricesList
        /// </summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                if (checkOutPricesList != null && checkOutPricesList.Any())
                {
                    foreach (CheckOutPricesDTO checkOutPricesDTO in checkOutPricesList)
                    {
                        if (checkOutPricesDTO.IsChanged && checkOutPricesDTO.IsActive == false)
                        {
                            CheckOutPricesBL checkOutPricesBLObj = new CheckOutPricesBL(executionContext, checkOutPricesDTO);
                            checkOutPricesBLObj.Delete(sqlTransaction);
                        }
                    }
                }
                log.LogMethodExit();
            }
            catch (SqlException ex)
            {
                log.Error(ex);
                if (ex.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
                else if (ex.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}

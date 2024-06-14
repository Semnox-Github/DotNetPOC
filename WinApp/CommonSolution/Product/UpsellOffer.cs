/********************************************************************************************
 * Project Name - UpsellOffers
 * Description  - Bussiness logic of UpsellOffers
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        13-Jan-2017   Amaresh      Created
 *2.50        22-Jan-2019   Jagan Mohana Created constructor UpsellOffersList and 
 *                                       new method SaveUpdateUpsellOffersList
 *2.70        07-Jul-2019   Indrajeet K  Created DeleteUpsellOfferList() and  DeleteUpsellOffer() for Hard Deletion    
 *2.70.2        17-Dec-2019   Jinto Thomas Modified: Constructor with Upselloffer id 
*2.110.00    04-Dec-2020     Prajwal S       Updated Three Tier
 ********************************************************************************************/
using System;
using Semnox.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// UpsellOffers allowes to access the UpsellOffers details based on the bussiness logic.
    /// </summary>
    public class UpsellOffer
    {
        private UpsellOffersDTO upsellOffersDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        public UpsellOffer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the offerId parameter
        /// </summary>
        /// <param name="offerId"></param>
        public UpsellOffer(ExecutionContext executionContext, int offerId, SqlTransaction sqlTransaction = null)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, offerId, sqlTransaction);
            UpsellOfferDataHandler upsellOfferDataHandler = new UpsellOfferDataHandler(sqlTransaction);
            this.upsellOffersDTO = upsellOfferDataHandler.GetUpsellOffer(offerId);
            if (upsellOffersDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "UpsellOffersDTO", offerId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="upsellOffersDTO">upsellOffersDTO</param>
        public UpsellOffer(ExecutionContext executionContext, UpsellOffersDTO upsellOffersDTO)
          :this(executionContext)
        {
            log.LogMethodEntry(executionContext, upsellOffersDTO);
            this.upsellOffersDTO = upsellOffersDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the list of UpsellOfferProductsDTO with upsell products
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<UpsellOfferProductsDTO> GetUpsellOfferProductsDTOList(int productId)
        {
            log.LogMethodEntry(productId);
            UpsellOfferDataHandler upsellOfferDataHandler = new UpsellOfferDataHandler();
            log.LogMethodExit();
            return upsellOfferDataHandler.GetUpsellOfferProducts(productId);
        }
        
        /// <summary>
        /// get UpsellOffersDTO Object
        /// </summary>
        public UpsellOffersDTO GetupsellOffersDTO
        {
            get { return upsellOffersDTO; }
        }

        /// <summary>
        /// Saves the UpsellOffers
        /// Checks if the OfferId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (upsellOffersDTO.IsChanged == false
               && upsellOffersDTO.OfferId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            UpsellOfferDataHandler upsellOfferDataHandler = new UpsellOfferDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }

            if (upsellOffersDTO.OfferId < 0)
            {
                upsellOffersDTO = upsellOfferDataHandler.InsertUpsellOffer(upsellOffersDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                upsellOffersDTO.AcceptChanges();
            }
            else if (upsellOffersDTO.IsChanged == true)
            {

                 upsellOffersDTO = upsellOfferDataHandler.UpdateUpsellOffer(upsellOffersDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                 upsellOffersDTO.AcceptChanges();
                
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the UpsellOffer details based on offerId
        /// </summary>
        /// <param name="offerId"></param>        
        public void DeleteUpsellOffer(int offerId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(offerId);
            try
            {
                UpsellOfferDataHandler upsellOfferDataHandler = new UpsellOfferDataHandler(sqlTransaction);
                upsellOfferDataHandler.DeleteUpsellOffer(offerId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }

        }



        /// <summary>
        /// Validates the RecipeEstimationDetailsDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (upsellOffersDTO == null)
            {
                //Validation to be implemented.
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

    }



    /// <summary>
    /// Manages the list of UpsellOffersDTO
    /// </summary>
    public class UpsellOffersList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<UpsellOffersDTO> upsellOffersDTOList = new List<UpsellOffersDTO>();

        /// <summary>
        /// No parameter constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public UpsellOffersList()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public UpsellOffersList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor to initialize upsellOffersList and executionContext
        /// </summary>
        public UpsellOffersList(ExecutionContext executionContext, List<UpsellOffersDTO> upsellOffersDTOList)
            :this(executionContext)
        {
            log.LogMethodEntry(upsellOffersDTOList, executionContext);
            this.upsellOffersDTOList = upsellOffersDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the UpsellOffersDTO List
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<UpsellOffersDTO> GetAllUpsellOffers(List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UpsellOfferDataHandler upsellOfferDataHandler = new UpsellOfferDataHandler(sqlTransaction);
            List<UpsellOffersDTO> upsellOffersDTOList = upsellOfferDataHandler.GetUpsellOfferList(searchParameters,sqlTransaction);
            log.LogMethodExit();
            return upsellOffersDTOList;
        }

        /// <summary>
        /// Returns the list of UpsellOfferProductsDTO with suggestive sell products 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<UpsellOfferProductsDTO> GetSuggestiveSellOfferProductsDTOList(int productId)
        {
            log.LogMethodEntry(productId);
            UpsellOfferDataHandler upsellOfferDataHandler = new UpsellOfferDataHandler();
            log.LogMethodExit();
            return upsellOfferDataHandler.GetSuggestiveSellOfferProducts(productId);
        }
        /// <summary>
        /// Saves or update the Upsell Offers List
        /// </summary>
        public void SaveUpdateUpsellOffersList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (upsellOffersDTOList == null ||
                upsellOffersDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < upsellOffersDTOList.Count; i++)
            {
                UpsellOffersDTO upsellOffersDTO = upsellOffersDTOList[i];
                if (upsellOffersDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    UpsellOffer upsellOffer = new UpsellOffer(executionContext, upsellOffersDTO);
                    upsellOffer.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving RecipeEstimationDetailsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RecipeEstimationDetailsDTO", upsellOffersDTO);
                    throw;
                }
            }
        }
        /// <summary>
        /// Hard Deletions for UpsellOffer
        /// </summary>
        public void DeleteUpsellOfferList()
        {
            log.LogMethodEntry();
            if (upsellOffersDTOList != null && upsellOffersDTOList.Count > 0)
            {
                foreach (UpsellOffersDTO upsellOffersDTO in upsellOffersDTOList)
                {
                    if (upsellOffersDTO.IsChanged && upsellOffersDTO.IsActive == false)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                UpsellOffer upsellOffer = new UpsellOffer(executionContext, upsellOffersDTO);
                                upsellOffer.DeleteUpsellOffer(upsellOffersDTO.OfferId, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the UpsellOffersDetailsDTO List for product Id List
        /// </summary>
        /// <param name="productIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductDTO</returns>
        public List<UpsellOffersDTO> GetUpsellOffersDTOListForProducts(List<int> productIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(productIdList, activeRecords, sqlTransaction);
            UpsellOfferDataHandler upsellOffersDataHandler = new UpsellOfferDataHandler(sqlTransaction);
            List<UpsellOffersDTO> upsellOffersDTOList = upsellOffersDataHandler.GetUpsellOffersDTOList(productIdList, activeRecords);
            log.LogMethodExit(upsellOffersDTOList);
            return upsellOffersDTOList;
        }
    }
}

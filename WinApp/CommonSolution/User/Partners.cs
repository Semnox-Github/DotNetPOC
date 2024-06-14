/********************************************************************************************
 * Project Name - Partners BL Class  
 * Description  - Bussiness logic of the Partners  class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        10-May-2016   Rakshith         Created 
 *2.70.2      15-Jul-2019   Girish Kundar    Modified : Save() method. Now Insert/Update method returns the DTO instead of Id. 
 *2.90        21-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Linq;
using System.Text;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    public partial class Partners
    {
        private PartnersDTO partnersDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        private Partners(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        //Constructor Call Corresponding Data Hander based id
        //And return Correspond Object
        //EX: "'Partners"'  DTO  ====>  ""Partners"" DataHandler
        public Partners( ExecutionContext executionContext, int partnerId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext ,partnerId);
            PartnersDatahandler partnersDatahandler = new PartnersDatahandler(sqlTransaction);
            partnersDTO = partnersDatahandler.GetPartnersDTO(partnerId);
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor
        /// </summary>
        public Partners(ExecutionContext executionContext, PartnersDTO partnersDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, partnersDTO);
            this.partnersDTO = partnersDTO;
            log.LogMethodExit();
        }


        /// <summary>
        /// Used For Save 
        /// It may be Insert Or Update
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            //ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            PartnersDatahandler partnersDatahandler = new PartnersDatahandler(sqlTransaction);
            try
            {
                if (partnersDTO.IsChangedRecursive == false)
                {
                    log.LogMethodExit(null, "Nothing to save.");
                    return;
                }
                List<ValidationError> validationErrors = Validate();
                if (validationErrors.Any())
                {
                    string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                    log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    throw new ValidationException(message, validationErrors);
                }
                if (partnersDTO.Active)
                {
                    if (partnersDTO.PartnerId < 0)
                    {
                        partnersDTO = partnersDatahandler.InsertPartner(partnersDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        partnersDTO.AcceptChanges();
            
                    }
                    else if (partnersDTO.IsChanged)
                    {
                        partnersDTO = partnersDatahandler.UpdatePartner(partnersDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        partnersDTO.AcceptChanges();
                    }
                    SavePartnersChild(sqlTransaction);
                 }
                else  // For Hard delete : Only for the existing BL , Not for New BL  
                {
                    if ((partnersDTO.PartnerRevenueShareList != null && partnersDTO.PartnerRevenueShareList.Any(x => x.IsActive == true)))
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 1143);
                        log.LogMethodExit(null, "Throwing Exception - " + message);
                        throw new ForeignKeyException(message);
                    }
                    log.LogVariableState("partnersDTO", partnersDTO);
                    SavePartnersChild(sqlTransaction);
                    if (partnersDTO.PartnerId >= 0)
                    {
                        Delete(partnersDTO.PartnerId);
                    }
                    partnersDTO.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Saving partnersDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SavePartnersChild(SqlTransaction sqlTransaction)
        {

            // for Child Records : :PartnerRevenueShareDTO
            if (partnersDTO.PartnerRevenueShareList != null &&
                partnersDTO.PartnerRevenueShareList.Any())
            {
                List<PartnerRevenueShareDTO> updatedPartnerRevenueShareDTOList = new List<PartnerRevenueShareDTO>();
                foreach (PartnerRevenueShareDTO partnerRevenueShareDTO in partnersDTO.PartnerRevenueShareList)
                {
                    if (partnerRevenueShareDTO.PartnerId != partnersDTO.PartnerId)
                    {
                        partnerRevenueShareDTO.PartnerId = partnersDTO.PartnerId;
                    }
                    if (partnerRevenueShareDTO.IsChanged)
                    {
                        updatedPartnerRevenueShareDTOList.Add(partnerRevenueShareDTO);
                    }
                }
                if (updatedPartnerRevenueShareDTOList.Any())
                {
                    PartnerRevenueShareList partnerRevenueShareListBL = new PartnerRevenueShareList(executionContext, updatedPartnerRevenueShareDTOList);
                    partnerRevenueShareListBL.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Delete the  PartnersDTO based on Id
        /// </summary>
        public int Delete(int partnerId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                PartnersDatahandler partnersDatahandler = new PartnersDatahandler(sqlTransaction);
                int deletedId = partnersDatahandler.DeletePartner(partnerId);
                log.LogMethodExit(deletedId);
                return deletedId;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Deleting partnersDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (partnersDTO == null)
            {
                //Validation to be implemented.
            }

            if (partnersDTO.PartnerRevenueShareList != null && partnersDTO.PartnerRevenueShareList.Any())
            {
                foreach (var partnerRevenueShareDTO in partnersDTO.PartnerRevenueShareList)
                {
                    if (partnerRevenueShareDTO.IsChanged)
                    {
                        PartnerRevenueShare partnerRevenueShare = new PartnerRevenueShare(executionContext, partnerRevenueShareDTO);
                        validationErrorList.AddRange(partnerRevenueShare.Validate(sqlTransaction)); //calls child validation method.
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// gets the GetPartnersDTO
        /// </summary>
        public PartnersDTO GetPartnersDTO
        {
            get { return partnersDTO; }
        }

    }

    public class PartnersList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<PartnersDTO> partnersDTOList = new List<PartnersDTO>();
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        public PartnersList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.partnersDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="partnersDTOList"></param>
        public PartnersList(ExecutionContext executionContext, List<PartnersDTO> partnersDTOList)
        {
            log.LogMethodEntry(executionContext, partnersDTOList);
            this.executionContext = executionContext;
            this.partnersDTOList = partnersDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns Search Request And returns List Of Corresponding Class  
        /// </summary>
        public List<PartnersDTO> GetAllPartnersList(List<KeyValuePair<PartnersDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);
                PartnersDatahandler partnersDatahandler = new PartnersDatahandler(sqlTransaction);
                List<PartnersDTO> partnersDTOList = partnersDatahandler.GetAllpartnersList(searchParameters);
                log.LogMethodExit(partnersDTOList);
                return partnersDTOList;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at GetAllPartnersList(searchParameters)", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Returns Search Request And returns List Of Corresponding Class  
        /// </summary>
        public List<PartnersDTO> GetAllPartnersDTOList(List<KeyValuePair<PartnersDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            PartnersDatahandler partnersDatahandler = new PartnersDatahandler();
            this.partnersDTOList = partnersDatahandler.GetAllpartnersList(searchParameters);
            if (partnersDTOList != null && partnersDTOList.Any() && loadChildRecords)
            {
                Build(partnersDTOList, loadActiveRecords, sqlTransaction);
            }

            log.LogMethodExit(partnersDTOList);
            return partnersDTOList;
        }

        private void Build(List<PartnersDTO> partnersDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(partnersDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, PartnersDTO> partnerIdDictionary = new Dictionary<int, PartnersDTO>();
            StringBuilder sb = new StringBuilder("");
            string partnerIdList;
            for (int i = 0; i < partnersDTOList.Count; i++)
            {
                if (partnersDTOList[i].PartnerId == -1 ||
                    partnerIdDictionary.ContainsKey(partnersDTOList[i].PartnerId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(partnersDTOList[i].PartnerId.ToString());
                partnerIdDictionary.Add(partnersDTOList[i].PartnerId, partnersDTOList[i]);
            }
            partnerIdList = sb.ToString();
            PartnerRevenueShareList partnerRevenueShareList = new PartnerRevenueShareList(executionContext);
            List<KeyValuePair<PartnerRevenueShareDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<PartnerRevenueShareDTO.SearchByParameters, string>>();
            searchByParams.Add(new KeyValuePair<PartnerRevenueShareDTO.SearchByParameters, string>(PartnerRevenueShareDTO.SearchByParameters.PARTNER_ID_LIST, partnerIdList.ToString()));
            searchByParams.Add(new KeyValuePair<PartnerRevenueShareDTO.SearchByParameters, string>(PartnerRevenueShareDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            if (activeChildRecords)
            {
                searchByParams.Add(new KeyValuePair<PartnerRevenueShareDTO.SearchByParameters, string>(PartnerRevenueShareDTO.SearchByParameters.ISACTIVE, "1"));
            }
            List<PartnerRevenueShareDTO> partnerRevenueShareDTOList = partnerRevenueShareList.GetAllPartnerRevenueShare(searchByParams);
            if (partnerRevenueShareDTOList != null && partnerRevenueShareDTOList.Any())
            {
                log.LogVariableState("partnerRevenueShareDTOList", partnerRevenueShareDTOList);
                foreach (var partnerRevenueShareDTO in partnerRevenueShareDTOList)
                {
                    if (partnerIdDictionary.ContainsKey(partnerRevenueShareDTO.PartnerId))
                    {
                        if (partnerIdDictionary[partnerRevenueShareDTO.PartnerId].PartnerRevenueShareList == null)
                        {
                            partnerIdDictionary[partnerRevenueShareDTO.PartnerId].PartnerRevenueShareList = new List<PartnerRevenueShareDTO>();
                        }
                        partnerIdDictionary[partnerRevenueShareDTO.PartnerId].PartnerRevenueShareList.Add(partnerRevenueShareDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the  list of partnersDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (partnersDTOList == null ||
                partnersDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < partnersDTOList.Count; i++)
            {
                var partnersDTO = partnersDTOList[i];
                if (partnersDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    Partners partners = new Partners(executionContext, partnersDTO);
                    partners.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving partnersDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("partnersDTO", partnersDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}

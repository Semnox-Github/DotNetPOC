/********************************************************************************************
 * Project Name - Customer Membership Progression
 * Description  - BL for CustomerMembershipProgression
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.70.2        19-Jul-2019    Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 *2.80          21-May-2020    Girish Kundar       Modified : Made default constructor as Private 
 *2.90        03-July-2020     Girish Kundar       Modified : Change as part of CardCodeDTOList replaced with AccountDTOList in CustomerDTO 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Business logic for CustomerMembershipProgression class.
    /// </summary>
    public class CustomerMembershipProgression
    {
        private CustomerMembershipProgressionDTO customerMembershipProgressionDTO;
        private CustomerDTO customerDTO;
        private readonly ExecutionContext executionContext;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of CustomerMembershipProgression class
        /// </summary>
        private CustomerMembershipProgression(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the membershipProgression id as the parameter
        /// Would fetch the customerRelationshipType object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public CustomerMembershipProgression(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            CustomerMembershipProgressionDataHandler customerMembershipProgressionDataHandler = new CustomerMembershipProgressionDataHandler(sqlTransaction);
            customerMembershipProgressionDTO = customerMembershipProgressionDataHandler.GetCustomerMembershipProgressionDTO(id);
            if (customerMembershipProgressionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "CustomerMembershipProgression", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates membershipProgression object using the CustomerMembershipProgressionDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerMembershipProgressionDTO">CustomerMembershipProgressionDTO object</param>
        public CustomerMembershipProgression(ExecutionContext executionContext, CustomerMembershipProgressionDTO customerMembershipProgressionDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerMembershipProgressionDTO);
            this.customerMembershipProgressionDTO = customerMembershipProgressionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates membershipProgression object using the customerDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="customerDTO">customerDTO object</param>
        public CustomerMembershipProgression(ExecutionContext executionContext, CustomerDTO customerDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, customerMembershipProgressionDTO);
            this.customerDTO = customerDTO;
            ////if(this.customerDTO.CustomerMembershipProgressionDTOList == null)
            //if (this.customerDTO.CustomerMembershipProgressionDTOList == null || (this.customerDTO.CustomerMembershipProgressionDTOList != null && this.customerDTO.CustomerMembershipProgressionDTOList.Count == 0))
            //{
            //    CustomerMembershipProgressionList customerMembershipProgressionList = new CustomerMembershipProgressionList(executionContext);
            //    List<KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string>> membershipProgressionSearchParam;
            //    membershipProgressionSearchParam = new List<KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string>>();
            //    membershipProgressionSearchParam.Add(new KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string>(CustomerMembershipProgressionDTO.SearchByParameters.CUSTOMER_ID, this.customerDTO.Id.ToString()));
            //    //membershipProgressionSearchParam.Add(new KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string>(CustomerMembershipProgressionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            //    //this.customerDTO.CustomerMembershipProgressionDTOList.AddRange(customerMembershipProgressionList.GetCustomerMembershipProgressionDTOList(membershipProgressionSearchParam, null));
            //    List<CustomerMembershipProgressionDTO> customerMembershipProgressionDTOLocList = customerMembershipProgressionList.GetCustomerMembershipProgressionDTOList(membershipProgressionSearchParam, null);
            //    this.customerDTO.CustomerMembershipProgressionDTOList = new List<CustomerMembershipProgressionDTO>();
            //    if(customerMembershipProgressionDTOLocList != null && customerMembershipProgressionDTOLocList.Count > 0)
            //    this.customerDTO.CustomerMembershipProgressionDTOList.AddRange(customerMembershipProgressionDTOLocList);

            //}
            log.LogMethodExit();
        }
        

        /// <summary>
        /// Saves the membership progression
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(int parentSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parentSiteId, sqlTransaction);
            CustomerMembershipProgressionDataHandler customerMembershipProgressionDataHandler = new CustomerMembershipProgressionDataHandler(sqlTransaction);
            if (customerMembershipProgressionDTO != null && customerMembershipProgressionDTO.IsChanged)
            {
                if (customerMembershipProgressionDTO.Id < 0)
                {
                    customerMembershipProgressionDTO = customerMembershipProgressionDataHandler.InsertMembershipProgression(customerMembershipProgressionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    customerMembershipProgressionDTO.AcceptChanges();
                }
                else
                {
                    if (customerMembershipProgressionDTO.IsChanged)
                    {
                        customerMembershipProgressionDTO = customerMembershipProgressionDataHandler.UpdateMembershipProgression(customerMembershipProgressionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        customerMembershipProgressionDTO.AcceptChanges();
                    }
                }
                //CreateRoamingData(parentSiteId, customerMembershipProgressionDTO, sqlTransaction);
            }
            if (customerDTO != null)
            {
                if(customerDTO.CustomerMembershipProgressionDTOList != null)
                {
                    foreach (CustomerMembershipProgressionDTO customerMembershipProgressionLocalDTO in customerDTO.CustomerMembershipProgressionDTOList)
                    {
                        if (customerMembershipProgressionLocalDTO.IsChanged)
                        {
                            if (customerMembershipProgressionLocalDTO.Id < 0)
                            {
                                customerMembershipProgressionDataHandler.InsertMembershipProgression(customerMembershipProgressionLocalDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                                customerMembershipProgressionLocalDTO.AcceptChanges();
                            }
                            else
                            {
                                if (customerMembershipProgressionLocalDTO.IsChanged)
                                {
                                    customerMembershipProgressionDataHandler.UpdateMembershipProgression(customerMembershipProgressionLocalDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                                    customerMembershipProgressionLocalDTO.AcceptChanges();
                                }
                            }
                            //CreateRoamingData(parentSiteId, customerMembershipProgressionLocalDTO, sqlTransaction);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        
        /// <summary>
        /// creates the membership progression entry
        /// If old active record is there it is updated
        /// and new record is created
        /// </summary>
        public void CreateMembershipProgressionEntry(int newMembershipId, DateTime? fromDate, DateTime? toDate)
        {
            log.LogMethodEntry(newMembershipId, fromDate, toDate);
            if(this.customerDTO == null)
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1477));
            }
            else
            {
                if (this.customerDTO.CustomerMembershipProgressionDTOList != null && this.customerDTO.CustomerMembershipProgressionDTOList.Any())
                {
                    List<CustomerMembershipProgressionDTO> latestMembershipProgressionList = this.customerDTO.CustomerMembershipProgressionDTOList.Where(cmp => (cmp.EffectiveToDate == null || cmp.EffectiveToDate >= fromDate)).ToList().OrderByDescending(cmp => cmp.EffectiveFromDate).ToList();
                    if (latestMembershipProgressionList != null && latestMembershipProgressionList.Any())
                    {
                        foreach (CustomerMembershipProgressionDTO latestMembershipProgressionDTO in latestMembershipProgressionList)
                        {
                            this.customerDTO.CustomerMembershipProgressionDTOList.Remove(latestMembershipProgressionDTO);
                            latestMembershipProgressionDTO.EffectiveToDate = Convert.ToDateTime(fromDate).AddSeconds(-1);
                            this.customerDTO.CustomerMembershipProgressionDTOList.Add(latestMembershipProgressionDTO);
                        }
                    }
                } 
                List<AccountDTO> primaryCardList = this.customerDTO.AccountDTOList.Where(card => card.PrimaryAccount == true && card.ValidFlag == true).ToList();
                int primaryCardId = -1;
                int primaryCardType = -1;
                if (primaryCardList == null)
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1478));
                }
                else
                {
                    primaryCardId = primaryCardList[0].AccountId;
                   // primaryCardType = primaryCardList[0].MembershipId;
                } 
                CustomerMembershipProgressionDTO newMembershipProgressionDTO = new CustomerMembershipProgressionDTO(-1, primaryCardId, primaryCardType,fromDate, executionContext.GetSiteId(), "", false, -1, newMembershipId, this.customerDTO.Id, fromDate, toDate, null, executionContext.GetUserId(),
                    ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                if (this.customerDTO.CustomerMembershipProgressionDTOList == null)
                    this.customerDTO.CustomerMembershipProgressionDTOList = new List<CustomerMembershipProgressionDTO>();
                this.customerDTO.CustomerMembershipProgressionDTOList.Add(newMembershipProgressionDTO);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the membership progression entry 
        /// </summary>
        public void UpdateMembershipProgressionEntry(int membershipId, DateTime? fromDate, DateTime? toDate)
        {
            log.LogMethodEntry(membershipId, fromDate, toDate);
            if (this.customerDTO == null)
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1477));
            }
            else
            {
                // List<CustomerMembershipProgressionDTO> latestMembershipProgressionList = this.customerDTO.CustomerMembershipProgressionDTOList.Where(cmp => (cmp.EffectiveToDate == null || cmp.EffectiveToDate >= fromDate)).ToList().OrderByDescending(cmp => cmp.EffectiveFromDate).ToList();
                List<CustomerMembershipProgressionDTO> latestMembershipProgressionList = this.customerDTO.CustomerMembershipProgressionDTOList.OrderByDescending(cmp => cmp.EffectiveFromDate).ToList();
                if (latestMembershipProgressionList != null && latestMembershipProgressionList.Count > 0)
                {
                    //foreach (CustomerMembershipProgressionDTO latestMembershipProgressionDTO in latestMembershipProgressionList)
                    //{
                    if (latestMembershipProgressionList[0].MembershipId == membershipId)
                    {
                        this.customerDTO.CustomerMembershipProgressionDTOList.Remove(latestMembershipProgressionList[0]);
                        latestMembershipProgressionList[0].EffectiveToDate = Convert.ToDateTime(toDate);
                        latestMembershipProgressionList[0].LastRetentionDate = ServerDateTime.Now;
                        this.customerDTO.CustomerMembershipProgressionDTOList.Add(latestMembershipProgressionList[0]);
                    }
                        //else
                        //{
                        //    this.customerDTO.CustomerMembershipProgressionDTOList.Remove(latestMembershipProgressionDTO);
                        //    latestMembershipProgressionDTO.EffectiveToDate = Convert.ToDateTime(fromDate).AddSeconds(-1);
                        //    this.customerDTO.CustomerMembershipProgressionDTOList.Add(latestMembershipProgressionDTO);
                        //}
                    //}
                } 
            }
            log.LogMethodExit();
        }

      
        /// <summary>
        /// creates the membership progression entry
        /// If old active record is there it is updated
        /// and new record is created
        /// </summary>
        public void CreatePurchasedMembershipEntry(int newMembershipId, DateTime? fromDate, DateTime? toDate)
        {
            log.LogMethodEntry(newMembershipId, fromDate, toDate);
            if (this.customerDTO == null)
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1477));
            }
            else
            {
                List<CustomerMembershipProgressionDTO> latestMembershipProgressionList = this.customerDTO.CustomerMembershipProgressionDTOList.Where(cmp => (cmp.EffectiveToDate == null || cmp.EffectiveToDate >= fromDate)).ToList().OrderByDescending(cmp => cmp.EffectiveFromDate).ToList();
                if (latestMembershipProgressionList != null)
                {
                    foreach (CustomerMembershipProgressionDTO latestMembershipProgressionDTO in latestMembershipProgressionList)
                    {
                        this.customerDTO.CustomerMembershipProgressionDTOList.Remove(latestMembershipProgressionDTO);
                        if(latestMembershipProgressionDTO.MembershipId == newMembershipId && latestMembershipProgressionDTO.EffectiveToDate > fromDate)
                        {
                            log.LogVariableState("latestMembershipProgressionDTO.EffectiveToDate ", latestMembershipProgressionDTO.EffectiveToDate);
                            TimeSpan t = Convert.ToDateTime(latestMembershipProgressionDTO.EffectiveToDate) - Convert.ToDateTime(fromDate);
                            DateTime newTodate = Convert.ToDateTime(toDate).AddDays(t.TotalDays);
                            toDate = (DateTime?)newTodate;
                            log.LogVariableState("new Todate ", toDate);
                            latestMembershipProgressionDTO.EffectiveToDate = Convert.ToDateTime(fromDate).AddSeconds(-1);
                        }
                        else
                        latestMembershipProgressionDTO.EffectiveToDate = Convert.ToDateTime(fromDate).AddSeconds(-1);

                        this.customerDTO.CustomerMembershipProgressionDTOList.Add(latestMembershipProgressionDTO);
                    }
                }
                List<AccountDTO> primaryCardList = this.customerDTO.AccountDTOList.Where(card => card.PrimaryAccount == true && card.ValidFlag == true).ToList();
                int primaryCardId = -1;
                int primaryCardType = -1;
                if (primaryCardList == null)
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1478));
                }
                else
                {
                    primaryCardId = primaryCardList[0].AccountId;
                   // primaryCardType = primaryCardList[0].MembershipId;
                }
                                                                                                                                                                                                                                                                                                             
                CustomerMembershipProgressionDTO newMembershipProgressionDTO = new CustomerMembershipProgressionDTO(-1, primaryCardId, primaryCardType, fromDate, executionContext.GetSiteId(), "", false, -1, newMembershipId, this.customerDTO.Id, fromDate, toDate, null, executionContext.GetUserId(), DateTime.Now, executionContext.GetUserId(), DateTime.Now);
                this.customerDTO.CustomerMembershipProgressionDTOList.Add(newMembershipProgressionDTO);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Expire the membership progression entry 
        /// </summary>
        public void ExpireMembershipProgressionEntry(DateTime? expireDate)
        {
            log.LogMethodEntry(expireDate);
           // DateTime fromDate = DateTime.Now.Date;
            DateTime currentDate = (expireDate == null? ServerDateTime.Now: (DateTime)expireDate);
            if (this.customerDTO == null)
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 1477));
            }
            else
            {
                List<CustomerMembershipProgressionDTO> latestMembershipProgressionList = this.customerDTO.CustomerMembershipProgressionDTOList.Where(cmp => (cmp.EffectiveToDate == null || cmp.EffectiveToDate >= currentDate)).ToList().OrderByDescending(cmp => cmp.EffectiveFromDate).ToList();
                if (latestMembershipProgressionList != null && latestMembershipProgressionList.Count > 0)
                {
                    foreach (CustomerMembershipProgressionDTO latestMembershipProgressionDTO in latestMembershipProgressionList)
                    {
                        this.customerDTO.CustomerMembershipProgressionDTOList.Remove(latestMembershipProgressionDTO);
                        latestMembershipProgressionDTO.EffectiveToDate = currentDate.AddSeconds(-1);
                        this.customerDTO.CustomerMembershipProgressionDTOList.Add(latestMembershipProgressionDTO); 
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CustomerMembershipProgressionDTO CustomerMembershipProgressionDTO
        {
            get
            {
                return customerMembershipProgressionDTO;
            }
        }
        //private void CreateRoamingData(int parentSiteId, CustomerMembershipProgressionDTO customerMembershipProgressionDTO, SqlTransaction sqlTransaction = null)
        //{
        //    log.LogMethodEntry(parentSiteId, customerMembershipProgressionDTO, sqlTransaction);
        //    if (parentSiteId > -1)
        //    {
        //        if (parentSiteId != customerMembershipProgressionDTO.Site_Id && executionContext.GetSiteId() > -1
        //            && customerMembershipProgressionDTO.Id > -1)
        //        {
        //            DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", customerMembershipProgressionDTO.Guid, "MembershipProgression", DateTime.Now, parentSiteId);
        //            DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
        //            dBSynchLogBL.Save(sqlTransaction);
        //        }
        //        RoamingDataInHQ(parentSiteId, customerMembershipProgressionDTO, sqlTransaction);
        //    }
        //    log.LogMethodExit();
        //}

        //private void RoamingDataInHQ(int parentSiteId, CustomerMembershipProgressionDTO customerMembershipProgressionDTO, SqlTransaction sqlTransaction)
        //{
        //    log.LogMethodEntry(parentSiteId, customerMembershipProgressionDTO.Id, sqlTransaction);
        //    if (executionContext.GetIsCorporate())
        //    {
        //        if (parentSiteId >= 0)
        //        {
        //            SiteList siteList = new SiteList(executionContext);
        //            List<SiteDTO> roamingSiteDTOList = null;
        //            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTO_ROAM_CUSTOMERS_ACROSS_ZONES"))
        //            {
        //                roamingSiteDTOList = siteList.GetAllSitesForRoaming(parentSiteId);
        //            }
        //            else
        //            {
        //                roamingSiteDTOList = siteList.GetRoamingSites(parentSiteId);
        //            }
        //            if (roamingSiteDTOList.Count > 0)
        //            {
        //                foreach (var siteDTO in roamingSiteDTOList)
        //                {
        //                    if (siteDTO.SiteId != parentSiteId)
        //                    {
        //                        DBSynchLogDTO dBSynchLogDTO = new DBSynchLogDTO("I", customerMembershipProgressionDTO.Guid, "MembershipProgression", DateTime.Now, siteDTO.SiteId);
        //                        DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext, dBSynchLogDTO);
        //                        dBSynchLogBL.Save(sqlTransaction);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    log.LogMethodExit();
        //}
    }

    /// <summary>
    /// Manages the list of MembershipProgression
    /// </summary>
    public class CustomerMembershipProgressionList
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CustomerMembershipProgressionList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CustomerRelationshipType list
        /// </summary>
        public List<CustomerMembershipProgressionDTO> GetCustomerMembershipProgressionDTOList(List<KeyValuePair<CustomerMembershipProgressionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CustomerMembershipProgressionDataHandler customerMembershipProgressionDataHandler = new CustomerMembershipProgressionDataHandler(sqlTransaction);
            List<CustomerMembershipProgressionDTO> returnValue = customerMembershipProgressionDataHandler.GetCustomerMembershipProgressionDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        public List<CustomerMembershipProgressionDTO> GetCustomerMembershipProgressionByCustomerIds(List<int> customerIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(customerIdList, sqlTransaction);
            CustomerMembershipProgressionDataHandler customerMembershipProgressionDataHandler = new CustomerMembershipProgressionDataHandler(sqlTransaction);
            List<CustomerMembershipProgressionDTO> returnValue = customerMembershipProgressionDataHandler.GetCustomerMembershipProgressionByCustomerIds(customerIdList);
            log.LogMethodExit(returnValue);
            return returnValue;
        } 
    }
}

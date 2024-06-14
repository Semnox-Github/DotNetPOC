/*****************************************************************************************************
 * Project Name - Requisition 
 * Description  - Bussiness logic of Requisition 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *****************************************************************************************************
 *1.00        08-Aug-2016   Suneetha.S      Created 
 *2.70        15-Jul-2019   Dakshakh raj    Modified : Save() method Insert/Update method returns DTO.
 *2.110.0    11-Dec-2020   Mushahid Faizan        Modified : Web Inventory UI Redesign changes
 *2.110.0     20-Feb-2020   Dakshakh Raj    Modified: Get Sequence method changes
 *****************************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// To create Requisition templates
    /// </summary> 
    public class RequisitionBL
    {
        private RequisitionDTO requisitionDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;


        private RequisitionDTO requisitionDTOonsave;
        private Product.ProductBL product;
        private List<RequisitionLinesDTO> requisitionLinesDTOList = new List<RequisitionLinesDTO>();
        private static readonly ConcurrentDictionary<int, UOMContainer> uomContainerDictionary = new ConcurrentDictionary<int, UOMContainer>();

        /// <summary>
        /// Default constructor of Requisition  class
        /// </summary>
        private RequisitionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the RQ id as the parameter
        /// Would fetch the RQ object from the database based on the id passed. 
        /// </summary>
        /// <param name="requisitionId">Requisition id</param>
        /// <param name="sqlTransaction">Sql transaction</param>
        public RequisitionBL(ExecutionContext executionContext, int requisitionId, SqlTransaction sqlTransaction = null, bool loadChildRecords = false, bool activeChildRecords = false)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, requisitionId, sqlTransaction);
            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            requisitionDTO = requisitionDataHandler.GetRequisitionsDTO(requisitionId);
            if (requisitionDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Requisition ", requisitionId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Loads the requisition of the passed guid 
        /// </summary>
        /// <param name="guid">Req guid</param>
        /// <param name="sqlTransaction">Sql trasnaction</param>
        public RequisitionBL(ExecutionContext executionContext, string guid, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(guid, sqlTransaction);
            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchByRequisitionParameters = new List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>>();
            searchByRequisitionParameters.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.GUID, guid));
            List<RequisitionDTO> RequisitionDTOList = requisitionDataHandler.GetRequisitionsList(searchByRequisitionParameters);
            if (RequisitionDTOList == null || (RequisitionDTOList != null && RequisitionDTOList.Count == 0))
            {
                requisitionDTO = new RequisitionDTO();
            }
            else
            {
                requisitionDTO = RequisitionDTOList[0];
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates templates type object using the requisitionDTO
        /// </summary>
        /// <param name="requisitionDTO">RequisitionDTO object</param>
        public RequisitionBL(ExecutionContext executionContext, RequisitionDTO requisitionDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(requisitionDTO);
            this.requisitionDTO = requisitionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the RequisitionLinesList based on the Requisition id.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            // load child records for - RequisitionLinesList
            RequisitionLinesList requisitionLineList = new RequisitionLinesList(executionContext);
            List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>> searchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
            searchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, requisitionDTO.RequisitionId.ToString()));
            if (activeChildRecords)
            {
                searchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG, "1"));
            }
            requisitionDTO.RequisitionLinesListDTO = requisitionLineList.GetAllRequisitionLines(searchParams, sqlTransaction);
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the templates 
        /// Checks if the template id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>requisitionId </returns>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (requisitionDTO.IsChangedRecursive == false
                && requisitionDTO.RequisitionId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RequisitionDataHandler requisitionsDataHandler = new RequisitionDataHandler(sqlTransaction);
            //Validate(sqlTransaction);
            if (requisitionDTO.RequisitionId < 0)
            {
                GetSequenceNumber(sqlTransaction, executionContext);
                requisitionDTO = requisitionsDataHandler.Insert(requisitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                requisitionDTO.AcceptChanges();
                InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, "Requisition Inserted",
                                                                requisitionDTO.GUID, false, executionContext.GetSiteId(), "InventoryRequisition", -1,
                                                                requisitionDTO.RequisitionId + ":" + requisitionDTO.RequisitionNo.ToString(), -1, executionContext.GetUserId(),
                                                                ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, inventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTransaction);
            }
            else
            {
                if (requisitionDTO.IsChanged)
                {
                    requisitionDTO = requisitionsDataHandler.Update(requisitionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    requisitionDTO.AcceptChanges();
                    if (!string.IsNullOrEmpty(requisitionDTO.GUID))
                    {
                        InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, "Requisition Updated",
                                                                 requisitionDTO.GUID, false, executionContext.GetSiteId(), "InventoryRequisition", -1,
                                                                 requisitionDTO.RequisitionId + ":" + requisitionDTO.RequisitionNo.ToString(), -1, executionContext.GetUserId(),
                                                                 ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                        InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, inventoryActivityLogDTO);
                        inventoryActivityLogBL.Save(sqlTransaction);
                    }
                }
            }

            if (requisitionDTO.RequisitionLinesListDTO != null)
            {
                foreach (RequisitionLinesDTO requisitionLineDTO in requisitionDTO.RequisitionLinesListDTO)
                {
                    if (requisitionLineDTO.RequisitionId == -1)
                    {
                        requisitionLineDTO.RequisitionId = requisitionDTO.RequisitionId;
                        requisitionLineDTO.RequisitionNo = requisitionDTO.RequisitionNo;
                    }
                    if (requisitionLineDTO.IsChanged == true)
                    {
                        RequisitionLinesBL requisitionLineBL = new RequisitionLinesBL(executionContext, requisitionLineDTO);
                        requisitionLineBL.Save(sqlTransaction);
                    }
                }
            }
            log.LogMethodExit();
        }



        public void SaveRequisitionForInbox(SqlTransaction sqlTransaction = null)
        {
            ApprovalRuleDTO approvalRuleDTO = new ApprovalRuleDTO();
            ApprovalRule approvalRule = new ApprovalRule(executionContext, approvalRuleDTO);
            List<UserMessagesDTO> userMessagesDTOList = new List<UserMessagesDTO>();
            UserMessages userMessages;
            UserMessagesList userMessagesList = new UserMessagesList(executionContext);
            RequisitionLinesDTO inventoryIssueLineSavedDTO;
            RequisitionLinesList requisitionLinesList = new RequisitionLinesList(executionContext);

            List<InventoryDocumentTypeDTO> inventoryDocumentTypeListOnDisplay = new List<InventoryDocumentTypeDTO>();

            InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);

            List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
            inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.APPLICABILITY, "RQ"));
            inventoryDocumentTypeListOnDisplay = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams, sqlTransaction);

            string code = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == ((requisitionDTO.RequisitionType == -1) ? -1 : (int)requisitionDTO.RequisitionType))).ToList<InventoryDocumentTypeDTO>()[0].Code;
            if (!string.IsNullOrWhiteSpace(code))
            {
                requisitionDTOonsave = new RequisitionDTO();
                List<RequisitionLinesDTO> requisitionLinesListOnDisplay = new List<RequisitionLinesDTO>();
                Validate(sqlTransaction);

                if (requisitionDTO.RequisitionId > 0)
                {
                    requisitionDTOonsave.RequisitionId = Convert.ToInt32(requisitionDTO.RequisitionId);
                }
                requisitionDTOonsave.RequisitionType = (int)requisitionDTO.RequisitionType;
                requisitionDTOonsave.RequestingDept = requisitionDTO.RequestingDept;
                requisitionDTOonsave.FromDepartment = requisitionDTO.FromDepartment;
                requisitionDTOonsave.ToDepartment = requisitionDTO.ToDepartment;
                requisitionDTOonsave.TemplateId = requisitionDTO.TemplateId;
                requisitionDTOonsave.EstimatedValue = requisitionDTO.EstimatedValue;
                requisitionDTOonsave.Remarks = requisitionDTO.Remarks;
                requisitionDTOonsave.IsActive = requisitionDTO.IsActive;
                requisitionDTOonsave.RequiredByDate = requisitionDTO.RequiredByDate;
                requisitionDTOonsave.RequisitionNo = requisitionDTO.RequisitionNo;
                requisitionDTOonsave.Status = requisitionDTO.Status;
                requisitionDTOonsave.FromSiteId = requisitionDTO.FromSiteId;
                requisitionDTOonsave.ToSiteId = requisitionDTO.ToSiteId;
                switch (code)
                {
                    case "PURQ":
                        break;
                    case "MLRQ":
                        break;
                    case "ISRQ":
                        break;
                    case "ITRQ":
                        break;
                }
                try
                {
                    RequisitionBL requisitionBL = new RequisitionBL(executionContext, requisitionDTOonsave);
                    requisitionBL.Save(sqlTransaction);
                    requisitionBL = new RequisitionBL(executionContext, requisitionDTOonsave.RequisitionId, sqlTransaction);
                    requisitionDTOonsave = requisitionBL.GetRequisitionsDTO;

                    if (requisitionDTOonsave.RequisitionId > -1)
                    {
                        requisitionLinesListOnDisplay = requisitionDTO.RequisitionLinesListDTO;

                        RequisitionLinesDTO requisitionLinesDTO = new RequisitionLinesDTO();
                        for (int i = 0; i < requisitionLinesListOnDisplay.Count; i++)
                        {
                            requisitionLinesDTO = requisitionLinesListOnDisplay[i];
                            if (requisitionLinesDTO.RequestedQuantity == 0 && requisitionLinesDTO.RequisitionLineId == -1)
                            {
                                continue;
                            }
                            requisitionLinesDTO.RequisitionId = requisitionDTOonsave.RequisitionId;
                            product = new Product.ProductBL(requisitionLinesDTO.ProductId);
                            requisitionLinesDTO.Price = product.GetProductPrice(requisitionLinesDTO.RequestedQuantity);
                            requisitionLinesDTO.RequisitionNo = requisitionDTOonsave.RequisitionNo;
                            RequisitionLinesBL requisitionLines = new RequisitionLinesBL(executionContext, requisitionLinesDTO);
                            requisitionLines.Save(sqlTransaction);
                            if (requisitionDTOonsave.Status.ToUpper().Equals("OPEN"))
                            {
                                requisitionBL = new RequisitionBL(executionContext, requisitionDTOonsave.RequisitionId, sqlTransaction);

                                UserContainerDTO user = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", executionContext.GetSiteId());
                                int roleId = user.RoleId;
                                approvalRuleDTO = approvalRule.GetApprovalRule(roleId, requisitionBL.GetRequisitionsDTO.RequisitionType, executionContext.GetSiteId());
                                if (approvalRuleDTO != null)
                                {
                                    if (approvalRuleDTO.NumberOfApprovalLevels > 0)
                                    {
                                        List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                                        userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                                        userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, requisitionBL.GetRequisitionsDTO.GUID));
                                        userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE, code));
                                        userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                                        userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams, sqlTransaction);
                                        if (userMessagesDTOList == null)
                                        {
                                            userMessages = new UserMessages(executionContext);
                                            userMessages.CreateUserMessages(approvalRuleDTO, "Inventory", code, requisitionBL.GetRequisitionsDTO.GUID, user.UserId, "Approval", "Pending for approval", sqlTransaction);
                                        }
                                    }
                                }
                                userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage("Inventory", code, requisitionBL.GetRequisitionsDTO.GUID, -1, -1, executionContext.GetSiteId(), sqlTransaction);
                                if (requisitionDTOonsave.ToSiteId == executionContext.GetSiteId() && (userMessagesDTOList != null && userMessagesDTOList.Count == 1 && userMessagesDTOList[0].ApprovalRuleID == -1))
                                {
                                    userMessagesDTOList[0].Status = UserMessagesDTO.UserMessagesStatus.APPROVED.ToString();
                                    userMessagesDTOList[0].ActedByUser = Convert.ToInt32(executionContext.GetUserId());
                                    userMessages = new UserMessages(userMessagesDTOList[0], executionContext);
                                    userMessages.Save(sqlTransaction);
                                    userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage("Inventory", code, requisitionBL.GetRequisitionsDTO.GUID, -1, -1, executionContext.GetSiteId(), sqlTransaction);
                                }
                                if (userMessagesDTOList == null || (userMessagesDTOList != null && userMessagesDTOList.Count == 0))
                                {
                                    if (requisitionLinesDTO.RequisitionLineId >= 0)
                                    {
                                        inventoryIssueLineSavedDTO = requisitionLinesList.GetRequisitionLine(requisitionLinesDTO.RequisitionLineId, sqlTransaction);
                                        if (inventoryIssueLineSavedDTO.IsActive)
                                        {
                                            requisitionBL.UpdateRequisitionDetails(inventoryIssueLineSavedDTO.ProductId, inventoryIssueLineSavedDTO.ApprovedQuantity, inventoryIssueLineSavedDTO.RequiredByDate);

                                        }
                                    }
                                    else
                                    {
                                        throw new Exception(MessageContainerList.GetMessage(executionContext, 1074));
                                    }
                                }
                            }
                        }
                        //if (requisitionDTOonsave != null && requisitionDTOonsave.Status.ToUpper().Equals("SUBMITTED") && requisitionLinesListOnDisplay.Count > 0)
                        //{
                        //    if (!code.Equals("ITRQ"))
                        //    {
                        //        if (requisitionBL.GetRequisitionsDTO != null)
                        //        {
                        //            requisitionBL.GetRequisitionsDTO.Status = "Complete";
                        //            requisitionBL.Save(sqlTransaction);
                        //        }
                        //    }
                        //}
                        if (code.Equals("ITRQ") && requisitionLinesListOnDisplay != null && requisitionLinesListOnDisplay.Count > 0 && requisitionBL.GetRequisitionsDTO.Status.Equals("SUBMITTED"))
                        {
                            double price = requisitionLinesListOnDisplay.Sum(x => x.Price * x.RequestedQuantity);
                            CreateTransaction(price, sqlTransaction);
                        }
                        PopulateLine(requisitionBL.GetRequisitionsDTO.RequisitionId);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        private void CreateTransaction(double trxAmount, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(trxAmount, sqlTrxn);
            string message = string.Empty;

            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CREATE_SALE_TRANSACTION").Equals("Y"))
            {
                Transaction.Transaction transaction = new Transaction.Transaction(GetUtility());
                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.PROFILE_PASSWORD, executionContext.GetSiteId().ToString()));
                List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(searchParameters);
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    transaction.customerDTO = customerDTOList[0];
                }
                Products products = new Products();
                ProductsDTO productsDTO = null;
                List<ProductsDTO> productsDTOList = products.GetProductByTypeList("INVENTORYINTERSTORE", executionContext.GetSiteId());
                if (productsDTOList != null)
                {
                    productsDTO = productsDTOList[0];
                }
                if (productsDTO != null)
                {
                    transaction.createTransactionLine(null, productsDTO.ProductId, trxAmount, 1, ref message);
                    if (transaction.SaveOrder(ref message, sqlTrxn) != 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, "Transaction is not created. " + message, "Transaction creation failed.");
                        throw new Exception((errorMessage));
                    }
                }
            }
            log.LogMethodExit();
        }


        void PopulateLine(int requisitionId)
        {
            log.LogMethodEntry(requisitionId);
            ProductDTO productDTO;
            RequisitionLinesList requisitionLinesList = new RequisitionLinesList(executionContext);
            requisitionLinesDTOList = new List<RequisitionLinesDTO>();
            List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>> requisitionLinesSearchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
            requisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG, "1"));
            requisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, requisitionId.ToString()));
            requisitionLinesDTOList = requisitionLinesList.GetAllRequisitionLines(requisitionLinesSearchParams);
            List<RequisitionLinesDTO> sortRequisitionLinesDTOList = new List<RequisitionLinesDTO>();
            if ((requisitionLinesDTOList != null) && (requisitionLinesDTOList.Any()))
            {
                foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionLinesDTOList)
                {
                    productDTO = GetProductDTO(requisitionLinesDTO.ProductId);

                    requisitionLinesDTO.Description = productDTO.Description;
                }
            }
            else
            {
                requisitionLinesDTOList = new List<RequisitionLinesDTO>();
            }
            sortRequisitionLinesDTOList = new List<RequisitionLinesDTO>(requisitionLinesDTOList);

            if (sortRequisitionLinesDTOList != null && sortRequisitionLinesDTOList.Any())
            {
                for (int i = 0; i < sortRequisitionLinesDTOList.Count; i++)
                {
                    int uomId = sortRequisitionLinesDTOList[i].UOMId;
                    if (uomId == -1)
                    {
                        if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                        {
                            if (ProductContainer.productDTOList.Find(x => x.ProductId == sortRequisitionLinesDTOList[i].ProductId).InventoryUOMId != -1)
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == sortRequisitionLinesDTOList[i].ProductId).InventoryUOMId;
                            }
                            else
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == sortRequisitionLinesDTOList[i].ProductId).UomId;
                            }
                        }
                    }
                    UOMContainer uomcontainer = GetUOMContainer(executionContext);
                    List<UOMDTO> uomDTOList = uomcontainer.relatedUOMDTOList.FirstOrDefault(x => x.Key == uomId).Value;
                }
            }
            log.LogMethodExit();
        }



        private double GetProductQuantity(int LocationId, int productId)
        {
            log.LogMethodEntry(LocationId, productId);
            double quantity = 0.0;
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            InventoryList inventoryList = new InventoryList();
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventoryPurchaseOrderLineSearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
            inventoryPurchaseOrderLineSearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, productId.ToString()));
            inventoryPurchaseOrderLineSearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, LocationId.ToString()));
            inventoryDTOList = inventoryList.GetAllInventory(inventoryPurchaseOrderLineSearchParams);
            if (inventoryDTOList != null && inventoryDTOList.Count > 0)
            {
                foreach (InventoryDTO inventoryDTO in inventoryDTOList)
                    quantity += inventoryDTO.Quantity;
            }
            log.LogMethodExit(quantity);
            return quantity;
        }

        private ProductDTO GetProductDTO(int productId)
        {
            log.LogMethodEntry(productId);

            List<ProductDTO> productDTOList = new List<ProductDTO>();
            Product.ProductList productList = new Product.ProductList();
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productPurchaseOrderLineSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            productPurchaseOrderLineSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, productId.ToString()));

            productDTOList = productList.GetAllProducts(productPurchaseOrderLineSearchParams);
            if (productDTOList != null && productDTOList.Count > 0)
            {
                return productDTOList[0];
            }
            log.LogMethodExit();
            return null;
        }

        private Utilities GetUtility()
        {
            Utilities Utilities = new Utilities();
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
            Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            Utilities.ExecutionContext.SetUserId(executionContext.GetUserId());


            Utilities.ParafaitEnv.Initialize();
            return Utilities;
        }
        /// <summary>
        /// Method to get the UOM Container
        /// </summary>
        /// <returns></returns>
        public static UOMContainer GetUOMContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            if (uomContainerDictionary.ContainsKey(executionContext.GetSiteId()) == false)
            {
                uomContainerDictionary[executionContext.GetSiteId()] = new UOMContainer(executionContext);
            }
            UOMContainer result = uomContainerDictionary[executionContext.GetSiteId()];
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// This method processes the approval or reject requests
        /// </summary>
        /// <param name="userMessagesDTO">UserMessagesDTO object</param>
        /// <param name="userMessagesStatus">UserMessagesDTO.UserMessagesStatus type data</param>
        /// <param name="utilities">The utilities object</param>
        /// <param name="sqlTransaction">SqlTransaction object.</param>
        public void ProcessRequests(UserMessagesDTO userMessagesDTO, UserMessagesDTO.UserMessagesStatus userMessagesStatus, Utilities utilities, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(userMessagesDTO, userMessagesStatus, sqlTransaction);
            bool isMasterSite = (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite);
            RequisitionList requisitionList = new RequisitionList(executionContext);
            RequisitionLinesList requisitionLinesList = new RequisitionLinesList(executionContext);
            UserMessages userMessages = new UserMessages(executionContext);
            List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchByRequisitionParameters = new List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>>();
            List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>> searchByRequisitionLinesParameters = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
            UserMessagesList userMessagesList = new UserMessagesList();
            List<UserMessagesDTO> userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage(userMessagesDTO.ModuleType, userMessagesDTO.ObjectType, userMessagesDTO.ObjectGUID, -1, -1, executionContext.GetSiteId(), sqlTransaction);

            if (userMessagesDTOList != null && userMessagesDTOList.Count > 0)
            {
                UserMessagesDTO approvedUserMessagesDTO = userMessages.GetHighestApprovedUserMessage(userMessagesDTO.ApprovalRuleID, -1, -1, userMessagesDTO.ModuleType, userMessagesDTO.ObjectType, userMessagesDTO.ObjectGUID, executionContext.GetSiteId(), sqlTransaction);
                foreach (UserMessagesDTO higherUserMessagesDTO in userMessagesDTOList)
                {
                    if (approvedUserMessagesDTO != null)
                    {
                        if (approvedUserMessagesDTO.Level < higherUserMessagesDTO.Level)
                        {
                            log.LogMethodExit("Ends-ProcessRequests(userMessagesDTO,userMessagesStatus,siteId,isMasterSite,sqlTransaction) method.");
                            return;
                        }
                    }
                }
            }

            searchByRequisitionParameters.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG, "1"));
            searchByRequisitionParameters.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.GUID, userMessagesDTO.ObjectGUID));

            List<RequisitionDTO> requisitionDTOList = requisitionList.GetAllRequisitions(searchByRequisitionParameters, null);
            if (requisitionDTOList == null || (requisitionDTOList != null && requisitionDTOList.Count == 0))
            {
                throw new Exception("There is no requisition record found.");
            }

            searchByRequisitionLinesParameters.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG, "1"));
            searchByRequisitionLinesParameters.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, requisitionDTOList[0].RequisitionId.ToString()));

            List<RequisitionLinesDTO> requisitionLinesDTOList = requisitionLinesList.GetAllRequisitionLines(searchByRequisitionLinesParameters);
            if (requisitionLinesDTOList == null || (requisitionLinesDTOList != null && requisitionLinesDTOList.Count == 0))
            {
                throw new Exception("There is no requisition line records found.");
            }
            if ((userMessagesDTO.ObjectType.Equals("ITRQ") && utilities.ParafaitEnv.SiteId == requisitionDTOList[0].FromSiteId) || (!userMessagesDTO.ObjectType.Equals("ITRQ")))//Transfered from master site or other site adding stock to inventory 
            {
                if (userMessagesStatus.Equals(UserMessagesDTO.UserMessagesStatus.APPROVED))
                {
                    foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionLinesDTOList)
                    {
                        if (requisitionLinesDTO.RequisitionLineId != -1
                                                   && requisitionLinesDTO.RequisitionLineId == requisitionLinesDTOList[requisitionLinesDTOList.Count - 1].RequisitionLineId)
                        {
                            if (!userMessagesDTO.ObjectType.Equals("ITRQ"))
                            {
                                if (requisitionDTOList[0] != null)
                                {
                                    requisitionDTOList[0].Status = "Complete";
                                }
                            }
                        }
                    }
                    requisitionDTOList[0].IsActive = true;
                    this.requisitionDTO = requisitionDTOList[0];
                    Save(sqlTransaction);
                }
                else if (userMessagesStatus.Equals(UserMessagesDTO.UserMessagesStatus.REJECTED))
                {
                    requisitionDTOList[0].Status = "Cancelled";
                    this.requisitionDTO = requisitionDTOList[0];
                    Save(sqlTransaction);
                }
            }
            else if (userMessagesDTO.ObjectType.Equals("ITRQ") && isMasterSite)//Not from master site and to master site or other site tarnsfer
            {
                if (userMessagesStatus.Equals(UserMessagesDTO.UserMessagesStatus.APPROVED))
                {
                    InventoryDocumentTypeList inventoryDocumentType = new InventoryDocumentTypeList(executionContext);
                    InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentType.GetInventoryDocumentType(requisitionDTOList[0].RequisitionType);
                    executionContext.SetSiteId(requisitionDTOList[0].SiteId);
                    requisitionDTOList[0].IsChanged = true;
                    requisitionDTOList[0].Status = "Complete";
                    this.requisitionDTO = requisitionDTOList[0];
                    Save(sqlTransaction);
                }
                else if (userMessagesStatus.Equals(UserMessagesDTO.UserMessagesStatus.REJECTED))
                {
                    requisitionDTOList[0].Status = "Cancelled";
                    this.requisitionDTO = requisitionDTOList[0];
                    Save(sqlTransaction);
                }
            }
            else
            {
                throw new Exception("Invalid request to process.");
            }

            log.LogMethodExit();
        }




        /// <summary>
        /// Validates the RequisitionDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (requisitionDTO.RequisitionType < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1065);
                throw new ValidationException(errorMessage);
            }
            if (requisitionDTO.RequestingDept == -1 || requisitionDTO.RequestingDept == 0)
            {
                if (requisitionDTO.RequisitionType > -1)
                {
                    InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                    InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType(requisitionDTO.RequisitionType);
                    if (inventoryDocumentTypeDTO.Code == "MLRQ")
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1066);
                        throw new ValidationException(errorMessage);
                    }
                }
                else
                {
                    requisitionDTO.RequestingDept = -1;
                }
            }
            if (requisitionDTO.RequisitionType > -1)
            {
                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType(requisitionDTO.RequisitionType);
                if (inventoryDocumentTypeDTO.Code != "ITRQ")
                {
                    if (requisitionDTO.FromDepartment < 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1067);
                        throw new ValidationException(errorMessage);
                    }
                    if (requisitionDTO.ToDepartment < 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1068);
                        throw new ValidationException(errorMessage);
                    }

                }
            }

            if (requisitionDTO.FromDepartment < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1067);//Select From Department
                throw new ValidationException(errorMessage);
            }
            if (requisitionDTO.ToDepartment < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1068);//Select To Department
                throw new ValidationException(errorMessage);
            }

            if (requisitionDTO.ToDepartment == requisitionDTO.FromDepartment) // from and to location should not be same
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1071);
                throw new ValidationException(errorMessage);
            }
            if (string.IsNullOrEmpty(requisitionDTO.Status))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1069);
                throw new ValidationException(errorMessage);
            }
            if (requisitionDTO.RequiredByDate != null)
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                DateTime selectedDate = requisitionDTO.RequiredByDate;
                DateTime todayDate = ServerDateTime.Now;
                if (selectedDate.Date < todayDate.Date.AddDays(1))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1070);
                    throw new ValidationException(errorMessage);
                }
            }
            else
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 15);
                throw new ValidationException(errorMessage);
            }

            if (requisitionDTO.RequisitionType > -1)
            {
                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType(requisitionDTO.RequisitionType);
                if (inventoryDocumentTypeDTO.Code == "ITRQ")
                {
                    if (requisitionDTO.ToSiteId > -1)
                    {
                        if (requisitionDTO.ToSiteId == executionContext.GetSiteId())
                        {
                            string errorMessage = MessageContainerList.GetMessage(executionContext, "To site can not be same site.");
                            throw new ValidationException(errorMessage);
                        }
                        requisitionDTO.FromSiteId = executionContext.GetSiteId();
                    }
                    else
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1186);
                        throw new ValidationException(errorMessage);
                    }
                }
            }
            LocationBL location = new LocationBL(executionContext, requisitionDTO.FromDepartment);
            if (location.GetLocationDTO != null && !location.GetLocationDTO.IsActive.Equals(true))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, "Select active from department.");
                throw new ValidationException(errorMessage);
            }
            location = new LocationBL(executionContext, requisitionDTO.ToDepartment);
            if (location.GetLocationDTO != null && !location.GetLocationDTO.IsActive.Equals(true))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, "Select active to department.");
                throw new ValidationException(errorMessage);
            }
            location = new LocationBL(executionContext, requisitionDTO.RequestingDept);
            if (location.GetLocationDTO != null && !location.GetLocationDTO.IsActive.Equals(true))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, "Select active requesting department.");
                throw new ValidationException(errorMessage);
            }

            if (requisitionDTO.RequisitionLinesListDTO != null && requisitionDTO.RequisitionLinesListDTO.Any())//No products selected. Please select required products
            {
                foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionDTO.RequisitionLinesListDTO)
                {
                    RequisitionLinesBL requisitionLinesBL = new RequisitionLinesBL(executionContext, requisitionLinesDTO);
                    requisitionLinesBL.Validate(sqlTransaction);
                }
            }
            else
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2050); //No products selected. Please select required products
                throw new ValidationException(errorMessage);
            }

            log.LogMethodExit();
        }
        /// <summary>
        /// Get Sequence Number
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="executionContext"></param>
        private void GetSequenceNumber(SqlTransaction sqlTransaction, ExecutionContext executionContext)
        {
            log.LogMethodEntry(sqlTransaction, executionContext);
            SequencesListBL sequencesListBL = new SequencesListBL(executionContext);
            SequencesDTO sequencesDTO = null;
            List<KeyValuePair<SequencesDTO.SearchByParameters, string>> searchBySeqParameters = new List<KeyValuePair<SequencesDTO.SearchByParameters, string>>();
            searchBySeqParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SEQUENCE_NAME, "InventoryRequisition"));
            List<SequencesDTO> sequencesDTOList = sequencesListBL.GetAllSequencesList(searchBySeqParameters);
            if (sequencesDTOList != null && sequencesDTOList.Any())
            {
                if (sequencesDTOList.Count == 1)
                {
                    sequencesDTO = sequencesDTOList[0];
                }
                else
                {
                    sequencesDTO = sequencesDTOList.FirstOrDefault(seq => seq.POSMachineId == executionContext.GetMachineId());
                    if (sequencesDTO == null)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2956, executionContext.GetMachineId()));
                    }
                }
                SequencesBL sequenceBL = new SequencesBL(executionContext, sequencesDTO);
                requisitionDTO.RequisitionNo = sequenceBL.GetNextSequenceNo(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RequisitionDTO GetRequisitionsDTO { get { return requisitionDTO; } }

        /// <summary>
        /// 
        /// Checks whether Requisition can be marked as Closed or still in progress
        /// </summary>
        public void UpdateRequisitionToClosedStatus()
        {
            log.LogMethodEntry();
            bool isClosed = true;
            bool isInprogress = false;
            foreach (RequisitionLinesDTO requisitionLineDTO in requisitionDTO.RequisitionLinesListDTO)
            {
                if (requisitionLineDTO.Status == "InProgress" || requisitionLineDTO.Status == "Closed")
                {
                    isInprogress = true;
                }
                if (requisitionLineDTO.ApprovedQuantity != requisitionLineDTO.RequestedQuantity)
                {
                    isClosed = false;
                    break;
                }

            }
            if (isClosed)
                requisitionDTO.Status = "Closed";
            else if (isInprogress)
                requisitionDTO.Status = "InProgress";
            log.LogMethodExit();
        }

        /// <summary>
        /// Checks whether Requisition can be fulfilled for product and qty passed
        /// </summary>
        /// <param name="productId">productId</param>
        /// <param name="prodQty">prodQty</param>
        /// <param name="receivedDate">receivedDate</param>
        /// <returns></returns>
        public double UpdateRequisitionDetails(int productId, double prodQty, DateTime receivedDate)
        {
            log.LogMethodEntry(productId, prodQty, receivedDate);
            RequisitionLinesList requisitionLineList = new RequisitionLinesList(executionContext);
            RequisitionLinesBL requisitionLinesBL;
            double pendingRequisitionQty;
            if (requisitionDTO != null)
            {
                if (requisitionDTO.RequisitionLinesListDTO != null)
                {
                    List<RequisitionLinesDTO> requisitionLineListDTO = requisitionDTO.RequisitionLinesListDTO.FindAll(rqLine => (rqLine.ProductId == productId && rqLine.ApprovedQuantity < rqLine.RequestedQuantity && (rqLine.Status == "InProgress" || rqLine.Status == "Submitted")));
                    if (requisitionLineListDTO != null && requisitionLineListDTO.Count > 0)
                    {
                        foreach (RequisitionLinesDTO requisitionLineDTO in requisitionLineListDTO)
                        {
                            if (prodQty <= 0)
                                break;
                            else
                            {
                                if (requisitionLineDTO.RequiredByDate == receivedDate)
                                {
                                    pendingRequisitionQty = requisitionLineList.GetPendingRequestedQty(requisitionLineDTO);
                                    if (prodQty > pendingRequisitionQty)
                                    {
                                        requisitionLinesBL = new RequisitionLinesBL(executionContext, requisitionLineDTO);
                                        requisitionLinesBL.UpdateRequisitionLineQtyNStatus(pendingRequisitionQty);
                                        prodQty = prodQty - pendingRequisitionQty;
                                    }
                                    else
                                    {
                                        requisitionLinesBL = new RequisitionLinesBL(executionContext, requisitionLineDTO);
                                        requisitionLinesBL.UpdateRequisitionLineQtyNStatus(Convert.ToInt32(prodQty));
                                        prodQty = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(prodQty);
            return prodQty;
        }
    }

    /// <summary>
    /// Manages the list of RequisitionTemplate
    /// </summary>
    public class RequisitionList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<RequisitionDTO> requisitionDTOList = new List<RequisitionDTO>();
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        /// <summary>
        /// Default constructor with executionContext
        /// </summary>
        public RequisitionList(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parmeterised constructor with 2 params
        /// </summary>
        /// <param name="requisitionDTOList"></param>
        /// <param name="executionContext"></param>
        public RequisitionList(ExecutionContext executionContext, List<RequisitionDTO> requisitionDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, requisitionDTOList);
            this.requisitionDTOList = requisitionDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Requisitions list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns> List of RequisitionDTO</returns>
        public List<RequisitionDTO> GetAllRequisitions(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            List<RequisitionDTO> requisitionDTOList = requisitionDataHandler.GetRequisitionsList(searchParameters);
            log.LogMethodExit(requisitionDTOList);
            return requisitionDTOList;
        }

        public List<RequisitionDTO> GetAllRequisition(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters,
                                                      bool loadChildRecords = false, bool activeChildRecords = true, int currentPage = 0,
                                                      int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            this.requisitionDTOList = requisitionDataHandler.GetRequisitionList(searchParameters, currentPage, pageSize);
            if (requisitionDTOList != null && requisitionDTOList.Any() && loadChildRecords)
            {
                Build(requisitionDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(requisitionDTOList);
            return requisitionDTOList;
        }

        private void Build(List<RequisitionDTO> requisitionDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)//added
        {
            Dictionary<int, RequisitionDTO> requisitionDTODictionary = new Dictionary<int, RequisitionDTO>();
            List<int> requisitionIdList = new List<int>();
            for (int i = 0; i < requisitionDTOList.Count; i++)
            {
                if (requisitionDTODictionary.ContainsKey(requisitionDTOList[i].RequisitionId))
                {
                    continue;
                }
                requisitionDTODictionary.Add(requisitionDTOList[i].RequisitionId, requisitionDTOList[i]);
                requisitionIdList.Add(requisitionDTOList[i].RequisitionId);
            }
            RequisitionLinesList requisitionLinesListBL = new RequisitionLinesList(executionContext);
            List<RequisitionLinesDTO> requisitionLinesDTOList = requisitionLinesListBL.GetRequisitionLinesDTOList(requisitionIdList, sqlTransaction);

            if (requisitionLinesDTOList != null && requisitionLinesDTOList.Any())
            {
                for (int i = 0; i < requisitionLinesDTOList.Count; i++)
                {
                    if (requisitionDTODictionary.ContainsKey(requisitionLinesDTOList[i].RequisitionId) == false)
                    {
                        continue;
                    }
                    RequisitionDTO requisitionDTO = requisitionDTODictionary[requisitionLinesDTOList[i].RequisitionId];
                    if (requisitionDTO.RequisitionLinesListDTO == null)
                    {
                        requisitionDTO.RequisitionLinesListDTO = new List<RequisitionLinesDTO>();
                    }
                    requisitionDTO.RequisitionLinesListDTO.Add(requisitionLinesDTOList[i]);
                }
            }
        }

        /// <summary>
        ///  Returns the Requisitions DTO
        /// </summary>
        /// <param name="requisitionId">requisitionId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>RequisitionDTO</returns>
        public RequisitionDTO GetRequisition(int requisitionId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(requisitionId, sqlTransaction);
            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            RequisitionDTO requisitionDTO = requisitionDataHandler.GetRequisitionsDTO(requisitionId);
            log.LogMethodExit(requisitionDTO);
            return requisitionDTO;
        }

        /// <summary>
        /// Returns the no of Requisition matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetRequisitionsCount(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            int requisitionsCount = requisitionDataHandler.GetRequisitionsCount(searchParameters);
            log.LogMethodExit(requisitionsCount);
            return requisitionsCount;
        }

        /// <summary>
        /// Returns the Product Id DTO
        /// </summary>
        /// <param name="barcodeNo">barcodeNo</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>barcodeNo </returns>
        public int GetProductId(string barcodeNo, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(barcodeNo, sqlTransaction);
            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            int bno = requisitionDataHandler.GetProduct(barcodeNo);
            log.LogMethodExit(bno);
            return bno;
        }

        /// <summary>
        /// Returns the requisition data table
        /// </summary>
        /// <param name="siteid">siteid</param>
        /// <param name="applicability">applicability</param>
        /// <param name="toSiteId">toSiteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>DataTable</returns>
        public System.Data.DataTable GetRequistionsToCreatePO(int siteid, string applicability, int toSiteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteid, applicability, toSiteId, sqlTransaction);
            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            DataTable dataTable = requisitionDataHandler.GetRequistionsToCreatePO(siteid, applicability, toSiteId);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Returns the Requisitions list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="fetchLinkedData">fetchLinkedData</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of RequisitionDTO</returns>
        public List<RequisitionDTO> GetAllRequisitions(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters, Boolean fetchLinkedData, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, fetchLinkedData, sqlTransaction);
            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            if (!fetchLinkedData)
            {
                log.LogMethodExit(requisitionDataHandler.GetRequisitionsList(searchParameters));
                return requisitionDataHandler.GetRequisitionsList(searchParameters);
            }
            else
            {
                List<RequisitionDTO> requisitionListDTO = requisitionDataHandler.GetRequisitionsList(searchParameters);
                Dictionary<int, RequisitionDTO> mapRequisitionDTO = new Dictionary<int, RequisitionDTO>();
                if (requisitionListDTO != null)
                {
                    string requisitionIds = "";
                    foreach (RequisitionDTO requisitionDTO in requisitionListDTO)
                    {
                        if (requisitionIds == "")
                            requisitionIds = requisitionDTO.RequisitionId.ToString();
                        else
                            requisitionIds = requisitionIds + " , " + requisitionDTO.RequisitionId.ToString();
                        mapRequisitionDTO.Add(requisitionDTO.RequisitionId, requisitionDTO);
                    }
                    if (requisitionIds != "")
                    {
                        RequisitionLinesList requisitionLinesListBL = new RequisitionLinesList(executionContext);
                        List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>> rqLineSearchParams;
                        rqLineSearchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
                        rqLineSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_IDS_STRING, requisitionIds));
                        //rqLineSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters., "Y"));
                        List<RequisitionLinesDTO> requisitionLinesListDTO = requisitionLinesListBL.GetAllRequisitionLines(rqLineSearchParams);
                        if (requisitionLinesListDTO != null)
                        {
                            foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionLinesListDTO)
                            {
                                if (mapRequisitionDTO.ContainsKey(requisitionLinesDTO.RequisitionId))
                                { mapRequisitionDTO[requisitionLinesDTO.RequisitionId].RequisitionLinesListDTO.Add(requisitionLinesDTO); }
                            }
                        }
                    }
                }
                log.LogMethodExit(requisitionListDTO);
                return requisitionListDTO;
            }
        }

        /// <summary>
        /// This method is will return Sheet object for RequisitionDTO.
        /// <returns></returns>
        public List<Sheet> BuildTemplate(bool loadRequisitionLines = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<Sheet> sheets = new List<Sheet>();
            Sheet requisitionSheet = new Sheet();
            Sheet requisitionLinesSheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();
            Row headerRowLines = new Row();

            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            RequisitionList requisitionList = new RequisitionList(executionContext);
            List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters = new List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>>();
            searchParameters.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            requisitionDTOList = requisitionList.GetAllRequisition(searchParameters, loadRequisitionLines, true, 0, 0, sqlTransaction);

            Row defaultValueRow = new Row();
            defaultValueRow.AddCell(new Cell());
            defaultValueRow.AddCell(new Cell(""));
            defaultValueRow.AddCell(new Cell("Please Enter RequisitionId < 0 to add new Records."));
            requisitionSheet.AddRow(defaultValueRow);

            Row defaultEmptyValueRow = new Row();
            requisitionSheet.AddRow(defaultEmptyValueRow);

            RequisitionExcelDTODefinition requisitionExcelDTODefinition = new RequisitionExcelDTODefinition(executionContext, "");
            requisitionExcelDTODefinition.BuildHeaderRow(headerRow);
            requisitionSheet.AddRow(headerRow);

            if (requisitionDTOList != null && requisitionDTOList.Any())
            {
                foreach (RequisitionDTO requisitionDTO in requisitionDTOList)
                {
                    requisitionExcelDTODefinition.Configure(requisitionDTO);
                    Row row = new Row();
                    requisitionExcelDTODefinition.Serialize(row, requisitionDTO);
                    requisitionSheet.AddRow(row);
                }
                sheets.Add(requisitionSheet);
            }
            if (loadRequisitionLines)
            {
                Row defaultFileNameValueRow = new Row();
                defaultFileNameValueRow.AddCell(new Cell());
                defaultFileNameValueRow.AddCell(new Cell(""));
                defaultFileNameValueRow.AddCell(new Cell("Please Enter RequisitionLineId < 0 to add new Records."));
                requisitionLinesSheet.AddRow(defaultFileNameValueRow);

                Row defaultEmptyRow = new Row();
                requisitionSheet.AddRow(defaultEmptyRow);

                RequisitionLinesExcelDTODefinition requisitionLinesExcelDTODefinition = new RequisitionLinesExcelDTODefinition(executionContext, "");
                requisitionLinesExcelDTODefinition.BuildHeaderRow(headerRowLines);
                requisitionLinesSheet.AddRow(headerRowLines);

                if (requisitionDTOList != null && requisitionDTOList.Any())
                {
                    foreach (RequisitionDTO requisitionDTO in requisitionDTOList)
                    {
                        if (requisitionDTO.RequisitionLinesListDTO != null && requisitionDTO.RequisitionLinesListDTO.Any())
                        {
                            foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionDTO.RequisitionLinesListDTO)
                            {
                                requisitionLinesExcelDTODefinition.Configure(requisitionLinesDTO);
                                Row rowForLines = new Row();
                                requisitionLinesExcelDTODefinition.Serialize(rowForLines, requisitionLinesDTO);
                                requisitionLinesSheet.AddRow(rowForLines);
                            }
                        }
                    }
                    sheets.Add(requisitionLinesSheet);

                }
            }
            log.LogMethodExit(sheets);
            return sheets;
        }


        public Dictionary<int, string> BulkUpload(List<Sheet> sheetList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheetList, sqlTransaction);

            Sheet requisitionSheet = new Sheet();
            Sheet requisitionLinesSheet = new Sheet();
            requisitionSheet = sheetList[0]; // Asssigning sheet[0] as requisition sheet
            requisitionLinesSheet = sheetList[1];

            RequisitionExcelDTODefinition requisitionExcelDTODefinition = new RequisitionExcelDTODefinition(executionContext, "");
            RequisitionLinesExcelDTODefinition requisitionLinesExcelDTODefinition = new RequisitionLinesExcelDTODefinition(executionContext, "");
            List<RequisitionDTO> rowRequisitionDTOList = new List<RequisitionDTO>();
            List<RequisitionLinesDTO> rowRequisitionLinesDTOList = new List<RequisitionLinesDTO>();

            for (int i = 1; i < requisitionSheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    RequisitionDTO rowRequisitionDTO = (RequisitionDTO)requisitionExcelDTODefinition.Deserialize(requisitionSheet[0], requisitionSheet[i], ref index);
                    rowRequisitionDTOList.Add(rowRequisitionDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                for (int j = 1; j < requisitionLinesSheet.Rows.Count; j++)
                {

                    int indexj = 0;
                    try
                    {
                        RequisitionLinesDTO rowRequisitionLinesDTO = (RequisitionLinesDTO)requisitionLinesExcelDTODefinition.Deserialize(requisitionLinesSheet[0], requisitionLinesSheet[j], ref indexj);
                        rowRequisitionLinesDTOList.Add(rowRequisitionLinesDTO);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                foreach (RequisitionDTO requisitionDTO in rowRequisitionDTOList)
                {
                    requisitionDTO.RequisitionLinesListDTO = rowRequisitionLinesDTOList;
                }
                try
                {
                    if (rowRequisitionDTOList != null && rowRequisitionDTOList.Any())
                    {
                        RequisitionList requisitionsListBL = new RequisitionList(executionContext, rowRequisitionDTOList);
                        requisitionsListBL.Save(sqlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            log.LogMethodExit(keyValuePairs);
            return keyValuePairs;
        }


        /// <summary>
        /// Saves RequisitionDTO
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            if (requisitionDTOList == null || requisitionDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < requisitionDTOList.Count; i++)
            {
                var requisitionDTO = requisitionDTOList[i];
                if (requisitionDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RequisitionBL requisitionBL = new RequisitionBL(executionContext, requisitionDTO);
                    //List<ValidationError> validationErrors = requisitionBL.Validate(sqlTransaction);
                    //if (validationErrors.Any())
                    //{
                    //    validationErrors.ToList().ForEach(c => c.RecordIndex = i + 1);
                    //    log.LogMethodExit(null, "Validation failed. " + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                    //    throw new ValidationException("Validation failed for Requisition.", validationErrors, i);
                    //}
                    requisitionBL.Save(sqlTransaction);
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
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving requisitionDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("requisitionDTO", requisitionDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Requisitions list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of RequisitionDTO</returns>
        public List<RequisitionDTO> GetAllRequisitionByRequisitionLineInfo(List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RequisitionDataHandler requisitionDataHandler = new RequisitionDataHandler(sqlTransaction);
            List<RequisitionDTO> requisitionDTOList = requisitionDataHandler.GetRequisitionByRequisitionLineInfo(searchParameters);
            log.LogMethodExit(requisitionDTOList);
            return requisitionDTOList;
        }
    }
}

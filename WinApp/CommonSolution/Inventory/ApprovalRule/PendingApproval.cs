/********************************************************************************************
 * Project Name - Inventory
 * Description  - Pending Approval
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
  *2.70.2        13-Aug-2019   Deeksha       Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class PendingApproval : UserControl
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private UserMessagesDTO userMessagesDTO;
        private InventoryDocumentTypeDTO inventoryDocumentTypeDTO;
        private int documentTypeId;
        public PendingApproval(Utilities _utilities, UserMessagesDTO _userMessagesDTO, int _documentTypeId)
        {
            log.LogMethodEntry(_utilities, _userMessagesDTO, _documentTypeId);
            InitializeComponent();
            utilities = _utilities;
            userMessagesDTO = _userMessagesDTO;
            documentTypeId = _documentTypeId;
            InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(utilities.ExecutionContext);
            inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType(documentTypeId, null);
            lblTypeOfApproval.Text = inventoryDocumentTypeDTO.Name;
            lblInitiator.Visible = false;
            lblInitiator.Text = "Initiated By: " + userMessagesDTO.CreatedBy;
            log.LogMethodExit();
        }


        public void PerformApproveAction()
        {
            log.LogMethodEntry();
            UserMessages userMessages = new UserMessages(utilities.ExecutionContext);
            using (ParafaitDBTransaction parafaitDBTrxn = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrxn.BeginTransaction();
                    userMessages.UpdateStatus(userMessagesDTO, UserMessagesDTO.UserMessagesStatus.APPROVED, documentTypeId, utilities.ParafaitEnv.User_Id, parafaitDBTrxn.SQLTrx);
                    InventoryIssueHeader inventoryIssueHeader ;
                    if (userMessagesDTO.ModuleType.Equals("Inventory") &&
                        (userMessagesDTO.ObjectType.Equals("AJIS") || userMessagesDTO.ObjectType.Equals("DIIS")
                        || userMessagesDTO.ObjectType.Equals("REIS") || userMessagesDTO.ObjectType.Equals("STIS")
                        || userMessagesDTO.ObjectType.Equals("ITIS")))
                    {
                        inventoryIssueHeader = new InventoryIssueHeader(userMessagesDTO.ObjectGUID, utilities.ExecutionContext);
                        inventoryIssueHeader.ProcessRequests(userMessagesDTO, UserMessagesDTO.UserMessagesStatus.APPROVED, utilities, parafaitDBTrxn.SQLTrx);
                    }
                    parafaitDBTrxn.EndTransaction();

                }
                catch (Exception ex)
                {
                    parafaitDBTrxn.RollBack();
                    log.Error("error while executing PerformApproveAction()" + ex.Message);
                    log.Fatal("Ends-PerformRejectAction() method. Thrown.");
                    throw ;
                }
            }
            log.LogMethodExit();
        }

        public void PerformRejectAction()
        {
            log.LogMethodEntry();
            UserMessages userMessages = new UserMessages(utilities.ExecutionContext);
            using (ParafaitDBTransaction parafaitDBTrxn = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrxn.BeginTransaction();
                    userMessages.UpdateStatus(userMessagesDTO, UserMessagesDTO.UserMessagesStatus.REJECTED, documentTypeId, utilities.ParafaitEnv.User_Id, parafaitDBTrxn.SQLTrx);
                    InventoryIssueHeader inventoryIssueHeader = new InventoryIssueHeader(utilities.ExecutionContext);
                    if (userMessagesDTO.ModuleType.Equals("Inventory") &&
                        (userMessagesDTO.ObjectType.Equals("AJIS") || userMessagesDTO.ObjectType.Equals("DIIS")
                        || userMessagesDTO.ObjectType.Equals("REIS") || userMessagesDTO.ObjectType.Equals("STIS")
                        || userMessagesDTO.ObjectType.Equals("ITIS")))
                    {
                        inventoryIssueHeader.ProcessRequests(userMessagesDTO, UserMessagesDTO.UserMessagesStatus.REJECTED, utilities, parafaitDBTrxn.SQLTrx);
                    }
                    parafaitDBTrxn.EndTransaction();
                }
                catch (Exception ex)
                {
                    parafaitDBTrxn.RollBack();
                    log.Error("error while executing PerformRejectAction()" + ex.Message);
                    log.Fatal("Ends-PerformRejectAction() method. Thrown.");
                    throw ;
                }
            }
            log.Debug("Ends-PerformRejectAction() method.");
        }
        public void PerformViewAction()
        {
            int fromSite = -1;
            log.LogMethodEntry();
            if (userMessagesDTO.ObjectType.Equals("RGPO"))
            {
                frmReceiveInventory frmReceiveInventory = new frmReceiveInventory(utilities,userMessagesDTO.ObjectGUID);
                frmReceiveInventory.ShowDialog();
            }
            else
            {
                fromSite = utilities.ParafaitEnv.SiteId;
                if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite && userMessagesDTO.ObjectType.Equals("ITIS"))
                {
                    InventoryIssueHeader issueHeader = new InventoryIssueHeader(userMessagesDTO.ObjectGUID, utilities.ExecutionContext);
                    utilities.ParafaitEnv.SiteId = issueHeader.getInventoryIssueHeaderDTO.SiteId;                    
                }
                InventoryIssueUI inventoryIssueUI = new InventoryIssueUI(utilities, userMessagesDTO.ObjectGUID);
                inventoryIssueUI.ShowDialog();
                utilities.ParafaitEnv.SiteId = fromSite;                
            }
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - Inventory
 * Description  - Inbox UI
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
using System.Collections.Generic;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class InboxUI : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private Utilities utilities;
        private string moduleType;
        public InboxUI(Utilities _utilities, string _moduleType)
        {
            log.LogMethodEntry(_utilities, _moduleType);
            InitializeComponent();
            utilities = _utilities;
            moduleType = _moduleType;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            log.LogMethodExit();
        }
        private void LoadHistory()
        {
            log.LogMethodEntry();
            UserMessagesList userMessagesList = new UserMessagesList();
            List<UserMessagesDTO> userMessagesDTOList = userMessagesList.GetHistoryUserMessage(utilities.ParafaitEnv.RoleId, utilities.ParafaitEnv.User_Id, moduleType, utilities.ParafaitEnv.Username, machineUserContext.GetSiteId(),(string.IsNullOrEmpty(cmbNotification.Text)?"NONE": cmbNotification.Text), null);
            BuildHistoryPanel(userMessagesDTOList);
            log.LogMethodExit();
        }
        private void BuildHistoryPanel(List<UserMessagesDTO> userMessagesDTOList)
        {
            log.LogMethodEntry(userMessagesDTOList);
            flpHistory.Controls.Clear();
            flpHistory.Controls.Add(pnlFilter);
            if (userMessagesDTOList != null)
            {
                foreach (UserMessagesDTO userMessagesDTO in userMessagesDTOList)
                {
                    TaskHistory taskHistory = new TaskHistory(utilities, userMessagesDTO);
                    taskHistory.Name = "Ucntrl" + userMessagesDTO.MessageId;
                    if (userMessagesDTO.Status.Equals(UserMessagesDTO.UserMessagesStatus.PENDING.ToString()))
                    {
                        taskHistory.lblHistoryMessage.Text = utilities.MessageUtils.getMessage(1546, userMessagesDTO.ObjectType, userMessagesDTO.Level);//"Task type " + userMessagesDTO.ObjectType + " pending for level " + userMessagesDTO.Level + " approval.";
                    }
                    else if (userMessagesDTO.Status.Equals(UserMessagesDTO.UserMessagesStatus.APPROVED.ToString()))
                    {
                        taskHistory.lblHistoryMessage.Text = utilities.MessageUtils.getMessage(1547, userMessagesDTO.ObjectType, userMessagesDTO.Level); //"Task type " + userMessagesDTO.ObjectType + " approved all levels(" + userMessagesDTO.Level + " levels).";
                    }
                    else if (userMessagesDTO.Status.Equals(UserMessagesDTO.UserMessagesStatus.REJECTED.ToString()))
                    {
                        taskHistory.lblHistoryMessage.Text = utilities.MessageUtils.getMessage(1548, userMessagesDTO.ObjectType, userMessagesDTO.Level);//"Task type " + userMessagesDTO.ObjectType + " rejected by " + userMessagesDTO.Level + " approver.";
                    }
                    else
                    {
                        taskHistory.lblHistoryMessage.Text = utilities.MessageUtils.getMessage(1549, userMessagesDTO.ObjectType, userMessagesDTO.Status.ToLower(), userMessagesDTO.Level);//"Task type " + userMessagesDTO.ObjectType + " is " + userMessagesDTO.Status.ToLower() + " by level " + userMessagesDTO.Level + ".";
                    }
                    taskHistory.lblHistoryDate.Text = userMessagesDTO.LastUpdatedDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    taskHistory.Tag = userMessagesDTO;
                    flpHistory.Controls.Add(taskHistory);
                }
            }
            log.LogMethodExit();
        }

        private void LoadPendingApprovals()
        {
            log.LogMethodEntry();
            UserMessagesList userMessagesList = new UserMessagesList();
            List<UserMessagesDTO> userMessagesDTOList = userMessagesList.GetAllMyPendingApprovalUserMessage(utilities.ParafaitEnv.RoleId, moduleType, machineUserContext.GetSiteId(), null);
            BuildApprovalPanel(userMessagesDTOList);
            log.LogMethodExit();
        }

        private void BuildApprovalPanel(List<UserMessagesDTO> userMessagesDTOList)
        {
            log.LogMethodEntry(userMessagesDTOList);
            flpPendingApproval.Controls.Clear();
            if (userMessagesDTOList != null)
            {
                foreach (UserMessagesDTO userMessagesDTO in userMessagesDTOList)
                {
                    InventoryDocumentTypeList inventoryDocumentTypeList;
                    InventoryDocumentTypeDTO inventoryDocumentTypeDTO;
                    int documentTypeid = -1;
                    if (moduleType.Equals("Inventory"))
                    {
                        inventoryDocumentTypeList = new InventoryDocumentTypeList(machineUserContext);
                        inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType(userMessagesDTO.ObjectType, machineUserContext.GetSiteId(),null);
                        if (inventoryDocumentTypeDTO == null)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1545));
                            break;
                        }
                        else
                        {
                            documentTypeid = inventoryDocumentTypeDTO.DocumentTypeId;
                        }
                    }
                    PendingApproval pendingApproval = new PendingApproval(utilities, userMessagesDTO, documentTypeid);
                    pendingApproval.Name = "Ucntrl" + userMessagesDTO.MessageId;
                    pendingApproval.lblMessage.Text = userMessagesDTO.Message;
                    pendingApproval.lblLevelOfApproval.Text = "Approval level: " + userMessagesDTO.Level;
                    pendingApproval.lblDate.Text = userMessagesDTO.CreationDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    pendingApproval.lnkApprove.LinkClicked+= new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkApprove_LinkClicked);
                    pendingApproval.lnkReject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkReject_LinkClicked);
                    //pendingApproval.Tag = userMessagesDTO;
                    //pendingApproval.lnkApprove.Tag = pendingApproval.lnkReject.Tag = documentTypeid;
                    if(userMessagesDTO.ApprovalRuleID==-1 && !userMessagesDTO.ObjectType.Equals("ITIS"))
                    {
                        pendingApproval.lnkApprove.Visible = false;
                        pendingApproval.lnkReject.Visible = false;
                        pendingApproval.lnkView.Text = utilities.MessageUtils.getMessage("Select Location");
                    }
                    pendingApproval.lnkView.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkView_LinkClicked);
                    flpPendingApproval.Controls.Add(pendingApproval);
                }
            }
            log.LogMethodExit();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadPendingApprovals();
            LoadHistory();
            log.LogMethodExit();
        }

        private void InboxUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            utilities.setLanguage(this);
            LoadPendingApprovals();
            LoadHistory();
            btnSearch.Width = 60;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }


        private void lnkApprove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                LinkLabel lnkLbl = (LinkLabel)sender;
                PendingApproval pa = (PendingApproval)lnkLbl.Parent;
                pa.PerformApproveAction();                
                btnRefresh.PerformClick();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to approve."+ ex.Message);
                log.Fatal("lnkApprove_LinkClicked() Event.Exception:" + ex.ToString());
            }
            finally
            {
                machineUserContext.SetSiteId((utilities.ParafaitEnv.IsCorporate) ? utilities.ParafaitEnv.SiteId : -1);
            }
            log.LogMethodExit();
        }
        private void lnkView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LinkLabel lnkLbl = (LinkLabel)sender;
            PendingApproval pa = (PendingApproval)lnkLbl.Parent;
            pa.PerformViewAction();
            machineUserContext.SetSiteId((utilities.ParafaitEnv.IsCorporate) ? utilities.ParafaitEnv.SiteId : -1);
            btnRefresh.PerformClick();
            log.LogMethodExit();
        }
        private void lnkReject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                log.LogMethodEntry(sender, e);
                LinkLabel lnkLbl = (LinkLabel)sender;
                PendingApproval pa = (PendingApproval)lnkLbl.Parent;
                pa.PerformRejectAction();
                btnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Rejection failed." + ex.Message);
                log.Fatal("lnkReject_LinkClicked() Event.Exception:" + ex.ToString());
            }
            finally
            {
                machineUserContext.SetSiteId((utilities.ParafaitEnv.IsCorporate) ? utilities.ParafaitEnv.SiteId : -1);
            }
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadHistory();
            log.LogMethodExit();
        }
    }
}

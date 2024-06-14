/********************************************************************************************
 * Project Name - Inventory
 * Description  - Task History
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
  *2.70.2        13-Aug-2019   Deeksha       Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class TaskHistory : UserControl
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private UserMessagesDTO userMessagesDTO;
        public TaskHistory(Utilities _utilities, UserMessagesDTO _userMessagesDTO)
        {
            log.LogMethodEntry(_utilities, _userMessagesDTO); 
            InitializeComponent();
            utilities = _utilities;
            userMessagesDTO = _userMessagesDTO;
            log.LogMethodExit();
        }

        private void lnkHistoryView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (userMessagesDTO.ObjectType.Equals("RGPO"))
            {
                frmReceiveInventory frmReceiveInventory = new frmReceiveInventory(utilities, userMessagesDTO.ObjectGUID);
                frmReceiveInventory.ShowDialog();
            }
            else
            {
                InventoryIssueUI inventoryIssueUI = new InventoryIssueUI(utilities, userMessagesDTO.ObjectGUID);
                inventoryIssueUI.ShowDialog();
            }
            log.LogMethodExit();
        }
    }
}

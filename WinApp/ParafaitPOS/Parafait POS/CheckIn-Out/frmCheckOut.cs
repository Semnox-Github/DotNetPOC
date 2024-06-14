/********************************************************************************************
 * Project Name - frmCheckOut
 * Description  - Check out selection form
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 *********************************************************************************************
*2.70.0       28-Jun-2019     Mathew Ninan   Use CheckInDTO and CheckInDetailDTO
*2.80         20-Aug-2019     Girish Kundar  Modified : Removed unused namespace's and Added logger methods. 
********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Parafait_POS
{
    public partial class CheckOut : Form
    {
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public delegate void ChekOutButtonSelection(object sender);
        /// <summary>
        /// call back method 
        /// </summary>
        public ChekOutButtonSelection setCallBack;

        public CheckOut(string childName, int CheckedInCount, int pausedCheckInCount , bool paused , CheckInDTO checkOutDTO,int pendingCheckInCount)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry(childName , CheckedInCount, pendingCheckInCount);
            POSStatic.Utilities.setLanguage();
            InitializeComponent();
            btnSingle.Text = POSStatic.Utilities.MessageUtils.getMessage("Check Out ") + childName;
            btnPauseSingle.Text = POSStatic.Utilities.MessageUtils.getMessage("Pause ") + childName;
            btnCheckInSingle.Text = POSStatic.Utilities.MessageUtils.getMessage("CheckIn ") + childName;
            if (checkOutDTO != null)
            {
                btnPauseSingle.Visible = btnPauseGroup.Visible = false;
                if (CheckedInCount <= 1)
                {
                    btnGroup.Visible = false;
                    btnCheckInGroup.Visible = false;
                    btnNo.Location = btnSingle.Location;
                    btnSingle.Location = btnGroup.Location;
                    this.Width = (this.Width - (btnGroup.Width * 3) - (btnGroup.Location.X * 3));                    
                }
                else
                {
                    btnNo.Location = btnPauseGroup.Location;
                    btnCheckInGroup.Visible = false;
                    this.Width = (this.Width - (btnPauseGroup.Width * 2) - (btnGroup.Location.X * 2));
                }
                if (pendingCheckInCount == 1)
                {
                    label1.Text = POSStatic.Utilities.MessageUtils.getMessage("Check In");
                    this.Text = POSStatic.Utilities.MessageUtils.getMessage("Perform Check In");
                    btnGroup.Visible = btnSingle.Visible = false;
                    btnCheckInGroup.Visible = false;
                    btnCheckInSingle.Location = btnGroup.Location;
                    btnNo.Location = btnSingle.Location;
                    btnCheckInSingle.Visible = true;
                    this.Width = (this.Width - (btnGroup.Width * 3) - (btnGroup.Location.X * 3));
                }
                else if(pendingCheckInCount > 1)
                {
                    label1.Text = POSStatic.Utilities.MessageUtils.getMessage("Check In");
                    this.Text = POSStatic.Utilities.MessageUtils.getMessage("Perform Check In");
                    btnGroup.Visible = btnSingle.Visible = false;
                    btnCheckInGroup.Visible = true;
                    btnNo.Location = btnPauseGroup.Location;
                    btnCheckInGroup.Location = btnGroup.Location;
                    btnCheckInSingle.Location = btnSingle.Location;
                    //this.Width = (this.Width - (btnGroup.Width * 3) - (btnGroup.Location.X * 3));
                    this.Width = (this.Width - (btnPauseGroup.Width * 2) - (btnGroup.Location.X * 2));
                }
            }
            if(checkOutDTO == null)
            {
                label1.Text = POSStatic.Utilities.MessageUtils.getMessage("Pause");
                this.Text = POSStatic.Utilities.MessageUtils.getMessage("Perform Pause");
                btnSingle.Visible = btnGroup.Visible = false;
                btnCheckInSingle.Visible = btnCheckInGroup.Visible = false;
                if(pausedCheckInCount > 1 || (CheckedInCount > 1 && pausedCheckInCount == -1)) //Group Pause
                {
                    btnNo.Location = btnPauseGroup.Location;
                    btnPauseGroup.Location = btnGroup.Location;
                    btnPauseSingle.Location = btnSingle.Location;
                    this.Width = (this.Width - (btnPauseGroup.Width * 2) - (btnGroup.Location.X * 2));
                }
                else  //Single user to be paused
                {
                    btnPauseGroup.Visible = false;
                    btnCheckInGroup.Visible = false;
                    btnNo.Location = btnSingle.Location;
                    btnPauseSingle.Location = btnGroup.Location;
                    this.Width = (this.Width - (btnPauseGroup.Width * 3) - (btnGroup.Location.X * 3));
                }
                
            }
            if(paused)
            {
                btnPauseGroup.Text = POSStatic.Utilities.MessageUtils.getMessage("UnPause Group");
                btnPauseSingle.Text = POSStatic.Utilities.MessageUtils.getMessage("UnPause ") + childName;
            }

            POSStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void btnChild_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            setCallBack(sender);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnFamily_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            setCallBack(sender);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void CheckOut_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (POSStatic.HIDE_CHECK_IN_DETAILS)
                btnChild_Click(null, null);

            log.LogMethodExit();
        }

        private void btnPauseGroup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            setCallBack(sender);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnPauseSingle_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            setCallBack(sender);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnCheckInSingle_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            setCallBack(sender);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }
        private void btnCheckInGroup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            setCallBack(sender);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }
    }
}

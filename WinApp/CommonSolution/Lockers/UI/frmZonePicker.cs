using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.Lockers
{
    public partial class frmZonePicker : Form
    {
        private static readonly  Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        string lockerMode;
        private LockerZonesDTO lockerZonesSelctedDTO;
        public LockerZonesDTO LockerZonesDTO { get { return lockerZonesSelctedDTO; } }
         Utilities utilities;
        public frmZonePicker( Utilities utilities, string lockerMode)
        {
            log.LogMethodEntry(utilities, lockerMode);
            InitializeComponent();
            this.utilities = utilities;
            this.lockerMode = lockerMode;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            LoadZone();
            log.LogMethodExit();
        }
        /// <summary>
        /// LoadZone
        /// </summary>
        private void LoadZone()
        {
            log.LogMethodEntry();
            LockerZonesList lockerZonesList = new LockerZonesList(machineUserContext);
            List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> lockerZoneSearchParams = new List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>>();
            lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            if(!string.IsNullOrEmpty(lockerMode))
            {
                lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.LOCKER_MODE, lockerMode));
            }
            lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            List<LockerZonesDTO> lockerParentZonesList = lockerZonesList.GetLockerZonesList(lockerZoneSearchParams);
            Button btn;
            flpZoneList.Controls.Clear();
            if(lockerParentZonesList!=null)
            {
                foreach (LockerZonesDTO lockerZonesDTO in lockerParentZonesList)
                {
                    btn = new Button();
                    btn.Name = lockerZonesDTO.ZoneName;
                    btn.Text = lockerZonesDTO.ZoneName;
                    btn.FlatStyle = btnSample.FlatStyle;
                    btn.Margin = btnSample.Margin;
                    btn.Size = btnSample.Size;
                    btn.BackgroundImage = btnSample.BackgroundImage;
                    btn.BackgroundImageLayout = btnSample.BackgroundImageLayout;
                    btn.FlatAppearance.BorderSize = btnSample.FlatAppearance.BorderSize;
                    btn.FlatAppearance.MouseOverBackColor = btnSample.FlatAppearance.MouseOverBackColor;
                    btn.Font = btnSample.Font;
                    btn.ForeColor = btnSample.ForeColor;
                    btn.Click += btnSample_Click;
                    btn.Tag = lockerZonesDTO;
                    flpZoneList.Controls.Add(btn);
                } 
            }
            else
            {
                log.Debug("lockerParentZonesList is Empty");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnSample_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSample_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button btnClicked = sender as Button;
            LockerZonesDTO lockerZonesDTO = btnClicked.Tag as LockerZonesDTO;
            lockerZonesSelctedDTO = lockerZonesDTO;
            this.DialogResult = DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnSelectAll_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            lockerZonesSelctedDTO = new LockerZonesDTO();
            lockerZonesSelctedDTO.ZoneCode = "ALL";
            lockerZonesSelctedDTO.LockerMode = ParafaitLockCardHandlerDTO.LockerSelectionMode.FREE.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnCancel_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }
    }
}

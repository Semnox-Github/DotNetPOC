/********************************************************************************************
 * Project Name - Media List Preview UI
 * Description  - User interface for Preview ui
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By           Remarks          
 *********************************************************************************************
 *2.70.2        14-Aug-2019   Dakshakh              Added logger methods
 *2.80.0        08-Jan-2019   Lakshminarayana       Modified for locker layout changes
 ********************************************************************************************/

using Semnox.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// PreviewUI Class.
    /// </summary>
    public partial class PreviewUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DisplayPanelDTO displayPanelDTO;
        private SignageClient.DisplayPanelBL displayPanel;
        private readonly DeviceClass readerDevice;

        /// <summary>
        /// Constructor of PreviewUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="displayPanelDTO">DisplayPanelDTO</param>
        /// <param name="readerDevice"></param>
        public PreviewUI(Utilities utilities, DisplayPanelDTO displayPanelDTO, DeviceClass readerDevice)
        {
            log.LogMethodEntry(utilities, displayPanelDTO);
            InitializeComponent();
            this.displayPanelDTO = displayPanelDTO;
            this.utilities = utilities;
            this.readerDevice = readerDevice;
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void PreviewUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dtpAsOnDate.Value = DateTime.Now;
            dtpAsOnTime.Value = DateTime.Now;
            log.LogMethodExit();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if(!string.IsNullOrWhiteSpace(displayPanelDTO.PCName))
            {
                DateTime dateTime = new DateTime(dtpAsOnDate.Value.Year, dtpAsOnDate.Value.Month, dtpAsOnDate.Value.Day, dtpAsOnTime.Value.Hour, dtpAsOnTime.Value.Minute, 0);
                displayPanel = new Semnox.Parafait.SignageClient.DisplayPanelBL(displayPanelDTO.PCName, dateTime, true, readerDevice);
                displayPanel.OnClose += DisplayPanel_OnClose;
                displayPanel.Start();
            }
            log.LogMethodExit();
        }

        private void DisplayPanel_OnClose(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.OK;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DisposeDisplayPanel();
            this.DialogResult = DialogResult.OK;
            log.LogMethodExit();
        }

        private void DisposeDisplayPanel()
        {
            log.LogMethodEntry();
            if(displayPanel != null)
            {
                displayPanel.Close();
                displayPanel.Dispose();
                displayPanel = null;
            }
            log.LogMethodExit();
        }
    }
}

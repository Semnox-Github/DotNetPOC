/********************************************************************************************
 * Project Name - Query PopUp UI
 * Description  - User interface for Query PopUp
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        14-Aug-2019   Deeksha             Added logger methods
 *2.100.0       13-Sep-2020   Girish Kundar       Modified : Phase -3 changes REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Windows.Forms;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Query Popup UI
    /// </summary>
    public partial class QueryPopupUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;
        int eventId = -1;
        string query = "";
        string eventName = "";
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        /// <summary>
        ///  Constructor of QueryPopupUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        /// <param name="eventId">EventId</param>
        /// <param name="query">Query</param>
        /// <param name="eventName">Eventname</param>
        public QueryPopupUI(Utilities utilities, int eventId, string query, string eventName = "")
        {
            log.LogMethodEntry(utilities, eventId, query, eventName);
            InitializeComponent();
            this.utilities = utilities;
            this.eventId = eventId;
            this.query = query;
            this.eventName = eventName;
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void QueryPopupUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblIDValue.Text = eventId.ToString();
            lblNameValue.Text = eventName;
            txtQuery.Focus();
            txtQuery.Text = query;
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (txtQuery.Text.Length == 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1130), utilities.MessageUtils.getMessage(1131));
            }
            else
            {
                EventDTO eventDTO = null;
                EventBL eventBL = new EventBL(machineUserContext, eventDTO);
                if(eventBL.CheckQuery(txtQuery.Text))
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1142));
                }
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.Cancel;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the query 
        /// </summary>
        public string QueryString
        {
            get
            {
                return txtQuery.Text;
            }

            set
            {
                txtQuery.Text = value;
            }
        }

    }
}

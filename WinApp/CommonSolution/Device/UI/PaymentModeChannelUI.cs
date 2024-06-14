/********************************************************************************************
 * Project Name - Device - UI
 * Description  - Class for  of PaymentModeChannelUI      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Device.PaymentGateway
{
    public partial class PaymentModeChannelUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int PaymentModeId = -1;
        public bool EditedFlag = false;
        private Utilities utilities;
        private ExecutionContext machineUserContext;

        public PaymentModeChannelUI(Utilities _Utilities, int paymentModeId)
        {
            log.LogMethodEntry(_Utilities, paymentModeId);
            PaymentModeId = paymentModeId;
            utilities = _Utilities;
            InitializeComponent();
            utilities.setupDataGridProperties(ref dgLookupValues);
            machineUserContext = ExecutionContext.GetExecutionContext();

            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }

            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            loadLookUps();
            log.LogMethodExit();
        }

        //PAYMENT_CHANNELS
        public void loadLookUps()
        {
            log.LogMethodEntry();
            string lookupName = "PAYMENT_CHANNELS";
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, lookupName));
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<LookupValuesDTO> LookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchlookupParameters);

            if (LookupValuesDTOList !=null && LookupValuesDTOList.Count > 0)
            {
                Core.GenericUtilities.SortableBindingList<LookupValuesDTO> sblookupValuesList = new SortableBindingList<LookupValuesDTO>(LookupValuesDTOList);
                dgLookupValues.DataSource = sblookupValuesList;
            }
            log.LogMethodExit();
        }

         
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(EditedFlag)
            {
              SavePaymentMode();
            }
            else
                return;
            log.LogMethodExit();
        }
        private void SavePaymentMode()
        {
            log.LogMethodEntry();
            if (dgLookupValues.CurrentRow == null || dgLookupValues.CurrentRow.IsNewRow || dgLookupValues.CurrentRow.Cells[0].Value == DBNull.Value)
                return;
            else
            {
                foreach (DataGridViewRow row in dgLookupValues.Rows)
                {
                    int lookupValueId = Convert.ToInt32(row.Cells["LookupValueId"].Value);

                    PaymentModeChannel paymentChannel = new PaymentModeChannel(machineUserContext);
                    PaymentModeChannelsDTO paymentChannelsDTO = getPaymentChannelsDTO(lookupValueId);

                    if (Convert.ToBoolean(row.Cells["chkPaymentEnable"].Value))
                    {
                        paymentChannelsDTO.PaymentModeId = PaymentModeId;
                        paymentChannelsDTO.LookupValueId = lookupValueId;
                        paymentChannel = new PaymentModeChannel(machineUserContext, paymentChannelsDTO);
                        paymentChannel.Save();
                    }
                    else
                    {
                        if (paymentChannelsDTO.PaymentModeChannelId >= 0)
                        {
                            paymentChannelsDTO.IsActive = false;
                            paymentChannel = new PaymentModeChannel(machineUserContext, paymentChannelsDTO);
                            paymentChannel.Save();
                            //int status = paymentChannel.Delete(paymentChannelsDTO.PaymentModeChannelId);
                        }
                    }
                    EditedFlag = false;
                }
            }
            log.LogMethodExit();
        }

        private PaymentModeChannelsDTO getPaymentChannelsDTO(int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            PaymentChannelList paymentChannelList = new PaymentChannelList(utilities.ExecutionContext);
            List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>> searchChannelParameters = new List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>>();
            searchChannelParameters.Add(new KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>(PaymentModeChannelsDTO.SearchByParameters.LOOKUP_VALUE_ID, lookupValueId.ToString()));
            searchChannelParameters.Add(new KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>(PaymentModeChannelsDTO.SearchByParameters.PAYMENT_MODE_ID, PaymentModeId.ToString()));
            List<PaymentModeChannelsDTO> paymentChannelsDTOList = paymentChannelList.GetAllPaymentChannels(searchChannelParameters);
            if (paymentChannelsDTOList.Count > 0)
            {
                log.LogMethodExit(paymentChannelsDTOList[0]);
                return paymentChannelsDTOList[0];
            }
            else
            {
                log.LogMethodExit();
                return new PaymentModeChannelsDTO();
            }
            
           
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender ,e);
            EditedFlag = false;
            loadLookUps();
            log.LogMethodExit();
        }

       

        private void dgLookupValues_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgLookupValues.CurrentRow == null || dgLookupValues.CurrentRow.IsNewRow || dgLookupValues.CurrentRow.Cells[0].Value == DBNull.Value)
                return;
            else
            {
                foreach (DataGridViewRow row in dgLookupValues.Rows)
                {
                    int lookupValueId = Convert.ToInt32(row.Cells["LookupValueId"].Value);

                    PaymentModeChannel paymentChannel = new PaymentModeChannel(machineUserContext);
                    PaymentModeChannelsDTO paymentChannelsDTO = getPaymentChannelsDTO(lookupValueId);
                    row.Cells["chkPaymentEnable"].Value = (paymentChannelsDTO.PaymentModeChannelId == -1 ? false : true);
                }
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void dgLookupValues_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                string headerText = dgLookupValues.Columns[e.ColumnIndex].HeaderText;
                if (headerText.ToLower() == "enable")
                {
                    EditedFlag = true;
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occurred at dgLookupValues_CellContentClick() method", ex);
                log.LogMethodExit(null, " Exception : " + ex.Message);
            }

        }        
    }
}

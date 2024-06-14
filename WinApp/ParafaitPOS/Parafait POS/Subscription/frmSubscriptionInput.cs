/********************************************************************************************
 * Project Name - FrmSubscriptionInput 
 * Description  - form class to receive Product Subscription Input
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     21-Dec-2020    Guru S A             Created for Subscription changes                                                                               
 ********************************************************************************************/
using System; 
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;

namespace Parafait_POS.Subscription
{
    /// <summary>
    /// FrmSubscriptionInput
    /// </summary>
    public partial class frmSubscriptionInput : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private int productId;
        private Products productsBL;
        //private ProductSubscriptionBL productScriptionBL;
        private SubscriptionHeaderDTO subscriptionHeaderDTO;
        private string NUMBER_FORMAT;
        private string AMOUNT_FORMAT;

        /// <summary>
        /// FrmSubscriptionInput
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="productId"></param>
        public frmSubscriptionInput(Utilities utilities, int productId)
        {
            log.LogMethodEntry(productId);
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            this.productId = productId;
            InitializeComponent();
            GetProductBL();
            BuildSubscriptionHeaderDTO();
            SetFormats();
            SetDisplay();
            this.utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void SetFormats()
        {
            log.LogMethodEntry();
            NUMBER_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            AMOUNT_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            log.LogMethodExit();
        }

        private void GetProductBL()
        {
            log.LogMethodEntry();
            productsBL = new Products(executionContext, productId, false);
            //ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext);
            //List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>>();
            //searchParam.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.PRODUCTS_ID, productId.ToString()));
            //searchParam.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            //List<ProductSubscriptionDTO> productSubscriptionDTOList = productSubscriptionListBL.GetProductSubscriptionDTOList(searchParam);
            //if (productSubscriptionDTOList != null && productSubscriptionDTOList.Any())
            //{
            //    productScriptionBL = new ProductSubscriptionBL(executionContext, productSubscriptionDTOList[0].ProductSubscriptionId);
            //}
            log.LogMethodExit();
        }

        private void BuildSubscriptionHeaderDTO()
        {
            log.LogMethodEntry();
            ProductSubscriptionBuilder productSubscriptionBuilder = new ProductSubscriptionBuilder(executionContext);
            subscriptionHeaderDTO = productSubscriptionBuilder.BuildSubscriptionDTO(productsBL.GetProductsDTO);
            log.LogMethodExit();
        }
        private void SetDisplay()
        {
            log.LogMethodEntry();
            LoadPaymentCollectionMode();
            if (subscriptionHeaderDTO != null)
            {
                txtSubscriptionCycle.Text = subscriptionHeaderDTO.SubscriptionCycle.ToString(NUMBER_FORMAT);
                txtSubscriptionCycleValidity.Text = subscriptionHeaderDTO.SubscriptionCycleValidity.ToString(NUMBER_FORMAT);
                txtSubscriptionName.Text = subscriptionHeaderDTO.ProductSubscriptionName;
                txtSubscriptionPrice.Text = subscriptionHeaderDTO.SubscriptionPrice.ToString(AMOUNT_FORMAT);
                txtUnitOfSubscriptionCycle.Text = UnitOfSubscriptionCycle.GetUnitOfSubscriptionCycleDescription(executionContext, subscriptionHeaderDTO.UnitOfSubscriptionCycle);
                cbxAutoRenew.Checked = subscriptionHeaderDTO.AutoRenew;
                cbxBillInAdvance.Checked = subscriptionHeaderDTO.BillInAdvance;
                cmbPaymentCollectionMode.SelectedValue = subscriptionHeaderDTO.SelectedPaymentCollectionMode;  
                if (subscriptionHeaderDTO.SelectedPaymentCollectionMode == SubscriptionPaymentCollectionMode.CUSTOMER_CHOICE)
                {
                    cmbPaymentCollectionMode.Enabled = true;
                }
                else
                {
                    cmbPaymentCollectionMode.Enabled = false;
                }
                if (subscriptionHeaderDTO.AutoRenew)
                {
                    cbxAutoRenew.Enabled = false;
                }
                else
                {
                    cbxAutoRenew.Enabled = true;
                }
            }
            log.LogMethodExit();
        }

        private void LoadPaymentCollectionMode()
        {
            log.LogMethodEntry();
            cmbPaymentCollectionMode = SubscriptionUIHelper.LoadSelectedPaymentCollectionMode(executionContext, cmbPaymentCollectionMode);
            log.LogMethodExit();
        }

        internal SubscriptionHeaderDTO GetSubscriptionHeaderDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit(subscriptionHeaderDTO);
            return subscriptionHeaderDTO;
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (cmbPaymentCollectionMode.Enabled && cmbPaymentCollectionMode.SelectedValue != null
                    && cmbPaymentCollectionMode.SelectedValue.ToString() == SubscriptionPaymentCollectionMode.CUSTOMER_CHOICE)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2892));
                    log.LogMethodExit("Please select payment collection mode for subscription");
                    return;
                }
                else
                {
                    subscriptionHeaderDTO.AutoRenew = cbxAutoRenew.Checked;
                    subscriptionHeaderDTO.SelectedPaymentCollectionMode = cmbPaymentCollectionMode.SelectedValue.ToString();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void blueButton_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button senderBtn = (Button)sender;
                senderBtn.BackgroundImage = global::Parafait_POS.Properties.Resources.pressed2;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void blueButton_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button senderBtn = (Button)sender;
                senderBtn.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}

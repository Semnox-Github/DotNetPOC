using com.clover.remotepay.sdk;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public partial class frmSignatureCaptureUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private VerifySignatureRequest signatureVerifyRequest;
        public VerifySignatureRequest VerifySignatureRequest
        {
            get
            {
                return signatureVerifyRequest;
            }
            set
            {
                signatureVerifyRequest = value;
                if (signatureVerifyRequest.Signature == null)
                {
                    spSignature.Visible = false;
                    lblMessage.Text = "Please verify signature on the paper.";
                }
                else
                {
                    lblMessage.Visible = false;
                    spSignature.Signature = signatureVerifyRequest.Signature;
                }
            }
        }
        public frmSignatureCaptureUI()
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.KeyPreview = true;
            log.LogMethodExit();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnAccept.Enabled = false;
            btnReject.Enabled = false;
            signatureVerifyRequest.Accept();
            this.DialogResult = DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnAccept.Enabled = false;
            btnReject.Enabled = false;
            signatureVerifyRequest.Reject();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }
    }   
}

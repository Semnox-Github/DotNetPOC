/********************************************************************************************
 * Project Name - AddressView
 * Description  -AddressView is a User Control component to display address details in the form.
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 2.70.2        18-Aug-2019       Girish Kundar   Created
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.Customer
{
    public partial class AddressView : UserControl
    {
        private readonly ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AddressDTO addressDTO = new AddressDTO();
        [field: NonSerialized]
        public event EventHandler UpdateList;
        private bool showKeyboardOnTextboxEntry;
        public event EventHandler<AddressDeleteEventArgs> DeleteAddressView;
        private bool controlEnabled = true;
        /// <summary>
        /// Constructor for AddressView
        /// </summary>
        public AddressView(bool showKeyboardOnTextboxEntry = true)
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.showKeyboardOnTextboxEntry = showKeyboardOnTextboxEntry;
            RecreateRegion();
            log.LogMethodExit();
        }

        /// <summary>
        /// Opens the frmCustomerAddress for the address Update.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LblAddress_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if(controlEnabled == false)
            {
                log.LogMethodExit(null, "Control disabled");
                return;
            }
            using (frmCustomerAddress frm = new frmCustomerAddress(addressDTO, showKeyboardOnTextboxEntry))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    UpdateUI();
                }
            }
            if (UpdateList != null)
            {
                UpdateList.Invoke(this, EventArgs.Empty);
            }
            UpdateList = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Remove the AddressDTO from the DTO List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (DeleteAddressView != null)
            {
                DeleteAddressView.Invoke(this, new AddressDeleteEventArgs(this.AddressDTO));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the Address Card View
        /// </summary>
        public void UpdateUI()
        {
            log.LogMethodEntry();
            lblAddress.Text = string.Empty;
            if (addressDTO.AddressType == AddressType.NONE)
                lblAddressType.Text = string.Empty;
            else
                lblAddressType.Text = addressDTO.AddressType.ToString();

            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(addressDTO.Line1))
            {
                sb.Append(string.Empty);
            }
            else
            {
                sb.Append(addressDTO.Line1);
            }

            if (string.IsNullOrEmpty(addressDTO.Line2))
            {
                sb.Append(string.Empty);
            }
            else
            {
                sb.AppendLine();
                sb.Append(addressDTO.Line2);
            }

            if (string.IsNullOrEmpty(addressDTO.Line3))
            {
                sb.Append(string.Empty);
            }
            else
            {
                sb.AppendLine();
                sb.Append(addressDTO.Line3);
            }

            if (string.IsNullOrEmpty(addressDTO.City))
            {
                sb.Append(string.Empty);
            }
            else
            {
                sb.AppendLine();
                sb.Append(addressDTO.City + " , ");
            }
            if (string.IsNullOrEmpty(addressDTO.City))
            {
                if (string.IsNullOrEmpty(addressDTO.PostalCode))
                {
                    sb.Append(string.Empty);
                }
                else
                {
                    sb.Append("\n" + addressDTO.PostalCode);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(addressDTO.PostalCode))
                {
                    sb.Append(string.Empty);
                }
                else
                {
                    sb.Append(addressDTO.PostalCode);
                }
            }


            if (string.IsNullOrEmpty(addressDTO.StateName))
            {
                sb.Append(string.Empty);
            }
            else
            {
                sb.Append(" ");
                sb.Append(addressDTO.StateName);
            }

            if (string.IsNullOrEmpty(addressDTO.CountryName))
            {
                sb.Append(string.Empty);
            }
            else
            {
                sb.AppendLine();
                sb.Append(addressDTO.CountryName);
            }

            sb.Append(".");
            lblAddress.Text = sb.ToString();
            log.LogMethodExit();
        }

        private int radius = 10;
        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                this.RecreateRegion();
            }
        }

        /// <summary>
        /// Method to get the rectangle object to make curved edges.
        /// </summary>
        /// <param name="bounds">bounds</param>
        /// <param name="radius">radius</param>
        /// <returns>GraphicsPath</returns>
        private GraphicsPath GetRoundRectangle(Rectangle bounds, int radius)
        {
            log.LogMethodEntry();
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y + bounds.Height - radius,
                        radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            log.LogMethodExit(path);
            return path;
        }

        /// <summary>
        /// Shows the Validation Error 
        /// </summary>
        /// <param name="addressDTO">addressDTO</param>
        public void ShowValidationError(AddressDTO addressDTO)
        {
            log.LogMethodEntry(addressDTO);
            if (this.addressDTO == addressDTO)
            {
                BackColor = Color.OrangeRed;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Clears the validation error color on control.
        /// </summary>
        public void ClearValdationError()
        {
            BackColor = Color.White;
        }

        /// <summary>
        /// Recreates the card view after update
        /// </summary>
        private void RecreateRegion()
        {
            log.LogMethodEntry();
            var bounds = ClientRectangle;
            bounds.Width--; bounds.Height--;
            using (var path = GetRoundRectangle(bounds, this.Radius))
                this.Region = new Region(path);
            this.Invalidate();
            log.LogMethodExit();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public AddressDTO AddressDTO
        {
            get
            {
                return addressDTO;
            }

            set
            {
                addressDTO = value;
                UpdateUI();
            }
        }

        internal void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            controlEnabled = value;
            btnClose.Visible = value;
            log.LogMethodExit();
        }

    }
}

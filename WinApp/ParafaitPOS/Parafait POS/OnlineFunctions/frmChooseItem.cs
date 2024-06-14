using Semnox.Core;
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

namespace Parafait_POS
{
    public partial class frmChooseItem : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<ItemPanel> itemPanelList;
        Utilities utilities;
        ItemPanel currentControl;
        public frmChooseItem(Utilities _utilities,  List<ItemPanel> itemPanelList)
        {
            log.LogMethodEntry();
            InitializeComponent();
            utilities = _utilities;
            this.itemPanelList = itemPanelList;
            this.DialogResult = DialogResult.Cancel;
            LoadForm();
            log.LogMethodExit(null);
        }
        private void LoadForm()
        {
            log.LogMethodEntry();
            if (itemPanelList != null)
            {
                flpItemControl.Controls.Clear();
                foreach (ItemPanel itemPanel in itemPanelList)
                {
                    itemPanel.Enter += new System.EventHandler(this.ItemPanel_Enter);
                    itemPanel.SetQuantityValue();
                    flpItemControl.Controls.Add(itemPanel);
                }
            }
            log.LogMethodExit(null);
        }
        private void ItemPanel_Enter(object sender, EventArgs e)
        {
            currentControl = sender as ItemPanel;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit(null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.OK;
            Close();
            log.LogMethodExit(null);
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();            
            showNumberPadForm('-');
            log.LogMethodExit(null);
        }
        void showNumberPadForm(char firstKey)
        {
            double varAmount = NumberPadForm.ShowNumberPadForm(utilities.MessageUtils.getMessage("Enter Amount"), firstKey, utilities);
            if (currentControl != null)
            {
                currentControl.SetQuantityValue(Convert.ToInt32(varAmount));
            }
            //if (varAmount >= 0)
            //{
            //    TextBox txtBox = null;
            //    try
            //    {
            //        if (this.ActiveControl.GetType().ToString().ToLower().Contains("textbox"))
            //            txtBox = this.ActiveControl as TextBox;
            //    }
            //    catch { }

            //    if (txtBox != null && !txtBox.ReadOnly)
            //    {
            //        txtBox.Text = varAmount.ToString();
            //        this.ValidateChildren();
            //    }
            //}
        }
    }
}

/********************************************************************************************
* Project Name - Parafait_POS - ItemPanel
* Description  - ItemPanel 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
********************************************************************************************* 
*2.60.0      21-Mar-2019      Iqbal              CHanges for reschedule feature 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class ItemPanel : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int productId;
        private string productName;
        private int productCount;
        private int actualCount;
        public int ProdId { get { return productId; } set { productId = value; } }
        public string ProdName { get { return productName; } set { productName = value; lblName.Text = productName + ": "; } }
        public int ProdCount { get { return productCount; } set { productCount = value; actualCount = productCount; } }
        public ItemPanel()
        {
            InitializeComponent();
        }

        private void nuQuantity_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (nuQuantity.Value > actualCount)
            {
                using (frmParafaitMessageBox msgBox = new frmParafaitMessageBox("Invalid Quantity", "Error", MessageBoxButtons.OK))
                {
                    msgBox.ShowDialog();
                    nuQuantity.Value = actualCount;
                }
            }
            else
            {
                productCount = Convert.ToInt32(nuQuantity.Value);
            }
            log.LogMethodExit();
        }
        public void SetQuantityValue(int quantity = -1)
        {
            log.LogMethodEntry(quantity);
            if (quantity > -1)
            {
                productCount = quantity;
            }
            nuQuantity.Value = productCount;
            log.LogMethodExit();
        }
    }
}

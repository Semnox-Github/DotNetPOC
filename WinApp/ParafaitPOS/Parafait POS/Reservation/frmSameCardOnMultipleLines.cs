/********************************************************************************************
 * Project Name - Reservation
 * Description  - frmSameCardOnMultipleLines form. Show summary of cards mapped to multiple lines
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.10.0     18-Sep-2022   Guru S A                Allow same card to be tapped on mulitple lines  
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmSameCardOnMultipleLines : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<KeyValuePair<string, List<string>>> sameCardProdList = new List<KeyValuePair<string, List<string>>>();
        private Utilities utilities = null;
        private int rowHeight = 30; 
        private int xAxisPosition = 5;
        private int yAxisPosition = 0;
        public frmSameCardOnMultipleLines(List<KeyValuePair<string, List<string>>> sameCardProdList)
        {
            log.LogMethodEntry(sameCardProdList);
            this.utilities = POSStatic.Utilities;
            InitializeComponent();
            this.sameCardProdList = sameCardProdList;
            utilities.setLanguage();
            ShowDataContent();
            utilities.setLanguage(this);
            this.DialogResult = DialogResult.OK;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void frmSameCardOnMultipleLines_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void ShowDataContent()
        {
            log.LogMethodEntry();
            if (sameCardProdList != null && sameCardProdList.Any())
            {
                pnlCardMapDetails.BorderStyle = BorderStyle.Fixed3D;
                pnlCardMapDetails.BackColor = Color.White;
                for (int i = 0; i < sameCardProdList.Count; i++)
                {
                    SetDataLineDetails(sameCardProdList[i]);
                }
                pnlCardMapDetails.Refresh();
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void SetDataLineDetails(KeyValuePair<string, List<string>> packageItem)
        {
            log.LogMethodEntry(packageItem); 
            Label lblItem = AddLabel("Card# " + packageItem.Key, xAxisPosition, 0); 
            if (packageItem.Value != null && packageItem.Value.Count > 0)
            {
                foreach (string packageChildItem in packageItem.Value)
                { 
                    AddLabel(packageChildItem, lblItem.Location.X , 2);
                }
                //pnlChildProduct.Refresh();
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private Label AddLabel(string data, int locationX, int sizePadding)
        {
            log.LogMethodEntry(data, locationX, sizePadding);
            Label lblItem = new Label();
            lblItem.Text = data;
            lblItem.Location = new Point(locationX + 5, yAxisPosition);
            lblItem.AutoSize = false;
            lblItem.TextAlign = ContentAlignment.MiddleLeft;
            lblItem.BackColor = Color.AliceBlue;
            lblItem.Width = pnlCardMapDetails.Width - 35- sizePadding;
            yAxisPosition = yAxisPosition + rowHeight;
            pnlCardMapDetails.Controls.Add(lblItem);
            log.LogMethodExit();
            return lblItem;
        } 
        private void btnNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.DialogResult = DialogResult.No;
            this.Close();
            log.LogMethodExit();
        } 
        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.DialogResult = DialogResult.Yes;
            this.Close();
            log.LogMethodExit();
        }

        private void ScrollBar_ButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
    }
}

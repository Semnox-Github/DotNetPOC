/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - Graph Form Select 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        13-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using System.Windows.Forms;

namespace ParafaitQueueManagement
{
    public partial class GraphFormSelect : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public GraphFormSelect()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void btngetGraph_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (graphrb1.Checked)
            {
                GraphicalView gview = new GraphicalView();
                gview.ShowDialog();
            }
            else
                if (graphrb2.Checked)
                {
                    BowlerStats bwlerstatistics = new BowlerStats();
                    bwlerstatistics.ShowDialog();
                }
            log.LogMethodExit();
        }

        private void GraphFormSelect_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();

        }
    }
}

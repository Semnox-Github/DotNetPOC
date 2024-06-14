/********************************************************************************************
 * Project Name - Digital Signage
 * Description  - ViewZone
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Deeksha             Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Parafait.DigitalSignage
{
    public partial class ViewZone : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        long ScreenID;
        int topLeftG, bottomRightG;
        string zoneNameG;
        List<int> cellList = new List<int>();
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private int verticalDiv = -1;
        private int horizontalDiv = -1;
        public ViewZone(long screenID,string zoneName, int topLeft, int bottomRight)
        {
            log.LogMethodEntry(screenID, zoneName, topLeft, bottomRight);
            InitializeComponent();
            //-button name is changed from 'Close' to 'btnClose'-as it was showing warning while compiling-28-05-2015
            ScreenID = screenID;
            topLeftG = topLeft;
            bottomRightG = bottomRight;
            zoneNameG = zoneName;
            log.LogMethodExit();
        }

        private void ViewZone_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ScreenSetupList screenSetupList = new ScreenSetupList(machineUserContext);
            List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>> screenSetupSearchParams = new List<KeyValuePair<ScreenSetupDTO.SearchByParameters, string>>();
            screenSetupSearchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.IS_ACTIVE, "1"));
            screenSetupSearchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.SCREEN_ID, ScreenID.ToString()));
            //screenSetupSearchParams.Add(new KeyValuePair<ScreenSetupDTO.SearchByParameters, string>(ScreenSetupDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<ScreenSetupDTO> screenSetupListOnDisplay = screenSetupList.GetAllScreenSetup(screenSetupSearchParams);
            if (screenSetupListOnDisplay!=null&&screenSetupListOnDisplay.Count > 0)
            {
                verticalDiv = Convert.ToInt32(screenSetupListOnDisplay[0].ScrDivVertical);
                horizontalDiv = Convert.ToInt32(screenSetupListOnDisplay[0].ScrDivHorizontal);
                populatePanel(Convert.ToInt32(screenSetupListOnDisplay[0].ScrDivHorizontal), Convert.ToInt32(screenSetupListOnDisplay[0].ScrDivVertical));
                MarkLabels(topLeftG, bottomRightG);
                lblName.Text = zoneNameG;
            }
            log.LogMethodExit();
        }

        private void populatePanel(int rows, int columns)
        {
            log.LogMethodEntry(rows, columns);
            panelZone.Controls.Clear();
            int wid = panelZone.Width / columns;
            int ht = panelZone.Height / rows;
            int num = 1;
            int yrowlabel = panelZone.Location.Y;
            int yrowlabelVertical = panelZone.Location.Y;
            int xrowlabel = panelZone.Location.X;
            for (int i = 0; i < rows; i++)
            {
                Label rowLabel = new Label();
                rowLabel.Font = new Font("arial", 7, FontStyle.Regular);
                rowLabel.Text = (i + 1).ToString("00");
                rowLabel.Tag = Convert.ToString(i + 1);
                rowLabel.Size = new System.Drawing.Size(wid, ht);
                rowLabel.Location = new Point(panelZone.Location.X - 35, yrowlabel);
                rowLabel.ForeColor = Color.SteelBlue;
                rowLabel.BackColor = Color.White;
                yrowlabel = yrowlabel + ht;
                grpZoneDetails.Controls.Add(rowLabel);

                for (int j = 0; j < columns; j++)
                {
                    if (i == 0)
                    {
                        Label colLabel = new Label();
                        colLabel.Font = new Font("arial", 7, FontStyle.Regular);
                        colLabel.Text = (j + 1).ToString("00");
                        colLabel.Tag = Convert.ToString(j + 1);
                        colLabel.Size = new System.Drawing.Size(wid, ht);
                        colLabel.Location = new Point(xrowlabel, panelZone.Location.Y - 20);
                        colLabel.ForeColor = Color.SteelBlue;
                        colLabel.BackColor = Color.White;
                        xrowlabel = xrowlabel + wid;
                        grpZoneDetails.Controls.Add(colLabel);
                    }

                    Label cell = new Label();
                    cell.Name = "quad";
                    cell.Font = new Font("arial", 10, FontStyle.Regular);
                    cell.Size = new Size(wid, ht);
                    cell.BorderStyle = BorderStyle.FixedSingle;
                    cell.Text = Convert.ToString(num);
                    cell.ForeColor = Color.DarkGray;
                    cell.BackColor = Color.White;
                    cell.Location = new Point(j * wid, i * ht);
                    cell.TextAlign = ContentAlignment.MiddleCenter;
                    cell.Tag = Convert.ToInt32(num);
                    cell.Click += new EventHandler(cell_Click);
                    panelZone.Controls.Add(cell);
                    num++;
                }
            }
            log.LogMethodExit();
        }

        private void cell_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Label lblClicked = (Label)sender;
            lblClicked.BackColor = Color.Red;
            cellList.Add(Convert.ToInt32(lblClicked.Tag));
            log.LogMethodExit();
        }

        private void MarkLabels(int lowLimit, int highLimit)
        {
            log.LogMethodEntry(lowLimit, highLimit);
            int beforeColumns = lowLimit % verticalDiv;
            int afterColumns = highLimit % verticalDiv;
            if(afterColumns == 0)
            {
                afterColumns = verticalDiv;
            }
            if(beforeColumns == 0)
            {
                beforeColumns = verticalDiv;
            }
            for (int i = 0; i < panelZone.Controls.Count; i++)
            {
                Control c = panelZone.Controls[i];
                if (Convert.ToInt32(c.Text) >= lowLimit && Convert.ToInt32(c.Text) <= highLimit)
                {
                    if(((i + 1) % verticalDiv >= beforeColumns || (i + 1) % verticalDiv == 0) && 
                        ((i + 1) % verticalDiv <= afterColumns && (afterColumns == verticalDiv || (i + 1) % verticalDiv != 0)))
                    {
                        c.BackColor = Color.Red;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            //-button name is changed from 'Close' to 'btnClose'-as it was showing warning while compiling-28-05-2015
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

    }
}

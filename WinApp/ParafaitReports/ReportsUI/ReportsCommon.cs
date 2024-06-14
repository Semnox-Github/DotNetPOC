/********************************************************************************************
* Project Name - Parafait Report
* Description  - ReportCommon 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80       23-Aug-2019      Jinto Thomas        Added logger into methods
********************************************************************************************/
using System;
using Semnox.Parafait.Reports;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Parafait.Report.Reports
{
    static class ReportsCommon
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<string> openForms = new List<string>();
        public static Color SkinColor = Color.White;
        public static Semnox.Core.Utilities.DeviceClass ReaderDevice;
        public static void openForm(Form parentForm, string childFormName, object[] Params, bool Reuse, bool modal)
        {
            log.LogMethodEntry(parentForm, childFormName, Params, Reuse, modal);
            Form form;
            if (Reuse)
            {
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == childFormName)
                    {                        
                        int pos = -1;
                        for (int i = 0; i < openForms.Count; i++)
                        {
                            if (openForms[i] == f.Name)
                                pos = i;
                        }
                        if (pos >= 0)
                        {
                            openForms.RemoveAt(pos);
                        }
                        openForms.Add(f.Name);
                        f.Show();
                        f.Activate();
                        f.Focus();
                        log.LogMethodExit("f.Name == childFormName");
                        return;
                    }
                }
            }

            try
            {
                switch (childFormName)
                {
                    case "RunReports": form = new RunReports(); break;
                    case "GamePlayReportviewer":
                        form = new GamePlayReportviewer((bool)Params[0], (int)Params[1], (string)Params[2], (string)Params[3], (string)Params[4], (DateTime)Params[5], (DateTime)Params[6], (List<clsReportParameters.SelectedParameterValue>)Params[7], (string)Params[8]); break;
                    case "RetailReportviewer":
                        form = new RetailReportviewer((bool)Params[0], (int)Params[1], (string)Params[2], (string)Params[3], (string)Params[4], (DateTime)Params[5], (DateTime)Params[6], (string)Params[7]); break;
                    case "SKUSearchReportviewer":
                        form = new SKUSearchReportviewer((bool)Params[0], (int)Params[1], (string)Params[2], (string)Params[3], (string)Params[4], (DateTime)Params[5], (DateTime)Params[6], (string)Params[7]); break;
                    case "TransactionReportviewer":
                        form = new TransactionReportviewer((bool)Params[0], (int)Params[1], (string)Params[2], (string)Params[3], (string)Params[4], (DateTime)Params[5], (DateTime)Params[6], (List<clsReportParameters.SelectedParameterValue>)Params[7], (string)Params[8]); break;
                    case "CustomReportviewer":
                        //int ReportID = Convert.ToInt32( Params[4]);
                        form = new CustomReportviewer((bool)Params[0], (string)Params[1], (string)Params[2], (string)Params[3], (int)Params[4], (DateTime)Params[5], (DateTime)Params[6], (int)Params[7], (List<clsReportParameters.SelectedParameterValue>)Params[8], (string)Params[9]); break;
                    case "GenericReportviewer":
                        form = new GenericReportViewer((bool)Params[0], (int)Params[1], (string)Params[2], (string)Params[3], (string)Params[4], (DateTime)Params[5], (DateTime)Params[6], (List<clsReportParameters.SelectedParameterValue>)Params[7], (string)Params[8]); break;
                    default: MessageBox.Show("Report Not Found", "Error"); return;
                }
            }
            catch(Exception ex)
            {
                log.Error("Throwing an exception " + ex.Message);
                throw new Exception(ex.Message);
            }
           
            form.Name = childFormName;
            Common.setupVisuals(form);

            try
            {
                form.Icon = new System.Drawing.Icon(Environment.CurrentDirectory + "\\Resources\\Parafait icon.ico");
            }
            catch { }

            if (modal)
            {
                form.Owner = parentForm;
                form.ShowDialog();
            }
            else
            {                
                form.Location = new System.Drawing.Point(0, 0);
                form.FormBorderStyle = FormBorderStyle.Sizable;
                form.StartPosition = FormStartPosition.Manual;                
                form.MdiParent = parentForm;
                form.Width = SystemInformation.WorkingArea.Width - 10;
                form.Height = SystemInformation.WorkingArea.Height - 60;
                form.AutoScroll = true;
                int pos = -1;
                for (int i = 0; i < openForms.Count; i++)
                {
                    if (openForms[i] == form.Name)
                        pos = i;
                }
                if (pos >= 0)
                {
                    openForms.RemoveAt(pos);
                }
                openForms.Add(form.Name);
                form.Show();
                form.Activate();                
            }
            log.LogMethodExit();
        }
    }
}

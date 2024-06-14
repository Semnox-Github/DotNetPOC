/********************************************************************************************
 * Project Name - Waiver
 * Description  - WaiverSignedUI class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Girish kundar    Modified :Added Logger methods and Removed Unused namespace's. 
  *2.70.2       15-Oct-2019   GUru S A         Waiver phase 2 changes
  *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Waiver
{
    public partial class WaiverSignedUI : Form
    {

        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<WaiverSignatureDTO> waiverSignedDTOList;
        private int TrxId;
        private int LineId;
        List<TabPage> tabPageList = new List<TabPage>();
        List<WebBrowser> webBrowserList = new List<WebBrowser>();
        int waiverCount = 1;
        bool isManualWaiver = true;
        string folderPath;
        /// <summary>
        /// Constructor of WaiverSetUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public WaiverSignedUI(int TrxId, int lineId, Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;

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
            this.TrxId = TrxId;
            this.LineId = lineId;
            folderPath = utilities.getParafaitDefaults("IMAGE_DIRECTORY");

            log.LogMethodExit();
        }

        private void WaiverSignedUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadWaiverSignedDocument();
            if (isManualWaiver)
            {
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2345, MessageContainerList.GetMessage(machineUserContext, "Waiver"), MessageContainerList.GetMessage(machineUserContext, "view")),
                                                MessageContainerList.GetMessage(machineUserContext, "View Waivers"));
                this.Close();
            }
            log.LogMethodExit();
        }

        private void LoadWaiverSignedDocument()
        {
            log.LogMethodEntry();
            try
            {
                WaiverSignatureListBL waversignedListBL = new WaiverSignatureListBL(machineUserContext);
                List<KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>> searchParameters = new List<KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>>();
                searchParameters.Add(new KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>(WaiverSignatureDTO.SearchByWaiverSignatureParameters.TRX_ID, this.TrxId.ToString()));
                searchParameters.Add(new KeyValuePair<WaiverSignatureDTO.SearchByWaiverSignatureParameters, string>(WaiverSignatureDTO.SearchByWaiverSignatureParameters.LINE_ID, this.LineId.ToString()));
                waiverSignedDTOList = waversignedListBL.GetWaiverSignatureDTOList(searchParameters);
                log.LogVariableState("WaiverSignedDTOList", waiverSignedDTOList);
                if (waiverSignedDTOList != null)
                {
                    for (int i = 0; i < waiverSignedDTOList.Count; i++)
                    {
                        //if(string.Equals(waiverSignedDTOList[i].SignedMode,"ONLINE"))
                        //{
                        //    string url = utilities.getParafaitDefaults("ONLINE_WAIVER_URL")+ "//"+ waiverSignedDTOList[i].CustomerSignedWaiverFileName;

                        //    using (WebClient wc = new WebClient())
                        //    {
                        //        wc.DownloadFileAsync(
                        //            new System.Uri(url),
                        //            Path.GetTempPath() + waiverSignedDTOList[0].CustomerSignedWaiverFileName
                        //        );
                        //    }

                        //    axAcroPDF.Name = waiverSignedDTOList[i].GUID;
                        //    axAcroPDF.CreateControl();
                        //    axAcroPDF.Margin = new Padding(0, 0, 0, 0);

                        //    string fileName = DecryptFile(waiverSignedDTOList[0].CustomerSignedWaiverFileName, Path.GetTempPath() + waiverSignedDTOList[0].CustomerSignedWaiverFileName);
                        //    log.Debug("fileName : " + fileName);
                        //    axAcroPDF.src = fileName;
                        //    if (!(File.Exists(fileName)))
                        //    {
                        //        throw new Exception(utilities.MessageUtils.getMessage(1508));
                        //    }
                        //    axAcroPDFList.Add(axAcroPDF);
                        //    tbCtrlViewWaiver.ItemSize = new Size(90, 50);
                        //    tabPage1.Text = "  Waiver  " + waiverCount++;
                        //    tabPageList.Add(tabPage1);
                        //    try
                        //    {
                        //        File.Delete(Path.GetTempPath() + waiverSignedDTOList[0].CustomerSignedWaiverFileName);
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        log.Error("File deletion failed." + ex.Message);
                        //    }
                        //    log.Debug("Ends SignedMode = ONLINE");
                        //}
                        //else
                        {
                            if (i == 0)
                            {
                                webBrowser.Name = waiverSignedDTOList[i].GUID;
                                webBrowser.CreateControl();

                                string file = waiverSignedDTOList[0].CustomerSignedWaiverFileName;
                                if (string.IsNullOrEmpty(file) == false)
                                {
                                    isManualWaiver = false;
                                    log.Debug("fileName : " + file);
                                    if (!file.Contains(".pdf"))
                                    {
                                        file = waiverSignedDTOList[0].CustomerSignedWaiverFileName + ".pdf";
                                    }
                                    log.LogVariableState("File Name : ", file);
                                    SqlCommand cmdImage = utilities.getCommand();
                                    cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";
                                    cmdImage.Parameters.AddWithValue("@FileName", folderPath + "\\" + file);
                                    try
                                    {
                                        object obj = cmdImage.ExecuteScalar();
                                        if (obj != null)
                                        {
                                            byte[] bytes = new byte[0];
                                            bytes = obj as byte[];
                                            using (MemoryStream ms = new MemoryStream(bytes))
                                            {
                                                File.WriteAllBytes(Path.GetTempPath() + file, bytes);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        log.Error("Error Reading File from " + folderPath);
                                    }

                                    webBrowser.Margin = new Padding(0, 0, 0, 0);

                                    string fileName = DecryptFile(file, Path.GetTempPath() + file);
                                    log.Debug("fileName : " + fileName);
                                    //axAcroPDF.src = fileName;
                                    if (!(File.Exists(fileName)))
                                    {
                                        throw new Exception(utilities.MessageUtils.getMessage(1508));
                                    }
                                    Uri urlLink = new Uri(fileName);
                                    webBrowser.Navigate(urlLink, false);
                                    webBrowserList.Add(webBrowser);
                                    tbCtrlViewWaiver.ItemSize = new Size(90, 50);
                                    tabPage1.Text = "  Waiver  " + waiverCount++;
                                    tabPageList.Add(tabPage1);
                                    //try
                                    //{
                                    //    File.Delete(Path.GetTempPath() + waiverSignedDTOList[0].CustomerSignedWaiverFileName);
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    log.Error("File deletion failed." + ex.Message);
                                    //}
                                }
                            }
                            else
                            {
                                TabPage tabPage = new TabPage();
                                tbCtrlViewWaiver.TabPages.Add(tabPage);
                                webBrowser = new WebBrowser();
                                tabPage.Controls.Add(webBrowser);
                                webBrowser.CreateControl();
                                webBrowser.Dock = DockStyle.Fill;
                                webBrowser.Width = tabPage.Width;
                                webBrowser.Height = tabPage.Height;
                                webBrowser.Margin = new Padding(0, 0, 0, 0);
                                tabPage.Name = waiverSignedDTOList[i].GUID;
                                webBrowser.Name = waiverSignedDTOList[i].GUID;
                                tabPage.Tag = waiverSignedDTOList[i];
                                tabPageList.Add(tabPage);
                                webBrowserList.Add(webBrowser);

                                if (string.IsNullOrEmpty(waiverSignedDTOList[i].CustomerSignedWaiverFileName) == false)
                                {
                                    isManualWaiver = false;
                                }

                                tabPage.Text = "  Waiver  " + waiverCount++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while loading WaiverSigedDTO list", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();

        }

        private void tbCtrlViewWaiver_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (tbCtrlViewWaiver.SelectedTab != null &&
                   tbCtrlViewWaiver.SelectedTab.Tag != null &&
                   tbCtrlViewWaiver.SelectedTab.Tag is WaiverSignatureDTO)
            {
                WaiverSignatureDTO temp = tbCtrlViewWaiver.SelectedTab.Tag as WaiverSignatureDTO;
                foreach (WebBrowser webBrowser in webBrowserList)
                {
                    if (webBrowser.Name == temp.GUID)
                    {
                        SqlCommand cmdImage = utilities.getCommand();
                        cmdImage.CommandText = "exec ReadBinaryDataFromFile @FileName";
                        string file = folderPath + "\\" + temp.CustomerSignedWaiverFileName;
                        if (string.IsNullOrEmpty(temp.CustomerSignedWaiverFileName) == false)
                        {
                            isManualWaiver = false;
                            if (!file.Contains(".pdf"))
                            {
                                file = folderPath + "\\" + temp.CustomerSignedWaiverFileName + ".pdf";
                            }
                            cmdImage.Parameters.AddWithValue("@FileName", file);
                            try
                            {
                                file = temp.CustomerSignedWaiverFileName;
                                if (!file.Contains(".pdf"))
                                {
                                    file = temp.CustomerSignedWaiverFileName + ".pdf";
                                }
                                log.LogVariableState("File Name : ", file);
                                object obj = cmdImage.ExecuteScalar();
                                if (obj != null)
                                {
                                    byte[] bytes = new byte[0];
                                    bytes = obj as byte[];
                                    using (MemoryStream ms = new MemoryStream(bytes))
                                    {
                                        File.WriteAllBytes(Path.GetTempPath() + file, bytes);
                                    }
                                }
                                //try
                                //{
                                //    File.Delete(Path.GetTempPath() + temp.CustomerSignedWaiverFileName);
                                //}
                                //catch (Exception ex)
                                //{
                                //    log.Error("File deletion failed." + ex.Message);
                                //}
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error Reading File from " + folderPath + ex.Message);
                            }

                            string fileName = DecryptFile(temp.CustomerSignedWaiverFileName, Path.GetTempPath() + temp.CustomerSignedWaiverFileName);
                            log.Debug("fileName : " + fileName);
                            //axAcroPDF.src = fileName;
                            Uri urlLink = new Uri(fileName);
                            webBrowser.Navigate(urlLink, false);
                            try
                            {
                                if (!(File.Exists(fileName)))
                                {
                                    throw new Exception(utilities.MessageUtils.getMessage(1508));
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error("Error occurred while loading WaiverSigedDTO list", ex);
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            webBrowser.DocumentText = MessageContainerList.GetMessage(machineUserContext, 2345, 
                                                                                  MessageContainerList.GetMessage(machineUserContext, "Waiver"), 
                                                                                  MessageContainerList.GetMessage(machineUserContext, "view")); 
                        }
                    }
                }
            }

            log.LogMethodExit();
        }

        private string DecryptFile(string inputFile, string outputFile)
        {
            log.LogMethodEntry(inputFile, outputFile);
            string password = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "WAIVER_ENCRYPTION_KEY");
            try
            {
                if (!(File.Exists(Path.GetTempPath() + inputFile)))
                {
                    GenericUtils genericUtils = new GenericUtils();
                    try
                    {
                        byte[] bytes = genericUtils.GetFileFromServer(utilities, utilities.getParafaitDefaults("IMAGE_DIRECTORY") + "\\" + inputFile);
                        File.WriteAllBytes(Path.GetTempPath() + inputFile, bytes);
                        if (!(File.Exists(Path.GetTempPath() + inputFile)))
                        {
                            throw new Exception(utilities.MessageUtils.getMessage(1508));
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                }

                log.Debug("Getting the file name from the Temp path");
                string filename = Path.GetTempPath() + inputFile;

                log.Debug("Initialize the PdfReader object");
                PdfReader reader = new PdfReader(filename, new System.Text.ASCIIEncoding().GetBytes(password));

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    PdfStamper stamper = new PdfStamper(reader, memoryStream);
                    stamper.Close();
                    reader.Close();
                    File.WriteAllBytes(outputFile, memoryStream.ToArray());
                }
                reader.Dispose();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit("Returning Decrypted File");
            return outputFile;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void WaiverSignedUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            DeleteFiles();
            log.LogMethodExit();
        }

        private void DeleteFiles()
        {
            log.LogMethodEntry();
            foreach (WebBrowser webBrowser in webBrowserList)
            {
                try
                {
                    FileInfo file = new FileInfo(webBrowser.Url.LocalPath.ToString());
                    if (file.Exists)
                    {
                        webBrowser.Dispose();
                        file.Delete();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }

            }
            log.LogMethodExit();
        }
    }
}

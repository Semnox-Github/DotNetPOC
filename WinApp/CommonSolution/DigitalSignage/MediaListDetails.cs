/********************************************************************************************
 * Project Name - Media List Details UI
 * Description  - User interface for media list details ui
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        09-Feb-2017   Raghuveera     Created 
 *2.70.2      14-Aug-2019   Dakshakh       Added logger methods
 *2.80.0      17-Feb-2019   Deeksha        Modified to Make DigitalSignage module as
 *                                         read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Semnox.Core.Utilities;
using Microsoft.VisualBasic.FileIO;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// User interface for media list details 
    /// </summary>
    public partial class MediaListDetails : Form
    {
        private long mediaID;
        private FileInfo finfo;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private MediaDTO mediaDTO = null;
        bool fileChoosen = false;
        private Utilities utilities; //= new ParafaitUtils.Utilities();
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        //BackgroundWorker bgworker = new BackgroundWorker();
        private MediaList mediaList;
        private List<LookupValuesDTO> lookupValuesDTOList;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Media List constructor 
        /// </summary>
        /// <param name="_Utilities">Holds the environment</param>
        /// <param name="mediaListID">The media id which is need to be displayed.</param>
        public MediaListDetails(Utilities _Utilities, long mediaListID)
        {
            log.LogMethodEntry(_Utilities, mediaListID);
            InitializeComponent();
            mediaID = mediaListID;
            mediaList = new MediaList(machineUserContext);
            mediaDTO = new MediaDTO();
            utilities = _Utilities;
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the status to the combo boxes
        /// </summary>
        private void LoadMediaType()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MEDIA_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if(lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDTOList[0].LookupValueId = -1;
                lookupValuesDTOList[0].LookupValue = "<SELECT>";
                bindingSource.DataSource = lookupValuesDTOList;
                cmbType.DataSource = lookupValuesDTOList;
                cmbType.ValueMember = "LookupValueId";
                cmbType.DisplayMember = "LookupValue";

                log.LogMethodExit();
            }
            catch(Exception e)
            {
                log.Error(e.Message);
            }
        }
        private void setPrevNext()
        {
            log.LogMethodEntry();

            List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>> mediaSearchParams = new List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>>();
            mediaSearchParams.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.IS_ACTIVE, "1"));
            mediaSearchParams.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<MediaDTO> mediaListOnDisplay = mediaList.GetAllMedias(mediaSearchParams);
            if(mediaListOnDisplay == null || mediaListOnDisplay.Count == 0)
            {
                lnkPrevious.Enabled = false;
                lnkNext.Enabled = false;
            }
            log.LogMethodExit();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int result;
            if(txtName.Text == string.Empty)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", lblName.Text.Replace("*", "").Replace(":", "")));
                return;
            }
            if(txtFileName.Text == string.Empty)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", lblFileName.Text.Replace("*", "").Replace(":", "")));
                return;
            }
            if(cmbType.SelectedIndex == 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", lblType.Text.Replace("*", "").Replace(":", "")));
                return;
            }
            if(txtSizeXInPixels.Text == string.Empty && txtSizeYInPixels.Text != string.Empty)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1145));
                return;
            }
            if(txtSizeXInPixels.Text != string.Empty && txtSizeYInPixels.Text == string.Empty)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1146));
                return;
            }
            if(!int.TryParse(txtSizeXInPixels.Text, out result) && txtSizeXInPixels.Text != string.Empty)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1133).Replace("&1", lblSizeXInPixels.Text.Replace("*", "").Replace(":", "")));
                return;
            }
            if(!int.TryParse(txtSizeYInPixels.Text, out result) && txtSizeYInPixels.Text != string.Empty)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1133).Replace("&1", lblSizeYInPixels.Text.Replace("*", "").Replace(":", "")));
                return;
            }
            if(cmbType.Text == "URLVIDEO" || cmbType.Text == "URLTV" || string.Equals(cmbType.Text, "WEBSITE", StringComparison.InvariantCultureIgnoreCase))
            {
                try
                {
                    if(cmbType.Text == "URLVIDEO")
                    {
                        if(txtLink.Text.Length > 0)
                        {
                            if((txtLink.Text.Trim()).Substring(0, 4) != "http")
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(1147), utilities.MessageUtils.getMessage(1148));
                                return;
                            }
                        }

                    }

                    if(mediaDTO == null)
                    {
                        mediaDTO = new MediaDTO();
                    }

                    mediaDTO.Name = txtName.Text;
                    mediaDTO.TypeId = (int)cmbType.SelectedValue;
                    mediaDTO.FileName = txtFileName.Text;
                    mediaDTO.IsActive = (chkActive.Checked) ? true : false;
                    mediaDTO.Description = rchDescription.Text;
                    if(!string.IsNullOrWhiteSpace(txtSizeXInPixels.Text))
                    {
                        mediaDTO.SizeXinPixels = Convert.ToInt32(txtSizeXInPixels.Text);
                    }
                    else
                    {
                        mediaDTO.SizeXinPixels = null;
                    }
                    if(!string.IsNullOrWhiteSpace(txtSizeYInPixels.Text))
                    {
                        mediaDTO.SizeYinPixels = Convert.ToInt32(txtSizeYInPixels.Text);
                    }
                    else
                    {
                        mediaDTO.SizeYinPixels = null;
                    }

                    Media media = new Media(machineUserContext, mediaDTO);
                    media.Save();
                }
                catch(ForeignKeyException)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                try
                {
                    try
                    {
                        //if(!File.Exists(Path.Combine(getSharedFolderPath(), Path.GetFileName(txtFileName.Text))))
                        if(fileChoosen)
                        {
                            fileChoosen = false;
                            lblStatusMessage.Visible = true;
                            lblStatusMessage.ForeColor = Color.Blue;
                            lblStatusMessage.Text = utilities.MessageUtils.getMessage(1149) + " " + Path.GetFileName(txtFileName.Text);

                            //  File.Copy(txtFileName.Text, Path.Combine(getSharedFolderPath(), Path.GetFileName(txtFileName.Text)), true);
                            FileSystem.CopyFile(txtFileName.Text, Path.Combine(getSharedFolderPath(), Path.GetFileName(txtFileName.Text)),UIOption.AllDialogs);
                            Application.DoEvents();

                            lblStatusMessage.Text = utilities.MessageUtils.getMessage(1150) + " " + Path.GetFileName(txtFileName.Text);
                            MessageBox.Show(utilities.MessageUtils.getMessage(1151));
                            lblStatusMessage.Text = "";
                            this.Close();
                        }
                    }
                    catch(IOException ex)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(988) + " " + Path.GetFileName(txtFileName.Text) + Environment.NewLine + ex.Message);
                        return;
                    }

                    if(mediaDTO == null)
                    {
                        mediaDTO = new MediaDTO();
                    }

                    mediaDTO.Name = txtName.Text;
                    mediaDTO.TypeId = (int)cmbType.SelectedValue;
                    mediaDTO.FileName = Path.GetFileName(txtFileName.Text);
                    mediaDTO.IsActive = (chkActive.Checked) ? true : false;
                    mediaDTO.Description = rchDescription.Text;
                    if(!string.IsNullOrWhiteSpace(txtSizeXInPixels.Text))
                    {
                        mediaDTO.SizeXinPixels = Convert.ToInt32(txtSizeXInPixels.Text);
                    }
                    else
                    {
                        mediaDTO.SizeXinPixels = null;
                    }
                    if(!string.IsNullOrWhiteSpace(txtSizeYInPixels.Text))
                    {
                        mediaDTO.SizeYinPixels = Convert.ToInt32(txtSizeYInPixels.Text);
                    }
                    else
                    {
                        mediaDTO.SizeYinPixels = null;
                    }
                    Media media = new Media(machineUserContext,mediaDTO);
                    media.Save();
                }
                catch(ForeignKeyException)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            /*btnBrowse.Visible = true;
            txtLink.Visible = false;
            lblLink.Visible = false;*/
            log.LogMethodExit();
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Refresh();
            log.LogMethodExit();

        }

        private void MediaListDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtName.MaxLength = 50;
            txtFileName.MaxLength = 100;
            rchDescription.MaxLength = 100;
            txtLink.MaxLength = 100;
            LoadMediaType();
            txtName.Select();
            if(cmbType.Text == "URLVIDEO" || cmbType.Text == "URLTV" || string.Equals(cmbType.Text, "WEBSITE", StringComparison.InvariantCultureIgnoreCase))
            {
                btnBrowse.Visible = false;
                lblPreviewVideo.Visible = true;
            }
            else
            {
                txtLink.Visible = false;
                lblPreviewVideo.Visible = false;
            }
            if(mediaID != -1)
                setupMediaFields(mediaID);


            lblStatusMessage.Visible = false;
            setPrevNext();
            log.LogMethodExit();
        }
        private void setupMediaFields(long mediaID)
        {
            log.LogMethodEntry(mediaID);
            cmbType.Enabled = false;
            picPreview.Visible = true;

            List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>> mediaSearchParams = new List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>>();
            mediaSearchParams.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.MEDIA_ID, mediaID.ToString()));
            mediaSearchParams.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<MediaDTO> mediaListOnDisplay = mediaList.GetAllMedias(mediaSearchParams);

            if(mediaListOnDisplay.Count > 0)
            {
                mediaDTO = new MediaDTO();
                mediaDTO = mediaListOnDisplay[0];
                txtName.Text = mediaListOnDisplay[0].Name;
                txtFileName.Text = mediaListOnDisplay[0].FileName;
                rchDescription.Text = mediaListOnDisplay[0].Description;
                cmbType.SelectedValue = mediaListOnDisplay[0].TypeId;
                if(cmbType.Text == "URLVIDEO" || cmbType.Text == "URLTV" || string.Equals(cmbType.Text, "WEBSITE", StringComparison.InvariantCultureIgnoreCase))
                {
                    // finfo = (dt.Rows[0]["File_Name"].ToString());
                    txtLink.Text = (mediaListOnDisplay[0].FileName);
                    lblPreviewVideo.Visible = true;
                    grpPreview.Visible = false;
                    picPreview.Visible = false;
                }
                else
                {
                    lblPreviewVideo.Visible = false;
                    finfo = new FileInfo(Path.Combine(getSharedFolderPath(), mediaListOnDisplay[0].FileName));
                }
                txtID.Text = mediaListOnDisplay[0].MediaId.ToString();
                if(mediaListOnDisplay[0].SizeXinPixels != null)
                {
                    txtSizeXInPixels.Text = mediaListOnDisplay[0].SizeXinPixels.ToString();
                }
                if(mediaListOnDisplay[0].SizeYinPixels != null)
                {
                    txtSizeYInPixels.Text = mediaListOnDisplay[0].SizeYinPixels.ToString();
                }
                if(mediaListOnDisplay[0].IsActive)
                    chkActive.Checked = true;
                else
                    chkActive.Checked = false;

                if(cmbType.Text != "URLVIDEO" || cmbType.Text != "URLTV" || string.Equals(cmbType.Text, "WEBSITE", StringComparison.InvariantCultureIgnoreCase) == false)
                {
                    try
                    {

                    }
                    catch
                    {
                        picPreview.Image = null;

                    }
                    if(checkVideo(mediaListOnDisplay[0].FileName))
                    {
                        try
                        {
                            picPreview.Visible = false;
                        }
                        catch(Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            OpenFileDialog fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.InitialDirectory = @"C:\";
            picPreview.Visible = true;
            switch(cmbType.Text)
            {
                case "IMAGE":
                    fd.Filter = "Image File|*.png;*.bmp;*.gif;*.jpg;*.jpeg;*.tif;*.tiff;*.PSD";
                    break;
                case "AUDIO":
                    fd.Filter = "Audio File|*.mp3;*.wma;*.wav;*.mid;*.ogg;*.ra;*.ram;*.rm";
                    break;
                case "VIDEO":
                    fd.Filter = "Video File|*.wmv;*.mov;*.avi;*.divx;*.mp4;*.mpg;*.mpeg;*.flv;*.mkv";
                    break;
                case "RSS Feed":
                    fd.Filter = "RSS File|*.RSS;*.rss";
                    break;
                default:
                    break;
            }
            if(fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFileName.Text = Path.GetFullPath(fd.FileName);
                fileChoosen = true;
                finfo = new FileInfo(Path.GetFullPath(fd.FileName));
                try
                {
                    if(picPreview != null && picPreview.Image != null)
                    {
                        picPreview.Image.Dispose();
                    }
                }
                catch
                {
                    grpPreview.Visible = false;
                    picPreview.Visible = false;
                }
            }
            log.LogMethodExit();
        }

        private void picPreview_MouseHover(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ToolTip tt = new ToolTip();
            if(finfo != null && finfo.Exists != false)
                tt.SetToolTip(this.picPreview, "Creation Time: " + finfo.CreationTime + Environment.NewLine + "File Size: " + finfo.Length / 1024 + " KB");
            log.LogMethodExit();
        }

        private string getSharedFolderPath()
        {
            log.LogMethodEntry();
            string sharedFolderPath = string.Empty;
            sharedFolderPath = utilities.getParafaitDefaults("UPLOAD_DIRECTORY");
            log.LogMethodExit(sharedFolderPath);
            return sharedFolderPath;
        }
        private bool checkFileExistsInDB(string filename)
        {
            log.LogMethodEntry(filename);
            MediaList mediaList = new MediaList(machineUserContext);
            List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>> mediaSearchParams = new List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>>();
            mediaSearchParams.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.NAME, filename));
            mediaSearchParams.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<MediaDTO> mediaListOnDisplay = mediaList.GetAllMedias(mediaSearchParams);

            if(mediaListOnDisplay != null && mediaListOnDisplay.Count > 0)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        private bool checkVideo(string videoname)
        {
            log.LogMethodEntry(videoname);
            string extension = Path.GetExtension(videoname);
            if(extension.Contains("wmv") || extension.Contains("mov") || extension.Contains("avi") || extension.Contains("divx") || extension.Contains("mp4")
                || extension.Contains("mpg") || extension.Contains("mpeg") || extension.Contains("flv") || extension.Contains("mkv"))
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        private void lnkPrevious_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            BrowseDisplay("P");
            log.LogMethodExit();
        }

        private void lnkNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            BrowseDisplay("N");
            log.LogMethodExit();
        }
        private void BrowseDisplay(string value)
        {
            log.LogMethodEntry(value);
            Form f = Application.OpenForms["frmMediaListUI"];
            DataGridView dgv = f.Controls.Find("dgvMediaList", true)[0] as DataGridView;
            int index = dgv.CurrentRow.Index;
            if(value == "P")
                if(index == 0)
                {
                    log.LogMethodExit();
                    return;
                }
                else
                    index--;
            else
                if(index == dgv.Rows.Count - 1)
            {
                log.LogMethodExit();
                return;
            }
            else
                index++;
            dgv.CurrentCell = dgv["fileNameDataGridViewTextBoxColumn", index];
            mediaID = Convert.ToInt64(dgv["mediaIdDataGridViewTextBoxColumn", index].Value);
            setupMediaFields(mediaID);
            if(cmbType.Text == "URLVIDEO" || cmbType.Text == "URLTV" || string.Equals(cmbType.Text, "WEBSITE", StringComparison.InvariantCultureIgnoreCase))
            {
                btnBrowse.Visible = false;
            }
            else
            {
                txtLink.Visible = false;
                btnBrowse.Visible = true;
                lblLink.Visible = false;
            }
            log.LogMethodExit();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            switch(cmbType.Text.ToUpper())
            {
                case "URLVIDEO":
                    lblLink.Visible = true;
                    btnBrowse.Visible = false;
                    txtLink.Visible = true;
                    lblPreviewVideo.Visible = true;
                    break;
                case "URLTV":
                    lblLink.Visible = true;
                    btnBrowse.Visible = false;
                    txtLink.Visible = true;
                    lblPreviewVideo.Visible = true;
                    break;
                case "WEBSITE":
                    lblLink.Visible = true;
                    btnBrowse.Visible = false;
                    txtLink.Visible = true;
                    lblPreviewVideo.Visible = true;
                    break;
                default:
                    btnBrowse.Visible = true;
                    lblLink.Visible = false;
                    txtLink.Visible = false;
                    lblPreviewVideo.Visible = false;
                    break;
            }
            log.LogMethodExit();
        }

        private void txtLink_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtFileName.Text = txtLink.Text;
            log.LogMethodExit();
        }

        private void lblPreviewVideo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            if(cmbType.Text == "URLVIDEO")
            {
                string videoURl = string.Empty;
                try
                {
                    videoURl = txtFileName.Text;
                    if(videoURl == "")
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1152));
                        log.LogMethodExit();
                        return;
                    }
                    frmURLVideo frmURLvideo = new frmURLVideo(videoURl);
                    frmURLvideo.ShowDialog();
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit();
        }


        private void performFileCopyOperation()
        {
            log.LogMethodEntry();
            try
            {
                if(fileChoosen)
                {
                    FileSystem.CopyFile(txtFileName.Text, Path.Combine(getSharedFolderPath(), Path.GetFileName(txtFileName.Text)), UIOption.AllDialogs);
                    fileChoosen = false;
                    lblStatusMessage.Visible = true;
                    lblStatusMessage.ForeColor = Color.Red;
                    lblStatusMessage.Text = utilities.MessageUtils.getMessage(1149) + " " + Path.GetFileName(txtFileName.Text);


                    File.Copy(txtFileName.Text, Path.Combine(getSharedFolderPath(), Path.GetFileName(txtFileName.Text)));

                    Application.DoEvents();

                    lblStatusMessage.Text = utilities.MessageUtils.getMessage(1150) + " " + Path.GetFileName(txtFileName.Text);
                }
            }
            catch(IOException ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(988) + " " + Path.GetFileName(txtFileName.Text) + Environment.NewLine + ex.Message);
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                btnAdd.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - SetUp.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80         4-Sep-2019       Deeksha        Added logger methods.
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class SetUp : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public SetUp()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void SetUp_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            loadConnectionInfo("Parafait Kiosk");
            loadAppInfo("Parafait Kiosk");
            Cursor.Show();
            log.LogMethodExit();
        }

        private void loadConnectionInfo(string fileName)
        {
            log.LogMethodEntry(fileName);
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = fileName + ".exe.config";  // relative path names possible

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            ConnectionStringsSection conSection = config.ConnectionStrings;
            conSection.SectionInformation.ForceSave = true;

            ConnectionStringSettingsCollection conStrings = conSection.ConnectionStrings;
            foreach (ConnectionStringSettings conSetting in conStrings)
            {
                if (!conSetting.Name.Contains("Parafait"))
                    continue;

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conSetting.ConnectionString);
                if (builder.Password.ToLower().Equals("parafait"))
                    conSetting.ConnectionString = StaticUtils.EncryptConnectionString(conSetting.ConnectionString);

                builder = new SqlConnectionStringBuilder(StaticUtils.getParafaitConnectionString(conSetting.ConnectionString));

                int pos1 = builder.DataSource.IndexOf('\\');
                if (pos1 == -1)
                {
                    txtDBMachine.Text = builder.DataSource;
                    txtDBInstance.Text = "";
                }
                else
                {
                    txtDBMachine.Text = builder.DataSource.Substring(0, pos1);
                    txtDBInstance.Text = builder.DataSource.Substring(pos1).TrimStart('\\');
                }

                txtUserId.Text = builder.UserID;
                txtPassword.Text = builder.Password;

                break;
            }
            log.LogMethodExit();
        }

        string getDataSource()
        {
            log.LogMethodEntry();
            string returnValue = txtDBMachine.Text + (string.IsNullOrEmpty(txtDBInstance.Text.Trim()) ? "" : "\\" + txtDBInstance.Text.Trim());
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private void saveConnectionInfo(string fileName)
        {
            log.LogMethodEntry(fileName);
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = fileName + ".exe.config";  // relative path names possible

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            ConnectionStringsSection conSection = config.ConnectionStrings;
            conSection.SectionInformation.ForceSave = true;

            ConnectionStringSettingsCollection conStrings = conSection.ConnectionStrings;
            foreach (ConnectionStringSettings conSetting in conStrings)
            {
                if (!conSetting.Name.Contains("Parafait"))
                    continue;

                string conString = conSetting.ConnectionString;
                conString = StaticUtils.getParafaitConnectionString(conString);

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conString);
                builder.DataSource = getDataSource();
                builder.Password = txtPassword.Text;

                conSetting.ConnectionString = StaticUtils.EncryptConnectionString(builder.ToString());

                config.Save(ConfigurationSaveMode.Modified);
            }
            log.LogMethodExit();
        }

        private void loadAppInfo(string fileName)
        {
            log.LogMethodEntry(fileName);
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = fileName + ".exe.config";  // relative path names possible

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            ConfigurationSectionGroup mgroup = config.SectionGroups["userSettings"];  // Loop over all groups
            if (mgroup == null)
            {
                log.LogMethodExit();
                return;
            }

            ClientSettingsSection clientSection;

            ConfigurationSection section = mgroup.Sections[fileName.Replace(' ', '_') + ".Properties.Settings"];
            if (section == null)
            {
                log.LogMethodExit();
                return;
            }

            clientSection = section as ClientSettingsSection;

            int elementCount = 0;
            foreach (SettingElement set in clientSection.Settings)
            {
                elementCount++;
                TextBox lbl = new TextBox();
                lbl.ReadOnly = true;
                lbl.BackColor = this.BackColor;
                lbl.BorderStyle = BorderStyle.None;
                lbl.Text = set.Name + ": ";
                lbl.AutoSize = false;
                lbl.Width = flpLabels1.Width - 8;
                lbl.TextAlign = HorizontalAlignment.Right;
                lbl.Height = 23;
                lbl.Font = txtDBMachine.Font;

                Control txt;
                if (set.Value.ValueXml.InnerText.ToLower().Equals("true") || set.Value.ValueXml.InnerText.ToLower().Equals("false"))
                {
                    txt = new CheckBox();
                    CheckBox chk = txt as CheckBox;
                    txt.Text = "";
                    txt.Name = set.Name;
                    txt.Width = flpValues1.Width - 5;
                    txt.Height = 23;
                    chk.AutoSize = false;
                    chk.Font = label1.Font;
                    chk.Checked = set.Value.ValueXml.InnerText.ToLower().Equals("true");
                }
                else
                {
                    txt = new TextBox();
                    txt.Font = txtDBMachine.Font;
                    txt.Name = set.Name;
                    txt.Text = set.Value.ValueXml.InnerText;
                    txt.Width = txtDBMachine.Width;
                    (txt as TextBox).AutoSize = false;
                    txt.Height = 23;
                }

                if (elementCount <= 20)
                {
                    flpLabels1.Controls.Add(lbl);
                    flpValues1.Controls.Add(txt);
                }
                else
                {
                    flpLabels2.Controls.Add(lbl);
                    flpValues2.Controls.Add(txt);
                }
            }
            log.LogMethodExit();
        }

        private void saveAppInfo(string fileName)
        {
            log.LogMethodEntry(fileName);
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = fileName + ".exe.config";  // relative path names possible

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            ConfigurationSectionGroup mgroup = config.SectionGroups["userSettings"];  // Loop over all groups
            if (mgroup == null)
            {
                log.LogMethodExit();
                return;
            }
            ClientSettingsSection clientSection;

            ConfigurationSection section = mgroup.Sections[fileName.Replace(' ', '_') + ".Properties.Settings"];
            if (section == null)
            {
                log.LogMethodExit();
                return;
            }

            clientSection = section as ClientSettingsSection;

            foreach (Control c in flpValues1.Controls)
            {
                foreach (SettingElement set in clientSection.Settings)
                {
                    if (c.Name == set.Name)
                    {
                        if (c.GetType().ToString().ToLower().Contains("textbox"))
                            set.Value.ValueXml.InnerText = c.Text;
                        else
                            set.Value.ValueXml.InnerText = (c as CheckBox).Checked ? "True" : "False";
                        break;
                    }
                }
            }

            foreach (Control c in flpValues2.Controls)
            {
                foreach (SettingElement set in clientSection.Settings)
                {
                    if (c.Name == set.Name)
                    {
                        if (c.GetType().ToString().ToLower().Contains("textbox"))
                            set.Value.ValueXml.InnerText = c.Text;
                        else
                            set.Value.ValueXml.InnerText = (c as CheckBox).Checked ? "True" : "False";
                        break;
                    }
                }
            }

            section.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            saveConnectionInfo("Parafait Kiosk");
            saveAppInfo("Parafait Kiosk");
            log.LogMethodExit();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ConfigurationManager.RefreshSection("connectionStrings");
            object o = ConfigurationManager.GetSection("connectionStrings");
            ConnectionStringsSection cons = o as ConnectionStringsSection;

            string conString = cons.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(StaticUtils.getParafaitConnectionString(conString));

            try
            {
                this.Cursor = Cursors.WaitCursor;
                conn.Open();
                conn.Close();
                MessageBox.Show("DB Connection successful");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Parafait Connection Test");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void SetUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            Cursor.Hide();
            log.LogMethodExit();
        }
    }
}

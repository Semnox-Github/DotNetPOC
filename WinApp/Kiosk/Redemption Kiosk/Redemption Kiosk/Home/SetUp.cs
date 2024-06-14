/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Setup UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Redemption_Kiosk
{
    public partial class SetUp : Form
    {
        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SetUp()
        {
            log.LogMethodEntry();
            InitializeComponent();
            flpLabels1.BackColor = flpLabels2.BackColor =
                flpValues1.BackColor = flpValues2.BackColor = Redemption_Kiosk.Common.PrimaryForeColor;
            log.LogMethodExit();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void SetUp_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                LoadConnectionInfo("Redemption Kiosk");
                LoadAppInfo("Redemption Kiosk");
                Cursor.Show();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadConnectionInfo(string fileName)
        {
            log.LogMethodEntry(fileName);
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = fileName + ".exe.config"  // relative path names possible
            };

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            ConnectionStringsSection conSection = config.ConnectionStrings;
            conSection.SectionInformation.ForceSave = true;

            ConnectionStringSettingsCollection conStrings = conSection.ConnectionStrings;
            foreach (ConnectionStringSettings conSetting in conStrings)
            {
                if (!conSetting.Name.Contains("Parafait"))
                {
                    continue;
                }

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

        string GetDataSource()
        {
            log.LogMethodEntry();
            return txtDBMachine.Text + (string.IsNullOrEmpty(txtDBInstance.Text.Trim()) ? "" : "\\" + txtDBInstance.Text.Trim());
        }

        private void SaveConnectionInfo(string fileName)
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
                builder.DataSource = GetDataSource();
                builder.Password = txtPassword.Text;

                conSetting.ConnectionString = StaticUtils.EncryptConnectionString(builder.ToString());

                config.Save(ConfigurationSaveMode.Modified);
            }
            log.LogMethodExit();
        }

        private void LoadAppInfo(string fileName)
        {
            log.LogMethodEntry(fileName);
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
                                                {
                                                    ExeConfigFilename = fileName + ".exe.config"  // relative path names possible
                                                };

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
                lbl.BackColor = flpLabels1.BackColor;
                lbl.ForeColor = Color.White;
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

        private void SaveAppInfo(string fileName)
        {
            log.LogMethodEntry(fileName);
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
                                                {
                                                    ExeConfigFilename = fileName + ".exe.config"  // relative path names possible
                                                };

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
                        {
                            set.Value.ValueXml.InnerText = c.Text;
                        }
                        else
                        {
                            set.Value.ValueXml.InnerText = (c as CheckBox).Checked ? "True" : "False";
                        }

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
                        {
                            set.Value.ValueXml.InnerText = c.Text;
                        }
                        else
                        {
                            set.Value.ValueXml.InnerText = (c as CheckBox).Checked ? "True" : "False";
                        }

                        break;
                    }
                }
            }

            section.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);
            log.LogMethodExit();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SaveConnectionInfo("Redemption Kiosk");
            SaveAppInfo("Redemption Kiosk");
            log.LogMethodExit();
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
                                                {
                                                    ExeConfigFilename = "Redemption Kiosk.exe.config"  // relative path names possible
                                                };

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            ConnectionStringsSection cons = config.ConnectionStrings;

            string conString = cons.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ConnectionString;
            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(StaticUtils.getParafaitConnectionString(conString));

            try
            {
                this.Cursor = Cursors.WaitCursor;
                conn.Open();
                conn.Close();
                Common.ShowMessage(Common.utils.MessageUtils.getMessage(1617));
                //"DB Connection successful" 
            }
            catch (Exception ex)
            {
                // Parafait_FnB_Kiosk.Common.logException(ex);
                log.Error(ex);
                Common.ShowMessage(ex.Message);
                //MessageBox.Show(ex.Message, "Parafait Connection Test");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void SetUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor.Hide();
            log.LogMethodExit();
        }
    }
}

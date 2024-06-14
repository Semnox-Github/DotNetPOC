using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Semnox.Core.GenericUtilities
{
    public static class VersionSynch
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static string serverFolder = getServerInstallerFolder();
        static string localInstallerDir = getLocalInstallerFolder();
        static string patchFile = "";
        static string installerExe = "";
        public static bool newVersionAvailable()
        {
            log.LogMethodEntry();
            try
            {
                if (localInstallerDir == "")
                {
                    log.LogMethodExit(false);
                    return false;
                }
                   

                patchFile = serverFolder + "ParafaitPatch.zip";
                installerExe = serverFolder + "ParafaitSetup.exe";
                
                FileInfo PatchFileInfo = new FileInfo(patchFile);

                FileInfo f = new FileInfo(localInstallerDir + "ParafaitPatch.zip");
                if (f.LastWriteTime < PatchFileInfo.LastWriteTime)
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
            catch(Exception ex)
            {
                log.Error("Error occured in new version available", ex);
                log.LogMethodExit(false);
                return false;
            }
           
        }

        public static string getServerInstallerFolder()
        {
            log.LogMethodEntry();
            try
            {
                string conString = Properties.Settings.Default.ParafaitConnectionString;
                int equalIndex = conString.IndexOf('=', conString.IndexOf("Data Source", StringComparison.CurrentCultureIgnoreCase));
                int slashIndex = conString.IndexOf('\\', equalIndex); // no instance name
                if (slashIndex == 0)
                    slashIndex = conString.IndexOf(';', equalIndex);
                string serverMachine = conString.Substring(equalIndex + 1, slashIndex - equalIndex);
                return "\\\\" + serverMachine + "\\Parafait Installer\\";
            }
            catch(Exception ex)
            {
                log.Error("Error occured in getserverInstallerFolder", ex);
                log.LogMethodExit("");
                return "";
            }
           
        }

        public static string getLocalInstallerFolder()
        {
            log.LogMethodEntry();
            DriveInfo[] drives = System.IO.DriveInfo.GetDrives();

            foreach (DriveInfo dr in drives)
            {
                if (dr.DriveType != DriveType.Fixed)
                    continue;
                string installerDir = dr + "Parafait Installer\\";
                if (Directory.Exists(installerDir))
                {
                    log.LogMethodExit(installerDir);
                    return installerDir;
                }
            }
            log.LogMethodExit("");
            return "";
        }

        static void copyFiles()
        {
            log.LogMethodEntry();
            File.Copy(patchFile, localInstallerDir + "ParafaitPatch.zip", true);
            File.Copy(installerExe, localInstallerDir + "ParafaitSetup.exe", true);
            log.LogMethodExit(null);
        }

        public static void SynchVersions()
        {
            log.LogMethodEntry();
            if (VersionSynch.newVersionAvailable())
            {
                try
                {
                    System.Windows.Forms.Form f = new Form();
                    f.Size = new Size(500, 190);
                    f.BackColor = System.Drawing.Color.DarkKhaki;

                    System.Windows.Forms.Label label1;
                    System.Windows.Forms.Button btnYes;
                    System.Windows.Forms.Label label2;
                    System.Windows.Forms.Button btnNo;

                    label1 = new System.Windows.Forms.Label();
                    btnYes = new System.Windows.Forms.Button();
                    label2 = new System.Windows.Forms.Label();
                    btnNo = new System.Windows.Forms.Button();

                    f.Controls.Add(btnNo);
                    f.Controls.Add(label2);
                    f.Controls.Add(btnYes);
                    f.Controls.Add(label1);
                    f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    f.Name = "VersionSynch";
                    f.StartPosition = FormStartPosition.Manual;
                    f.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2 - f.Width / 2, 60);
                    f.Text = "Parafait Version Synch";

                    // 
                    // label1
                    // 
                    label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                | System.Windows.Forms.AnchorStyles.Right)));
                    label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    label1.ForeColor = System.Drawing.Color.DarkBlue;
                    label1.Location = new System.Drawing.Point(12, 48);
                    label1.Name = "label1";
                    label1.Size = new System.Drawing.Size(489, 32);
                    label1.TabIndex = 0;
                    label1.Text = "New version of Parafait Software found on server. ";
                    label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                    // 
                    // btnYes
                    // 
                    btnYes.Location = new System.Drawing.Point(159, 155);
                    btnYes.Name = "btnYes";
                    btnYes.Size = new System.Drawing.Size(75, 23);
                    btnYes.TabIndex = 1;
                    btnYes.Text = "OK";
                    btnYes.UseVisualStyleBackColor = true;
                    btnYes.Click += new EventHandler(btnYes_Click);

                    // 
                    // label2
                    // 
                    label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                | System.Windows.Forms.AnchorStyles.Right)));
                    label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    label2.ForeColor = System.Drawing.Color.DarkBlue;
                    label2.Location = new System.Drawing.Point(12, 101);
                    label2.Name = "label2";
                    label2.Size = new System.Drawing.Size(489, 32);
                    label2.TabIndex = 3;
                    label2.Text = "Click OK to upgrade Parafait software on this computer (Recommended).";
                    label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
                    // 
                    // btnNo
                    // 
                    btnNo.Location = new System.Drawing.Point(280, 155);
                    btnNo.Name = "btnNo";
                    btnNo.Size = new System.Drawing.Size(75, 23);
                    btnNo.TabIndex = 4;
                    btnNo.Text = "Cancel";
                    btnNo.UseVisualStyleBackColor = true;
                    f.CancelButton = btnNo;
                    f.TopMost = true;
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        copyFiles();
                        System.Diagnostics.Process.Start("ParafaitVersionSynch.exe", Environment.CommandLine);
                        Environment.Exit(0);
                    }
                }
                catch(Exception ex)
                {
                    log.Error("Error occured in synch versions", ex);
                }
            }
            log.LogMethodExit(null);
        }

        static void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Form f = ((Form)((Button)sender).Parent);
            f.DialogResult = DialogResult.OK;
            f.Close();
            log.LogMethodExit(null);
        }
    }
}

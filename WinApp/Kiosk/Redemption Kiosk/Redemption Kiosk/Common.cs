/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Common cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 *2.4.0       11-Sep-2018      Archana              Modified for customer registration changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Redemption_Kiosk
{
    public static class Common
    {
        internal static Utilities utils;
        internal static ParafaitEnv parafaitEnv;

        internal static System.Drawing.Color PrimaryForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(47)))), ((int)(((byte)(140)))));
        internal static System.Drawing.Color RedColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(31)))), ((int)(((byte)(54)))));
        internal static System.Drawing.Color GreenColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(189)))), ((int)(((byte)(34)))));

        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal static int TimeOutDuration = 60;

        internal static void OpenScreen(ScreenModel.UIPanelElement callingElement)
        {
            log.LogMethodEntry(callingElement);
            try
            {
                ScreenModel screen = new ScreenModel(callingElement.ActionScreenId);

                frmRedemptionKioskBaseForm f = (frmRedemptionKioskBaseForm)Assembly.GetExecutingAssembly().CreateInstance("Redemption_Kiosk." + screen.CodeObjectName);
                f.SetScreenModel(screen, callingElement);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMessage(ex.Message);
            }

            log.LogMethodExit();
        }

        internal static void InitEnv()
        {
            log.LogMethodEntry();
            try
            {
                utils = new Utilities();
                parafaitEnv = utils.ParafaitEnv;
                parafaitEnv.SetPOSMachine("", Environment.MachineName);
                UsersList usersListBL = new UsersList(utils.ExecutionContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters;
                searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.LOGIN_ID, "Redemption Kiosk User"));
                List<UsersDTO> usersListDTO = usersListBL.GetAllUsers(searchParameters);
                if (usersListDTO != null)
                {
                    utils.ParafaitEnv.User_Id = usersListDTO[0].UserId;
                    utils.ParafaitEnv.Username = usersListDTO[0].UserName;
                    utils.ParafaitEnv.LoginID = usersListDTO[0].LoginId;
                }
                else
                {
                    throw new Exception(utils.MessageUtils.getMessage(1610));
                    //Kiosk user is not created
                }

                parafaitEnv.Initialize();
                Semnox.Parafait.KioskCore.KioskStatic.Utilities = utils;
                int rdsScreenTimeout;
                try
                {
                    rdsScreenTimeout = Convert.ToInt32(utils.getParafaitDefaults("MONEY_SCREEN_TIMEOUT"));
                    TimeOutDuration = rdsScreenTimeout;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    TimeOutDuration = 60;
                } 
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
                Program.ShowTaskbar();
                Environment.Exit(0);
            }
            log.LogMethodExit();
        }

        internal static DialogResult ShowDialog(string message)
        {
            log.LogMethodEntry(message);
            log.Info(message);
            return (new frmRedemptionKioskDisplayMessage(message, true)).ShowDialog();
        }

        internal static DialogResult ShowMessage(string message)
        {
            log.LogMethodEntry(message);
            log.Info(message);
            return (new frmRedemptionKioskDisplayMessage(message)).ShowDialog();
        }
         

        internal static void GoHome()
        {
            log.LogMethodEntry();
            for (int openFrmCount = Application.OpenForms.Count - 1; openFrmCount > 0; openFrmCount--)
            {
                if(Application.OpenForms[openFrmCount].Name != "frmRedemptionKioskHomeScreen" 
                   && Application.OpenForms[openFrmCount].Name != "frmRedemptionKioskSplashScreen"
                  ) 
                {
                    Application.OpenForms[openFrmCount].Close();
                }
            }
            log.LogMethodExit();
        }

        internal static int GetScreenId(string objectCodeName)
        {

            log.LogMethodEntry(objectCodeName);
            int screenId = -1;
            object screenIdObj = Common.utils.executeScalar(@"select screenId 
                                                             from AppScreens
                                                            where CodeObjectName = @objectCodeName
                                                             and AppScreenProfileId = (select AppScreenProfileId 
                                                                                         from AppScreenProfile 
                                                                                        where AppScreenProfileName = 'Redemption Kiosk')",
                                                             new SqlParameter[] { new SqlParameter("@objectCodeName", objectCodeName)});
            if(screenIdObj != null)
            {
                try { screenId = Convert.ToInt32(screenIdObj); } catch (Exception ex) { log.Error(ex); }
            }
            log.LogMethodExit(screenId);
            return screenId;
        }
        internal delegate bool CustomValidateAction(ScreenModel.UIPanelElement element);
        static CustomValidateAction customValidateAction;

        internal delegate void CustomUpdateHeader();
        static CustomUpdateHeader customUpdateHeader;
        public static void RenderPanelContent(ScreenModel _screenModel, Control panel, int panelIndex)
        {
            log.LogMethodEntry(_screenModel, panel, panelIndex);
            ScreenModel.UIPanel modelPanel = _screenModel.GetPanelByIndex(panelIndex);
            if (modelPanel != null)
            {
                if (modelPanel.PanelWidth >= 0)
                {
                    panel.Width = modelPanel.PanelWidth;
                    panel.Left = (Screen.PrimaryScreen.WorkingArea.Width - panel.Width) / 2;
                    panel.Left = (1080 - panel.Width) / 2;
                }
                int index = 1;
                foreach (var pb in panel.Controls.OfType<PictureBox>())
                {
                    ScreenModel.UIPanelElement element = modelPanel.getElementByIndex(index);
                    panel.BackgroundImage = element.Attribute.DisplayImage;
                    panel.BackgroundImageLayout = ImageLayout.None;
                    panel.Height = element.Attribute.DisplayImage.Height;
                    (pb as PictureBox).Visible = false;
                    index++;
                    if (element.ActionScreenId != -1)
                    {
                        panel.Click += delegate
                        {
                            if (customValidateAction(element))
                            {
                                Common.OpenScreen(element.Clone());
                            }
                            customUpdateHeader();
                        };
                    }
                }
                foreach (Control c in panel.Controls)
                {
                    if (c is Panel || c is FlowLayoutPanel)
                    {
                        foreach (var pb in c.Controls.OfType<PictureBox>())
                        {
                            ScreenModel.UIPanelElement panelElement = modelPanel.getElementByIndex(index);
                            c.BackgroundImage = panelElement.Attribute.DisplayImage;
                            c.BackgroundImageLayout = ImageLayout.None;
                            c.Height = panelElement.Attribute.DisplayImage.Height - 2;
                            c.Width = panelElement.Attribute.DisplayImage.Width;
                            (pb as PictureBox).Visible = false;
                            index++;
                            break;
                        }
                    }
                    else if (c is Button)
                    {
                        ScreenModel.UIPanelElement element = modelPanel.getElementByIndex(index);
                        if (element != null)
                        {
                            if (element.Attribute.DisplayImage == null)
                            {
                                c.Text = element.Attribute.DisplayText;
                            }
                            else
                            {
                                Button b = c as Button;
                                b.Name = element.ElementName;
                                b.Image = element.Attribute.DisplayImage;
                                b.Size = b.Image.Size;
                                b.Height -= 2; // to remove the border line
                                b.Text = element.Attribute.DisplayText;
                                //b.Tag = element.Parameters[0].UserSelectedValue;
                            }
                            if (element.ActionScreenId != -1)
                            {
                                c.Click += delegate
                                {
                                    //if (this._callingElement == null || this._callingElement.UIPanelElementId != element.UIPanelElementId)
                                    {
                                        if (customValidateAction(element))
                                        {
                                            Common.OpenScreen(element.Clone());
                                        }
                                        customUpdateHeader();
                                    }
                                };
                            }
                        }
                        else
                        {
                            c.Visible = false;
                        }
                        index++;
                    }
                }
            }
            else
            {
                panel.Visible = false;
            }
            log.LogMethodExit();
        }
    }
}



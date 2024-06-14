/********************************************************************************************
 * Project Name - Achievement
 * Description  - AchievementDetailsPosUI
 * 
 **************
 **Version Log
 **************
 *Version       Date            Modified By             Remarks          
 *********************************************************************************************
 *2.90.0        29-May-2020     Dakashakh raj           Modified :Ability to handle multiple projects. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.Achievements;

using System.IO;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.Achievements
{
    /// <summary>
    /// AchievementDetailsPosUI
    /// </summary>
    public partial class AchievementDetailsPosUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// AchievementDetailsPosUI with parameters
        /// </summary>
        /// <param name="_Utilities"></param>
        /// <param name="cardNumber"></param>
        public AchievementDetailsPosUI(Utilities _Utilities,string cardNumber)
        {
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvAchievementClassLevelExtended);
            utilities.setupDataGridProperties(ref dgvScoreLog);
            utilities.setLanguage(this);
         
            grpCardDetails.Visible = false;
            txtStatus.Text = "";
            if(!string.IsNullOrEmpty(cardNumber))
            {
                txtCardNumber.Text = cardNumber;
                getCardDetails();
            }
            
        }

        private void btnGetCardDetails_Click(object sender, EventArgs e)
        {
            getCardDetails();
        }


        /// <summary>
        /// LoadCardDetails
        /// </summary>
        /// <param name="cardNumeber"></param>
        public void LoadCardDetails(string cardNumeber)
        {

            txtCardNumber.Text = cardNumeber;
            getCardDetails();

        }


        private void getCardDetails()
        {
            txtStatus.Text = "";
            try
            {
                if (string.IsNullOrEmpty(txtCardNumber.Text))
                {
                    grpCardDetails.Visible = false;
                    MessageBox.Show("Please enter Card number");
                    return;
                }
                else
                {
                    CardCoreBL cardCoreBL = new CardCoreBL(txtCardNumber.Text);
                    PlayerAchievementBL playerAchievementBL = new PlayerAchievementBL();
                    CardCoreDTO cardCoreDTO = cardCoreBL.GetCardCoreDTO;
                    if (cardCoreDTO.CardId > 0)
                    {
                        SetCustomerDetails(cardCoreDTO.Customer_id);
                        AchievementParams achievementParams = new AchievementParams()
                        {
                            CardId = cardCoreDTO.CardId
                        };
                        List<PlayerAchievement> PlayerAchievementList = playerAchievementBL.GetTopNAchievementPlayers(achievementParams);

                        grpCardDetails.Visible = true;
                        lblIssueDate.Text = cardCoreDTO.Issue_date.ToString();
                        if (cardCoreDTO.Customer_id <= 0)
                            lblRegister.Text = "Not Registered";
                        else
                            lblRegister.Text = "Registered";

                        if (PlayerAchievementList.Count > 0)
                        {
                            lblPoints.Text = PlayerAchievementList[0].Points.ToString(); ;

                            //lblName.Text = PlayerAchievementList[0].FirstName;
                            //lblEmail.Text = PlayerAchievementList[0].EmailId;
                            string directory = utilities.getParafaitDefaults("IMAGE_DIRECTORY");
                            string filePath = directory + "\\" + PlayerAchievementList[0].PhotoURL;

                            if (File.Exists(filePath))
                            {
                                this.ptbCustomerImage.SizeMode = PictureBoxSizeMode.Zoom;
                                ptbCustomerImage.Image = Image.FromFile(filePath);
                            }
                            //imgMedal
                            BindingSource bindingSource = new BindingSource();
                            bindingSource.DataSource = PlayerAchievementList[0].AchievementLevelList;
                            dgvAchievementClassLevelExtended.DataSource = bindingSource;
                        }
                        else
                        {
                            BindingSource bindingSource = new BindingSource();
                            bindingSource.DataSource = new List<AchievementLevelExtended>();
                            dgvAchievementClassLevelExtended.DataSource = bindingSource;
                        }
                        //lblScore.Text = cardCoreDTO.s;


                        //BindAchievementClassLevelExtendedGrid(cardCoreDTO.CardId);
                        BindScoreLog(cardCoreDTO.CardId);
                    }
                    else
                    {
                        ResetAll();
                        txtStatus.Text = "Invalid Card";
                      
                    }
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex.Message);
            }


        }

        private void SetCustomerDetails(int customerId)
        {
            CustomerBL customers = new CustomerBL(utilities.ExecutionContext, customerId);
            if (customers.CustomerDTO.Id > 0)
            {
                lblName.Text = customers.CustomerDTO.FirstName + " " + customers.CustomerDTO.LastName;

                string directory = utilities.getParafaitDefaults("IMAGE_DIRECTORY");
                string filePath = directory + "\\" + customers.CustomerDTO.PhotoURL;

                if (File.Exists(filePath))
                {
                    ptbCustomerImage.Image = Image.FromFile(filePath);
                }
            }

        }


        private void ResetAll()
        {
            BindScoreLog(-1);
            txtCardNumber.Text = "";
            lblIssueDate.Text = "";
            lblPoints.Text = "";
            lblRegister.Text = "";
            lblName.Text = "";
            lblEmail.Text = "";
            txtStatus.Text = "";
            grpCardDetails.Visible = false;


         
        }

    

        private void BindScoreLog(int cardId)
        {
            AchievementScoreLogExtendedList achievementScoreLogExtendedList = new AchievementScoreLogExtendedList();


            List<AchievementScoreLogExtendedDTO> achievementScoreLogExtended = achievementScoreLogExtendedList.GetAchievementScoreLogExtendedList(cardId);


            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = achievementScoreLogExtended;
            dgvScoreLog.DataSource = bindingSource;

        }

        private void dgvAchievementClassLevelExtended_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //
            string directory = utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            foreach (DataGridViewRow row in dgvAchievementClassLevelExtended.Rows)
            {
                if (row.Cells["Picture"].Value != null && !string.IsNullOrEmpty(row.Cells["Picture"].Value.ToString()))
                {
                    string filePath = directory+"\\" + row.Cells["Picture"].Value.ToString();  

                    if (File.Exists(filePath))
                    {
                        Image image = Image.FromFile(filePath);
                        row.Cells["imgMedal"].Value = image;
                    }
                }


            }
        }

        
        private void btnKeypad_Click(object sender, EventArgs e)
        {
            CurrentAlphanumericTextBox = txtCardNumber;
            showAlphaNumberPadForm('-');
        }


        TextBox CurrentAlphanumericTextBox;
        AlphaNumericKeyPad keypad;
        void showAlphaNumberPadForm(char firstKey)
        {
            log.Debug("Starts-showAlphaNumberPadForm()");//Added for logger function on 08-Mar-2016
            if (CurrentAlphanumericTextBox != null)
            {
                if (keypad == null || keypad.IsDisposed)
                {
                    keypad = new AlphaNumericKeyPad(this, CurrentAlphanumericTextBox);
                    if (this.PointToScreen(CurrentAlphanumericTextBox.Location).Y + 60 + keypad.Height < Screen.PrimaryScreen.WorkingArea.Height)
                        keypad.Location = new Point(this.Location.X, this.PointToScreen(CurrentAlphanumericTextBox.Location).Y + 60);
                    else
                        keypad.Location = new Point(this.Location.X, this.PointToScreen(CurrentAlphanumericTextBox.Location).Y - keypad.Height);
                    keypad.Show();
                }
                else if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    keypad.Show();
                }
            }
            log.Debug("Ends-showAlphaNumberPadForm()");//Added for logger function on 08-Mar-2016
        }


        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ResetAll();
        }
    }
}

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// Add to account UI Class
    /// </summary>
    public partial class AddToAccountUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private decimal? credits;
        private decimal? courtesy;
        private decimal? bonus;
        private decimal? time;
        private int? techGames;
        private int? ticketCount;
        private List<AccountGameDTO> accountGameDTOList;

        

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="technicianCard"></param>
        public AddToAccountUI(Utilities utilities, bool technicianCard)
        {
            log.LogMethodEntry(utilities, technicianCard);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setLanguage(this);
            
            if (!technicianCard)
                textBoxTechGames.Enabled = false;
            else
            {
                txtCourtesy.Enabled = txtCredits.Enabled = txtBonus.Enabled = txtTime.Enabled = false;
            }

            if (utilities.ParafaitEnv.LoginID != "semnox" && utilities.getParafaitDefaults("ALLOW_MANUAL_CARD_UPDATE") == "N")
            {
                txtCourtesy.Enabled = txtCredits.Enabled = txtBonus.Enabled = txtTime.Enabled = false;
                grpEntt.Enabled = false;
            }
            ThemeUtils.SetupVisuals(this);
            log.LogMethodExit();
        }

        private void AddToAccount_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            GameList gameList = new GameList();
            List<GameDTO> gameDTOList;
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            gameDTOList = gameList.GetGameList(searchParameters);
            if (gameDTOList == null)
            {
                gameDTOList = new List<GameDTO>();
            }

            gameDTOList.Insert(0, new GameDTO());
            gameDTOList[0].GameId = -1;
            gameDTOList[0].GameName = "-All-";

            cmbGames.DisplayMember = "GameName";
            cmbGames.ValueMember = "GameId";
            cmbGames.DataSource = gameDTOList;

            
            List<GameProfileDTO> gameProfileDTOList;
            List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>> searchGameProfileParameters = new List<KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>>();
            searchGameProfileParameters.Add(new KeyValuePair<GameProfileDTO.SearchByGameProfileParameters, string>(GameProfileDTO.SearchByGameProfileParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            GameProfileList gameProfileList = new GameProfileList(utilities.ExecutionContext, searchGameProfileParameters);
            gameProfileDTOList = gameProfileList.GetGameProfileList;
            if (gameProfileDTOList == null)
            {
                gameProfileDTOList = new List<GameProfileDTO>();
            }

            gameProfileDTOList.Insert(0, new GameProfileDTO());
            gameProfileDTOList[0].GameProfileId = -1;
            gameProfileDTOList[0].ProfileName = "-All-";

            cmbGameProfile.DisplayMember = "ProfileName";
            cmbGameProfile.ValueMember = "GameProfileId";
            cmbGameProfile.DataSource = gameProfileDTOList;

            cmbFrequency.DisplayMember = "Value";
            cmbFrequency.ValueMember = "Key";
            cmbFrequency.DataSource = GetAccountGameFrequencyDataSource();
            cmbFrequency.SelectedValue = "N";

            LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "CARD_GAMES_ENTITLEMENT_TYPES"));
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> accountGameEntitlementTypeList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (accountGameEntitlementTypeList == null)
            {
                accountGameEntitlementTypeList = new List<LookupValuesDTO>();
            }
            accountGameEntitlementTypeList.Insert(0, new LookupValuesDTO());
            accountGameEntitlementTypeList[0].LookupValue = "";
            accountGameEntitlementTypeList[0].Description = "Default";
            BindingSource bs = new BindingSource();
            bs.DataSource = accountGameEntitlementTypeList;
            cmbEntType.DataSource = bs;
            cmbEntType.ValueMember = "LookupValue";
            cmbEntType.DisplayMember = "Description";
        }

        private List<KeyValuePair<string, string>> GetAccountGameFrequencyDataSource()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> cardGameFrequencyDataSource = new List<KeyValuePair<string, string>>();
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("N", MessageContainerList.GetMessage(utilities.ExecutionContext, "None")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("D", MessageContainerList.GetMessage(utilities.ExecutionContext, "Daily")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("W", MessageContainerList.GetMessage(utilities.ExecutionContext, "Weekly")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("M", MessageContainerList.GetMessage(utilities.ExecutionContext, "Monthly")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("Y", MessageContainerList.GetMessage(utilities.ExecutionContext, "Yearly")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("B", MessageContainerList.GetMessage(utilities.ExecutionContext, "Birthday")));
            cardGameFrequencyDataSource.Add(new KeyValuePair<string, string>("A", MessageContainerList.GetMessage(utilities.ExecutionContext, "Anniversary")));
            log.LogMethodExit(cardGameFrequencyDataSource);
            return cardGameFrequencyDataSource;
        }

        private void txtCredits_Validating(object sender, CancelEventArgs e)
        {
            if (!validateAndFormatDecimal((TextBox)sender))
                e.Cancel = true;
        }

        private void txtCourtesy_Validating(object sender, CancelEventArgs e)
        {
            if (!validateAndFormatDecimal((TextBox)sender))
                e.Cancel = true;
        }

        private void txtBonus_Validating(object sender, CancelEventArgs e)
        {
            if (!validateAndFormatDecimal((TextBox)sender))
                e.Cancel = true;
        }

        private void txtTime_Validating(object sender, CancelEventArgs e)
        {
            if (!validateAndFormatDecimal((TextBox)sender))
                e.Cancel = true;
        }

        private void textBoxTechGames_Validating(object sender, CancelEventArgs e)
        {
            if (!validateAndFormatInteger((TextBox)sender))
                e.Cancel = true;
        }

        private void textBoxTicketCount_Validating(object sender, CancelEventArgs e)
        {
            if (!validateAndFormatInteger((TextBox)sender))
                e.Cancel = true;
        }

        private bool validateAndFormatInteger(TextBox txt)
        {
            txt.Text = txt.Text.Trim();
            if (!string.IsNullOrEmpty(txt.Text))
            {
                int i;
                if(int.TryParse(txt.Text, out i))
                {
                    if (i < 0)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 647));
                        return false;
                    }
                    txt.Text = i.ToString(utilities.getNumberFormat());
                    return true;
                }
                else
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 646));
                    return false;
                }
            }
            return true;
        }

        private bool validateAndFormatDecimal(TextBox txt)
        {
            txt.Text = txt.Text.Trim();
            if (!string.IsNullOrEmpty(txt.Text))
            {
                if (!utilities.isNumber(txt.Text))
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext,646));
                    return false;
                }
                else
                {
                    if(Convert.ToDecimal(txt.Text) <0)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 647));
                        return false;
                    }
                    txt.Text = Math.Abs(Convert.ToDecimal(txt.Text)).ToString(utilities.getAmountFormat());
                    return true;
                }
            }
            return true;
        }

        private void cmbGameProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGameProfile.SelectedIndex > 0)
            {
                cmbGames.SelectedIndex = 0;
                cmbGames.Enabled = false;
                txtOptionalAttribute.Text = "";
                txtOptionalAttribute.Enabled = false;
            }
            else
            {
                cmbGames.Enabled = true;
                txtOptionalAttribute.Enabled = true;
            }
        }

        private void txtGameCount_Validating(object sender, CancelEventArgs e)
        {
            if (!validateAndFormatDecimal((TextBox)sender))
                e.Cancel = true;
        }

        private decimal? GetDecimalValue(string decimalValueString)
        {
            log.LogMethodEntry(decimalValueString);
            decimal? result = null;
            decimal d;
            if (decimal.TryParse(decimalValueString, out d))
            {
                result = d;
            }
            log.LogMethodExit(result);
            return result;
        }

        private int? GetIntegerValue(string integerValueString)
        {
            log.LogMethodEntry(integerValueString);
            int i;
            int? result = null;
            if (int.TryParse(integerValueString, out i))
            {
                result = i;
            }
            log.LogMethodExit(result);
            return result;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            credits = GetDecimalValue(txtCredits.Text);
            bonus = GetDecimalValue(txtBonus.Text);
            courtesy = GetDecimalValue(txtCourtesy.Text);
            time = GetDecimalValue(txtTime.Text);
            techGames = GetIntegerValue(textBoxTechGames.Text);
            ticketCount = GetIntegerValue(textBoxTicketCount.Text);
            int? repeatCount = GetIntegerValue(txtRepeat.Text);
            if (repeatCount.HasValue == false || repeatCount.Value <= 0)
            {
                repeatCount = 1;
            }
            decimal? entitlementValue = GetDecimalValue(txtGameCount.Text);
            if (entitlementValue.HasValue && entitlementValue.Value > 0)
            {
                accountGameDTOList = new List<AccountGameDTO>();
                for (int i = 0; i < repeatCount; i++)
                {
                    AccountGameDTO accountGameDTO = new AccountGameDTO();
                    accountGameDTO.GameId = Convert.ToInt32(cmbGames.SelectedValue);
                    accountGameDTO.GameProfileId = Convert.ToInt32(cmbGameProfile.SelectedValue);
                    decimal? optionalAttribute = GetDecimalValue(txtOptionalAttribute.Text);
                    if(optionalAttribute.HasValue)
                    {
                        accountGameDTO.OptionalAttribute = ((int)optionalAttribute.Value).ToString();
                    }
                    else
                    {
                        accountGameDTO.OptionalAttribute = null;
                    }
                    accountGameDTO.Quantity = entitlementValue.Value;
                    accountGameDTO.Frequency = cmbFrequency.SelectedValue != null ? cmbFrequency.SelectedValue.ToString() : "N";
                    accountGameDTO.EntitlementType = cmbFrequency.SelectedValue != null  && string.IsNullOrWhiteSpace(cmbFrequency.SelectedValue.ToString())? cmbFrequency.SelectedValue.ToString() : null;
                    accountGameDTOList.Add(accountGameDTO);
                }
            }
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void txtOptionalAttribute_Validating(object sender, CancelEventArgs e)
        {
            if (!validateAndFormatDecimal((TextBox)sender))
                e.Cancel = true;
        }

        private void cmbEntType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEntType.SelectedIndex == 0)
            {
                txtRepeat.Text = "";
                txtRepeat.Enabled = false;
            }
            else
                txtRepeat.Enabled = true;
        }

        /// <summary>
        /// Get Method of credits field
        /// </summary>
        public decimal? Credits
        {
            get
            {
                return credits;
            }
        }
        /// <summary>
        /// Get Method of courtesy field
        /// </summary>
        public decimal? Courtesy
        {
            get
            {
                return courtesy;
            }
        }
        /// <summary>
        /// Get Method of bonus field
        /// </summary>
        public decimal? Bonus
        {
            get
            {
                return bonus;
            }
        }

        /// <summary>
        /// Get Method of time field
        /// </summary>
        public decimal? Time
        {
            get
            {
                return time;
            }
        }

        /// <summary>
        /// Get Method of techGames field
        /// </summary>
        public int? TechGames
        {
            get
            {
                return techGames;
            }
        }

        /// <summary>
        /// Get Method of ticketCount field
        /// </summary>
        public int? TicketCount
        {
            get
            {
                return ticketCount;
            }
        }

        /// <summary>
        /// Get Method of accountGameDTOList field
        /// </summary>
        public List<AccountGameDTO> AccountGameDTOList
        {
            get
            {
                return accountGameDTOList;
            }
        }
    }
}

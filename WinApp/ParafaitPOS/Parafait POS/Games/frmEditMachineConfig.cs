/********************************************************************************************
 * Project Name - Parafait_POS.Games
 * Description  - New screen to update reader configuration values
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
*2.70.3       20-Dec-2019      Archana        Created
*2.70.3       06-May-2020      Deeksha        Modified to set default value for game status and scroll bar.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SqlClient;

namespace Parafait_POS.Games
{
    public partial class frmGameManagement : Form
    {
        private List<string> defaultMachineAttributes = new List<string>();
        private List<string> lookupMachineAttributeConfigs = new List<string>();
        private void LoadEditMachineConfig(object MachineId = null)
        {
            log.LogMethodEntry(MachineId);
            Cursor = Cursors.WaitCursor;
            cmbEMCTicketModeSettings.SelectedItem = null;
            txtEMCMacAddress.Clear();
            txtEMCMachineName.Clear();
            cmbEMCGameMachineStatus.SelectedIndex = -1;
            txtEMCCoinPulseGap.Clear();
            txtEMCCoinPulseWidth.Clear();
            txtEMCTicketPulseGap.Clear();
            txtEMCTicketPulseWidth.Clear();
            txtEMCInitialLEDPattern.Clear();
            txtEMCCoinMachineHopper.Clear();
            txtEMCDisplayLanguage.Clear();
            txtEMCPulseCount.Clear();
            selectedReferenceMachine = -1;
            flpEditMachineConfigs.VerticalScroll.Value = 0;
            chkStartInPhysicalTicket.Checked = false;
            chkEMCTicketEater.Checked = false;
            ClearLookupConfigFields();
            
            this.cmbEditMachines.SelectedIndexChanged -= this.cmbEditMachines_SelectedIndexChanged;
            this.cmbEMCMachine.SelectedIndexChanged -= this.cmbEMCMachine_SelectedIndexChanged;
            this.cmbEditMachineLimited.SelectedIndexChanged -= this.cmbEditMachinesLimited_SelectedIndexChanged;
            if (machineDTOList != null && machineDTOList.Count > 0)
            {
                cmbEMCMachine.DataSource = machineDTOList;
                cmbEMCMachine.DisplayMember = showMachineNameWithGameName == false ? "MachineName" : "MachineNameGameName";
                cmbEMCMachine.ValueMember = "MachineId";
                cmbEMCMachine.SelectedIndex = 0;

                cmbEMCReferenceMachine.DataSource = machineDTOList;
                cmbEMCReferenceMachine.DisplayMember = showMachineNameWithGameName == false ? "MachineName" : "MachineNameGameName";
                cmbEMCReferenceMachine.ValueMember = "MachineId";
            }
            if (gameStatusLookupValueDTOList != null && gameStatusLookupValueDTOList.Count > 0)
            {
                cmbEMCGameMachineStatus.DataSource = gameStatusLookupValueDTOList;
                cmbEMCGameMachineStatus.DisplayMember = "Description";
                cmbEMCGameMachineStatus.ValueMember = "LookupValueId";
                cmbEMCGameMachineStatus.SelectedItem = null;
            }

            if (!defaultMachineAttributes.Contains("NUMBER_OF_COINS"))
                defaultMachineAttributes.Add("NUMBER_OF_COINS");
            if (!defaultMachineAttributes.Contains("COIN_PULSE_WIDTH"))
                defaultMachineAttributes.Add("COIN_PULSE_WIDTH");
            if (!defaultMachineAttributes.Contains("TICKET_PULSE_WIDTH"))
                defaultMachineAttributes.Add("TICKET_PULSE_WIDTH");
            if (!defaultMachineAttributes.Contains("COIN_PULSE_GAP"))
                defaultMachineAttributes.Add("COIN_PULSE_GAP");
            if (!defaultMachineAttributes.Contains("TICKET_PULSE_GAP"))
                defaultMachineAttributes.Add("TICKET_PULSE_GAP");
            if (!defaultMachineAttributes.Contains("DISPLAY_LANGUAGE"))
                defaultMachineAttributes.Add("DISPLAY_LANGUAGE");
            if (!defaultMachineAttributes.Contains("TICKET_EATER"))
                defaultMachineAttributes.Add("TICKET_EATER");
            if (!defaultMachineAttributes.Contains("COIN_PUSHER_MACHINE"))
                defaultMachineAttributes.Add("COIN_PUSHER_MACHINE");
            if (!defaultMachineAttributes.Contains("INITIAL_LED_PATTERN"))
                defaultMachineAttributes.Add("INITIAL_LED_PATTERN");
            if (!defaultMachineAttributes.Contains("START_IN_PHYSICAL_TICKET_MODE"))
                defaultMachineAttributes.Add("START_IN_PHYSICAL_TICKET_MODE");

            LoadMachineConfigSetupFromLookUps();
            this.cmbEMCMachine.SelectedIndexChanged += this.cmbEMCMachine_SelectedIndexChanged;
            if (MachineId == null)
            {
                selectedReferenceMachine = -1;
            }
            else
            {
                cmbEMCMachine.SelectedIndex = -1;
                cmbEMCMachine.SelectedValue = MachineId;
            }
            cmbEMCReferenceMachine.SelectedValue = selectedReferenceMachine;
            Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void LoadMachineConfigSetupFromLookUps()
        {
            log.LogMethodEntry();
            Cursor = Cursors.WaitCursor;
            List<string> attributeList = new List<string>();
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "GAME_PROFILE_ATTRIBUTES"));
            lookupValuesDTOList = new LookupValuesList(POSStatic.Utilities.ExecutionContext).GetAllLookupValues(searchlookupParameters);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
            {
                foreach(LookupValuesDTO lookup in lookupValuesDTOList)
                {
                    if(lookup.Description == "Y")
                    {
                        attributeList.Add(lookup.LookupValue);
                    }
                }
            }
            if(attributeList.Count >0)
            {
                AddMachineAttributeControl(attributeList);
            }
            Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void AddMachineAttributeControl(List<string> attributeList)
        {
            log.LogMethodEntry(attributeList);
            string attributes = string.Empty;
            foreach (string s in attributeList)
            {
                if (!defaultMachineAttributes.Contains(s) && !lookupMachineAttributeConfigs.Contains(s))
                {
                    lookupMachineAttributeConfigs.Add(s);
                    attributes = attributes + s + ",";
                }                
            }
            if (!string.IsNullOrEmpty(attributes))
            {
                attributes = attributes.Substring(0, attributes.Length - 1);

                GameSystem gameSystem = new GameSystem();
                List<MachineAttributeDTO> machineAttributeDTOs = gameSystem.GetMachineAttributes();
                List<MachineAttributeDTO> lookupMachineAttributeDTO = new List<MachineAttributeDTO>();
                if (lookupMachineAttributeConfigs.Count >0)
                {
                    foreach(string machineAttributeName in lookupMachineAttributeConfigs)
                    {
                        lookupMachineAttributeDTO.Add(machineAttributeDTOs.Find(x => x.AttributeName.ToString() == machineAttributeName));
                    }
                }
                if (lookupMachineAttributeDTO != null && lookupMachineAttributeDTO.Count > 0)
                {
                    foreach (MachineAttributeDTO machineAttribute in lookupMachineAttributeDTO)
                    {
                        string checkBoxText = machineAttribute.AttributeName.ToString();
                        string[] s = checkBoxText.Split('_');
                        string newCheckBoxText = string.Empty;
                        TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                        for (int i = 0; i <= s.Length - 1; i++)
                        {
                            s[i] = s[i].ToLower();
                            newCheckBoxText += " " + ti.ToTitleCase(s[i]);
                        }
                        Panel panel = new Panel();
                        panel.Width = panelTicketPulseGap.Width;
                        panel.Height = panelTicketPulseGap.Height;
                        panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)))));

                        if (machineAttribute.IsFlag == "Y")
                        {
                            Label label = new Label();
                            label.Name = "lbl" + machineAttribute.AttributeName.ToString();
                            label.Size = new Size(lblTicketMode.Size.Width + 10, lblTicketMode.Height);
                            label.Text = newCheckBoxText;
                            label.FlatStyle = FlatStyle.Standard;
                            label.AutoSize = false;
                            label.TextAlign = ContentAlignment.MiddleRight;
                            label.AutoEllipsis = true;
                            panel.Controls.Add(label);
                            label.Location = new Point(panel.Location.X, label.Location.Y);
                            CheckBox chkBox = new CheckBox();
                            chkBox.Tag = machineAttribute.AttributeName;
                            chkBox.Name = "chkBox" + machineAttribute.AttributeName.ToString();
                            chkBox.Text = "";
                            //chkBox.AutoSize = true;
                            //chkBox.RightToLeft = RightToLeft.Yes;
                            chkBox.Location = new Point(txtEMCCoinPulseWidth.Location.X, label.Location.Y + 5);
                            panel.Controls.Add(chkBox);
                        }
                        else
                        {
                            Label label = new Label();
                            label.Name = "lbl" + machineAttribute.AttributeName.ToString();
                            label.Size = new Size(lblTicketMode.Size.Width + 10, lblTicketMode.Height);
                            label.Text = newCheckBoxText + ":";
                            label.FlatStyle = FlatStyle.Standard;
                            label.AutoSize = false;
                            label.TextAlign = ContentAlignment.MiddleRight;
                            label.AutoEllipsis = true;
                            panel.Controls.Add(label);
                            label.Location = new Point(panel.Location.X, label.Location.Y);
                            TextBox textBox = new TextBox();
                            panel.Controls.Add(textBox);
                            textBox.Location = new Point(txtEMCCoinPulseWidth.Location.X, label.Location.Y);
                            textBox.Tag = machineAttribute.AttributeName;
                            textBox.Width = txtEMCCoinPulseWidth.Width;
                            textBox.Height = txtEMCCoinPulseWidth.Height;
                            textBox.Name = "txtEMC" + machineAttribute.AttributeName.ToString();
                            textBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBoxConfig_KeyPress);
                        }
                        flpEditMachineConfigs.Controls.Add(panel);
                    }
                }
            }
            log.LogMethodExit();
        }
                
        private void cmbEMCMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (cmbEMCMachine.SelectedIndex >= 0 && Convert.ToInt32(cmbEMCMachine.SelectedValue) != -1)
                {
                    Cursor = Cursors.WaitCursor;
                    MachineDTO machineDTO = machineDTOList.Find(x=>x.MachineId == Convert.ToInt32(cmbEMCMachine.SelectedValue));
                    txtEMCMacAddress.Text = machineDTO.MacAddress;
                    txtEMCMachineName.Text = machineDTO.MachineName;
                    cmbEMCGameMachineStatus.SelectedValue = SelectedGameMachineStatus(machineDTO.IsActive);
                    txtEMCCoinPulseGap.Text = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.COIN_PULSE_GAP && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.COIN_PULSE_GAP && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue.ToString()) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.COIN_PULSE_GAP).AttributeValue.ToString());
                    txtEMCCoinPulseWidth.Text = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.COIN_PULSE_WIDTH && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.COIN_PULSE_WIDTH && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue.ToString()) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.COIN_PULSE_WIDTH).AttributeValue.ToString());
                    txtEMCTicketPulseGap.Text = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.TICKET_PULSE_GAP && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.TICKET_PULSE_GAP && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue.ToString()) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.TICKET_PULSE_GAP).AttributeValue.ToString());
                    txtEMCTicketPulseWidth.Text = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.TICKET_PULSE_WIDTH && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.TICKET_PULSE_WIDTH && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue.ToString()) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.TICKET_PULSE_WIDTH).AttributeValue.ToString());
                    txtEMCInitialLEDPattern.Text = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.INITIAL_LED_PATTERN && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.INITIAL_LED_PATTERN && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue.ToString()) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.INITIAL_LED_PATTERN).AttributeValue.ToString());
                    txtEMCCoinMachineHopper.Text = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.COIN_PUSHER_MACHINE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.COIN_PUSHER_MACHINE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue.ToString()) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.COIN_PUSHER_MACHINE).AttributeValue.ToString());
                    txtEMCDisplayLanguage.Text = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.DISPLAY_LANGUAGE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.DISPLAY_LANGUAGE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue.ToString()) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.DISPLAY_LANGUAGE).AttributeValue.ToString());
                    txtEMCPulseCount.Text = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue.ToString()) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS).AttributeValue.ToString());
                    cmbEMCTicketModeSettings.SelectedItem = GetSelectedTicketMode(machineDTO.TicketMode);
                    chkEMCTicketEater.Checked = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.TICKET_EATER && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.TICKET_EATER && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue == "1" ? true : false) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.TICKET_EATER).AttributeValue == "1" ? true : false);
                    chkStartInPhysicalTicket.Checked = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.START_IN_PHYSICAL_TICKET_MODE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.START_IN_PHYSICAL_TICKET_MODE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue == "1" ? true : false) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.START_IN_PHYSICAL_TICKET_MODE).AttributeValue == "1" ? true : false);
                    
                    if (lookupMachineAttributeConfigs.Count > 0)
                    {
                        foreach (string config in lookupMachineAttributeConfigs)
                        {
                            MachineAttributeDTO machineAttributeDTO = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName.ToString() == config && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName.ToString() == config && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName.ToString() == config));
                            Control c = GetMachineAttributeControl(flpEditMachineConfigs, config);
                            if (c == null)
                            {
                                continue;
                            }
                            if ((c is TextBox) && (c.Tag != null))
                            {
                                c.Text = machineAttributeDTO.AttributeValue.ToString();
                            }
                            else if ((c is CheckBox) && (c.Tag != null))
                            {
                                CheckBox chkBox = c as CheckBox;
                                chkBox.Checked = machineAttributeDTO.AttributeValue == "1" ? true : false;
                            }
                        }
                    }

                    //List<MachineDTO> machineDTOList = GetMachineList();
                    if (machineDTOList != null)
                    {
                        //Selected Machine should not be shown in reference machine combo box
                        int selectedMachine = Convert.ToInt32(cmbEMCMachine.SelectedValue);
                        List<MachineDTO> tempPachineDTOList = machineDTOList.Where(x => x.MachineId != selectedMachine).ToList();
                        if (tempPachineDTOList != null)
                        {
                            cmbEMCReferenceMachine.DataSource = tempPachineDTOList;
                            cmbEMCReferenceMachine.DisplayMember = showMachineNameWithGameName == false ? "MachineName" : "MachineNameGameName";
                            cmbEMCReferenceMachine.ValueMember = "MachineId";

                            if (machineDTO.ReferenceMachineId != -1)
                            {
                                if (machineDTOList.Any(x => x.MachineId == machineDTO.ReferenceMachineId))
                                {
                                    selectedReferenceMachine = tempPachineDTOList.Where(x => x.MachineId == Convert.ToInt32(machineDTO.ReferenceMachineId)).First().MachineId;
                                }
                            }
                            else
                            {
                                selectedReferenceMachine = -1;
                            }
                            cmbEMCReferenceMachine.SelectedValue = selectedReferenceMachine;
                        }
                    }                   

                }
            }
            catch(Exception ex)
            {
                log.Error(ex);                
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private string GetSelectedTicketMode(string ticketMode)
        {
            log.LogMethodEntry(ticketMode);
            if (ticketMode == "D")
            {
                return MachineDTO.TICKETMODE.DEFAULT.ToString();
            }
            else if (ticketMode == "T")
            {
                return MachineDTO.TICKETMODE.PHYSICAL.ToString();
            }
            else if (ticketMode == "E")
            {
                return MachineDTO.TICKETMODE.ETICKET.ToString();
            }
            log.LogMethodExit();
            return string.Empty;            
        }


        private Control GetMachineAttributeControl(Control parent, string config)
        {
            log.LogMethodEntry();
            if(parent.Tag != null && parent.Tag.ToString() == config)
            {                
                return parent;
            }
            foreach (Control child in parent.Controls)
            {
                Control result = GetMachineAttributeControl(child, config);
                if(result != null)
                {
                    return result;
                }
            }
            log.LogMethodExit();
            return null;
        }

        private void btnSaveEditMachineConfig_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Cursor = Cursors.WaitCursor;
            ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
            try
            {
                parafaitDBTrx.BeginTransaction();
                SqlTransaction sqlTransaction = parafaitDBTrx.SQLTrx;
                if (cmbEMCMachine.SelectedIndex >= 0 && Convert.ToInt32(cmbEMCMachine.SelectedValue) != -1)
                {
                    //Machine machine = new Machine(Convert.ToInt32(cmbEMCMachine.SelectedValue));
                    MachineDTO machineDTO = machineDTOList.Find(x => x.MachineId == Convert.ToInt32(cmbEMCMachine.SelectedValue));//machine.GetMachineDTO;
                    if (machineDTO != null)
                    {
                        LookupValuesDTO lookupValueDTO = (new LookupValues(POSStatic.Utilities.ExecutionContext, Convert.ToInt32(cmbEMCGameMachineStatus.SelectedValue))).LookupValuesDTO;
                        machineDTO.IsActive = lookupValueDTO.LookupValue;
                        foreach (Control childControl in flpEditMachineConfigs.Controls)
                        {
                            if (childControl.Controls.Count > 0)
                            {
                                foreach (Control c in childControl.Controls)
                                {
                                    if (c is TextBox)
                                    {
                                        if (c.Tag != null)
                                        {
                                            if (machineDTO.GameMachineAttributes.Any(x => x.AttributeName.ToString() == c.Tag.ToString() && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                                            {
                                                if (machineDTO.GameMachineAttributes.Find(x => x.AttributeName.ToString() == c.Tag.ToString() && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue != c.Text)
                                                {
                                                    machineDTO.GameMachineAttributes.Find(x => x.AttributeName.ToString() == c.Tag.ToString() && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue = c.Text;                                                 
                                                }
                                                POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(cmbEMCMachine.SelectedValue), POSStatic.Utilities.ParafaitEnv.POSMachineId, POSStatic.Utilities.ParafaitEnv.POSMachine, "Machine attribute updated from POS. " + c.Tag.ToString() + " value is changed to " + c.Text, "", "", MachineAttributeLogDTO.UpdateTypes.EDIT_MACHINE.ToString(), 1, sqlTransaction);
                                            }
                                            else
                                            {
                                                MachineAttributeDTO.MachineAttribute attribute = (MachineAttributeDTO.MachineAttribute)Enum.Parse(typeof(MachineAttributeDTO.MachineAttribute), c.Tag.ToString());
                                                MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, attribute, c.Text, "N", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, POSStatic.Utilities.ExecutionContext.GetSiteId(), POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now, -1, POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now);
                                                machineDTO.GameMachineAttributes.Add(updatedAttribute);
                                                POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(cmbEMCMachine.SelectedValue), POSStatic.Utilities.ParafaitEnv.POSMachineId, POSStatic.Utilities.ParafaitEnv.POSMachine, "Machine attribute updated from POS. " + c.Tag.ToString() + " value is changed to " + c.Text, "", "", MachineAttributeLogDTO.UpdateTypes.EDIT_MACHINE.ToString(), 1, sqlTransaction);
                                            }                                            
                                        }
                                    }
                                    else if (c is CheckBox)
                                    {
                                        CheckBox checkBoxContol = c as CheckBox;
                                        if (c.Tag != null)
                                        {
                                            if (machineDTO.GameMachineAttributes.Any(x => x.AttributeName.ToString() == c.Tag.ToString() && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                                            {
                                                if (machineDTO.GameMachineAttributes.Find(x => x.AttributeName.ToString() == c.Tag.ToString() && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue != (checkBoxContol.Checked == true ? "1" : "0"))
                                                {
                                                    machineDTO.GameMachineAttributes.Find(x => x.AttributeName.ToString() == c.Tag.ToString() && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue = checkBoxContol.Checked == true ? "1" : "0";
                                                }
                                                POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(cmbEMCMachine.SelectedValue), POSStatic.Utilities.ParafaitEnv.POSMachineId, POSStatic.Utilities.ParafaitEnv.POSMachine, "Machine attribute updated from POS. " + c.Tag.ToString() + " value is changed to " + (checkBoxContol.Checked == true ? "1" : "0"), "", "", MachineAttributeLogDTO.UpdateTypes.EDIT_MACHINE.ToString(), 1, sqlTransaction);
                                            }
                                            else
                                            {
                                                MachineAttributeDTO.MachineAttribute attribute = (MachineAttributeDTO.MachineAttribute)Enum.Parse(typeof(MachineAttributeDTO.MachineAttribute), c.Tag.ToString());
                                                MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, attribute, checkBoxContol.Checked == true ? "1" : "0", "Y", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, POSStatic.Utilities.ExecutionContext.GetSiteId(), POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now, -1, POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now);
                                                machineDTO.GameMachineAttributes.Add(updatedAttribute);
                                                POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(cmbEMCMachine.SelectedValue), POSStatic.Utilities.ParafaitEnv.POSMachineId, POSStatic.Utilities.ParafaitEnv.POSMachine, "Machine attribute updated from POS. " + c.Tag.ToString() + " value is changed to " + (checkBoxContol.Checked == true ? "1" : "0"), "", "", MachineAttributeLogDTO.UpdateTypes.EDIT_MACHINE.ToString(), 1, sqlTransaction);
                                            }                                            
                                        }
                                    }
                                }
                            }
                        }
                        if (cmbEMCTicketModeSettings.SelectedItem != null)
                        {
                            if (cmbEMCTicketModeSettings.SelectedItem.ToString().ToUpper() == MachineDTO.TICKETMODE.DEFAULT.ToString())
                            {
                                machineDTO.TicketMode = "D";
                            }
                            else if (cmbEMCTicketModeSettings.SelectedItem.ToString().ToUpper() == MachineDTO.TICKETMODE.PHYSICAL.ToString())
                            {
                                machineDTO.TicketMode = "T";
                            }
                            else if (cmbEMCTicketModeSettings.SelectedItem.ToString().ToUpper() == MachineDTO.TICKETMODE.ETICKET.ToString())
                            {
                                machineDTO.TicketMode = "E";
                            }
                            else
                            {
                                machineDTO.TicketMode = "D";
                            }
                        }
                    }
                    machineDTO.MachineCommunicationLogDTO = null;
                    Machine machine = new Machine(machineDTO);
                    machine.Save(sqlTransaction);
                    parafaitDBTrx.EndTransaction();
                    machineDTOList[machineDTOList.FindIndex(x => x.MachineId == machine.GetMachineDTO.MachineId)] = machine.GetMachineDTO;
                    POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage("Configuration settings saved successfully"));
                    LoadEditMachineConfig(cmbEMCMachine.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                parafaitDBTrx.RollBack();
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                parafaitDBTrx.Dispose();
                Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        void ClearLookupConfigFields()
        {
            log.LogMethodEntry();
            try
            {
                if (lookupMachineAttributeConfigs.Count > 0)
                {
                    foreach (string config in lookupMachineAttributeConfigs)
                    {
                        Control c = GetMachineAttributeControl(flpEditMachineConfigs, config);
                        if (c == null)
                        {
                            continue;
                        }
                        if ((c is TextBox) && (c.Tag != null))
                        {
                            c.Text = string.Empty;
                        }
                        else if ((c is CheckBox) && (c.Tag != null))
                        {
                            CheckBox chkBox = c as CheckBox;
                            chkBox.Checked = false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
    }
}

/********************************************************************************************
 * Project Name - Parafait_POS.Games
 * Description  -  A Ui for frmGameManagement
 * 
 **************
 **Version Log
 **************
 *Version        Date            Modified By     Remarks          
 *********************************************************************************************
 *2.70.2         20-Aug-2019     Girish Kundar   Modified : Added Logger methods and Removed unused namespace's 
 *2.70.3         20-Dec-2019     Archana         Modified : Converted to 3 tier structure as part of 
 *                                               GameManagement changes to add EditMachineConfig and EditMachineimited screen
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Game;
using Semnox.Parafait.DigitalSignage;

namespace Parafait_POS.Games
{
    public partial class frmGameManagement : Form
    {
        private void LoadAddMachine()
        {
            log.LogMethodEntry();
            txtMacAddress.Clear();
            txtMachineName.Clear();
            txtCoinPulseCount.Text = "1";
            txtSerialNumber.Clear(); 
            txtMachineTag.Clear();
            
            chkReverseDisplay.Checked = false;

            cmbAps.DataSource = hubListDTO;
            cmbAps.ValueMember = "MasterId";
            cmbAps.DisplayMember = "HubNameWithMachineCount";
            cmbAps.SelectedIndex = -1;

            if (gameDTOList != null && gameDTOList.Any())
            {
                cmbGames.DataSource = gameDTOList;
                cmbGames.ValueMember = "GameId";
                cmbGames.DisplayMember = "GameName";
            }
            if (themeListDTO != null && themeListDTO.Any())
            {
                cmbThemeNo.DataSource = themeListDTO;
                cmbThemeNo.ValueMember = "Id";
                cmbThemeNo.DisplayMember = "ThemeNameWithThemeNumber";
            }
            cmbThemeNo.SelectedIndex = -1;

            if (machineDTOList != null && machineDTOList.Count > 0)
            {
                
                cmbAddReferenceMachine.DataSource = machineDTOList;
                cmbAddReferenceMachine.DisplayMember = showMachineNameWithGameName == false ? "MachineName" : "MachineNameGameName";
                cmbAddReferenceMachine.ValueMember = "MachineId";
            }
            cmbAddReferenceMachine.SelectedIndex = -1;
            this.cmbEditMachines.SelectedIndexChanged -= this.cmbEditMachines_SelectedIndexChanged;
            this.cmbEMCMachine.SelectedIndexChanged -= this.cmbEMCMachine_SelectedIndexChanged;
            this.cmbEditMachineLimited.SelectedIndexChanged -= this.cmbEditMachinesLimited_SelectedIndexChanged;
            log.LogMethodExit();
        }
        private void cmbGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtMachineName.Text = cmbGames.Text;
            log.LogMethodExit();
        }

        private void btnAddMachine_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (cmbGames.SelectedIndex >= 0)
                {
                    Cursor = Cursors.WaitCursor;
                    txtMachineName.Text = txtMachineName.Text.Trim();
                    if (txtMachineName.Text != "")
                    {
                        if (machineDTOList != null && machineDTOList.Any() && machineDTOList.Exists(x => x.MachineName == txtMachineName.Text))
                        {
                            POSUtils.ParafaitMessageBox("Machine " + txtMachineName.Text + " already exists in system", "Game Management : Add Machine");
                            log.Info("Ends-btnAddMachine_Click() as Machine " + txtMachineName.Text + " already exists in system");//Added for logger function on 08-Mar-2016
                            return;
                        }

                        if (cmbAps.SelectedIndex <= 0)
                        {
                            POSUtils.ParafaitMessageBox("Please select an access point", "Game Management : Add Machine");
                            log.Info("Ends-btnAddMachine_Click() as no access point is selected");//Added for logger function on 08-Mar-2016
                            return;
                        }

                        int numCoins = 1;
                        txtCoinPulseCount.Text = txtCoinPulseCount.Text.Trim();
                        Int32.TryParse(txtCoinPulseCount.Text, out numCoins);
                        if (numCoins <= 0 || numCoins > 10)
                            numCoins = 1;

                        //Start Modification: for inserting serial number on 27-Dec-2016 
                        object serialNumber;
                        if (string.IsNullOrEmpty(txtSerialNumber.Text.Trim()))
                        {
                            serialNumber = DBNull.Value;
                        }
                        else
                        {
                            serialNumber = txtSerialNumber.Text;
                        }

                        MachineDTO machineDTO = new MachineDTO();
                        machineDTO.MachineId = -1;
                        machineDTO.GameId = Convert.ToInt32(cmbGames.SelectedValue);
                        machineDTO.IsActive = "Y";
                        machineDTO.GroupTimer = "N";
                        machineDTO.LastUpdatedBy = POSStatic.ParafaitEnv.LoginID;
                        machineDTO.LastUpdateDate = POSStatic.Utilities.getServerTime();
                        machineDTO.MacAddress = txtMacAddress.Text;
                        machineDTO.MachineName = txtMachineName.Text;
                        machineDTO.MasterId = Convert.ToInt32(cmbAps.SelectedValue);
                        machineDTO.SerialNumber = serialNumber.ToString();
                        machineDTO.MachineTag = (string.IsNullOrEmpty(txtMachineTag.Text.Trim())) ? "" : txtMachineTag.Text.ToString();
                        machineDTO.ShowAd = "D";
                        machineDTO.TicketMode = "D";
                        machineDTO.TicketAllowed = "Y";
                        machineDTO.TimerMachine = "N";
                        machineDTO.NumberOfCoins = numCoins;
                        machineDTO.MachineCommunicationLogDTO = null;
                        machineDTO.ThemeId = cmbThemeNo.SelectedIndex != -1 ? Convert.ToInt32(cmbThemeNo.SelectedValue) : -1;
                        machineDTO.ReferenceMachineId = Convert.ToInt32(cmbAddReferenceMachine.SelectedIndex) == -1 ? -1 : Convert.ToInt32(cmbAddReferenceMachine.SelectedValue);
                        Machine machine = new Machine(machineDTO, POSStatic.Utilities.ExecutionContext, null, true);
                        machineDTO.AddToAttributes(MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION, chkReverseDisplay.Checked ? "1" : "0");
                        machineDTO.AddToAttributes(MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS, numCoins.ToString());                        
                        machineDTO.MachineAddress = machine.GetMachineAddress(Convert.ToInt32(cmbAps.SelectedValue));
                        machine.Save();
                        POSStatic.Utilities.MachineLog.logMachineUpdate(machine.GetMachineDTO.MachineId, "Machine added from POS", "", "", MachineAttributeLogDTO.UpdateTypes.ADD_MACHINE.ToString(), 1);
                        machineDTOList.Add(machine.GetMachineDTO);

                        if (showMachineNameWithGameName)
                        {
                            machineDTOList = machineDTOList.OrderBy(x => x.MachineNameGameName).ToList();
                        }
                        else
                        {
                            machineDTOList = machineDTOList.OrderBy(x => x.MachineName).ToList();
                        }
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "Machine added successfully"), "Game Management : Add Machine");
                        log.Info("btnAddMachine_Click() - Machine added successfully");//Added for logger function on 08-Mar-2016                       
                    }
                }
                Cursor = Cursors.WaitCursor;
                GetHubListDTO();
                LoadAddMachine();
            }
            catch (ValidationException vex)
            {
                log.Error(e);
                POSUtils.ParafaitMessageBox(vex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message, "Game Management : Add Machine");
            }
            finally
            {
                Cursor = Cursors.Default;
            }            
            log.LogMethodExit();
        }
       
    }
}

/********************************************************************************************
 * Project Name - Parafait_POS.Games
 * Description  -  A UI for frmGameManagement
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
*2.70.2       20-Aug-2019     Girish Kundar   Modified : Added Logger methods and Removed unused namespace's 
*2.70.3       20-Dec-2019     Archana         Modified : Converted to 3 tier structure as part of 
*                                             GameManagement changes to add EditMachineConfig and EditMachineimited screen
*2.80.0       14-Jun-2020     Deeksha         Fix for game mgt Edit machine issue if there is no themes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Game;

namespace Parafait_POS.Games
{
    public partial class frmGameManagement : Form
    {
        int selectedReferenceMachine = -1;
        void editMachineLoad(object MachineId = null)
        {
            log.LogMethodEntry(MachineId);
            try
            {
                Cursor = Cursors.WaitCursor;
                
                this.cmbEditMachines.SelectedIndexChanged -= this.cmbEditMachines_SelectedIndexChanged;
                this.cmbEMCMachine.SelectedIndexChanged -= this.cmbEMCMachine_SelectedIndexChanged;
                this.cmbEditMachineLimited.SelectedIndexChanged -= this.cmbEditMachinesLimited_SelectedIndexChanged;
                selectedReferenceMachine = -1;
                txtEditMacAddress.Clear();
                txtEditMachineName.Clear();
                txtEditPulseCount.Clear();
                txtEditSerialNo.Clear();
                txtEditMachineTag.Clear();
                cmbEditAP.SelectedIndex = -1;
                cmbEditThemeNo.SelectedIndex = -1;
                chkEditReverseDisplay.Checked = false;

                if (hubListDTO != null && hubListDTO.Any())
                {
                    cmbEditAP.DataSource = hubListDTO;
                    cmbEditAP.ValueMember = "MasterId";
                    cmbEditAP.DisplayMember = "HubNameWithMachineCount";
                }
                if (machineDTOList != null && machineDTOList.Any())
                {
                    cmbEditMachines.DataSource = machineDTOList;
                    cmbEditMachines.ValueMember = "MachineId";
                    cmbEditMachines.DisplayMember = showMachineNameWithGameName == false ? "MachineName" : "MachineNameGameName";
                    cmbEditMachines.SelectedIndex = 0;

                    cmbEditReferenceMachine.DataSource = machineDTOList;
                    cmbEditReferenceMachine.DisplayMember = showMachineNameWithGameName == false ? "MachineName" : "MachineNameGameName";
                    cmbEditReferenceMachine.ValueMember = "MachineId";
                }
                if (themeListDTO != null && themeListDTO.Any())
                {
                    cmbEditThemeNo.DataSource = themeListDTO;
                    cmbEditThemeNo.ValueMember = "Id";
                    cmbEditThemeNo.DisplayMember = "ThemeNameWithThemeNumber";
                }

                this.cmbEditMachines.SelectedIndexChanged += this.cmbEditMachines_SelectedIndexChanged;
                if (MachineId == null)
                {
                    selectedReferenceMachine = -1;
                }
                else
                {
                    cmbEditMachines.SelectedIndex = -1;
                    cmbEditMachines.SelectedValue = MachineId;
                }
                cmbEditReferenceMachine.SelectedValue = selectedReferenceMachine;

                if (gameStatusLookupValueDTOList != null && gameStatusLookupValueDTOList.Count > 0)
                {
                    cmbGameMachineStatus.DataSource = gameStatusLookupValueDTOList;
                    cmbGameMachineStatus.DisplayMember = "Description";
                    cmbGameMachineStatus.ValueMember = "LookupValueId";
                }                
            }
            catch(Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }        

        private void cmbEditMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (cmbEditMachines.SelectedIndex >= 0 && Convert.ToInt32(cmbEditMachines.SelectedValue) != -1)
                {
                    Cursor = Cursors.WaitCursor;
                    MachineDTO machineDTO = machineDTOList.Find(x => x.MachineId == Convert.ToInt32(cmbEditMachines.SelectedValue));
                      
                    txtEditMachineName.Text = machineDTO.MachineName;
                    txtEditMacAddress.Text = machineDTO.MacAddress;
                    cmbEditThemeNo.SelectedValue = machineDTO.ThemeId;
                    if (machineDTO.GameMachineAttributes != null && machineDTO.GameMachineAttributes.Any())
                    {
                        txtEditPulseCount.Text = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue.ToString()) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS).AttributeValue.ToString());
                        chkEditReverseDisplay.Checked = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue == "1" ? true : false) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION).AttributeValue == "1" ? true : false);
                    }
                    
                    cmbGameMachineStatus.SelectedValue = SelectedGameMachineStatus(machineDTO.IsActive);

                    cmbEditAP.SelectedValue = machineDTO.MasterId;
                    txtEditSerialNo.Text = machineDTO.SerialNumber; 
                    txtEditMachineTag.Text = machineDTO.MachineTag; 
                    txtEditMachineName.ReadOnly = true; 

                    if (machineDTOList != null)
                    {
                        //Selected Machine should not be shown in reference machine combo box
                        int selectedMachine = Convert.ToInt32(cmbEditMachines.SelectedValue);
                        List<MachineDTO> tempPachineDTOList = machineDTOList.Where(x => x.MachineId != selectedMachine).ToList();
                        if (tempPachineDTOList != null)
                        {
                            log.Debug("Loading ReferenceMachine");
                            cmbEditReferenceMachine.DataSource = tempPachineDTOList;
                            cmbEditReferenceMachine.DisplayMember = showMachineNameWithGameName == false ? "MachineName" : "MachineNameGameName";
                            cmbEditReferenceMachine.ValueMember = "MachineId";

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
                            cmbEditReferenceMachine.SelectedValue = selectedReferenceMachine;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnSaveEditMachine_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (cmbEditMachines.SelectedIndex >= 0 && Convert.ToInt32(cmbEditMachines.SelectedValue) != -1)
                {
                    Cursor = Cursors.WaitCursor;
                    int numCoins = 1;
                    txtEditPulseCount.Text = txtEditPulseCount.Text.Trim();
                    Int32.TryParse(txtEditPulseCount.Text, out numCoins);
                    if (numCoins <= 0 || numCoins > 10)
                        numCoins = 1;
                    
                    object serialNumber;
                    if (string.IsNullOrEmpty(txtEditSerialNo.Text.Trim()))
                    {
                        serialNumber = DBNull.Value;
                    }
                    else
                    {
                        serialNumber = txtEditSerialNo.Text;
                    }

                    LookupValuesDTO lookupValueDTO = (new LookupValues(POSStatic.Utilities.ExecutionContext, Convert.ToInt32(cmbGameMachineStatus.SelectedValue))).LookupValuesDTO;

                    MachineDTO machineDTO = machineDTOList.Find(x => x.MachineId == Convert.ToInt32(cmbEditMachines.SelectedValue));//machine.GetMachineDTO;
                    machineDTO.IsActive = lookupValueDTO.LookupValue;
                    machineDTO.MacAddress = txtEditMacAddress.Text;
                    machineDTO.MasterId = Convert.ToInt32(cmbEditAP.SelectedValue);
                    machineDTO.SerialNumber = serialNumber.ToString();
                    machineDTO.MachineTag = (string.IsNullOrEmpty(txtEditMachineTag.Text.Trim())) ? string.Empty : (txtEditMachineTag.Text);
                    machineDTO.ThemeId = (cmbEditThemeNo.SelectedValue == null) ? -1 : Convert.ToInt32(cmbEditThemeNo.SelectedValue);
                    machineDTO.ReferenceMachineId = Convert.ToInt32(cmbEditReferenceMachine.SelectedValue);
                     
                    if (machineDTO.GameMachineAttributes.Any(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                    {
                        machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue = chkEditReverseDisplay.Checked == true ? "1" : "0";
                    }
                    else
                    {                        
                        MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION, chkEditReverseDisplay.Checked == true ? "1" : "0", "Y", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, POSStatic.Utilities.ExecutionContext.GetSiteId(), POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now, -1, POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now);
                        machineDTO.GameMachineAttributes.Add(updatedAttribute);
                    }
                                       
                    if (machineDTO.GameMachineAttributes.Any(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                    {
                        machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue = numCoins.ToString();
                    }
                    else
                    {
                        MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS, numCoins.ToString(), "N", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, POSStatic.Utilities.ExecutionContext.GetSiteId(), POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now, -1, POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now);
                        machineDTO.GameMachineAttributes.Add(updatedAttribute);
                    }
                    POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(cmbEditMachines.SelectedValue), "Machine attribute updated from POS. REVERSE_DISPLAY_DIRECTION changed to " + chkEditReverseDisplay.Checked, "", "", MachineAttributeLogDTO.UpdateTypes.EDIT_MACHINE.ToString(), 1);
                    POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(cmbEditMachines.SelectedValue), "Machine attribute updated from POS. NUMBER_OF_COINS changed to " + numCoins.ToString(), "", "", MachineAttributeLogDTO.UpdateTypes.EDIT_MACHINE.ToString(), 1);
                    machineDTO.MachineCommunicationLogDTO = null;
                    Machine machine = new Machine(machineDTO);
                    machine.Save();
                    machineDTOList[machineDTOList.FindIndex(x => x.MachineId == machine.GetMachineDTO.MachineId)] = machine.GetMachineDTO;
                    POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(cmbEditMachines.SelectedValue), "Machine updated from POS", "", "", MachineAttributeLogDTO.UpdateTypes.EDIT_MACHINE.ToString(), 1);
                    Machine machineBL = new Machine(POSStatic.Utilities.ExecutionContext);

                    List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID, Convert.ToString(cmbEditMachines.SelectedValue)));

                    MachineList machineListBL = new MachineList(POSStatic.Utilities.ExecutionContext);
                    List<MachineDTO> machineList = machineListBL.GetMachineList(searchParameters);
                    if (machineList != null)
                    {
                        foreach (MachineDTO referenceMachineDTO in machineList)
                        {
                            if (referenceMachineDTO.IsActive != lookupValueDTO.LookupValue)
                            {
                                referenceMachineDTO.IsActive = lookupValueDTO.LookupValue;
                                referenceMachineDTO.MachineCommunicationLogDTO = null;
                                machineBL.SetMachineDTO = referenceMachineDTO;                                
                                machineBL.Save();
                                machineDTOList[machineDTOList.FindIndex(x => x.MachineId == machine.GetMachineDTO.MachineId)] = machineBL.GetMachineDTO;
                                POSStatic.Utilities.MachineLog.logMachineUpdate(referenceMachineDTO.MachineId, "Machine updated from POS. Active Status changed to " + lookupValueDTO.LookupValue, "", "", MachineAttributeLogDTO.UpdateTypes.EDIT_MACHINE.ToString(), 1);
                            }
                        }
                    }
                    editMachineLoad(cmbEditMachines.SelectedValue);
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext,"Machine updated successfully"), "Game Management : Edit Machine");
                }
            }
            catch(ValidationException vex)
            {
                log.Error(e);
                POSUtils.ParafaitMessageBox(vex.Message);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext,"Unexpected error occurred while saving machine updates"));
            } 
            finally
            {
                Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
    }
}

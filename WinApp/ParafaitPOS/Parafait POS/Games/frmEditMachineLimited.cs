/********************************************************************************************
 * Project Name - Parafait_POS.Games
 * Description  -  A UI for EditMachineLimited
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.3      21-Nov-2019      Archana        Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_POS.Games
{
    public partial class frmGameManagement : Form
    {
        void LoadEditMachineLimited(object machineId = null)
        {
            log.LogMethodEntry(machineId);
            Cursor = Cursors.WaitCursor;
            txtEditLimitedMachineName.Clear();
            txtEditLimitedMachineTag.Clear();
            txtEditLimitedSerialNo.Clear();
            txtEditLimitedMACAddr.Clear();
            selectedReferenceMachine = -1;         
            
            this.cmbEditMachines.SelectedIndexChanged -= this.cmbEditMachines_SelectedIndexChanged;
            this.cmbEMCMachine.SelectedIndexChanged -= this.cmbEMCMachine_SelectedIndexChanged;
            this.cmbEditMachineLimited.SelectedIndexChanged -= this.cmbEditMachinesLimited_SelectedIndexChanged;
            if (machineDTOList != null && machineDTOList.Count > 0)
            {
                cmbEditMachineLimited.DataSource = machineDTOList;                
                cmbEditMachineLimited.DisplayMember = showMachineNameWithGameName == false ? "MachineName" : "MachineNameGameName";
                cmbEditMachineLimited.ValueMember = "MachineId";
                cmbEditMachineLimited.SelectedIndex = 0;

                cmbEditLimitedReferenceMachine.DataSource = machineDTOList;
                cmbEditLimitedReferenceMachine.DisplayMember = showMachineNameWithGameName == false ? "MachineName" : "MachineNameGameName";
                cmbEditLimitedReferenceMachine.ValueMember = "MachineId";
            }
           
            if (hubListDTO != null && hubListDTO.Any())
            {
                cmbEditLimitedAccessPoint.DataSource = hubListDTO;
                cmbEditLimitedAccessPoint.ValueMember = "MasterId";
                cmbEditLimitedAccessPoint.DisplayMember = "HubNameWithMachineCount";
            }
            if (themeListDTO != null && themeListDTO.Any())
            {
                cmbEditLimitedThemeNo.DataSource = themeListDTO;
                cmbEditLimitedThemeNo.ValueMember = "Id";
                cmbEditLimitedThemeNo.DisplayMember = "ThemeNameWithThemeNumber";
            }
            if (gameStatusLookupValueDTOList != null && gameStatusLookupValueDTOList.Count > 0)
            {
                cmbEditLimitedGameMachineStatus.DataSource = gameStatusLookupValueDTOList;
                cmbEditLimitedGameMachineStatus.DisplayMember = "Description";
                cmbEditLimitedGameMachineStatus.ValueMember = "LookupValueId";
            }
            cmbEditLimitedAccessPoint.SelectedIndex = -1;
            cmbEditLimitedThemeNo.SelectedIndex = -1;
            cmbEditLimitedReferenceMachine.SelectedIndex = -1;
            cmbEditMachineLimited.SelectedIndex = -1;
            this.cmbEditMachineLimited.SelectedIndexChanged += this.cmbEditMachinesLimited_SelectedIndexChanged;
            if (machineId == null)
            {
                selectedReferenceMachine = -1;
            }
            else
            {
                cmbEditMachineLimited.SelectedIndex = -1;
                cmbEditMachineLimited.SelectedValue = machineId;
            }
            Cursor = Cursors.Default;
            log.LogMethodExit();
        }
                

        private void cmbEditMachinesLimited_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            
            if (cmbEditMachineLimited.SelectedIndex >= 0 && Convert.ToInt32(cmbEditMachineLimited.SelectedValue) != -1)
            {
                Cursor = Cursors.WaitCursor;               
                MachineDTO machineDTO = machineDTOList.Find(x => x.MachineId == Convert.ToInt32(cmbEditMachineLimited.SelectedValue));               
                txtEditLimitedMACAddr.Text = machineDTO.MacAddress;
                txtEditLimitedMachineName.Text = machineDTO.MachineName;
                txtEditLimitedSerialNo.Text = machineDTO.SerialNumber;
                txtEditLimitedMachineTag.Text = machineDTO.MachineTag;
                cmbEditLimitedAccessPoint.SelectedValue = machineDTO.MasterId;
                cmbEditLimitedThemeNo.SelectedValue = machineDTO.ThemeId;
                cmbEditLimitedGameMachineStatus.SelectedValue = SelectedGameMachineStatus(machineDTO.IsActive);
                if (machineDTO.GameMachineAttributes != null && machineDTO.GameMachineAttributes.Any())
                {
                    chbEditLimitedReverseDisplay.Checked = (machineDTO.GameMachineAttributes.Exists(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)) == true ? (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue == "1" ? true : false) : (machineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION).AttributeValue == "1" ? true : false);
                }
                txtEditMachineName.ReadOnly = true; //Make Machine Name read-only 06-Apr-2016
                if (machineDTOList != null)
                {
                    //Selected Machine should not be shown in reference machine combo box
                    int selectedMachine = Convert.ToInt32(cmbEditMachineLimited.SelectedValue);
                    List<MachineDTO> tempPachineDTOList = machineDTOList.Where(x => x.MachineId != selectedMachine).ToList();
                    if (tempPachineDTOList != null)
                    {                        
                        log.Debug("Loading ReferenceMachine");
                        cmbEditLimitedReferenceMachine.DataSource = tempPachineDTOList;
                        cmbEditLimitedReferenceMachine.DisplayMember = showMachineNameWithGameName == false ? "MachineName" : "MachineNameGameName";
                        cmbEditLimitedReferenceMachine.ValueMember = "MachineId";

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
                        cmbEditLimitedReferenceMachine.SelectedValue = selectedReferenceMachine;
                    }
                }
            }
            Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        int SelectedGameMachineStatus(string activeFlagStatus)
        {
            log.LogMethodEntry(activeFlagStatus);
            int lookupId = -1;
            if (gameStatusLookupValueDTOList != null && gameStatusLookupValueDTOList.Count > 0)
            {
                lookupId = gameStatusLookupValueDTOList.Find(x => x.LookupValue == activeFlagStatus).LookupValueId;
            }
            log.LogMethodExit(lookupId);
            return lookupId;            
        }

        private void btnSaveEditLimited_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Cursor = Cursors.WaitCursor;
                if (cmbEditMachineLimited.SelectedIndex >= 0 && Convert.ToInt32(cmbEditMachineLimited.SelectedValue) != -1)
                {
                    Machine machineBL = new Machine(Convert.ToInt32(cmbEditMachineLimited.SelectedValue));
                    if (!string.IsNullOrEmpty(txtEditSerialNo.Text.Trim()))
                    {
                        machineBL.GetMachineDTO.SerialNumber = txtEditLimitedSerialNo.Text;
                    }
                    LookupValuesDTO lookupValueDTO = (new LookupValues(POSStatic.Utilities.ExecutionContext, Convert.ToInt32(cmbEditLimitedGameMachineStatus.SelectedValue))).LookupValuesDTO;
                    machineBL.GetMachineDTO.IsActive = lookupValueDTO.LookupValue;
                    machineBL.GetMachineDTO.MasterId = Convert.ToInt32(cmbEditLimitedAccessPoint.SelectedValue);
                    machineBL.GetMachineDTO.ThemeId = Convert.ToInt32(cmbEditLimitedThemeNo.SelectedValue);
                    
                    if (machineBL.GetMachineDTO.GameMachineAttributes.Any(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                    {
                        machineBL.GetMachineDTO.GameMachineAttributes.Find(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).AttributeValue = chbEditLimitedReverseDisplay.Checked == true ? "1" : "0";
                    }
                    else
                    {
                        MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION, chbEditLimitedReverseDisplay.Checked == true ? "1" : "0", "Y", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, POSStatic.Utilities.ExecutionContext.GetSiteId(), POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now, -1, POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now);
                        machineBL.GetMachineDTO.GameMachineAttributes.Add(updatedAttribute);
                    }
                    POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(cmbEditMachineLimited.SelectedValue), "Machine attribute updated from POS. REVERSE_DISPLAY_DIRECTION changed to " + chbEditLimitedReverseDisplay.Checked, "", "", MachineAttributeLogDTO.UpdateTypes.EDIT_MACHINE.ToString(), 1);
                    machineBL.GetMachineDTO.MachineCommunicationLogDTO = null;
                    machineBL.Save();
                    POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(cmbEditMachineLimited.SelectedValue), "Machine updated from POS", "", "", MachineAttributeLogDTO.UpdateTypes.EDIT_MACHINE.ToString(), 1);
                    machineDTOList[machineDTOList.FindIndex(x => x.MachineId == machineBL.GetMachineDTO.MachineId)] = machineBL.GetMachineDTO;
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext,"Machine updated successfully"), "Game Management : Edit Machine");
                    LoadEditMachineLimited(machineBL.GetMachineDTO.MachineId);
                }
                
            }
            catch (ValidationException vex)
            {
                log.Error(e);
                POSUtils.ParafaitMessageBox(vex.Message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox("Unexpected error occurred while saving machine updates");
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
    }
}


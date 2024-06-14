/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Game Management Edit Machine View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy              Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Languages;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Game;
using Semnox.Parafait.Languages;
using Semnox.Parafait.ViewContainer;
using System.Threading.Tasks;

namespace Semnox.Parafait.GamesUI
{
    public class GMEditMachineVM : BaseWindowViewModel
    {
        #region Members
        private string heading;
        private MachineDTO editMachineDetails;
        private bool editMachineAccess;
        private bool editLimitedAccess;
        private bool editConfigAccess;
        private GMEditMachineView editMachineView;
        private string message;

        private ObservableCollection<HubDTO> hubNames;
        private ObservableCollection<GameContainerDTO> gameNames;
        private ObservableCollection<ThemeContainerDTO> themeName;
        private ObservableCollection<MachineDTO> refMachineName;
        private ObservableCollection<LookupValuesContainerDTO> machineStatus;
        private ObservableCollection<string> ticketModes;

        private HubDTO selectedHubName;
        private MachineDTO selectedRefMachineName;
        private ThemeContainerDTO selectedThemeName;
        private LookupValuesContainerDTO selectedMachineStatus;
        private string selectedTicketMode;
        private LookupsContainerDTO gameMachineStatus;
        private LookupsContainerDTO gameMachineAttributes;

        private ICommand editMachineCommand;
        private ICommand cancelMachineCommand;

        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Members

        #region Constructor
        public GMEditMachineVM(MachineDTO selectedMachine, ObservableCollection<MachineDTO> machinesList, ObservableCollection<HubDTO> hubsList, ObservableCollection<GameContainerDTO> gamesList, ObservableCollection<ThemeContainerDTO> ThemeList,
            bool editmachineaccess, bool editlimitedaccess, bool editconfigaccess, bool showMachineNameWithGameName, LookupsContainerDTO gameMachineStatus, LookupsContainerDTO gameMachineAttributes,
            ExecutionContext executioncontext, Grid dynamicgrid)
        {
            log.LogMethodEntry(selectedMachine, machinesList, hubsList, gamesList, ThemeList, editmachineaccess, editlimitedaccess, editconfigaccess, showMachineNameWithGameName, gameMachineStatus, gameMachineAttributes, executioncontext, dynamicgrid);
            log.Info("Edit machine page is opened for machine id" + selectedMachine.MachineId);

            editMachineCommand = new DelegateCommand(EditMachine);
            cancelMachineCommand = new DelegateCommand(CancelEditMachine);

            this.executionContext = executioncontext;
            this.gameMachineStatus = gameMachineStatus;
            this.gameMachineAttributes = gameMachineAttributes;
            MachineStatus = new ObservableCollection<LookupValuesContainerDTO>();
            HubNames = hubsList;
            GameNames = gamesList;
            RefMachineNames = machinesList;
            ThemeNames = ThemeList;
            FooterVM = new FooterVM(executioncontext)
            {
                Message = string.Empty,
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Collapsed,
            };
            editMachineDetails = new MachineDTO();
            editMachineDetails = selectedMachine;
            Heading = EditMachineDetails.MachineName;
            this.editMachineAccess = editmachineaccess;
            this.editLimitedAccess = editlimitedaccess;
            this.editConfigAccess = editconfigaccess;

            LoadDynamicFields(dynamicgrid);

            log.LogMethodExit();

        }
        #endregion Constructor

        #region Properties
        public string Heading
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(heading);
                return heading;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref heading, value);
            }
        }

        public MachineDTO EditMachineDetails
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(editMachineDetails);
                return editMachineDetails;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref editMachineDetails, value);
            }
        }

        public ObservableCollection<HubDTO> HubNames
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(hubNames);
                return hubNames;
            }
            set
            {
                log.LogMethodEntry(value);
                hubNames = value;
                SetProperty(ref hubNames, value);
            }
        }

        public HubDTO SelectedHubName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedHubName);
                return selectedHubName;
            }
            set
            {
                log.LogMethodEntry(value);
                selectedHubName = value;
                if (value != null)
                {
                    EditMachineDetails.MasterId = value.MasterId;
                }
                SetProperty(ref selectedHubName, value);
            }
        }

        public ObservableCollection<string> TicketModes
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ticketModes);
                return ticketModes;
            }
            set
            {
                log.LogMethodEntry(value);
                ticketModes = value;
                SetProperty(ref ticketModes, value);
            }
        }

        public string SelectedTicketMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedTicketMode);
                return selectedTicketMode;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    string upperValue = value.ToUpper();
                    if (upperValue.Equals(MessageViewContainerList.GetMessage(executionContext, MachineDTO.TICKETMODE.DEFAULT.ToString())))
                    {
                        EditMachineDetails.TicketMode = "D";
                    }
                    else if (upperValue.Equals(MessageViewContainerList.GetMessage(executionContext, MachineDTO.TICKETMODE.PHYSICAL.ToString())))
                    {
                        EditMachineDetails.TicketMode = "P";
                    }
                    else
                    {
                        EditMachineDetails.TicketMode = "E";
                    }
                }
                SetProperty(ref selectedTicketMode, value);
            }
        }

        public ObservableCollection<GameContainerDTO> GameNames
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(gameNames);
                return gameNames;
            }
            set
            {
                log.LogMethodEntry(value);
                gameNames = value;
                SetProperty(ref gameNames, value);
            }
        }

        public ObservableCollection<ThemeContainerDTO> ThemeNames
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(themeName);
                return themeName;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref themeName, value);
            }
        }

        public ThemeContainerDTO SelectedThemeName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedThemeName);
                return selectedThemeName;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    EditMachineDetails.ThemeId = value.Id;
                }
                SetProperty(ref selectedThemeName, value);
            }
        }

        public ObservableCollection<MachineDTO> RefMachineNames
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(refMachineName);
                return refMachineName;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref refMachineName, value);
            }
        }

        public MachineDTO SelectedRefMachineName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedRefMachineName);
                return selectedRefMachineName;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    EditMachineDetails.ReferenceMachineId = value.MachineId;
                }
                SetProperty(ref selectedRefMachineName, value);
            }
        }

        public LookupValuesContainerDTO SelectedMachineStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedMachineStatus);
                return selectedMachineStatus;
            }
            set
            {
                log.LogMethodEntry(value);
                if (value != null)
                {
                    EditMachineDetails.IsActive = value.LookupValue;
                }
                SetProperty(ref selectedMachineStatus, value);
            }
        }

        public ObservableCollection<LookupValuesContainerDTO> MachineStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(machineStatus);
                return machineStatus;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref machineStatus, value);
            }
        }

        public string SuccessMessage
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(message);
                return message;
            }
            set
            {
                log.LogMethodEntry(value);
                message = value;
            }
        }

        #endregion Properties


        #region Methods

        private Grid LoadDynamicFields(Grid grid)
        {
            log.LogMethodEntry(grid);
            try
            {
                ColumnDefinition firstColumnDefinition = new ColumnDefinition();
                ColumnDefinition secondColumnDefinition = new ColumnDefinition();
                firstColumnDefinition.Width = new GridLength(1, GridUnitType.Star);
                secondColumnDefinition.Width = new GridLength(1, GridUnitType.Star);

                grid.ColumnDefinitions.Add(firstColumnDefinition);
                grid.ColumnDefinitions.Add(secondColumnDefinition);

                Grid leftGrid = new Grid();
                leftGrid.Name = "LeftContentGridContainer";
                Grid rightGrid = new Grid();
                rightGrid.Name = "RightContentGridContainer";


                StackPanel leftStackPanel = new StackPanel();
                StackPanel rightStackPanel = new StackPanel();

                leftStackPanel.Margin = new Thickness(16, 0, 8, 0);
                rightStackPanel.Margin = new Thickness(8, 0, 16, 0);

                string position = "Left";

                CustomTextBox txtGameName = new CustomTextBox();
                txtGameName.Heading = MessageViewContainerList.GetMessage(executionContext, "Game Name");
                txtGameName.Size = CommonUI.Size.Medium;
                txtGameName.IsEnabled = false;
                txtGameName.Text = EditMachineDetails.GameName;

                Binding gameNameWidthBinding = new Binding();
                gameNameWidthBinding.Source = leftGrid;
                gameNameWidthBinding.Path = new PropertyPath("ActualWidth");
                txtGameName.SetBinding(CustomTextBox.MaxWidthProperty, gameNameWidthBinding);

                if (position == "Left")
                {
                    leftStackPanel.Children.Add(txtGameName);
                    position = "Right";
                }
                else
                {
                    rightStackPanel.Children.Add(txtGameName);
                    position = "Left";
                }

                CustomTextBox txtMachineName = new CustomTextBox();
                txtMachineName.Heading = MessageViewContainerList.GetMessage(executionContext, "Machine Name");
                txtMachineName.Size = CommonUI.Size.Medium;
                txtMachineName.IsEnabled = false;
                txtMachineName.Text = EditMachineDetails.MachineName;


                Binding machineNameWidthBinding = new Binding();
                machineNameWidthBinding.Source = leftGrid;
                machineNameWidthBinding.Path = new PropertyPath("ActualWidth");
                txtMachineName.SetBinding(CustomTextBox.MaxWidthProperty, machineNameWidthBinding);

                if (position == "Left")
                {
                    leftStackPanel.Children.Add(txtMachineName);
                    position = "Right";
                }
                else
                {
                    rightStackPanel.Children.Add(txtMachineName);
                    position = "Left";
                }

                if (editMachineAccess || editLimitedAccess)
                {
                    CustomTextBox txtSerialNumber = new CustomTextBox();
                    txtSerialNumber.Heading = MessageViewContainerList.GetMessage(executionContext, "Serial Number", null);
                    txtSerialNumber.Size = CommonUI.Size.Medium;
                    txtSerialNumber.Text = EditMachineDetails.SerialNumber;

                    Binding serialNumberWidthBinding = new Binding();
                    serialNumberWidthBinding.Source = leftGrid;
                    serialNumberWidthBinding.Path = new PropertyPath("ActualWidth");
                    txtSerialNumber.SetBinding(CustomTextBox.MaxWidthProperty, serialNumberWidthBinding);


                    if (editMachineAccess)
                    {
                        txtSerialNumber.IsEnabled = true;
                        Binding serialnumberBinding = new Binding();
                        serialnumberBinding.Source = EditMachineDetails;
                        serialnumberBinding.Path = new PropertyPath("SerialNumber");
                        serialnumberBinding.Mode = BindingMode.TwoWay;
                        serialnumberBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                        txtSerialNumber.SetBinding(CustomTextBox.TextProperty, serialnumberBinding);

                    }
                    else
                    {
                        txtSerialNumber.IsEnabled = false;
                    }

                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(txtSerialNumber);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(txtSerialNumber);
                        position = "Left";
                    }
                }


                if (editMachineAccess || editLimitedAccess)
                {
                    CustomTextBox txtMachineTag = new CustomTextBox();
                    txtMachineTag.Heading = "Machine Tag";
                    txtMachineTag.Size = CommonUI.Size.Medium;
                    txtMachineTag.Text = EditMachineDetails.MachineTag;

                    Binding machineTagWidthBinding = new Binding();
                    machineTagWidthBinding.Source = leftGrid;
                    machineTagWidthBinding.Path = new PropertyPath("ActualWidth");
                    txtMachineTag.SetBinding(CustomTextBox.MaxWidthProperty, machineTagWidthBinding);

                    if (editMachineAccess)
                    {
                        txtMachineTag.IsEnabled = true;
                        Binding machinetagBinding = new Binding();
                        machinetagBinding.Source = EditMachineDetails;
                        machinetagBinding.Path = new PropertyPath("MachineTag");
                        machinetagBinding.Mode = BindingMode.TwoWay;
                        machinetagBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                        txtMachineTag.SetBinding(CustomTextBox.TextProperty, machinetagBinding);
                    }
                    else
                    {
                        txtMachineTag.IsEnabled = false;
                    }

                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(txtMachineTag);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(txtMachineTag);
                        position = "Left";
                    }
                }

                CustomTextBox txtMacAddress = new CustomTextBox();
                txtMacAddress.Heading = MessageViewContainerList.GetMessage(executionContext, "MAC Address");
                txtMacAddress.Size = CommonUI.Size.Medium;
                txtMacAddress.Text = EditMachineDetails.MacAddress;

                Binding macAddressWidthBinding = new Binding();
                macAddressWidthBinding.Source = leftGrid;
                macAddressWidthBinding.Path = new PropertyPath("ActualWidth");
                txtMacAddress.SetBinding(CustomTextBox.MaxWidthProperty, macAddressWidthBinding);

                if (editMachineAccess)
                {
                    txtMacAddress.IsEnabled = true;
                    Binding macaddressBinding = new Binding();
                    macaddressBinding.Source = EditMachineDetails;
                    macaddressBinding.Path = new PropertyPath("MacAddress");
                    macaddressBinding.Mode = BindingMode.TwoWay;
                    macaddressBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    txtMacAddress.SetBinding(CustomTextBox.TextProperty, macaddressBinding);
                }
                else
                {
                    txtMacAddress.IsEnabled = false;
                }
                if (position == "Left")
                {
                    leftStackPanel.Children.Add(txtMacAddress);
                    position = "Right";
                }
                else
                {
                    rightStackPanel.Children.Add(txtMacAddress);
                    position = "Left";
                }


                if (editMachineAccess || editConfigAccess)
                {
                    MachineStatus.Clear();
                    foreach (LookupValuesContainerDTO lookupValues in gameMachineStatus.LookupValuesContainerDTOList)
                    {
                        MachineStatus.Add(lookupValues);
                    }
                    CustomComboBox cmbMachineStatus = new CustomComboBox();
                    cmbMachineStatus.Heading = MessageViewContainerList.GetMessage(executionContext, "Machine Status", null);
                    cmbMachineStatus.Size = CommonUI.Size.Medium;
                    cmbMachineStatus.IsEditable = true;

                    Binding machineStatusWidthBinding = new Binding();
                    machineStatusWidthBinding.Source = leftGrid;
                    machineStatusWidthBinding.Path = new PropertyPath("ActualWidth");
                    cmbMachineStatus.SetBinding(CustomTextBox.MaxWidthProperty, machineStatusWidthBinding);

                    Binding machineStatusBinding = new Binding("MachineStatus");
                    machineStatusBinding.Source = this;
                    BindingOperations.SetBinding(cmbMachineStatus, ItemsControl.ItemsSourceProperty, machineStatusBinding);
                    cmbMachineStatus.SetBinding(CustomComboBox.ItemsSourceProperty, machineStatusBinding);

                    cmbMachineStatus.DisplayMemberPath = "Description";

                    SelectedMachineStatus = MachineStatus.Where(x => x.Description == gameMachineStatus.LookupValuesContainerDTOList.Where(y => y.LookupValue == EditMachineDetails.IsActive).FirstOrDefault().Description).FirstOrDefault();
                    Binding selectedmachineStatusBinding = new Binding("SelectedMachineStatus");
                    selectedmachineStatusBinding.Source = this;
                    BindingOperations.SetBinding(cmbMachineStatus, System.Windows.Controls.Primitives.Selector.SelectedItemProperty, selectedmachineStatusBinding);

                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(cmbMachineStatus);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(cmbMachineStatus);
                        position = "Left";
                    }
                }
                else
                {
                    CustomTextBox txtMachineStatus = new CustomTextBox();
                    txtMachineStatus.Heading = MessageViewContainerList.GetMessage(executionContext, "Machine Status");
                    txtMachineStatus.Size = CommonUI.Size.Medium;
                    txtMachineStatus.IsEnabled = false;
                    txtMachineStatus.Text = gameMachineStatus.LookupValuesContainerDTOList.Where(x => x.LookupValue == EditMachineDetails.IsActive).FirstOrDefault().Description;

                    Binding machineStatusWidthBinding = new Binding();
                    machineStatusWidthBinding.Source = leftGrid;
                    machineStatusWidthBinding.Path = new PropertyPath("ActualWidth");
                    txtMachineStatus.SetBinding(CustomTextBox.MaxWidthProperty, machineStatusWidthBinding);

                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(txtMachineStatus);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(txtMachineStatus);
                        position = "Left";
                    }
                }

                if (EditMachineDetails.MasterId != -1)
                {
                    SelectedHubName = hubNames.Where(x => x.MasterId == EditMachineDetails.MasterId).FirstOrDefault();
                }

                if (editMachineAccess || editLimitedAccess)
                {
                    CustomComboBox cmbAccessPoint = new CustomComboBox();
                    cmbAccessPoint.Heading = MessageViewContainerList.GetMessage(executionContext, "Access Point");
                    cmbAccessPoint.Size = CommonUI.Size.Medium;
                    cmbAccessPoint.IsEditable = true;
                    cmbAccessPoint.ItemsSource = hubNames;
                    cmbAccessPoint.DisplayMemberPath = "HubNameWithMachineCount";

                    Binding accessPointWidthBinding = new Binding();
                    accessPointWidthBinding.Source = leftGrid;
                    accessPointWidthBinding.Path = new PropertyPath("ActualWidth");
                    cmbAccessPoint.SetBinding(CustomTextBox.MaxWidthProperty, accessPointWidthBinding);

                    Binding selectedaccesspointBinding = new Binding("SelectedHubName");
                    selectedaccesspointBinding.Source = this;
                    BindingOperations.SetBinding(cmbAccessPoint, System.Windows.Controls.Primitives.Selector.SelectedItemProperty, selectedaccesspointBinding);

                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(cmbAccessPoint);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(cmbAccessPoint);
                        position = "Left";
                    }
                }

                if (editMachineAccess || editLimitedAccess)
                {
                    CustomComboBox cmbTheme = new CustomComboBox();
                    cmbTheme.Heading = MessageViewContainerList.GetMessage(executionContext, "Theme Number");
                    cmbTheme.Size = CommonUI.Size.Medium;
                    cmbTheme.IsEditable = true;
                    cmbTheme.ItemsSource = ThemeNames;
                    cmbTheme.DisplayMemberPath = "ThemeNameWithThemeNumber";

                    Binding themetWidthBinding = new Binding();
                    themetWidthBinding.Source = leftGrid;
                    themetWidthBinding.Path = new PropertyPath("ActualWidth");
                    cmbTheme.SetBinding(CustomTextBox.MaxWidthProperty, themetWidthBinding);

                    if (EditMachineDetails.ThemeId != -1)
                    {
                        //SelectedThemeName = ThemeNames.Where(x => x.Id == EditMachineDetails.ThemeId).FirstOrDefault();
                        SelectedThemeName = ThemeNames.FirstOrDefault(x => x.Id == EditMachineDetails.ThemeId);
                    }

                    Binding selectedthemeBinding = new Binding("SelectedThemeName");
                    selectedthemeBinding.Source = this;
                    BindingOperations.SetBinding(cmbTheme, System.Windows.Controls.Primitives.Selector.SelectedItemProperty, selectedthemeBinding);

                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(cmbTheme);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(cmbTheme);
                        position = "Left";
                    };
                }

                if (editMachineAccess)
                {
                    CustomComboBox cmbReferenceMachine = new CustomComboBox();
                    cmbReferenceMachine.Heading = MessageViewContainerList.GetMessage(executionContext, "Reference Machine");
                    cmbReferenceMachine.Size = CommonUI.Size.Medium;
                    cmbReferenceMachine.IsEditable = true;
                    cmbReferenceMachine.ItemsSource = RefMachineNames;
                    cmbReferenceMachine.DisplayMemberPath = "MachineName";

                    Binding referenceMachineWidthBinding = new Binding();
                    referenceMachineWidthBinding.Source = leftGrid;
                    referenceMachineWidthBinding.Path = new PropertyPath("ActualWidth");
                    cmbReferenceMachine.SetBinding(CustomTextBox.MaxWidthProperty, referenceMachineWidthBinding);

                    if (EditMachineDetails.ReferenceMachineId != -1)
                    {
                        SelectedRefMachineName = RefMachineNames.Where(x => x.MachineId == EditMachineDetails.ReferenceMachineId).FirstOrDefault();
                    }
                    Binding selectedrefmachineBinding = new Binding("SelectedRefMachineName");
                    selectedrefmachineBinding.Source = this;
                    BindingOperations.SetBinding(cmbReferenceMachine, System.Windows.Controls.Primitives.Selector.SelectedItemProperty, selectedrefmachineBinding);

                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(cmbReferenceMachine);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(cmbReferenceMachine);
                        position = "Left";
                    };
                }
                else
                {
                    CustomTextBox txtReferenceMachine = new CustomTextBox();
                    txtReferenceMachine.Heading = MessageViewContainerList.GetMessage(executionContext, "Reference Machine");
                    txtReferenceMachine.Size = CommonUI.Size.Medium;
                    txtReferenceMachine.IsEnabled = false;


                    Binding referenceMachineWidthBinding = new Binding();
                    referenceMachineWidthBinding.Source = leftGrid;
                    referenceMachineWidthBinding.Path = new PropertyPath("ActualWidth");
                    txtReferenceMachine.SetBinding(CustomTextBox.MaxWidthProperty, referenceMachineWidthBinding);

                    if (EditMachineDetails.ReferenceMachineId != -1)
                    {
                        txtReferenceMachine.Text = RefMachineNames.Where(x => x.MachineId == EditMachineDetails.ReferenceMachineId).FirstOrDefault().MachineName;
                    }
                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(txtReferenceMachine);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(txtReferenceMachine);
                        position = "Left";
                    };
                }

                if (editMachineAccess || editConfigAccess)
                {
                    CustomTextBox txtPulseCount = new CustomTextBox();
                    txtPulseCount.Heading = MessageViewContainerList.GetMessage(executionContext, "Pulse Count");
                    txtPulseCount.Size = CommonUI.Size.Medium;
                    txtPulseCount.IsEnabled = true;
                    txtPulseCount.ValidationType = ValidationType.NumberOnly;

                    Binding pulseCountWidthBinding = new Binding();
                    pulseCountWidthBinding.Source = leftGrid;
                    pulseCountWidthBinding.Path = new PropertyPath("ActualWidth");
                    txtPulseCount.SetBinding(CustomTextBox.MaxWidthProperty, pulseCountWidthBinding);


                    Binding pulsecountBinding = new Binding();
                    pulsecountBinding.Source = EditMachineDetails;
                    pulsecountBinding.Path = new PropertyPath("NumberOfCoins");
                    pulsecountBinding.Mode = BindingMode.TwoWay;
                    pulsecountBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    txtPulseCount.SetBinding(CustomTextBox.TextProperty, pulsecountBinding);
                    txtPulseCount.Text = EditMachineDetails.NumberOfCoins.ToString() == "-1" ? "1" : EditMachineDetails.NumberOfCoins.ToString();


                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(txtPulseCount);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(txtPulseCount);
                        position = "Left";
                    };
                }

                if (editMachineAccess || editLimitedAccess)
                {
                    CustomCheckBox cbxReverseDisplay = new CustomCheckBox();
                    cbxReverseDisplay.Heading = MessageViewContainerList.GetMessage(executionContext, "Reverse Display");
                    cbxReverseDisplay.Size = CommonUI.Size.Medium;
                    cbxReverseDisplay.IsEnabled = true;
                    cbxReverseDisplay.IsChecked = EditMachineDetails.GameMachineAttributes.Where(x => x.AttributeName.ToString() == "REVERSE_DISPLAY_DIRECTION").FirstOrDefault().AttributeValue == "1" ? true : false;

                    if (!EditMachineDetails.GameMachineAttributes.Any(x => x.AttributeName.ToString() == "REVERSE_DISPLAY_DIRECTION" && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                    {
                        MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION, EditMachineDetails.GameMachineAttributes.Where(x => x.AttributeName.ToString() == "REVERSE_DISPLAY_DIRECTION").FirstOrDefault().AttributeValue, "Y", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, executionContext.GetSiteId(), executionContext.GetUserId(), DateTime.Now, -1, executionContext.GetUserId(), DateTime.Now);
                        EditMachineDetails.GameMachineAttributes.Add(updatedAttribute);
                        EditMachineDetails.IsChanged = true;
                    }

                    Binding cbxReverseDisplayBinding = new Binding();
                    cbxReverseDisplayBinding.Source = EditMachineDetails.GameMachineAttributes.Where(x => x.AttributeName.ToString() == "REVERSE_DISPLAY_DIRECTION" && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).FirstOrDefault();
                    cbxReverseDisplayBinding.Path = new PropertyPath("AttributeValue");
                    cbxReverseDisplayBinding.Converter = new StringtoBoolConverter();
                    cbxReverseDisplayBinding.Mode = BindingMode.TwoWay;
                    cbxReverseDisplayBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                    cbxReverseDisplay.SetBinding(CustomCheckBox.IsCheckedProperty, cbxReverseDisplayBinding);

                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(cbxReverseDisplay);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(cbxReverseDisplay);
                        position = "Left";
                    }
                }

                if (editConfigAccess)
                {
                    // add ticket mode setting
                    TicketModes = new ObservableCollection<string>()
                    {
                        MessageViewContainerList.GetMessage(executionContext,MachineDTO.TICKETMODE.DEFAULT.ToString()),
                        MessageViewContainerList.GetMessage(executionContext,MachineDTO.TICKETMODE.PHYSICAL.ToString()),
                        MessageViewContainerList.GetMessage(executionContext,MachineDTO.TICKETMODE.ETICKET.ToString())
                    };
                    CustomComboBox cmbTicketMode = new CustomComboBox();
                    cmbTicketMode.Heading = MessageViewContainerList.GetMessage(executionContext, "Ticket Mode");
                    cmbTicketMode.Size = CommonUI.Size.Medium;
                    cmbTicketMode.IsEditable = true;
                    cmbTicketMode.ItemsSource = TicketModes;
              

                    Binding ticketModeWidthBinding = new Binding();
                    ticketModeWidthBinding.Source = leftGrid;
                    ticketModeWidthBinding.Path = new PropertyPath("ActualWidth");
                    cmbTicketMode.SetBinding(CustomTextBox.MaxWidthProperty, ticketModeWidthBinding);

                    if (EditMachineDetails.TicketMode != null)
                    {
                        if (EditMachineDetails.TicketMode == "D")
                        {
                            SelectedTicketMode = MessageViewContainerList.GetMessage(executionContext,"DEFAULT");
                        }
                        else if (EditMachineDetails.TicketMode == "P")
                        {
                            SelectedTicketMode = MessageViewContainerList.GetMessage(executionContext,"PHYSICAL");
                        }
                        else if (EditMachineDetails.TicketMode == "E")
                        {
                            SelectedTicketMode = MessageViewContainerList.GetMessage(executionContext,"ETICKET");
                        }
                    }
                    Binding selectedticketmodeBinding = new Binding("SelectedTicketMode");
                    selectedticketmodeBinding.Source = this;
                    BindingOperations.SetBinding(cmbTicketMode, System.Windows.Controls.Primitives.Selector.SelectedItemProperty, selectedticketmodeBinding);

                    if (position == "Left")
                    {
                        leftStackPanel.Children.Add(cmbTicketMode);
                        position = "Right";
                    }
                    else
                    {
                        rightStackPanel.Children.Add(cmbTicketMode);
                        position = "Left";
                    };

                    // add dynamic attributes
                    ReaderConfigurationContainerDTOCollection systemAttributes = ReaderConfigurationViewContainerList.GetReaderConfigurationContainerDTOCollection(executionContext.GetSiteId(), null, false);
                    log.LogVariableState("systemAttributes", systemAttributes);

                    foreach (LookupValuesContainerDTO l in gameMachineAttributes.LookupValuesContainerDTOList.Where(x => x.Description == "Y" &&
                    x.LookupValue != "NUMBER_OF_COINS").OrderBy(x => x.LookupValue).ToList())
                    {
                        if (systemAttributes.ReaderConfigurationContainerDTOList.Where(x => x.AttributeName.ToString() == l.LookupValue).FirstOrDefault().IsFlag == "Y")
                        {
                            string addField = "N";
                            if (l.LookupName == "REVERSE_DISPLAY_DIRECTION")
                            {
                                if (!editLimitedAccess && !editMachineAccess)
                                {
                                    addField = "Y";
                                }
                                else
                                {
                                    addField = "N";
                                }
                            }
                            else
                            {
                                addField = "Y";
                            }
                            if (addField == "Y")
                            {
                                CustomCheckBox checkBox = new CustomCheckBox();
                                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                                checkBox.Name = "cbx" + l.LookupValue.ToString();
                                string[] s = l.LookupValue.ToString().Split('_');
                                string heading = string.Empty;
                                for (int i = 0; i <= s.Length - 1; i++)
                                {
                                    s[i] = s[i].ToLower();
                                    heading += " " + ti.ToTitleCase(s[i]);
                                }

                                checkBox.Heading = MessageViewContainerList.GetMessage(executionContext, heading);
                                checkBox.Size = CommonUI.Size.Medium;

                                if (!EditMachineDetails.GameMachineAttributes.Any(x => x.AttributeName.ToString() == l.LookupValue.ToString() && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                                {
                                    MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, systemAttributes.ReaderConfigurationContainerDTOList.Where(x => x.AttributeName.ToString() == l.LookupValue).FirstOrDefault().AttributeName, EditMachineDetails.GameMachineAttributes.Where(x => x.AttributeName.ToString() == l.LookupValue).FirstOrDefault().AttributeValue, "Y", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, executionContext.GetSiteId(), executionContext.GetUserId(), DateTime.Now, -1, executionContext.GetUserId(), DateTime.Now);

                                    EditMachineDetails.GameMachineAttributes.Add(updatedAttribute);
                                    EditMachineDetails.IsChanged = true;
                                }

                                Binding b = new Binding();
                                b.Source = EditMachineDetails.GameMachineAttributes.Where(x => x.AttributeName.ToString() == l.LookupValue.ToString() && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).FirstOrDefault();
                                b.Path = new PropertyPath("AttributeValue");
                                b.Converter = new StringtoBoolConverter();
                                b.Mode = BindingMode.TwoWay;
                                b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                                checkBox.SetBinding(CustomCheckBox.IsCheckedProperty, b);

                                if (position == "Left")
                                {
                                    leftStackPanel.Children.Add(checkBox);
                                    position = "Right";
                                }
                                else
                                {
                                    rightStackPanel.Children.Add(checkBox);
                                    position = "Left";

                                }
                            }
                        }
                        else
                        {
                            CustomTextBox customTextBox = new CustomTextBox();
                            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                            string txtFieldName = "txt" + l.LookupValue.ToString();
                            customTextBox.Name = txtFieldName;
                            string[] lookupCollection = l.LookupValue.ToString().Split('_');
                            string heading = string.Empty;
                            for (int i = 0; i <= lookupCollection.Length - 1; i++)
                            {
                                lookupCollection[i] = lookupCollection[i].ToLower();
                                heading += " " + textInfo.ToTitleCase(lookupCollection[i]);
                            }
                            customTextBox.Heading = MessageViewContainerList.GetMessage(executionContext, heading);
                            customTextBox.Size = CommonUI.Size.Medium;
                            customTextBox.IsEnabled = true;
                            customTextBox.ValidationType = ValidationType.NumberOnly;

                            Binding wb = new Binding();
                            wb.Source = leftGrid;
                            wb.Path = new PropertyPath("ActualWidth");
                            customTextBox.SetBinding(CustomTextBox.MaxWidthProperty, wb);

                            if (!EditMachineDetails.GameMachineAttributes.Any(x => x.AttributeName.ToString() == l.LookupValue.ToString() && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                            {
                                MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, systemAttributes.ReaderConfigurationContainerDTOList.Where(x => x.AttributeName.ToString() == l.LookupValue).FirstOrDefault().AttributeName, EditMachineDetails.GameMachineAttributes.Where(x => x.AttributeName.ToString() == l.LookupValue.ToString()).FirstOrDefault().AttributeValue, "N", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, executionContext.GetSiteId(), executionContext.GetUserId(), DateTime.Now, -1, executionContext.GetUserId(), DateTime.Now);
                                EditMachineDetails.GameMachineAttributes.Add(updatedAttribute);
                                EditMachineDetails.IsChanged = true;
                            }

                            Binding b = new Binding();
                            b.Source = EditMachineDetails.GameMachineAttributes.Where(x => x.AttributeName.ToString() == l.LookupValue.ToString() && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).FirstOrDefault();
                            b.Path = new PropertyPath("AttributeValue");
                            b.Mode = BindingMode.TwoWay;
                            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                            customTextBox.SetBinding(CustomTextBox.TextProperty, b);

                            if (position == "Left")
                            {
                                leftStackPanel.Children.Add(customTextBox);
                                position = "Right";
                            }
                            else
                            {
                                rightStackPanel.Children.Add(customTextBox);
                                position = "Left";
                            }
                        }
                    }
                }

                // added checkbox for Erase QRPlay - prashanth
                CustomCheckBox cbxEraseQRPlayIdentifier = new CustomCheckBox();
                cbxEraseQRPlayIdentifier.Heading = MessageViewContainerList.GetMessage(executionContext, "Erase QR Play Identifier");
                cbxEraseQRPlayIdentifier.Size = CommonUI.Size.Medium;
                cbxEraseQRPlayIdentifier.IsEnabled = true;
                cbxEraseQRPlayIdentifier.IsChecked = EditMachineDetails.EraseQRPlayIdentifier ? true : false;

                Binding cbxEraseQRPlayIdentifierBinding = new Binding();
                cbxEraseQRPlayIdentifierBinding.Source = EditMachineDetails;
                cbxEraseQRPlayIdentifierBinding.Path = new PropertyPath("EraseQRPlayIdentifier");
                cbxEraseQRPlayIdentifierBinding.Mode = BindingMode.TwoWay;
                cbxEraseQRPlayIdentifierBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                cbxEraseQRPlayIdentifier.SetBinding(CustomCheckBox.IsCheckedProperty, cbxEraseQRPlayIdentifierBinding);

                if (position == "Left")
                {
                    leftStackPanel.Children.Add(cbxEraseQRPlayIdentifier);
                    position = "Right";
                }
                else
                {
                    rightStackPanel.Children.Add(cbxEraseQRPlayIdentifier);
                    position = "Left";
                }
                //

                leftGrid.Children.Add(leftStackPanel);
                rightGrid.Children.Add(rightStackPanel);


                Grid.SetColumn(leftGrid, 0);
                Grid.SetColumn(rightGrid, 1);
                grid.Children.Add(leftGrid);
                grid.Children.Add(rightGrid);



                log.LogMethodExit(grid);
                return grid;
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                this.SuccessMessage = ex.Message.ToString();
                throw;
            };

        }

        public ExecutionContext GetExecutionContext()
        {
            log.LogMethodEntry();
            log.LogMethodExit(executionContext);
            return executionContext;
        }

        private void CancelEditMachine(object param)
        {
            log.LogMethodEntry(param);
            log.Info("Edit machine page is closed");
            if (param != null)
            {
                editMachineView = param as GMEditMachineView;
                if (editMachineView != null)
                {
                    SuccessMessage = String.Empty;
                    editMachineView.Close();
                }
            }
            log.LogMethodExit();
        }

        private async void EditMachine(object param)
        {
            log.LogMethodEntry(param);

            try
            {
                log.Info("Edit machine button is selected for machine id" + EditMachineDetails.MachineId);

                String ErrorMessage = String.Empty;
                editMachineView = param as GMEditMachineView;

                IsLoadingVisible = true;

                if (SelectedHubName == null)
                {
                    ErrorMessage = MessageViewContainerList.GetMessage(executionContext, 2795, null);
                }

                //  Add edit machine details
                if (EditMachineDetails.NumberOfCoins.ToString() == string.Empty)
                {
                    EditMachineDetails.NumberOfCoins = 1;

                }
                if (EditMachineDetails.NumberOfCoins <= 0 || EditMachineDetails.NumberOfCoins > 10)
                {
                    EditMachineDetails.NumberOfCoins = 1;
                }

                if (!EditMachineDetails.GameMachineAttributes.Any(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                {
                    MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS, EditMachineDetails.NumberOfCoins.ToString(), "N", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, executionContext.GetSiteId(), executionContext.GetUserId(), DateTime.Now, -1, executionContext.GetUserId(), DateTime.Now);
                    EditMachineDetails.GameMachineAttributes.Add(updatedAttribute);
                    EditMachineDetails.IsChanged = true;
                }
                else
                {
                    EditMachineDetails.GameMachineAttributes.Where(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE).FirstOrDefault().AttributeValue = EditMachineDetails.NumberOfCoins.ToString();
                }

                if (ErrorMessage != String.Empty)
                {
                    ShowMessagePopup(MessageViewContainerList.GetMessage(executionContext, "Error"), MessageViewContainerList.GetMessage(executionContext, "Required fields"), ErrorMessage);
                    return;
                }
                EditMachineDetails.IsChanged = true;
                    List<MachineDTO> machines = new List<MachineDTO>();
                    machines.Add(EditMachineDetails);

                    log.LogVariableState("Edit machine details ", machines);
                    string result = await PostMachineAsync(executionContext, machines);

                    if (result == "Success")
                    {
                        IMachineUseCases iMachineUseCases = GameUseCaseFactory.GetMachineUseCases(executionContext);
                        List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchparams = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                        searchparams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID, EditMachineDetails.MachineId.ToString()));
                        List<MachineDTO> machineswithref = await iMachineUseCases.GetMachines(searchparams, true);
                        if (machineswithref != null && machineswithref.Any())
                        {
                            foreach (MachineDTO refmachines in machineswithref)
                            {
                                if (refmachines.IsActive != EditMachineDetails.IsActive)
                                {
                                    refmachines.IsActive = EditMachineDetails.IsActive;
                                    List<MachineDTO> rmachines = new List<MachineDTO>();
                                    rmachines.Add(refmachines);

                                    log.LogVariableState("Edit reference machine details ", rmachines);
                                    string rresult = await PostMachineAsync(executionContext, rmachines);
                                    if (rresult != "Success")
                                    {
                                        // dont close window. keep details user entered.
                                        ErrorMessage = MessageViewContainerList.GetMessage(executionContext, 2796, null);
                                        ShowMessagePopup(MessageViewContainerList.GetMessage(executionContext, "Failure"), MessageViewContainerList.GetMessage(executionContext, "Try Again!"), ErrorMessage);
                                    }
                                }
                            }
                        }
                        ErrorMessage = MessageViewContainerList.GetMessage(executionContext, 2797, null);
                        ShowMessagePopup(MessageViewContainerList.GetMessage(executionContext, "Success"), MessageViewContainerList.GetMessage(executionContext, "Machine Updated"), ErrorMessage);
                        // should add to ref machine list so that when come from game management again LOV reflects newly added machine
                        IsLoadingVisible = false;
                    }
                    else
                    {
                        IsLoadingVisible = false;
                        // dont close window. keep details user entered.
                        ErrorMessage = MessageViewContainerList.GetMessage(executionContext, 2796);
                        ShowMessagePopup(MessageViewContainerList.GetMessage(executionContext, "Failure"), MessageViewContainerList.GetMessage(executionContext, "Try Again!"), ErrorMessage);
                    }
                    log.LogMethodExit();
            }
            catch (UnauthorizedException ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                throw;
            }
            catch (ValidationException vex)
            {
                IsLoadingVisible = false;
                log.Error(vex);
                SetFooterContent(MessageViewContainerList.GetMessage(executionContext, 2796, null) + "-" + vex.Message.ToString(), MessageType.Error);
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(executionContext, 2796, null) + "-" + ex.Message.ToString(), MessageType.Error);
            };
        }

        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);

            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = editMachineView;
            messagePopupView.DataContext = new GenericMessagePopupVM(executionContext)
            {
                Heading = heading,
                SubHeading = subHeading,
                Content = content,
                CancelButtonText = MessageViewContainerList.GetMessage(executionContext, "OK"),
                TimerMilliSeconds = 5000,
                PopupType = PopupType.Timer,
            };

            messagePopupView.ShowDialog();

            GenericMessagePopupVM messagePopupVM = messagePopupView.DataContext as GenericMessagePopupVM;

            if (messagePopupVM != null && messagePopupVM.Heading.ToLower() == "Success".ToLower())
            {
                SuccessMessage = messagePopupVM.Content;
                if (editMachineView != null)
                {
                    editMachineView.Close();
                }
            }
            log.LogMethodExit();
        }
        private async Task<string> PostMachineAsync(ExecutionContext executionContext, List<MachineDTO> machinesList)
        {
            log.LogMethodEntry(executionContext, machinesList);
            IMachineUseCases iMachineUseCases = GameUseCaseFactory.GetMachineUseCases(executionContext);
            string result = await iMachineUseCases.SaveMachines(machinesList);
            log.LogMethodExit(result);
            return result;
        }
        #endregion Methods

        //#endregion Methods

        #region Commands
        public ICommand EditMachineCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(editMachineCommand);
                return editMachineCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                editMachineCommand = value;
            }
        }

        public ICommand CancelMachineCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(cancelMachineCommand);
                return cancelMachineCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                cancelMachineCommand = value;
            }
        }


        #endregion Commands
    }
}

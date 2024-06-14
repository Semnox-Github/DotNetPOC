/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Game Management Add Machine View Model
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
using System.Linq;
using System.Windows.Input;
using Semnox.Parafait.DigitalSignage;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.ViewContainer;
using System.Threading.Tasks;
using System.Windows;
namespace Semnox.Parafait.GamesUI
{

    public class GMAddMachineVM : BaseWindowViewModel
    {
        #region Members
        private StatusCode statusCode;
        private GMAddMachineView addMachineView;
        private string message;
        private string pulseCount;
        private bool reverseDisplay;
        private string machineDisplayMemberPath;

        private ObservableCollection<HubDTO> hubsList;
        private ObservableCollection<GameContainerDTO> gamesList;
        private ObservableCollection<ThemeContainerDTO> ThemeList;
        private ObservableCollection<MachineDTO> machineList;
        private List<string> selectedCollection;
        private string addMachineName;

        private ObservableCollection<AllowedMachineNamesDTO> allowedMachineName;//added
        private ObservableCollection<KeyValuePair<int, string>> allowedMachineNameKeyValuePairs;
        private KeyValuePair<int, string> selectedAllowedMachineNameKeyValuePair;


        private ObservableCollection<HubDTO> hubNames;
        private ObservableCollection<GameContainerDTO> gameNames;
        private ObservableCollection<ThemeContainerDTO> themeNames;
        private ObservableCollection<MachineDTO> refMachineNames;
        private ObservableCollection<string> ticketModes;

        private HubDTO selectedHubName;
        private GameContainerDTO selectedGameName;
        private ThemeContainerDTO selectedThemeName;
        private MachineDTO selectedRefMachineName;
        private MachineDTO addMachineDetails;
        private AllowedMachineNamesDTO selectedMachineName;
        private string selectedTicketMode;

        private ICommand addMachineCommand;
        private ICommand cancelAddMachineCommand;
        private bool showComboBox;
        private bool manualMachineNameEntry;
        private bool machineNameEnabled;
        private bool allowedmachineNameExist;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Members

        #region properties
        public KeyValuePair<int, string> SelectedAllowedMachineNameKeyValuePair
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedAllowedMachineNameKeyValuePair);
                return selectedAllowedMachineNameKeyValuePair;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedAllowedMachineNameKeyValuePair, value);
                if (selectedAllowedMachineNameKeyValuePair.Value != null)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, string.Empty), MessageType.None);
                }
            }

        }
        public bool MachineNameEnabled
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(machineNameEnabled);
                return machineNameEnabled;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref machineNameEnabled, value);
            }
        }
        public List<string> SelectedCollection
        {
            get
            {
                return selectedCollection;
            }
            set
            {
                if (!Equals(selectedCollection, value))
                {
                    selectedCollection = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool ShowComboBox
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showComboBox);
                return showComboBox;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref showComboBox, value);
            }
        }
        public bool ManualMachineNameEntry
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(manualMachineNameEntry);
                return manualMachineNameEntry;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref manualMachineNameEntry, value);
            }
        }
        #endregion

        #region Constructor

        public GMAddMachineVM(ObservableCollection<MachineDTO> machinesList,
                               ObservableCollection<HubDTO> hubsList,
                               ObservableCollection<GameContainerDTO> gamesList,
                               ObservableCollection<ThemeContainerDTO> ThemeList,
                               bool showMachineNameWithGameName,
                              ExecutionContext executionContext)
        {
            log.Info("Add machine page is opened");
            log.LogMethodEntry(machinesList, hubsList, gamesList, ThemeList, showMachineNameWithGameName, ExecutionContext);

            ExecutionContext = executionContext;

            addMachineCommand = new DelegateCommand(AddMachineAsync);
            cancelAddMachineCommand = new DelegateCommand(CancelAddMachine);

            AddMachineDetails = new MachineDTO();
            AddMachineDetails.GameMachineAttributes = new List<MachineAttributeDTO>();

            this.hubsList = hubsList;
            this.gamesList = gamesList;
            this.ThemeList = ThemeList;
            this.machineList = machinesList;
            FooterVM = new FooterVM(ExecutionContext)
            {
                Message = string.Empty,
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Collapsed,
            };
            SelectedCollection = new List<string>() { "GameName", "GameTag" };
            HubNames = new ObservableCollection<HubDTO>(hubsList.Where(x => x.DirectMode == "Y" && x.IsActive == true));
            GameNames = new ObservableCollection<GameContainerDTO>(gamesList);
            ManualMachineNameEntry = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(ExecutionContext, "ENABLE_USER_ENTRY_OF_MACHINE");
            if (ManualMachineNameEntry == true)
            {
                MachineNameEnabled = false;
                ShowComboBox = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2800), MessageType.Info);
            }
            else
            {
                MachineNameEnabled = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2800), MessageType.Info);
            }
            if (machinesList != null)
            {
                RefMachineNames = new ObservableCollection<MachineDTO>(machinesList);
            }
            else
            {
                RefMachineNames = new ObservableCollection<MachineDTO>();
            }
            if (ThemeList != null)
            {
                ThemeNames = new ObservableCollection<ThemeContainerDTO>(ThemeList);
            }
            else
            {
                ThemeNames = new ObservableCollection<ThemeContainerDTO>();
            }
            TicketModes = new ObservableCollection<string>()
                    {
                        MessageViewContainerList.GetMessage(ExecutionContext,MachineDTO.TICKETMODE.DEFAULT.ToString()),
                        MessageViewContainerList.GetMessage(ExecutionContext,MachineDTO.TICKETMODE.PHYSICAL.ToString()),
                        MessageViewContainerList.GetMessage(ExecutionContext,MachineDTO.TICKETMODE.ETICKET.ToString())
                    };

            SelectedTicketMode = (ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "REAL_TICKET_MODE")).Equals("Y") ? MessageViewContainerList.GetMessage(ExecutionContext, MachineDTO.TICKETMODE.DEFAULT.ToString()) : MessageViewContainerList.GetMessage(ExecutionContext, MachineDTO.TICKETMODE.ETICKET.ToString());
            if (showMachineNameWithGameName)
            {
                MachineDisplayMemberPath = "MachineNameGameName";
            }
            else
            {
                MachineDisplayMemberPath = "MachineName";
            }

            log.LogMethodExit();
        }

        #endregion Construct

        #region Properties
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
                SetProperty(ref selectedTicketMode, value);
            }
        }
        public StatusCode StatusCode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(statusCode);
                return statusCode;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref statusCode, value);
                log.LogMethodEntry(statusCode);
            }
        }

        public string MachineDisplayMemberPath
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(machineDisplayMemberPath);
                return machineDisplayMemberPath;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref machineDisplayMemberPath, value);
            }
        }

        public MachineDTO AddMachineDetails
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addMachineDetails);
                return addMachineDetails;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref addMachineDetails, value);
            }
        }
        public string AddMachineName
        {
            get
            {
                return addMachineName;
            }
            set
            {
                SetProperty(ref addMachineName, value);
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
                SetProperty(ref message, value);
            }
        }

        public string PulseCount
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(pulseCount);
                return pulseCount;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref pulseCount, value);
            }
        }

        public bool ReverseDisplay
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(reverseDisplay);
                return reverseDisplay;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref reverseDisplay, value);
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
                SetProperty(ref hubNames, value);
            }
        }
        public ObservableCollection<AllowedMachineNamesDTO> AllowedMachineName//added
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(allowedMachineName);
                return allowedMachineName;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref allowedMachineName, value);
            }
        }
        public ObservableCollection<KeyValuePair<int, string>> AllowedMachineNameKeyValuePairs//added
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(allowedMachineNameKeyValuePairs);
                return allowedMachineNameKeyValuePairs;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref allowedMachineNameKeyValuePairs, value);
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
                SetProperty(ref selectedHubName, value);
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
                SetProperty(ref gameNames, value);
            }
        }

        public GameContainerDTO SelectedGameName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedGameName);
                return selectedGameName;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedGameName, value);
                AddMachineName = string.Empty;
                AllowedMachineNameKeyValuePairs = new ObservableCollection<KeyValuePair<int, string>>();
                List<KeyValuePair<int, string>> test;
                if (selectedGameName != null)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, string.Empty), MessageType.None);
                    Semnox.Parafait.Game.Game game = new Semnox.Parafait.Game.Game(SelectedGameName.GameId, ExecutionContext);
                    GameDTO gameDTO = game.GetGameDTO;
                    if (gameDTO != null)
                    {
                        if (ManualMachineNameEntry == true)
                        {
                            AddMachineName = gameDTO.GameName;
                        }
                    }
                    else
                    {
                        AddMachineName = string.Empty;
                    }
                    ShowComboBox = false;
                    MachineNameEnabled = true;
                    log.LogVariableState("Selected game:", selectedGameName);
                    List<AllowedMachineNamesDTO> allowedMachineNamesDTOList = GetAllowedMachineNames(selectedGameName.GameId);
                    if (allowedMachineNamesDTOList != null && allowedMachineNamesDTOList.Any())
                    {
                        string allowedMachineNameIdList = string.Join(",", allowedMachineNamesDTOList.Select(x => x.AllowedMachineId));
                        MachineList machineList = new MachineList(ExecutionContext);
                        List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> mSearchParams = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                        mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.GAME_ID, selectedGameName.GameId.ToString()));
                        mSearchParams.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.ALLOWED_MACHINE_ID_LIST, allowedMachineNameIdList));
                        List<MachineDTO> machineDTOList = machineList.GetMachineList(mSearchParams);
                        if (machineDTOList != null && machineDTOList.Any())
                        {
                            foreach (MachineDTO machineDTO in machineDTOList)
                            {
                                allowedMachineNamesDTOList.RemoveAll(x => x.AllowedMachineId == machineDTO.AllowedMachineID);
                            }
                        }
                    }
                   
                    if (allowedMachineNamesDTOList != null && allowedMachineNamesDTOList.Any())
                    {
                        foreach (AllowedMachineNamesDTO allowedMachineNamesDTO in allowedMachineNamesDTOList)
                        {
                            AllowedMachineNameKeyValuePairs.Add(new KeyValuePair<int, string>(allowedMachineNamesDTO.AllowedMachineId, allowedMachineNamesDTO.MachineName));
                            log.LogVariableState("AllowedMachines", AllowedMachineNameKeyValuePairs);
                        }
                    }
                    if (AllowedMachineNameKeyValuePairs != null && AllowedMachineNameKeyValuePairs.Count == 0)
                    {
                        AllowedMachineNameKeyValuePairs = null;
                    }
                    if (AllowedMachineNameKeyValuePairs != null && AllowedMachineNameKeyValuePairs.Any())
                    {
                        if (ManualMachineNameEntry == false)
                        {
                            ShowComboBox = true;
                            MachineNameEnabled = true;
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5033), MessageType.Info);//Please select the machine from the allowed machine name
                            return;
                        }
                    }
                    if (AllowedMachineNameKeyValuePairs == null && allowedmachineNameExist == false && ManualMachineNameEntry == false)
                    {
                        test = new List<KeyValuePair<int, string>>();
                        AllowedMachineNameKeyValuePairs = new ObservableCollection<KeyValuePair<int, string>>(test);
                        MachineNameEnabled = false;
                        ShowComboBox = false;
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5006, SelectedGameName.GameName), MessageType.Warning);//Allowed machine names doesn''t exist,please check the set up.
                        return;
                    }
                    if (AllowedMachineNameKeyValuePairs == null && allowedmachineNameExist == true && ManualMachineNameEntry == false)
                    {
                        test = new List<KeyValuePair<int, string>>();
                        AllowedMachineNameKeyValuePairs = new ObservableCollection<KeyValuePair<int, string>>(test);
                        ShowComboBox = true;
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 5007), MessageType.Info);
                        return;
                    }
                    if (AllowedMachineNameKeyValuePairs == null && ManualMachineNameEntry == true)
                    {
                        ShowComboBox = false;
                        MachineNameEnabled = true;
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, string.Empty), MessageType.None);
                        return;

                    }


                }
            }
        }

        public ObservableCollection<ThemeContainerDTO> ThemeNames
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(themeNames);
                return themeNames;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref themeNames, value);
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
                SetProperty(ref selectedThemeName, value);
            }
        }

        public ObservableCollection<MachineDTO> RefMachineNames
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(refMachineNames);
                return refMachineNames;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref refMachineNames, value);
            }
        }
        public AllowedMachineNamesDTO SelectedMachineName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedMachineName);
                return selectedMachineName;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref selectedMachineName, value);
                if (selectedMachineName != null)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, string.Empty), MessageType.None);
                }
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
                SetProperty(ref selectedRefMachineName, value);
            }
        }


        #endregion Properties

        #region Methods
        private void SetLoadingVisible(bool isVisible)
        {
            log.LogMethodEntry(isVisible);
            if ( !Equals(IsLoadingVisible, isVisible))
            {
                IsLoadingVisible = isVisible;
                RaiseCanExecuteChanged();
            }
            log.LogMethodExit();
        }
        public void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (CancelMachineCommand as DelegateCommand).RaiseCanExecuteChanged();
            (AddMachineCommand as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();

        }
        private async void AddMachineAsync(object param)
        {
            log.LogMethodEntry(param);
            if (SelectedAllowedMachineNameKeyValuePair.Value != null && ManualMachineNameEntry == false)//added
            {
                AddMachineName = SelectedAllowedMachineNameKeyValuePair.Value;
                addMachineDetails.AllowedMachineID = SelectedAllowedMachineNameKeyValuePair.Key;
            }
            if (AddMachineName != null && ManualMachineNameEntry == true)
            {
                AddMachineName = AddMachineName;
                AllowedMachineNamesListBL allowedMachineNamesListBL = new AllowedMachineNamesListBL(ExecutionContext);
                List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>> allowedMachinesearchParameters = new List<KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>>();
                allowedMachinesearchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.SITE_ID, ExecutionContext.GetSiteId().ToString()));
                allowedMachinesearchParameters.Add(new KeyValuePair<AllowedMachineNamesDTO.SearchByParameters, string>(AllowedMachineNamesDTO.SearchByParameters.MACHINE_NAME, AddMachineName));
                AllowedMachineNamesDTO allowedMachineNamesDTO = allowedMachineNamesListBL.GetAllowedMachineNamesList(allowedMachinesearchParameters).FirstOrDefault();
                if (allowedMachineNamesDTO != null)
                {
                    addMachineDetails.AllowedMachineID = allowedMachineNamesDTO.AllowedMachineId;
                }
                else
                {
                    addMachineDetails.AllowedMachineID = -1;
                }
            }
            log.Info("Adding new machine ok button clicked for " + AddMachineName);
            try
            {
                addMachineView = param as GMAddMachineView;

                SetLoadingVisible(true);
                //IsLoadingVisible = true;

                String ErrorMessage = String.Empty;

                // Validations
                if (AddMachineDetails != null)
                {
                    if (AddMachineName == null || string.IsNullOrWhiteSpace(AddMachineName))
                    {
                        ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2798, null);
                    }
                    else
                    {
                        if (AddMachineName.Trim() == String.Empty)
                        {
                            ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2798, null);
                        }
                        else
                        {
                            // if machine already exists, throw error
                            if (this.machineList.Any(x => x.MachineName == AddMachineName))
                            {
                                ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2799, AddMachineName);
                            }

                            else
                            {
                                if (SelectedGameName == null)
                                {
                                    ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2800, null);
                                }
                                else
                                {
                                    if (SelectedHubName == null)
                                    {
                                        ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2795, null);
                                    }
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(AddMachineName))
                    {
                        AddMachineDetails.MachineName = AddMachineName.Trim();

                    }
                    //  Add new machine details
                    try
                    {
                        if (PulseCount == string.Empty)
                        {
                            PulseCount = "1";
                        }

                        int numCoins = Convert.ToInt32(PulseCount);
                        if (numCoins <= 0 || numCoins > 10)
                        {
                            numCoins = 1;
                        }
                        AddMachineDetails.NumberOfCoins = numCoins;
                    }
                    catch (FormatException)
                    {
                        ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, "Invalid format for pulse count");
                        return;
                    }

                    if (ErrorMessage != String.Empty)
                    {
                        ShowMessagePopup(MessageViewContainerList.GetMessage(ExecutionContext, "Error"), MessageViewContainerList.GetMessage(ExecutionContext, "Required fields"), ErrorMessage);
                        SetLoadingVisible(false);
                        return;
                    }

                    if (SelectedTicketMode != null) // added by prashanth
                    {
                        if (SelectedTicketMode.Equals(MessageViewContainerList.GetMessage(ExecutionContext, MachineDTO.TICKETMODE.DEFAULT.ToString())))
                        {
                            AddMachineDetails.TicketMode = "D";
                        }
                        else if (SelectedTicketMode.Equals(MessageViewContainerList.GetMessage(ExecutionContext, MachineDTO.TICKETMODE.PHYSICAL.ToString())))
                        {
                            AddMachineDetails.TicketMode = "P";
                        }
                        else
                        {
                            AddMachineDetails.TicketMode = "E";
                        }
                    }

                    if (SelectedHubName != null)
                    {
                        AddMachineDetails.MasterId = SelectedHubName.MasterId;
                    }
                    if (SelectedGameName != null)
                    {
                        AddMachineDetails.GameId = SelectedGameName.GameId;
                    }

                    if (SelectedThemeName != null)
                    {
                        AddMachineDetails.ThemeId = SelectedThemeName.Id;
                    }
                    if (SelectedRefMachineName != null)
                    {
                        AddMachineDetails.ReferenceMachineId = SelectedRefMachineName.MachineId;
                    }

                    AddMachineDetails.MachineId = -1;
                    AddMachineDetails.IsActive = "Y";
                    AddMachineDetails.TicketAllowed = "Y";
                    AddMachineDetails.TimerMachine = "N";
                    AddMachineDetails.GroupTimer = "N";
                    AddMachineDetails.ShowAd = "D";
                    AddMachineDetails.IsChanged = true;

                    MachineAttributeDTO machattrnumcoin = new MachineAttributeDTO(MachineAttributeDTO.MachineAttribute.NUMBER_OF_COINS, AddMachineDetails.NumberOfCoins.ToString(), MachineAttributeDTO.AttributeContext.MACHINE);
                    MachineAttributeDTO machattrrevdisp = new MachineAttributeDTO(MachineAttributeDTO.MachineAttribute.REVERSE_DISPLAY_DIRECTION, ReverseDisplay ? "1" : "0", MachineAttributeDTO.AttributeContext.MACHINE);

                    AddMachineDetails.GameMachineAttributes.Add(machattrnumcoin);
                    AddMachineDetails.GameMachineAttributes.Add(machattrrevdisp);

                    List<MachineDTO> machines = new List<MachineDTO>();
                    machines.Add(AddMachineDetails);
                    log.LogVariableState("Add machine details ", machines);
                    IMachineUseCases iMachineUseCases = GameUseCaseFactory.GetMachineUseCases(ExecutionContext);
                    string result = await iMachineUseCases.SaveMachines(machines);
                    log.LogVariableState("addmachine result", result);
                    SetLoadingVisible(false);
                    if (result == "Success")
                    {
                        ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2801, AddMachineName);
                        ShowMessagePopup(MessageViewContainerList.GetMessage(ExecutionContext, "Success"), MessageViewContainerList.GetMessage(ExecutionContext, "Machine Added"), ErrorMessage);
                        // should add to ref machine list so that when come from game management again LOV reflects newly added machine
                    }
                    else
                    {
                        // dont close window. keep details user entered.
                        ErrorMessage = MessageViewContainerList.GetMessage(ExecutionContext, 2802, null);
                        ShowMessagePopup(MessageViewContainerList.GetMessage(ExecutionContext, "Failure", null), MessageViewContainerList.GetMessage(ExecutionContext, "Try Again!", null), ErrorMessage);
                    }
                    SetLoadingVisible(false);
                }
                log.LogMethodExit();
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;
            }
            catch (ValidationException vex)
            {
                SetLoadingVisible(false);
                log.Error(vex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2802, null) + "-" + vex.Message.ToString(),MessageType.Error);
               
            }
            catch (Exception ex)
            {
                SetLoadingVisible(false);
                log.Error(ex);
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2802, null) + "-" + ex.Message.ToString(), MessageType.Error);
            }
        }
        private List<AllowedMachineNamesDTO> GetAllowedMachineNames(int gameId)
        {
            log.LogMethodEntry();
            List<GameDTO> gameList = null;
            List<AllowedMachineNamesDTO> AllowedMachineNames = null;
            if (gameId > 0)
            {
                SetLoadingVisible(true);
                try
                {
                    IGameUseCases gameUseCases = GameUseCaseFactory.GetGameUseCases(ExecutionContext);
                    using (NoSynchronizationContextScope.Enter())
                    {
                        List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
                        searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.GAME_ID, gameId.ToString()));
                        searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.IS_ACTIVE, "Y"));
                        Task<List<GameDTO>> task = gameUseCases.GetGames(searchParameters, true, 0, 1000, true);
                        task.Wait();
                        gameList = task.Result;
                        log.LogVariableState("List of games", gameList);
                    }
                    if (gameList != null)
                    {
                        GameDTO gameDTO = gameList.Where(x => x.GameId == gameId).FirstOrDefault();
                        if (gameDTO != null)
                        {
                            if (gameDTO.AllowedMachineDTOList != null && gameDTO.AllowedMachineDTOList.Count > 0)
                            {
                                AllowedMachineNames = gameDTO.AllowedMachineDTOList.Where(x => x.IsActive == true).ToList();
                                allowedmachineNameExist = true;
                            }
                            else
                            {
                                AllowedMachineNames = null;
                                allowedmachineNameExist = false;
                                ShowComboBox = false;
                            }
                        }
                    }
                    SetLoadingVisible(false);
                }
                catch (UnauthorizedException ex)
                {
                    log.Error(ex);
                    throw;
                }
                catch (ValidationException vex)
                {
                    SetLoadingVisible(false);
                    log.Error(vex);
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2802, null) + "-" + vex.Message.ToString(), MessageType.Error);
                }
                catch (Exception ex)
                {
                    SetLoadingVisible(false);
                    log.Error(ex);
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 2802, null) + "-" + ex.Message.ToString(), MessageType.Error);
                }
                finally
                {
                    IsLoadingVisible = false;
                }
            }
            log.LogMethodExit(AllowedMachineNames);
            return AllowedMachineNames;
        }
        private void ShowMessagePopup(string heading, string subHeading, string content)
        {
            log.LogMethodEntry(heading, subHeading, content);

            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            messagePopupView.Owner = addMachineView;

            GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                Heading = heading,
                SubHeading = subHeading,
                Content = content,
                CancelButtonText = MessageViewContainerList.GetMessage(ExecutionContext, "OK"),
                TimerMilliSeconds = 5000,
                PopupType = PopupType.Timer,
            };

            messagePopupView.DataContext = messagePopupVM;
            messagePopupView.ShowDialog();

            if (messagePopupVM != null && messagePopupVM.Heading != null &&
                messagePopupVM.Heading.ToLower() == MessageViewContainerList.GetMessage(ExecutionContext, "Success").ToLower())
            {
                StatusCode = StatusCode.Success;
                CloseAddWindow(messagePopupVM.Content);
            }
            log.LogMethodExit();
        }

        private void CancelAddMachine(object param)
        {
            log.LogMethodEntry(param);
            log.Info("Add machine page is closed");
            if (param != null)
            {
                addMachineView = param as GMAddMachineView;
                StatusCode = StatusCode.Failure;
                CloseAddWindow(String.Empty);
            }
            log.LogMethodExit();
        }

        private void CloseAddWindow(string message)
        {
            log.LogMethodEntry(message);
            SuccessMessage = message;
            if (addMachineView != null)
            {
                addMachineView.Close();
            }
            log.LogMethodExit();
        }

        public ExecutionContext GetExecutionContext()
        {
            log.LogMethodEntry();
            log.LogMethodExit(ExecutionContext);
            return ExecutionContext;

        }

        #endregion Methods

        #region Commands
        public ICommand AddMachineCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addMachineCommand);
                return addMachineCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                addMachineCommand = value;
            }
        }

        public ICommand CancelMachineCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodEntry(cancelAddMachineCommand);
                return cancelAddMachineCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                cancelAddMachineCommand = value;
            }
        }

        #endregion Commands

    }
}

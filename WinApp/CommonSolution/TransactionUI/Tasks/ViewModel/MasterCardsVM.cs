/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Master cards View model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-Aug-2021   Prashanth            Created for POS UI Redesign 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Semnox.Core.Utilities;
using Semnox.Parafait.AccountsUI;
using Semnox.Parafait.CommonUI;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.TransactionUI
{
    public class MasterCardsVM : TaskBaseViewModel
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool showCardAndNumericUpDownGrid;
        private bool showNumericUpDown;
        private int ssidValue;
        private GenericToggleButtonsVM genericToggleButtonsVM;
        private CardDetailsVM cardDetailsVM;
        private CustomToggleButtonItem selectedToggleButton;
        private MasterCardsView masterCardsView;
        private ICommand loadedCommand;
        private ICommand toggleCheckedCommand;
        private ICommand cardAddedCommand;
        private ICommand backButtonCommand;
        #endregion

        #region Properties
        public ICommand BackButtonCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(backButtonCommand);
                return backButtonCommand;
            }
            set
            {
                log.LogMethodEntry(backButtonCommand, value);
                SetProperty(ref backButtonCommand, value);
                log.LogMethodExit(backButtonCommand);
            }
        }
        public int SSIDValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ssidValue);
                return ssidValue;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref ssidValue, value);
                log.LogMethodExit();
            }
        }

        public GenericToggleButtonsVM GenericToggleButtonsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericToggleButtonsVM);
                return genericToggleButtonsVM;
            }
            set
            {
                log.LogMethodEntry(genericToggleButtonsVM, value);
                SetProperty(ref genericToggleButtonsVM, value);
                log.LogMethodExit();
            }
        }

        public CardDetailsVM CardDetailsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardDetailsVM);
                return cardDetailsVM;
            }
            set
            {
                log.LogMethodEntry(cardDetailsVM, value);
                SetProperty(ref cardDetailsVM, value);
                log.LogMethodExit();
            }
        }

        public ICommand LoadedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(loadedCommand);
                return loadedCommand;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref loadedCommand, value);
                log.LogMethodExit();
            }
        }

        public ICommand CardAddedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardAddedCommand);
                return cardAddedCommand;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref cardAddedCommand, value);
                log.LogMethodExit();
            }
        }

        public ICommand ToggleCheckedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(toggleCheckedCommand);
                return toggleCheckedCommand;
            }
            set
            {
                log.LogMethodEntry(toggleCheckedCommand, value);
                SetProperty(ref toggleCheckedCommand, value);
                log.LogMethodExit();
            }
        }

        public bool ShowCardAndNumericUpDownGrid
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showCardAndNumericUpDownGrid);
                return showCardAndNumericUpDownGrid;
            }
            set
            {
                log.LogMethodEntry(showCardAndNumericUpDownGrid, value);
                SetProperty(ref showCardAndNumericUpDownGrid, value);
                log.LogMethodExit();
            }
        }

        public bool ShowNumericUpDown
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(showNumericUpDown);
                return showNumericUpDown;
            }
            set
            {
                log.LogMethodEntry(showNumericUpDown, value);
                SetProperty(ref showNumericUpDown, value);
                log.LogMethodExit();
            }
        }
        #endregion

        #region Constructor
        public MasterCardsVM(ExecutionContext executionContext, DeviceClass cardReader) : base(executionContext, cardReader)
        {
            SSIDValue = 1;
            ObservableCollection<CustomToggleButtonItem> customToggleButtonItems = new ObservableCollection<CustomToggleButtonItem>()
            {
                new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(executionContext, "'Free play' card"),
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                            FontWeight = FontWeights.Bold,
                        }
                    },
                    Key = "freePlayCard",
                    IsChecked = true,
                },
                new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(executionContext, "'Exit Free Play' Card"),
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                            FontWeight = FontWeights.Bold,
                            TextSize = TextSize.Large
                        }
                    },
                    Key = "exitFreePlayCard"
                },
                new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(executionContext, "Enter Free Play Mode"),
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                            FontWeight = FontWeights.Bold,
                        }
                    },
                    Key="enterFreePlayMode"
                },
                new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(executionContext, "Exit Free Play Mode"),
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                            FontWeight = FontWeights.Bold,
                        }
                    },
                    Key="exitFreePlayMode"
                },
                new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(executionContext, "Invalidate 'Free Play' Cards"),
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                            FontWeight = FontWeights.Bold,
                        }
                    },
                    Key="invalidate"
                },
                new CustomToggleButtonItem()
                {
                    DisplayTags = new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(executionContext, "'Change SSID' Card"),
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                            FontWeight = FontWeights.Bold,
                        }
                    },
                    Key="changeSSID"
                }
            };
            GenericToggleButtonsVM = new GenericToggleButtonsVM()
            {
                ToggleButtonItems = customToggleButtonItems,
                Columns = 3,
                IsVerticalOrientation = true,

            };
            PropertyChanged += OnPropertyChanged;
            CardDetailsVM = new CardDetailsVM(executionContext);
            CardDetailsVM.PropertyChanged += OnPropertyChanged;
            LoadedCommand = new DelegateCommand(OnLoadedCommand);
            BackButtonCommand = new DelegateCommand(OnBackButtonClicked, ButtonEnable);
            ToggleCheckedCommand = new DelegateCommand(OnToggleChecked);
            ShowCardAndNumericUpDownGrid = true;
            ShowNumericUpDown = false;
            ShowRemark = false;
            CardDetailsVM.EnableManualEntry = false;
            CardTappedEvent += HandleCardRead;
            CardAddedCommand = new DelegateCommand(OnCardAdded);
            OkCommand = new DelegateCommand(OnOkCommand, ButtonEnable);
            ClearCommand = new DelegateCommand(OnClearCommand, ButtonEnable);
        }
        #endregion

        #region Methods
        private void OnBackButtonClicked(object parameter)
        {
            if(parameter != null)
            {
                log.LogMethodEntry(parameter);
                PerformClose(parameter);
            }
            log.LogMethodExit();
        }
        private void OnCardAdded(object parameter)
        {
            log.LogMethodEntry(parameter);
            HandleCardRead();
            log.LogMethodExit();
        }
        private void HandleCardRead()
        {
            SetFooterContent(string.Empty, MessageType.None);
            if(TappedAccountDTO != null)
            {
                CardDetailsVM.AccountDTO = TappedAccountDTO;
                try
                {
                    byte[] dataBuffer = new byte[16];
                    string message = "";
                    CardType cardType = CardReader.CardType;
                    byte[] key;
                    if (cardType == CardType.MIFARE_ULTRA_LIGHT_C)
                    {
                        List<MifareKeyContainerDTO> mifareKeyContainerDTOs = MifareKeyViewContainerList.GetMifareKeyContainerDTOList(ExecutionContext).ToList();
                        MifareKeyContainerDTO mifareKeyContainerDTO = mifareKeyContainerDTOs.FirstOrDefault(x => x.Type == MifareKeyContainerDTO.MifareKeyType.ULTRA_LIGHT_C.ToString());
                        if (mifareKeyContainerDTO == null)
                        {
                            throw new Exception("unable to find Mifare key");
                        }
                        ByteArray byteArray = new ByteArray(mifareKeyContainerDTO.KeyString);
                        key = byteArray.Value;
                    }
                    else
                    {
                        key = GetKey();
                    }
                    log.Debug("Reading the card details ");
                    bool response = CardReader.read_data(4, 1, key, ref dataBuffer, ref message);
                    if (response)
                    {
                        byte[] decr = EncryptionAES.Decrypt(dataBuffer, GetKeyFromCardNumber());
                        int token = 0;
                        string cmd = "", data = "";
                        int i = 0;
                        while (i < decr.Length)
                        {
                            while (decr[i++] != '_')
                            {
                                continue;
                            }


                            byte[] tempb = new byte[20];
                            int ind = 0;
                            for (int j = i; j < decr.Length; j++)
                            {
                                if (decr[j] == '_')
                                {
                                    i = j;
                                    break;
                                }
                                tempb[ind++] = decr[j];
                            }

                            if (token == 0)
                            {
                                cmd = Encoding.UTF8.GetString(tempb).TrimEnd('\0');
                                token++;
                            }
                            else if (token == 1)
                            {
                                data = BitConverter.ToUInt16(new byte[] { tempb[0], tempb[1] }, 0).ToString();
                                break;
                            }
                        }
                        string displayMessage = string.Empty;
                        switch (cmd)
                        {
                            case "FREEPLAY": displayMessage = "Free Play " + data; break;
                            case "ENDFREEP": displayMessage = "Exit Free Play"; break;
                            case "SSIDCHNG": displayMessage = "Change SSID " + data; break;
                            default: displayMessage = ""; break;
                        }
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, displayMessage),MessageType.Info);
                    }
                }
                catch//(Exception ex)
                {
                    //SetFooterContent(ex.Message, MessageType.Error);
                }

            }
        }
        private void OnToggleChecked(object parameter)
        {
            selectedToggleButton = GenericToggleButtonsVM.SelectedToggleButtonItem;
            if(selectedToggleButton.Key.Equals("freePlayCard") ||
               selectedToggleButton.Key.Equals("exitFreePlayCard") ||
               selectedToggleButton.Key.Equals("changeSSID"))
            {
                ShowCardAndNumericUpDownGrid = true;
                if (selectedToggleButton.Key.Equals("changeSSID"))
                {
                    ShowNumericUpDown = true;
                }
                else
                {
                    ShowNumericUpDown = false;
                }
            }
            else
            {
                ShowCardAndNumericUpDownGrid = false;
                ShowNumericUpDown = false;
            }
        }

        private void OnLoadedCommand(object parameter)
        {
            log.LogMethodEntry();
            if(parameter != null)
            {
                masterCardsView = parameter as MasterCardsView;
                bool isapproved = ShowManagerApproval(parameter);
                if (!isapproved)
                {
                    PerformClose(parameter);
                }
            }
            log.LogMethodExit();
        }

        private async void OnOkCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty, MessageType.None);
            try
            {
                IsLoadingVisible = true;
                if (selectedToggleButton.Key.Equals("enterFreePlayMode"))
                {
                    CardConfigurationDTO configurationDTO = new CardConfigurationDTO(null, null, ManagerId);
                    string result = await TaskUseCaseFactory.GetTaskUseCases(ExecutionContext).UpdateEnterFreePlayMode(configurationDTO);
                    if (result.Equals("Success"))
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Free play mode set in configuration"), MessageType.Info);
                    }
                    IsLoadingVisible = false;
                    return;
                }
                else if (selectedToggleButton.Key.Equals("exitFreePlayMode"))
                {
                    CardConfigurationDTO configurationDTO = new CardConfigurationDTO(null, null, ManagerId);
                    string result = await TaskUseCaseFactory.GetTaskUseCases(ExecutionContext).UpdateExitFreePlayMode(configurationDTO);
                    if (result.Equals("Success"))
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Free play mode exited"), MessageType.Info);
                    }
                    IsLoadingVisible = false;
                    return;
                }
                else if (selectedToggleButton.Key.Equals("invalidate"))
                {
                    CardConfigurationDTO configurationDTO = new CardConfigurationDTO(null, null, ManagerId);
                    string result = await TaskUseCaseFactory.GetTaskUseCases(ExecutionContext).InvalidateFreePlayCards(configurationDTO);
                    if (result.Equals("Success"))
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Free play cards have been invalidated. Please restart game readers."), MessageType.Info);
                    }
                    IsLoadingVisible = false;
                    return;
                }

                if(CardDetailsVM.AccountDTO == null)
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 257), MessageType.Error);
                    IsLoadingVisible = false;
                    return;
                }

                byte[] dataBuffer = new byte[16];
                CardType cardType = CardReader.CardType;
                byte[] key;
                if(cardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    List<MifareKeyContainerDTO> mifareKeyContainerDTOs = MifareKeyViewContainerList.GetMifareKeyContainerDTOList(ExecutionContext).ToList();
                    MifareKeyContainerDTO mifareKeyContainerDTO = mifareKeyContainerDTOs.FirstOrDefault(x => x.Type == MifareKeyContainerDTO.MifareKeyType.ULTRA_LIGHT_C.ToString() && x.IsCurrent);
                    if(mifareKeyContainerDTO == null)
                    {
                        IsLoadingVisible = false;
                        throw new Exception("unable to find Mifare key");
                    }
                    ByteArray byteArray = new ByteArray(mifareKeyContainerDTO.KeyString);
                    key = byteArray.Value;
                }
                else
                {
                    key = GetKey();
                }
                string message = string.Empty;
                bool response = CardReader.read_data(4, 1, key, ref dataBuffer, ref message);
                if (!response)
                {
                    ShowMessagePopView("Unable to read card");
                    IsLoadingVisible = false;
                    return;
                }
                byte[] tempArr = new byte[0];
                string CMD = "";
                if (selectedToggleButton.Key.Equals("freePlayCard"))
                {
                    tempArr = Encoding.ASCII.GetBytes("_FREEPLAY_");
                    byte[] counter = BitConverter.GetBytes(Convert.ToUInt16(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext,"FREE_PLAY_CARD_MAGIC_COUNTER")));
                    byte[] newArr = new byte[tempArr.Length + counter.Length];
                    Array.Copy(tempArr, newArr, tempArr.Length);
                    Array.Copy(counter, 0, newArr, tempArr.Length, counter.Length);
                    tempArr = newArr;
                    CMD = "FREEPLAY";
                }
                else if (selectedToggleButton.Key.Equals("exitFreePlayCard"))
                {
                    tempArr = Encoding.ASCII.GetBytes("_ENDFREEP_");
                    CMD = "ENDFREEP";
                }

                else if (selectedToggleButton.Key.Equals("changeSSID"))
                {
                    tempArr = Encoding.ASCII.GetBytes("_SSIDCHNG_");
                    byte[] ssid = BitConverter.GetBytes(Convert.ToUInt16(SSIDValue));
                    byte[] newArr = new byte[tempArr.Length + ssid.Length];
                    Array.Copy(tempArr, newArr, tempArr.Length);
                    Array.Copy(ssid, 0, newArr, tempArr.Length, ssid.Length);
                    tempArr = newArr;
                    CMD = "SSIDCHNG_" + SSIDValue.ToString();
                }

                byte[] Arr = new byte[16];
                Array.Copy(tempArr, 0, Arr, 1, tempArr.Length);
                Arr[0] = 0x4c;
                Arr[tempArr.Length + 1] = (byte)'_';
                Arr[tempArr.Length + 2] = 0x4c;

                tempArr = EncryptionAES.Encrypt(Arr, GetKeyFromCardNumber());
                cardType = CardReader.CardType;
                if (cardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    List<MifareKeyContainerDTO> mifareKeyContainerDTOs = MifareKeyViewContainerList.GetMifareKeyContainerDTOList(ExecutionContext).ToList();
                    MifareKeyContainerDTO mifareKeyContainerDTO = mifareKeyContainerDTOs.FirstOrDefault(x => x.Type == MifareKeyContainerDTO.MifareKeyType.ULTRA_LIGHT_C.ToString() && x.IsCurrent);
                    if (mifareKeyContainerDTO == null)
                    {
                        IsLoadingVisible = false;
                        throw new Exception("unable to find Mifare key");
                    }
                    ByteArray byteArray = new ByteArray(mifareKeyContainerDTO.KeyString);
                    key = byteArray.Value;
                }
                else
                {
                    key = GetKey();
                }
                response = CardReader.write_data(4, 1, key, tempArr, ref message);
                if (!response)
                {
                    ShowMessagePopView("Writing data to card failed: " + message);
                    IsLoadingVisible = false;
                    return;
                }
                CardReader.beep();
                CardConfigurationDTO cardConfigurationDTO = new CardConfigurationDTO(CardDetailsVM.AccountDTO.TagNumber,CMD, ManagerId);
                await TaskUseCaseFactory.GetTaskUseCases(ExecutionContext).ConfigureCard(cardConfigurationDTO);
                IsLoadingVisible = false;
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Master card created successfully"), MessageType.Info);
            }
            catch (Exception ex)
            {
                IsLoadingVisible = false;
                SetFooterContent(ex.Message, MessageType.Error);
            }
            finally
            {
                IsLoadingVisible = false;
            }
        }

        private byte[] GetKey()
        {
            List<MifareKeyContainerDTO> mifareKeyViewContainerList= MifareKeyViewContainerList.GetMifareKeyContainerDTOList(ExecutionContext).ToList();
            MifareKeyContainerDTO mifareKeyContainerDTO = mifareKeyViewContainerList.FirstOrDefault(x => x.Type == MifareKeyContainerDTO.MifareKeyType.CLASSIC.ToString() && x.IsCurrent);
            if(mifareKeyContainerDTO == null)
            {
                throw new Exception("Unable to find the mifare key");
            }
            ByteArray byteArray = new ByteArray(mifareKeyContainerDTO.KeyString);
            return byteArray.Value;
        }

        byte[] GetKeyFromCardNumber()
        {
            log.LogMethodEntry();
            string encryptionKey = SystemOptionViewContainerList.GetSystemOption(ExecutionContext, "Parafait Keys", "MifareCard");// "46A97988SEMNOX!1CCCC9D1C581D86EE";
            string cardNumber = CardDetailsVM.AccountDTO.TagNumber;
            byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
            key[16] = Convert.ToByte(Convert.ToInt32(cardNumber[0].ToString() + cardNumber[1].ToString(), 16));
            key[17] = Convert.ToByte(Convert.ToInt32(cardNumber[2].ToString() + cardNumber[3].ToString(), 16));
            key[18] = Convert.ToByte(Convert.ToInt32(cardNumber[4].ToString() + cardNumber[5].ToString(), 16));
            key[19] = Convert.ToByte(Convert.ToInt32(cardNumber[6].ToString() + cardNumber[7].ToString(), 16));
            log.LogMethodExit("encryptionKey");
            return key;
        }
        private void OnClearCommand(object parameter)
        {
            log.LogMethodEntry();
            if(CardDetailsVM != null && CardDetailsVM.AccountDTO != null)
            {
                CardDetailsVM.AccountDTO = null;
            }
            log.LogMethodExit();
        }

        private void ShowMessagePopView(string content)
        {
            log.LogMethodEntry(content);
            GenericMessagePopupView messagePopupView = new GenericMessagePopupView();
            GenericMessagePopupVM messagePopupVM = new GenericMessagePopupVM(ExecutionContext)
            {
                Heading = MessageViewContainerList.GetMessage(ExecutionContext, 1879),
                Content = MessageViewContainerList.GetMessage(ExecutionContext, content),
                OkButtonText = MessageViewContainerList.GetMessage(this.ExecutionContext, "OK", null),
                MessageButtonsType = MessageButtonsType.OK
            };
            messagePopupView.DataContext = messagePopupVM;
            if (masterCardsView != null)
            {
                messagePopupView.Owner = masterCardsView;
            }
            messagePopupView.ShowDialog();
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            switch (e.PropertyName)
            {
                case "IsLoadingVisible":
                    if (sender is CardDetailsVM)
                    {
                        IsLoadingVisible = cardDetailsVM.IsLoadingVisible;
                    }
                    else
                    {
                        RaiseCanExecuteChanged();
                    }
                    break;
            }
            log.LogMethodExit();
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            if (genericToggleButtonsVM != null)
            {
                genericToggleButtonsVM.IsLoadingVisible = IsLoadingVisible;
                genericToggleButtonsVM.RaiseCanExecuteChanged();
            }
            (BackButtonCommand as DelegateCommand).RaiseCanExecuteChanged();
            (OkCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ClearCommand as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        #endregion
    }
}

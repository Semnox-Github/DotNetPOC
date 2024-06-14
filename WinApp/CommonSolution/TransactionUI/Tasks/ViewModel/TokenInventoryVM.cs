/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - TokenCardInventory View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     25-May-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.CommonUI;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Semnox.Parafait.ViewContainer;
using System.Windows;
using Semnox.Parafait.Customer.Accounts;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace Semnox.Parafait.TransactionUI
{
    public class TokenInventoryVM : ViewModelBase
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int posMachineType;
        private int kioskMachineType;
        private int otherMachineType;
        private int tagTypeToken;
        private int tagTypeCard;
        private int activityTypeOnHand;
        private int activityTypePurchase;
        private int activityTypeTransfer;
        private int activityTypeOther;
        private ICommand confirmClickedCommand;
        private ICommand clearClickedCommand;
        private DisplayTagsVM displayTagsVM;
        private FooterVM footerVM;
        private string moduleName;
        DateTime lastSundayDate;
        private string tokenFromKiosk;
        private string tokenFromPOS;
        private TokenCardInventoryDTO tokenKioskTag;
        private TokenCardInventoryDTO tokenPOSTag;
        private string remainingOnHandToken;
        private TokenCardInventoryDTO tokenHandTag;
        private string transferredToken;
        private TokenCardInventoryDTO transferredTokenTag;
        private string totalCardsOnHand;
        private TokenCardInventoryDTO cardsOnHandTag;
        private string cardsPurchased;
        private TokenCardInventoryDTO cardPurchasedTag;
        private ICommand navigationClickCommand;
        #endregion

        #region Properties  
        
        public string TokenFromKiosk
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tokenFromKiosk);
                return tokenFromKiosk;
            }
            set
            {
                log.LogMethodEntry(tokenFromKiosk, value);
                SetProperty(ref tokenFromKiosk, value);
                log.LogMethodExit(tokenFromKiosk);

            }
        }

        public TokenCardInventoryDTO TokenKioskTag
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tokenKioskTag);
                return tokenKioskTag;
            }

            set
            {
                log.LogMethodEntry(tokenKioskTag, value);
                SetProperty(ref tokenKioskTag, value);
                log.LogMethodExit(tokenKioskTag);
            }
        }

        public string TokenfromPOS
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tokenFromPOS);
                return tokenFromPOS;
            }
            set
            {
                log.LogMethodEntry(tokenFromPOS, value);
                SetProperty(ref tokenFromPOS, value);
                log.LogMethodExit(tokenFromPOS);
            }
        }

        public TokenCardInventoryDTO TokenPOSTag
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tokenPOSTag);
                return tokenPOSTag;
            }
            set
            {
                log.LogMethodEntry(tokenPOSTag, value);
                SetProperty(ref tokenPOSTag, value);
                log.LogMethodExit(tokenPOSTag);
            }
        }

        public string RemainingOnHandToken
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(remainingOnHandToken);
                return remainingOnHandToken;
            }
            set
            {
                log.LogMethodEntry(remainingOnHandToken, value);
                SetProperty(ref remainingOnHandToken, value);
                log.LogMethodExit(remainingOnHandToken);
            }
        }

        public TokenCardInventoryDTO TokenHandTag
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(tokenHandTag);
                return tokenHandTag;
            }
            set
            {
                log.LogMethodEntry(tokenHandTag, value);
                SetProperty(ref tokenHandTag, value);
                log.LogMethodExit(tokenHandTag);
            }
        }

        public string TransferredToken
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transferredToken);
                return transferredToken;
            }
            set
            {
                log.LogMethodEntry(transferredToken, value);
                SetProperty(ref transferredToken, value);
                log.LogMethodExit(transferredToken);
            }
        }

        public TokenCardInventoryDTO TransferredTokenTag
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(transferredTokenTag);
                return transferredTokenTag;
            }
            set
            {
                log.LogMethodEntry(transferredTokenTag, value);
                SetProperty(ref transferredTokenTag, value);
                log.LogMethodExit(transferredTokenTag);
            }
        }

        public string TotalCardsOnHand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(totalCardsOnHand);
                return totalCardsOnHand;
            }
            set
            {
                log.LogMethodEntry(totalCardsOnHand, value);
                SetProperty(ref totalCardsOnHand, value);
                log.LogMethodExit(totalCardsOnHand);
            }
        }

        public TokenCardInventoryDTO CardsOnHandTag
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardsOnHandTag);
                return cardsOnHandTag;
            }
            set
            {
                log.LogMethodEntry(cardsOnHandTag, value);
                SetProperty(ref cardsOnHandTag, value);
                log.LogMethodExit(cardsOnHandTag);
            }
        }

        public string CardsPurchased
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardsPurchased);
                return cardsPurchased;
            }
            set
            {
                log.LogMethodEntry(cardsPurchased, value);
                SetProperty(ref cardsPurchased, value);
                log.LogMethodExit(cardsPurchased);
            }
        }

        public TokenCardInventoryDTO CardPurchasedTag
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cardPurchasedTag);
                return cardPurchasedTag;
            }
            set
            {
                log.LogMethodEntry(cardPurchasedTag, value);
                SetProperty(ref cardPurchasedTag, value);
                log.LogMethodExit(cardPurchasedTag);
            }
        }



        public string ModuleName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(moduleName);
                return moduleName;
            }
            set
            {
                log.LogMethodEntry(moduleName, value);
                SetProperty(ref moduleName, value);
                log.LogMethodExit(moduleName);
            }
        }



        public FooterVM FooterVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(footerVM);
                return footerVM;
            }
            set
            {
                log.LogMethodEntry(footerVM, value);
                SetProperty(ref footerVM, value);
                log.LogMethodExit(footerVM);
            }
        }

        public DisplayTagsVM DisplayTagsVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(displayTagsVM);
                return displayTagsVM;
            }
            set
            {
                log.LogMethodEntry(displayTagsVM, value);
                SetProperty(ref displayTagsVM, value);
                log.LogMethodExit(displayTagsVM);
            }
        }

        public ICommand ClearClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(clearClickedCommand);
                return clearClickedCommand;
            }
            set
            {
                log.LogMethodEntry(clearClickedCommand, value);
                SetProperty(ref clearClickedCommand, value);
                log.LogMethodExit(clearClickedCommand);
            }
        }

        public ICommand ConfirmClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(confirmClickedCommand);
                return confirmClickedCommand;
            }
            set
            {
                log.LogMethodEntry(confirmClickedCommand, value);
                SetProperty(ref confirmClickedCommand, value);
                log.LogMethodExit(confirmClickedCommand);
            }
        }
        public ICommand NavigationClickCommand
        {
            set
            {
                log.LogMethodEntry(navigationClickCommand, value);
                SetProperty(ref navigationClickCommand, value);
                log.LogMethodExit(navigationClickCommand);
            }
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(navigationClickCommand);
                return navigationClickCommand;
            }
        }



        #endregion

        #region constructor

        public TokenInventoryVM(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ModuleName = MessageViewContainerList.GetMessage(executionContext, "Token/Card Inventory");
            clearClickedCommand = new DelegateCommand(OnClearClickedCommand, ButtonEnable);
            NavigationClickCommand = new DelegateCommand(OnNavigationClickedCommand, ButtonEnable);
            ConfirmClickedCommand = new DelegateCommand(OnConfirmClickedCommand, ButtonEnable);
            PropertyChanged += OnPropertyChanged;
            DisplayTagsVM = new DisplayTagsVM();
            ExecutionContext = executionContext;
            FooterVM = new FooterVM(ExecutionContext)
            {
                MessageType = MessageType.None,
                HideSideBarVisibility = Visibility.Collapsed,
            };
            CardMaintenanceLoad();
            log.LogMethodExit();
        }
        #endregion


        #region Methods
        private void OnNavigationClickedCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            TokenInventoryView window = parameter as TokenInventoryView;
            window.Close();
            log.LogMethodExit();
        }
        private void CardMaintenanceLoad()
        {
            log.LogMethodEntry();

            UpdateLookUpValues();
            string businessDayStartTime = ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "BUSINESS_DAY_START_TIME");
            int businessHour = 6;
            if (!string.IsNullOrEmpty(businessDayStartTime))
            {
                try
                {
                    businessHour = Convert.ToInt32(businessDayStartTime);
                }
                catch
                {
                    businessHour = 6;
                }
            }
            //check today is monday and buisness hour not crossed the get last 2 sunday date
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && DateTime.Now.Hour < businessHour)
            {
                lastSundayDate = DateTime.Today.Date.AddDays(-8).AddHours(businessHour);
            }
            else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)//If Today is sunday consider last sunday
            {
                lastSundayDate = DateTime.Today.Date.AddDays(-7).AddHours(businessHour);
            }
            else
            {
                DateTime input = DateTime.Today.Date;
                int days = DayOfWeek.Sunday - input.DayOfWeek;
                lastSundayDate = input.AddDays(days).AddHours(businessHour);
            }
            UpdateCardInventoryTab();
            log.LogMethodExit();
        }

        private async void UpdateCardInventoryTab()
        {
            log.LogMethodEntry();
            DisplayTagsVM.DisplayTags = new ObservableCollection<ObservableCollection<DisplayTag>>()
                {
                    new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text = MessageViewContainerList.GetMessage(ExecutionContext, "Location"),

                        },
                        new DisplayTag()
                        {
                            Text = SiteViewContainerList.GetCurrentSiteContainerDTO(ExecutionContext).SiteName,
                            TextSize = TextSize.Small,
                            FontWeight = FontWeights.Bold
                        },
                    },
                    new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text=MessageViewContainerList.GetMessage(ExecutionContext,"Week Ending"),

                        },
                        new DisplayTag()
                        {
                            Text= MessageViewContainerList.GetMessage(ExecutionContext,"Sunday") + "(" + lastSundayDate.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext,"DATE_FORMAT")) +")",
                            TextSize = TextSize.Small,
                            FontWeight = FontWeights.Bold
                        },
                    },
                    new ObservableCollection<DisplayTag>()
                    {
                        new DisplayTag()
                        {
                            Text= MessageViewContainerList.GetMessage(ExecutionContext, "Technician Name"),

                        },
                        new DisplayTag()
                        {
                            Text = UserViewContainerList.GetUserContainerDTO(ExecutionContext.SiteId,ExecutionContext.UserId).UserName,
                            TextSize = TextSize.Small,
                            FontWeight = FontWeights.Bold
                        },
                    },
                };

            await PopulateTokenCardInventory();
            log.LogMethodExit();
        }

        public void SetFooterContent(string message, MessageType messageType)
        {
            log.LogMethodEntry(message, messageType);
            if (footerVM != null)
            {
                this.footerVM.Message = message;
                this.footerVM.MessageType = messageType;
            }
            log.LogMethodExit();
        }

        private void OnClearClickedCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            SetFooterContent(string.Empty,MessageType.None);
            if (TokenKioskTag.cardInventoryKeyId > -1)
            {
                TokenFromKiosk = TokenKioskTag.Number.ToString();
            }
            else
            {
                TokenFromKiosk = null;
            }
            if (TokenPOSTag.cardInventoryKeyId > -1)
            {
                TokenfromPOS = TokenPOSTag.Number.ToString();
            }
            else
            {
                TokenfromPOS = null;
            }
            if (TokenHandTag.cardInventoryKeyId > -1)
            {
                RemainingOnHandToken = TokenHandTag.Number.ToString();
            }
            else
            {
                RemainingOnHandToken = null;
            }
            if (TransferredTokenTag.cardInventoryKeyId > -1)
            {
                TransferredToken = TransferredTokenTag.Number.ToString();
            }
            else
            {
                TransferredToken = null;
            }
            if (CardPurchasedTag.cardInventoryKeyId > -1)
            {
                CardsPurchased = CardPurchasedTag.Number.ToString();
            }
            else
            {
                CardsPurchased = null;
            }
            if (CardsOnHandTag.cardInventoryKeyId > -1)
            {
                TotalCardsOnHand = CardsOnHandTag.Number.ToString();
            }
            else
            {
                TotalCardsOnHand = null;
            }
            log.LogMethodExit();
        }

        private void OnConfirmClickedCommand(object parameter)
        {
            log.LogMethodEntry(parameter);
            Save();
            log.LogMethodExit();
        }



        private async Task PopulateTokenCardInventory()
        {
            log.LogMethodEntry();
            if (ValidateLookupValues())
            {

                List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParams = new List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>>();
                searchParams.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.SITE_ID, ExecutionContext.SiteId.ToString()));
                searchParams.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.DATE, lastSundayDate.ToString("MM-dd-yyyy hh", CultureInfo.InvariantCulture)));

                ITokenCardInventoryUseCases tokenCardInventoryUseCase = TokenCardInventoryUseCaseFactory.GetTokenCardInventoryUseCases(ExecutionContext);
                List<TokenCardInventoryDTO> tokenCardInventoryDTOs = null;
                try
                {
                    IsLoadingVisible = true;
                    tokenCardInventoryDTOs = await tokenCardInventoryUseCase.GetAllTokenCardInventoryDTOsList(searchParams);
                }
                catch (ValidationException vex)
                {
                    log.Error(vex);
                    IsLoadingVisible = false;
                    SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
                catch (UnauthorizedException uaex)
                {
                    log.Error(uaex);
                    IsLoadingVisible = false;
                    throw;
                }
                catch (ParafaitApplicationException pax)
                {
                    log.Error(pax);
                    IsLoadingVisible = false;
                    SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    IsLoadingVisible = false;
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
                finally
                {
                    IsLoadingVisible = false;
                }
                TokenCardInventoryDTO tokenCardInventoryDTO;
                if (tokenCardInventoryDTOs != null && tokenCardInventoryDTOs.Count > 0)
                {
                    tokenCardInventoryDTO = tokenCardInventoryDTOs.Find(x => x.MachineType == posMachineType);
                    if (tokenCardInventoryDTO != null)
                    {

                        if (tokenCardInventoryDTO.cardInventoryKeyId > -1)
                        {
                            TokenfromPOS = tokenCardInventoryDTO.Number.ToString();
                        }
                        else
                        {
                            TokenfromPOS = null;
                        }
                        TokenPOSTag = tokenCardInventoryDTO;
                    }

                    tokenCardInventoryDTO = tokenCardInventoryDTOs.Find(x => x.MachineType == kioskMachineType);
                    if (tokenCardInventoryDTO != null)
                    {

                        if (tokenCardInventoryDTO.cardInventoryKeyId > -1)
                        {
                            TokenFromKiosk = tokenCardInventoryDTO.Number.ToString();
                        }
                        else
                        {
                            TokenFromKiosk = null;
                        }
                        TokenKioskTag = tokenCardInventoryDTO;
                    }

                    tokenCardInventoryDTO = tokenCardInventoryDTOs.Find(x => x.TagType == tagTypeToken && x.MachineType == otherMachineType && x.ActivityType == activityTypeOnHand);
                    if (tokenCardInventoryDTO != null)
                    {

                        if (tokenCardInventoryDTO.cardInventoryKeyId > -1)
                        {
                            RemainingOnHandToken = tokenCardInventoryDTO.Number.ToString();
                        }
                        else
                        {
                            RemainingOnHandToken = null;
                        }
                        TokenHandTag = tokenCardInventoryDTO;
                    }

                    tokenCardInventoryDTO = tokenCardInventoryDTOs.Find(x => x.TagType == tagTypeToken && x.MachineType == otherMachineType && x.ActivityType == activityTypeTransfer);
                    if (tokenCardInventoryDTO != null)
                    {

                        if (tokenCardInventoryDTO.cardInventoryKeyId > -1)
                        {
                            TransferredToken = tokenCardInventoryDTO.Number.ToString();
                        }
                        else
                        {
                            TransferredToken = null;
                        }
                        TransferredTokenTag = tokenCardInventoryDTO;
                    }

                    tokenCardInventoryDTO = tokenCardInventoryDTOs.Find(x => x.TagType == tagTypeCard && x.MachineType == otherMachineType && x.ActivityType == activityTypeOnHand);
                    if (tokenCardInventoryDTO != null)
                    {

                        if (tokenCardInventoryDTO.cardInventoryKeyId > -1)
                        {
                            TotalCardsOnHand = tokenCardInventoryDTO.Number.ToString();
                        }
                        else
                        {
                            TotalCardsOnHand = null;
                        }
                        CardsOnHandTag = tokenCardInventoryDTO;
                    }

                    tokenCardInventoryDTO = tokenCardInventoryDTOs.Find(x => x.TagType == tagTypeCard && x.MachineType == otherMachineType && x.ActivityType == activityTypePurchase);
                    if (tokenCardInventoryDTO != null)
                    {

                        if (tokenCardInventoryDTO.cardInventoryKeyId > -1)
                        {
                            CardsPurchased = tokenCardInventoryDTO.Number.ToString();
                        }
                        else
                        {
                            CardsPurchased = null;
                        }
                        CardPurchasedTag = tokenCardInventoryDTO;
                    }
                }
            }
            else
            {
                SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1198) + MessageViewContainerList.GetMessage(ExecutionContext, "Validation"), MessageType.Error);
            }
            log.LogMethodExit();
        }

        void UpdateLookUpValues()
        {
            log.LogMethodEntry();
            tagTypeToken = GetLookupValuesDTO("TAG_TYPE", "TOKEN").LookupValueId;
            tagTypeCard = GetLookupValuesDTO("TAG_TYPE", "CARD").LookupValueId;

            activityTypeOnHand = GetLookupValuesDTO("ACTIVITY_TYPE", "ON_HAND").LookupValueId;
            activityTypePurchase = GetLookupValuesDTO("ACTIVITY_TYPE", "PURCHASE").LookupValueId;
            activityTypeTransfer = GetLookupValuesDTO("ACTIVITY_TYPE", "TRANSFER").LookupValueId;
            activityTypeOther = GetLookupValuesDTO("ACTIVITY_TYPE", "OTHER").LookupValueId;

            posMachineType = GetLookupValuesDTO("MACHINE_TYPE", "POS").LookupValueId;
            kioskMachineType = GetLookupValuesDTO("MACHINE_TYPE", "KIOSK").LookupValueId;
            otherMachineType = GetLookupValuesDTO("MACHINE_TYPE", "OTHER").LookupValueId;
            log.LogMethodExit();
        }

        LookupValuesContainerDTO GetLookupValuesDTO(string lookupName, string lookupValue)
        {
            log.LogMethodExit(lookupName, lookupValue);

            LookupsContainerDTO lookupsContainerDTO = LookupsViewContainerList.GetLookupsContainerDTO(ExecutionContext.SiteId, lookupName);
            if (lookupsContainerDTO.LookupValuesContainerDTOList != null && lookupsContainerDTO.LookupValuesContainerDTOList.Count > 0)
            {
                LookupValuesContainerDTO lookupValuesContainerDTO = lookupsContainerDTO.LookupValuesContainerDTOList.Find(x => x.LookupValue.Equals(lookupValue));
                if (lookupValuesContainerDTO != null)
                {
                    log.LogMethodExit(lookupValuesContainerDTO);
                    return lookupValuesContainerDTO;
                }
            }
            return new LookupValuesContainerDTO();
        }

        bool ValidateLookupValues()
        {
            log.LogMethodEntry();
            if (posMachineType == -1 || kioskMachineType == -1 || otherMachineType == -1 //Check MachineType
                 || activityTypeOnHand == -1 || activityTypeTransfer == -1 || activityTypePurchase == -1 || activityTypeOther == -1 // Check activityType 
                 || tagTypeCard == -1 || tagTypeToken == -1) //Check TagType
            {
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private async void Save()
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            try
            {

                if (ValidateLookupValues())
                {
                    TokenCardInventoryDTO tokenInventoryDTO;
                    List<TokenCardInventoryDTO> saveTokenCardInventoryDTOList = new List<TokenCardInventoryDTO>();
                    List<TokenCardInventoryDTO> updateTokenCardInventoryDTOList = new List<TokenCardInventoryDTO>();
                    #region Save Token Collected KIOSK
                    if (!ValidateText("TokenFromKiosk"))
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(TokenFromKiosk))
                    {
                        if (TokenKioskTag != null && TokenKioskTag.cardInventoryKeyId > -1)
                        {
                            tokenInventoryDTO = TokenKioskTag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(TokenFromKiosk);
                            updateTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(TokenFromKiosk);
                            tokenInventoryDTO.TagType = tagTypeToken;
                            tokenInventoryDTO.MachineType = kioskMachineType;
                            tokenInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenInventoryDTO.Action = "ADD";
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                            saveTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }


                    }
                    #endregion

                    #region Save Token Collected POS
                    if (!ValidateText("TokenfromPOS"))
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(TokenfromPOS))
                    {
                        if (TokenPOSTag != null && TokenPOSTag.cardInventoryKeyId > -1)
                        {
                            tokenInventoryDTO = TokenPOSTag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(TokenfromPOS);
                            updateTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(TokenfromPOS);
                            tokenInventoryDTO.TagType = tagTypeToken;
                            tokenInventoryDTO.MachineType = posMachineType;
                            tokenInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenInventoryDTO.Action = "ADD";
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                            saveTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }

                    }
                    #endregion

                    #region Save remaining tokens
                    if (!ValidateText("RemainingOnHandToken"))
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(RemainingOnHandToken))
                    {
                        if (TokenHandTag != null && TokenHandTag.cardInventoryKeyId > -1)
                        {
                            tokenInventoryDTO = TokenHandTag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(RemainingOnHandToken);
                            updateTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(RemainingOnHandToken);
                            tokenInventoryDTO.TagType = tagTypeToken;
                            tokenInventoryDTO.MachineType = otherMachineType;
                            tokenInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenInventoryDTO.Action = "ADD";
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                            saveTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }


                    }
                    #endregion

                    #region Save Transferred tokens
                    if (!ValidateText("TransferredToken"))
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(TransferredToken))
                    {
                        if (TransferredTokenTag != null && TransferredTokenTag.cardInventoryKeyId > -1)
                        {
                            tokenInventoryDTO = TransferredTokenTag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(TransferredToken);
                            updateTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(TransferredToken);
                            tokenInventoryDTO.TagType = tagTypeToken;
                            tokenInventoryDTO.MachineType = otherMachineType;
                            tokenInventoryDTO.ActivityType = activityTypeTransfer;
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                            tokenInventoryDTO.Action = "ADD";
                            saveTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }

                    }
                    #endregion

                    #region Save Cards on Hand
                    if (!ValidateText("TotalCardsOnHand"))
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(TotalCardsOnHand))
                    {
                        if (CardsOnHandTag != null && CardsOnHandTag.cardInventoryKeyId > -1)
                        {
                            tokenInventoryDTO = CardsOnHandTag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(TotalCardsOnHand);
                            updateTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(TotalCardsOnHand);
                            tokenInventoryDTO.TagType = tagTypeCard;
                            tokenInventoryDTO.MachineType = otherMachineType;
                            tokenInventoryDTO.ActivityType = activityTypeOnHand;
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                            tokenInventoryDTO.Action = "ADD";
                            saveTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }

                    }
                    #endregion

                    #region Save Cards Purchased
                    if (!ValidateText("CardsPurchased"))
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(CardsPurchased))
                    {
                        if (CardPurchasedTag != null && CardPurchasedTag.cardInventoryKeyId > -1)
                        {
                            tokenInventoryDTO = CardPurchasedTag as TokenCardInventoryDTO;
                            tokenInventoryDTO.Number = Convert.ToInt32(CardsPurchased);
                            updateTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }
                        else
                        {
                            tokenInventoryDTO = new TokenCardInventoryDTO();
                            tokenInventoryDTO.Number = Convert.ToInt32(CardsPurchased);
                            tokenInventoryDTO.TagType = tagTypeCard;
                            tokenInventoryDTO.MachineType = otherMachineType;
                            tokenInventoryDTO.ActivityType = activityTypePurchase;
                            tokenInventoryDTO.Actiondate = lastSundayDate;
                            tokenInventoryDTO.Action = "ADD";
                            saveTokenCardInventoryDTOList.Add(tokenInventoryDTO);
                        }

                    }
                    #endregion
                    ITokenCardInventoryUseCases tokenCardInventoryUseCase = TokenCardInventoryUseCaseFactory.GetTokenCardInventoryUseCases(ExecutionContext);
                    List<TokenCardInventoryDTO> resultFromSave = null;
                    List<TokenCardInventoryDTO> resultFromUpdate = null;
                    try
                    {
                        
                        IsLoadingVisible = true;
                        if (saveTokenCardInventoryDTOList != null && saveTokenCardInventoryDTOList.Count > 0)
                        {
                            resultFromSave = await tokenCardInventoryUseCase.SaveCardInventory(saveTokenCardInventoryDTOList);
                        }
                        if (updateTokenCardInventoryDTOList != null && updateTokenCardInventoryDTOList.Count > 0)
                        {
                            resultFromUpdate = await tokenCardInventoryUseCase.UpdateCardInventory(updateTokenCardInventoryDTOList);
                        }
                        
                    }
                    catch (ValidationException vex)
                    {
                        log.Error(vex);
                        IsLoadingVisible = false;
                        SetFooterContent(vex.Message.ToString(), MessageType.Error);
                    }
                    catch (UnauthorizedException uaex)
                    {
                        log.Error(uaex);
                        IsLoadingVisible = false;
                        throw;
                    }
                    catch (ParafaitApplicationException pax)
                    {
                        log.Error(pax);
                        IsLoadingVisible = false;
                        SetFooterContent(pax.Message.ToString(), MessageType.Error);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        IsLoadingVisible = false;
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                    }
                    finally
                    {
                        IsLoadingVisible = false;
                    }
                    await PopulateTokenCardInventory();
                    if (resultFromSave != null || resultFromUpdate != null)
                    {
                        SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1197, "Inventory details"), MessageType.Info);
                    }
                }
                else
                {
                    SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 1198) + MessageViewContainerList.GetMessage(ExecutionContext, "Validation"), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                SetFooterContent(ex.Message, MessageType.Error);
            }
            finally
            {
                IsLoadingVisible = false;
            }
            log.LogMethodExit();
        }
        private void RaiseCanExecuteChanged()
        {
            log.LogMethodEntry();
            (ClearClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (ConfirmClickedCommand as DelegateCommand).RaiseCanExecuteChanged();
            (NavigationClickCommand as DelegateCommand).RaiseCanExecuteChanged();
            log.LogMethodExit();
        }
        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            switch (e.PropertyName)
            {
                case "IsLoadingVisible":
                    RaiseCanExecuteChanged();
                    break;
                case "TokenFromKiosk":
                case "TokenfromPOS":
                case "RemainingOnHandToken":
                case "TransferredToken":
                case "TotalCardsOnHand":
                case "CardsPurchased":
                    ValidateText(e.PropertyName);
                    break;
            }
            log.LogMethodExit();
        }


        private bool ValidateText(string propertyName)
        {
            log.LogMethodEntry();
            SetFooterContent(string.Empty, MessageType.None);
            bool isValid = true;
            switch (propertyName)
            {
                case "TokenFromKiosk":
                    if (!string.IsNullOrWhiteSpace(TokenFromKiosk))
                    {
                        if (Convert.ToInt32(TokenFromKiosk) < 0)
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 34, "Token Collected from Kiosk"), MessageType.Warning);
                            isValid = false;
                        }                                               
                    }                    
                    break;
                case "TokenfromPOS":
                    if (!string.IsNullOrWhiteSpace(TokenfromPOS))
                    {
                        if (Convert.ToInt32(TokenfromPOS) < 0)
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 34, "Token Collected from Pos"), MessageType.Warning);
                            isValid = false;
                        }                                             
                    }
                    break;
                case "RemainingOnHandToken":
                    if (!string.IsNullOrWhiteSpace(RemainingOnHandToken))
                    {
                        if (Convert.ToInt32(RemainingOnHandToken) < 0)
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 34, "Remaining on Hand Token"), MessageType.Warning);
                            isValid = false;
                        }                       
                    }
                    break;
                case "TransferredToken":
                    if (!string.IsNullOrWhiteSpace(TransferredToken))
                    {
                        if (Convert.ToInt32(TransferredToken) < 0)
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 34, "Transferred Tokens"), MessageType.Warning);
                            isValid = false;
                        }
                    }
                    break;
                case "TotalCardsOnHand":
                    if (!string.IsNullOrWhiteSpace(TotalCardsOnHand))
                    {
                        if (Convert.ToInt32(TotalCardsOnHand) < 0)
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 34, "Total Cards on Hand"), MessageType.Warning);
                            isValid = false;
                        }
                    }
                    break;
                case "CardsPurchased":
                    if (!string.IsNullOrWhiteSpace(CardsPurchased))
                    {
                        if (Convert.ToInt32(CardsPurchased) < 0)
                        {
                            SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, 34, "Cards Purchased"), MessageType.Warning);
                            isValid = false;
                        }
                    }
                    break;
            }
            log.LogMethodExit(isValid);
            return isValid;
        }
        #endregion

    }
}

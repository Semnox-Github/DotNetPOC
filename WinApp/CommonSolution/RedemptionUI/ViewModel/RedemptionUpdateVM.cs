/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - view model for redemption update view
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Windows.Input;
using System.Collections.ObjectModel;
using System;
using System.Linq;

using Semnox.Parafait.CommonUI;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.ViewContainer;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Semnox.Parafait.RedemptionUI
{
    public class RedemptionUpdateVM : ViewModelBase
    {
        #region Members
        private bool ismultiScreenRowTwo;
        private bool multiScreenMode;
        private bool isUpdateButtonEnable;
        private bool updateClicked;
        private bool issuccess;

        private string title;
        private string heading;
        private string cancelButtonText;
        private string updateButtonText;
        private string remarks;
        private string addremarks;
        private string selectedRedemptionStatus;
        private string redeemedDate;
        private string preparedDate;

        private ObservableCollection<string> redemptionStatus;
        private RedemptionDTO redemptionDTO;
        private RedemptionMainUserControlVM redemptionMainUserControlVM ;

        private ICommand cancelCommand;
        private ICommand updateCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public bool IsMultiScreenRowTwo
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(ismultiScreenRowTwo);
                return ismultiScreenRowTwo;
            }
            set
            {
                log.LogMethodEntry(ismultiScreenRowTwo, value);
                SetProperty(ref ismultiScreenRowTwo, value);
                log.LogMethodExit(ismultiScreenRowTwo);
            }
        }
        public bool IsSuccess
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(issuccess);
                return issuccess;
            }
            set
            {
                log.LogMethodEntry(issuccess, value);
                SetProperty(ref issuccess, value);
                log.LogMethodExit(issuccess);
            }
        }
        public string RedeemedDate
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redeemedDate);
                return redeemedDate;
            }
            set
            {
                log.LogMethodEntry(redeemedDate, value);
                SetProperty(ref redeemedDate, value);
                log.LogMethodExit(redeemedDate);
            }
        }
        public string PreparedDate
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(preparedDate);
                return preparedDate;
            }
            set
            {
                log.LogMethodEntry(preparedDate, value);
                SetProperty(ref preparedDate, value);
                log.LogMethodExit(preparedDate);
            }
        }
        public bool UpdateClicked
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(updateClicked);
                return updateClicked;
            }
            set
            {
                log.LogMethodEntry(updateClicked, value);
                SetProperty(ref updateClicked, value);
                log.LogMethodExit(updateClicked);
            }
        }

        public bool MultiScreenMode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(multiScreenMode);
                return multiScreenMode;
            }
            set
            {
                log.LogMethodEntry(multiScreenMode, value);
                SetProperty(ref multiScreenMode, value);
                log.LogMethodExit(multiScreenMode);
            }
        }

        public string Title
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(title);
                return title;
            }
            set
            {
                log.LogMethodEntry(title, value);
                SetProperty(ref title, value);
                log.LogMethodExit(title);
            }
        }

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
                log.LogMethodEntry(heading, value);
                SetProperty(ref heading, value);
                log.LogMethodExit(heading);
            }
        }

        public string CancelButtonText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cancelButtonText);
                return cancelButtonText;
            }
            set
            {
                log.LogMethodEntry(cancelButtonText, value);
                SetProperty(ref cancelButtonText, value);
                log.LogMethodExit(cancelButtonText);
            }
        }

        public string UpdateButtonText
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(updateButtonText);
                return updateButtonText;
            }
            set
            {
                log.LogMethodEntry(updateButtonText, value);
                SetProperty(ref updateButtonText, value);
                log.LogMethodExit(updateButtonText);
            }
        }

        public string Remarks
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(remarks);
                return remarks;
            }
            set
            {
                log.LogMethodEntry(remarks, value);
                SetProperty(ref remarks, value);
                log.LogMethodExit(remarks);
            }
        }
        public string AddRemarks
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(addremarks);
                return addremarks;
            }
            set
            {
                log.LogMethodEntry(addremarks, value);
                SetProperty(ref addremarks, value);
                log.LogMethodExit(addremarks);
            }
        }
        public string SelectedRedemptionStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedRedemptionStatus);
                return selectedRedemptionStatus;
            }
            set
            {
                log.LogMethodEntry(selectedRedemptionStatus, value);
                SetProperty(ref selectedRedemptionStatus, value);
                log.LogMethodExit(selectedRedemptionStatus);
            }
        }

        public ObservableCollection<string> RedemptionStatus
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionStatus);
                return redemptionStatus;
            }
            set
            {
                log.LogMethodEntry(redemptionStatus, value);
                SetProperty(ref redemptionStatus, value);
                log.LogMethodExit(redemptionStatus);
            }
        }

        public RedemptionDTO RedemptionDTO
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionDTO);
                return redemptionDTO;
            }
            set
            {
                log.LogMethodEntry(redemptionDTO, value);
                SetProperty(ref redemptionDTO, value);
                log.LogMethodExit(redemptionDTO);
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(cancelCommand);
                return cancelCommand;
            }
            set
            {
                log.LogMethodEntry(cancelCommand, value);
                SetProperty(ref cancelCommand, value);
                log.LogMethodExit(cancelCommand);
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(updateCommand);
                return updateCommand;
            }
            set
            {
                log.LogMethodEntry(updateCommand, value);
                SetProperty(ref updateCommand, value);
                log.LogMethodExit(updateCommand);
            }
        }

        public bool IsUpdateButtonEnable
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isUpdateButtonEnable);
                return isUpdateButtonEnable;
            }
            set
            {
                log.LogMethodEntry(isUpdateButtonEnable, value);
                SetProperty(ref isUpdateButtonEnable, value);
                log.LogMethodExit(isUpdateButtonEnable);
            }
        }
        #endregion

        #region Methods

        private void OnCancelClicked(object parameter)
        {
            log.LogMethodEntry();
            updateClicked = false;
            CloseWindow(parameter);
            log.LogMethodExit();
        }
        private async Task<List<RedemptionDTO>> GetRedemption(string redemptionId)
        {
            log.LogMethodEntry(redemptionId);
            List<RedemptionDTO> searchedRedemptionList = new List<RedemptionDTO>();
            IRedemptionUseCases RedemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
            List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchparams = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();
            searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, Convert.ToString(ExecutionContext.GetSiteId())));
            if (!string.IsNullOrEmpty(redemptionId))
            {
                searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEPTION_ID, redemptionId));
            }
            searchparams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.LOAD_GIFT_CARD_TICKET_ALLOCATION_DETAILS, "Y"));
            Task<List<RedemptionDTO>> taskGetRedemption = RedemptionUseCases.GetRedemptionOrders(searchparams);
            searchedRedemptionList = await taskGetRedemption;
            if (searchedRedemptionList == null)
            {
                searchedRedemptionList = new List<RedemptionDTO>();
            }
            log.LogMethodExit(searchedRedemptionList);
            return searchedRedemptionList;
        }
        private async void OnUpdateClicked(object parameter)
        {
            log.LogMethodEntry(parameter);
            IsSuccess = false;
            RedemptionActivityDTO activityDTO = new RedemptionActivityDTO();
            activityDTO.Source = redemptionDTO.Source;
            activityDTO.Remarks = redemptionDTO.Remarks;
            redemptionDTO.RedemptionStatus = selectedRedemptionStatus; // From UI what is captured.
            RedemptionActivityDTO.RedemptionActivityStatusEnum activityStatusEnum;
            switch (selectedRedemptionStatus)
            {
                case "ABANDONED":
                    activityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.ABANDONED;
                    break;
                case "DELIVERED":
                    activityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.DELIVERED;
                    break;
                case "OPEN":
                    activityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.OPEN;
                    break;
                case "PREPARED":
                    activityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.PREPARED;
                    break;
                case "REVERSED":
                    activityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.REVERSED;
                    break;
                case "SUSPENDED":
                    activityDTO.Status = RedemptionActivityDTO.RedemptionActivityStatusEnum.SUSPENDED;
                    break;
            }
            string Remarks = ((String.IsNullOrEmpty(activityDTO.Remarks) == true ? "" : activityDTO.Remarks + " ") + ExecutionContext.GetUserId() + ": " + AddRemarks);
            activityDTO.Remarks = Remarks.Substring(((Remarks.Length-2000)<0)?0: (Remarks.Length - 2000), Remarks.Length);
            IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(ExecutionContext);
            try
            {
                redemptionDTO = await redemptionUseCases.UpdateRedemptionStatus(redemptionDTO.RedemptionId, activityDTO);
                IsSuccess = true;
                if (redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions != null && redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions.Count>0)
                {
                    int index = redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions.FindIndex(x => x.RedemptionId == redemptionDTO.RedemptionId);
                    if (index >= 0)
                    {
                        redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions.RemoveAt(index);
                        redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions.Insert(index, redemptionDTO);
                    }
                }
                if (redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList != null && redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList.Count>0)
                {
                    int index = redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList.FindIndex(x => x.RedemptionId == redemptionDTO.RedemptionId);
                    if (index >= 0)
                    {
                        redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList.RemoveAt(index);
                        redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList.Insert(index, redemptionDTO);
                    }
                }
                redemptionMainUserControlVM.RedemptionUserControlVM.SelectedRedemptionDTO = redemptionDTO;
                if (redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions != null && !redemptionMainUserControlVM.RedemptionUserControlVM.ShowSearchCloseIcon)
                {
                    redemptionMainUserControlVM.RedemptionUserControlVM.RenderCompleteValues(redemptionMainUserControlVM.RedemptionUserControlVM.TodayCompletedRedemptions);
                }
                else if (redemptionMainUserControlVM.RedemptionUserControlVM.ShowSearchCloseIcon && redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList != null)
                {
                    redemptionMainUserControlVM.RedemptionUserControlVM.SetCustomDataGridVM(completedOrSuspendedRedemptions: redemptionMainUserControlVM.RedemptionUserControlVM.RedemptionDTOList);
                }
                redemptionMainUserControlVM.RedemptionUserControlVM.SetOtherRedemptionList(RedemptionUserControlVM.ActionType.Complete, redemptionDTO);
            }
            catch (ValidationException vex)
            {
                log.Error(vex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(vex.Message.ToString(), MessageType.Error);
                }
            }
            catch (UnauthorizedException uaex)
            {
                log.Error(uaex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.ShowRelogin(ExecutionContext.GetUserId());
                }
            }
            catch (ParafaitApplicationException pax)
            {
                log.Error(pax);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(pax.Message.ToString(), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (redemptionMainUserControlVM != null)
                {
                    redemptionMainUserControlVM.SetFooterContent(MessageViewContainerList.GetMessage(ExecutionContext, "Something went wrong. Please try again." + ex.Message), MessageType.Error);
                }
            }
            updateClicked = true;
            CloseWindow(parameter);
            log.LogMethodExit();
        }

        private void CloseWindow(object parameter)
        {
            log.LogMethodEntry(parameter);
            if (parameter != null)
            {
                RedemptionUpdateView redemptionUpdateView = parameter as RedemptionUpdateView;
                if (redemptionUpdateView != null)
                {
                    redemptionUpdateView.Close();
                }
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor

        public RedemptionUpdateVM(ExecutionContext executionContext, RedemptionMainUserControlVM redemptionMainUserControlVM)
        {
            log.LogMethodEntry(executionContext, redemptionMainUserControlVM);
            ExecuteAction(() =>
            {
                this.ExecutionContext = executionContext;
                this.redemptionMainUserControlVM = redemptionMainUserControlVM;
                cancelCommand = new DelegateCommand(OnCancelClicked);
                updateCommand = new DelegateCommand(OnUpdateClicked);
                updateClicked = false;

                title = MessageViewContainerList.GetMessage(executionContext, "Redemption Update Details");
                heading = MessageViewContainerList.GetMessage(executionContext, "RO-");
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<List<RedemptionDTO>> task = GetRedemption(redemptionMainUserControlVM.RedemptionUserControlVM.SelectedRedemptionDTO.RedemptionId.ToString());
                    task.Wait();
                    if (task.Result != null)
                    {
                        redemptionDTO = task.Result.FirstOrDefault();
                    }
                    else
                    {
                        redemptionDTO = redemptionMainUserControlVM.RedemptionUserControlVM.SelectedRedemptionDTO;
                    }
                }
                if (redemptionDTO!=null && redemptionDTO.RedeemedDate != null && redemptionDTO.RedeemedDate != DateTime.MinValue)
                {
                    DateTime redeemdateTime= (DateTime)redemptionDTO.RedeemedDate;
                    RedeemedDate = redeemdateTime.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext,"DATETIME_FORMAT"));
                }
                if (redemptionDTO != null && redemptionDTO.OrderCompletedDate != null && redemptionDTO.OrderCompletedDate != DateTime.MinValue)
                {
                    DateTime prepdateTime = (DateTime)redemptionDTO.OrderCompletedDate;
                    PreparedDate = prepdateTime.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(ExecutionContext, "DATETIME_FORMAT"));
                }
                if (!string.IsNullOrEmpty(redemptionDTO.RedemptionOrderNo))
                {
                    heading = redemptionDTO.RedemptionOrderNo.ToString();
                }
                multiScreenMode = false;

                cancelButtonText = MessageViewContainerList.GetMessage(executionContext, "CANCEL");
                updateButtonText = MessageViewContainerList.GetMessage(executionContext, "UPDATE");

                redemptionStatus = new ObservableCollection<string>();

                if (redemptionDTO.RedemptionStatus.ToLower() == "open")
                {
                    redemptionStatus.Add(RedemptionDTO.RedemptionStatusEnum.OPEN.ToString());
                    redemptionStatus.Add(RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString());
                    redemptionStatus.Add(RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString());
                }
                else if (redemptionDTO.RedemptionStatus.ToLower() == "prepared")
                {
                    redemptionStatus.Add(RedemptionDTO.RedemptionStatusEnum.PREPARED.ToString());
                    redemptionStatus.Add(RedemptionDTO.RedemptionStatusEnum.DELIVERED.ToString());
                }
                else
                {
                    foreach (string s in Enum.GetNames(typeof(RedemptionDTO.RedemptionStatusEnum)))
                    {
                        redemptionStatus.Add(s);
                    }
                }
                selectedRedemptionStatus = redemptionDTO.RedemptionStatus;
                remarks = redemptionDTO.Remarks;
                isUpdateButtonEnable = true;
            });
            log.LogMethodExit();
        }

        #endregion

    }
}

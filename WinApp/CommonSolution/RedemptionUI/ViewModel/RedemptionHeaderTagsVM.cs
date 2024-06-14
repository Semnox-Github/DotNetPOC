/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Redemption - view model for header section
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/

using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using Semnox.Parafait.CommonUI;

namespace Semnox.Parafait.RedemptionUI
{
    public class RedemptionHeaderTagsVM : ViewModelBase
    {
        #region Members
        private bool multiScreenMode;
        private RedemptionHeaderTagsUserControl redemptionHeaderTagsUserControl;
        private RedemptionHeaderGroup selectedRedemptionHeaderGroup;
        private ObservableCollection<RedemptionHeaderGroup> headerGroups;
        private ICommand headerTagClickedCommand;
        private ICommand loadedCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
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

        public RedemptionHeaderGroup SelectedRedemptionHeaderGroup
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedRedemptionHeaderGroup);
                return selectedRedemptionHeaderGroup;
            }
        }

        public ObservableCollection<RedemptionHeaderGroup> HeaderGroups
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(headerGroups);
                return headerGroups;
            }
            set
            {
                log.LogMethodEntry(headerGroups, value);
                SetProperty(ref headerGroups, value);
                log.LogMethodExit(headerGroups);
            }
        }

        public ICommand HeaderTagClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(headerTagClickedCommand);
                return headerTagClickedCommand;
            }
            set
            {
                log.LogMethodEntry(headerTagClickedCommand, value);
                SetProperty(ref headerTagClickedCommand, value);
                log.LogMethodExit(headerTagClickedCommand);
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
                log.LogMethodEntry(loadedCommand, value);
                SetProperty(ref loadedCommand, value);
                log.LogMethodExit(loadedCommand);
            }
        }

        #endregion

        #region Methods
        private void OnHeaderTagClicked(object parameter)
        {
            log.LogMethodEntry();
            if (parameter != null)
            {
                RedemptionHeaderTag redemptionHeaderTag = parameter as RedemptionHeaderTag;
                if (redemptionHeaderTag != null)
                {
                    if (!redemptionHeaderTag.IsActive || !multiScreenMode)
                    {
                        foreach (RedemptionHeaderGroup redemptionHeaderGroup in headerGroups)
                        {
                            if (redemptionHeaderGroup.RedemptionHeaderTags.Contains(redemptionHeaderTag))
                            {
                                //redemptionHeaderGroup.RedemptionHeaderTags = new ObservableCollection<RedemptionHeaderTag>(redemptionHeaderGroup.RedemptionHeaderTags.Select
                                //    (h => { h.IsActive = false; return h; }));
                                //redemptionHeaderTag.IsActive = true;
                                redemptionHeaderGroup.SelectedRedemptionHeaderTag = redemptionHeaderTag;
                                this.selectedRedemptionHeaderGroup = redemptionHeaderGroup;
                                break;
                            }
                        }
                        if (redemptionHeaderTagsUserControl != null)
                        {
                            redemptionHeaderTagsUserControl.RaiseHeaderTagClickedEvent();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void OnLoaded(object parameter)
        {
            log.LogMethodEntry();
            if (parameter != null)
            {
                redemptionHeaderTagsUserControl = parameter as RedemptionHeaderTagsUserControl;
            }
            log.LogMethodExit();
        }
        #endregion

        #region Constructor
        public RedemptionHeaderTagsVM()
        {
            log.LogMethodEntry();
            headerGroups = new ObservableCollection<RedemptionHeaderGroup>();
            headerTagClickedCommand = new DelegateCommand(OnHeaderTagClicked);
            loadedCommand = new DelegateCommand(OnLoaded);

            multiScreenMode = false;

            log.LogMethodExit();
        }
        #endregion
    }
}

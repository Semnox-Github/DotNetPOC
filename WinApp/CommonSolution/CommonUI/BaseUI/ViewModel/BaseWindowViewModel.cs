/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Game Management View Model
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.ViewContainer;

namespace Semnox.Parafait.CommonUI
{
    public abstract class BaseWindowViewModel : ViewModelBase
    {
        #region Members
        private DisplayTagsVM displayTagsVM = null;
        private ContextSearchVM contextSearchVM;
        private GenericContentVM genericContentVM = null;
        private GenericRightSectionContentVM genericRightSectionContentVM = null;
        private FooterVM footerVM = null;
        private LeftPaneVM leftPaneVM = null;

        private ICommand leftPaneMenuSelectedCommand;
        private ICommand leftPaneNavigationClickedCommand;
        private ICommand searchCommand;
        private ICommand isSelectedCommand;
        private ICommand previousNavigationCommand;
        private ICommand nextNavigationCommand;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
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
                log.LogMethodEntry(value);
                SetProperty(ref displayTagsVM, value);
                log.LogMethodExit(displayTagsVM);
            }
        }

        public GenericRightSectionContentVM GenericRightSectionContentVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericRightSectionContentVM);
                return genericRightSectionContentVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref genericRightSectionContentVM, value);
                log.LogMethodExit(genericRightSectionContentVM);
            }
        }
        public ContextSearchVM ContextSearchVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(contextSearchVM);
                if (contextSearchVM == null)
                {
                    contextSearchVM = new ContextSearchVM(ExecutionContext);
                }
                return contextSearchVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref contextSearchVM, value);
            }
        }
        public GenericContentVM GenericContentVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(genericContentVM);
                return genericContentVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref genericContentVM, value);
                log.LogMethodExit(genericContentVM);
            }
        }

        public LeftPaneVM LeftPaneVM
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(leftPaneVM);
                return leftPaneVM;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref leftPaneVM, value);
                log.LogMethodExit(leftPaneVM);
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
                log.LogMethodEntry(value);
                SetProperty(ref footerVM, value);
                log.LogMethodExit(footerVM);
            }
        }

        public ICommand LeftPaneMenuSelectedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(leftPaneMenuSelectedCommand);
                return leftPaneMenuSelectedCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref leftPaneMenuSelectedCommand, value);
                log.LogMethodExit(leftPaneMenuSelectedCommand);
            }
        }

        public ICommand LeftPaneNavigationClickedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(leftPaneNavigationClickedCommand);
                return leftPaneNavigationClickedCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                SetProperty(ref leftPaneNavigationClickedCommand, value);
                log.LogMethodExit(leftPaneNavigationClickedCommand);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(searchCommand);
                return searchCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                searchCommand = value;
            }
        }

        public ICommand IsSelectedCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(isSelectedCommand);
                return isSelectedCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                isSelectedCommand = value;
            }

        }

        public ICommand PreviousNavigationCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(previousNavigationCommand);
                return previousNavigationCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                previousNavigationCommand = value;
            }
        }

        public ICommand NextNavigationCommand
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(nextNavigationCommand);
                return nextNavigationCommand;
            }
            set
            {
                log.LogMethodEntry(value);
                nextNavigationCommand = value;
            }
        }
        #endregion

        #region Methods
        public void SetFooterContent(string message, MessageType messageType)
        {
            log.LogMethodEntry(message);
            if (FooterVM != null)
            {
                FooterVM.Message = message;
                FooterVM.MessageType = messageType;
            }
            log.LogMethodExit();
        }

        internal ExecutionContext GetExecutionContext()
        {
            log.LogMethodEntry();
            log.LogMethodEntry(ExecutionContext);
            return ExecutionContext;
        }

        internal void SetGenericContentVM(List<DisplayValues> displayParams, ObservableCollection<ComboBoxField> comboBoxFields,
            ObservableCollection<string> headings, ObservableCollection<string> comboSearchHeadings,
            ObservableCollection<int> searchIndexes)
        {
            GenericContentVM = new GenericContentVM(ExecutionContext)
            {
                Headings = headings,

                DisplayParams = new ObservableCollection<DisplayValues>(displayParams),

                ComboSearchHeadings = comboSearchHeadings,

                ContextSearchVM = new ContextSearchVM(ExecutionContext)
                {
                    Heading = MessageViewContainerList.GetMessage(ExecutionContext, "Search", null),
                    SearchIndexes = searchIndexes
                },

                ComboGroupVM = new ComboGroupVM()
                {
                    ComboList = comboBoxFields
                }
            };
        }

        public void SetGenericRightSectionContent(string heading, string subHeading, ObservableCollection<RightSectionPropertyValues> rightSectionPropertValues,
            bool canPreviousExecute, bool canNextExecute)
        {
            if (GenericRightSectionContentVM != null)
            {
                GenericRightSectionContentVM.Heading = heading;
                GenericRightSectionContentVM.SubHeading = subHeading;
                GenericRightSectionContentVM.IsPreviousNavigationEnabled = canPreviousExecute;
                GenericRightSectionContentVM.IsNextNavigationEnabled = canNextExecute;
                GenericRightSectionContentVM.PropertyCollections = rightSectionPropertValues;
            }
        }

        internal void ExecuteActionWithFooter(Action method)
        {
            log.LogMethodEntry();
            try
            {
                method();
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                throw;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                SetFooterContent(ex.ToString(), MessageType.Error);
            }
            finally
            {
                log.LogMethodExit();
            }
        }
        #endregion

    }
}

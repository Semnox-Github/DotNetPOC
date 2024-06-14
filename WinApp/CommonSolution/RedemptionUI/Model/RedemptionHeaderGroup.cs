/********************************************************************************************
* Project Name - POS Redesign
* Description  - Redemption - model for redemption header group
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
********************************************************************************************/
using System.Collections.ObjectModel;

using Semnox.Parafait.CommonUI;

namespace Semnox.Parafait.RedemptionUI
{
    public class RedemptionHeaderGroup : ViewModelBase
    {
        #region Members
        private string userName;
        private ColorCode colorCode;
        private RedemptionHeaderTag selectedRedemptionHeaderTag;
        private ObservableCollection<RedemptionHeaderTag> redemptionHeaderTags;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public RedemptionHeaderTag SelectedRedemptionHeaderTag
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(selectedRedemptionHeaderTag);
                return selectedRedemptionHeaderTag;
            }
            set
            {
                log.LogMethodEntry(selectedRedemptionHeaderTag, value);
                SetProperty(ref selectedRedemptionHeaderTag, value);
                log.LogMethodExit(selectedRedemptionHeaderTag);
            }
        }

        public ColorCode ColorCode
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(colorCode);
                return colorCode;
            }
            set
            {
                log.LogMethodEntry(colorCode, value);
                SetProperty(ref colorCode, value);
                log.LogMethodExit(colorCode);
            }
        }

        public string UserName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(userName);
                return userName;
            }
            set
            {
                log.LogMethodEntry(userName, value);
                SetProperty(ref userName, value);
                log.LogMethodExit(userName);
            }
        }

        public ObservableCollection<RedemptionHeaderTag> RedemptionHeaderTags
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(redemptionHeaderTags);
                return redemptionHeaderTags;
            }
            set
            {
                log.LogMethodEntry(redemptionHeaderTags, value);
                SetProperty(ref redemptionHeaderTags, value);
                log.LogMethodExit(redemptionHeaderTags);
            }
        }
        #endregion

        #region Constructor
        public RedemptionHeaderGroup()
        {
            log.LogMethodEntry();
            userName = string.Empty;
            selectedRedemptionHeaderTag = null;
            redemptionHeaderTags = new ObservableCollection<RedemptionHeaderTag>();
            log.LogMethodExit();
        }
        #endregion
    }
}

/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - model for content area display
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.ObjectModel;

namespace Semnox.Parafait.CommonUI
{
    public class DisplayParameters
    {
        #region Members
        private int id;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ObservableCollection<string> displayNames;
        #endregion

        #region Properties
        public int Id
        {
            get
            {
                log.LogMethodEntry();
                return id;
            }
            set
            {
                log.LogMethodEntry(value);
                id = value;
            }
        }

        public ObservableCollection<string> ParameterNames
        {
            get
            {
                log.LogMethodEntry();
                return displayNames;
            }
            set
            {
                log.LogMethodEntry(value);
                displayNames = value;
            }
        }

        public string DisplayName
        {
            get
            {
                log.LogMethodEntry();
                return String.Join(" - ", displayNames);
            }
        }
        #endregion
    }
}

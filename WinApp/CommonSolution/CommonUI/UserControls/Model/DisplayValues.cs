/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Model for selected item on content area
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System.Collections.ObjectModel;

namespace Semnox.Parafait.CommonUI
{
    public enum ObjectType
    {
        Machine = 0,
        Hub = 1
    }

    public class DisplayValues
    {

        #region Members
        private ObjectType _objectType;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ObservableCollection<string> _displayItems;

        private int id;
        #endregion

        #region Properties
        public ObjectType ObjectType
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_objectType);
                return _objectType;
            }
            set
            {
                log.LogMethodEntry(value);
                _objectType = value;
            }
        }

        public int Id
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(id);
                return id;
            }
            set
            {
                log.LogMethodEntry();
                id = value;
            }
        }

        public ObservableCollection<string> DisplayItems
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_displayItems);
                return _displayItems;
            }
            set
            {
                log.LogMethodEntry(value);
                _displayItems = value;
            }
        }
        #endregion

    }
}

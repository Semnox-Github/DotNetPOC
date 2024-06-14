/******************************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - RightSection Property Values to show details of selected item in content area
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *******************************************************************************************************
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ******************************************************************************************************/
 namespace Semnox.Parafait.CommonUI
{
    public class RightSectionPropertyValues
    {
        #region Members
        private string _property;
        private string _value;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public string Property
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_property);
                return _property;
            }
            set
            {
                log.LogMethodEntry(value);
                _property = value;
            }
        }

        public string Value
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(_value);
                return _value;
            }
            set
            {
                log.LogMethodEntry(value);
                _value = value;
            }
        }
        #endregion
    }
}

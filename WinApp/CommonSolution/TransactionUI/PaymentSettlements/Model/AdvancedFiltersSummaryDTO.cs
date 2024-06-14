/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - AdvancedFiltersSummaryDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     17-Sep-2021    Fiona                  Created 
 ********************************************************************************************/
using Semnox.Parafait.CommonUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Semnox.Parafait.TransactionUI
{
    public class AdvancedFiltersSummaryDTO : ViewModelBase  
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string attributeDisplayName;
        private string condition;
        private Visibility addLaybelVisibility;
        private string attributeValue;
        private string enabledAttribute;
        private string value;

        public string EnabledAttributeName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return enabledAttribute;
            }
            set
            {
                log.LogMethodEntry();
                enabledAttribute = value;
                log.LogMethodExit();
            }
        }
        public string Condition
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(condition);
                return condition;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref condition, value);
                log.LogMethodExit();
            }
        }
        public string AttributeDisplayName
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(attributeDisplayName);
                return attributeDisplayName;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref attributeDisplayName, value);
                log.LogMethodExit();
            }
        }
        public Visibility AddLabelVisibility
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return addLaybelVisibility;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref addLaybelVisibility, value);
                log.LogMethodExit();
            }
        }
        public string AttributeValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit();
                return attributeValue;
            }
            set
            {
                log.LogMethodEntry();
                SetProperty(ref attributeValue, value);
                log.LogMethodExit();
            }
        }
        public string Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
        
    }
}

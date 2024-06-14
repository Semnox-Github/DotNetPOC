/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - model for generic data entry
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     12-Aug-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Semnox.Parafait.CommonUI
{
    public class CustomFamilyTypeFace
    {
        #region Members
        private FamilyTypeface familyTypeface;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Properties
        public FamilyTypeface FamilyTypeface
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(familyTypeface);
                return familyTypeface;
            }
            set
            {
                log.LogMethodEntry(familyTypeface,value);
                familyTypeface = value;
                log.LogMethodExit(familyTypeface);
            }
        }
        public string Style
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(familyTypeface);
                return familyTypeface.Style.ToString();
            }
        }
        public string Stretch
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(familyTypeface);
                return familyTypeface.Stretch.ToString();
            }
        }
        public string Weight
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(familyTypeface);
                return familyTypeface.Weight.ToString();
            }
        }
        public string CombinedValue
        {
            get
            {
                log.LogMethodEntry();
                log.LogMethodExit(familyTypeface);
                return familyTypeface.Style.ToString() + " - " + familyTypeface.Stretch.ToString() + " - " + familyTypeface.Weight.ToString();
            }
        }
        #endregion

        #region Methods
        #endregion

        #region Constructor
        public CustomFamilyTypeFace(FamilyTypeface familyTypeface)
        {
            log.LogMethodEntry();
            this.familyTypeface = familyTypeface;
            log.LogMethodExit();
        }
        #endregion
    }
}

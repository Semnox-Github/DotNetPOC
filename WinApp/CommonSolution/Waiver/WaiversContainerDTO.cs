/********************************************************************************************
* Project Name - WaiverSet
* Description  - WaiversContainerDTO class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Semnox.Parafait.Waiver
{
    public class WaiversContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string name;
        private string waiverFileName;
        private int? validForDays;
        private DateTime? effectiveDate;

        public WaiversContainerDTO()
        {
            log.LogMethodEntry();
            validForDays = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public WaiversContainerDTO( string name, string waiverFileName, int? validForDays,
                                   DateTime? effectiveDate)

            : this()
        {
            log.LogMethodEntry(name, waiverFileName, validForDays, effectiveDate);
         
            this.name = name;
            this.waiverFileName = waiverFileName;
            this.validForDays = validForDays;
            this.effectiveDate = effectiveDate;
          
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Get/Set method of the Waiver File Name field
        /// </summary>
        [DisplayName("Waiver File Name")]
        public string WaiverFileName { get { return waiverFileName; } set { waiverFileName = value; } }


        /// <summary>
        /// Get/Set method of the Valid For Days field
        /// </summary>
        [DisplayName("Valid For Days")]
        public int? ValidForDays { get { return validForDays; } set { validForDays = value; } }

        /// <summary>
        /// Get/Set method of the EffectiveDate field
        /// </summary>
        [DisplayName("Effective Date")]
        public DateTime? EffectiveDate { get { return effectiveDate; } set { effectiveDate = value; } }

    }
}

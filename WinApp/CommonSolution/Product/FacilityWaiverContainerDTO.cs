/********************************************************************************************
 * Project Name - Products
 * Description  - Data object of FacilityWaiverContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.130.00   16-Aug-2021    Prajwal S          Created                                                       
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    public class FacilityWaiverContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int facilityWaiverId;
        private int facilityId;
        private int waiverSetId;
        private DateTime? effectiveFrom;
        private DateTime? effectiveTo;
        private string guid;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public FacilityWaiverContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor with required data fields.
        /// </summary>
        public FacilityWaiverContainerDTO(int facilityWaiverId, int facilityId, int waiverSetId, DateTime? effectiveFrom, DateTime? effectiveTo, string guid)
            : this()
        {
            log.LogMethodEntry(facilityWaiverId, facilityId, waiverSetId, effectiveFrom, effectiveTo, guid);
            this.facilityWaiverId = facilityWaiverId;
            this.facilityId = facilityId;
            this.waiverSetId = waiverSetId;
            this.effectiveFrom = effectiveFrom;
            this.effectiveTo = effectiveTo;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the FacilityWaiverId field
        /// </summary>
        public int FacilityWaiverId { get { return facilityWaiverId; } set { facilityWaiverId = value; } }

        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        public int FacilityId { get { return facilityId; } set { facilityId = value; } }

        /// <summary>
        /// Get/Set method of the WaiverSetId field
        /// </summary>
        public int WaiverSetId { get { return waiverSetId; } set { waiverSetId = value; } }

        /// <summary>
        /// Get/Set method of the EffectiveFrom field
        /// </summary>
        public DateTime? EffectiveFrom { get { return effectiveFrom; } set { effectiveFrom = value; } }

        /// <summary>
        /// Get/Set method of the EffectiveFrom field
        /// </summary>
        public DateTime? EffectiveTo { get { return effectiveTo; } set { effectiveTo = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }

    }
}
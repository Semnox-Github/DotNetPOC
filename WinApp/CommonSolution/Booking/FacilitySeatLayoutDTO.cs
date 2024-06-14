/* Project Name - ReservationCoreDTO Programs 
* Description  - Data object of the FacilitySeatLayoutDTO
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Semnox.Parafait.Booking
{
    public class FacilitySeatLayoutDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int layoutId;
        int facilityId;
        string rowColumnName;
        char type;
        int rowColumnIndex;
        char hasSeats;
        string guid;
        bool synchStatus;
        int site_id;
        int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FacilitySeatLayoutDTO()
        {
            this.layoutId= -1;
            this.facilityId =-1;
            this.rowColumnName= "";
            this.site_id = -1;
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public FacilitySeatLayoutDTO(int layoutId, int facilityId, string rowColumnName, char type ,int rowColumnIndex,
                                        char hasSeats, string guid, bool synchStatus, int site_id, int masterEntityId)
        {
            log.Debug("Starts-FacilitySeatLayoutDTO(with all the data fields) Parameterized constructor.");
            this.layoutId=layoutId;
            this.facilityId = facilityId;
            this.rowColumnName=rowColumnName;
            this.type =type;
            this.rowColumnIndex=rowColumnIndex;
            this.hasSeats=hasSeats;
            this.guid=guid;
            this.synchStatus=synchStatus;
            this.site_id=site_id;
            this.masterEntityId=masterEntityId;
            log.Debug("Ends-FacilitySeatLayoutDTO(with all the data fields) Parameterized constructor.");
        }



        /// <summary>
        /// Get/Set method of the LayoutId field
        /// </summary>
        [DisplayName("LayoutId")]
        [DefaultValue(-1)]
        public int LayoutId { get { return layoutId; } set { layoutId= value; } }

        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        [DisplayName("FacilityId")]
        [DefaultValue(-1)]
        public int FacilityId { get { return facilityId; } set { facilityId = value; } }

        /// <summary>
        /// Get/Set method of the RowColumnName field
        /// </summary>
        [DisplayName("RowColumnName")]
        [DefaultValue("")]
        public string RowColumnName { get { return rowColumnName; } set { rowColumnName= value; } }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        [DisplayName("Type")]
        public char Type { get { return type; } set { type = value; } }
         
        /// <summary>
        /// Get/Set method of the RowColumnIndex field
        /// </summary>
        [DisplayName("RowColumnIndex")]
        [DefaultValue(-1)]
        public int RowColumnIndex  { get { return rowColumnIndex; } set { rowColumnIndex= value; } }

        /// <summary>
        /// Get/Set method of the HasSeats field
        /// </summary>
        [DisplayName("HasSeats")]
        public char HasSeats  { get { return hasSeats; } set { hasSeats= value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid= value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus= value; } }

        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        [DefaultValue(-1)]
        public int Site_id { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [DefaultValue(-1)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId= value; } }
        
    }
}

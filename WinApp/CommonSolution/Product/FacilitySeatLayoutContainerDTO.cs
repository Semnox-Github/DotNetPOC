/********************************************************************************************
 * Project Name - Products
 * Description  - Data object of FacilitySeatLayoutContainer
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
    public class FacilitySeatLayoutContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int layoutId;
        private int facilityId;
        private string rowColumnName;
        private char type;
        private int rowColumnIndex;
        private char hasSeats;
        private string guid;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FacilitySeatLayoutContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all Container fields
        /// </summary>
        public FacilitySeatLayoutContainerDTO(int layoutId, int facilityId, string rowColumnName, char type, int rowColumnIndex,
                                       char hasSeats, string guid)
            : this()
        {
            log.LogMethodEntry(layoutId, facilityId, rowColumnName, type, rowColumnIndex, hasSeats, guid);
            this.layoutId = layoutId;
            this.facilityId = facilityId;
            this.rowColumnName = rowColumnName;
            this.type = type;
            this.rowColumnIndex = rowColumnIndex;
            this.hasSeats = hasSeats;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LayoutId field
        /// </summary>
        public int LayoutId { get { return layoutId; } set { layoutId = value;} }

        /// <summary>
        /// Get/Set method of the FacilityId field
        /// </summary>
        public int FacilityId { get { return facilityId; } set { facilityId = value;} }

        /// <summary>
        /// Get/Set method of the RowColumnName field
        /// </summary>
        public string RowColumnName { get { return rowColumnName; } set { rowColumnName = value;} }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        public char Type { get { return type; } set { type = value;} }

        /// <summary>
        /// Get/Set method of the RowColumnIndex field
        /// </summary>
        public int RowColumnIndex { get { return rowColumnIndex; } set { rowColumnIndex = value;} }

        /// <summary>
        /// Get/Set method of the HasSeats field
        /// </summary>
        public char HasSeats { get { return hasSeats; } set { hasSeats = value;} }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { guid = value;} }

    }
}

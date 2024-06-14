
/********************************************************************************************
 * Project Name - 
 * Description  - DayLookupDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.2       18-Sep-2019    Dakshakh         Modified : Added logger
 ********************************************************************************************/
using System;
using System.ComponentModel;
namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    ///  This is the Day lookup value data object class. This acts as data holder for the lookup value business object
    /// </summary>   
    /// 
    public class DayLookupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByLookupValuesParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        int day;
        string display;
        /// <summary>
        /// Default constructor
        /// </summary>
        public DayLookupDTO()
        {
            log.LogMethodEntry();
            day = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DayLookupDTO(int day, string display)
        {
            log.LogMethodEntry(day, display);
            this.day = day;
            this.display = display;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Day field
        /// </summary>
        [DisplayName("Day")]
        public int Day { get { return day; } set { day = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LookupName field
        /// </summary>
        [DisplayName("Display")]
        public string Display { get { return display; } set { display = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || day < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

    }
}

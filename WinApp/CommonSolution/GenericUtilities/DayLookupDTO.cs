/********************************************************************************************
 * Project Name - DayLookupDTO
 * Description  - Data object of DayLookupDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019     Dakshakh raj     Modified : Logs
 ********************************************************************************************/
using System;
using System.ComponentModel;
namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    ///  This is the Day lookup value data object class. This acts as data holder for the lookup value business object
    /// </summary>   
    /// 
    public class DayLookupDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
        /// <param name="day">day</param>
        /// <param name="display">display</param>
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

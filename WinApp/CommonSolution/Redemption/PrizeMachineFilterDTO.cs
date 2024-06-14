/********************************************************************************************
 * Project Name - PrizeMachineFilter DTO
 * Description  - Data object of PrizeMachineFilter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        11-May-2017   Lakshminarayana          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// This is the PrizeMachineFilter data object class.
    /// </summary>
    public class PrizeMachineFilterDTO
    {
        DateTime startOfPeriod;
        DateTime endOfPeriod;
        string dispenseCategory;
        List<string> machineList;

        /// <summary>
        /// Get/Set method of the StartOfPeriod field
        /// </summary>
        public DateTime StartOfPeriod
        {
            get
            {
                return startOfPeriod;
            }

            set
            {
                startOfPeriod = value;
            }
        }

        /// <summary>
        /// Get/Set method of the EndOfPeriod field
        /// </summary>
        public DateTime EndOfPeriod
        {
            get
            {
                return endOfPeriod;
            }

            set
            {
                endOfPeriod = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DispenseCategory field
        /// </summary>
        public string DispenseCategory
        {
            get
            {
                return dispenseCategory;
            }

            set
            {
                dispenseCategory = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MachineList field
        /// </summary>
        public List<string> MachineList
        {
            get
            {
                return machineList;
            }

            set
            {
                machineList = value;
            }
        }
    }
}

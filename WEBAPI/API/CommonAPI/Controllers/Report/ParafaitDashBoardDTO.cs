/********************************************************************************************
 * Project Name - Reports
 * Description  - DataObject of ParafaitDashBoardBL for Reports module
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80        10-Jun-2020   Vikas Dwivedi        Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Semnox.CommonAPI.Reports
{
    public class ParafaitDashBoardDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private string posMachine;
        private string mode;
        private decimal? voidAmount;
        private decimal? netAmount;
        private decimal? tax;
        private decimal? grossTotal;

        public ParafaitDashBoardDTO()
        {
            log.LogMethodEntry();
            posMachine = string.Empty;
            mode = string.Empty;
            voidAmount = null;
            netAmount = null;
            tax = null;
            grossTotal = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PosMachine field
        /// </summary>
        [DisplayName("PosMachine")]
        public string PosMachine
        {
            get
            {
                return posMachine;
            }
            set
            {
                posMachine = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Mode field
        /// </summary>
        [DisplayName("Mode")]
        public string Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Mode field
        /// </summary>
        [DisplayName("VoidAmount")]
        public decimal? VoidAmount
        {
            get
            {
                return voidAmount;
            }
            set
            {
                voidAmount = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Mode field
        /// </summary>
        [DisplayName("NetAmount")]
        public decimal? NetAmount
        {
            get
            {
                return netAmount;
            }
            set
            {
                netAmount = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the Mode field
        /// </summary>
        [DisplayName("Tax")]
        public decimal? Tax
        {
            get
            {
                return tax;
            }
            set
            {
                tax = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the Mode field
        /// </summary>
        [DisplayName("GrossTotal")]
        public decimal? GrossTotal
        {
            get
            {
                return grossTotal;
            }
            set
            {
                grossTotal = value;
                this.IsChanged = true;
            }
        }

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
                    return notifyingObjectIsChanged;
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
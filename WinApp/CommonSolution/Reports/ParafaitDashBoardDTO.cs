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
using System.ComponentModel;

namespace Semnox.Parafait.Reports
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

        private string totalCards;
        private string issuedCards;
        private string balanceCards;
        private string avgDaliyCardRequirement;
        private string recomReorderDate;

        private string totalCustomer;
        private string avgCustomerPerDay;
        private string avgCollectionPerCustomer;
        private string totalPlayCount;
        private string avgPlayCountPerCustomer;

        private string machineName;
        private string machineAddress;
        private int totalPlays;
        private decimal? totalAmount;
        private double? daliyPlayCount;
        private double? daliyCollection;
        

        public ParafaitDashBoardDTO()
        {
            log.LogMethodEntry();
            posMachine = string.Empty;
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
                
            }
        }

        /// <summary>
        /// Get/Set method of the TotalCards field
        /// </summary>
        [DisplayName("TotalCards")]
        public string TotalCards
        {
            get
            {
                return totalCards;
            }
            set
            {
                totalCards = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IssuedCards field
        /// </summary>
        [DisplayName("IssuedCards")]
        public string IssuedCards
        {
            get
            {
                return issuedCards;
            }
            set
            {
                issuedCards = value;
            }
        }

        /// <summary>
        /// Get/Set method of the BalanceCards field
        /// </summary>
        [DisplayName("BalanceCards")]
        public string BalanceCards
        {
            get
            {
                return balanceCards;
            }
            set
            {
                balanceCards = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AvgDaliyCardRequirement field
        /// </summary>
        [DisplayName("AvgDaliyCardRequirement")]
        public string AvgDaliyCardRequirement
        {
            get
            {
                return avgDaliyCardRequirement;
            }
            set
            {
                avgDaliyCardRequirement = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RecomReorderDate field
        /// </summary>
        [DisplayName("RecomReorderDate")]
        public string RecomReorderDate
        {
            get
            {
                return recomReorderDate;
            }
            set
            {
                recomReorderDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TotalCustomer field
        /// </summary>
        [DisplayName("TotalCustomer")]
        public string TotalCustomer
        {
            get
            {
                return totalCustomer;
            }
            set
            {
                totalCustomer = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AvgCustomerPerDay field
        /// </summary>
        [DisplayName("AvgCustomerPerDay")]
        public string AvgCustomerPerDay
        {
            get
            {
                return avgCustomerPerDay;
            }
            set
            {
                avgCustomerPerDay = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AvgCollectionPerCustomer field
        /// </summary>
        [DisplayName("AvgCollectionPerCustomer")]
        public string AvgCollectionPerCustomer
        {
            get
            {
                return avgCollectionPerCustomer;
            }
            set
            {
                avgCollectionPerCustomer = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TotalPlayCount field
        /// </summary>
        [DisplayName("TotalPlayCount")]
        public string TotalPlayCount
        {
            get
            {
                return totalPlayCount;
            }
            set
            {
                totalPlayCount = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the AvgPlayCountPerCustomer field
        /// </summary>
        [DisplayName("AvgPlayCountPerCustomer")]
        public string AvgPlayCountPerCustomer
        {
            get
            {
                return avgPlayCountPerCustomer;
            }
            set
            {
                avgPlayCountPerCustomer = value;
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
            }
        }
    }
}

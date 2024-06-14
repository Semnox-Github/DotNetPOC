using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Tags
{
    
        /// <summary>
        /// Summary description for PurchasesStruct
        /// </summary>
        public class PurchasesStruct
        {
            private string purchasedDate;
            private int purchaseProductId;
            private string purchaseProductName;
            private string purchaseAmount;
            private string purchaseCredits;
            private string purchaseBonus;
            private string purchaseCourtesy;
            private string purchaseTickets;

            /// <summary>
            /// Default Constructor
            /// </summary>
            public PurchasesStruct()
            {
            }

            /// <summary>
            /// Parameterized Contructor
            /// </summary>
            public PurchasesStruct(int purchaseIdPassed, string purchaseDatePassed, string productNamePassed, string purchaseAmountPassed, string purchaseCreditsPassed, string purchaseBonusPassed, string purchaseCourtesyPassed, string purchaseTicketsPassed)
            {
                purchaseProductId = purchaseIdPassed;
                purchasedDate = purchaseDatePassed;
                purchaseProductName = productNamePassed;
                purchaseAmount = purchaseAmountPassed;
                purchaseCredits = purchaseCreditsPassed;
                purchaseBonus = purchaseBonusPassed;
                purchaseCourtesy = purchaseCourtesyPassed;
                purchaseTickets = purchaseTicketsPassed;
            }

            /// <summary>
            /// Get/Set method of the PurchasedDate field
            /// </summary>
            public string PurchasedDate { get { return purchasedDate; } set { purchasedDate = value; } }

            /// <summary>
            /// Get/Set method of the PurchasedProductId field
            /// </summary>
            public int PurchasedProductId { get { return purchaseProductId; } set { purchaseProductId = value; } }

            /// <summary>
            /// Get/Set method of the PurchasedProductName field
            /// </summary>
            public string PurchasedProductName { get { return purchaseProductName; } set { purchaseProductName = value; } }

            /// <summary>
            /// Get/Set method of the PurchaseAmount field
            /// </summary>
            public string PurchaseAmount { get { return purchaseAmount; } set { purchaseAmount = value; } }

            /// <summary>
            /// Get/Set method of the PurchaseCredits field
            /// </summary>
            public string PurchaseCredits { get { return purchaseCredits; } set { purchaseCredits = value; } }

            /// <summary>
            /// Get/Set method of the PurchaseBonus field
            /// </summary>
            public string PurchaseBonus { get { return purchaseBonus; } set { purchaseBonus = value; } }

            /// <summary>
            /// Get/Set method of the PurchaseCourtesy field
            /// </summary>
            public string PurchaseCourtesy { get { return purchaseCourtesy; } set { purchaseCourtesy = value; } }

            /// <summary>
            /// Get/Set method of the PurchaseTickets field
            /// </summary>
            public string PurchaseTickets { get { return purchaseTickets; } set { purchaseTickets = value; } }


        
    }
}
/********************************************************************************************
* Project Name - Loyalty
* Description  - ThirdPartyLoyalty - Factory class for the Loyalty programs
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
*2.120.0     12-Dec-2020      Girish Kundar       Created
*********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ThirdParty.ThirdPartyLoyalty
{
    /// <summary>
    /// ThirdPartyLoyalty
    /// </summary>
    public enum ThirdPartyLoyalty
    {
        /// <summary>
        /// No Fiscal printer.
        /// </summary>
        None,
        /// <summary>
        /// ALOHA Loyalty program
        /// </summary>
        ALOHA,
        /// <summary>
        /// PUNCHH Loyalty program
        /// </summary>
        PUNCHH,
        /// <summary>
        /// CAPILARY Loyalty program
        /// </summary>
        CAPILLARY
    }

    /// <summary>
    /// ThirdPartyLoyaltyFactory
    /// </summary>
    public class ThirdPartyLoyaltyFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static ThirdPartyLoyaltyFactory thirdPartyLoyaltyFactory;
        protected Dictionary<ThirdPartyLoyalty, LoyaltyPrograms> loyaltyProgramsDictionary=null;

        /// <summary>
        /// Parafait utilities
        /// </summary>
        protected Utilities utilities = null;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="utilities"></param>
        public virtual void Initialize(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            if (this.utilities == null)
            {
                this.utilities = utilities;
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// GetInstance
        /// </summary>
        /// <returns></returns>
        public static ThirdPartyLoyaltyFactory GetInstance()
        {
            log.LogMethodEntry();
            if (thirdPartyLoyaltyFactory == null)
            {
                thirdPartyLoyaltyFactory = new ThirdPartyLoyaltyFactory();
            }
            log.LogMethodExit(thirdPartyLoyaltyFactory);
            return thirdPartyLoyaltyFactory;
        }

        /// <summary>
        /// GetLoyaltyProgram
        /// </summary>
        /// <param name="loyaltyProgramsString"></param>
        /// <returns></returns>
        public LoyaltyPrograms GetLoyaltyProgram(string loyaltyProgramsString)
        {
            log.LogMethodEntry(loyaltyProgramsString);
            ThirdPartyLoyalty thirdPartyLoyalty;
            LoyaltyPrograms loyaltyPrograms = null;

            if (Enum.TryParse<ThirdPartyLoyalty>(loyaltyProgramsString, out thirdPartyLoyalty))
            {
                if (loyaltyProgramsDictionary == null)
                {
                    loyaltyProgramsDictionary = new Dictionary<ThirdPartyLoyalty, LoyaltyPrograms>();
                }

                if (loyaltyProgramsDictionary.ContainsKey(thirdPartyLoyalty))
                {
                    loyaltyPrograms = loyaltyProgramsDictionary[thirdPartyLoyalty];
                }
                else
                {
                    loyaltyPrograms = GetThirdPartyLoyaltyInstance(thirdPartyLoyalty);
                    if (loyaltyPrograms == null)
                    {
                        log.LogMethodExit(null, "LoyaltyPrograms Configuration Exception - Error occured while creating the loyaltyPrograms. type: " + thirdPartyLoyalty.ToString());
                    }
                    else
                    {
                        loyaltyProgramsDictionary.Add(thirdPartyLoyalty, loyaltyPrograms);
                    }
                }
            }
            else
            {
                log.LogMethodExit(null, "loyalty Programs Configuration Exception - ThirdPartyLoyalty enum not configured with loyalty Programs name: " + thirdPartyLoyalty.ToString());
            }

            log.LogMethodExit(loyaltyPrograms);
            return loyaltyPrograms;
        }

        /// <summary>
        /// GetThirdPartyLoyaltyInstance
        /// </summary>
        /// <param name="thirdPartyLoyaltyName"></param>
        /// <returns></returns>
        private LoyaltyPrograms GetThirdPartyLoyaltyInstance(ThirdPartyLoyalty thirdPartyLoyaltyName)
        {
            log.LogMethodEntry(thirdPartyLoyaltyName);
            LoyaltyPrograms loyaltyProgram = null;

            switch (thirdPartyLoyaltyName)
            {
                case ThirdPartyLoyalty.None:
                    {
                        loyaltyProgram = new LoyaltyPrograms(utilities);
                        break;
                    }
                case ThirdPartyLoyalty.ALOHA:
                    {
                        loyaltyProgram = new AlohaBL(utilities);
                        break;
                    }
                case ThirdPartyLoyalty.CAPILLARY:
                    {
                        loyaltyProgram = new Capillary(utilities);
                        break;
                    }
                case ThirdPartyLoyalty.PUNCHH:
                    {
                        loyaltyProgram = new PunchhBL(utilities);
                        break;
                    }
            }
            log.LogMethodExit(loyaltyProgram);
            return loyaltyProgram;
        }
    }
}
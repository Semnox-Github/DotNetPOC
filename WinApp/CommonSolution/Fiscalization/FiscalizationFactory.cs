/********************************************************************************************
 * Project Name - Fiscalization
 * Description  - Class for FiscalizationFactory 
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *2.140.0     03-Dec-2021       Dakshakh           Created 
 *2.150.5     20-Jun-2023       Guru S A           Modified for reprocessing improvements  
 *2.155.1     13-Aug-2023       Guru S A           Modified for Chile fiscalization       
 ********************************************************************************************/
using System;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;

namespace Semnox.Parafait.Fiscalization
{
    /// <summary>
    /// Fiscalizations
    /// </summary>
    public enum ParafaitFiscalizationNames
    {
        /// <summary>
        /// None.
        /// </summary>
        None,
        /// <summary>
        /// Ecuador fiscalization
        /// </summary>
        ECUADOR,
        /// <summary>
        /// PERU fiscalization
        /// </summary>
        PERU,
        /// <summary>
        /// CHILE fiscalization
        /// </summary>
        CHILE
    }
    
    public static class FiscalizationFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Parafait Fiscalizations
        /// </summary>
        /// <param name="parafaitFiscalizationsName"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ParafaitFiscalization GetParafaitFiscalizations(ParafaitFiscalizationNames parafaitFiscalizationsName, ExecutionContext executionContext, Utilities utilities)
        {
            log.LogMethodEntry(parafaitFiscalizationsName, executionContext, "utilities");
            ParafaitFiscalization parafaitFiscalization = null;
            switch (parafaitFiscalizationsName)
            {
                case ParafaitFiscalizationNames.None:
                    {
                        string message = "Please check the fiscalization setup";
                        throw new Exception(message);
                    }
                case ParafaitFiscalizationNames.ECUADOR:
                    {
                        parafaitFiscalization = new EcuadorFiscalization(executionContext, utilities);
                        break;
                    }
                case ParafaitFiscalizationNames.PERU:
                    {
                        parafaitFiscalization = new PeruFiscalization(executionContext, utilities);
                        break;
                    }
                case ParafaitFiscalizationNames.CHILE:
                    {
                        parafaitFiscalization = new ChileFiscalization(executionContext, utilities);
                        break;
                    }
            }
            log.LogMethodExit(parafaitFiscalization);
            return parafaitFiscalization;
        }
        /// <summary>
        /// Get Parafait Fiscalization List
        /// </summary>
        /// <param name="parafaitFiscalizationsName"></param>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static ParafaitFiscalizationList GetParafaitFiscalizationList(ParafaitFiscalizationNames parafaitFiscalizationsName, ExecutionContext executionContext, Utilities utilities)
        {
            log.LogMethodEntry(parafaitFiscalizationsName, executionContext, "utilities");
            ParafaitFiscalizationList parafaitFiscalizationList = null;
            switch (parafaitFiscalizationsName)
            {
                case ParafaitFiscalizationNames.None:
                    {
                        string message = "Please check the fiscalization setup";
                        throw new Exception(message);
                    }
                case ParafaitFiscalizationNames.ECUADOR:
                    {
                        parafaitFiscalizationList = new EcuadorFiscalizationList(executionContext, utilities);
                        break;
                    }
                case ParafaitFiscalizationNames.PERU:
                    {
                        parafaitFiscalizationList = new PeruFiscalizationList(executionContext, utilities);
                        break;
                    }
                case ParafaitFiscalizationNames.CHILE:
                    {
                        parafaitFiscalizationList = new ChileFiscalizationList(executionContext, utilities);
                        break;
                    }
            }
            log.LogMethodExit(parafaitFiscalizationList);
            return parafaitFiscalizationList;
        }
        /// <summary>
        /// GetParafaitFiscalizationList
        /// </summary>
        /// <param name="fiscalization"></param>
        /// <returns></returns>
        public static ParafaitFiscalizationNames GetParafaitFiscalizationNames(string fiscalization)
        {
            log.LogMethodEntry(fiscalization);
            ParafaitFiscalizationNames parafaitFiscalizationName = ParafaitFiscalizationNames.None;
            string sValue = fiscalization.ToUpper();
            switch (sValue)
            {
                case "PERU":
                    parafaitFiscalizationName = ParafaitFiscalizationNames.PERU;
                    break;
                case "ECUADOR":
                    parafaitFiscalizationName = ParafaitFiscalizationNames.ECUADOR;
                    break;
                case "CHILE":
                    parafaitFiscalizationName = ParafaitFiscalizationNames.CHILE;
                    break;
                default:
                    break;

            }
            log.LogMethodExit();
            return parafaitFiscalizationName;
        }
    }
}

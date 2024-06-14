/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - AdjustmentDTO class - This would return adjustment types
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    /// <summary>
    /// This class holds the Adjustments configuarations
    /// </summary>
    public class CapabilityAdjustments
    {
        private static readonly  logging.Logger log = new  logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int maxAdjustmentsPerTransaction;
        private List<List<string>> allowedAdjCombinations;

        public CapabilityAdjustments()
        {
            log.LogMethodEntry();
            maxAdjustmentsPerTransaction = 3;
            allowedAdjCombinations = new List<List<string>>();
            log.LogMethodExit();
        }

        public CapabilityAdjustments(int maxAdjustmentsPerTransaction)
            :this()
        {
            log.LogMethodEntry();
            this.maxAdjustmentsPerTransaction = maxAdjustmentsPerTransaction;
            string[] list = Enum.GetNames(typeof(AdjustmentTypes));
            List<string> addStringList = new List<string>();
            List<string> removeStringList = new List<string>();
            foreach(string adjustmentTypes in list)
            {
                if (adjustmentTypes.StartsWith("add"))
                {
                    addStringList.Add(adjustmentTypes);
                }
                else if (adjustmentTypes.StartsWith("remove"))
                {
                    removeStringList.Add(adjustmentTypes);
                }
            }
            allowedAdjCombinations.Add(addStringList);
            allowedAdjCombinations.Add(removeStringList);
            log.LogMethodExit();
        }
        public int maximumAdjustmentsPerTransaction { get { return maxAdjustmentsPerTransaction; } set { maxAdjustmentsPerTransaction = value; } }
        public List<List<string>> allowedAdjustmentCombinations { get { return allowedAdjCombinations; } set { allowedAdjCombinations = value; } }
    }
}

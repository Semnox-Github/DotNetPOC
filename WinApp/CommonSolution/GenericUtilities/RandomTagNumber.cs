/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Represents a random tag number
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        5-Jul-2019      Lakshminarayana     Created 
 ********************************************************************************************/

using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Represents a random tag number
    /// </summary>
    public class RandomTagNumber : TagNumber
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public RandomTagNumber(ExecutionContext executionContext) :
            base(executionContext, GetRandomTagNumberString(executionContext))
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="tagNumberLengthList"></param>
        public RandomTagNumber(ExecutionContext executionContext, TagNumberLengthList tagNumberLengthList) :
            base(executionContext, GetRandomTagNumberString(executionContext, tagNumberLengthList), tagNumberLengthList)
        {
            log.LogMethodEntry(executionContext, tagNumberLengthList);
            log.LogMethodExit();
        }

        private static string GetRandomTagNumberString(ExecutionContext executionContext)
        {
            return GetRandomTagNumberString(executionContext, new TagNumberLengthList(executionContext));
        }

        private static string GetRandomTagNumberString(ExecutionContext executionContext,
            TagNumberLengthList tagNumberLengthList)
        {
            log.LogMethodEntry(executionContext, tagNumberLengthList);

            RandomString randomTagNumberString = new RandomString(tagNumberLengthList.MinimumValidTagNumberLength,
                "1234567890GHIJKLMNOPQRSUVWXYZ");
            log.LogMethodExit(randomTagNumberString.Value);
            return randomTagNumberString.Value;
        }
    }
}
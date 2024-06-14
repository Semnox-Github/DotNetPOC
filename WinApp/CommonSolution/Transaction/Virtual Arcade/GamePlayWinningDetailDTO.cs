/********************************************************************************************
 * Project Name - GamePlayWinnings DTO                                                                         
 * Description  - Dto to hold the customer winnings details for each level 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/
namespace Semnox.Parafait.Transaction.VirtualArcade
{
    /// <summary>
    /// This class hold the customer winnigs details like with creditplus types and values
    /// </summary>
    public class GamePlayWinningDetailDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Any other fields from credit plus lines can be added
        private string creditPlusType;
        private decimal creditPlus;

        /// <summary>
        /// GamePlayWinningDetailDTO
        /// </summary>
        public GamePlayWinningDetailDTO()
        {
            creditPlus = 0;
            creditPlusType = string.Empty;
        }


        /// <summary>
        /// GamePlayWinningDetailDTO
        /// </summary>
        /// <param name="creditPlusType"></param>
        /// <param name="creditPlus"></param>
        public GamePlayWinningDetailDTO(string creditPlusType, decimal creditPlus)
        {
            log.LogMethodEntry(creditPlusType, creditPlus);
            this.creditPlusType = creditPlusType;
            this.creditPlus = creditPlus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return creditPlusType; } set { creditPlusType = value; } }

        /// <summary>
        /// Value
        /// </summary>
        public decimal Value { get { return creditPlus; } set { creditPlus = value; } }
    }
}
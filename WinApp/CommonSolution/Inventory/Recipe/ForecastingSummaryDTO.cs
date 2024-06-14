/********************************************************************************************
 * Project Name - Inventory
 * Description  - Forecasting Summary DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0     15-Sep-2020   Deeksha                 Created for Recipe management enhancement
 ********************************************************************************************/

namespace Semnox.Parafait.Inventory.Recipe
{
    
    public class ForecastingSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ForecastingSummaryDTO()
        {
            log.LogMethodEntry();
            ProductId = -1;
            RecipeName = null;
            Stock = 0;
            EstimatedMonthlyQuantity = 0;
            EstimatedWeeklyQuantity = 0;
            PlannedQuantity = 0;
            ProducedQuantity = 0;
            EstimationDetails = null;
            UOM = null;
            log.LogMethodExit();
        }

        public int ProductId { get; set; }
        public string RecipeName { get; set; }
        public decimal? Stock { get; set; }
        public decimal? EstimatedMonthlyQuantity { get; set; }
        public decimal? EstimatedWeeklyQuantity { get; set; }
        public decimal? PlannedQuantity { get; set; }
        public decimal? ProducedQuantity { get; set; }
        public string EstimationDetails { get; set; }
        public string UOM { get; set; }
    }
}

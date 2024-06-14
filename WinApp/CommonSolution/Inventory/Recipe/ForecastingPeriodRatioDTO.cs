/********************************************************************************************
* Project Name - Forecasting Period Ratio BL 
* Description  - Class to hold Forecasting data based on the  historical Days value.
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*2.100.0    26-Sep-20      Deeksha              Created 
********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Inventory.Recipe
{

    public class ForecastingPeriodRatioDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int dataPoints;
        private double historicalDays;
        private double ratio;
        private AccountingCalendarMasterDTO accountingCalendarMasterDTO;

         public int DataPoints { get { return dataPoints; } set { dataPoints = value; } }

        public double Ratio { get { return ratio; } set { ratio = value; } }

        public double HistoricalDays { get { return historicalDays; } set { historicalDays = value; } }

        public decimal? Period30 { get { return period30; } set { period30 = value; } }

        public decimal? Period60 { get { return period60; } set { period60 = value; } }

        public decimal? Period90 { get { return period90; } set { period90 = value; } }

        public decimal? Period120 { get { return period120; } set { period120 = value; } }

        public decimal? Period150 { get { return period150; } set { period150 = value; } }
        public decimal? Period180 { get { return period180; } set { period180 = value; } }
        public decimal? Period210 { get { return period210; } set { period210 = value; } }
        public decimal? Period240 { get { return period240; } set { period240 = value; } }
        public decimal? Period270 { get { return period270; } set { period270 = value; } }
        public decimal? Period300 { get { return period300; } set { period300 = value; } }
        public decimal? Period330 { get { return period330; } set { period330 = value; } }
        public decimal? Period365 { get { return period365; } set { period365 = value; } }


        private int periodDataPointsId;
        private decimal? period30;
        private decimal? period60;
        private decimal? period90;
        private decimal? period120;
        private decimal? period150;
        private decimal? period180;
        private decimal? period210;
        private decimal? period240;
        private decimal? period270;
        private decimal? period300;
        private decimal? period330;
        private decimal? period365;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;


        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  PERIOD DATA POINTS field
            /// </summary>
            PERIOD_DATA_POINTS,
            /// <summary>
            /// Search by  PERIOD 30 field
            /// </summary>
            PERIOD_30,

            /// <summary>
            /// Search by  PERIOD 60 field
            /// </summary>
            PERIOD_60,

            /// <summary>
            /// Search by  PERIOD 90 field
            /// </summary>
            PERIOD_90,

            /// <summary>
            /// Search by  PERIOD 120 field
            /// </summary>
            PERIOD_120,

            /// <summary>
            /// Search by PERIOD 150 field
            /// </summary>
            PERIOD_150,

            /// <summary>
            /// Search by PERIOD 180 field
            /// </summary>
            PERIOD_180,
            /// <summary>
            /// Search by PERIOD 210 field
            /// </summary>
            PERIOD_210,
            /// <summary>
            /// Search by PERIOD 240 field
            /// </summary>
            PERIOD_240,
            /// <summary>
            /// Search by PERIOD 270 field
            /// </summary>

            PERIOD_270,
            /// <summary>
            /// Search by PERIOD 300 field
            /// </summary>

            PERIOD_300,
            /// <summary>
            /// Search by PERIOD 330 field
            /// </summary>

            PERIOD_330,
            /// <summary>
            /// Search by PERIOD 365 field
            /// </summary>

            PERIOD_365,
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        public ForecastingPeriodRatioDTO(int periodDataPointsId,int dataPoints, decimal? period30, decimal? period60, decimal? period90, decimal? period120, decimal? period150, decimal? period180,
                                        decimal? period210, decimal? period240, decimal? period270, decimal? period300, decimal? period330, decimal? period365)
        {
            log.LogMethodEntry();
            this.periodDataPointsId = periodDataPointsId;
            this.dataPoints = dataPoints;
            this.period30 = period30;
            this.period60 = period60;
            this.period90 = period90;
            this.period120 = period120;
            this.period150 = period150;
            this.period180 = period180;
            this.period210 = period210;
            this.period240 = period240;
            this.period270 = period270;
            this.period300 = period300;
            this.period330 = period330;
            this.period365 = period365;
            log.LogMethodExit();
        }

        public ForecastingPeriodRatioDTO(int periodDataPointsId, int periodDataPoints , decimal? period30 ,decimal? period60 ,decimal? period90 ,decimal? period120, 
                                        decimal? period150, decimal? period180, decimal? period210, decimal? period240 ,decimal? period270, decimal? period300,
                                        decimal? period330, decimal? period365,
                                        bool isActive, string createdBy ,DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string guid,
                                        int siteId, bool synchStatus, int masterEntityId)
            :this(periodDataPointsId ,periodDataPoints, period30, period60, period90, period120, period150, period180, period210, period240, period270, period300, period330, period365)
        {
            log.LogMethodEntry();
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        public ForecastingPeriodRatioDTO(int dataPoints, double historicalDays , double ratio)
        {
            log.LogMethodEntry(dataPoints, ratio, historicalDays);
            this.dataPoints = dataPoints;
            this.ratio = ratio;
            this.historicalDays = historicalDays;
            log.LogMethodExit();
        }

        public AccountingCalendarMasterDTO AccountingCalendarMasterDTO { get { return accountingCalendarMasterDTO; } set { accountingCalendarMasterDTO = value; } }
    }
}

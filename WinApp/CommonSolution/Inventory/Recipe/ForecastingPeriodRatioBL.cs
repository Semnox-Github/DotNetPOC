/********************************************************************************************
* Project Name - Forecasting Period Ratio BL 
* Description  - Logic to maintain Forecasting data based on the  historical Days value.
* 
**************
**Version Log
**************
*Version     Date          Modified By         Remarks          
*********************************************************************************************
*2.100.0    26-Sep-20      Deeksha              Created 
********************************************************************************************/
using System;
using System.Linq;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class ForecastingTypePointListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext =  ExecutionContext.GetExecutionContext();
        public static List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList = null;
        private static List<ForecastingPeriodRatioDTO> foreCastingList = null;

        public ForecastingTypePointListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            if (accountingCalendarMasterDTOList == null)
            {
                GetAccountingCalendarDTOList();
            }
            if (foreCastingList == null)
            {
                GetAllForecastingPointsList();
            }
            log.LogMethodExit();
        }


        private void GetAllForecastingPointsList()
        {
            log.LogMethodEntry();
            foreCastingList = new List<ForecastingPeriodRatioDTO>();
            List<KeyValuePair<ForecastingPeriodRatioDTO.SearchByParameters, string>> searchparams = new List<KeyValuePair<ForecastingPeriodRatioDTO.SearchByParameters, string>>();
            searchparams.Add(new KeyValuePair<ForecastingPeriodRatioDTO.SearchByParameters, string>(ForecastingPeriodRatioDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<ForecastingPeriodRatioDTO> forecastingTypeList = GetForecastingPeriodRatioDTOList(searchparams);
            List<decimal?> period30Ratio = forecastingTypeList.Select(x => x.Period30).ToList();
            BuildForecastingList(forecastingTypeList,30, period30Ratio);
            List<decimal?> period60Ratio = forecastingTypeList.Select(x => x.Period60).ToList();
            BuildForecastingList(forecastingTypeList,60, period60Ratio);
            List<decimal?> period90Ratio = forecastingTypeList.Select(x => x.Period90).ToList();
            BuildForecastingList(forecastingTypeList,90, period90Ratio);
            List<decimal?> period120Ratio = forecastingTypeList.Select(x => x.Period120).ToList();
            BuildForecastingList(forecastingTypeList,120, period120Ratio);
            List<decimal?> period150Ratio = forecastingTypeList.Select(x => x.Period150).ToList();
            BuildForecastingList(forecastingTypeList,150, period150Ratio);
            List<decimal?> period180Ratio = forecastingTypeList.Select(x => x.Period180).ToList();
            BuildForecastingList(forecastingTypeList,180, period180Ratio);
            List<decimal?> period210Ratio = forecastingTypeList.Select(x => x.Period210).ToList();
            BuildForecastingList(forecastingTypeList,210, period210Ratio);
            List<decimal?> period240Ratio = forecastingTypeList.Select(x => x.Period240).ToList();
            BuildForecastingList(forecastingTypeList,240, period240Ratio);
            List<decimal?> period270Ratio = forecastingTypeList.Select(x => x.Period270).ToList();
            BuildForecastingList(forecastingTypeList,270, period270Ratio);
            List<decimal?> period300Ratio = forecastingTypeList.Select(x => x.Period300).ToList();
            BuildForecastingList(forecastingTypeList,300, period300Ratio);
            List<decimal?> period330Ratio = forecastingTypeList.Select(x => x.Period330).ToList();
            BuildForecastingList(forecastingTypeList,330, period330Ratio);
            List<decimal?> period365Ratio = forecastingTypeList.Select(x => x.Period365).ToList();
            BuildForecastingList(forecastingTypeList,365, period365Ratio);
            log.LogMethodExit();
        }

        private void BuildForecastingList(List<ForecastingPeriodRatioDTO> forecastingTypeList,int historicalDays , List<decimal?> period30Ratio)
        {
            log.LogMethodEntry(forecastingTypeList , period30Ratio);
            try
            {
                List<int> dataPoints = forecastingTypeList.Select(x => x.DataPoints).ToList();
                for (int i = 0; i < forecastingTypeList.Count; i++)
                {
                    if (period30Ratio[i] != null)
                    {
                        ForecastingPeriodRatioDTO fdto = new ForecastingPeriodRatioDTO(dataPoints[i], historicalDays, Convert.ToDouble(period30Ratio[i]));
                        foreCastingList.Add(fdto);
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        public List<ForecastingPeriodRatioDTO> GetPeriodDataPointsRatioAndTrx(int historicalDays , DateTime dateTime)
        {
            log.LogMethodEntry(historicalDays);
            List<ForecastingPeriodRatioDTO> forecastingTypeList = new List<ForecastingPeriodRatioDTO>();
            try
            {
                if (historicalDays <= 30)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(30, dateTime);
                }
                else if (historicalDays <= 60)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(60, dateTime);
                }
                else if (historicalDays <= 90)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(90, dateTime);
                }
                else if (historicalDays <= 120)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(120, dateTime);

                }
                else if (historicalDays <= 150)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(150, dateTime);
                }
                else if (historicalDays <= 180)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(180, dateTime);
                }
                else if (historicalDays <= 210)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(210, dateTime);
                }
                else if (historicalDays <= 240)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(240, dateTime);
                }
                else if (historicalDays <= 270)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(270, dateTime);
                }
                else if (historicalDays <= 300)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(300, dateTime);
                }
                else if (historicalDays <= 330)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(330, dateTime);
                }
                else if (historicalDays <= 365)
                {
                    forecastingTypeList = GetForecastingPointsListBasedonHistoricalDays(365, dateTime);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(forecastingTypeList);
            return forecastingTypeList;
        }

        private void GetAccountingCalendarDTOList()
        {
            log.LogMethodEntry();
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime currentDate = serverTimeObject.GetServerDateTime();
            AccountingCalendarMasterListBL accountingCalendarMasterListBL = new AccountingCalendarMasterListBL(executionContext);
            List<KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>>();
            searchParameter.Add(new KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>(AccountingCalendarMasterDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            accountingCalendarMasterDTOList = accountingCalendarMasterListBL.GetAccountingCalendarMasterDTOList(searchParameter);
            log.LogMethodExit();
        }

        private List<ForecastingPeriodRatioDTO> GetForecastingPeriodRatioDTOList(List<KeyValuePair<ForecastingPeriodRatioDTO.SearchByParameters, string>> searchParameters , SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ForecastingPeriodRatioDataHandler forecastingPeriodRatioDataHandler = new ForecastingPeriodRatioDataHandler(sqltransaction);
              List<ForecastingPeriodRatioDTO> forecastingPeriodRatioDTOList = forecastingPeriodRatioDataHandler.GetForecastingPeriodRatioDTOList(searchParameters);
            log.LogMethodExit(forecastingPeriodRatioDTOList);
            return forecastingPeriodRatioDTOList;
        }

        private List<ForecastingPeriodRatioDTO> GetForecastingPointsListBasedonHistoricalDays(int historicalDays , DateTime date)
        {
            log.LogMethodEntry(historicalDays, date);
            List<AccountingCalendarMasterDTO> accountingCalendarDTOList = accountingCalendarMasterDTOList.FindAll(
                                                   x => x.AccountingCalenderDate > date.AddDays(-365)
                                                   & x.AccountingCalenderDate <= date).ToList();
            List<ForecastingPeriodRatioDTO> forecastingTypeList = foreCastingList.Where(x => x.HistoricalDays == historicalDays).ToList();
            foreach (ForecastingPeriodRatioDTO forecastingDTO in forecastingTypeList)
            {
                AccountingCalendarMasterDTO dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date.AddDays(-forecastingDTO.DataPoints));

                if (forecastingDTO.DataPoints == 15)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.AccountingCalenderDate >= dto.AccountingCalenderDate.AddDays(-18) 
                                                          && x.AccountingCalenderDate <= dto.AccountingCalenderDate.AddDays(-12)
                                                          && x.DayWeek == dto.DayWeek)
                                                         .OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 30)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek & x.Month == ((dto.Month - 1) <= 0 ? (12 - (1 - dto.Month)) : (dto.Month - 1))).OrderByDescending(x=>x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                if (forecastingDTO.DataPoints == 45)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.AccountingCalenderDate >= dto.AccountingCalenderDate.AddDays(-48)
                                                          && x.AccountingCalenderDate <= dto.AccountingCalenderDate.AddDays(-42)
                                                          && x.DayWeek == dto.DayWeek)
                                                         .OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 60)   
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek & x.Month == ((dto.Month - 2) <= 0 ? (12 - (2 - dto.Month)) : (dto.Month - 2))).OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
               
                else if (forecastingDTO.DataPoints == 90)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek & x.Month == ((dto.Month - 3) <= 0 ? (12 - (3 - dto.Month)) : (dto.Month - 3))).OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 120)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek  & x.Month == ((dto.Month - 4) <= 0 ? (12 - (4 - dto.Month)) : (dto.Month - 4))).OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 150)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek  & x.Month == ((dto.Month - 5) <= 0 ? (12 - (5 - dto.Month)) : (dto.Month - 5))).OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 180)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek & x.Month == ((dto.Month - 6) <= 0 ? (12 - (6 - dto.Month)) : (dto.Month - 6))).OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 210)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek & x.Month == ((dto.Month - 7) <= 0 ? (12 - (7 - dto.Month)) : (dto.Month - 7))).OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 240)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek  & x.Month == ((dto.Month - 8) <= 0 ? (12 - (8 - dto.Month)) : (dto.Month - 8))).OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 270)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek & x.Month == ((dto.Month - 9) <= 0 ? (12 - (9 - dto.Month)) : (dto.Month - 9))).OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 300)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek & x.Month == ((dto.Month - 10) <= 0 ? (12 - (10 - dto.Month)) : (dto.Month - 10))).OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 330)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek & x.Month == ((dto.Month - 11) <= 0 ? (12 - (11 - dto.Month)) : (dto.Month - 11))).OrderByDescending(x => x.AccountingCalenderDate).ToList().FirstOrDefault();
                }
                else if (forecastingDTO.DataPoints == 365)
                {
                    dto = accountingCalendarDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                    dto = accountingCalendarDTOList.Where(x => x.DayWeek == dto.DayWeek  & x.Month == ((dto.Month - 12) <= 0 ? (12 - (12 - dto.Month)) : (dto.Month - 12))).OrderByDescending(x => x.AccountingCalenderDate).ToList().LastOrDefault();
                }
                if (dto == null)
                {
                    dto = new AccountingCalendarMasterDTO();
                }
                forecastingDTO.AccountingCalendarMasterDTO = dto;
            }
            log.LogMethodExit(forecastingTypeList);
            return forecastingTypeList;
        }
    }
}

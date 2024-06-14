/********************************************************************************************
 * Project Name - Concurrent Library
 * Description  - Library Class to generate Forecast 
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       20-Jul-2020   Deeksha             Created : Generates forecast for the past 365 days till today.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Threading;
using System.Messaging;
using System.Globalization;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Inventory.Recipe;

namespace Semnox.Parafait.ConcurrentManager
{
    public partial class ConcLib
    {
        public string InventoryForecastingEngine(int RequestId, string LogFileName)
        {
            log.LogMethodEntry(RequestId, LogFileName);
            _requestId = RequestId;
            _logFileName = LogFileName;
            MessageQueue messageQueue = MessageQueueUtils.GetMessageQueue(RequestId);

            System.Threading.ThreadStart thr = delegate
            {
                while (true)
                {
                    System.Messaging.Message msg = messageQueue.Receive();

                    msg.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                    QueueMessage = msg.Body.ToString();
                    if (QueueMessage.Equals("SHUTDOWN"))
                        break;
                }
            };

            System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(thr));
            th.Start();

            int loopCount = 6;
            while (QueueMessage.Equals("SHUTDOWN") == false)
            {
                System.Windows.Forms.Application.DoEvents();

                if (loopCount++ < 6)
                {
                    Thread.Sleep(5 * 1000);
                    continue;
                }
                loopCount = 0;
                RecipeEstimationHeaderBL recipeEstimationHeaderBL = null;
                LookupValuesList serverTimeObject = new LookupValuesList(_utilities.ExecutionContext);
                string currentDatetime = serverTimeObject.GetServerDateTime().ToString("yyyy-MM-dd");
                log.LogVariableState("currentDatetime", currentDatetime);
                TimeSpan ts = new TimeSpan(6, 00, 0);
                currentDatetime = currentDatetime + " " + ts + " " + "AM";
                DateTime currentDate = Convert.ToDateTime(currentDatetime);
                if (_utilities.getParafaitDefaults("ENABLE_FORECASTING_BATCH_JOB").Equals("Y"))
                {
                    RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(_utilities.ExecutionContext);
                    List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchEstimationParameter = new List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>>();
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_FROM_DATE, currentDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_TO_DATE, currentDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID, _utilities.ExecutionContext.GetSiteId().ToString()));
                    List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = recipeEstimationHeaderListBL.GetAllRecipeEstimationHeaderDTOList(searchEstimationParameter, true, true);
                    if (recipeEstimationHeaderDTOList == null || recipeEstimationHeaderDTOList.Any() == false)
                    {
                        using (ParafaitDBTransaction dbtrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                dbtrx.BeginTransaction();
                                RecipeEstimationHeaderDTO recipeEstimationHeaderDTO = new RecipeEstimationHeaderDTO(-1, currentDate, currentDate,
                                                                                                    0, 0, false, 365,
                                                                                                   -1, null, null, true);
                                recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(_utilities.ExecutionContext, recipeEstimationHeaderDTO);
                                recipeEstimationHeaderBL.BuildForecastData(dbtrx.SQLTrx);
                                dbtrx.EndTransaction();
                            }
                            catch (Exception ex)
                            {
                                dbtrx.RollBack();
                                log.Error(ex);
                                writeToLog(ex.Message);
                                log.LogMethodExit();
                                return ex.Message;
                            }
                        }
                    }
                    else
                    {
                        string message = "Data Exists";
                        writeToLog("message");
                        log.LogMethodExit(message);
                    }
                }
                else
                {
                    writeToLog("ENABLE_FORECASTING_BATCH_JOB - Configuration is disabled");
                }

                //Purge Old Data
                using (ParafaitDBTransaction dbtrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        int purgeDataInDays = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault<decimal>(_utilities.ExecutionContext, "PURGE_FORECASTING_DATA_BEFORE_DAYS"));

                        RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(_utilities.ExecutionContext);
                        List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchEstimationParameter = new List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>>();
                        searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_TO_DATE, currentDate.AddDays(-purgeDataInDays).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID, _utilities.ExecutionContext.GetSiteId().ToString()));
                        List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = recipeEstimationHeaderListBL.GetAllRecipeEstimationHeaderDTOList(searchEstimationParameter, true, true);
                        if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Count > 0)
                        {
                            dbtrx.BeginTransaction();
                            recipeEstimationHeaderBL.PurgeOldData(purgeDataInDays, dbtrx.SQLTrx);
                            dbtrx.EndTransaction();
                            writeToLog("Data Purge Completed");
                        }
                        else
                        {
                            writeToLog("Data before Purge Days Does not exists");
                        }
                    }
                    catch(Exception ex)
                    {
                        dbtrx.RollBack();
                        log.Error(ex);
                        writeToLog(ex.Message);
                    }
                }
            }
            writeToLog("Forecast Completed");
            log.LogMethodExit("Success");
            return "Success";
        }
    }
}

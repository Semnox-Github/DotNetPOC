/********************************************************************************************
 * Project Name - Utilities  Class
 * Description  - SystemOptionViewContainerList holds multiple  SystemOptionView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// SystemOptionViewContainerList holds multiple  SystemOptionView containers based on siteId, userId and POSMachineId
    /// </summary>
    public class SystemOptionViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, SystemOptionViewContainer> systemOptionViewContainerCache = new Cache<int, SystemOptionViewContainer>();
        private static Timer refreshTimer;

        static SystemOptionViewContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = systemOptionViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                SystemOptionViewContainer systemOptionViewContainer;
                if (systemOptionViewContainerCache.TryGetValue(uniqueKey, out systemOptionViewContainer))
                {
                    systemOptionViewContainerCache[uniqueKey] = systemOptionViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static SystemOptionViewContainer GetSystemOptionViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            SystemOptionViewContainer result = systemOptionViewContainerCache.GetOrAdd(siteId, (k) => new SystemOptionViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the SystemOptionContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static SystemOptionContainerDTOCollection GetSystemOptionContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            SystemOptionViewContainer systemOptionViewContainer = GetSystemOptionViewContainer(siteId);
            SystemOptionContainerDTOCollection result = systemOptionViewContainer.GetSystemOptionContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            SystemOptionViewContainer systemOptionViewContainer = GetSystemOptionViewContainer(siteId);
            systemOptionViewContainerCache[siteId] = systemOptionViewContainer.Refresh(true);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the option value based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="optionType">option value type</param>
        /// <param name="optionName">option value name</param>
        /// <returns></returns>
        public static string GetSystemOption(ExecutionContext executionContext, string optionType, string optionName)
        {
            log.LogMethodEntry(executionContext, optionType, optionName);
            SystemOptionViewContainer systemOptionContainer = GetSystemOptionViewContainer(executionContext.GetSiteId());
            string optionValue = systemOptionContainer.GetSystemOption(optionType, optionName);
            log.LogMethodExit();
            return optionValue;
        }

        /// <summary>
        /// Returns the option value based on the execution context. 
        /// If the option value not defined then returns the optionvalue passed
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="optionType">option value type</param>
        /// <param name="optionName"></param>
        /// <param name="defaultOptionValue"></param>
        /// <returns></returns>
        public static string GetSystemOption(ExecutionContext executionContext, string optionType, string optionName, string defaultOptionValue)
        {
            log.LogMethodEntry(executionContext, optionType, optionName, defaultOptionValue);
            string result = GetSystemOption(executionContext, optionType, optionName);
            if (string.IsNullOrWhiteSpace(result))
            {
                result = defaultOptionValue;
            }
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Generic method returns the option value based on the execution context.
        /// Tries to convert the option value to the specified type.
        /// </summary>
        /// <typeparam name="T">value type (bool, char, double, decimal, float, int)</typeparam>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="optionType">option value type</param>
        /// <param name="optionName">option value name</param>
        /// <returns></returns>
        public static T GetSystemOption<T>(ExecutionContext executionContext, string optionType, string optionName) where T : struct
        {
            log.LogMethodEntry(executionContext, optionType, optionName);
            T optionValue;
            string optionValueString = GetSystemOption(executionContext, optionType, optionName);
            try
            {
                optionValue = (T)ConvertStringToType<T>(optionValueString);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting application optionvalue", ex);
                log.LogVariableState("optionValueString", optionValueString);
                log.Debug("Trying create the option value of the type: " + typeof(T).ToString());
                optionValue = default(T);
            }
            log.LogMethodExit();
            return optionValue;
        }

        /// <summary>
        /// Generic method returns the option value based on the execution context.
        /// Tries to convert the option value to the specified type.
        /// If no option value is provided user passed option value is returned
        /// </summary>
        /// <typeparam name="T">value type (bool, char, double, decimal, float, int)</typeparam>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="optionType">option value type</param>
        /// <param name="optionName">option value name</param>
        /// <returns></returns>
        public static T GetSystemOption<T>(ExecutionContext executionContext, string optionType, string optionName, T defaultOptionValue) where T : struct
        {
            log.LogMethodEntry(executionContext, optionType, optionName, defaultOptionValue);
            T result;
            string optionValueString = GetSystemOption(executionContext, optionType, optionName);
            if (string.IsNullOrWhiteSpace(optionValueString))
            {
                result = defaultOptionValue;
                log.LogMethodExit(result, "Default value is empty. returning the option value passed");
                return result;
            }
            try
            {
                result = (T)ConvertStringToType<T>(optionValueString);
            }
            catch (Exception)
            {
                log.Debug("Error occurred while converting optionValueString to optionValue. returning the optionValue provided by the client.");
                log.LogVariableState("optionValueString", optionValueString);
                log.LogVariableState("optionValueName", optionName);
                log.LogVariableState("optionValue", defaultOptionValue);
                result = defaultOptionValue;
            }
            log.LogMethodExit();
            return result;
        }

        private static object ConvertStringToType<T>(string value) where T : struct
        {
            log.LogMethodEntry(value);
            object convertedValue = null;
            if (typeof(T) == typeof(Color))
            {
                convertedValue = ColorTranslator.FromHtml(value);
            }
            else if (typeof(T) == typeof(bool))
            {
                if (value == "Y" || value == "1")
                {
                    convertedValue = true;
                }
                else
                {
                    convertedValue = false;
                }
            }
            else
            {
                convertedValue = Convert.ChangeType(value, typeof(T));
            }
            log.LogMethodExit(convertedValue);
            return convertedValue;
        }
    }
}

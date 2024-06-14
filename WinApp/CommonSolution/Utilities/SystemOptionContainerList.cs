/********************************************************************************************
 * Project Name - Utilities
 * Description  - SystemOptionMasterList class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020   Lakshminarayana     Created: POS Redesign
 ********************************************************************************************/

using System;
using System.Drawing;
using System.Timers;

namespace Semnox.Core.Utilities
{
    public static class SystemOptionContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, SystemOptionContainer> systemOptionContainerCache = new Cache<int, SystemOptionContainer>();
        private static Timer refreshTimer;

        static SystemOptionContainerList()
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
            var uniqueKeyList = systemOptionContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                SystemOptionContainer systemOptionContainer;
                if (systemOptionContainerCache.TryGetValue(uniqueKey, out systemOptionContainer))
                {
                    systemOptionContainerCache[uniqueKey] = systemOptionContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static SystemOptionContainer GetSystemOptionContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            SystemOptionContainer result = systemOptionContainerCache.GetOrAdd(siteId, (k) => new SystemOptionContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static SystemOptionContainerDTOCollection GetSystemOptionContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            SystemOptionContainer container = GetSystemOptionContainer(siteId);
            SystemOptionContainerDTOCollection result = container.GetSystemOptionContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            SystemOptionContainer systemOptionContainer = GetSystemOptionContainer(siteId);
            systemOptionContainerCache[siteId] = systemOptionContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the option value based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="optionType">option value type</param>
        /// <param name="optionName">option value name</param>
        /// <returns></returns>
        public static string GetSystemOption(int siteId, string optionType, string optionName)
        {
            log.LogMethodEntry(siteId, optionType, optionName);
            SystemOptionContainer systemOptionContainer = GetSystemOptionContainer(siteId);
            string optionValue = systemOptionContainer.GetSystemOption(optionType, optionName);
            log.LogMethodExit();
            return optionValue;
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
            SystemOptionContainer systemOptionContainer = GetSystemOptionContainer(executionContext.GetSiteId());
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
        /// Returns the option value based on the site Id. 
        /// If the option value not defined then returns the optionvalue passed
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="optionType">option value type</param>
        /// <param name="optionName"></param>
        /// <param name="defaultOptionValue"></param>
        /// <returns></returns>
        public static string GetSystemOption(int siteId, string optionType, string optionName, string defaultOptionValue)
        {
            log.LogMethodEntry(siteId, optionType, optionName, defaultOptionValue);
            string result = GetSystemOption(siteId, optionType, optionName);
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
        /// </summary>
        /// <typeparam name="T">value type (bool, char, double, decimal, float, int)</typeparam>
        /// <param name="siteId">current application execution context</param>
        /// <param name="optionType">option value type</param>
        /// <param name="optionName">option value name</param>
        /// <returns></returns>
        public static T GetSystemOption<T>(int siteId, string optionType, string optionName) where T : struct
        {
            log.LogMethodEntry(siteId, optionType, optionName);
            T optionValue;
            string optionValueString = GetSystemOption(siteId, optionType, optionName);
            try
            {
                optionValue = (T)ConvertStringToType<T>(optionValueString);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting application option value", ex);
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

        /// <summary>
        /// Generic method returns the option value based on the execution context.
        /// Tries to convert the option value to the specified type.
        /// If no option value is provided user passed option value is returned
        /// </summary>
        /// <typeparam name="T">value type (bool, char, double, decimal, float, int)</typeparam>
        /// <param name="siteId">current application execution context</param>
        /// <param name="optionType">option value type</param>
        /// <param name="optionName">option value name</param>
        /// <param name="defaultOptionValue">option value if not found</param>
        /// <returns></returns>
        public static T GetSystemOption<T>(int siteId, string optionType, string optionName, T defaultOptionValue) where T : struct
        {
            log.LogMethodEntry(siteId, optionType, optionName, defaultOptionValue);
            T result;
            string optionValueString = GetSystemOption(siteId, optionType, optionName);
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
                convertedValue = ColorTranslator.FromHtml(value); ;
            }
            else if (typeof(T) == typeof(bool))
            {
                if (value == "Y")
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

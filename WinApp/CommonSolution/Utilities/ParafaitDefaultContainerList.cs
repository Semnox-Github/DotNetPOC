/********************************************************************************************
 * Project Name - Utilities
 * Description  - ParafaitDefaultMasterList class to get the List of lookup from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Core.Utilities
{
    public static class ParafaitDefaultContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, Sample.ParafaitDefaultContainer> defaultContainerCache = new Cache<int, Sample.ParafaitDefaultContainer>();
        private static Timer refreshTimer;

        static ParafaitDefaultContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e) 
        {
            log.LogMethodEntry();
            var uniqueKeyList = defaultContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                Sample.ParafaitDefaultContainer parafaitDefaultContainer;
                if (defaultContainerCache.TryGetValue(uniqueKey, out parafaitDefaultContainer))
                {
                    defaultContainerCache[uniqueKey] = parafaitDefaultContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static Sample.ParafaitDefaultContainer GetParafaitDefaultContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            Sample.ParafaitDefaultContainer result = defaultContainerCache.GetOrAdd(siteId,(k) => new Sample.ParafaitDefaultContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the ParafaitDefaultContainerDTOCollection data structure used to build the view container
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userPkId"></param>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public static ParafaitDefaultContainerDTOCollection GetParafaitDefaultContainerDTOCollection(int siteId, int userPkId, int machineId)
        {
            Sample.ParafaitDefaultContainer container = GetParafaitDefaultContainer(siteId);
            return container.GetParafaitDefaultContainerDTOCollection(userPkId, machineId);
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            Sample.ParafaitDefaultContainer parafaitDefaultContainer = GetParafaitDefaultContainer(siteId);
            defaultContainerCache[siteId] = parafaitDefaultContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the default value based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="defaultValueName">default value name</param>
        /// <returns></returns>
        public static string GetParafaitDefault(int siteId, string defaultValueName)
        {
            log.LogMethodEntry(siteId, defaultValueName);
            Sample.ParafaitDefaultContainer parafaitDefaultContainer = GetParafaitDefaultContainer(siteId);
            string defaultValue = parafaitDefaultContainer.GetParafaitDefault(defaultValueName, -1, -1);  
            log.LogMethodExit();
            return defaultValue;
        }

        public static ParafaitDefaultsDTO GetParafaitDefaultDTO(int siteId, string defaultValueName)
        {
            log.LogMethodEntry(siteId, defaultValueName);
            Sample.ParafaitDefaultContainer parafaitDefaultContainer = GetParafaitDefaultContainer(siteId);
            ParafaitDefaultsDTO defaultValue = parafaitDefaultContainer.GetParafaitDefault(defaultValueName);
            log.LogMethodExit(defaultValue);
            return defaultValue;
        }
        /// <summary>
        /// Returns the default value based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="defaultValueName">default value name</param>
        /// <returns></returns>
        public static string GetParafaitDefault(ExecutionContext executionContext, string defaultValueName)
        {
            log.LogMethodEntry(executionContext, defaultValueName);
            Sample.ParafaitDefaultContainer parafaitDefaultContainer = GetParafaitDefaultContainer(executionContext.GetSiteId());
            string defaultValue = parafaitDefaultContainer.GetParafaitDefault(defaultValueName, executionContext.GetUserPKId(), executionContext.GetMachineId());  
            log.LogMethodExit();
            return defaultValue;
        }

        /// <summary>
        /// Returns the default value based on the execution context. 
        /// If the default value not defined then returns the defaultvalue passed
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="defaultValueName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetParafaitDefault(ExecutionContext executionContext, string defaultValueName, string defaultValue)
        {
            log.LogMethodEntry(executionContext, defaultValueName, defaultValue);
            string result = GetParafaitDefault(executionContext, defaultValueName);
            if (string.IsNullOrWhiteSpace(result))
            {
                result = defaultValue;
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Returns the default value based on the execution context. 
        /// If the default value not defined then returns the defaultvalue passed
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="defaultValueName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetParafaitDefault(int siteId, string defaultValueName, string defaultValue)
        {
            log.LogMethodEntry(siteId, defaultValueName, defaultValue);
            string result = GetParafaitDefault(siteId, defaultValueName);
            if (string.IsNullOrWhiteSpace(result))
            {
                result = defaultValue;
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Generic method returns the default value based on the execution context.
        /// Tries to convert the default value to the specified type.
        /// </summary>
        /// <typeparam name="T">value type (bool, char, double, decimal, float, int)</typeparam>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="defaultValueName">default value name</param>
        /// <returns></returns>
        public static T GetParafaitDefault<T>(ExecutionContext executionContext, string defaultValueName) where T : struct
        {
            log.LogMethodEntry(executionContext, defaultValueName);
            T defaultValue;
            string defaultValueString = GetParafaitDefault(executionContext, defaultValueName);
            try
            {
                defaultValue = (T)ConvertStringToType<T>(defaultValueString);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting application defaultvalue", ex);
                log.LogVariableState("defaultValueString", defaultValueString);
                log.Debug("Trying create the default value of the type: " + typeof(T).ToString());
                defaultValue = default(T);
            }
            log.LogMethodExit();
            return defaultValue;
        }

        /// <summary>
        /// Generic method returns the default value based on the execution context.
        /// Tries to convert the default value to the specified type.
        /// </summary>
        /// <typeparam name="T">value type (bool, char, double, decimal, float, int)</typeparam>
        /// <param name="siteId">current application execution context</param>
        /// <param name="defaultValueName">default value name</param>
        /// <returns></returns>
        public static T GetParafaitDefault<T>(int siteId, string defaultValueName) where T : struct
        {
            log.LogMethodEntry(siteId, defaultValueName);
            T defaultValue;
            string defaultValueString = GetParafaitDefault(siteId, defaultValueName);
            try
            {
                defaultValue = (T)ConvertStringToType<T>(defaultValueString);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while converting application default value", ex);
                log.LogVariableState("defaultValueString", defaultValueString);
                log.Debug("Trying create the default value of the type: " + typeof(T).ToString());
                defaultValue = default(T);
            }
            log.LogMethodExit();
            return defaultValue;
        }

        /// <summary>
        /// Generic method returns the default value based on the execution context.
        /// Tries to convert the default value to the specified type.
        /// If no default value is provided user passed default value is returned
        /// </summary>
        /// <typeparam name="T">value type (bool, char, double, decimal, float, int)</typeparam>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="defaultValueName">default value name</param>
        /// <returns></returns>
        public static T GetParafaitDefault<T>(ExecutionContext executionContext, string defaultValueName, T defaultValue) where T : struct
        {
            log.LogMethodEntry(executionContext, defaultValueName, defaultValue);
            T result;
            string defaultValueString = GetParafaitDefault(executionContext, defaultValueName);
            if (string.IsNullOrWhiteSpace(defaultValueString))
            {
                result = defaultValue;
                log.LogMethodExit(result, "Default value is empty. returning the default value passed");
                return result;
            }
            try
            {
                result = (T)ConvertStringToType<T>(defaultValueString);
            }
            catch (Exception)
            {
                log.Debug("Error occurred while converting defaultValueString to defaultValue. returning the defaultValue provided by the client.");
                log.LogVariableState("defaultValueString", defaultValueString);
                log.LogVariableState("defaultValueName", defaultValueName);
                log.LogVariableState("defaultValue", defaultValue);
                result = defaultValue;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Generic method returns the default value based on the execution context.
        /// Tries to convert the default value to the specified type.
        /// If no default value is provided user passed default value is returned
        /// </summary>
        /// <typeparam name="T">value type (bool, char, double, decimal, float, int)</typeparam>
        /// <param name="siteId">current application execution context</param>
        /// <param name="defaultValueName">default value name</param>
        /// <param name="defaultValue">default value if not found</param>
        /// <returns></returns>
        public static T GetParafaitDefault<T>(int siteId, string defaultValueName, T defaultValue) where T : struct
        {
            log.LogMethodEntry(siteId, defaultValueName, defaultValue);
            T result;
            string defaultValueString = GetParafaitDefault(siteId, defaultValueName);
            if (string.IsNullOrWhiteSpace(defaultValueString))
            {
                result = defaultValue;
                log.LogMethodExit(result, "Default value is empty. returning the default value passed");
                return result;
            }
            try
            {
                result = (T)ConvertStringToType<T>(defaultValueString);
            }
            catch (Exception)
            {
                log.Debug("Error occurred while converting defaultValueString to defaultValue. returning the defaultValue provided by the client.");
                log.LogVariableState("defaultValueString", defaultValueString);
                log.LogVariableState("defaultValueName", defaultValueName);
                log.LogVariableState("defaultValue", defaultValue);
                result = defaultValue;
            }
            log.LogMethodExit(result);
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

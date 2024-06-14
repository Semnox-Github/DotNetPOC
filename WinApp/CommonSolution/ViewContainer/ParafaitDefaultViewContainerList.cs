/********************************************************************************************
 * Project Name - Utilities  Class
 * Description  - ParafaitDefaultViewContainerList holds multiple  ParafaitDefaultView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// ParafaitDefaultViewContainerList holds multiple  ParafaitDefaultView containers based on siteId, userId and POSMachineId
    /// </summary>
    public class ParafaitDefaultViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<string, ParafaitDefaultViewContainer> parafaitDefaultViewContainerCache = new Cache<string, ParafaitDefaultViewContainer>();
        private static Timer refreshTimer;

        static ParafaitDefaultViewContainerList()
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
            var uniqueKeyList = parafaitDefaultViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ParafaitDefaultViewContainer parafaitDefaultViewContainer;
                if (parafaitDefaultViewContainerCache.TryGetValue(uniqueKey, out parafaitDefaultViewContainer))
                {
                    parafaitDefaultViewContainerCache[uniqueKey] = parafaitDefaultViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static ParafaitDefaultViewContainer GetParafaitDefaultViewContainer(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ParafaitDefaultViewContainer result = GetParafaitDefaultViewContainer(executionContext.GetSiteId(), executionContext.GetUserPKId(), executionContext.GetMachineId());
            log.LogMethodExit(result);
            return result;
        }

        private static ParafaitDefaultViewContainer GetParafaitDefaultViewContainer(int siteId, int userPkId, int machineId)
        {
            log.LogMethodEntry(siteId, userPkId, machineId);
            string uniqueKey = GetUniqueKey(siteId, userPkId, machineId);
            ParafaitDefaultViewContainer result = parafaitDefaultViewContainerCache.GetOrAdd(uniqueKey, (k) => new ParafaitDefaultViewContainer(siteId, userPkId, machineId));
            log.LogMethodExit(result);
            return result;
        }

        private static string GetUniqueKey(int siteId, int userPkId, int machineId)
        {
            log.LogMethodEntry(siteId, userPkId, machineId);
            string uniqueKey = "SiteId:" + siteId + "UserId:" + userPkId + "MachineId:" + machineId;
            log.LogMethodExit(uniqueKey);
            return uniqueKey;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            Rebuild(executionContext.GetSiteId(), executionContext.GetUserPKId(), executionContext.GetMachineId());
            log.LogMethodExit();
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId, int userPkId, int machineId)
        {
            log.LogMethodEntry(siteId, userPkId, machineId);
            string uniqueKey = GetUniqueKey(siteId, userPkId, machineId);
            ParafaitDefaultViewContainer parafaitDefaultViewContainer = GetParafaitDefaultViewContainer(siteId, userPkId, machineId);
            parafaitDefaultViewContainerCache[uniqueKey] = parafaitDefaultViewContainer.Refresh(true);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the default value based on the execution context
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="userPkId">userPkId</param>
        /// <param name="machineId">machineId</param>
        /// <param name="defaultValueName">default value name</param>
        /// <returns></returns>
        public static string GetParafaitDefault(int siteId, int userPkId, int machineId, string defaultValueName)
        {
            log.LogMethodEntry(siteId, userPkId, machineId, defaultValueName);
            ParafaitDefaultViewContainer parafaitDefaultViewContainer = GetParafaitDefaultViewContainer(siteId, userPkId, machineId);
            string defaultValue = parafaitDefaultViewContainer.GetParafaitDefault(defaultValueName);
            log.LogMethodExit();
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
            ParafaitDefaultViewContainer parafaitDefaultViewContainer = GetParafaitDefaultViewContainer(executionContext);
            string defaultValue = parafaitDefaultViewContainer.GetParafaitDefault(defaultValueName);
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
                log.Error("Error occurred while converting application default value", ex);
                log.LogVariableState("defaultValueString", defaultValueString);
                log.Debug("Trying create the default value of the type: " + typeof(T).ToString());
                defaultValue = (T)Activator.CreateInstance(typeof(T));
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
        /// <param name="defaultValue">default value</param>
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
                log.Debug("Error occured while converting defaultValueString to defaultValue. returning the defaultValue provided by the client.");
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
                convertedValue = ColorTranslator.FromHtml(value);
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

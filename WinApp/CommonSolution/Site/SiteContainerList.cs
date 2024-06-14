/********************************************************************************************
 * Project Name - Site
 * Description  - SiteMasterList holds the site container
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Timers;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// Holds the site container object
    /// </summary>
    public class SiteContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static SiteContainer siteContainer;
        private static Timer refreshTimer;

        static SiteContainerList()
        {
            log.LogMethodEntry();
            siteContainer = new SiteContainer();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            siteContainer = siteContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the list of SiteContainerDTO
        /// </summary>
        /// <returns></returns>
        public static SiteContainerDTOCollection GetSiteContainerDTOCollection()
        {
            log.LogMethodEntry();
            var result = siteContainer.GetSiteContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the list of SiteContainerDTO
        /// </summary>
        /// <returns></returns>
        public static List<SiteContainerDTO> GetSiteContainerDTOList()
        {
            log.LogMethodEntry();
            var result = siteContainer.GetSiteContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the customer key for a given site
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <returns></returns>
        public static string GetCustomerKey(int siteId)
        {
            log.LogMethodEntry();
            var result = siteContainer.GetCustomerKey(siteId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// return the current site container DTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static SiteContainerDTO GetCurrentSiteContainerDTO(ExecutionContext executionContext)
        {
            return GetCurrentSiteContainerDTO(executionContext.SiteId);
        }

        /// <summary>
        /// return the current site container DTO
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static SiteContainerDTO GetCurrentSiteContainerDTO(int siteId)
        {
            log.LogMethodEntry();
            SiteContainerDTO siteContainerDTO = siteContainer.GetSiteContainerDTO(siteId);
            log.LogMethodExit(siteContainerDTO);
            return siteContainerDTO;
        }

        /// <summary>
        /// Returns whether this is a HQ environment
        /// </summary>
        /// <returns></returns>
        public static bool IsCorporate()
        {
            log.LogMethodEntry();
            var result = siteContainer.IsCorporate();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild()
        {
            log.LogMethodEntry();
            siteContainer = siteContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the site id of the master site. 
        /// </summary>
        /// <returns></returns>
        public static int GetMasterSiteId()
        {
            log.LogMethodEntry();
            int result = siteContainer.GetMasterSiteId();
            log.LogMethodExit(result);
            return result;
        }
        
        public static DateTime? ToSiteDateTime(ExecutionContext executionContext, DateTime? hqDateTime)
        {
            log.LogMethodEntry(executionContext, hqDateTime);
            DateTime? result = ToSiteDateTime(executionContext.SiteId, hqDateTime);
            log.LogMethodExit(result);
            return result;
        }


        public static DateTime? ToSiteDateTime(int siteId, DateTime? hqDateTime)
        {
            log.LogMethodEntry(siteId, hqDateTime);
            if (hqDateTime.HasValue == false)
            {
                log.LogMethodExit(hqDateTime, "hqDateTime.HasValue == false");
                return hqDateTime;
            }
            DateTime result = ToSiteDateTime(siteId, hqDateTime.Value);
            log.LogMethodExit(result);
            return result;
        }

        public static DateTime ToSiteDateTime(ExecutionContext executionContext, DateTime hqDateTime)
        {
            log.LogMethodEntry(executionContext, hqDateTime);
            DateTime result = ToSiteDateTime(executionContext.SiteId, hqDateTime);
            log.LogMethodExit(result);
            return result;
        }

        public static DateTime ToSiteDateTime(int siteId, DateTime hqDateTime)
        {
            log.LogMethodEntry(siteId, hqDateTime);
            if(IsCorporate() == false)
            {
                log.LogMethodExit(hqDateTime, "IsCorporate == false");
                return hqDateTime;
            }
            if (hqDateTime == DateTime.MinValue || hqDateTime == DateTime.MaxValue)
            {
                log.LogMethodExit(hqDateTime, "hqDateTime == DateTime.MinValue or DateTime.MaxValue");
                return hqDateTime;
            }
            DateTime result = siteContainer.ToSiteDateTime(siteId, hqDateTime);
            log.LogMethodExit(result);
            return result;
        }

        public static DateTime? FromSiteDateTime(ExecutionContext executionContext, DateTime? siteDateTime)
        {
            log.LogMethodEntry(executionContext, siteDateTime);
            DateTime? result = FromSiteDateTime(executionContext.SiteId, siteDateTime);
            log.LogMethodExit(result);
            return result;
        }

        public static DateTime? FromSiteDateTime(int siteId, DateTime? siteDateTime)
        {
            log.LogMethodEntry(siteId, siteDateTime);
            if (siteDateTime.HasValue == false)
            {
                log.LogMethodExit(siteDateTime, "siteDateTime.HasValue == false");
                return siteDateTime;
            }
            DateTime result = FromSiteDateTime(siteId, siteDateTime.Value);
            log.LogMethodExit(result);
            return result;
        }

        public static DateTime FromSiteDateTime(ExecutionContext executionContext, DateTime siteDateTime)
        {
            log.LogMethodEntry(executionContext, siteDateTime);
            DateTime result = FromSiteDateTime(executionContext.SiteId, siteDateTime);
            log.LogMethodExit(result);
            return result;
        }

        public static DateTime FromSiteDateTime(int siteId, DateTime siteDateTime)
        {
            log.LogMethodEntry(siteId, siteDateTime);
            if (IsCorporate() == false)
            {
                log.LogMethodExit(siteDateTime, "IsCorporate == false");
                return siteDateTime;
            }
            if (siteDateTime == DateTime.MinValue || siteDateTime == DateTime.MaxValue)
            {
                log.LogMethodExit(siteDateTime, "siteDateTime == DateTime.MinValue or DateTime.MaxValue");
                return siteDateTime;
            }
            DateTime result = siteContainer.FromSiteDateTime(siteId, siteDateTime);
            log.LogMethodExit(result);
            return result;
        }

        public static DateTime CurrentDateTime(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            DateTime result = CurrentDateTime(executionContext.SiteId);
            log.LogMethodExit(result);
            return result;
        }

        public static DateTime CurrentDateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime result = siteContainer.CurrentDateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }

        public static List<T> FromSiteDateTime<T>(ExecutionContext executionContext, List<T> dtoList, string ignoreColumns = "", string siteIdColumn = "SiteId")
        {
            log.LogMethodEntry(executionContext, dtoList, ignoreColumns, siteIdColumn);
            if (executionContext == null)
            {
                List<T> result = FromSiteDateTime(dtoList, ignoreColumns, siteIdColumn);
                log.LogMethodExit(result, "executionContext == null");
                return result;
            }
            if (IsCorporate() == false)
            {
                log.LogMethodExit(dtoList, "IsCorporate == false");
                return dtoList;
            }
            if (dtoList == null || dtoList.Any() == false)
            {
                log.LogMethodExit("dtoList is empty");
                return dtoList;
            }
            MethodInfo acceptChangesMethod = typeof(T).GetMethod("AcceptChanges", BindingFlags.Public | BindingFlags.Instance);
            foreach (var dto in dtoList)
            {
                FromSiteDateTime(executionContext, dto, ignoreColumns);
                if (acceptChangesMethod != null)
                {
                    acceptChangesMethod.Invoke(dto, new object[] { });
                }
            }
            return dtoList;
        }

        public static T FromSiteDateTime<T>(ExecutionContext executionContext, T dto, string ignoreColumns = "", string siteIdColumn = "SiteId")
        {
            log.LogMethodEntry(executionContext, dto, ignoreColumns, siteIdColumn);
            if(executionContext == null)
            {
                T result = FromSiteDateTime(dto, ignoreColumns, siteIdColumn);
                log.LogMethodExit(result, "executionContext == null");
                return result;
            }
            if (dto == null)
            {
                log.LogMethodExit("dto == null");
                return dto;
            }
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                if (ignoreColumns != null && ignoreColumns.Contains(p.Name))
                {
                    continue;
                }
                // Only modify the date time properties
                if (p.PropertyType != typeof(DateTime) && p.PropertyType != typeof(DateTime?)) { continue; }

                // If not writable then cannot null it; if not readable then cannot check it's value
                if (!p.CanWrite || !p.CanRead) { continue; }

                MethodInfo mget = p.GetGetMethod(false);
                MethodInfo mset = p.GetSetMethod(false);

                // Get and set methods have to be public
                if (mget == null) { continue; }
                if (mset == null) { continue; }

                if (p.PropertyType == typeof(DateTime?))
                {
                    DateTime? previousValue = p.GetValue(dto, null) as DateTime?;
                    DateTime? currentValue = FromSiteDateTime(executionContext.SiteId, previousValue);
                    if (previousValue != currentValue)
                    {
                        p.SetValue(dto, currentValue);
                    }
                }
                else
                {
                    DateTime previousValue = (DateTime)p.GetValue(dto, null);
                    DateTime currentValue = FromSiteDateTime(executionContext.SiteId, previousValue);
                    if (previousValue != currentValue)
                    {
                        p.SetValue(dto, currentValue);
                    }
                }
            }
            log.LogMethodExit(dto);
            return dto;
        }

        public static T ToSiteDateTime<T>(ExecutionContext executionContext, T dto, string ignoreColumns = "")
        {
            log.LogMethodEntry(executionContext, dto);
            if (dto == null)
            {
                log.LogMethodExit("dto == null");
                return dto;
            }
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                if (ignoreColumns != null && ignoreColumns.Contains(p.Name))
                {
                    continue;
                }
                // Only modify the date time properties
                if (p.PropertyType != typeof(DateTime) && p.PropertyType != typeof(DateTime?)) { continue; }

                // If not writable then cannot null it; if not readable then cannot check it's value
                if (!p.CanWrite || !p.CanRead) { continue; }

                MethodInfo mget = p.GetGetMethod(false);
                MethodInfo mset = p.GetSetMethod(false);

                // Get and set methods have to be public
                if (mget == null) { continue; }
                if (mset == null) { continue; }

                if (p.PropertyType == typeof(DateTime?))
                {
                    DateTime? previousValue = p.GetValue(dto, null) as DateTime?;
                    DateTime? currentValue = ToSiteDateTime(executionContext.SiteId, previousValue);
                    if (previousValue != currentValue)
                    {
                        p.SetValue(dto, currentValue);
                    }
                }
                else
                {
                    DateTime previousValue = (DateTime)p.GetValue(dto, null);
                    DateTime currentValue = ToSiteDateTime(executionContext.SiteId, previousValue);
                    if (previousValue != currentValue)
                    {
                        p.SetValue(dto, currentValue);
                    }
                }
            }
            log.LogMethodExit(dto);
            return dto;
        }

        public static List<T> FromSiteDateTime<T>(List<T> dtoList, string ignoreColumns = "", string siteIdColumn = "SiteId")
        {
            log.LogMethodEntry(dtoList, ignoreColumns);
            if (dtoList == null || dtoList.Any() == false)
            {
                log.LogMethodExit("dtoList is empty");
                return dtoList;
            }
            MethodInfo acceptChangesMethod = typeof(T).GetMethod("AcceptChanges", BindingFlags.Public | BindingFlags.Instance);
            foreach (var dto in dtoList)
            {
                FromSiteDateTime(dto, ignoreColumns, siteIdColumn);
                if (acceptChangesMethod != null)
                {
                    acceptChangesMethod.Invoke(dto, new object[] { });
                }
            }
            return dtoList;
        }

        public static T FromSiteDateTime<T>(T dto, string ignoreColumns = "", string siteIdColumn = "SiteId")
        {
            log.LogMethodEntry(dto);
            if (dto == null)
            {
                log.LogMethodExit("dto == null");
                return dto;
            }

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int siteId = -1;
            if(siteContainer.IsCorporate())
            {
                PropertyInfo siteIdPropertyInfo = properties.FirstOrDefault(x => x.Name == siteIdColumn);
                siteId = (int)siteIdPropertyInfo.GetValue(dto);
            }
            
            foreach (PropertyInfo p in properties)
            {
                if (ignoreColumns != null && ignoreColumns.Contains(p.Name))
                {
                    continue;
                }
                // Only modify the date time properties
                if (p.PropertyType != typeof(DateTime) && p.PropertyType != typeof(DateTime?)) { continue; }

                // If not writable then cannot null it; if not readable then cannot check it's value
                if (!p.CanWrite || !p.CanRead) { continue; }

                MethodInfo mget = p.GetGetMethod(false);
                MethodInfo mset = p.GetSetMethod(false);

                // Get and set methods have to be public
                if (mget == null) { continue; }
                if (mset == null) { continue; }

                if (p.PropertyType == typeof(DateTime?))
                {
                    DateTime? previousValue = p.GetValue(dto, null) as DateTime?;
                    DateTime? currentValue = FromSiteDateTime(siteId, previousValue);
                    if (previousValue != currentValue)
                    {
                        p.SetValue(dto, currentValue);
                    }
                }
                else
                {
                    DateTime previousValue = (DateTime)p.GetValue(dto, null);
                    DateTime currentValue = FromSiteDateTime(siteId, previousValue);
                    if (previousValue != currentValue)
                    {
                        p.SetValue(dto, currentValue);
                    }
                }
            }
            log.LogMethodExit(dto);
            return dto;
        }
    }
}

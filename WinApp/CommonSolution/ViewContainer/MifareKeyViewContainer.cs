/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Container class, retrieves and caches the MIFARE keys used for reading and writing MIFARE tags
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Container class, retrieves and caches the MIFARE keys used for reading and writing MIFARE tags
    /// </summary>
    public class MifareKeyViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly MifareKeyContainerDTOCollection mifareKeyContainerDTOCollection;
        private readonly int siteId;

        internal MifareKeyViewContainer(int siteId):this(siteId, GetMifareKeyContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static MifareKeyContainerDTOCollection GetMifareKeyContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            MifareKeyContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IMifareKeyUseCases mifareKeyUseCases = MifareKeyUseCaseFactory.GetIMifareKeyUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<MifareKeyContainerDTOCollection> task = mifareKeyUseCases.GetMifareKeyContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving MifareKeyContainerDTOCollection.", ex);
                result = new MifareKeyContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }

        internal MifareKeyViewContainer(int siteId, MifareKeyContainerDTOCollection mifareKeyContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, mifareKeyContainerDTOCollection);
            this.siteId = siteId;
            this.mifareKeyContainerDTOCollection = mifareKeyContainerDTOCollection;
            log.LogMethodExit();
        }

        /// <summary>
        /// returns the latest in MifareKeyContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal MifareKeyContainerDTOCollection GetMifareKeyContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (mifareKeyContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(mifareKeyContainerDTOCollection);
            return mifareKeyContainerDTOCollection;
        }

        internal MifareKeyViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if(LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            MifareKeyContainerDTOCollection latestMifareKeyContainerDTOCollection = GetMifareKeyContainerDTOCollection(siteId, mifareKeyContainerDTOCollection.Hash, rebuildCache);
            if (latestMifareKeyContainerDTOCollection == null || 
                latestMifareKeyContainerDTOCollection.MifareKeyContainerDTOList == null ||
                latestMifareKeyContainerDTOCollection.MifareKeyContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            MifareKeyViewContainer result = new MifareKeyViewContainer(siteId, latestMifareKeyContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }

        internal IEnumerable<MifareKeyContainerDTO> GetMifareKeyContainerDTOList()
        {
            log.LogMethodEntry();
            List<MifareKeyContainerDTO> result = mifareKeyContainerDTOCollection.MifareKeyContainerDTOList;
            log.LogMethodExit(result);
            return result;
        }
    }
}

/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - FileResourceFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.0      21-Sep-2021      Lakshminarayana           Created 
 ********************************************************************************************/
using System;
using System.Configuration;
using System.IO;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.Core.GenericUtilities.FileResources
{
    public class FileResourceFactory
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static FileResource GetFileResource(ExecutionContext executionContext, string defaultValueName, string fileName, bool secure)
        {
            log.LogMethodEntry(executionContext, defaultValueName, fileName, secure);
            FileResource result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                string webApiUrl = ConfigurationManager.AppSettings["WEB_API_URL"];
                string origin = ConfigurationManager.AppSettings["WEB_API_ORIGIN_KEY"];
                result = new CachedFileResource(new RemoteFileResource(executionContext, defaultValueName, fileName, secure, webApiUrl, origin));
            }
            else
            {
                if(IsFileSavedInAppServer(defaultValueName, fileName))
                {
                    result = new LocalFileResource(executionContext, defaultValueName, fileName, secure);
                }
                else
                {
                    string webApiUrl = ConfigurationManager.AppSettings["WEB_API_URL"];
                    string origin = ConfigurationManager.AppSettings["WEB_API_ORIGIN_KEY"];
                    if (string.IsNullOrWhiteSpace(webApiUrl) || 
                        string.IsNullOrWhiteSpace(origin))
                    {
                        result = new CachedFileResource(new DataBaseFileResource(executionContext ,defaultValueName, fileName, secure));
                    }
                    else
                    {
                        result = new CachedFileResource(new RemoteFileResource(executionContext, defaultValueName, fileName, secure, webApiUrl, origin));
                    }
                }
            }

            log.LogMethodExit(result);
            return result;
        }

        private static bool IsFileSavedInAppServer(string defaultValueName, string fileName)
        {
            log.LogMethodEntry(defaultValueName, fileName);
            bool result = false;
            string folderName = ParafaitDefaultContainerList.GetParafaitDefault(SiteContainerList.GetMasterSiteId(), defaultValueName);
            if (string.IsNullOrWhiteSpace(folderName))
            {
                return result;
            }
            result = Directory.Exists(folderName);
            log.LogMethodExit(result);
            return result;
        }
    }
}

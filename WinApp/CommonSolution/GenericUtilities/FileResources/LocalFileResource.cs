/********************************************************************************************
 * Project Name -GenericUtilities
 * Description  -LocalFileResource class to store files 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00     21-Sep-2021       Lakshminarayana           Created
 ********************************************************************************************/
using System;
using System.IO;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities.FileResources
{
    public class LocalFileResource : FileResource
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public LocalFileResource(ExecutionContext executionContext, string defaultValueName, string fileName, bool secure)
            :base(executionContext, defaultValueName, fileName, secure)
        {
            log.LogMethodEntry(executionContext, defaultValueName, fileName, secure);
            log.LogMethodExit();
        }

        protected override async Task<Stream> GetImpl()
        {
            log.LogMethodEntry();
            string filePath = await GetLocalPath();
            if(string.IsNullOrWhiteSpace(filePath))
            {
                throw new Exception("File " + fileName + " does not exists");
            }
            if (File.Exists(filePath) == false)
            {
                throw new Exception("File " + fileName + " does not exists");
            }
            Stream result = File.Open(filePath, FileMode.Open, FileAccess.Read);
            log.LogMethodExit("Returning data from local file system.");
            return result;
        }

        /// <summary>
        /// To Get the hash value of the file
        /// </summary>
        /// <returns>hash value of the file</returns>
        public override async Task<string> GetHash()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            string filePath = await GetLocalPath();
            if (string.IsNullOrWhiteSpace(filePath))
            {
                log.LogMethodExit(result, "file not found");
                return result;
            }
            using (Stream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                result = GetHash(stream);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// To Get the file folder path
        /// </summary>
        /// <returns>Local Folder Path</returns>
        public override async Task<string> GetLocalPath()
        {
            log.LogMethodEntry();
            string folderName = ParafaitDefaultContainerList.GetParafaitDefault(SiteContainerList.GetMasterSiteId(), defaultValueName);
            if (string.IsNullOrWhiteSpace(folderName))
            {
                return string.Empty;
            }
            string filePath = Path.Combine(folderName, fileName);
            log.LogMethodExit();
            return filePath;
        }

        /// <summary>
        /// save the file
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public override async Task<bool> Save(Stream inputStream)
        {
            log.LogMethodEntry();
            string filePath = await GetLocalPath();
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write,
                    FileShare.None, 4096, useAsync: true))
            {
                if(secure)
                {
                    await Encryption.Encrypt(inputStream, fs);
                }
                else
                {
                    await inputStream.CopyToAsync(fs);
                }
            }
            log.LogMethodExit();
            return true;
        }
    }
}

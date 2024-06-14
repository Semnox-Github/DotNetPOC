/********************************************************************************************
 * Project Name -GenericUtilities
 * Description  -DataBaseFileResource class to get and store files 
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
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.Core.GenericUtilities.FileResources
{
    public class DataBaseFileResource : FileResource
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private byte[] fileDataByteArray;

        public DataBaseFileResource(ExecutionContext executionContext, string defaultValueName, string fileName, bool secure)
            : base(executionContext, defaultValueName, fileName, secure)
        {
            log.LogMethodEntry(defaultValueName, fileName, secure);
            log.LogMethodExit();
        }


        /// <summary>
        /// Get the file
        /// </summary>
        /// <returns>hash value of the file</returns>
        protected override async Task<Stream> GetImpl()
        {
            log.LogMethodEntry();
            MemoryStream result;
            if(fileDataByteArray != null)
            {
                result = new MemoryStream(fileDataByteArray);
                log.LogMethodExit();
                return result;
            }
            string filePath = await GetLocalPath();
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new Exception("File " + fileName + " does not exists");
            }
            FileServiceDataHandler fileServiceDataHandler = new FileServiceDataHandler();
            object fileData = fileServiceDataHandler.GetFile(filePath);
            if (fileData == DBNull.Value)
            {
                throw new Exception("File " + fileName + " does not exists");
            }
            fileDataByteArray = fileData as byte[];
            if (fileDataByteArray == null)
            {
                throw new Exception("File " + fileName + " does not exists");
            }
            result = new MemoryStream(fileDataByteArray);
            log.LogMethodExit();
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
            try
            {
                Stream stream = await Get();
                result = GetHash(stream);
            }
            catch
            {
                result = string.Empty;
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// To Get the local path of the file
        /// </summary>
        /// <returns>hash value of the file</returns>
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
        /// <param name="inputStream">inputStream</param>
        /// <returns></returns>
        public override async Task<bool> Save(Stream inputStream)
        {
            log.LogMethodEntry();
            FileServiceDataHandler fileServiceDataHandler = new FileServiceDataHandler();
            using (MemoryStream ms = new MemoryStream())
            {
                if (secure)
                {
                    await Encryption.Encrypt(inputStream, ms);
                }
                else
                {
                    await inputStream.CopyToAsync(ms);
                }
                string filePath = await GetLocalPath();
                fileServiceDataHandler.SaveFile(filePath, ms.ToArray());
            }
            bool result = true;
            log.LogMethodExit("Saving to the server through DB");
            return result;
        }
    }
}

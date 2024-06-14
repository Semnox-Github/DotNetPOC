/********************************************************************************************
 * Project Name -GenericUtilities
 * Description  -CachedFileResource class to store cache files 
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
using System.Reflection;
using Semnox.Core.Utilities;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities.FileResources
{
    public class CachedFileResource : FileResource
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static bool cacheAvailable;
        private static string cacheFolderPath;
        private readonly FileResource fileResource;

        static CachedFileResource()
        {
            log.LogMethodEntry();
            cacheFolderPath = GetCacheFolderPath();
            if(string.IsNullOrWhiteSpace(cacheFolderPath))
            {
                cacheAvailable = false;
            }
            cacheAvailable = IsDirectoryWritable(cacheFolderPath);
            log.LogMethodExit();
        }

        /// <summary>
        /// To check Availability of cache 
        /// </summary>
        /// <param name="dirPath">dirPath</param>
        /// <param name="throwIfFails">throwIfFails</param>
        /// <returns></returns>
        private static bool IsDirectoryWritable(string dirPath, bool throwIfFails = false)
        {
            log.LogMethodEntry(dirPath,throwIfFails);
            try
            {
                if(Directory.Exists(dirPath) == false)
                {
                    Directory.CreateDirectory(dirPath);
                }
                using (FileStream fs = File.Create(Path.Combine(dirPath,Path.GetRandomFileName()),1,FileOptions.DeleteOnClose))
                { }
                log.LogMethodExit();
                return true;
            }
            catch(Exception  ex)
            {
                log.Error(ex);
                if (throwIfFails)
                    throw;
                else
                    return false;
            }
        }

        /// <summary>
        /// To Get the cache Folder path
        /// </summary>
        /// <returns>Cache Folder Path</returns>
        private static string GetCacheFolderPath()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            try
            {
                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                result = Path.Combine(assemblyFolder, "CachedFiles");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = string.Empty;
            }
            log.LogMethodExit();
            return result;
        }

        public CachedFileResource(FileResource fileResource)
            :base(fileResource.ExecutionContext,fileResource.DefaultValueName, fileResource.FileName, fileResource.Secure)
        {
            log.LogMethodEntry(fileResource);
            this.fileResource = fileResource;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get the file
        /// </summary>
        /// <returns></returns>
        public async override Task<Stream> Get()
        {
            log.LogMethodEntry();
            string localFilePath = await GetLocalPath();
            Stream result;
            if (string.IsNullOrWhiteSpace(localFilePath))
            {
                result = await fileResource.Get();
                return result;
            }
            result = await base.Get();
            log.LogMethodExit();
            return result;
        }

        protected override async Task<Stream> GetImpl()
        {
            log.LogMethodEntry();
            Stream result = File.Open(GetCachedFilePath(), FileMode.Open, FileAccess.Read);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// To check Availability of cache file
        /// </summary>
        /// <returns></returns>
        private bool IsCachedFileExists()
        {
            log.LogMethodEntry();
            bool result = false;
            string cachedFilePath = GetCachedFilePath();
            if (string.IsNullOrWhiteSpace(cachedFilePath))
            {
                log.LogMethodExit(result);
                return result;
            }
            result = File.Exists(cachedFilePath);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// To check cache file status based on hash 
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsCachedFileLatest()
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                using (FileStream fs = new FileStream(GetCachedFilePath(), FileMode.Open))
                {
                    string localFileHash = GetHash(fs);
                    string serverFileHash;
                    try
                    {
                        serverFileHash = await GetHash();
                    }
                    catch (Exception)
                    {
                        serverFileHash = string.Empty;
                    }
                    result = string.IsNullOrWhiteSpace(serverFileHash) || serverFileHash == localFileHash;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// save files to the cache
        /// </summary>
        /// <param name="dirPath">dirPath</param>
        /// <param name="throwIfFails">throwIfFails</param>
        /// <returns></returns>
        private async Task<bool> SaveToCache(Stream serverFileStream)
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                string cachedFilePath = GetCachedFilePath();
                if (Directory.Exists(Path.GetDirectoryName(cachedFilePath)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(cachedFilePath));
                }
                using (FileStream fs = new FileStream(cachedFilePath, FileMode.Create, FileAccess.Write,
                        FileShare.None, 4096, useAsync: true))
                {
                    if (secure)
                    {
                        await Encryption.Encrypt(serverFileStream, fs);
                    }
                    else
                    {
                        await serverFileStream.CopyToAsync(fs);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// To get hash value of the file
        /// </summary>
        /// <returns></returns>
        public override async Task<string> GetHash()
        {
            log.LogMethodEntry();
            string result = await fileResource.GetHash();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// To get local path of the file
        /// </summary>
        /// <returns>File path</returns>
        public override async Task<string> GetLocalPath()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            if(cacheAvailable == false)
            {
                log.LogMethodExit(result, "caching is disabled");
                return result;
            }
            if(IsCachedFileExists() == false || await IsCachedFileLatest() == false)
            {
                if(await DownloadServerFile())
                {
                    result = GetCachedFilePath();
                }
            }
            else
            {
                result = GetCachedFilePath();
            }
            log.LogMethodExit(result);
            return result;
        }

        private async Task<bool> DownloadServerFile()
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                Stream serverFileStream = await fileResource.Get();
                result = await SaveToCache(serverFileStream);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                result = false;
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// To get cache file path 
        /// </summary>
        /// <returns>File path</returns>
        private string GetCachedFilePath()
        {
            log.LogMethodEntry();
            string result = string.Empty;
            if (cacheAvailable == false)
            {
                log.LogMethodExit(result);
                return result;
            }
            result = Path.Combine(cacheFolderPath, defaultValueName, fileName);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// save file
        /// </summary>
        /// <param name="inputStream">inputStream</param>
        /// <returns></returns>
        public override async Task<bool> Save(Stream inputStream)
        {
            log.LogMethodEntry();
            bool result = false;
            if(cacheAvailable == false)
            {
                result = await fileResource.Save(inputStream);
                log.LogMethodExit(result, "caching is disabled");
                return result;
            }
            if(await SaveToCache(inputStream) == false)
            {
                throw new Exception("Unable to save the file to local cache");
            }

            using (Stream cachedFileStream = secure? await GetSecureStream() : await GetImpl())
            {
                result = await fileResource.Save(cachedFileStream);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}

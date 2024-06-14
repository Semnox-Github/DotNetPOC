/********************************************************************************************
 * Project Name -GenericUtilities
 * Description  -FileResource class to handle files 
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
using System.Security.Cryptography;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities.FileResources
{
    public abstract class FileResource
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly ExecutionContext executionContext;
        protected readonly string defaultValueName;
        protected readonly string fileName;
        protected readonly bool secure;
        public FileResource(ExecutionContext executionContext, string defaultValueName, string fileName, bool secure)
        {
            log.LogMethodEntry(executionContext, defaultValueName, fileName, secure);
            this.executionContext = executionContext;
            this.defaultValueName = defaultValueName;
            this.fileName = fileName;
            this.secure = secure;
            log.LogMethodExit();
        }

        public async virtual Task<Stream> Get()
        {
            log.LogMethodEntry();
            Stream result;
            if (secure)
            {
                result = await GetSecureStream();
            }
            else
            {
                result = await GetImpl();
            }
            log.LogMethodExit();
            return result;
        }

        protected async Task<Stream> GetSecureStream()
        {
            log.LogMethodEntry();
            Stream result;
            bool isEncrypted = false;
            using (Stream stream = await GetImpl())
            {
                isEncrypted = await Encryption.IsValidEncryptedStream(stream);
            }
            if (isEncrypted == false)
            {
                result = await GetImpl();
                log.LogMethodExit("returning unencrypted stream");
                return result;
            }
            Stream encryptedStream = await GetImpl();
            result = await Encryption.Decrypt(encryptedStream);
            log.LogMethodExit("returning decrypted stream");
            return result;
        }

        protected virtual Task<Stream> GetImpl()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            throw new InvalidOperationException();
        }

        public abstract Task<bool> Save(Stream inputStream);

        public abstract Task<string> GetLocalPath();

        public abstract Task<string> GetHash();

        protected string GetHash(Stream stream)
        {
            log.LogMethodEntry();
            ByteArray byteArray;
            using (MD5 md5 = MD5.Create())
            {
                byteArray = new ByteArray(md5.ComputeHash(stream));
            }
            string result = byteArray.ToString();
            log.LogMethodExit(result);
            return result;
        }

        public ExecutionContext ExecutionContext
        {
            get
            {
                return executionContext;
            }
        }

        /// <summary>
        /// Get method of the DefaultValueName field
        /// </summary>
        public string DefaultValueName
        {
            get
            {
                return defaultValueName;
            }
        }

        /// <summary>
        /// Get method of the FileName field
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        /// <summary>
        /// Get method of the Secure field
        /// </summary>
        public bool Secure
        {
            get
            {
                return secure;
            }
        }
    }
}

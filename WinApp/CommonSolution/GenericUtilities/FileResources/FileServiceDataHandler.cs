/********************************************************************************************
 * Project Name - Utilities
 * Description  - FileServiceDataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.140.00    15-Sep-2021   Lakshminarayana       Created 
  ********************************************************************************************/
using System;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities.FileResources
{
    public class FileServiceDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of ProfileDataHandler class
        /// </summary>
        public FileServiceDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns file 
        /// <param name="fileUrl">file url</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        public object GetFile(string fileUrl)
        {
            log.LogMethodEntry(fileUrl);
            object file = null;
            string query = @"exec ReadBinaryDataFromFile @FileUrl";
            file = dataAccessHandler.executeScalar(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@FileUrl", fileUrl) }, null);
            log.LogMethodExit("file");
            return file;
        }

        /// <summary>
        /// Save the file to destination url
        /// </summary>
        /// <param name="destinationFileUrl">destination url</param>
        /// <param name="file">file to be saved</param>
        /// <returns></returns>
        public void SaveFile(string destinationFileUrl, byte[] file)
        {
            log.LogMethodEntry(destinationFileUrl, "file");
            try
            {
                string query = @"exec SaveBinaryDataToFile @bytes, @FileName";
                dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@bytes", file), dataAccessHandler.GetSQLParameter("@FileName", destinationFileUrl) }, null);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while saving the file", ex);
                throw ex;
            }
            log.LogMethodExit();
        }
    }
}

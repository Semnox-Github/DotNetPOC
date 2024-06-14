/********************************************************************************************
 * Project Name - Concurrent Programs
 * Description  - DB Connector DTO Class for Concurrent Programs
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    public class DatabaseConnectorDTO
    {
        private bool isMultiDb;
        private string connectionString;
        private string message;
        private string applicationFolderPath;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public DatabaseConnectorDTO()
        {
            isMultiDb = false;
            connectionString = string.Empty;
            message = string.Empty;
            applicationFolderPath = null;
        }


        /// <summary>
        ///  Get/Set method of the IsMultiDb field
        /// </summary>
        public bool IsMultiDb
        {
            get { return isMultiDb; }
            set { isMultiDb = value; }
        }

        /// <summary>  
        ///  Get/Set method of the ConnectionString field
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        /// <summary>
        ///  Get/Set method of the Message field
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }


        /// <summary>
        ///  Get/Set method of the ApplicationFolderPath field
        /// </summary>
        public string ApplicationFolderPath
        {
            get { return applicationFolderPath; }
            set { applicationFolderPath = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// 
    /// </summary>
    public class ReportServerCommon
    {
        /// <summary>
        /// Defualt Constructor
        /// </summary>
        public ReportServerCommon()
        {
            isMultiDb = false;
            connectionString = "";
            message = "";
            isConnectionEncrypted = false;
        }

        private bool isMultiDb;
        private string connectionString;
        private string message;
        private bool isConnectionEncrypted;

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
        ///  Get/Set method of the IsConnectionEncrypted field
        /// </summary>
        public bool IsConnectionEncrypted
        {
            get { return isConnectionEncrypted; }
            set { isConnectionEncrypted = value; }
        }

    }
}

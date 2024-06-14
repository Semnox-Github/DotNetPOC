/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Data object of the FileInfo
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        13-May-2019    Lakshminarayana          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class FileInfoDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string name;
        private string hash;

        /// <summary>
        /// Default constructor
        /// </summary>
        public FileInfoDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public FileInfoDTO(string name, string hash)
        {
            log.LogMethodEntry(name, hash);
            this.name = name;
            this.hash = hash;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Hash { get { return hash; } set { hash = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}

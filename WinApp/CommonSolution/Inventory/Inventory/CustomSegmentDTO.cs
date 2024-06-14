/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - Mirror Object of SegmentsDTO 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.0    28-Jun-2019      Mehraj        Created                          
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    class CustomSegmentDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        private string name;
        private string segmentDefinationId;
        private string dataSourceType;
        private string isMandatory;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CustomSegmentDTO()
        {
            log.LogMethodEntry();
            segmentDefinationId = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="segmentDefinationId"></param>
        /// <param name="dataSourceType"></param>
        /// <param name="isMandatory"></param>
        public CustomSegmentDTO(string name, string segmentDefinationId, string dataSourceType, string isMandatory)
        {
            log.LogMethodEntry();
            this.name = name;
            this.segmentDefinationId = segmentDefinationId;
            this.dataSourceType = dataSourceType;
            this.isMandatory = isMandatory;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SegmentDefinationId field
        /// </summary>
        public string SegmentDefinationId { get { return segmentDefinationId; } set { segmentDefinationId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DataSourceType field
        /// </summary>
        public string DataSourceType { get { return dataSourceType; } set { dataSourceType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsMandatory field
        /// </summary>
        public string IsMandatory { get { return isMandatory; } set { isMandatory = value; this.IsChanged = true; } }

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

        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }


    }
}

/********************************************************************************************
 * Project Name - Utilities
 * Description  - TaskTypesContainerDTO class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    02-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Semnox.Core.Utilities
{
   public class TaskTypesContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int taskTypeId;
        string taskType;
        string requiresManagerApproval;
        string displayInPos;
        string taskTypeName;
        public TaskTypesContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        /// 
        public TaskTypesContainerDTO(int taskTypeIdPassed, string taskTypePassed, string requiresManagerApprovalPassed, string displayInPosPassed,
                                     string taskTypeNamePassed) : this()
        {
            log.LogMethodEntry(taskTypeIdPassed,taskTypePassed,requiresManagerApprovalPassed,displayInPosPassed,
                               taskTypeNamePassed);
            this.taskTypeId= taskTypeIdPassed;
            this.taskType= taskTypePassed;
            this.requiresManagerApproval= requiresManagerApprovalPassed;
            this.displayInPos= displayInPosPassed;
            this.taskTypeName= taskTypeNamePassed;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the TaskTypeId field
        /// </summary>
        [DisplayName("Task Type Id")]
        [ReadOnly(true)]
        public int TaskTypeId { get { return taskTypeId; } set { taskTypeId = value; } }
        /// <summary>
        /// Get/Set method of the TaskType field
        /// </summary>
        [DisplayName("Task Type")]
        [ReadOnly(true)]
        public string TaskType { get { return taskType; } set { taskType = value; } }
        /// <summary>
        /// Get/Set method of the RequiresManagerApproval field
        /// </summary>
        [DisplayName("Requires Manager Approval")]
        [ReadOnly(true)]
        public string RequiresManagerApproval { get { return requiresManagerApproval; } set { requiresManagerApproval=value; } }
        /// <summary>
        /// Get/Set method of the DisplayInPos field
        /// </summary>
        [DisplayName("Display In Pos")]
        [ReadOnly(true)]
        public string DisplayInPos { get { return displayInPos; } set { displayInPos = value; } }
        /// <summary>
        /// Get/Set method of the TaskTypeName field
        /// </summary>
        [DisplayName("Task Type Name")]
        [ReadOnly(true)]
        public string TaskTypeName { get { return taskTypeName; } set { taskTypeName = value; } }

    }
}

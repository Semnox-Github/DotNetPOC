/********************************************************************************************
 * Project Name - Utilities
 * Description  - TaskTypesContainerDTOCollection class 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Core.Utilities
{
   public  class TaskTypesContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TaskTypesContainerDTO> taskTypesContainerDTOList;
        private string hash;

        public TaskTypesContainerDTOCollection()
        {
            log.LogMethodEntry();
            taskTypesContainerDTOList = new List<TaskTypesContainerDTO>();
            log.LogMethodExit();
        }
        public TaskTypesContainerDTOCollection(List<TaskTypesContainerDTO> taskTypesContainerDTOList)
        {
            log.LogMethodEntry(taskTypesContainerDTOList);
            this.taskTypesContainerDTOList = taskTypesContainerDTOList;
            if (taskTypesContainerDTOList == null)
            {
                taskTypesContainerDTOList = new List<TaskTypesContainerDTO>();
            }
            hash = new DtoListHash(taskTypesContainerDTOList);
            log.LogMethodExit();
        }

        public List<TaskTypesContainerDTO> TaskTypesContainerDTOList
        {
            get
            {
                return taskTypesContainerDTOList;
            }

            set
            {
                taskTypesContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }

    }
}

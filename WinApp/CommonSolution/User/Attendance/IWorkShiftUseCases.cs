﻿/********************************************************************************************
* Project Name - User
* Description  - Interface for WorkShift Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     01-Apr-2021     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public interface IWorkShiftUseCases
    {
        Task<List<WorkShiftDTO>> GetWorkShift(List<KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>> searchParameters, bool loadChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction = null);
        Task<List<WorkShiftDTO>> SaveWorkShift(List<WorkShiftDTO> shiftDTOList);
    }
}

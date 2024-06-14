﻿/********************************************************************************************
 * Project Name - IReaderConfigurationContainerDataService  Class
 * Description  - IReaderConfigurationContainerDataService class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Parafait.Game
{
    public interface IReaderConfigurationContainerDataService
    {
        List<MachineAttributeDTO> Get(DateTime? maxLastUpdatedDate, string hash);
    }
}

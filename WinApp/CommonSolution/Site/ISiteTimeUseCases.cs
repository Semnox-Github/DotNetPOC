/********************************************************************************************
 * Project Name - Site
 * Description  - ISiteTimeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      08-07-2021     Prajwal S               Created : F&B web design
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Site
{
    public interface ISiteTimeUseCases
    {
        Task<DateTime> GetSiteTime(int siteId);
    }
}

/********************************************************************************************
 * Project Name -TET 
 * Description  - VisitorRequest
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By                    Remarks          
 *********************************************************************************************
 *1.00        26-Feb-2022   Nagendra Prasad(Vidita)       Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Semnox.Parafait.ThirdParty.TET
{
    public class VisitorRequest
    {
        public class GetVisitorRequest
        {
            public string OrderID { set; get; }
        }
    }
}
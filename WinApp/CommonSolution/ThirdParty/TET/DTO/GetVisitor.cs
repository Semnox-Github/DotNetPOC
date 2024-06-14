/********************************************************************************************
 * Project Name -TET 
 * Description  - GetVisitorResponse
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
    public class GetVisitorResponse
    {
        public class GetVisitorResponseRnt
        {
            public DateTime VisitorDate { set; get; }
            public string VisitorEmail { set; get; }
            public int NumberOfVisitor { set; get; }
        }
    }
}
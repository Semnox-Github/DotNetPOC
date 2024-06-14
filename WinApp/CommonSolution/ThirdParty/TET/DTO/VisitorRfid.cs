/********************************************************************************************
 * Project Name -TET 
 * Description  - VisitorRfid
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
    public class VisitorRfid
    {
        public class VisitorRfidRequest
        {
            public List<Visitor> VisitorId { set; get; }
        }
        public class Visitor
        {
            public string Rfid { set; get; }
        }
    }
}
/********************************************************************************************
* Project Name - ThirdParty
* Description  - AlohaBSPResponse
* 
**************
**Version Log
**************
*Version     Date             Modified By         Remarks          
*********************************************************************************************
 *2.130.7       26-July-2021      Girish Kundar      Created
*********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.AlohaBSP
{
    public class Attribute
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class TopicId
    {
        public string name { get; set; }
    }

    public class AlohaBSPResponse
    {
        public List<Attribute> attributes { get; set; }
        public TopicId topicId { get; set; }
        public string payload { get; set; }
    }
}

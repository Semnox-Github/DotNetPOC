/********************************************************************************************
 * Project Name - Publish To site Controller                                                                         
 * Description  - Controller of the Game class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.50        12-Dec-2018    Jagan Mohana          Created 
 *2.90        12-Jun-2020    Girish Kundar         Modified :  Enhanced to support bulk publishing feature 
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.Publish
{
    public class PublishToSiteDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool isSelected { get; set; }
        public bool isSite { get; set; }
        public string EntityName { get; set; } 
        public string EntityId { get; set; }
        public List<PublishToSiteDTO> Children { get; set; }
        
    }
}

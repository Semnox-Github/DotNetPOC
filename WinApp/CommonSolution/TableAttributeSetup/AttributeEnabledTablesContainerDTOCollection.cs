/********************************************************************************************
 * Project Name - Device 
 * Description  - Data object of AttributeEnabledTablesContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.140.0      24-Aug-2021   Fiona                  Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class AttributeEnabledTablesContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AttributeEnabledTablesContainerDTO> attributeEnabledTablesContainerDTOList;
        private string hash;
        public AttributeEnabledTablesContainerDTOCollection()
        {
            log.LogMethodEntry();
            this.attributeEnabledTablesContainerDTOList = new List<AttributeEnabledTablesContainerDTO>();
            log.LogMethodExit();
        }
        public AttributeEnabledTablesContainerDTOCollection(List<AttributeEnabledTablesContainerDTO> attributeEnabledTablesContainerDTOList)
        {
            log.LogMethodEntry();
            this.attributeEnabledTablesContainerDTOList = attributeEnabledTablesContainerDTOList;
            if (this.attributeEnabledTablesContainerDTOList == null)
            {
                this.attributeEnabledTablesContainerDTOList = new List<AttributeEnabledTablesContainerDTO>();
            }
            this.hash = new DtoListHash(GetDTOList(attributeEnabledTablesContainerDTOList));
            log.LogMethodExit();
        }
        private IEnumerable<object> GetDTOList(List<AttributeEnabledTablesContainerDTO> attributeEnabledTablesContainerDTOList)
        {
            log.LogMethodEntry(attributeEnabledTablesContainerDTOList);
            foreach (AttributeEnabledTablesContainerDTO attributeEnabledTablesContainerDTO in attributeEnabledTablesContainerDTOList.OrderBy(x => x.TableName))
            {
                yield return attributeEnabledTablesContainerDTO;
            }
            log.LogMethodExit();
        }
        public List<AttributeEnabledTablesContainerDTO> AttributeEnabledTablesContainerDTOList
        {
            get
            {
                return attributeEnabledTablesContainerDTOList;
            }

            set
            {
                attributeEnabledTablesContainerDTOList = value;
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

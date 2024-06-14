/********************************************************************************************
 * Project Name - Device 
 * Description  - Data object of EnabledAttributesContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.140.0      17-Aug-2021   Fiona                   Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class EnabledAttributesContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<EnabledAttributesContainerDTO> enabledAttributesContainerDTOList;
        private string hash;
        public EnabledAttributesContainerDTOCollection()
        {
            log.LogMethodEntry();
            this.enabledAttributesContainerDTOList = new List<EnabledAttributesContainerDTO>();
            log.LogMethodExit();
        }
        public EnabledAttributesContainerDTOCollection(List<EnabledAttributesContainerDTO> enabledAttributesContainerDTOList)
        {
            log.LogMethodEntry();
            this.enabledAttributesContainerDTOList = enabledAttributesContainerDTOList;
            if (this.enabledAttributesContainerDTOList == null)
            {
                this.enabledAttributesContainerDTOList = new List<EnabledAttributesContainerDTO>();
            }
            this.hash = new DtoListHash(GetDTOList(enabledAttributesContainerDTOList));
            log.LogMethodExit();
        }

        private IEnumerable<object> GetDTOList(List<EnabledAttributesContainerDTO> enabledAttributesContainerDTOList)
        {
            log.LogMethodEntry(enabledAttributesContainerDTOList);
            foreach (EnabledAttributesContainerDTO enabledAttributesContainerDTO in enabledAttributesContainerDTOList.OrderBy(x => x.EnabledAttributeName))
            {
                yield return enabledAttributesContainerDTO;
            }
            log.LogMethodExit();
        }
        public List<EnabledAttributesContainerDTO> EnabledAttributesContainerDTOList
        {
            get
            {
                return enabledAttributesContainerDTOList;
            }

            set
            {
                enabledAttributesContainerDTOList = value;
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

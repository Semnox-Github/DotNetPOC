/********************************************************************************************
 * Project Name - Generic Utilities
 * Description  - Business Logic for ulc key data.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.0      04-Sep-2019   Mushahid Faizan     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class UlcKeyCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<UlcKeyDTO> values;
        public UlcKeyCollection(List<UlcKeyDTO> values)
        {
            log.LogMethodEntry(values);
            if (values == null || values.Count == 0)
            {
                this.values = new List<UlcKeyDTO>();
                return;
            }

            this.values = values;


            log.LogMethodExit();
        }

        public UlcKeyCollection(string pipeSeparatedUlcKeysString)
        {
            log.LogMethodEntry(pipeSeparatedUlcKeysString);
            values = new List<UlcKeyDTO>();
            if (string.IsNullOrEmpty(pipeSeparatedUlcKeysString))
            {
                return;
            }
            string[] ulcKeyStringArray =
                pipeSeparatedUlcKeysString.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> ulcKeyStringHashSet = new HashSet<string>();
            foreach (string ulcKeyString in ulcKeyStringArray)
            {
                if (UlcKey.IsValidKeyString(ulcKeyString) == false ||
                    ulcKeyStringHashSet.Contains(ulcKeyString))
                {
                    continue;
                }

                UlcKeyDTO ulcKeyDto = new UlcKeyDTO { Key = ulcKeyString };
                values.Add(ulcKeyDto);
                ulcKeyStringHashSet.Add(ulcKeyString);
            }

            if (values.Count > 0)
            {
                values.Last().CurrentKey = true;
            }
            log.LogMethodExit();
        }

        public List<UlcKeyDTO> Values
        {
            get { return values; }
        }

        public string PipeSeparatedUlcKeysString
        {
            get
            {
                string result = string.Empty;
                if (values == null || values.Count == 0)
                {
                    return result;
                }

                IEnumerable<UlcKeyDTO> filteredKeys = values.Where(x => UlcKey.IsValidKeyString(x.Key));
                result = string.Join("|", filteredKeys.OrderBy(x => x.CurrentKey ? 1 : 0).Select(x => x.Key));
                return result;
            }
        }
    }
}

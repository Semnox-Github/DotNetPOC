using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public class DPLFactory
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DPLFactory(Utilities utilities)
        {
            log.LogMethodEntry();
            this.utilities = utilities;
            log.LogMethodExit();
        }

        public Boolean  ValidFileFormat(string dplFileFormatCode)
        {
            log.LogMethodEntry(dplFileFormatCode); 
            if (Enum.IsDefined(typeof(DPLFile.DPLFileFormat), dplFileFormatCode))
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }
        public DPLFile GetDPLFileObject(string dplFileFormatCode, StreamReader dplNewFile)
        {
            log.Debug("Begins-GetDPLFileObject.");
            DPLFile.DPLFileFormat dplFileFormat;
            if (Enum.IsDefined(typeof(DPLFile.DPLFileFormat), dplFileFormatCode))
                dplFileFormat = (DPLFile.DPLFileFormat)Enum.Parse(typeof(DPLFile.DPLFileFormat), dplFileFormatCode, true);
            else
                dplFileFormat = DPLFile.DPLFileFormat.DPLFILERI;
            log.Debug("Ends-GetDPLFileObject.");
            switch (dplFileFormat)
            {
                case DPLFile.DPLFileFormat.DPLFILERI: return new DPLFileRI(utilities, dplNewFile);
                default: return new DPLFileRI(utilities, dplNewFile);
            }
            
        }
    }
}

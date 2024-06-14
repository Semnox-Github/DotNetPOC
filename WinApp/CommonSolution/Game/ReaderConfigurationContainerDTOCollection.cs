using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Game
{
    public class ReaderConfigurationContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ReaderConfigurationContainerDTO> readerConfigurationContainerDTOList;
        private string hash;

        public ReaderConfigurationContainerDTOCollection()
        {
            log.LogMethodEntry();
            readerConfigurationContainerDTOList = new List<ReaderConfigurationContainerDTO>();
            log.LogMethodExit();
        }

        public ReaderConfigurationContainerDTOCollection(List<ReaderConfigurationContainerDTO> readerConfigurationContainerDTOList)
        {
            log.LogMethodEntry(readerConfigurationContainerDTOList);
            this.readerConfigurationContainerDTOList = readerConfigurationContainerDTOList;
            if (readerConfigurationContainerDTOList == null)
            {
                readerConfigurationContainerDTOList = new List<ReaderConfigurationContainerDTO>();
            }
            hash = new DtoListHash(readerConfigurationContainerDTOList);
            log.LogMethodExit();
        }

        public List<ReaderConfigurationContainerDTO> ReaderConfigurationContainerDTOList
        {
            get
            {
                return readerConfigurationContainerDTOList;
            }

            set
            {
                readerConfigurationContainerDTOList = value;
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


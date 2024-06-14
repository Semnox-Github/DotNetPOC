/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to hold the External Product Group  details .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    07-Apr-2022   Ashish Bhat             Created : External  REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ThirdParty.External
{
    public class AllowedCreditType
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsSupported { get; set; }

        public AllowedCreditType()
        {
            log.LogMethodEntry();
            Name = string.Empty;
            Type = string.Empty;
            IsSupported = false;
            log.LogMethodExit();
        }

        public AllowedCreditType(string name, string type,
                                         bool isSupported)
        {
            log.LogMethodEntry(name, type, isSupported);
            this.Name = name;
            this.Type = type;
            this.IsSupported = isSupported;
            log.LogMethodExit();
        }
    }

    public class AllowedTask
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Name { get; set; }
        public bool IsSupported { get; set; }

        public AllowedTask()
        {
            log.LogMethodEntry();
            Name = string.Empty;
            IsSupported = false;
            log.LogMethodExit();
        }

        public AllowedTask(string name, bool isSupported)
        {
            log.LogMethodEntry(name, isSupported);
            this.Name = name;
            this.IsSupported = isSupported;
            log.LogMethodExit();
        }
    }

    public class ExternalConfigurationDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public List<AllowedCreditType> AllowedCreditTypes { get; set; }
        public int CardNumberLength { get; set; }
        public int MaxDecimalPlaces { get; set; }
        public List<string> PaymentTypes { get; set; }
        public bool VirtualPlay { get; set; }
        public List<AllowedTask> AllowedTasks { get; set; }
        public ExternalConfigurationDTO()
        {
            log.LogMethodEntry();
            CardNumberLength = -1;
            MaxDecimalPlaces = -1;
            VirtualPlay = false;
            AllowedTasks = new List<AllowedTask>();
            PaymentTypes = new List<string>();

            log.LogMethodExit();
        }
        public ExternalConfigurationDTO(List<AllowedCreditType> AllowedCreditTypes, int CardNumberLength,int MaxDecimalPlaces,
                                        List<string> PaymentTypes, bool Virtuaplay, List<AllowedTask> AllowedTasks)
        {
            log.LogMethodEntry(AllowedCreditTypes, CardNumberLength, MaxDecimalPlaces, PaymentTypes, Virtuaplay, AllowedTasks);

            this.AllowedCreditTypes = AllowedCreditTypes;
            this.CardNumberLength = CardNumberLength;
            this.MaxDecimalPlaces = MaxDecimalPlaces;
            this.PaymentTypes = PaymentTypes;
            this.VirtualPlay = VirtualPlay;
            this.AllowedTasks = AllowedTasks;
            log.LogMethodExit();


        }
    }
}

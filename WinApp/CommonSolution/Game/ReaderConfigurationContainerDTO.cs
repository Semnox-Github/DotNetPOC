using System.ComponentModel;

namespace Semnox.Parafait.Game
{
    public class ReaderConfigurationContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string attributeValue;
        private int attributeId;
        private string isFlag;
        private string isSoftwareAttribute;
        private MachineAttributeDTO.AttributeContext contextOfAttrib;
        private MachineAttributeDTO.MachineAttribute attribute;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ReaderConfigurationContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with parameter
        /// </summary>  
        
        public ReaderConfigurationContainerDTO(int attributeId, MachineAttributeDTO.MachineAttribute machineAttribute, string attributeValue, string isFlag, string isSoftwareAttribute,
                                MachineAttributeDTO.AttributeContext attributeContext)
            : this()
        {
            log.LogMethodEntry(attributeId, machineAttribute, attributeValue, isFlag, isSoftwareAttribute, attributeContext);
            this.attributeId = attributeId;
            this.attribute = machineAttribute;
            this.attributeValue = attributeValue;
            this.isFlag = isFlag;
            this.isSoftwareAttribute = isSoftwareAttribute;
            this.contextOfAttrib = attributeContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set for AttributeName
        /// </summary>
        public MachineAttributeDTO.MachineAttribute AttributeName { get { return attribute; } set { attribute = value; } }
        /// <summary>

        /// <summary>
        /// Get/Set for AttributeValue
        /// </summary>
        public string AttributeValue { get { return attributeValue; } set {attributeValue = value; }}
        /// <summary>
        /// Get/Set for AttributeId
        /// </summary>
        public int AttributeId { get { return attributeId; } set { attributeId = value;  } }
        /// <summary>
        /// Get/Set for IsFlag
        /// </summary>
        public string IsFlag { get { return isFlag; } set { isFlag = value;  } }
        /// <summary>
        /// Get/Set for IsSoftwareAttribute
        /// </summary>
        public string IsSoftwareAttribute { get { return isSoftwareAttribute; } set { isSoftwareAttribute = value;  } }
        /// <summary>
        /// Get/Set for ContextAttribute
        /// </summary>
        public MachineAttributeDTO.AttributeContext ContextOfAttribute { get { return contextOfAttrib; } set { contextOfAttrib = value;  } }
    }
}

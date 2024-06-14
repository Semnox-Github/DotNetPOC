/********************************************************************************************
 * Project Name - Printer                                                                        
 * Description  -CashdrawerContainerDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
 *2.140.0     11-Aug-2021      Girish Kundar     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Printer.Cashdrawers
{
   public class CashdrawerContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int cashdrawerId;
        private string cashdrawerName;
        private string interfaceType;
        private string communicationString;
        private int serialPort;
        private int serialPortBaud;
        private bool isSystem;
        private bool isActive;

        /// <summary>
        /// default constructor
        /// </summary>
        public CashdrawerContainerDTO()
        {
            log.LogMethodEntry();
            cashdrawerId = -1;
            cashdrawerName = string.Empty;
            communicationString = string.Empty;
            isSystem = false;
            interfaceType = CashdrawerIntefaceTypes.RECEIPTPRINTER.ToString();
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public CashdrawerContainerDTO(int cashdrawerId, string cashdrawerName, string interfaceType,
                                               string communicationString, int serialPort, int serialPortBaud,
                                               bool isSystem, bool isActive)
    : this()
        {
            log.LogMethodEntry(cashdrawerId, cashdrawerName, interfaceType, communicationString, serialPort, serialPortBaud,
                               isSystem, isActive);
            this.cashdrawerId = cashdrawerId;
            this.cashdrawerName = cashdrawerName;
            this.interfaceType = interfaceType;
            this.communicationString = communicationString;
            this.serialPort = serialPort;
            this.serialPortBaud = serialPortBaud;
            this.isSystem = isSystem;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the cashdrawerId field
        /// </summary>
        public int CashdrawerId { get { return cashdrawerId; } set {  cashdrawerId = value; } }
        /// <summary>
        /// Get/Set method of the cashdrawerName field
        /// </summary>
        public string CashdrawerName { get { return cashdrawerName; } set {  cashdrawerName = value; } }
        /// <summary>
        /// Get/Set method of the interfaceType field
        /// </summary>
        public string InterfaceType { get { return interfaceType; } set {  interfaceType = value; } }

        /// <summary>
        /// Get/Set method of the PrintString field
        /// </summary>
        public string CommunicationString { get { return communicationString; } set {  communicationString = value; } }
        /// <summary>
        /// Get/Set method of the serialPort field
        /// </summary>
        public int SerialPort { get { return serialPort; } set {  serialPort = value; } }
        /// <summary>
        /// Get/Set method of the PrintString field
        /// </summary>
        public int SerialPortBaud { get { return serialPortBaud; } set {  serialPortBaud = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsSystem { get { return isSystem; } set {  isSystem = value; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set {  isActive = value; } }

    }
}

/*=========================================================================================
'  Copyright(C):    Semnox Solutions 
' 
'  Description:     This sample program outlines the steps on how to
'                   transact with Mifare 1K/4K cards using ACR128
'  
'  Author :         Iqbal Mohammad
'
'  Module :         ACR122U.cs
'   
'  Date   :         June 18, 20014
'
'  Revision Trail:  
' =========================================================================================
 *  Modified to add Logger Methods by Deeksha on 09-Aug-2019
'=========================================================================================*/

using System.Collections.Generic;
using System.Threading;


namespace Semnox.Parafait.Device
{
    public class RioPro : ACRReader
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public RioPro()
            :base()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public RioPro(int DeviceAddress)
            :base(DeviceAddress)
        {
            log.LogMethodEntry(DeviceAddress);
            log.LogMethodExit();
        }

        public RioPro(byte[] defaultKey)
            : base(defaultKey)
        {
            log.LogMethodEntry("defaultKey");
            log.LogMethodExit();
        }

        public RioPro(int DeviceAddress, List<byte[]> defaultKey)
            :base(DeviceAddress, defaultKey)
        {
            log.LogMethodEntry(DeviceAddress, "defaultKey");
            log.LogMethodExit();
        }

        public override string readCardNumber()
        {
            log.LogMethodEntry();
            string cardNumber = base.readCardNumber();
            if (cardNumber != "")
            {
                bool response = Validate();

                if (response)
                {
                    log.LogMethodExit(cardNumber);
                    return cardNumber;
                }
            }
            log.LogMethodExit();
            return "";
        }

        internal override void initialize()
        {
            log.LogMethodEntry();
            _ModelNumber = "riopro";
            base.initialize();
            Thread.Sleep(5);
            stopListener();
            log.LogMethodExit();
        }
    }
}
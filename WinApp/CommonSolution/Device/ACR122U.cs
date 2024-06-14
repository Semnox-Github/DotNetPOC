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
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.80.7       21-Feb-2022     Guru S A            ACR reader performance fix 
'=========================================================================================*/

using System;
using System.Collections.Generic;
using System.Threading;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device
{
    public class ACR122U : ACRReader
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ACR122U()
            :base()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public ACR122U(DeviceDefinition deviceDefinition)
        :base(deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
            log.LogMethodExit();
        }

        public ACR122U(int DeviceAddress)
            :base(DeviceAddress)
        {
            log.LogMethodEntry(DeviceAddress);
            log.LogMethodExit();
        }

        public ACR122U(byte[] defaultKey)
            : base(defaultKey)
        {
            log.LogMethodEntry("defaultKey");
            log.LogMethodExit();
        }

        public ACR122U(int DeviceAddress, List<byte[]> defaultKey)
            :base(DeviceAddress, defaultKey)
        {
            log.LogMethodEntry(DeviceAddress, "defaultKey");
            log.LogMethodExit();
        }
        
        public ACR122U(int deviceAddress, bool readerForRechargeOnly)
          : base(deviceAddress, readerForRechargeOnly)
        {
            log.LogMethodEntry(deviceAddress, readerForRechargeOnly);
            log.LogMethodExit();
        }
        public override void beep(int repeat, bool asynchronous)
        {
            log.LogMethodEntry(repeat, asynchronous);
            if (asynchronous)
            {
                Thread thr = new Thread(() =>
                {
                    base.beep(0xFF, 0x00, 0x40, 0x00, 0x04, 0x01, 0x00, (byte)(repeat), 0x01);
                });

                thr.Start();
            }
            else
                base.beep(0xFF, 0x00, 0x40, 0x00, 0x04, 0x01, 0x00, (byte)(repeat), 0x01);
            log.LogMethodExit();
        }

        public override bool Validate()
        {
            log.LogMethodEntry();
            bool result;
            try
            {
                //currentChipType = acsProvider.getChipType();
                if (currentChipType == CHIP_TYPE.MIFARE_ULTRALIGHT)
                {
                    isRoamingCard = false;
                    TagSiteId = SiteId;
                    result = true;
                }
                else
                {
                    result = base.Validate();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while validating the card.", ex);
                result = false;
            }

            log.LogMethodExit(result);
            return result;
        }

        protected override void setDefaultLEDBuzzerState()
        {
            log.LogMethodEntry();
            base.setDefaultLEDBuzzerState(0xFF, 0x00, 0x52, 0x00, 0x00);
            log.LogMethodExit();
        }

        internal override void initialize()
        {
            log.LogMethodEntry();
            _ModelNumber = "122 ";
            base.initialize();
            log.LogMethodExit();
        }
    }
}
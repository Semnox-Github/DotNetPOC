/********************************************************************************************
 * Project Name - Device
 * Description  - This sample program outlines the steps on how to transact with Mifare cards using ACR1222L
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.70.2       07-Aug-2019       Deeksha            Added Logger Methods.
 * 2.80.7       21-Feb-2022      Guru S A            ACR reader performance fix 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device
{
    public class ACR1222L : ACRReader
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ACR1222L()
            :base()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public ACR1222L(DeviceDefinition deviceDefinition)
        :base(deviceDefinition)
        {
            log.LogMethodEntry(deviceDefinition);
            log.LogMethodExit();
        }

        public ACR1222L(int DeviceAddress)
            :base(DeviceAddress)
        {
            log.LogMethodEntry(DeviceAddress);
            log.LogMethodExit();
        }

        public ACR1222L(byte[] defaultKey)
            : base(defaultKey)
        {
            log.LogMethodEntry("defaultKey");
            log.LogMethodExit();
        }

        public ACR1222L(int DeviceAddress, List<byte[]> defaultKey)
            :base(DeviceAddress, defaultKey)
        {
            log.LogMethodEntry(DeviceAddress, "defaultKey");
            log.LogMethodExit();
        }

        public ACR1222L(string SerialNumber)
            : base(SerialNumber)
        {
            log.LogMethodEntry(SerialNumber);
            log.LogMethodExit();
        }
        public ACR1222L(int deviceAddress, bool readerForRechargeOnly)
          : base(deviceAddress, readerForRechargeOnly)
        {
            log.LogMethodEntry(deviceAddress, readerForRechargeOnly);
            log.LogMethodExit();
        }
        public ACR1222L(string serialNumber, bool readerForRechargeOnly)
           : base(serialNumber, readerForRechargeOnly)
        {
            log.LogMethodEntry(serialNumber, readerForRechargeOnly);
            log.LogMethodExit();
        }

        public override void beep(int repeat, bool asynchronous)
        {
            log.LogMethodEntry(repeat, asynchronous);
            if (asynchronous)
            {
                Thread thr = new Thread(() =>
                {
                    base.beep(0xE0, 0x00, 0x00, 0x28, 0x01, (byte)repeat);
                });

                thr.Start();
            }
            else
                base.beep(0xE0, 0x00, 0x00, 0x28, 0x01, (byte)repeat);
            log.LogMethodExit();
        }

        public override void beep(int repeat = 1)
        {
            log.LogMethodEntry(repeat);
            beep(repeat, true);
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
            base.setDefaultLEDBuzzerState(0xE0, 0x00, 0x00, 0x21, 0x01, 0x8f);
            log.LogMethodExit();
        }

        internal override void initialize()
        {
            log.LogMethodEntry();
            _ModelNumber = "1222";
            base.initialize();
            log.LogMethodExit();
        }

        internal override void initialize(string serialNumber)
        {
            log.LogMethodEntry(serialNumber);
            _ModelNumber = "1222";
            base.initialize(serialNumber);
            log.LogMethodExit();
        }

        public override string getSerialNumber()
        {
            log.LogMethodEntry();
            ACSProvider lclACS = new ACSProvider();
            try
            {
                lclACS.readerName = acsProvider.readerName;
                lclACS.connectDirect();

                Apdu apduC = new Apdu();
                apduC.lengthExpected = 21;
                apduC.data = new byte[] { 0xE0, 0x00, 0x00, 0x33, 0x00 };

                uint operationControlCode = (uint)PcscProvider.FILE_DEVICE_SMARTCARD + 3500 * 4;
                lclACS.sendCardControl(ref apduC, operationControlCode);
                string returnValue = ASCIIEncoding.ASCII.GetString(apduC.response, 5, 16);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while getting SerialNumber", expn);
                log.LogMethodExit();
                return "";
            }

            finally
            {
                try
                {
                    lclACS.disconnect();
                }
                catch (Exception expn)
                {
                    log.Error("Error disconnecting", expn);
                }
            }
        }

        public override void ClearDisplay()
        {
            log.LogMethodEntry();
            ACSProvider lclACS = new ACSProvider();
            try
            {
                lclACS.readerName = acsProvider.readerName;
                lclACS.connectDirect();

                Apdu apduC = new Apdu();
                apduC.lengthExpected = 2;
                apduC.data = new byte[] { 0xFF, 0x00, 0x60, 0x00, 0x00 };

                uint operationControlCode = (uint)PcscProvider.FILE_DEVICE_SMARTCARD + 3500 * 4;
                lclACS.sendCardControl(ref apduC, operationControlCode);

            }
            catch (Exception expn)
            {
                log.Error("Error occurred while clearing display.", expn);
            }
            finally
            {
                try
                {
                    lclACS.disconnect();
                }
                catch (Exception expn)
                {
                    log.Error("Error disconnecting", expn);
                }
            }
            log.LogMethodExit();
        }

        public override void DisplayMessage(params string[] Lines)
        {
            log.LogMethodEntry(Lines);
            ACSProvider lclACS = new ACSProvider();
            try
            {
                lclACS.readerName = acsProvider.readerName;
                lclACS.connectDirect();
                uint operationControlCode = (uint)PcscProvider.FILE_DEVICE_SMARTCARD + 3500 * 4;

                byte fontSet = 0x00;
                int lineOffset = 64;
                //if (Lines.Length > 2)
                //{
                //    fontSet = 0x20;
                //    lineOffset = 32;
                //}

                bool boldFont = false;

                int lineIndex = 0;
                foreach (string line in Lines)
                {
                    byte[] msg = Encoding.GetEncoding("GB18030").GetBytes(line.PadRight(16, ' '));
                    if (msg.Length > 16)
                        Array.Resize(ref msg, 16);

                    Apdu apdu = new Apdu();
                    apdu.lengthExpected = 2;
                    apdu.data = new byte[5 + msg.Length];

                    apdu.data[0] = 0xFF;
                    apdu.data[1] = 0x00;
                    apdu.data[2] = 0x69;
                    apdu.data[3] = (byte)(lineIndex * lineOffset);
                    apdu.data[4] = (byte)msg.Length;

                    Array.Copy(msg, 0, apdu.data, 5, msg.Length);

                    if (boldFont)
                        apdu.data[1] |= 0x01;

                    apdu.data[1] |= fontSet;

                    lclACS.sendCardControl(ref apdu, operationControlCode);
                    lineIndex++;

                    if (lineIndex == 2)
                        break;
                }
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while Displaying message.", expn);
            }
            finally
            {
                try
                {
                    lclACS.disconnect();
                }
                catch (Exception expn)
                {
                    log.Error("Error disconnecting", expn);
                }
            }
            log.LogMethodExit();
        }

        public override void DisplayMessage(int LineNumber, string Message)
        {
            log.LogMethodEntry(LineNumber, Message);
            if (LineNumber > 2)
            {
                log.LogMethodExit();
                return;
            }

            ACSProvider lclACS = new ACSProvider();
            try
            {
                lclACS.readerName = acsProvider.readerName;
                lclACS.connectDirect();
                uint operationControlCode = (uint)PcscProvider.FILE_DEVICE_SMARTCARD + 3500 * 4;

                byte fontSet = 0x00;
                int lineOffset = 64;

                bool boldFont = false;

                byte[] msg = Encoding.GetEncoding("GB18030").GetBytes(Message.PadRight(16, ' '));
                if (msg.Length > 16)
                    Array.Resize(ref msg, 16);

                Apdu apdu = new Apdu();
                apdu.lengthExpected = 2;
                apdu.data = new byte[5 + msg.Length];

                apdu.data[0] = 0xFF;
                apdu.data[1] = 0x00;
                apdu.data[2] = 0x69;
                apdu.data[3] = (byte)((LineNumber - 1) * lineOffset);
                apdu.data[4] = (byte)msg.Length;

                Array.Copy(msg, 0, apdu.data, 5, msg.Length);

                if (boldFont)
                    apdu.data[1] |= 0x01;

                apdu.data[1] |= fontSet;

                lclACS.sendCardControl(ref apdu, operationControlCode);
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while Displaying message.", expn);
            }
            finally
            {
                try
                {
                    lclACS.disconnect();
                }
                catch (Exception expn)
                {
                    log.Error("Error disconnecting", expn);
                }
            }
            log.LogMethodExit();
        }
    }
}
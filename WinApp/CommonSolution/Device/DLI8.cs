/********************************************************************************************
 * Project Name - Device
 * Description  - Contains methods related to reading and writing the data
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.70.2      07-Aug-2019       Deeksha        Added Logger Methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using DLISDK.RFID;

namespace Semnox.Parafait.Device
{
    public class DLI8 : MifareDevice
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string com;
        RFIDDevice rfid;
        public DLI8()
            :base()
        {
            log.LogMethodEntry();
            if (!initialize())
            {
                throw new Exception("Device initialization failed.");
            }            
            startListener();
            log.LogMethodExit(); 
        }
        public DLI8(byte[] defaultKey)
            : base(new List<byte[]> { defaultKey })
        {
            log.LogMethodEntry("defaultKey");
            initialize();
            log.LogMethodExit();
        }
        private bool initialize()
        {
            log.LogMethodEntry();
            try
            {
                com =  ConfigurationManager.AppSettings["COMPort"];
                if (!string.IsNullOrEmpty(com))
                {
                    rfid = new DLISDK.RFID.Devices.USN3170.USN3170Device(com);
                    rfid.Open();
                    rfid.Close();
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while executing initialize() ", ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        public override string readCardNumber()
        {
            lock (LockObject)
            {
                log.LogMethodEntry();
                string cardNumber = "";
                try
                {
                    rfid.Open();
                    rfid.AntennaOff();
                    rfid.AntennaOn(); //optional command. **If the Request command is used the antenna is turned on automatically**
                    var testCard1 = rfid.Request();
                    if (testCard1 != 0)
                    {
                        //sometimes the card is close enough to the RF field to reply back with a request, but far enough away to have a transmission error.
                        //asking the card again and comparing the results helps eliminate this.
                        var testCard2 = rfid.Request();
                        if (testCard1 == testCard2)
                        {
                            var card = rfid.Select();
                            cardNumber = BitConverter.ToString(card.UID);
                            cardNumber = cardNumber.Replace("-", "");
                        }
                    }
                }
                catch(Exception ex)
                {
                    log.Error("Error occurred while executing readCardNumber() ", ex);
                    log.LogMethodExit();
                    return "";
                }
                finally
                {
                    if (rfid != null)
                    {
                        rfid.AntennaOff();
                        rfid.Close();
                    }
                }
                log.LogMethodExit("cardNumber");
                return cardNumber;
            }
        }
        public override bool read_data(int blockAddress, int numberOfBlocks, byte[] paramAuthKey, ref byte[] paramReceivedData, ref string message)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(blockAddress, numberOfBlocks, "paramAuthKey", paramReceivedData, message);
                try
                {
                    rfid.Open();
                    var testCard1 = rfid.Request();
                    if (testCard1 != 0)
                    {
                        //sometimes the card is close enough to the RF field to reply back with a request, but far enough away to have a transmission error.
                        //asking the card again and comparing the results helps eliminate this.
                        var testCard2 = rfid.Request();
                        if (testCard1 == testCard2)
                        {
                            var card = rfid.Select();
                            rfid.AuthenticateMifareClassic(0, blockAddress, paramAuthKey);
                            paramReceivedData = rfid.ReadMifareClassic(blockAddress);
                        }
                    }
                    log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    log.Error(message);
                    log.LogMethodExit(false);
                    return false;
                }
                finally
                {
                    if (rfid != null)
                    {
                        rfid.AntennaOff();
                        rfid.Close();
                    }
                }
            }
        }
        public override bool write_data(int blockAddress, int numberOfBlocks, byte[] authKey, byte[] writeData, ref string message)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(blockAddress, numberOfBlocks, "authKey", writeData, message);
                try
                {
                    rfid.Open();
                    var testCard1 = rfid.Request();
                    if (testCard1 != 0)
                    {
                        //sometimes the card is close enough to the RF field to reply back with a request, but far enough away to have a transmission error.
                        //asking the card again and comparing the results helps eliminate this.
                        var testCard2 = rfid.Request();
                        if (testCard1 == testCard2)
                        {
                            var card = rfid.Select();
                            rfid.AuthenticateMifareClassic(0, blockAddress, authKey);
                            for (int i = 0; i < 16; i++)
                            {
                                writeData[i] = 0x01;
                            }
                            rfid.WriteToMifareClassic(blockAddress, writeData);
                        }
                    }
                    log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    log.Error(message);
                    log.LogMethodExit(false);
                    return false;
                }
                finally
                {
                    if (rfid != null)
                    {
                        rfid.AntennaOff();
                        rfid.Close();
                    }
                }
            }
        }
        public override bool change_authentication_key(int blockAddress, byte[] currentAuthKey, byte[] newAuthKey, ref string message)
        {
            lock (LockObject)
            {
                log.LogMethodEntry(blockAddress, "currentAuthKey", "newAuthKey", message);
                try
                {

                    byte[] dataBuffer = new byte[0];
                    if (!read_data(blockAddress, 1, currentAuthKey, ref dataBuffer, ref message))
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                    rfid.Open();
                    var test1 = rfid.Request();
                    if (test1 == 0)
                    {
                        throw new Exception("Card read failed.");
                    }
                    //sometimes the card is close enough to the RF field to reply back with a request, but far enough away to have a transmission error.
                    //asking the card again and comparing the results helps eliminate this.
                    var test2 = rfid.Request();

                    if (test1 == test2)
                    {

                        var card = rfid.Select();
                        rfid.AuthenticateMifareClassic(0, blockAddress, currentAuthKey);
                        for (int i = 0; i < 6; i++)
                            dataBuffer[i] = newAuthKey[i];

                        rfid.WriteToMifareClassic(blockAddress, dataBuffer);
                        log.LogMethodExit(true);
                        return true;
                    }
                    log.LogMethodExit(false);
                    return false;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    log.Error(message); 
                    log.LogMethodExit(false);
                    return false;
                }
                finally
                {
                    if (rfid != null)
                    {
                        rfid.AntennaOff();
                        rfid.Close();
                    }
                }
            }
        }
        public override void beep(int repeat, bool asynchronous)
        {
            log.LogMethodEntry(repeat, asynchronous);
            try
            {
                Console.Beep(1000, 100);
                //rfid.Buzz(repeat, new TimeSpan(0, 0, 1));
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while executing beep() ", ex);
            }
        }

        public override void Authenticate(byte blockNumber, byte[] key)
        {
            log.LogMethodEntry(blockNumber,"key");
            try
            {
                rfid.Open();
                var test1 = rfid.Request();
                if (test1 == 0)
                {
                    throw new Exception("Card read failed.");
                }
                //sometimes the card is close enough to the RF field to reply back with a request, but far enough away to have a transmission error.
                //asking the card again and comparing the results helps eliminate this.
                var test2 = rfid.Request();

                if (test1 == test2)
                {
                    var card = rfid.Select();
                    rfid.AuthenticateMifareClassic(0, blockNumber, key);
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while executing Authenticate() ", ex);
                log.LogMethodExit(null,"Throwing Exception" +ex.Message);
                throw;
            }
            finally
            {
                if (rfid != null)
                {
                    rfid.AntennaOff();
                    rfid.Close();
                }
            }                
        }
    }
}

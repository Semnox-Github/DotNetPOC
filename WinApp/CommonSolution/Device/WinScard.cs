/********************************************************************************************
 * Project Name - Device
 * Description  - WinScard
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.70.2      08-Aug-2019       Deeksha        Added Logger Methods.
 ********************************************************************************************/
using System.Collections.Generic;


namespace Semnox.Parafait.Device
{
    public class WinScard : MifareDevice
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WinScardAPI rfid;
        public WinScard()
            : base()
        {
            log.LogMethodEntry();
            rfid = new WinScardAPI();
            log.LogMethodExit();
        }

        public WinScard(byte[] defaultKey)
            : base(new List<byte[]> { defaultKey })
        {
            log.LogMethodEntry("defaultKey");
            rfid = new WinScardAPI();
            log.LogMethodExit();
        }

        public override string readCardNumber()
        {
            lock (LockObject)
            {
                log.LogMethodEntry();
                try
                {
                    rfid.Open();
                    rfid.Connect();
                    string returnValue = rfid.Select();
                    log.LogMethodExit(returnValue);
                    return returnValue;
                }
                finally
                {
                    if (rfid != null)
                    {
                        rfid.Close();
                    }
                }
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
                    rfid.Connect();
                    rfid.LoadKey(paramAuthKey);
                    rfid.Authenticate(blockAddress);
                    paramReceivedData = rfid.ReadBlock(blockAddress);
                    log.LogMethodExit(true);
                    return true;
                }
                finally
                {
                    if (rfid != null)
                    {
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
                    rfid.Connect();
                    rfid.LoadKey(authKey);
                    rfid.Authenticate(blockAddress);
                    rfid.WriteBlock(blockAddress, writeData);
                    log.LogMethodExit(true);
                    return true;
                }
                finally
                {
                    if (rfid != null)
                    {
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
                byte[] dataBuffer = new byte[0];
                if (!read_data(blockAddress, 1, currentAuthKey, ref dataBuffer, ref message))
                {
                    log.LogMethodExit(false);
                    return false;
                }
                try
                {
                    rfid.Open();
                    rfid.Connect();
                    rfid.LoadKey(currentAuthKey);
                    rfid.Authenticate(blockAddress);
                    for (int i = 0; i < 6; i++)
                        dataBuffer[i] = newAuthKey[i];

                    rfid.WriteBlock(blockAddress, dataBuffer);
                    log.LogMethodExit(true);
                    return true;
                }
                finally
                {
                    if (rfid != null)
                    {
                        rfid.Close();
                    }
                }
            }
        }

        public override void Authenticate(byte blockNumber, byte[] key)
        {
            log.LogMethodEntry(blockNumber, "key");
            try
            {
                rfid.Open();
                rfid.Connect();
                rfid.LoadKey(key);
                rfid.Authenticate(blockNumber);
            }
            finally
            {
                if (rfid != null)
                {
                    rfid.Close();
                }
            }
            log.LogMethodExit();
        }
    }
}

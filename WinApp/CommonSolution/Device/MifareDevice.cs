/********************************************************************************************
 * Project Name - Device
 * Description  - Contains Methods and Properties related to Mifare device's functionality
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 * 1.01         1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.70        30-Jul-2019      Mathew Ninan        Added disconnect of ACS in listener method
 *                                                  once listener has completed card operations
 *2.70.2       08-Aug-2019      Deeksha             Modified to add logger methods. 
 *2.80.7       21-Feb-2022      Guru S A            ACR reader performance fix 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device
{
    public class MifareDevice : DeviceClass
    {
        protected List<byte[]> authKeyArray;
        protected List<byte[]> FinalAuthKeys = new List<byte[]>();
        protected byte[] customerAuthKey = new byte[6];
        protected byte[] defaultAuthKey = new byte[0];
        protected int SiteId = 0;
        protected const int PURSE_BLOCK = 4;
        protected bool readerIsForRechargeOnly = false;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool s70Card;

        public override void Dispose()
        {
            log.LogMethodEntry();
            stopListener();
            base.Dispose();
            log.LogMethodExit();
        }

        ~MifareDevice()
        {
            log.LogMethodEntry();
            Dispose();
            log.LogMethodExit();
        }

        public MifareDevice()
        {
            log.LogMethodEntry();
            authKeyArray = getAuthKeyList();

            if (!customerAuthKey[0].Equals(0))
                FinalAuthKeys.Add(customerAuthKey);

            FinalAuthKeys.AddRange(authKeyArray);
            log.LogMethodExit();
        }

        public MifareDevice(DeviceDefinition deviceDefinition)
            :base(deviceDefinition)
        {
            log.LogMethodEntry();
            authKeyArray = new List<byte[]>();
            foreach (var mifareKeyContainerDTO in deviceDefinition.MifareKeyContainerDTOList)
            {
                if (mifareKeyContainerDTO.Type == MifareKeyContainerDTO.MifareKeyType.CLASSIC.ToString())
                {
                    ByteArray byteArray = new ByteArray(mifareKeyContainerDTO.KeyString);
                    authKeyArray.Add(byteArray.Value);
                    FinalAuthKeys.Add(byteArray.Value);
                    if (mifareKeyContainerDTO.IsCurrent)
                    {
                        defaultAuthKey = customerAuthKey = byteArray.Value;
                    }
                }
            }
            SiteId = deviceDefinition.SiteId;
            this.readerIsForRechargeOnly = deviceDefinition.ReaderIsForRechargeOnly;
            log.LogMethodExit();
        }

        public MifareDevice(List<byte[]> defaultKey)
        {
            log.LogMethodEntry("defaultKey");
            authKeyArray = new List<byte[]>();
            authKeyArray.AddRange(defaultKey);

            if (defaultKey.Count > 0)
                defaultAuthKey = customerAuthKey = defaultKey[0];

            FinalAuthKeys.AddRange(authKeyArray);
            log.LogMethodExit();
        }

        public MifareDevice(bool readerIsForRechargeOnly)
        {
            log.LogMethodEntry(readerIsForRechargeOnly);
            this.readerIsForRechargeOnly = readerIsForRechargeOnly;
            authKeyArray = getAuthKeyList();

            if (!customerAuthKey[0].Equals(0))
                FinalAuthKeys.Add(customerAuthKey);

            FinalAuthKeys.AddRange(authKeyArray);
            log.LogMethodExit();
        }

        public override void Register(EventHandler currEventHandler)
        {
            log.LogMethodEntry();
            base.Register(currEventHandler);
            beep();
            log.LogMethodExit();
        }

        List<byte[]> getAuthKeyList()
        {
            log.LogMethodEntry();
            string strAuthKeys = Encryption.GetParafaitKeys("MifareAuthorization");
            DBUtils utils = new DBUtils();
            object AuthKeys = utils.executeScalar("select AuthKey from ProductKey");

            if (AuthKeys != null && AuthKeys != DBNull.Value)
                strAuthKeys += "|" + System.Text.Encoding.Default.GetString(AuthKeys as byte[]);

            string[] stringArray = strAuthKeys.Split('|');
            List<byte[]> authKeyArray = new List<byte[]>();

            foreach (string str in stringArray)
            {
                try
                {
                    string ke = Encryption.Decrypt(str);
                    if (ke.Length > 17)
                    {
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                        DateTime expiryDate = DateTime.ParseExact(ke.Substring(17), "dd-MMM-yyyy", provider);
                        if (expiryDate < ServerDateTime.Now.Date)
                            continue;
                    }

                    if (ke.Length > 0)
                    {
                        byte[] authKey = new byte[6];
                        string[] sa = ke.Substring(0, 17).Split('-');
                        int i = 0;
                        foreach (string s in sa)
                        {
                            authKey[i++] = Convert.ToByte(s, 16);
                        }

                        authKeyArray.Add(authKey);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred  while executing getAuthKeyList", ex);
                }
            }

            ParafaitEnv env = new ParafaitEnv(utils);

            int customerKey = env.MifareCustomerKey;
            if (customerKey <= 0)
            {
                MessageBox.Show(new MessageUtils(utils, env).getMessage(291, customerKey));
            }

            string key = Encryption.GetParafaitKeys("NonMifareAuthorization");
            for (int i = 0; i < 5; i++)
                customerAuthKey[i] = Convert.ToByte(key[i]);
            customerAuthKey[5] = Convert.ToByte(customerKey);

            if (env.getParafaitDefaults("MIFARE_CARD").Equals("Y"))
                defaultAuthKey = authKeyArray[0];
            else
                defaultAuthKey = customerAuthKey;

            SiteId = Convert.ToInt32(utils.executeScalar("select site_id from site"));
            log.LogMethodExit("authKeyArray");
            return authKeyArray;
        }

        /* Read-only reader */
        internal Thread CardListenerThread;
        internal bool RunThread = true;
        internal object LockObject = new object();
        public override void stopListener()
        {
            log.LogMethodEntry();
            if (CardListenerThread != null)
            {
                RunThread = false;

                try
                {
                    if (!CardListenerThread.Join(2000))
                        CardListenerThread.Abort();
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred  while executing StopListener()", ex);
                }
            }
            log.LogMethodExit();
        }

        public override void startListener()
        {
            log.LogMethodEntry();

            if (RunThread == true && CardListenerThread != null && CardListenerThread.IsAlive)
            {
                log.LogMethodExit("Listener thread already running");
                return;
            }

            stopListener();

            synContext = SynchronizationContext.Current;

            RunThread = true;
            CardListenerThread = new Thread(new ThreadStart(Listener));
            CardListenerThread.IsBackground = true;
            CardListenerThread.Start();
            log.LogMethodExit();
        }

        protected virtual void Listener()
        {
            log.LogMethodEntry();
            bool response = false;
            string prevCardNumber = "";

            while (RunThread == true)
            {
                lock (LockObject)
                {
                    string cardNumber = readCardNumber();
                    if (cardNumber != "")
                    {
                        log.LogVariableState("CardNumber, PrevCardNumber: ", cardNumber + ',' + prevCardNumber);
                        if (cardNumber != prevCardNumber)
                        {
                            response = Validate();

                            if (response)
                            {
                                prevCardNumber = cardNumber;
                                log.LogVariableState("Listener Card Number: ", cardNumber);
                                FireDeviceReadCompleteEvent(cardNumber);
                                beep();
                            }
                            else
                            {
                                if (DeviceNonreadableHandler != null)
                                {
                                    prevCardNumber = cardNumber;
                                    log.LogVariableState("Listener Card Number: ", cardNumber);
                                    FireDeviceReadUnAuthenticateEvent(cardNumber);
                                    beep();
                                }
                                else
                                {
                                    beep(2, false);
                                }
                            }
                        }
                        try
                        {
                            log.LogVariableState("Disconnect", "Disconnecting Card Reader in Listener");
                            disconnect();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred  while executing disconnect()", ex);
                        }
                    }
                    else
                    {
                        prevCardNumber = cardNumber = "";
                    }

                    Thread.Sleep(100);
                }
            }
            log.LogMethodExit();
        }

        public virtual bool Validate()
        {
            log.LogMethodEntry();
            bool response = false;
            if (readerIsForRechargeOnly)
            {
               log.LogMethodExit("return true as reader is for recharge only, skipping validation");
                return true;
            }

            foreach (byte[] key in FinalAuthKeys)
            {
                try
                {
                    Authenticate(PURSE_BLOCK, key);
                    response = true;
                    log.LogVariableState("Response True for key: ", response);
                }
                catch (Exception ex)
                {
                    response = false;
                    log.Error("Error occurred while executing Validate()" + ex.Message);
                    log.LogVariableState("Response false for key: ", response);
                }

                if (response)
                {
                    if (bytesEqual(key, defaultAuthKey, 6) == false // if other than default key was allowed, and key is not customer authkey then change key to default key
                        && bytesEqual(key, customerAuthKey, 6) == false)
                    {
                        string message = "";
                        response = change_authentication_key(PURSE_BLOCK + 3, key, defaultAuthKey, ref message);
                        if (!response)
                        {
                            log.LogMethodExit(false);
                            return false;
                        }
                        else
                            beep();
                        defaultKeyChanged = true;
                        isRoamingCard = false;
                        TagSiteId = SiteId;
                        byte[] siteIdBuffer = new byte[16];
                        siteIdBuffer[0] = (byte)(SiteId >> 0);
                        siteIdBuffer[1] = (byte)(SiteId >> 8);
                        siteIdBuffer[2] = (byte)(SiteId >> 16);
                        siteIdBuffer[3] = (byte)(SiteId >> 24);

                        if (!write_data(6, 1, defaultAuthKey, siteIdBuffer, ref message))
                        {
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    else
                    {
                        defaultKeyChanged = false;
                        string message = "";
                        byte[] siteIdBuffer = new byte[16];
                        if (!read_data(6, 1, key, ref siteIdBuffer, ref message))
                        {
                            log.LogMethodExit(false);
                            return false;
                        }
                        int tagSiteId = BitConverter.ToInt32(siteIdBuffer, 0);
                        TagSiteId = tagSiteId;
                        if (tagSiteId == SiteId)
                            isRoamingCard = false;
                        else
                            isRoamingCard = true;
                            
                    }
                    log.LogMethodExit(true);
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;
        } 
        public override string readValidatedCardNumber()
        {
            log.LogMethodEntry();
            string cardNumber = readCardNumber();
            if (!string.IsNullOrEmpty(cardNumber))
            {
                if (Validate())
                {
                    log.LogMethodExit(cardNumber);
                    return cardNumber;
                }
            }
            log.LogMethodExit(string.Empty);
            return string.Empty;
        }

        protected bool bytesEqual(byte[] byte1, byte[] byte2, int len)
        {
            log.LogMethodEntry(byte1, byte2, len);
            if (byte1.Length < len)
                len = byte1.Length;

            if (byte1.Length != byte2.Length)
            {
                log.LogMethodExit(false);
                return false;
            }

            for (int i = 0; i < byte1.Length; i++)
                if (byte1[i] != byte2[i])
                {
                    log.LogMethodExit(false);
                    return false;
                }
            log.LogMethodExit(true);
            return true;
        }

        public override CardType CardType
        {
            get
            {
                return CardType.MIFARE;
            }
        }
    }
}

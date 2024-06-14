/********************************************************************************************
 * Project Name - MifareCard
 * Description  - Mifare Card class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using POSCore;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device;
//using Semnox.Parafait.EncryptionUtils;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Transaction
{
    public class MifareCard: Card
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TagNumberParser tagNumberParser;
        protected byte[] authKey;
        protected byte[] basicAuthKey = new byte[6];
        protected byte[] basicUlcKey = new byte[16];
        internal int customerKey;

        private const int SITEIDPOS = 10;
        private const int SITEIDSIZE = 1;
        private const int GAMEPLAYPOS = 13;
        private const int GAMEPLAYSIZE = 2;

        private const int PURSE_BLOCK = 4;
        private const int PURSE_BLOCK_SIZE = 2;
        private const int PURSE_AUTH_BLOCK = 7;
        private const int ENTITLEMENT_BLOCK = 8;
        private const int ENTITLEMENT_SECTORS = 2;
        private const int ENT_BLOCK_SIZE = 3;
        private const int TOTAL_BUFFER_SIZE = 128;
        MifareDataClass mifareData;

        private const string READY = "Ready to Upload";
        private const string DUPLICATE_IN_SYSTEM = "Duplicate in System";
        const int DATA_START_BLOCK = 8;
        const int PLAYS_SAVED_COUNT_POS = 3;
        const int MAXBLOCKS_MIFARE_S50 = 63;
        const string INVALID_DATA = "Invalid Value";
        const string DUPLICATE_IN_CARD = "Duplicate in Master card";
        private const string NO_DATA_IN_SYSTEM = "Machine Id/ Site code does not exist";
        
        private int[] MIFARE_S50_AUTHBLOCKS = { 7, 11, 15, 19, 23, 27, 31, 35, 39, 43, 47, 51, 55, 59, 63 };
        private int[] mifareS50DataBlocks = {	8, 9, 10, 12, 13, 14, 16, 17, 18, 20, 21, 22, 24, 25, 26, 28, 29, 30, 32, 33, 34,
                                                36, 37, 38, 40, 41, 42, 44, 45, 46, 48, 49, 50, 52, 53, 54, 56, 57, 58, 60, 61, 62};
        private const byte HexKey = 0x4C;
        private readonly UlcKeyStore ulcKeyStore;
        public class MifareDataClass
        {
            string _CardNumber;
            byte[] _key;
            public class Entitlement
            {
                public Byte EntType_IdType;
                public Byte Id;
                public UInt16 EntCount;
                public UInt32 ExpiryTime;
                public int BlockNumber;
            }

            public struct MiFareStructure
            {
                public UInt32 Credits;
                public UInt32 Bonus;
                public UInt32 Courtesy;
                public UInt32 CreditPlusCredits;
                public Byte Version;
                public Byte VIP;
                public Byte DiscountAllowed;
                public Byte HexKey;
                public UInt32 CardDeposit;
                public UInt32 DiscountPercentage;
                public UInt32 Future;
                public List<Entitlement> Entitlements;
                public bool EntitlementChanged;
            }

            byte[] getKey()
            {
                log.LogMethodEntry();

                string encryptionKey = Encryption.GetParafaitKeys("MifareCard");

                byte[] key = Encoding.UTF8.GetBytes(encryptionKey);
                key[16] = Convert.ToByte(Convert.ToInt32(_CardNumber[0].ToString() + _CardNumber[1].ToString(), 16));
                key[17] = Convert.ToByte(Convert.ToInt32(_CardNumber[2].ToString() + _CardNumber[3].ToString(), 16));
                key[18] = Convert.ToByte(Convert.ToInt32(_CardNumber[4].ToString() + _CardNumber[5].ToString(), 16));
                key[19] = Convert.ToByte(Convert.ToInt32(_CardNumber[6].ToString() + _CardNumber[7].ToString(), 16));

                log.LogMethodExit(key);
                return key;
            }

            public byte[] getBytes()
            {
                log.LogMethodEntry();

                byte[] arr = new byte[TOTAL_BUFFER_SIZE];

                byte[] tempArr = new byte[16];

                Array.Copy(BitConverter.GetBytes(Structure.Credits), 0, arr, 0, 4);
                Array.Copy(BitConverter.GetBytes(Structure.Bonus), 0, arr, 4, 4);
                Array.Copy(BitConverter.GetBytes(Structure.Courtesy), 0, arr, 8, 4);
                Array.Copy(BitConverter.GetBytes(Structure.CreditPlusCredits), 0, arr, 12, 4);
                arr[16] = Structure.Version;
                arr[17] = Structure.VIP;
                arr[18] = Structure.DiscountAllowed;
                arr[19] = Structure.HexKey;
                Array.Copy(BitConverter.GetBytes(Structure.CardDeposit), 0, arr, 20, 4);
                Array.Copy(BitConverter.GetBytes(Structure.DiscountPercentage), 0, arr, 24, 4);
                Array.Copy(BitConverter.GetBytes(Structure.Future), 0, arr, 28, 4);

                Array.Copy(arr, tempArr, 16);
                tempArr = EncryptionAES.Encrypt(tempArr, _key);
                Array.Copy(tempArr, arr, 16);

                Array.Copy(arr, 16, tempArr, 0, 16);
                tempArr = EncryptionAES.Encrypt(tempArr, _key);
                Array.Copy(tempArr, 0, arr, 16, 16);

                int i = 0;
                int offset = 8;
                byte[] entArray = new byte[ENTITLEMENT_SECTORS * ENT_BLOCK_SIZE * 16];
                foreach (Entitlement ent in Structure.Entitlements)
                {
                    offset = 8 * i;
                    Array.Copy(BitConverter.GetBytes(ent.EntType_IdType), 0, entArray, offset, 1);
                    Array.Copy(BitConverter.GetBytes(ent.Id), 0, entArray, offset + 1, 1);
                    Array.Copy(BitConverter.GetBytes(ent.EntCount), 0, entArray, offset + 2, 2);
                    Array.Copy(BitConverter.GetBytes(ent.ExpiryTime), 0, entArray, offset + 4, 4);
                    i++;
                }

                offset = PURSE_BLOCK_SIZE * 16;
                for (int j = 0; j < ENT_BLOCK_SIZE * ENTITLEMENT_SECTORS; j++)
                {
                    Array.Copy(entArray, j * 16, tempArr, 0, 16);
                    tempArr = EncryptionAES.Encrypt(tempArr, _key);
                    Array.Copy(tempArr, 0, arr, offset + j * 16, 16);
                }

                log.LogMethodExit(arr);
                return arr;
            }

            public MiFareStructure fromBytes(byte[] arr)
            {
                log.LogMethodEntry(arr);

                byte[] tempArr = new byte[16];

                Array.Copy(arr, tempArr, 16);
                tempArr = EncryptionAES.Decrypt(tempArr, _key);
                Array.Copy(tempArr, arr, 16);

                Array.Copy(arr, 16, tempArr, 0, 16);
                tempArr = EncryptionAES.Decrypt(tempArr, _key);
                Array.Copy(tempArr, 0, arr, 16, 16);

                byte[] value = new byte[4];
                Array.Copy(arr, 0, value, 0, 4);
                Structure.Credits = BitConverter.ToUInt32(value, 0);

                Array.Copy(arr, 4, value, 0, 4);
                Structure.Bonus = BitConverter.ToUInt32(value, 0);

                Array.Copy(arr, 8, value, 0, 4);
                Structure.Courtesy = BitConverter.ToUInt32(value, 0);

                Array.Copy(arr, 12, value, 0, 4);
                Structure.CreditPlusCredits = BitConverter.ToUInt32(value, 0);

                Structure.Version = arr[16];
                Structure.VIP = arr[17];
                Structure.DiscountAllowed = arr[18];
                Structure.HexKey = arr[19];

                Array.Copy(arr, 20, value, 0, 4);
                Structure.CardDeposit = BitConverter.ToUInt32(value, 0);

                Array.Copy(arr, 24, value, 0, 4);
                Structure.DiscountPercentage = BitConverter.ToUInt32(value, 0);

                Array.Copy(arr, 28, value, 0, 4);
                Structure.Future = BitConverter.ToUInt32(value, 0);

                log.LogMethodExit(Structure);
                return Structure;
            }

            // each block has 2 entitlement records of 8 bytes each
            /*
               Data on the card
               Sector 2 and Sector 3 is being used to store the entitlement. Each entitlement is of 8 bytes. 
               So totally 6 entitlements can be stored in a sector and in our case as there are 2 sectors in use, total of 12 entitlements can be stored.
 
               The format will be
               Byte 0 - Type - Top nibble will indicate the type of entitlement. In case of game entitlement we will store the top nibble as 1. 
                               The bottom nibble will indicate what is the id. It will be 0 if game id, 1 if game profile id and 2 if generic.
               Byte 1 - Id. This would be game id or game profile id or will be 255 to indicate generic (255 generic is redundant as byte 0 bottom nibble would have indicated already, but we'll keep it just as precaution)
               Byte 2 and 3 - Count - Number of games in this case
               Byte 4 - 7 - Future use to store the time
 
               So, in case it's an entitlement of 20 games for the game id 37, the data would be
               0x10, 0x25, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00 
            */
            public void getEntitlements(byte[] sector)
            {
                log.LogMethodExit(sector);

                byte[] tempArr = new byte[16];
                byte[] value = new byte[4];

                for (int i = 0; i < ENT_BLOCK_SIZE; i++)
                {
                    Array.Copy(sector, i * 16, tempArr, 0, 16);
                    tempArr = EncryptionAES.Decrypt(tempArr, _key);

                    for (int j = 0; j < 2; j++)
                    {
                        int offset = 8 * j;
                        Entitlement ent = new Entitlement();
                        ent.EntType_IdType = tempArr[offset];
                        ent.Id = tempArr[offset + 1];
                        Array.Copy(tempArr, offset + 2, value, 0, 2);
                        ent.EntCount = BitConverter.ToUInt16(value, 0);
                        Array.Copy(tempArr, offset + 4, value, 0, 4);
                        ent.ExpiryTime = BitConverter.ToUInt32(value, 0);
                        if (ent.EntCount > 0)
                        {
                            foreach (Entitlement blankEnt in Structure.Entitlements)
                            {
                                if (blankEnt.EntType_IdType == 0)
                                {
                                    blankEnt.EntType_IdType = ent.EntType_IdType;
                                    blankEnt.Id = ent.Id;
                                    blankEnt.EntCount = ent.EntCount;
                                    blankEnt.ExpiryTime = ent.ExpiryTime;

                                    break;
                                }
                            }
                        }
                    }
                }

                log.LogMethodExit(null);
            }

            public void RationalizeEntitlements()
            {
                log.LogMethodEntry();

                // if all entitlements are 0, and if there are -1 entitlements, make them also 0
                bool nonZeroEntFound = false;
                foreach (Entitlement ent in Structure.Entitlements)
                {
                    if (ent.EntCount > 0 && ent.EntCount != 0xffff)
                    {
                        nonZeroEntFound = true;
                        break;
                    }
                }

                if (!nonZeroEntFound)
                {
                    foreach (Entitlement ent in Structure.Entitlements)
                    {
                        if (ent.EntCount != 0)
                        {
                            ent.EntCount = 0;
                            Structure.EntitlementChanged = true;
                        }
                    }
                }

                log.LogMethodExit(null);
            }

            public MiFareStructure Structure;
            private MiFareStructure createBlankStructure()
            {
                log.LogMethodEntry();

                Structure = new MiFareStructure();
                Structure.Bonus =
                Structure.CardDeposit =
                Structure.Courtesy =
                Structure.CreditPlusCredits =
                Structure.Credits =
                Structure.DiscountAllowed = 0;
                Structure.DiscountPercentage = 0;
                Structure.Future =
                Structure.VIP = 0;
                Structure.Version = 1;
                Structure.HexKey = HexKey;
                Structure.Entitlements = new List<Entitlement>();
                Structure.EntitlementChanged = false;

                for (int i = 0; i < ENTITLEMENT_SECTORS * ENT_BLOCK_SIZE * 2; i++)
                {
                    Entitlement ent = new Entitlement();
                    ent.BlockNumber = i / 2;
                    Structure.Entitlements.Add(ent);
                }

                log.LogMethodExit(Structure);
                return Structure;
            }

            public MifareDataClass(string CardNumber)
            {
                log.LogMethodEntry(CardNumber);

                _CardNumber = CardNumber;
                _key = getKey();
                createBlankStructure();

                log.LogMethodExit(null);
            }

            public void SetValues(double Credits, double Bonus, double Courtesy, double CreditPlusCredits, double CardDeposit, char VIP, char DiscountAllowed, double DiscountPercentage, List<Card.Entitlement> EntList)
            {
                log.LogMethodEntry(Credits, Bonus, Courtesy, CreditPlusCredits, CardDeposit, VIP, DiscountAllowed, DiscountPercentage, EntList);

                Structure.Credits = Credits < 0 ? 0 : Convert.ToUInt32(Credits * 100);
                Structure.Bonus = Bonus < 0 ? 0 : Convert.ToUInt32(Bonus * 100);
                Structure.Courtesy = Courtesy < 0 ? 0 : Convert.ToUInt32(Courtesy * 100);
                Structure.CreditPlusCredits = CreditPlusCredits < 0 ? 0 : Convert.ToUInt32(CreditPlusCredits * 100);
                Structure.CardDeposit = Convert.ToUInt32(CardDeposit * 100);
                if (VIP == 'Y')
                    Structure.VIP = 1;
                if (DiscountAllowed == 'Y')
                    Structure.DiscountAllowed = 1;
                Structure.DiscountPercentage = Convert.ToUInt32(DiscountPercentage * 100);

                Structure.Version = 1;
                Structure.HexKey = HexKey;
                Structure.Future = 0;

                foreach (Card.Entitlement ent in EntList)
                {
                    Entitlement entRaw = new Entitlement();
                    entRaw.EntType_IdType = (byte)((ent.EntType << 4) | ent.IdType);
                    entRaw.Id = ent.UserIdentifier;
                    entRaw.EntCount = ent.EntCount;
                    bool found = false;
                    foreach (Entitlement cardEnt in Structure.Entitlements)
                    {
                        if (cardEnt.EntType_IdType == entRaw.EntType_IdType
                            && cardEnt.Id == entRaw.Id)
                        {
                            if (entRaw.EntCount == 0xffff) 
                            {
                                if (cardEnt.EntCount == 0xffff) // -1 count already exists for this ent. ignore
                                {
                                    found = true;
                                }
                            }
                            else
                            {
                                if (cardEnt.EntCount != 0xffff) // regular count exists for this ent. add.
                                {
                                    cardEnt.EntCount = (ushort)(cardEnt.EntCount + entRaw.EntCount);
                                    Structure.EntitlementChanged = true;
                                    found = true;
                                }
                            }
                            
                            if (found)
                                break;
                        }
                    }

                    if (!found)
                    {
                        foreach (Entitlement cardEnt in Structure.Entitlements)
                        {
                            if (cardEnt.EntType_IdType == 0) // find the first available slot in the structure
                            {
                                cardEnt.EntType_IdType = entRaw.EntType_IdType;
                                cardEnt.Id = entRaw.Id;
                                cardEnt.EntCount = entRaw.EntCount;
                                Structure.EntitlementChanged = true;
                                found = true;
                                break;
                            }
                        }
                    }
                }

                log.LogMethodExit(null);
            }
        }

        public MifareCard(DeviceClass readerDevice, string cardNumber, string ploginId, Utilities ParafaitUtilities)
            : base(ParafaitUtilities)
        {
            log.LogMethodEntry(readerDevice, cardNumber, ploginId, ParafaitUtilities);
            ulcKeyStore = new UlcKeyStore();
            tagNumberParser = new TagNumberParser(ParafaitUtilities.ExecutionContext);
            CardNumber = cardNumber;
            loginId = ploginId;
            ReaderDevice = readerDevice;

            isMifare = true;

            mifareData = new MifareDataClass(CardNumber);
            formAuthKeys();

            getCardDetails(cardNumber);

            log.LogMethodExit(null);
        }

        private void formAuthKeys()
        {
            log.LogMethodEntry();

            customerKey = Utilities.ParafaitEnv.MifareCustomerKey;
            authKey = new byte[6];

            string key = Encryption.Decrypt(Encryption.GetParafaitKeys("BasicAuthorization"));
            Encoding.ASCII.GetBytes(key, 0, 6, basicAuthKey, 0);

            try
            {
                if (ulcKeyStore.DefaultUltralightCKeys.Count > 0)
                {
                    basicUlcKey = ulcKeyStore.DefaultUltralightCKeys.LastOrDefault().Value;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while fething the basic ulc key", ex);
            }
            

            if (Utilities.ParafaitEnv.MIFARE_CARD == false) // mifare read only mode
            {
                key = Encryption.Decrypt(Encryption.GetParafaitKeys("MifareAuthorization"));
                string[] sa = key.Substring(0, 17).Split('-');
                int i = 0;
                foreach (string s in sa)
                {
                    authKey[i++] = Convert.ToByte(s, 16);
                }
            }
            else
            {
                key = Encryption.GetParafaitKeys("NonMifareAuthorization");
                for (int i = 0; i < 5; i++)
                    authKey[i] = Convert.ToByte(key[i]);
                authKey[5] = Convert.ToByte(customerKey);
            }

            log.LogMethodExit(null);
        }

        public override void createCard(System.Data.SqlClient.SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);

            bool response;
            string message = "";

            TagNumber tagNumber;
            if (tagNumberParser.TryParse(ReaderDevice.readCardNumber(), out tagNumber) == false)
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Place the original card for new issue");
                throw new ApplicationException("Place the original card for new issue");
            }
            if (tagNumber.Value != CardNumber)
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Place the original card for new issue");
                throw new ApplicationException("Place the original card for new issue");
            }

            base.createCard(SQLTrx);

            mifareData.SetValues(addCredits, addBonus, addCourtesy, addCreditPlusCardBalance, face_value, vip_customer, 'N', 0, base.Entitlements);

            byte[] buffer = mifareData.getBytes();
            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.change_authentication_key(PURSE_AUTH_BLOCK, basicUlcKey, ulcKeyStore.LatestCustomerUlcKey.Value, ref message);
            }
            else
            {
                response = ReaderDevice.change_authentication_key(PURSE_AUTH_BLOCK, basicAuthKey, authKey, ref message);
            }
            
            if (!response)
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Authenticating card failed. Failed on block " + PURSE_BLOCK);
                throw new ApplicationException("Authenticating card failed. Failed on block " + PURSE_BLOCK);
            }

            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.write_data(PURSE_BLOCK, PURSE_BLOCK_SIZE, ulcKeyStore.LatestCustomerUlcKey.Value, buffer, ref message);
            }
            else
            {
                response = ReaderDevice.write_data(PURSE_BLOCK, PURSE_BLOCK_SIZE, authKey, buffer, ref message);    
            }
            
            if (!response)
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Writing data to card failed: " + message);
                throw new ApplicationException("Writing data to card failed: " + message);
            }

            if (base.Entitlements.Count > 0)
            {
                if (!writeEntitlements(buffer, ref message, true))
                {
                    string savMessage = message;
                    if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                    {
                        response = ReaderDevice.change_authentication_key(PURSE_AUTH_BLOCK, ulcKeyStore.LatestCustomerUlcKey.Value, basicUlcKey, ref message);
                    }
                    else
                    {
                        response = ReaderDevice.change_authentication_key(PURSE_AUTH_BLOCK, authKey, basicAuthKey, ref message);
                    }
                    if (!response)
                    {
                        log.LogMethodExit(null, "Throwing ApplicationExceptionReset Authkey failed on block " + PURSE_BLOCK);
                        throw new ApplicationException("Reset Authkey failed on block " + PURSE_BLOCK);
                    }

                    log.LogMethodExit(null, "Throwing ApplicationException "+savMessage);
                    throw new ApplicationException(savMessage);
                }
            }
            ReaderDevice.beep(2);

            log.LogMethodExit(null);
        }

        public override void ReverseEntitlements(List<Card.Entitlement> EntList)
        {
            log.LogMethodEntry(EntList);

            foreach (Card.Entitlement ent in EntList)
            {
                MifareDataClass.Entitlement entRaw = new MifareDataClass.Entitlement();
                entRaw.EntType_IdType = (byte)((ent.EntType << 4) | ent.IdType);
                entRaw.Id = ent.UserIdentifier;
                entRaw.EntCount = ent.EntCount;
                bool found = false;
                foreach (MifareDataClass.Entitlement cardEnt in mifareData.Structure.Entitlements)
                {
                    if (cardEnt.EntType_IdType == entRaw.EntType_IdType
                        && cardEnt.Id == entRaw.Id)
                    {
                        if (entRaw.EntCount == 0xffff)
                        {
                            if (cardEnt.EntCount == 0xffff) // -1 count already exists for this ent. ignore
                            {
                                cardEnt.EntCount = 0;
                                mifareData.Structure.EntitlementChanged = true;
                                found = true;
                            }
                        }
                        else
                        {
                            if (cardEnt.EntCount != 0xffff) // regular count exists for this ent. add.
                            {
                                if ((cardEnt.EntCount - entRaw.EntCount) > 0)
                                    cardEnt.EntCount = (ushort)(cardEnt.EntCount - entRaw.EntCount);
                                else
                                    cardEnt.EntCount = 0;
                                mifareData.Structure.EntitlementChanged = true;
                                found = true;
                            }
                        }

                        if (found)
                            break;
                    }
                }
            }

            log.LogMethodExit(null);           
        }

        bool writeEntitlements(byte[] buffer, ref string message, bool createCard)
        {
            log.LogMethodEntry(buffer, message, createCard);

            byte[] entBuffer = new byte[ENT_BLOCK_SIZE * 16];
            bool response;
            byte[] defaultKey = new byte[6];

            for (int i = 0; i < ENTITLEMENT_SECTORS; i++)
            {
                if (createCard)
                {
                    for (int j = i * 6; j < i * 6 + 6; j++)
                    {
                        if (mifareData.Structure.Entitlements[j].EntCount != 0)
                        {
                            mifareData.Structure.EntitlementChanged = true;
                            break;
                        }
                    }
                }

                if (mifareData.Structure.EntitlementChanged)
                {
                    if (createCard == false) // try writing in case of recharge
                    {
                        Array.Copy(buffer, PURSE_BLOCK_SIZE * 16 + i * ENT_BLOCK_SIZE * 16, entBuffer, 0, ENT_BLOCK_SIZE * 16);
                        if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                        {
                            response = ReaderDevice.write_data(ENTITLEMENT_BLOCK + i * 4, ENT_BLOCK_SIZE, ulcKeyStore.LatestCustomerUlcKey.Value, entBuffer, ref message);
                        }
                        else
                        {
                            response = ReaderDevice.write_data(ENTITLEMENT_BLOCK + i * 4, ENT_BLOCK_SIZE, authKey, entBuffer, ref message);
                        }
                    }
                    else
                        response = false;

                    if (!response)
                    {
                        byte[] dummyRead = new byte[16];
                        response = ReaderDevice.read_data_basic_auth(ENTITLEMENT_BLOCK + 3 + i * 4, 1, ref defaultKey, ref dummyRead, ref message);
                        if (response)
                        {
                            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                            {
                                response = !ReaderDevice.change_authentication_key(ENTITLEMENT_BLOCK + 3 + i * 4, defaultKey, ulcKeyStore.LatestCustomerUlcKey.Value, ref message);
                            }
                            else
                            {
                                response = !ReaderDevice.change_authentication_key(ENTITLEMENT_BLOCK + 3 + i * 4, defaultKey, authKey, ref message);
                            }
                            
                        }
                        if (!response) // unable to read with default key or change key successful
                        {
                            Array.Copy(buffer, PURSE_BLOCK_SIZE * 16 + i * ENT_BLOCK_SIZE * 16, entBuffer, 0, ENT_BLOCK_SIZE * 16);
                            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                            {
                                response = ReaderDevice.write_data(ENTITLEMENT_BLOCK + i * 4, ENT_BLOCK_SIZE, ulcKeyStore.LatestCustomerUlcKey.Value, entBuffer, ref message);
                            }
                            else
                            {
                                response = ReaderDevice.write_data(ENTITLEMENT_BLOCK + i * 4, ENT_BLOCK_SIZE, authKey, entBuffer, ref message);
                            }
                            
                            if (!response)
                            {
                                message = "Writing ent data to card failed: " + message;

                                log.LogVariableState("Message ", message);
                                log.LogMethodExit(false);
                                return false;
                            }
                        }
                        else
                        {
                            message = "Setting ent block authkey failed on block " + ENTITLEMENT_BLOCK + 3 + i * 4;

                            log.LogVariableState("Message ", message);
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
            }

            log.LogVariableState("Message ", message);
            log.LogMethodExit(true);
            return true;
        }

        public override void rechargeCard(System.Data.SqlClient.SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);

            TagNumber tagNumber;
            if (tagNumberParser.TryParse(ReaderDevice.readCardNumber(), out tagNumber) == false)
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Place the original card for new issue");
                throw new ApplicationException("Place the original card " + CardNumber + " for Recharge.");
            }

            if (tagNumber.Value != CardNumber)
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Place the original card " + CardNumber + " for Recharge [" + tagNumber.Value + "]");
                throw new ApplicationException("Place the original card " + CardNumber + " for Recharge [" + tagNumber.Value + "]");
            }

            base.rechargeCard(SQLTrx);

            bool response;
            string message = "";

            mifareData.SetValues(mifareData.Structure.Credits / 100.0 + addCredits,
                                mifareData.Structure.Bonus / 100.0 + addBonus,
                                mifareData.Structure.Courtesy / 100.0 + addCourtesy,
                                mifareData.Structure.CreditPlusCredits / 100.0 + addCreditPlusCardBalance,
                                mifareData.Structure.CardDeposit/100.0,
                                vip_customer,
                                'N', 0, base.Entitlements);

            byte[] buffer = mifareData.getBytes();

            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.write_data(PURSE_BLOCK, PURSE_BLOCK_SIZE, ulcKeyStore.LatestCustomerUlcKey.Value, buffer, ref message);
            }
            else
            {
                response = ReaderDevice.write_data(PURSE_BLOCK, PURSE_BLOCK_SIZE, authKey, buffer, ref message);
            }
            if (!response)
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Writing data to card failed: " + message);
                throw new ApplicationException("Writing data to card failed: " + message);
            }

            if (base.Entitlements.Count > 0 || mifareData.Structure.EntitlementChanged)
            {
                if (!writeEntitlements(buffer, ref message, false))
                {
                    log.LogMethodExit(null, "Throwing ApplicationException - Writing ent data to card failed: " + message);
                    throw new ApplicationException("Writing ent data to card failed: " + message);
                }
            }

            ReaderDevice.beep(2);
            log.LogMethodExit(null);
        }

        public override bool AddCreditsToCard(double addCredits, SqlTransaction SQLTrx, ref string message, double usedCredits = 0, double addMiFareCreditPlusCardBalance = 0)
        {
            log.LogMethodEntry(addCredits, SQLTrx, message, usedCredits, addMiFareCreditPlusCardBalance);

            if (!base.AddCreditsToCard(addCredits, SQLTrx, ref message, usedCredits))
            {
                log.LogVariableState("Message ", message);
                log.LogMethodExit(false);
                return false;
            }

            bool response = updateMifareCard(false, ref message, addCredits, 0, 0, addMiFareCreditPlusCardBalance);
            if (!response)
                message = "Writing add credits to card failed";

            log.LogVariableState("Message ", message);
            log.LogMethodExit(response);
            return response;
        }

        /// <summary>
        /// bool updateMifareCard updates the values in mifare card
        /// </summary>
        /// <param name="message"></param>
        /// <param name="values"></param>
        /// <returns>boolean</returns>
        public override bool updateMifareCard(bool AbsoluteOrIncremental, ref string message, params object[] values)
        {
            log.LogMethodEntry(AbsoluteOrIncremental, message, values);

            TagNumber tagNumber;
            if (tagNumberParser.TryParse(ReaderDevice.readCardNumber(), out tagNumber) == false)
            {
                message = "Place the original card for Update";
                log.LogMethodExit(false, message);
                return false;
            }

            if (tagNumber.Value != CardNumber)
            {
                message = "Place the original card for Update";
                log.LogMethodExit(false, message);
                return false;
            }

            bool response;

            response = getBalance();
            if (!response)
            {
                message = "Reading balance failed";

                log.LogVariableState("messae ,", message);
                log.LogMethodExit(false);
                return false;
            }

            if (values.Length > 0)
                mifareData.Structure.Credits = returnFinalValue(mifareData.Structure.Credits, values[0], AbsoluteOrIncremental);
            
            if (values.Length > 1)
                mifareData.Structure.Bonus = returnFinalValue(mifareData.Structure.Bonus, values[1], AbsoluteOrIncremental);

            if (values.Length > 2)
                mifareData.Structure.Courtesy = returnFinalValue(mifareData.Structure.Courtesy, values[2], AbsoluteOrIncremental);

            if (values.Length > 3)
                mifareData.Structure.CreditPlusCredits = returnFinalValue(mifareData.Structure.CreditPlusCredits, values[3], AbsoluteOrIncremental);

            if (values.Length > 4)
                mifareData.Structure.CardDeposit = returnFinalValue(mifareData.Structure.CardDeposit, values[4], AbsoluteOrIncremental);

            if (values.Length > 5)
                mifareData.Structure.VIP = (byte)(values[5].ToString() == "Y" ? 1 : 0);

            if (values.Length > 6)
                mifareData.Structure.DiscountAllowed = (byte)(values[6].ToString() == "Y" ? 1 : 0);

            if (values.Length > 7)
                mifareData.Structure.DiscountPercentage = returnFinalValue(mifareData.Structure.DiscountPercentage, values[7], AbsoluteOrIncremental);

            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.write_data(PURSE_BLOCK, PURSE_BLOCK_SIZE, ulcKeyStore.LatestCustomerUlcKey.Value, mifareData.getBytes(), ref message);
            }
            else
            {
                response = ReaderDevice.write_data(PURSE_BLOCK, PURSE_BLOCK_SIZE, authKey, mifareData.getBytes(), ref message);
            }
            
            if (!response)
                message = "Writing data to card failed";

            ReaderDevice.beep(2);

            log.LogVariableState("message ,", message);
            log.LogMethodExit(response);
            return response;
        }

        UInt32 returnFinalValue(UInt32 input, object value, bool AbsoluteOrIncremental)
        {
            log.LogMethodEntry(input, value, AbsoluteOrIncremental);

            double addValue = Convert.ToDouble(value) * 100;

            if (AbsoluteOrIncremental) //absolute
            {
                if (addValue < 0)
                {
                    log.LogMethodExit(0);
                    return 0;
                }
                else
                {
                    UInt32 returnValue = Convert.ToUInt32(addValue);
                    log.LogMethodExit(returnValue);
                    return returnValue;
                }
            }
            else
            {
                if (addValue < 0)
                {
                    UInt32 returnValue = Math.Max(0, input - Convert.ToUInt32(Math.Abs(addValue)));
                    log.LogMethodExit(returnValue);
                    return returnValue;
                }
                else
                {
                    UInt32 returnValue = input + Convert.ToUInt32(addValue);
                    log.LogMethodExit(returnValue);
                    return returnValue;
                }
            }
        }

        public override void getCardDetails(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);

            byte[] dataBuffer = new byte[ENT_BLOCK_SIZE * 16];
            bool response;
            string message = "";

            base.getCardDetails(cardNumber);

            // authenticate with allowed default keys
            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.read_data_basic_auth(PURSE_BLOCK, 1, ref basicUlcKey, ref dataBuffer, ref message); 
            }
            else
            {
                response = ReaderDevice.read_data_basic_auth(PURSE_BLOCK, 1, ref basicAuthKey, ref dataBuffer, ref message); 
            }
            
            if (response)
            {
                if (card_id != -1 && Utilities.ParafaitEnv.MIFARE_CARD == true) // ignore if it is mifare readonly environment. reading with basic auth is valid
                {
                    message = "Tapped card " + cardNumber + " is NEW but exists in system. Contact Admin.";

                    log.LogMethodExit(null, "Throwing ApplicationException - "+message);
                    throw new ApplicationException(message);

                    //base.invalidateCard(null);
                    //card_id = -1;
                    //base.getCardDetails(cardNumber);
                }
                else
                    ReaderDevice.beep();
            }
            else
            {
                if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    response = ReaderDevice.read_data(PURSE_BLOCK, ENT_BLOCK_SIZE, ulcKeyStore.LatestCustomerUlcKey.Value, ref dataBuffer, ref message);
                }
                else
                {
                    response = ReaderDevice.read_data(PURSE_BLOCK, ENT_BLOCK_SIZE, authKey, ref dataBuffer, ref message);
                }
                if (response)
                {
                    updateMiFareData(dataBuffer);
                    CardStatus = "ISSUED";

                    if (card_id == -1) // mifare card has data but no record in DB.
                    {
                        if (Utilities.ParafaitEnv.AUTO_CREATE_MISSING_MIFARE_CARD)
                        {
                            using (SqlConnection cnn = Utilities.createConnection())
                            {
                                SqlTransaction SQLTrx = cnn.BeginTransaction();
                                try
                                {
                                    base.createCard(SQLTrx);
                                    SQLTrx.Commit();
                                }
                                catch(Exception ex)
                                {
                                    log.Error("Unable to Commit the SQLTransaction! ", ex);
                                    SQLTrx.Rollback();
                                    log.LogMethodExit("Throwing Exception " + ex);    
                                    throw ex;
                                }
                            }
                        }
                        else
                        {
                            //message = "Tapped card is ISSUED but does not exist in system. Please retry or contact Admin.";
                            //The message text was changed on 30-Jun-2015. Previous message is commented above
                            message = "Card " + cardNumber + " is Expired or Invalid in system. Please contact Admin.                         ";
                            log.LogMethodExit(null, "Throwing ApplicationException - " + message);
                            throw new ApplicationException(message);
                        }
                    }

                    if (mifareData.Structure.Version >= 1)
                    {
                        int i = 0;
                        while (i < ENTITLEMENT_SECTORS)
                        {
                            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                            {
                                response = ReaderDevice.read_data(ENTITLEMENT_BLOCK + i * 4, ENT_BLOCK_SIZE, ulcKeyStore.LatestCustomerUlcKey.Value, ref dataBuffer, ref message);
                            }
                            else
                            {
                                response = ReaderDevice.read_data(ENTITLEMENT_BLOCK + i * 4, ENT_BLOCK_SIZE, authKey, ref dataBuffer, ref message);
                            }
                            if (response)
                                mifareData.getEntitlements(dataBuffer); // ignore in case there is an error reading entitlement
                            else
                                break;
                            i++;
                        }
                        mifareData.RationalizeEntitlements();

                        CardGames = 0;
                        foreach (MifareDataClass.Entitlement ent in mifareData.Structure.Entitlements)
                        {
                            if ((ent.EntType_IdType & 0xF0) == 0x10 && ent.EntCount != 0xffff)
                            {
                                CardGames += ent.EntCount;
                            }
                        }
                    }

                    ReaderDevice.beep();
                }
                else
                {
                    message += "; Get Card Details failed.";
                    log.LogMethodExit(null,"Throwing ApplicationException - "+message);
                    throw new ApplicationException(message);
                }
            }
        }

        void updateMiFareData(byte[] dataBuffer)
        {
            log.LogMethodEntry(dataBuffer);

            mifareData.fromBytes(dataBuffer);
            if (mifareData.Structure.HexKey.Equals(HexKey) == false)
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Invalid HexKey(" + CardNumber + ")");
                throw new ApplicationException("Invalid HexKey (" + CardNumber + ")");
            }

            const int maxVal = 99999999;
            if (mifareData.Structure.Credits > maxVal
            || mifareData.Structure.Courtesy > maxVal
            || mifareData.Structure.Bonus > maxVal
            || mifareData.Structure.CreditPlusCredits > maxVal
            || mifareData.Structure.CardDeposit > maxVal)
            {
                Utilities.EventLog.logEvent("POS", 'E', CardNumber + "-"
                                            + mifareData.Structure.Credits.ToString() + "-"
                                            + mifareData.Structure.Courtesy.ToString() + "-"
                                            + mifareData.Structure.Bonus.ToString() + "-"
                                            + mifareData.Structure.CreditPlusCredits.ToString() + "-"
                                            + mifareData.Structure.CardDeposit.ToString(), "MiFare Max value error", "MIFARE", 3);

                log.LogMethodExit(null, "Throwing ApplicationException - Error Reading Card (" + CardNumber + "). Please Transfer card to a New card");
                throw new ApplicationException("Error Reading Card (" + CardNumber + "). Please Transfer card to a New card");
            }

            credits = Convert.ToDouble(mifareData.Structure.Credits / 100.0);
            courtesy = Convert.ToDouble(mifareData.Structure.Courtesy / 100.0);
            bonus = Convert.ToDouble(mifareData.Structure.Bonus / 100.0);
            CreditPlusCardBalance = Convert.ToDouble(mifareData.Structure.CreditPlusCredits / 100.0);
            face_value = (float)Convert.ToDouble(mifareData.Structure.CardDeposit / 100.0);

            mifareData.Structure.Entitlements.Clear();
            for (int i = 0; i < ENTITLEMENT_SECTORS * ENT_BLOCK_SIZE * 2; i++)
            {
                mifareData.Structure.Entitlements.Add(new MifareDataClass.Entitlement());
            }

            log.LogMethodExit(null);
            return;
        }

        public override bool checkCardExists()
        {
            log.LogMethodEntry();

            bool response;
            byte[] dataBuffer = new byte[16];
            string message = "";
            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.read_data(PURSE_BLOCK, 1, ulcKeyStore.LatestCustomerUlcKey.Value, ref dataBuffer, ref message);
            }
            else
            {
                response = ReaderDevice.read_data(PURSE_BLOCK, 1, authKey, ref dataBuffer, ref message);
            }
            if (!response)
            {
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
        }

        public override bool refund_MCard(ref string message)
        {
            log.LogMethodEntry(message);

            bool response;
            byte[] value = Enumerable.Repeat((byte)0x00, PURSE_BLOCK_SIZE * 16).ToArray();

            TagNumber tagNumber;
            if (tagNumberParser.TryParse(ReaderDevice.readCardNumber(), out tagNumber) == false)
            {
                message = "Place the original card for Update";
                log.LogMethodExit(false, message);
                return false;
            }

            if (tagNumber.Value != CardNumber)
            {
                message = "Place the original card for Update";
                log.LogMethodExit(false, message);
                return false;
            }

            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.change_authentication_key(PURSE_AUTH_BLOCK, ulcKeyStore.LatestCustomerUlcKey.Value, basicUlcKey, ref message);
            }
            else
            {
                response = ReaderDevice.change_authentication_key(PURSE_AUTH_BLOCK, authKey, basicAuthKey, ref message);
            }
            if (!response)
            {
                message += "Changing authentication key for refund failed. Failed on " + PURSE_BLOCK;

                log.LogVariableState("Message ,", message);
                log.LogMethodExit(false);
                return false;
            }

            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.write_data(PURSE_BLOCK, PURSE_BLOCK_SIZE, basicUlcKey, value, ref message);
            }
            else
            {
                response = ReaderDevice.write_data(PURSE_BLOCK, PURSE_BLOCK_SIZE, basicAuthKey, value, ref message);
            }
            if (!response)
            {
                message += "Clearing the data failed.";
            }

            if (ReaderDevice.CardType != CardType.MIFARE_ULTRA_LIGHT_C)
            {
                for (int i = 0; i < ENTITLEMENT_SECTORS; i++)
                {
                    response = ReaderDevice.change_authentication_key(ENTITLEMENT_BLOCK + 3 + i * 4, authKey, basicAuthKey, ref message);
                    if (!response)
                        message += "Changing authentication key for refund failed. Failed on " + ENTITLEMENT_BLOCK + 3 + i * 4;
                }
            }

            ReaderDevice.beep();

            log.LogVariableState("Message ,", message);
            log.LogMethodExit(true);
            return true;
        }
        
        public bool getBalance()
        {
            log.LogMethodEntry();

            byte[] dataBuffer = new byte[PURSE_BLOCK_SIZE * 16];
            bool response;
            string message = "";
            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.read_data(PURSE_BLOCK, PURSE_BLOCK_SIZE, authKey, ref dataBuffer, ref message);
            }
            else
            {
                response = ReaderDevice.read_data(PURSE_BLOCK, PURSE_BLOCK_SIZE, authKey, ref dataBuffer, ref message);
            }
            
            if (response)
            {
                updateMiFareData(dataBuffer);

                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }

        public override bool setChildSiteCode(ref byte[] purseDataBuffer, int siteCode, ref string message)
        {
            log.LogMethodEntry(purseDataBuffer, siteCode, message);

            bool response;
            for (int i = SITEIDPOS; i < (SITEIDPOS + SITEIDSIZE); i++)
                purseDataBuffer[i] = Convert.ToByte(siteCode >> (8 * (i - SITEIDPOS)) & 0xFF);

            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.write_data(PURSE_BLOCK, 1, ulcKeyStore.LatestCustomerUlcKey.Value, purseDataBuffer, ref message);
            }
            else
            {
                response = ReaderDevice.write_data(PURSE_BLOCK, 1, authKey, purseDataBuffer, ref message);
            }
            if (!response)
            {
                log.LogVariableState("Message ,", message);
                log.LogMethodExit(false);
                return false;
            }

            log.LogVariableState("Message ,", message);
            log.LogMethodExit(true);
            return true;
        }

        public override int getChildSiteCode(ref byte[] purseDataBuffer, ref string message)
        {
            log.LogMethodEntry(purseDataBuffer, message);

            bool response;
            int siteCode = 0;
            message = "";
            if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
            {
                response = ReaderDevice.read_data(PURSE_BLOCK, 1, ulcKeyStore.LatestCustomerUlcKey.Value, ref purseDataBuffer, ref message);
            }
            else
            {
                response = ReaderDevice.read_data(PURSE_BLOCK, 1, authKey, ref purseDataBuffer, ref message);
            }
            if (!response)
            {
                if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    response = ReaderDevice.read_data(PURSE_BLOCK, 1, basicUlcKey, ref purseDataBuffer, ref message);
                }
                else
                {
                    response = ReaderDevice.read_data(PURSE_BLOCK, 1, basicAuthKey, ref purseDataBuffer, ref message);
                }
                
                if (response)
                    message = "NEW";
                else
                    message = "INVALID";
            }
            else
            {
                for (int i = SITEIDPOS; i < SITEIDPOS + SITEIDSIZE; i++)
                    siteCode += purseDataBuffer[SITEIDPOS] << 8 * (i - SITEIDPOS);
            }

            log.LogVariableState("Message ,", message);
            log.LogMethodExit(siteCode);
            return siteCode;
        }
        
        private string getValue(byte[] dataArray, int offset, int size)
        {
            log.LogMethodEntry(dataArray, offset, size);

            string value = "";
            for (int i = size - 1; i >= 0; i--)
            {
                value = String.Concat(value, (dataArray[offset] - 16).ToString());
                offset++;
            }

            log.LogMethodExit(value);
            return value;
        }

        public override int getPlayCount()
        {
            log.LogMethodEntry();

            byte[] dataBuffer = new byte[16];
            string message = "";
            bool returnSuccess = false;
            int gamePlayCount = 0;
            try
            {
                if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    returnSuccess = ReaderDevice.read_data(PURSE_BLOCK, 1, ulcKeyStore.LatestCustomerUlcKey.Value, ref dataBuffer, ref message);
                }
                else
                {
                    returnSuccess = ReaderDevice.read_data(PURSE_BLOCK, 1, authKey, ref dataBuffer, ref message);
                }
                if (returnSuccess)
                {
                    for (int i = 0; i < (GAMEPLAYSIZE); i++)
                        gamePlayCount = (dataBuffer[i + GAMEPLAYPOS] << (8 * i)) + gamePlayCount;
                }
                else
                {
                    log.LogMethodExit(-1);
                    return -1;
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured while reading game play data", ex);
                gamePlayCount = -1;
            }

            log.LogMethodExit(gamePlayCount);
            return gamePlayCount;
        }

        public override string getPlayDetails(int gamePlayNumber, ref int siteCode, ref int machineId, ref double startingBalance, ref double endingBalance)
        {
            log.LogMethodEntry(gamePlayNumber, siteCode, machineId, startingBalance, endingBalance);

            int c = getPlayCount();
            if (c == -1)
            {
                log.LogVariableState("siteCode ,", siteCode);
                log.LogVariableState("machineId ", machineId);
                log.LogVariableState("startingBalance ,", startingBalance);
                log.LogVariableState("endingBalance ,", endingBalance);
                log.LogMethodExit("Error: No data found");
                return "Error: No data found";
            }

            bool returnSuccess = false;
            byte[] dataBuffer = new byte[16];
            
            string message = "";
            try
            {
                if (ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    returnSuccess = ReaderDevice.read_data(mifareS50DataBlocks[gamePlayNumber], 1, basicUlcKey, ref dataBuffer, ref message);
                }
                else
                {
                    returnSuccess = ReaderDevice.read_data(mifareS50DataBlocks[gamePlayNumber], 1, basicAuthKey, ref dataBuffer, ref message);
                }
                if (returnSuccess)
                {
                    try
                    {
                        siteCode = dataBuffer[0];
                        machineId = dataBuffer[1];
                        startingBalance = (dataBuffer[2] + (dataBuffer[3] << 8) + (dataBuffer[4] << 16) + (dataBuffer[5] << 24)) / 100;
                        endingBalance = (dataBuffer[6] + (dataBuffer[7] << 8) + (dataBuffer[8] << 16) + (dataBuffer[9] << 24)) / 100;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while reading mifare card data", ex);

                        log.LogVariableState("returnSuccess ", returnSuccess);
                        log.LogVariableState("siteCode ,", siteCode);
                        log.LogVariableState("machineId ", machineId);
                        log.LogVariableState("startingBalance ,", startingBalance);
                        log.LogVariableState("endingBalance ,", endingBalance);
                        log.LogMethodExit("Error: " + ex.Message);
                        return "Error: " + ex.Message;
                    }
                }
                if (!returnSuccess)
                {
                    log.LogVariableState("returnSuccess ", returnSuccess);
                    log.LogVariableState("siteCode ,", siteCode);
                    log.LogVariableState("machineId ", machineId);
                    log.LogVariableState("startingBalance ,", startingBalance);
                    log.LogVariableState("endingBalance ,", endingBalance);
                    log.LogMethodExit("Error: Card reading failed");
                    return "Error: Card reading failed";
                }

                log.LogVariableState("siteCode ,", siteCode);
                log.LogVariableState("machineId ", machineId);
                log.LogVariableState("startingBalance ,", startingBalance);
                log.LogVariableState("endingBalance ,", endingBalance);
                log.LogMethodExit("");
                return "";
            }
            catch (Exception ex)
            {
                log.Error("Error occured while reading mifare card data", ex);
                log.LogVariableState("returnSuccess ", returnSuccess);
                log.LogVariableState("siteCode ,", siteCode);
                log.LogVariableState("machineId ", machineId);
                log.LogVariableState("startingBalance ,", startingBalance);
                log.LogVariableState("endingBalance ,", endingBalance);
                log.LogMethodExit("Error: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }
    }
}

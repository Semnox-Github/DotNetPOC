using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using MTSCRANET;
using MTLIB;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Class ClsDynamagMagnesafe
    /// </summary>
    public class ClsDynamagMagnesafe
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MTSCRA m_SCRA;
        private List<MTLIB.MTDeviceInformation> _deviceList;

        /// <summary>
        /// Class _cardData
        /// </summary>
        public class _cardData
        {
            /// <summary>
            /// String m_szTrack2Data
            /// </summary>
            public string m_szTrack2Data;

            /// <summary>
            /// String m_szDUKPTKSN
            /// </summary>
            public string m_szDUKPTKSN;
        }

        /// <summary>
        /// Card Data State Changed Event
        /// </summary>
        public class CardDataStateChangedEventArgs : EventArgs
        {
            /// <summary>
            /// Get/Set CardData
            /// </summary>
            public _cardData CardData
            {
                get;
                private set;
            }

            /// <summary>
            /// Card Data State Changed Event
            /// </summary>
            /// <param name="cardData"></param>
            public CardDataStateChangedEventArgs(_cardData cardData)
            {
                CardData = cardData;
            }
        }

        private EventHandler ehCardDataEventHandler;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ClsDynamagMagnesafe()
        {
            log.LogMethodEntry();
            SetupCallBacks();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// SetupCallCacks Method
        /// </summary>
        public void SetupCallBacks()
        {
            log.LogMethodEntry();

            m_SCRA = new MTSCRA();

            m_SCRA.OnDeviceList += OnDeviceList;
            m_SCRA.OnDeviceConnectionStateChanged += OnDeviceConnectionStateChanged;
            m_SCRA.OnCardDataState += OnCardDataStateChanged;
            m_SCRA.OnDataReceived += OnDataReceived;
            m_SCRA.OnDeviceResponse += OnDeviceResponse;

            log.LogMethodExit(null);
        }

        private void connect()
        {
            log.LogMethodEntry();

            if (m_SCRA.isDeviceConnected())
            {
                log.LogMethodExit(null);
                return;
            }

            MTLIB.MTConnectionType connectionType = MTLIB.MTConnectionType.USB;

            m_SCRA.requestDeviceList(connectionType);

            if (_deviceList.Count > 0)
            {
                m_SCRA.setConnectionType(connectionType);

                string address = _deviceList[0].Address;

                m_SCRA.setAddress(address);

                m_SCRA.openDevice();
            }
            else
            {
                log.LogMethodExit(null, "Throwing Exception - No devices connected");
                throw new Exception("No devices connected");
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Destructor ClsDynamagMagnesafe
        /// </summary>
        ~ClsDynamagMagnesafe()
        {
            log.LogMethodEntry();
            disconnect();
            log.LogMethodExit(null);
        }

        private void disconnect()
        {
            log.LogMethodEntry();

            if (m_SCRA.isDeviceConnected())
            {
                m_SCRA.closeDevice();
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get Card Information
        /// </summary>
        /// <param name="swipedCardData"></param>
        public void getCardInfo(IMTCardData swipedCardData)
        {
            log.LogMethodEntry(swipedCardData);

            try
            {
                _cardData cardData = new _cardData();

                cardData.m_szTrack2Data = swipedCardData.getTrack2();
                cardData.m_szDUKPTKSN = swipedCardData.getKSN();

                if (ehCardDataEventHandler != null)
                    ehCardDataEventHandler(this, new CardDataStateChangedEventArgs(cardData));
            }
            catch (Exception ex)
            {
                log.Error("Unable to get the card data",ex);
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Devices On List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="connectionType"></param>
        /// <param name="deviceList"></param>
        protected void OnDeviceList(object sender, MTLIB.MTConnectionType connectionType, List<MTLIB.MTDeviceInformation> deviceList)
        {
            log.LogMethodEntry(sender, connectionType, deviceList);

            _deviceList = deviceList;

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Device On Connect State Changed Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="state"></param>
        protected void OnDeviceConnectionStateChanged(object sender, MTLIB.MTConnectionState state)
        {
            log.LogMethodEntry(sender, state);
            log.LogMethodExit(null);
        }

        /// <summary>
        /// card Data State Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="state"></param>
        protected void OnCardDataStateChanged(object sender, MTLIB.MTCardDataState state)
        {
            log.LogMethodEntry(sender, state);

            switch (state)
            {
                case MTCardDataState.DataError:
                    break;
                case MTCardDataState.DataNotReady:
                    break;
                case MTCardDataState.DataReady:
                    break;
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Data Received Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cardData"></param>
        protected void OnDataReceived(object sender, IMTCardData cardData)
        {
            log.LogMethodEntry(sender, cardData);

            //sendToDisplay("[Raw Data]");
            //sendToDisplay(m_SCRA.getResponseData());

            getCardInfo(cardData);

            log.LogMethodExit(null);
        }

        /// <summary>
        /// On Device Response Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        protected void OnDeviceResponse(object sender, string data)
        {
            log.LogMethodEntry(sender, data);

            //sendToDisplay("[Device Response]");
            //sendToDisplay(data);

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Card Data Event Open
        /// </summary>
        /// <param name="CardDataEventHandler"></param>
        /// <returns></returns>
        public bool Open(EventHandler CardDataEventHandler)
        {
            log.LogMethodEntry(CardDataEventHandler);

            try
            {
                connect();
                ehCardDataEventHandler = CardDataEventHandler;

                log.LogMethodExit(true);
                return true;
            }
            catch(Exception ex) 
            {
                log.Error("Error when connecting to Card Data Handler", ex);
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Card Data Event Close
        /// </summary>
        public void Close()
        {
            log.LogMethodEntry();

            try
            {
                disconnect();
            }
            catch(Exception ex)
            {
                log.Error("Error occured whendisconnecting");
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            log.LogMethodExit(null);
        }
    }
}

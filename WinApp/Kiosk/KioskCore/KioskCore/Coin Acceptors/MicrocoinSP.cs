/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - MicrocoinSP.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        9-Oct-2023       Iqbal            Created
 ********************************************************************************************/
using System;
using Astrosys.Sdk;

namespace Semnox.Parafait.KioskCore.CoinAcceptor
{
    public class MicrocoinSP : CoinAcceptor
    {
        CoinValidator _CoinValidator;

        private static bool SDKInitialized = false;
        private int receivedCoinCategory = 0;

        public MicrocoinSP()
        {
            log.LogMethodEntry();
            if (SDKInitialized == false)
            {
                // Hook up all the Sdk events
                AstrosysSdk.Instance.DeviceConnected += Instance_DeviceConnected;
                AstrosysSdk.Instance.DeviceDisconnected += Instance_DeviceDisconnected;
                AstrosysSdk.Instance.ErrorOccurred += Instance_ErrorOccurred;

                AstrosysSdk.Instance.Start(false, true, false); 
            } 

            foreach (BaseDevice device in AstrosysSdk.Instance.GetDevices(false))
            {
                if (device.DeviceType == DeviceType.CoinValidator)
                {
                    _CoinValidator = device as CoinValidator;
                    break;
                }
            }

            if (_CoinValidator == null)
                throw new ApplicationException("Microcoin SP Validator device not found");

            if (SDKInitialized == false)
            {
                MySetAcceptance();

                SDKInitialized = true; 
                AstrosysSdk.Instance.StopPolling(); 
				_CoinValidator.IsEnabled = false;
            } 
            log.LogMethodExit();
        }

        private void _CoinValidator_ErrorOccurred(object sender, CoinValidatorErrorOccurredEventArgs e)
        {
            log.LogMethodEntry();
            string message = e.ErrorCode + ": " + e.Message;
            log.Debug("MicrocoinSP: " + message);
            KioskStatic.logToFile("MicrocoinSP: " + message);
            log.LogMethodExit();
        }

        private void _CoinValidator_CoinAccepted(object sender, CoinAcceptedEventArgs e)
        {
            log.LogMethodEntry();
            string message = $"Coin Received: Denom: {e.CoinCategory}, Coin Name: {e.CoinName}, Value: {e.CurrencyValue}";

            receivedCoinCategory = e.CoinCategory;
            dataReceivedEvent?.Invoke();

            log.Debug(message);
            KioskStatic.logToFile(message);
            log.LogMethodExit();
        }

        private void Instance_ErrorOccurred(object sender, ErrorOccurredEventArgs e)
        {
            log.LogMethodEntry();
            log.Debug(e.Message);
            KioskStatic.logToFile(e.Message);
            log.LogMethodExit();
        }

        private void Instance_DeviceDisconnected(object sender, DeviceDisconnectedEventArgs e)
        {
            log.LogMethodEntry();
            log.Debug(e.Device.ToString() + " disconnected");
            KioskStatic.logToFile(e.Device.ToString() + " disconnected");
            log.LogMethodExit();
        }

        private void Instance_DeviceConnected(object sender, DeviceConnectedEventArgs e)
        {
            log.LogMethodEntry();
            log.Debug(e.Device.ToString() + " connected");
            KioskStatic.logToFile(e.Device.ToString() + " connected");
            log.LogMethodExit();
        }

        public override bool ProcessReceivedData(byte[] dummy, ref string message)
        {            
            log.LogMethodEntry(dummy, message);
            try
            {
                ReceivedCoinDenomination = receivedCoinCategory;
                receivedCoinCategory = 0;
                message = KioskStatic.config.Coins[ReceivedCoinDenomination].Name + " accepted";
                KioskStatic.logToFile(message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in ProcessReceivedData: " + ex.Message);
            }
            log.LogMethodExit(true);
            return true;
        }

        public override void disableCoinAcceptor()
        {
            log.LogMethodEntry();
            try
            {
                _CoinValidator.IsEnabled = false;
                AstrosysSdk.Instance.StopPolling();
                _CoinValidator.CoinAccepted -= _CoinValidator_CoinAccepted;
                _CoinValidator.ErrorOccurred -= _CoinValidator_ErrorOccurred;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in disableCoinAcceptor: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public override bool set_acceptance(bool isTokenMode = false)
        {
            log.LogMethodEntry(isTokenMode);
			bool returnValue = false;
            if (isTokenMode == false)
			{
				returnValue = true;
			}
            else
            {
                try
                {
                    returnValue = MySetAcceptance(true);   
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    KioskStatic.logToFile(ex.Message);
                    returnValue = false;
                }
            }
			if (returnValue)
			{
			    _CoinValidator.CoinAccepted += _CoinValidator_CoinAccepted;
                _CoinValidator.ErrorOccurred += _CoinValidator_ErrorOccurred;
				AstrosysSdk.Instance.StartPolling();
				_CoinValidator.IsEnabled = true; 
			}
			
            log.LogMethodExit(returnValue);
			return returnValue;
        }

        private bool MySetAcceptance(bool isTokenMode = false)
        {
            log.LogMethodEntry(isTokenMode);

            _CoinValidator.DisableAllCategories();
            var validCategories = _CoinValidator.GetValidCategoryIndexes();

            foreach (var cat in validCategories)
            {
                if (cat < KioskStatic.config.Coins.Length)
                {
                    if (KioskStatic.config.Coins[cat] != null
                        && ((isTokenMode == false && KioskStatic.config.Coins[cat].isToken == false)
                        || (isTokenMode == true && KioskStatic.config.Coins[cat].isToken == true)))
                    {
                        try
                        {
                            _CoinValidator.EnableCategory(cat);
                        }
                        catch { }
                    }
                }
                else
                {
                    string mes = $"Coin Category {cat} not defined in setup";
                    log.Debug(mes);
                    KioskStatic.logToFile(mes);
                }
            }
            //_CoinValidator.IsEnabled = true;
            log.LogMethodExit(true);
            return true;
        }
    }
}
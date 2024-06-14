/********************************************************************************************
* Project Name - Device.Biometric
* Description  - To support Futronic & Morpho Device
*  
**************
**Version Log
**************
*Version       Date          Modified By        Remarks          
*********************************************************************************************
*2.80          09-Arp-2020   Indrajet Kumar     Created
********************************************************************************************/
using System;
using System.Text;

namespace Semnox.Parafait.Device.Biometric
{
    public enum BiometricDeviceType
    {
        /// <summary>
        /// Biometric Device FUTRONICFS84
        /// </summary>
        FUTRONICFS84,
        /// <summary>
        /// Biometric Device MORPHO MSO 1300
        /// </summary>
        MORPHO,
    }
    public class BiometricFactory 
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static BiometricFactory biometricFactory;

        /// <summary>
        /// Indicates whether the Biometric Factory is initialized.
        /// </summary>
        protected bool initialized = false;

        /// <summary>
        /// Initializes the Biometric Factory.
        /// </summary>       
        public virtual void Initialize()
        {
            log.LogMethodEntry();
            if (!initialized)
            {
                initialized = true;
            }
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Returns a singleton instance of payment gateway factory.
        /// </summary>
        /// <returns></returns>
        public static BiometricFactory GetInstance()
        {
            log.LogMethodEntry();
            if (biometricFactory == null)
            {
                biometricFactory = new BiometricFactory();
            }
            log.LogMethodExit(biometricFactory);
            return biometricFactory;
        }
        public FingerPrintReader GetBiometricDeviceType(string biometricDeviceTypeString)
        {
            log.LogMethodEntry(biometricDeviceTypeString);
            BiometricDeviceType biometricDeviceType;
            FingerPrintReader fingerPrintReader = null;
            if (Enum.TryParse<BiometricDeviceType>(biometricDeviceTypeString, out biometricDeviceType))
            {                
                fingerPrintReader = CreateBiometricDeviceTypeInstance(biometricDeviceType);
            }
            else
            {
                log.LogMethodExit(null,"");                
            }
            log.LogMethodExit(fingerPrintReader);
            return fingerPrintReader;
        }
        private FingerPrintReader CreateBiometricDeviceTypeInstance(BiometricDeviceType biometricDeviceType)
        {
            log.LogMethodEntry(biometricDeviceType);
            FingerPrintReader fingerPrintReader = null;
            switch (biometricDeviceType)
            {
                case BiometricDeviceType.FUTRONICFS84:
                    {
                        fingerPrintReader = new Semnox.Parafait.Device.Biometric.FutronicFS84.FutronicFS84();
                        break;
                    }
                case BiometricDeviceType.MORPHO:
                    {
                        try
                        {
                            Type type = Type.GetType("Semnox.Parafait.Device.Biometric.Morpho.ParafaitMorphoFingerPrintReader, MorphoFingerPrintReader");
                            fingerPrintReader = Activator.CreateInstance(type) as FingerPrintReader;
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while creating MORPHO fingerprint reader", ex);
                            throw;
                        }
                        break;
                    }
            }            
            log.LogMethodExit(fingerPrintReader);
            return fingerPrintReader;
        }        
    }
}

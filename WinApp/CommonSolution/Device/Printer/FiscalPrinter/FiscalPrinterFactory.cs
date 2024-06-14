/********************************************************************************************
 * Project Name - Device                                                                      
 * Description  - Factory class to retrieve fiscal printers
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 **             27-Jul-2017   Archana Kulal       Created
 *2.90.0        14-Jul-2020   Gururaja Kanjan     Updated for processing failed entries of fiskaltrust integration
 *2.140.0       08-Feb-2022	  Girish Kundar       Modified: Smartro Fiscalization
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Parafait.Device.Printer.FiscalPrint.Smartro;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
    public enum FiscalPrinters
    {
        /// <summary>
        /// No Fiscal printer.
        /// </summary>
        None,
        /// <summary>
        /// RCHGlobe fiscalPrinter
        /// </summary>
        RCHGlobe,
        /// <summary>
        /// HUGIN fiscalPrinter
        /// </summary>
        HUGIN,
        /// <summary>
        /// ELTRADE fiscalPrinter
        /// </summary>
        ELTRADE,
        /// <summary>
        /// Incotex fiscalPrinter
        /// </summary>
        Incotex,
        /// <summary>
        /// Ditron fiscalPrinter
        /// </summary>
        Ditron,
        /// <summary>
        /// BowaPegas fiscalPrinter
        /// </summary>
        BowaPegas,
        /// <summary>
        /// SRP-812 fiscalPrinter
        /// </summary>
        BIXOLON_SRP_812,
        /// <summary>
        /// CroatiaFiscalization
        /// </summary>
        CroatiaFiscalization,
        /// <summary>
        /// GermanFiscalization - FiscalTrust
        /// </summary>
        FiskalTrust,
        /// <summary>
        /// KoreaFiscalization - SmartroKorea
        /// </summary>
        Smartro
    }

    public class FiscalPrinterFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static FiscalPrinterFactory fiscalPrinterFactory;
        protected Dictionary<FiscalPrinters, FiscalPrinter> fiscalPrinterDictionary=null;

        /// <summary>
        /// Parafait utilities
        /// </summary>
        protected Utilities utilities = null;

        
        public virtual void Initialize(Utilities utilities, bool unAttendedMode = false)
        {
            log.LogMethodEntry(utilities, unAttendedMode);
            if (this.utilities == null)
            {
                this.utilities = utilities;
            }
            log.LogMethodExit(null);
        }

        public static FiscalPrinterFactory GetInstance()
        {
            log.LogMethodEntry();
            if (fiscalPrinterFactory == null)
            {
                fiscalPrinterFactory = new FiscalPrinterFactory();
            }
            log.LogMethodExit(fiscalPrinterFactory);
            return fiscalPrinterFactory;
        }

        public FiscalPrinter GetFiscalPrinter(string fiscalPrinterString, bool unAttendedMode = false)
        {
            log.LogMethodEntry(fiscalPrinterString, unAttendedMode);
            FiscalPrinters fiscalPrinterName;
            FiscalPrinter fiscalPrinter = null;

            if (Enum.TryParse<FiscalPrinters>(fiscalPrinterString, out fiscalPrinterName))
            {
                if (fiscalPrinterDictionary == null)
                {
                    fiscalPrinterDictionary = new Dictionary<FiscalPrinters, FiscalPrinter>();
                }

                if (fiscalPrinterDictionary.ContainsKey(fiscalPrinterName))
                {
                    fiscalPrinter = fiscalPrinterDictionary[fiscalPrinterName];
                }
                else
                {
                    fiscalPrinter = getFiscalPrinterInstance(fiscalPrinterName, unAttendedMode);
                    if (fiscalPrinter == null)
                    {
                        log.LogMethodExit(null, "Fiscal Printer Configuration Exception - Error occured while creating the fiscalPrinter. type: " + fiscalPrinterName.ToString());
                    }
                    else
                    {
                        fiscalPrinterDictionary.Add(fiscalPrinterName, fiscalPrinter);
                    }
                }
            }
            else
            {
                log.LogMethodExit(null, "Fiscal Printer Configuration Exception - FiscalPrinters enum not configured with printer name: " + fiscalPrinterName.ToString());
            }

            log.LogMethodExit(fiscalPrinter);
            return fiscalPrinter;
        }

        
        private FiscalPrinter getFiscalPrinterInstance(FiscalPrinters fiscalPrinterName, bool unAttendedMode)
        {
            log.LogMethodEntry(fiscalPrinterName, unAttendedMode);
            FiscalPrinter fiscalPrinter = null;

            switch (fiscalPrinterName)
            {
                case FiscalPrinters.None:
                    {
                        fiscalPrinter = new FiscalPrinter(utilities);
                        break;
                    }
                case FiscalPrinters.RCHGlobe:
                    {
                        fiscalPrinter = new RCHGlobe(utilities);
                        break;
                    }
                case FiscalPrinters.HUGIN:
                    {
                        fiscalPrinter = new HUGIN(utilities);
                        break;
                    }
                case FiscalPrinters.Incotex:
                    {
                        fiscalPrinter = new Incotex(utilities);
                        break;
                    }
                case FiscalPrinters.ELTRADE:
                    {
                        fiscalPrinter = new Eltrade(utilities);
                        break;
                    }
                case FiscalPrinters.Ditron:
                    {
                        fiscalPrinter = new Ditron(utilities);
                        break;
                    }
                case FiscalPrinters.BowaPegas:
                    {
                        fiscalPrinter = new BowaPegas(utilities);
                        break;
                    }
                case FiscalPrinters.BIXOLON_SRP_812:
                    {
                        fiscalPrinter = new BIXOLON_SRP_812(utilities);
                        break;
                    }
                case FiscalPrinters.CroatiaFiscalization:
                    {
                        fiscalPrinter = new CroatiaFiscalization(utilities);
                        break;
                    }
                case FiscalPrinters.FiskalTrust:
                    {
                        fiscalPrinter = new FiskaltrustPrinter(utilities);
                        break;
                    }
                case FiscalPrinters.Smartro:
                    {
                        fiscalPrinter = new Smartro.Smartro(utilities, unAttendedMode);
                        break;
                    }
            }
            log.LogMethodExit(fiscalPrinter);
            return fiscalPrinter;
        }
    }
}
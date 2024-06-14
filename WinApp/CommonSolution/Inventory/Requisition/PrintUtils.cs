/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - Print Utils
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       13-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/

namespace Semnox.Parafait.Inventory
{
    public static class PrintUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void SetOutputFileName(string filename)
        {
            log.LogMethodEntry(filename);
            string exeName = CommonFuncs.Utilities.getParafaitDefaults("PDF_CONFIG_EXE");
            string parameter;
            System.Diagnostics.Process p;

            parameter = "/C";
            p =System.Diagnostics.Process.Start(exeName, parameter); // change output file name
            p.WaitForExit();

            parameter = "/S output \"" +  filename + "\"";
            //p = System.Diagnostics.Process.Start(exeName, parameter);  
            //p.WaitForExit();

            var process = new System.Diagnostics.Process();
            process.StartInfo.Arguments = parameter;
            process.StartInfo.FileName = exeName;
            //process.StartInfo.UseShellExecute = true;

            process.Start();
            process.WaitForExit();

            parameter = "/S showsaveas \"never\"";
            p = System.Diagnostics.Process.Start(exeName, parameter);  
            p.WaitForExit();
           
            parameter = "/S disableoptiondialog \"yes\"";
            p = System.Diagnostics.Process.Start(exeName, parameter);  
            p.WaitForExit();
            
            parameter = "/S confirmoverwrite \"no\"";
            p = System.Diagnostics.Process.Start(exeName, parameter);  
            p.WaitForExit();
            
            parameter = "/S showsettings \"never\"";
            p = System.Diagnostics.Process.Start(exeName, parameter); 
            p.WaitForExit();
            
            parameter = "/S showpdf \"no\"";
            p = System.Diagnostics.Process.Start(exeName, parameter);  
            p.WaitForExit();
        
            parameter = "/S rememberlastfilename \"no\"";
            p = System.Diagnostics.Process.Start(exeName, parameter);  
            p.WaitForExit();

            parameter = "/S rememberlastfoldername \"no\"";
            p = System.Diagnostics.Process.Start(exeName, parameter);   
            p.WaitForExit();
            log.LogMethodExit();
        }

        public static string GetPDFPrinterName()
        {
            log.LogMethodEntry();
            //Common.initEnv();
            string returnValue = (CommonFuncs.Utilities.getParafaitDefaults("PDF_WRITER_PRINTER"));
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}

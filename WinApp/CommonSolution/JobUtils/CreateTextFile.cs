/********************************************************************************************
 * Project Name - JobUtils
 * Description  - Create Text Files
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Specific class for creating text file which implements ICreateFile interface
    /// </summary>
    public class CreateTextFile : ICreateFile
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities Utilities;
        /// <summary>
        /// Property to hold file name dateformat
        /// </summary>
        public string FileNameDateFormat { get; set; }
        /// <summary>
        /// Parameterized constructor which instanciate the parafait utility class
        /// </summary>
        /// <param name="_Utilities">i</param>
        public CreateTextFile(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            Utilities = _Utilities;
            FileNameDateFormat= ConfigurationManager.AppSettings["FileDateTimeFormat"].ToString();
            log.LogMethodExit();
        }
        /// <summary>
        /// CreateFile method which get data from IQuery and create the text file and store into FileStorePath
        /// </summary>
        /// <param name="query">Interface Qquery for querying database</param>
        /// <param name="fileStorePath">path to store .txt files</param>
        /// <returns>Returns 1 on successfull file creation, 0 failure, -1 no records found for the day</returns>
        public int CreateFile(IQuery query, ref string fileStorePath)
        {
            log.LogMethodEntry(query, fileStorePath);
            int status = 0;
            int recordCount = 0;
            DataTable resultDt = query.GetQueryResult();

            recordCount = resultDt.Rows.Count;
            if (resultDt != null && recordCount > 0)
            {                
                string lineEntry = string.Empty;
                //write header into text file
                lineEntry += string.Join("|", resultDt.Columns.OfType<DataColumn>().Select(r => r.ToString()));

                lineEntry += Environment.NewLine;//enter to next line

                //write rows into text file
                foreach(DataRow row in resultDt.Rows)
                {
                    lineEntry += String.Join("|", row.ItemArray);

                    lineEntry += Environment.NewLine;//enter to next line
                }
                if (!string.IsNullOrEmpty(lineEntry))
                {
                    //write Count File
                    File.WriteAllText(GetFileName(fileStorePath + query.FileName, ".cnt"), recordCount.ToString(), Encoding.UTF8);

                    //write Text File
                    fileStorePath = GetFileName(fileStorePath + query.FileName, ".txt");
                    File.WriteAllText(fileStorePath, lineEntry, Encoding.UTF8);
                    status = 1;                   
                }
            }
            else
            {
                status = -1;
            }
            log.LogMethodExit(status);
            return status;
        }

        private string GetFileName(string path, string extension)
        {
            log.LogMethodEntry(path, extension);
            string returnValue = path + DateTime.Now.ToString(FileNameDateFormat) + extension;
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}

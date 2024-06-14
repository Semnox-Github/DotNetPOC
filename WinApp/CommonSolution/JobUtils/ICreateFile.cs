using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Interface for creating file 
    /// </summary>
    public interface ICreateFile
    {
        /// <summary>
        /// Member method with definition to create file and store to path sent
        /// </summary>
        /// <param name="iquery">IQuery Interface passed as a parameter</param>
        /// <param name="fileStorePath">path store the file after successful creation</param>
        /// <returns>Returns 1 on successfull file creation, 0 failure, -1 no records found for the day</returns>
        int CreateFile(IQuery iquery, ref string fileStorePath);
    }
}

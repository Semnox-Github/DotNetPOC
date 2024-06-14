using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// POS or any other application Authenticate manager funtion can be passed through this class
    /// </summary>    
    public delegate bool AuthenticateManagerDelegate(ref int managerId);
}

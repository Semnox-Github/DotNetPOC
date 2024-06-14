using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Delegate that invokes to customized message box the message.
    /// </summary>
    public delegate DialogResult MessageBoxDelegate(string Message, string Title, MessageBoxButtons msgboxButtons = MessageBoxButtons.OK, MessageBoxIcon msgboxIcon = MessageBoxIcon.None);
}

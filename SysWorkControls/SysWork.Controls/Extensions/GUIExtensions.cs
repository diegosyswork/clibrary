using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Controls.Extensions
{
    public static class GUIExtensions
    {
        public static void InvokeIfRequired(this System.Windows.Forms.Control c, Action<System.Windows.Forms.Control> action)
        {
            if (c.InvokeRequired)
            {
                c.Invoke(new Action(() => action(c)));
            }
            else
            {
                action(c);
            }
        }

    }
}

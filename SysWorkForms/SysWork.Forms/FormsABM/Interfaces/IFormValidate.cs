using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Forms.FormsABM.FormsABM.Interfaces
{
    public interface IFormValidate
    {
        void ValidaControl(object sender, CancelEventArgs e);
        void SeteaEventoValidating();
    }
}

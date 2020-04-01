using System.ComponentModel;

namespace SysWork.Forms.FormsABM.FormsABM.Interfaces
{
    public interface IFormValidate
    {
        void ValidaControl(object sender, CancelEventArgs e);
        void SetValidatinEvent();
    }
}

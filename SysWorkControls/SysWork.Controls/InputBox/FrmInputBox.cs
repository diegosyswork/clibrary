using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysWork.Controls.InputBox
{
    internal partial class FrmInputBox : Form
    {
        private int margenVertical = 20;

        public FrmInputBox()
        {
            InitializeComponent();
        }

        private void lblPrompt_SizeChanged(object sender, EventArgs e)
        {
            redimensionar();
        }
        
        private void txtInput_SizeChanged(object sender, EventArgs e)
        {
            redimensionar();
        }

        private void redimensionar() 
        {
            txtInput.Top = lblPrompt.Top + lblPrompt.Height + margenVertical;
            btnAceptar.Top = txtInput.Top + txtInput.Height + (margenVertical );
            btnCancelar.Top = btnAceptar.Top;
            this.Height = btnAceptar.Top + btnAceptar.Height + (margenVertical*3);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}

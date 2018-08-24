using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysWork.Forms.FormsABM.FormsABM.Interfaces
{
    public interface IFormABM
    {
        bool _noLanzarEventosCombos { get; set; }
        bool _noValidarFormulario { get; set; }
        bool _validacionFinal { get; set; }
        void ObtenerDaos();
        void InicializarFormulario();
        void ModoEdicion(bool permiteEdicion);
        void AsignarDatos();
        bool DatosValidos();
        bool ActualizarRegistro();
        bool DatosValidosEliminacion();
        bool EliminarRegistro();
        void BuscaNuevoCodigo();
    }
}

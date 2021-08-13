using System;
using System.Collections.Generic;
using System.Text;

namespace SysWork.Data.GenericDataManager.CodeWriter
{
    // TODO:  Agregar la  posibilidad de crear: Clasico Singleton, DbContext Singleton, Clasico Instanciable, DbContext Instanciable, agregado el 08/03/20021
    public enum EDatamagerStyle
    {
        Singleton,
        SingletonPublicProperties,
        Instantiable
    }

}

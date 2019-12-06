using System.Data;
using System.Dynamic;

namespace SysWork.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Dynamic.DynamicObject" />
    public class DataRecordDynamicWrapper : DynamicObject
    {
        private IDataRecord _dataRecord;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DataRecordDynamicWrapper"/> class.
        /// </summary>
        /// <param name="dataRecord">The data record.</param>
        public DataRecordDynamicWrapper(IDataRecord dataRecord)
        {
            _dataRecord = dataRecord;
        }

        /// <summary>
        /// Proporciona la implementación para las operaciones que obtienen valores de miembro. Las clases derivadas de la <see cref="T:System.Dynamic.DynamicObject" /> clase puede invalidar este método para especificar un comportamiento dinámico para operaciones como obtener un valor para una propiedad.
        /// </summary>
        /// <param name="binder">Proporciona información sobre el objeto que llamó a la operación dinámica. El binder.Name proporciona el nombre del miembro en el que se realiza la operación dinámica. Por ejemplo, para el Console.WriteLine(sampleObject.SampleProperty) instrucción, donde sampleObject es una instancia de la clase derivada de la <see cref="T:System.Dynamic.DynamicObject" /> (clase), binder.Name devuelve "SampleProperty". El binder.IgnoreCase propiedad especifica si el nombre de miembro distingue mayúsculas de minúsculas.</param>
        /// <param name="result">El resultado de la operación get. Por ejemplo, si se llama al método para una propiedad, puede asignar el valor de la propiedad <paramref name="result" />.</param>
        /// <returns>
        /// true si la operación es correcta; de lo contrario, false. Si este método devuelve false, el enlazador en tiempo de ejecución del lenguaje determina el comportamiento. (En la mayoría de los casos, se produce una excepción en tiempo de ejecución).
        /// </returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _dataRecord[binder.Name];
            return result != null;
        }
    }
}

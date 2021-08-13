using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SysWork.Data.Common;
using SysWork.Data.GenericRepository;
using SysWork.Data.Mapping;
using SysWork.Data.Common.ValueObjects;
using InterfaceB2B.Data.Entities;
using System.Data;

namespace InterfaceB2B.Data.Repositories
{
	/// <summary>
	/// This class was created automatically with the RepositoryClassFromDb class.
	/// Inherited from GenericRepository which allows you to perform the following actions: 
	/// Add , 
	/// AddRange , 
	/// DeleteAll ,
	/// DeleteByGenericWhereFilter ,
	/// DeleteById ,
	/// DeleteByLambdaExpressionFilter ,
	/// Exists ,
	/// Find ,
	/// GetAll ,
	/// GetByGenericWhereFilter ,
	/// GetById ,
	/// GetByLambdaExpressionFilter ,
	/// GetDataTableByGenericWhereFilter ,
	/// GetDataTableByLambdaExpressionFilter ,
	/// GetListByGenericWhereFilter ,
	/// GetListByLambdaExpressionFilter ,
	/// RecordCount,
	/// Update,
	/// UpdateRange.
	/// 	/// Additionally, methods were also created for the unique keys.
	/// </summary>

	public class V_MA_ARTICULOSRepository : BaseRepository<V_MA_ARTICULOS>
	{
		public V_MA_ARTICULOSRepository(string connectionString, EDatabaseEngine databaseEngine) : base(connectionString, databaseEngine)
		{

		}

		public V_MA_ARTICULOS GetByCodSegunProveedor(string cuentaProveedor, string codSegunProveedor, IDbTransaction dbTransaction = null)
		{
			codSegunProveedor = codSegunProveedor.Trim();
			cuentaProveedor = cuentaProveedor.Trim();

			return GetByLambdaExpressionFilter(a => a.CODIGOARTPROVEEDOR.Trim() == "", dbTransaction);
		}
	}
}

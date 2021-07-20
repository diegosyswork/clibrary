using SysWork.Data.Common.Attributes;
using SysWork.Data.Mapping;

namespace Demo.SysWork.Data.Entities
{
    [Table(Name = "States")]
    public class State
    {
        /// <summary>
        /// **********************************************************************
        /// 
        /// Esta clase fue generada automaticamente por la clase EntityClassFromDb
        /// 
        /// Fecha: 29/11/2019 17:19:27
        /// 
        /// DataSource: NT-SYSWORK\SQLEXPRESS
        /// InitialCatalog: TEST_SYSWORK_DATA
        /// 
        /// **********************************************************************
        /// </summary>

        [Column(IsPrimaryKey = true)]
        public long IdState { get; set; }
        [Column()]
        public string StateCode { get; set; }
        [Column()]
        public string Description { get; set; }
    }
}

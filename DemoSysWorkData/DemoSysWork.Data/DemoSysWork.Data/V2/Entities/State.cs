using SysWork.Data.GenericRepostory.Attributes;

namespace Demo.SysWork.Data.Entities
{
    [DbTable(Name = "States")]
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

        [DbColumn(IsPrimary = true)]
        public long IdState { get; set; }
        [DbColumn()]
        public string StateCode { get; set; }
        [DbColumn()]
        public string Description { get; set; }
    }
}

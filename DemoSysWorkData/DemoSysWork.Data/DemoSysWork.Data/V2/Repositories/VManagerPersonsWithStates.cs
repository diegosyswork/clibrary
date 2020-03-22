﻿using Demo.SysWork.Data.V2.Entities;
using SysWork.Data.Common;
using SysWork.Data.GenericRepository;

namespace Demo.SysWork.Data.Repositories
{
    public class VManagerPersonsWithStates : BaseViewManager<VW_PersonsWithStates>
    {
        public VManagerPersonsWithStates(string connectionString, EDataBaseEngine dataBaseEngine) : base(connectionString, dataBaseEngine)
        {

        }
    }
}
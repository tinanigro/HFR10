using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Hfr.Database
{
    interface IDataRepository
    {
        void Initialize();
        void Drop();
        void Clear();
    }
}

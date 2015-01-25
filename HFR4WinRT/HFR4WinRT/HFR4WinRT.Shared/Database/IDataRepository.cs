using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace HFR4WinRT.Database
{
    interface IDataRepository
    {
        void Initialize();
        void Drop();
        void Clear();
    }
}

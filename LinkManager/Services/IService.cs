using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkManager.Services
{
    interface IService<T>
    {
        List<T> GetAll();
        T GetById(int id);
        T Add(T element);
        T Edit(T element);
        T Delete(int id);
    }
}

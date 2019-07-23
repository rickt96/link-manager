using System.Collections.Generic;

namespace LinkManager.Services
{
    /// <summary>
    /// Interfaccia per le operazioni CRUD
    /// </summary>
    /// <typeparam name="T"></typeparam>
    interface IService<T>
    {
        List<T> GetAll();
        T GetById(int id);
        T Add(T element);
        T Edit(T element);
        T Delete(int id);
    }
}

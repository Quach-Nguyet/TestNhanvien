using QuanLyNhanVien.Models;
using System;
using System.Collections.Generic;

namespace QuanLyNhanVien.Repository
{
    public interface IRepository<T> where T : BaseEntity
        {
            void Add(T item);
            void Remove(string MaNhanVien);
            void Update(T item);
            T FindByID(string MaNhanVien);
            IEnumerable<T> FindAll();
    }
}

using ELearning.Bl.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearning.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private AppDbContext db;
        public BaseRepository(AppDbContext _db)
        {
            db = _db;
        }
        public void Add(T model)
        {
            db.Set<T>().Add(model);
            SaveData();
        }

        public void Delete(int id)
        {
            var result = db.Set<T>().Find(id);
            if (result != null)
            {
                db.Set<T>().Remove(result!);
            }

            SaveData();
        }

        public IEnumerable<T> GetAll()
        {
            return db.Set<T>().ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await db.Set<T>().ToListAsync();
        }

        public T GetById(int id)
        {
            var result = db.Set<T>().Find(id);
            return result!;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var result = await db.Set<T>().FindAsync(id);
            return result!;
        }

        public void SaveData()
        {
            db.SaveChanges();
        }

        public void Update(T model)
        {

            db.Set<T>().Update(model!);
            SaveData();
        }
    }
}

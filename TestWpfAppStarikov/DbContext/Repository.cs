using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWpfAppStarikov.DbContext
{
    using System.Data.Entity;

    using TestWpfAppStarikov.Models;

    public class Repository
    {
        public static IQueryable<TEntity> Select<TEntity>(DbContext context)
    where TEntity : class
        {
            //ClientContext context = new ClientContext();
            // Здесь мы можем указывать различные настройки контекста,
            // например выводить в отладчик сгенерированный SQL-код
            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));
            // Загрузка данных с помощью универсального метода Set
            return context.Set<TEntity>();
        }

        public static void Insert<TEntity>(TEntity entity, DbContext context) where TEntity : class
        {
            // Настройки контекста
            //using (ClientContext context = new ClientContext())
            //{
                context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

                context.Entry(entity).State = EntityState.Added;
                context.SaveChanges();
            //}
        }

        /// <summary>
        /// Запись нескольких полей в БД
        /// </summary>
        public static void Insert<TEntity>(IEnumerable<TEntity> entities, DbContext context) where TEntity : class
        {
            // Настройки контекста
            //using (ClientContext context = new ClientContext())
            //{

                // Отключаем отслеживание и проверку изменений для оптимизации вставки множества полей
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));


                foreach (TEntity entity in entities) context.Entry(entity).State = EntityState.Added;
                context.SaveChanges();

                context.Configuration.AutoDetectChangesEnabled = true;
                context.Configuration.ValidateOnSaveEnabled = true;
            //}
        }

        public static void Update<TEntity>(TEntity entity, DbContext context)
    where TEntity : class
        {
            //using (ClientContext context = new ClientContext())
            //{
                // Настройки контекста
                context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

                context.Entry<TEntity>(entity).State = EntityState.Modified;
                context.SaveChanges();
            //}
        }

        public static void Delete<TEntity>(TEntity entity, DbContext context)
    where TEntity : class
        {
            // Настройки контекста
            //ClientContext context = new ClientContext();
            context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

            context.Entry<TEntity>(entity).State = EntityState.Deleted;
            context.SaveChanges();
        }
    }
}

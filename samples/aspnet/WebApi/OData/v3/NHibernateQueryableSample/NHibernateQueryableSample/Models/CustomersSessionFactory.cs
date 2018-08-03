using System;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NHibernateQueryableSample.Models
{
    public static class CustomersSessionFactory
    {
        private static Lazy<ISessionFactory> _instance = new Lazy<ISessionFactory>(() =>
            {
                ISessionFactory factory = BuildSessionFactory();
                SeedDatabase(factory);
                return factory;
            });
        private const string _dbFile = "sqlite.db";

        public static ISessionFactory Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private static ISessionFactory BuildSessionFactory()
        {
            return Fluently
                .Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(_dbFile))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<CustomerMap>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            if (File.Exists(_dbFile))
            {
                File.Delete(_dbFile);
            }

            new SchemaExport(config).Create(false, true);
        }

        private static void SeedDatabase(ISessionFactory sessionFactory)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(new Customer { Name = "Raghu", City = "Redmond", Address = "Address Line", State = "WA", Orders = new[] { new Order { Amount = 100, Quantity = 10 } } });
                    session.SaveOrUpdate(new Customer { Name = "Ram", City = "Redmond", Address = "Address Line", State = "WA", Orders = new[] { new Order { Amount = 10, Quantity = 10 } } });
                    transaction.Commit();
                }
                session.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Todo.Tests
{
    public abstract class MockableDbSetWithIQueryable<T> : DbSet<T>, IQueryable<T> where T : class 
    { 
        public abstract IEnumerator<T> GetEnumerator(); 
        public abstract Expression Expression { get; } 
        public abstract Type ElementType { get; } 
        public abstract IQueryProvider Provider { get; } 
    }
}

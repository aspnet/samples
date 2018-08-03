using System.ComponentModel;
using System.Web.OData.Query;
using NHibernate;

namespace System.Web.OData.NHibernate
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class ODataQueryOptionExtensions
    {
        public static IQuery ApplyTo(this ODataQueryOptions query, ISession session)
        {
            string from = "from " + query.Context.ElementClrType.Name + " $it" + Environment.NewLine;

            // convert $filter to HQL where clause.
            WhereClause where = ToFilterQuery(query.Filter);

            // convert $orderby to HQL orderby clause.
            string orderBy = ToOrderByQuery(query.OrderBy);

            // create a query using the where clause and the orderby clause.
            string queryString = from + where.Clause + orderBy;
            IQuery hQuery = session.CreateQuery(queryString);
            for (int i = 0; i < where.PositionalParameters.Length; i++)
            {
                hQuery.SetParameter(i, where.PositionalParameters[i]);
            }

            // Apply $skip.
            hQuery = hQuery.Apply(query.Skip);

            // Apply $top.
            hQuery = hQuery.Apply(query.Top);

            return hQuery;
        }

        private static IQuery Apply(this IQuery query, TopQueryOption topQuery)
        {
            if (topQuery != null)
            {
                query = query.SetMaxResults(topQuery.Value);
            }

            return query;
        }

        private static IQuery Apply(this IQuery query, SkipQueryOption skipQuery)
        {
            if (skipQuery != null)
            {
                query = query.SetFirstResult(skipQuery.Value);
            }

            return query;
        }

        private static string ToOrderByQuery(OrderByQueryOption orderByQuery)
        {
            return NHibernateOrderByBinder.BindOrderByQueryOption(orderByQuery);
        }

        private static WhereClause ToFilterQuery(FilterQueryOption filterQuery)
        {
            return NHibernateFilterBinder.BindFilterQueryOption(filterQuery);
        }
    }
}

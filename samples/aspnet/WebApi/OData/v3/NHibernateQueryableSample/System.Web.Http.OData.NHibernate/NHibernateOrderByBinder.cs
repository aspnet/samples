using System.Text;
using System.Web.Http.OData.Query;
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;

namespace System.Web.Http.OData.NHibernate
{
    public class NHibernateOrderByBinder
    {
        public static string BindOrderByQueryOption(OrderByQueryOption orderByQuery)
        {
            StringBuilder sb = new StringBuilder();

            if (orderByQuery != null)
            {
                sb.Append("order by ");
                foreach (var orderByNode in orderByQuery.OrderByNodes)
                {
                    var orderByPropertyNode = orderByNode as OrderByPropertyNode;

                    if (orderByPropertyNode != null)
                    {
                        sb.Append(orderByPropertyNode.Property.Name);
                        sb.Append(orderByPropertyNode.Direction == OrderByDirection.Ascending ? " asc," : " desc,");
                    }
                    else
                    {
                        throw new ODataException("Only ordering by properties is supported");
                    }
                }
            }

            if (sb[sb.Length - 1] == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}

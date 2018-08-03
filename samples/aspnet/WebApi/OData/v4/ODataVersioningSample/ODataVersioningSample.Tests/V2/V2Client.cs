using System;
using System.Linq;
using ODataVersioningSample.Tests.V2.Default;
using ODataVersioningSample.Tests.V2.ODataVersioningSample.V2.ViewModels;

namespace ODataVersioningSample.Tests.V2
{
    public class V2Client
    {
        private long _lastIndex;

        public void RunTests()
        {
            Console.WriteLine("V2 Client Tests:");
            GetProducts();
            QueryProducts();
            GetProduct();
            GetProductFamily();
            PostProduct();
            LinkProductToFamily();
            PatchProduct();
            DeleteProduct();
        }

        private void GetProducts()
        {
            Console.WriteLine("Get Products");
            Container c = Container.New();
            var people = c.Products.ToList();
            foreach (var p in people)
            {
                Console.WriteLine("\t{0}: {1}", p.ID, p.Name);
            }

            _lastIndex = people.Select(p => p.ID).Max();
        }

        private void QueryProducts()
        {
            Console.WriteLine("Get Products Since 2008/1/1");
            Container c = Container.New();
            var startDate = new DateTime(2008, 1, 1);
            var people = c.Products.Where(p => p.ReleaseDate > new DateTimeOffset(startDate));
            foreach (var p in people)
            {
                Console.WriteLine("\t{0}: {1}", p.ID, p.Name);
            }
        }

        private void GetProduct()
        {
            Console.WriteLine("Get Product");
            Container c = Container.New();
            var product = c.Products.Where(p => p.ID == _lastIndex).Single();
            Console.WriteLine("\t{0} was found", product.Name);
        }

        private void GetProductFamily()
        {
            Console.WriteLine("Get Product Family");
            Container c = Container.New();
            var product = c.Products.Where(p => p.ID == _lastIndex).Single();
            c.LoadProperty(product, "Family");
            Console.WriteLine("\t{0} was found", product.Family.Name);
        }

        private void PostProduct()
        {
            Console.WriteLine("Post Product");
            Container c = Container.New();
            Product product = new Product { Name = "Windows 8" };
            c.AddToProducts(product);
            c.SaveChanges();
            Console.WriteLine("\t{0} was added", product.Name);
            GetProducts();
        }

        private void LinkProductToFamily()
        {
            Console.WriteLine("Link Product to Family");
            Container c = Container.New();
            var product = c.Products.Where(p => p.ID == _lastIndex).Single();
            var family = c.ProductFamilies.Where(p => p.Name.Contains("Windows")).Single();

            c.SetLink(product, "Family", family);
            c.SaveChanges();

            Console.WriteLine("\t{0} was linked to {0} family", product.Name, family.Name);
            GetProducts();
        }

        private void PatchProduct()
        {
            Console.WriteLine("Patch Product");
            Container c = Container.New();
            var product = c.Products.Where(p => p.ID == _lastIndex).Single();
            product.ReleaseDate = new DateTime(2012, 10, 26);
            c.UpdateObject(product);
            c.SaveChanges();
            Console.WriteLine("\t{0}'s release date is changed to {1}", product.Name, product.ReleaseDate);
            GetProducts();
        }

        private void DeleteProduct()
        {
            Console.WriteLine("Delete Product");
            Container c = Container.New();
            var product = c.Products.Where(p => p.ID == _lastIndex).Single();
            c.DeleteObject(product);
            c.SaveChanges();
            Console.WriteLine("\t{0} was deleted", product.Name);
            GetProducts();
        }
    }
}

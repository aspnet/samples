using Microsoft.OData.Client;
using System;
using System.Linq;

namespace ODataClient
{
    /// <summary>
    /// This sample client uses OData Client Code Generator to communicate with ODataServiceSample
    /// </summary>  
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the OData Web Api command line client sample.");
            Console.WriteLine("\tType '?' for options.");

            while (true)
            {
                Console.Write("> ");
                string commmand = Console.ReadLine().ToLower();
                switch (commmand)
                {
                    case "get products":
                        Get_Products();
                        break;

                    case "query products":
                        Query_Products();
                        break;

                    case "get productfamilies":
                        Get_ProductFamilies();
                        break;

                    case "post productfamily":
                        Post_ProductFamily();
                        break;

                    case "delete productfamily":
                        Delete_ProductFamily();
                        break;

                    case "patch productfamily":
                        Patch_ProductFamily();
                        break;

                    case "put productfamily":
                        Put_ProductFamily();
                        break;

                    case "get productfamily.products":
                        Get_ProductFamily_Products();
                        break;

                    case "post productfamily.products":
                        Post_ProductFamily_Products();
                        break;

                    case "get productfamily.supplier":
                        Get_ProductFamily_Supplier();
                        break;

                    case "put product..family":
                        Put_Product_link_Family();
                        break;

                    case "delete product..family":
                        Delete_Product_link_Family();
                        break;

                    case "post productfamily..products":
                        Post_ProductFamily_link_Products();
                        break;

                    case "delete productfamily..products":
                        Delete_ProductFamily_link_Products();
                        break;

                    case "put productfamily..supplier":
                        Put_ProductFamily_link_Supplier();
                        break;

                    case "invoke action":
                        Invoke_Action();
                        break;

                    case "test":
                        Test();
                        break;

                    case "?":
                    case "h":
                    case "help":
                        PrintOptions();
                        break;

                    case "q":
                    case "quit":
                        return;
                    default:
                        HandleUnknownCommand();
                        break;

                }
            }
        }
        private static void Test()
        {
            Get_Products();
            Query_Products();
            Get_ProductFamily_Products();
            Get_ProductFamily_Supplier();

            Get_ProductFamilies();
            Post_ProductFamily();
            Get_ProductFamilies();
            Patch_ProductFamily();
            Get_ProductFamilies();
            Put_ProductFamily();
            Get_ProductFamilies();

            Post_ProductFamily_Products();

            Delete_ProductFamily();
            Get_ProductFamilies();

            Put_Product_link_Family();
            Delete_Product_link_Family();
            
            Post_ProductFamily_link_Products();
            Delete_ProductFamily_link_Products();

            Put_ProductFamily_link_Supplier();
            Invoke_Action();
        }

        private static void Get_Products()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< get products >>");
            foreach (var product in container.Products)
            {
                Console.WriteLine("\t{0}-{1}", product.Id, product.Name);
            }
        }
        private static void Query_Products()
        {
            var container = new Container();

            Console.WriteLine("\n\t<< query products >>");
            Console.WriteLine("\n\tGet top 4 products");
            foreach (var product in container.Products.Take(4))
            {
                Console.WriteLine("\t{0}-{1}", product.Id, product.Name);
            }

            Console.WriteLine("\n\tGet products with name starting with 'Microsoft Office'");
            foreach (var product in container.Products.Where(p => p.Name.StartsWith("Microsoft Office")))
            {
                Console.WriteLine("\t{0}-{1}", product.Id, product.Name);
            }

            Console.WriteLine("\n\tGet all products which expire soon");
            foreach (var product in container.Products
                .Where(prd => prd.SupportedUntil != null)
                .OrderBy(prd => prd.SupportedUntil))
            {

                Console.WriteLine("\t{0}-{1}-{2}", product.SupportedUntil.HasValue ? product.SupportedUntil.Value.ToString() : "N/A", product.Id, product.Name);
            }
        }
        private static void Put_Product_link_Family()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< put product..family >>");
            var product = container.Products.AsEnumerable().First();
            var family = container.ProductFamilies.AsEnumerable().Skip(1).First();
            Console.WriteLine("\tAssociating \n\tProduct: Id={0}, Name={1} \n\tTo\n\tProudctFamily: Id={2}, Name={3}",
                product.Id, product.Name, family.Id, family.Name);

            container.SetLink(product, "Family", family);
            container.SaveChanges();
        }

        private static void Delete_Product_link_Family()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< delete product..family >>");
            var product = container.Products.AsEnumerable().First();
            container.LoadProperty(product, "Family");

            Console.WriteLine("\tUnassociating \n\tProduct: Id={0}, Name={1} \n\tFrom\n\tProudctFamily: Id={2}, Name={3}",
                product.Id, product.Name, product.Family.Id, product.Family.Name);

            container.SetLink(product, "Family", null);
            container.SaveChanges();
        }

        private static void Get_ProductFamilies()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< get productfamilies >>");
            foreach (var productFamily in container.ProductFamilies)
            {
                Console.WriteLine("\t{0}-{1}: {2}", productFamily.Id, productFamily.Name, productFamily.Description);
            }
        }

        private static void Post_ProductFamily()
        {
            var container = new Container();

            Console.WriteLine("\n\t<< post productfamily >>");
            var newData = new ProductFamily
            {
                Id = 4,
                Name = "SQL SERVER",
                Description = "A relational database engine."
            };

            Console.WriteLine("\tCreating ProductFamily with Id={0}, Name={1}, Description={2}", newData.Id, newData.Name, newData.Description);

            container.AddObject("ProductFamilies", newData);
            container.SaveChanges();
        }

        private static void Patch_ProductFamily()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< patch productfamily >>");
            var key = 4;
            var family = container.ProductFamilies.Where(pf => pf.Id == key).AsEnumerable().SingleOrDefault();

            if (family != null)
            {
                Console.WriteLine("\tPatching ProductFamily with Id={0}, Name={1}", family.Id, family.Name);

                family.Description = "Patched Description";
                container.UpdateObject(family);

                container.SaveChanges();
            }
            else
            {
                Console.WriteLine("\tProductFamily with Id '{0}' not found.", key);
            }
        }

        private static void Delete_ProductFamily()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< delete productfamily >>");
            var key = 4;
            ProductFamily family = container.ProductFamilies.Where(pf => pf.Id == key).FirstOrDefault();

            if (family != null)
            {
                Console.WriteLine("\tDeleting ProductFamily with Id={0}, Name={1}", family.Id, family.Name);

                container.DeleteObject(family);
                container.SaveChanges();
            }
            else
            {
                Console.WriteLine("\tProductFamily with Id '{0}' not found.", key);
            }
        }

        private static void Put_ProductFamily()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< put productfamily >>");
            var key = 4;
            var family = container.ProductFamilies.Where(pf => pf.Id == key).FirstOrDefault();
            if (family != null)
            {
                Console.WriteLine("\tUpdating ProductFamily with Id={0}, Name={1}", family.Id, family.Name);

                family.Description = "Updated Description";
                container.UpdateObject(family);

                container.SaveChanges(SaveChangesOptions.ReplaceOnUpdate);
            }
            else
            {
                Console.WriteLine("\tProductFamily with Id '{0}' not found.", key);
            }
        }

        private static void Put_ProductFamily_link_Supplier()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< put productfamily..supplier >>");
            var family = container.ProductFamilies.OrderBy(pf => pf.Id).First();
            var supplier = container.Suppliers.Where(s => s.Id == 1).First();

            Console.WriteLine("\tAssociating \n\tProductFamily: Id={0}, Name={1} \n\tTo\n\tSupplier: Id={2}, Name={3}",
                family.Id, family.Name, supplier.Id, supplier.Name);

            container.SetLink(family, "Supplier", supplier);
            container.SaveChanges();
        }

        private static void Delete_ProductFamily_link_Products()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< delete productfamily..products >>");
            var product = container.Products.OrderBy(p => p.Id).First(); 
            var family = container.ProductFamilies.OrderBy(pf => pf.Id).First();

            Console.WriteLine("\tUnassociating \n\tProduct: Id={0}, Name={1} \n\tTo\n\tProudctFamily: Id={2}, Name={3}",
                product.Id, product.Name, family.Id, family.Name);

            container.DeleteLink(family, "Products", product);
            container.SaveChanges();
        }

        private static void Post_ProductFamily_link_Products()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< post productfamily..products >>");
            var product = container.Products.OrderBy(p => p.Id).First(); // OrderBy need to avoid Take throw.
            var family = container.ProductFamilies.OrderBy(pf => pf.Id).First();

            Console.WriteLine("\tAssociating \n\tProduct: Id={0}, Name={1} \n\tTo\n\tProudctFamily: Id={2}, Name={3}",
                product.Id, product.Name, family.Id, family.Name);

            container.AddLink(family, "Products", product);
            container.SaveChanges();
        }

        private static void Get_ProductFamily_Products()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< get productfamily.products >>");
            var products = container.ProductFamilies.Where(p => p.Id == 3).SelectMany(p => p.Products);
            foreach (var product in products)
            {
                Console.WriteLine("\t{0}-{1}", product.Id, product.Name);
            }
        }

        private static void Get_ProductFamily_Supplier()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< get productfamily.supplier >>");
            var query = container.ProductFamilies.Where(p => p.Id == 1).Select(p => p.Supplier);
            foreach (var supplier in query)
            {
                Console.WriteLine("\t{0}-{1}", supplier.Id, supplier.Name);
            }
        }

        private static void Post_ProductFamily_Products()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< post productfamily.products >>");
            var key = 4;
            ProductFamily family = container.ProductFamilies.Where(pf => pf.Id == key).AsEnumerable().SingleOrDefault();

            var sql2012 = new Product
            {
                Name = "SQL Server 2012",
                ReleaseDate = new DateTime(2012, 3, 6),
                SupportedUntil = new DateTime(2017, 7, 11)
            };
            container.AddRelatedObject(family, "Products", sql2012);

            Console.WriteLine("\tCreating Product with Name={0} under ProductFamily with name {1}", sql2012.Name, family.Name);
            container.SaveChanges();

        }

        private static void Invoke_Action()
        {
            var container = new Container();
            Console.WriteLine("\n\t<< invoke action >>");
            string uri = container.BaseUri.AbsoluteUri + "ProductFamilies(1)/ODataService.Models.CreateProduct";
            var results = container.Execute<int>(new Uri(uri), "POST", true, new BodyOperationParameter("Name", "New Product"));
            var result = results.Single();
            Console.WriteLine("\t" + @"action CreateProduct({{ ""Name"": ""New Product"" }}) returned {0}", result);
        }

        private static void PrintOptions()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("\tget products                   -> Print all the Products.");
            Console.WriteLine("\tquery products                 -> Query Products.");
            Console.WriteLine("\tget productfamilies            -> Print all the ProductFamilies.");
            Console.WriteLine("\tget productfamily.products     -> Print all the Products in the Office family.");
            Console.WriteLine("\tget productfamily.supplier     -> Print the supplier of the MS-DOS family.");
            Console.WriteLine("\tpost productfamily             -> Create productfamily 4 (SQL SERVER).");
            Console.WriteLine("\tpatch productfamily            -> Patch ProductFamily 4 (SQL SERVER).");
            Console.WriteLine("\tput productfamily              -> Replace ProductFamily 4 (SQL SERVER).");
            Console.WriteLine("\tdelete productfamily           -> Delete productfamily 4 (SQL SERVER).");
            Console.WriteLine("\tput product..family            -> Set Product.Family to a ProductFamily.");
            Console.WriteLine("\tdelete product..family         -> Set Product.Family to NULL.");
            Console.WriteLine("\tpost productfamily..products   -> ProductFamily.Products.Add(product).");
            Console.WriteLine("\tdelete productfamily..products -> ProductFamily.Products.Remove(product).");
            Console.WriteLine("\tput productfamily..supplier    -> ProductFamily.Supplier = supplier.");
            Console.WriteLine("\tinvoke action                  -> Invoke the CreateProduct action.");
            Console.WriteLine("\ttest                           -> Run all the above commands");
            Console.WriteLine("\t?                              -> Print Available Commands.");
            Console.WriteLine("\tquit                           -> Quit.");
        }

        private static void HandleUnknownCommand()
        {
            Console.WriteLine("command not recognized, enter '?' for options");
        }
    }
}

using System;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;
using Microsoft.OData.Edm;
using ODataService.Models;

namespace ODataService
{
    /// <summary>
    /// Helper class to build the EdmModels by either explicit or implicit method.
    /// </summary>
    public static class ModelBuilder
    {
        /// <summary>
        /// Get the EdmModel
        /// </summary>
        public static IEdmModel GetEdmModel()
        {
            // build the model by convention
            return GetImplicitEdmModel();
            // or build the model by hand
            // return GetExplicitEdmModel();
        }

        /// <summary>
        /// Generates a model from a few seeds (i.e. the names and types of the entity sets)
        /// by applying conventions.
        /// </summary>
        /// <returns>An implicitly configured model</returns>        
        static IEdmModel GetImplicitEdmModel()
        {
            ODataModelBuilder modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<Product>("Products");
            modelBuilder.EntitySet<ProductFamily>("ProductFamilies");
            modelBuilder.EntitySet<Supplier>("Suppliers");

            var createProduct = modelBuilder.EntityType<ProductFamily>().Action("CreateProduct");
            createProduct.Parameter<string>("Name");
            createProduct.Returns<int>();
            modelBuilder.Namespace = typeof (ProductFamily).Namespace;
            return modelBuilder.GetEdmModel();
        }

        /// <summary>
        /// Generates a model explicitly.
        /// </summary>
        static IEdmModel GetExplicitEdmModel()
        {
            ODataModelBuilder modelBuilder = new ODataModelBuilder();

            var products = modelBuilder.EntitySet<Product>("Products");

            products.HasIdLink(
                entityContext =>
                {
                    object id;
                    entityContext.EdmObject.TryGetPropertyValue("Id", out id);
                    return new Uri(entityContext.Url.CreateODataLink(
                                    new EntitySetPathSegment(entityContext.NavigationSource.Name),
                                    new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(id, ODataVersion.V4))));
                },
                followsConventions: true);

            products.HasEditLink(
                entityContext =>
                {
                    object id;
                    entityContext.EdmObject.TryGetPropertyValue("Id", out id);
                    return new Uri(entityContext.Url.CreateODataLink(
                                    new EntitySetPathSegment(entityContext.NavigationSource.Name),
                                    new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(id, ODataVersion.V4))));
                },
                followsConventions: true);

            var suppliers = modelBuilder.EntitySet<Supplier>("Suppliers");

            suppliers.HasIdLink(
                entityContext =>
                {
                    object id;
                    entityContext.EdmObject.TryGetPropertyValue("Id", out id);
                    return new Uri(entityContext.Url.CreateODataLink(
                                    new EntitySetPathSegment(entityContext.NavigationSource.Name),
                                    new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(id, ODataVersion.V4))));
                },
                followsConventions: true);

            suppliers.HasEditLink(
                entityContext =>
                {
                    object id;
                    entityContext.EdmObject.TryGetPropertyValue("Id", out id);
                    return new Uri(entityContext.Url.CreateODataLink(
                                    new EntitySetPathSegment(entityContext.NavigationSource.Name),
                                    new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(id, ODataVersion.V4))));
                },
                followsConventions: true);

            var families = modelBuilder.EntitySet<ProductFamily>("ProductFamilies");

            families.HasIdLink(
                entityContext =>
                {
                    object id;
                    entityContext.EdmObject.TryGetPropertyValue("Id", out id);
                    return new Uri(entityContext.Url.CreateODataLink(
                                    new EntitySetPathSegment(entityContext.NavigationSource.Name),
                                    new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(id, ODataVersion.V4))));
                },
                followsConventions: true);

            families.HasEditLink(
                entityContext =>
                {
                    object id;
                    entityContext.EdmObject.TryGetPropertyValue("Id", out id);
                    return new Uri(entityContext.Url.CreateODataLink(
                                    new EntitySetPathSegment(entityContext.NavigationSource.Name),
                                    new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(id, ODataVersion.V4)))
                                    );
                },
                followsConventions: true);

            var product = products.EntityType;

            product.HasKey(p => p.Id);
            product.Property(p => p.Name);
            product.Property(p => p.ReleaseDate);
            product.Property(p => p.SupportedUntil);

            modelBuilder.EntityType<RatedProduct>().DerivesFrom<Product>().Property(rp => rp.Rating);

            var address = modelBuilder.ComplexType<Address>();
            address.Property(a => a.City);
            address.Property(a => a.Country);
            address.Property(a => a.State);
            address.Property(a => a.Street);
            address.Property(a => a.ZipCode);

            var supplier = suppliers.EntityType;
            supplier.HasKey(s => s.Id);
            supplier.Property(s => s.Name);
            supplier.ComplexProperty(s => s.Address);

            var productFamily = families.EntityType;
            productFamily.HasKey(pf => pf.Id);
            productFamily.Property(pf => pf.Name);
            productFamily.Property(pf => pf.Description);

            // Create relationships and bindings in one go
            products.HasRequiredBinding(p => p.Family, families);
            families.HasManyBinding(pf => pf.Products, products);
            families.HasOptionalBinding(pf => pf.Supplier, suppliers);
            suppliers.HasManyBinding(s => s.ProductFamilies, families);

            // Create navigation Link builders
            products.HasNavigationPropertiesLink(
                product.NavigationProperties,
                (entityContext, navigationProperty) =>
                {
                    object id;
                    entityContext.EdmObject.TryGetPropertyValue("Id", out id);
                    return new Uri(entityContext.Url.CreateODataLink(
                        new EntitySetPathSegment(entityContext.NavigationSource.Name),
                        new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(id, ODataVersion.V4)),
                        new NavigationPathSegment(navigationProperty.Name)));
                },
                followsConventions: true);

            families.HasNavigationPropertiesLink(
                productFamily.NavigationProperties,
                (entityContext, navigationProperty) =>
                {
                    object id;
                    entityContext.EdmObject.TryGetPropertyValue("Id", out id);
                    return new Uri(entityContext.Url.CreateODataLink(
                        new EntitySetPathSegment(entityContext.NavigationSource.Name),
                        new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(id, ODataVersion.V4)),
                        new NavigationPathSegment(navigationProperty.Name)));
                },
                followsConventions: true);

            suppliers.HasNavigationPropertiesLink(
                supplier.NavigationProperties,
                (entityContext, navigationProperty) =>
                {
                    object id;
                    entityContext.EdmObject.TryGetPropertyValue("Id", out id);
                    return new Uri(entityContext.Url.CreateODataLink(
                        new EntitySetPathSegment(entityContext.NavigationSource.Name),
                        new KeyValuePathSegment(ODataUriUtils.ConvertToUriLiteral(id, ODataVersion.V4)),
                        new NavigationPathSegment(navigationProperty.Name)));
                },
                followsConventions: true);

            ActionConfiguration createProduct = productFamily.Action("CreateProduct");
            createProduct.Parameter<string>("Name");
            createProduct.Returns<int>();

            modelBuilder.Namespace = typeof (ProductFamily).Namespace;
            return modelBuilder.GetEdmModel();
        }
    }
}

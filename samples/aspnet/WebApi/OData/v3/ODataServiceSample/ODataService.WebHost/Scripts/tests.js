if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
          ? args[number]
          : match;
        });
    };
}
QUnit.config.reorder = false;

asyncTest("get products", function () {
    expect(1);

    OData.read(
      "/Products",
      function (data) {
          ok(data.results.length > 0);
          console.log("get products");
          $.each(data.results, function (index, product) {
              "\t{0}-{1}".format(product.ID, product.Name);
          });
          start();
      }
    );
});

module("query products");
asyncTest("Get top 4 products", function () {
    expect(1);

    OData.read(
      "/Products?$top=4",
      function (data) {
          ok(data.results.length == 4);
          console.log("Get top 4 products");
          $.each(data.results, function (index, product) {
              console.log("\t" + product.ID + "-" + product.Name);
          });
          start();
      }
    );
});
asyncTest("Get products with name starting with 'Microsoft Office'", function () {
    expect(1);

    OData.read(
      "/Products?$filter=startswith(Name, 'Microsoft Office')",
      function (data) {
          ok(data.results.length > 0);
          console.log("Get products with name starting with 'Microsoft Office'");
          $.each(data.results, function (index, product) {
              console.log("\t" + product.ID + "-" + product.Name);
          });
          start();
      }
    );
});
asyncTest("Get all products which expire soon", function () {
    expect(1);

    OData.read(
      "/Products?$filter=SupportedUntil ne null&$orderby=SupportedUntil",
      function (data) {
          ok(data.results.length > 0);
          console.log("Get all products which expire soon");
          $.each(data.results, function (index, product) {
              console.log("\t" + product.SupportedUntil + "-" + product.ID + "-" + product.Name);
          });
          start();
      }
    );
});

module();
var getProducts = function (expectedCount) {
    OData.read(
      "/ProductFamilies",
      function (data) {
          ok(data.results.length == expectedCount);
          console.log("get productfamilies");
          $.each(data.results, function (index, family) {
              console.log("\t" + family.ID + "-" + family.Name + ":" + family.Description);
          });
          start();
      }
    );
};
asyncTest("get productfamilies", function () {
    expect(1);
    getProducts(3);
});

asyncTest("get productfamily.products", function () {
    expect(1);

    OData.read(
      "/ProductFamilies(3)/Products",
      function (data) {
          ok(data.results.length > 0);
          console.log("get productfamily.products");
          $.each(data.results, function (index, product) {
              console.log("\t" + product.ID + "-" + product.Name);
          });
          start();
      }
    );
});

asyncTest("get productfamily.supplier", function () {
    expect(1);

    OData.read(
      "/ProductFamilies(1)/Supplier",
      function (supplier) {
          ok(supplier);
          console.log("get productfamily.supplier");
          console.log("\t" + supplier.ID + "-" + supplier.Name);
          start();
      }
    );
});

asyncTest("post productfamily", function () {
    expect(2);

    OData.request({
        requestUri: "/ProductFamilies",
        method: "POST",
        data: { ID: 4, Name: "SQL Server", Description: "A relational database engine." }
    }, function (sql) {
        ok(sql);
        console.log("post productfamily");
        console.log("\tCreating ProductFamily with Id={0}, Name={1}, Description={2}".format(sql.ID, sql.Name, sql.Description));
        getProducts(4);
    }, function (err) {
        console.log(err.message);
        ok(false);
        start();
    }
    );
});

asyncTest("patch productfamilies", function () {
    expect(2);

    console.log("patch productfamilies");
    OData.read("/ProductFamilies(4)",
        function (family) {
            if (family) {
                console.log("\tPatching ProductFamily with Id={0}, Name={1}".format(family.ID, family.Name));
                OData.request({
                    requestUri: "/ProductFamilies(4)",
                    method: "PATCH",
                    data: { Description: "Patched Description" }
                }, function () {
                    ok(true);
                    getProducts(4);
                }, function (err) {
                    console.log(err.message);
                    ok(false);
                    start();
                });
            }
            else {
                console.log("\tProductFamily with Id '{0}' not found.".format(4));
                start();
            }
        });
});

asyncTest("put productfamilies", function () {
    expect(2);

    console.log("put productfamilies");
    OData.read("/ProductFamilies(4)",
        function (family) {
            if (family) {
                console.log("\tUpdating ProductFamily with Id={0}, Name={1}".format(family.ID, family.Name));
                family.Description = "Updated Description";
                OData.request({
                    requestUri: "/ProductFamilies(4)",
                    method: "PUT",
                    data: { ID: family.ID, Name: family.Name, Description: family.Description }
                }, function () {
                    ok(true);
                    getProducts(4);
                }, function (err) {
                    console.log(err.message);
                    ok(false);
                    start();
                });
            }
            else {
                console.log("\tProductFamily with Id '{0}' not found.".format(4));
                start();
            }
        });
});

asyncTest("post productfamily.products", function () {
    expect(1);

    console.log("post productfamily.products");
    OData.request({
        requestUri: "/ProductFamilies(4)/Products",
        method: "POST",
        data: { Name: "SQL Server 2012", ReleaseDate: new Date(2012, 3, 6), SupportedUntil: new Date(2017, 7, 11) }
    }, function (product) {
        ok(product);
        console.log("post product");
        console.log("\tCreating Product with Id={0}, Name={1}".format(product.ID, product.Name));
        start();
    }, function (err) {
        console.log(err.message);
        ok(false);
        start();
    });
});

asyncTest("delete productfamilies", function () {
    expect(2);

    console.log("delete productfamilies");
    OData.read("/ProductFamilies(4)",
        function (family) {
            if (family) {
                console.log("\tDeleting ProductFamily with Id={0}, Name={1}".format(family.ID, family.Name));
                OData.request({
                    requestUri: "/ProductFamilies(4)",
                    method: "DELETE"
                }, function () {
                    ok(true);
                    getProducts(3);
                }, function (err) {
                    console.log(err.message);
                    ok(false);
                    start();
                });
            }
            else {
                console.log("\tProductFamily with Id '{0}' not found.".format(4));
                start();
            }
        });
});

asyncTest("put product..family", function () {
    expect(3);

    console.log("put product..family");
    OData.read("/Products?$top=1",
        function (data) {
            ok(data.results.length == 1);
            var product = data.results[0];
            OData.read("/ProductFamilies?$skip=1&$top=1",
                function (data) {
                    ok(data.results.length == 1);
                    var family = data.results[0];
                    console.log("\tAssociating \n\tProduct: Id={0}, Name={1} \n\tTo\n\tProudctFamily: Id={2}, Name={3}".format(
                        product.ID, product.Name, family.ID, family.Name));

                    OData.request({
                        requestUri: "/Products({0})/$links/Family".format(product.ID),
                        method: "PUT",
                        data: { url: family.__metadata.id }
                    }, function () {
                        ok(true);
                        start();
                    }, function (err) {
                        console.log(err.message);
                        ok(false);
                        start();
                    });
                });
        });
});

asyncTest("delete product..family", function () {
    expect(3);

    console.log("delete product..family");
    OData.read("/Products?$top=1",
        function (data) {
            ok(data.results.length == 1);
            var product = data.results[0];
            OData.read("/Products({0})/Family".format(product.ID),
                function (family) {
                    ok(family);
                    console.log("\tUnassociating \n\tProduct: Id={0}, Name={1} \n\tFrom\n\tProudctFamily: Id={2}, Name={3}".format(
                        product.ID, product.Name, family.ID, family.Name));

                    OData.request({
                        requestUri: "/Products({0})/$links/Family".format(product.ID),
                        method: "DELETE"
                    }, function () {
                        ok(true);
                        start();
                    }, function (err) {
                        console.log(err.message);
                        ok(false);
                        start();
                    });
                });
        });
});

asyncTest("post productfamily..products", function () {
    expect(3);

    console.log("post productfamily..products");
    OData.read("/Products?$orderby=ID&$top=1",
        function (data) {
            ok(data.results.length == 1);
            var product = data.results[0];
            OData.read("/ProductFamilies?$orderby=ID&$top=1",
                function (data) {
                    ok(data.results.length == 1);
                    var family = data.results[0];
                    console.log("\tAssociating \n\tProduct: Id={0}, Name={1} \n\tTo\n\tProudctFamily: Id={2}, Name={3}".format(
                        product.ID, product.Name, family.ID, family.Name));

                    OData.request({
                        requestUri: "/ProductFamilies({0})/$links/Products".format(family.ID),
                        method: "POST",
                        data: {
                            url: product.__metadata.id
                        }
                    }, function () {
                        ok(true);
                        start();
                    }, function (err) {
                        console.log(err.message);
                        ok(false);
                        start();
                    });
                });
        });
});

asyncTest("delete productfamily..products", function () {
    expect(3);

    console.log("delete productfamily..products");
    OData.read("/Products?$orderby=ID&$top=1",
        function (data) {
            ok(data.results.length == 1);
            var product = data.results[0];
            OData.read("/ProductFamilies?$orderby=ID&$top=1",
                function (data) {
                    ok(data.results.length == 1);
                    var family = data.results[0];
                    console.log("\tUnassociating \n\tProduct: Id={0}, Name={1} \n\tTo\n\tProudctFamily: Id={2}, Name={3}".format(
                        product.ID, product.Name, family.ID, family.Name));

                    OData.request({
                        requestUri: "/ProductFamilies({0})/$links/Products({1})".format(family.ID, product.ID),
                        method: "DELETE"
                    }, function () {
                        ok(true);
                        start();
                    }, function (err) {
                        console.log(err.message);
                        ok(false);
                        start();
                    });
                });
        });
});

asyncTest("put productfamily..supplier", function () {
    expect(3);

    console.log("put productfamily..supplier");
    OData.read("/Suppliers?$orderby=ID&$top=1",
        function (data) {
            ok(data.results.length == 1);
            var supplier = data.results[0];
            OData.read("/ProductFamilies?$orderby=ID&$top=1",
                function (data) {
                    ok(data.results.length == 1);
                    var family = data.results[0];
                    console.log("\tAssociating \n\tProductFamily: Id={0}, Name={1} \n\tTo\n\tSupplier: Id={2}, Name={3}".format(
                        family.ID, family.Name, supplier.ID, supplier.Name));

                    OData.request({
                        requestUri: "/ProductFamilies({0})/$links/Supplier".format(family.ID),
                        method: "PUT",
                        data: {
                            url: supplier.__metadata.id
                        }
                    }, function () {
                        ok(true);
                        start();
                    }, function (err) {
                        console.log(err.message);
                        ok(false);
                        start();
                    });
                });
        });
});

asyncTest("invoke action", function () {
    expect(1);

    console.log("invoke action");
    OData.request({
        requestUri: "/ProductFamilies(1)/CreateProduct",
        method: "POST",
        data: { Name: "New Product" }
    }, function (data) {
        ok(true);
        console.log("\taction CreateProduct({ \"Name\": \"New Product\" }) returned {0}".format(data.value));
        start();
    }, function(err) {
        ok(false);
        start();
    });
});



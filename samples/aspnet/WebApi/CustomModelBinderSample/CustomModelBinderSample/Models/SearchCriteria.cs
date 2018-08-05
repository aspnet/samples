using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomModelBinderSample.Models
{
    public class SearchCriteria
    {
        public SearchCriteria()
        {
            // NOTE:
            //  1.  Here we are instantiating the list because when a request like
            //      "/api/products/search?searchterm=contoso" is made, Web API does not
            //      create a default instance. If you would like to avoid null checks for
            //      this property in other parts of code in the application, you can instantiate like below.
            //  2.  You could also modify 'EmptyCollectionModelBinder' to create a default instance instead and
            //      and avoid the following instantiation.
            this.Categories = new List<ProductCategoryType>();
        }

        public List<ProductCategoryType> Categories { get; set; }

        [Required]
        public string SearchTerm { get; set; }
    }
}

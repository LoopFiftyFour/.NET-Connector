using Loop54.Model;
using Loop54.Model.Response;
using Loop54.Test.AspNetMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loop54.Test.AspNetMvc
{
    public static class ModelUtils
    {
        public static FacetViewModel CreateFacet(Facet responseFacet)
        {
            if (responseFacet.Type == FacetType.Distinct)
            {
                DistinctFacet dist = responseFacet.AsDistinct();
                return new DistinctFacetViewModel
                {
                    Name = dist.Name,
                    Options = dist.Items.ToDictionary(key => key.GetItem<string>(), value => value.Count)
                };
            }
            else
            {
                RangeFacet range = responseFacet.AsRange();
                return new RangeFacetViewModel
                {
                    Name = range.Name,
                    Min = range.GetMin<string>(),
                    Max = range.GetMax<string>()
                };
            }
        }

        public static IList<ProductViewModel> GetViewModelFromEntities(IList<Entity> entities)
        {
            return entities.Select(e => new ProductViewModel
            {
                ImageUrl = e.GetAttributeValueOrDefault<string>("ImageUrl"),
                Name = e.GetAttributeValueOrDefault<string>("Title"),
                Price = e.GetAttributeValueOrDefault<double>("Price"),
                Manufacturer = e.GetAttributeValueOrDefault<string>("Manufacturer"),
                Category = e.GetAttributeValueOrDefault<string>("Category")
            }).ToList();
        }
    }
}

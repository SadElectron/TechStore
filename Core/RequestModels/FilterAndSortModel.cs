using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestModels;

public class FilterAndSortModel
{
    public string sort { get; set; } = string.Empty;
    public string sortValue { get; set; } = string.Empty ;
    public List<ProductFilterModel> filters { get; set; } = new List<ProductFilterModel>();
    
}

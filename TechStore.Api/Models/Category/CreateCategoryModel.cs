using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Api.Models.Category;

public class CreateCategoryModel
{
    public string CategoryName { get; set; } = default!;
}

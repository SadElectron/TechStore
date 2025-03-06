using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Api.Models.Abstract;

namespace TechStore.Api.Models.Category;

public class CreateCategoryModel : IValidationModel
{
    public string CategoryName { get; set; } = default!;
}

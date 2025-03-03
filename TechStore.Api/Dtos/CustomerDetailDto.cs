using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Api.Dtos;

public class CustomerDetailDto
{
    public required string PropName { get; set; }
    public required string PropValue { get; set; }
}

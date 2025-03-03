using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Api.Dtos;

public class CustomerImageDto
{
    public required byte[] File { get; set; }
}

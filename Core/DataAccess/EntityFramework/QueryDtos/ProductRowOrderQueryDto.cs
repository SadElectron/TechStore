using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFramework.QueryDtos;

public class ProductRowOrderQueryDto
{
    public Guid Id { get; set; }
    public double RowOrder { get; set; }
}

using Core.Entities.Concrete;
using Core.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Results;
public record EntityDeleteResult(bool IsSuccessful, string Message);
public record EntityCreateResult<T>(bool IsSuccessful, T? Entity, string Message = "");
public record EntityUpdateResult<T>(bool IsSuccessful, T? Entity, string Message = "");





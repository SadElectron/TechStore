using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Api.Dtos;

public record UserDto(string Email, string UserName, string Password);

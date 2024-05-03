using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Validation
{
    public class CPUValidator: AbstractValidator<CPU>
    {
        public CPUValidator()
        {
            RuleFor(c => c.Brand).NotEmpty().WithMessage("Brand is required.");
            RuleFor(c => c.ModelName).NotEmpty().WithMessage("Model Name is required.");
            RuleFor(c => c.CoreCount).GreaterThan(0).WithMessage("Core Count must be greater than 0.");
            RuleFor(c => c.ThreadCount).GreaterThan(0).WithMessage("Thread Count must be greater than 0.");
            RuleFor(c => c.BaseClockSpeed).NotEmpty().WithMessage("Base Clock Speed is required.");
            RuleFor(c => c.MaxTurboFrequency).NotEmpty().WithMessage("Max Turbo Frequency is required.");
            RuleFor(c => c.CacheSize).NotEmpty().WithMessage("Cache Size is required.");
            RuleFor(c => c.SocketType).NotEmpty().WithMessage("Socket Type is required.");
            RuleFor(c => c.IntegratedGraphics).NotEmpty().WithMessage("Integrated Graphics is required.");
            RuleFor(c => c.MemorySupport).NotEmpty().WithMessage("Memory Support is required.");
            RuleFor(c => c.Tdp).NotEmpty().WithMessage("TDP is required.");
            RuleFor(c => c.LaunchDate).NotEmpty().WithMessage("Launch Date is required.");
            RuleFor(c => c.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }
    }
}

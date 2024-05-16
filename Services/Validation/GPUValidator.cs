using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Validation
{
    public class GPUValidator : AbstractValidator<GPU>
    {
        public GPUValidator()
        {
            RuleFor(gpu => gpu.Brand).NotEmpty().WithMessage("Brand is required.");
            RuleFor(gpu => gpu.ModelName).NotEmpty().WithMessage("Model name is required.");
            RuleFor(gpu => gpu.GraphicEngine).NotEmpty().WithMessage("Graphic engine is required.");
            RuleFor(gpu => gpu.VramSize).NotEmpty().WithMessage("VRAM size is required.");
            RuleFor(gpu => gpu.VramType).NotEmpty().WithMessage("VRAM type is required.");
            RuleFor(gpu => gpu.BaseClockSpeed).NotEmpty().WithMessage("Base clock speed is required.");
            RuleFor(gpu => gpu.BoostClockSpeed).NotEmpty().WithMessage("Boost clock speed is required.");
            RuleFor(gpu => gpu.MemoryBusWidth).NotEmpty().WithMessage("Memory bus width is required.");
            RuleFor(gpu => gpu.Tdp).NotEmpty().WithMessage("TDP is required.");
            RuleFor(gpu => gpu.PowerConnectors).NotEmpty().WithMessage("Power connectors information is required.");
            RuleFor(gpu => gpu.Interface).NotEmpty().WithMessage("Interface information is required.");
            RuleFor(gpu => gpu.DisplayPorts).NotEmpty().WithMessage("Display ports information is required.");
            RuleFor(gpu => gpu.LaunchDate).NotEmpty().WithMessage("Launch date is required.");
            RuleFor(gpu => gpu.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
        }

    }
}

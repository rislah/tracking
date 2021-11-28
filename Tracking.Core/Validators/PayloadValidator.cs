using System;
using FluentValidation;
using Tracking.Core.Models;

namespace Tracking.Core.Validators
{
    public class PayloadValidator : AbstractValidator<Payload>
    {
        public PayloadValidator()
        {
            Console.WriteLine("called");;
        }
    }
}
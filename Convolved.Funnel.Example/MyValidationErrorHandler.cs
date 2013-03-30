using System;
using Convolved.Funnel.Tasks;
using Convolved.Funnel.Validation;

namespace Convolved.Funnel.Example
{
    public class MyValidationErrorHandler : ErrorHandler<ValidationError>
    {
        public override void Handle(ValidationError error)
        {
            throw new NotImplementedException();
        }
    }
}
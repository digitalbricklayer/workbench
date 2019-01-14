using System;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public sealed class BucketModelItemViewModel : ModelItemViewModel
    {
        public BucketModelItemViewModel(Model theModel)
            : base(theModel)
        {
        }

        public override void Edit()
        {
            throw new NotImplementedException();
        }
    }
}

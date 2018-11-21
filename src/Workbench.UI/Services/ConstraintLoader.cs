using System;
using System.Diagnostics;
using Caliburn.Micro;
using Workbench.Core.Models;
using Workbench.ViewModels;

namespace Workbench.Services
{
    /// <summary>
    /// Maps a constraint model into a view model.
    /// </summary>
    public sealed class ConstraintLoader
    {
        private readonly IWindowManager _windowManager;

        public ConstraintLoader(IWindowManager theWindowManager)
        {
            _windowManager = theWindowManager;
        }

        public ConstraintModelItemViewModel MapFrom(ConstraintModel theConstraintModel)
        {
            Debug.Assert(theConstraintModel.HasIdentity);

            switch (theConstraintModel)
            {
                case ExpressionConstraintModel expressionConstraint:
                    return new ExpressionConstraintModelItemViewModel(expressionConstraint, _windowManager);

                case AllDifferentConstraintModel allDifferentConstraint:
                    return new AllDifferentConstraintModelItemViewModel(allDifferentConstraint, _windowManager);

                default:
                    throw new NotSupportedException("Error loading constraint. Unknown constraint type");
            }
        }
    }
}
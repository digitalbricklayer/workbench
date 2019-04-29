using System;
using Caliburn.Micro;
using Workbench.Core.Models;

namespace Workbench.ViewModels
{
    public class SolutionStatsPanelViewModel : Screen
    {
        private TimeSpan _duration;

        /// <summary>
        /// Gets or sets the time taken to create the solution.
        /// </summary>
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                NotifyOfPropertyChange();
            }
        }

        public void BindTo(SolutionModel theSolution)
        {
            Duration = theSolution.Duration;
        }
    }
}

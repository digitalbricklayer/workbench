﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DynaApp.ViewModels
{
    /// <summary>
    /// Abstract base for view-model classes that need to implement INotifyPropertyChanged.
    /// </summary>
    public abstract class AbstractViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Event raised to indicate that a property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
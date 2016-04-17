using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Wkiro.ImageClassification.Infrastructure.Wpf
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDataErrorInfo, IDisposable
    {
        #region INotifyPropertyChanged members

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IDataErorInfo members
        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <value>The error.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public virtual string Error
        {
            get { throw new System.NotImplementedException(); }
        }
        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.String.</returns>
        public virtual string this[string propertyName] => string.Empty;

        #endregion

        public virtual void Dispose()
        {

        }
    }
}

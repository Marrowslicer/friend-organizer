using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using FriendOrganizer.UI.ViewModel;

namespace FriendOrganizer.UI.Wrapper
{
    public class NotifyDataErrorInfoBase : ViewModelBase, INotifyDataErrorInfo
    {
        private Dictionary<string, List<string>> m_errorsByPropertyName =
            new Dictionary<string, List<string>>();

        public bool HasErrors => m_errorsByPropertyName.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return m_errorsByPropertyName.ContainsKey(propertyName)
                ? m_errorsByPropertyName[propertyName]
                : null;
        }

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            base.OnPropertyChanged(nameof(HasErrors));
        }

        protected void AddError(string propertyName, string error)
        {
            if (!m_errorsByPropertyName.ContainsKey(propertyName))
            {
                m_errorsByPropertyName[propertyName] = new List<string>();
            }

            if (!m_errorsByPropertyName[propertyName].Contains(error))
            {
                m_errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if (m_errorsByPropertyName.ContainsKey(propertyName))
            {
                m_errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;

namespace Autofire.Support.Utils.ViewModel
{
    public abstract class AbstractModalViewModel<T> : AbstractViewModel, IDataErrorInfo
    {
        public delegate void OnAcceptedHandler(T viewModel);

        public T Content
        {
            get
            { return GetContent(); }
        }

        public abstract T GetContent();

        public void ShowModal(OnAcceptedHandler handler)
        {
            var view = GetView();
            DialogHost.Show(view, GetCallback(handler));
        }

        private DialogClosingEventHandler GetCallback(OnAcceptedHandler handler)
        {
            return (object sender, DialogClosingEventArgs args) =>
            {
                if (args.Parameter != null && (bool)args.Parameter)
                {
                    handler(Content);
                }
            };
        }

        private UIElement GetView()
        {
            var view = ViewLocator.LocateForModel(Content, null, null);
            ViewModelBinder.Bind(this, view, null);
            return view;
        }

        private ISet<string> invalidProps = new HashSet<string>();

        public bool CanExecute
        {
            get
            {
                return Data == 0;
            }
        }

        public short data;
        public short Data
        {
            get
            {
                return data;
            }
            set
            {
                Set(ref data, value, nameof(CanExecute));
            }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return OnValidate(propertyName); }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        protected virtual string OnValidate(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            string error = string.Empty;
            var value = this.GetType().GetProperty(propertyName).GetValue(this, null);
            var results = new List<ValidationResult>();

            var context = new ValidationContext(this, null, null) { MemberName = propertyName };

            var result = Validator.TryValidateProperty(value, context, results);

            if (!result)
            {
                invalidProps.Add(propertyName);
                Data--;
                var validationResult = results.First();
                return validationResult.ErrorMessage;
            }
            else
            {
                if (invalidProps.Contains(propertyName))
                {
                    invalidProps.Remove(propertyName);
                    Data++;
                }
                return error;
            }
        }
    }
}

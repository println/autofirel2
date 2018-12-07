using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Caliburn.Micro;

namespace Autofire.Support.Utils.ViewModel.Reactive
{
    public abstract class AbstractReactiveViewModel : AbstractViewModel
    {

        public static IEventAggregator EventAggregator { get; set; }

        protected virtual bool Save<T>(object target, Expression<Func<T>> objectPropertyExpression, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (base.Set<T>(target, objectPropertyExpression, newValue, propertyName))
            {
                PublishSave(propertyName);
                return true;
            }
            return false;
        }

        protected virtual bool Save<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (base.Set<T>(ref field, newValue, propertyName))
            {
                PublishSave(propertyName);
                return true;
            }
            return false;
        }

        protected virtual bool Notify<T>(object target, Expression<Func<T>> objectPropertyExpression, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (base.Set<T>(target, objectPropertyExpression, newValue, propertyName))
            {
                PublishNotify(propertyName);
                return true;
            }
            return false;
        }

        protected virtual bool Notify<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (base.Set<T>(ref field, newValue, propertyName))
            {
                PublishNotify(propertyName);
                return true;
            }
            return false;
        }

        private void PublishSave(string propertyName)
        {
            EventAggregator.PublishOnCurrentThread(new OnSaveMessage()
            {
                PropertyName = propertyName,
                Type = this.GetType()
            });
        }

        private void PublishNotify(string propertyName)
        {
            EventAggregator.PublishOnCurrentThread(new OnNotifyMessage()
            {
                PropertyName = propertyName,
                Type = this.GetType()
            });
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Caliburn.Micro;

namespace Autofire.Support.Utils.ViewModel
{
    public abstract class AbstractNotifyPropertyChanged : PropertyChangedBase
    {
        protected virtual bool Set<T>(object target, Expression<Func<T>> objectPropertyExpression, T newValue, [CallerMemberName] string propertyName = null)
        {
            var expr = (MemberExpression)objectPropertyExpression.Body;
            var prop = (PropertyInfo)expr.Member;
            var oldValue = (T)prop.GetValue(target, null);

            var changed = !EqualityComparer<T>.Default.Equals(oldValue, newValue);

            if (changed)
            {
                prop.SetValue(target, newValue, null);
                NotifyOfPropertyChange(propertyName);
            }

            return changed;
        }

        public override bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            NotifyOfPropertyChange(propertyName);
            return true;
        }
    }
}

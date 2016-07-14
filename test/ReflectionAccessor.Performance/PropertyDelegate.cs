using System;
using System.Reflection;

namespace ReflectionAccessor.Performance
{
    public interface IPropertyDelegate
    {
        object GetValue(object instance);
        void SetValue(object instance, object value);
    }

    public class PropertyDelegate<TEntity, TValue> : IPropertyDelegate
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly Lazy<Func<TEntity, TValue>> _getterDelegate;
        private readonly Lazy<Action<TEntity, TValue>> _setterDelegate;

        public PropertyDelegate(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
            _getterDelegate = new Lazy<Func<TEntity, TValue>>(() => (Func<TEntity, TValue>)_propertyInfo.GetMethod.CreateDelegate(typeof(Func<TEntity, TValue>)));
            _setterDelegate = new Lazy<Action<TEntity, TValue>>(() => (Action<TEntity, TValue>)_propertyInfo.SetMethod.CreateDelegate(typeof(Action<TEntity, TValue>)));
        }

        public object GetValue(object instance)
        {
            return _getterDelegate.Value.Invoke((TEntity)instance);
        }

        public void SetValue(object instance, object value)
        {
            _setterDelegate.Value.Invoke((TEntity)instance, (TValue)value);
        }
    }
}
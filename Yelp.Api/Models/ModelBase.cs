﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;
using Newtonsoft.Json;

namespace Yelp.Api.Models
{
    public abstract class TrackedChangesModelBase : ModelBase
    {
        private List<string> _changedProperties = new List<string>();
        
        internal void ClearPropertiesChangedList()
        {
            _changedProperties.Clear();
        }

        public Dictionary<string, object> GetChangedProperties()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            foreach (var propertyName in _changedProperties)
            {
                PropertyInfo pi = this.GetType().GetRuntimeProperty(propertyName);

                var jsonProp = pi.CustomAttributes.FirstOrDefault(f => f.AttributeType.GetTypeInfo() == typeof(JsonPropertyAttribute).GetTypeInfo());
                if (jsonProp != null && jsonProp.ConstructorArguments.Any())
                {
                    var argument = jsonProp.ConstructorArguments.FirstOrDefault();
                    var name = argument.Value.ToString().Replace("\"", "");
                    if (!string.IsNullOrEmpty(name))
                    {
                        var value = pi.GetValue(this);
                        if (value != null)
                            dic.Add(name, value);
                    }
                }
                else
                {
                    var value = pi.GetValue(this);
                    if (value != null)
                        dic.Add(propertyName, value);
                }
            }

            return dic;
        }

        protected internal override void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.NotifyPropertyChanged(propertyName);
            if (_changedProperties.Contains(propertyName) == false)
                _changedProperties.Add(propertyName);
        }
    }

    public abstract class ModelBase : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Data Binding

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (System.Collections.Generic.EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
            else
            {
                storage = value;
                this.NotifyPropertyChanged(propertyName);
                return true;
            }
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Optional name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected internal virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <typeparam name="T">Type of the property in the expression.</typeparam>
        /// <param name="property">Expression to retrieve the property. Example: () => this.FirstName</param>
        protected void NotifyPropertyChanged<T>(Expression<Func<T>> property)
        {
            var propertyName = this.GetPropertyName<T>(property);
            this.NotifyPropertyChanged(propertyName);
        }

        /// <summary>
        /// Gets the string name of a property expression.
        /// </summary>
        /// <typeparam name="T">Type of the property in the expression.</typeparam>
        /// <param name="property">Expression to retrieve the property. Example: () => this.FirstName</param>
        /// <returns>String value representing the property name.</returns>
        private string GetPropertyName<T>(Expression<Func<T>> property)
        {
            MemberExpression memberExpression = this.GetMememberExpression<T>(property);
            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Gets the MemberExpression from a property expression.
        /// </summary>
        /// <typeparam name="T">Type of the property in the expression.</typeparam>
        /// <param name="property">Expression to retrieve the property. Example: () => this.FirstName</param>
        /// <returns>MemberExpression instance presenting the property expression.</returns>
        private MemberExpression GetMememberExpression<T>(Expression<Func<T>> property)
        {
            var lambda = (LambdaExpression)property;

            MemberExpression memberExpression;
            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else
                memberExpression = (MemberExpression)lambda.Body;

            return memberExpression;
        }

        /// <summary>
        /// Retrieves a PropertyInfo object representing the property in the specified expression.
        /// </summary>
        /// <typeparam name="T">Type of the property in the expression.</typeparam>
        /// <param name="property">Expression to retrieve the property. Example: () => this.FirstName</param>
        /// <returns>PropertyInfo object of the expression property else null if not found.</returns>
        protected internal PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> property)
        {
            if (property != null && property.Body is MemberExpression)
            {
                var mex = (MemberExpression)property.Body;
                if (mex != null && mex.Member is PropertyInfo)
                    return mex.Member as PropertyInfo;
            }
            return null;
        }

        ///// <summary>
        ///// Sets a PropertyInfo object with passed in value.
        ///// </summary>
        ///// <param name="pi">PropertyInfo instance representing the property that needs to be set.</param>
        ///// <param name="value">Value to set to the specified PropertyInfo instance.</param>
        //protected internal void SetPropertyValue(PropertyInfo pi, object value)
        //{
        //    if (pi != null && pi.CanWrite)
        //        pi.SetValue(this, value, null);
        //}

        #endregion
    }
}
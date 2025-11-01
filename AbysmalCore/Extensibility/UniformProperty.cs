using AbysmalCore.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AbysmalCore.Extensibility
{
    /// <summary>
    /// Defines a property or field in the Abysmal Extensibility Framework
    /// </summary>
    [DebugInfo("abysmal extensibility framework property", false)]
    public class UniformProperty
    {
        private enum propertyType
        {
            property,
            field,
            privateMember,
        }

        private object _info;
        private object _instance;
        private propertyType _type;

        /// <summary>
        /// The name of the property or field
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The type of the property or field
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// Whether the property or field is private
        /// </summary>
        public bool IsPrivate => _type == propertyType.privateMember;
        /// <summary>
        /// Gets and sets the value of the property or field
        /// </summary>
        public object? Value
        {
            get
            {
                if (_type == propertyType.property)
                {
                    PropertyInfo? inf = _info as PropertyInfo;
                    if (inf!.CanRead)
                        return inf.GetMethod?.Invoke(_instance, null);
                    else
                    {
                        AbysmalDebug.Log(this, Name + " is write-only, cannot get value in " + _instance.GetType().FullName);
                        return null;
                    }
                }
                else if (_type == propertyType.field)
                {
                    FieldInfo? inf = _info as FieldInfo;
                    return inf!.GetValue(_instance);
                }
                else return null;
            }
            set
            {
                if (_type == propertyType.property)
                {
                    PropertyInfo? inf = _info as PropertyInfo;
                    if (inf!.CanWrite)
                        inf.SetMethod?.Invoke(_instance, [value]);
                    else AbysmalDebug.Log(this, Name + " is read-only, cannot set value in " + _instance.GetType().FullName);
                }
                else if (_type == propertyType.field)
                {
                    FieldInfo? inf = _info as FieldInfo;
                    if (inf!.IsLiteral || inf!.IsInitOnly)
                        AbysmalDebug.Log(this, Name + " is read-only, cannot set value in " + _instance.GetType().FullName);
                    else inf.SetValue(_instance, value);
                }
            }
        }

        /// <summary>
        /// Creates a new AbysmalExtensibilityProperty wrapping the specified PropertyInfo
        /// </summary>
        /// <param name="pi">The PropertyInfo to wrap</param>
        /// <param name="instance">The instance of the underlying class</param>
        public UniformProperty(PropertyInfo pi, object instance)
        {
            _type = propertyType.property;
            _instance = instance;
            _info = pi;

            Name = pi.Name;
            Type = pi.PropertyType;
        }

        /// <summary>
        /// Creates a new AbysmalExtensibilityProperty wrapping the specified FieldInfo
        /// </summary>
        /// <param name="fi">The FieldInfo to wrap</param>
        /// <param name="instance">The instance of the underlying class</param>
        public UniformProperty(FieldInfo fi, object instance)
        {
            if (fi.IsPublic) _type = propertyType.field;
            else _type = propertyType.privateMember;

            _instance = instance;
            _info = fi;

            Name = fi.Name;
            Type = fi.FieldType;
        }
    }
}

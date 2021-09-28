using NaughtyAttributes;
using UnityEngine;
namespace Fossil
{
    [System.Serializable]
    public class FloatReflectionValueAccessor : ReflectionValueAccessor<float>, IFloatValueAccessor { }
    [System.Serializable]
    public class BoolReflectionValueAccessor : ReflectionValueAccessor<bool>, IBoolValueAccessor { }
    [System.Serializable]
    public class IntReflectionValueAccessor : ReflectionValueAccessor<int>, IIntValueAccessor { }

    [System.Serializable]
    public class ReflectionValueAccessor<T>
    {
        public ReferenceType referenceType;
        bool UseObjectReference => referenceType == ReferenceType.objectReference;
        bool UseClassReference => referenceType == ReferenceType.classReference;
        [ShowIf("useObjectReference")]
        public Object objectReference;
        [ShowIf("useClassReference")]
        public string classReference;
        public string propertyName;

        [ShowNativeProperty]
        object CurrentValue => CheckCacheAndGetValue();
        [ShowNativeProperty]
        bool ValidType => typeCache != null;
        System.Type typeCache;
        [ShowNativeProperty]
        bool ValidProperty => propertyInfoCache != null;
        System.Reflection.PropertyInfo propertyInfoCache;

        public enum ReferenceType
        {
            classReference,
            objectReference
        }

        public void GetPropertyInfo()
        {
            if (UseObjectReference)
            {
                typeCache = objectReference.GetType();
            }
            else
            {
                if (!string.IsNullOrEmpty(classReference))
                {
                    typeCache = System.Type.GetType(classReference);
                }
            }
            if (typeCache != null)
            {
                propertyInfoCache = typeCache.GetProperty(propertyName);
            }
        }

        private T CheckCacheAndGetValue()
        {
            GetPropertyInfo();
            return GetValue();
        }

        public T GetValue()
        {
            if (propertyInfoCache == null)
            {
                GetPropertyInfo();
            }
            if (propertyInfoCache == null)
            {
                return default;
            }
            return (T)propertyInfoCache.GetValue(UseObjectReference ? objectReference : null);
        }

        public void SetValue(T o)
        {
            if (propertyInfoCache == null)
            {
                GetPropertyInfo();
            }
            if (propertyInfoCache == null)
            {
                return;
            }
            propertyInfoCache.SetValue(UseObjectReference ? objectReference : null, o);
        }
    }
}
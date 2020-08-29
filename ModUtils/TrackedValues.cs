using System;
using System.Collections;
using System.Collections.Generic;
//using System.Reflection;

namespace ChemistryClass.ModUtils {

    public class NullableEnsured<ValueType> {
        public ValueType Value;
        public readonly bool EncapsulatesStruct = typeof(ValueType).IsValueType;
        public bool IsNull => Value == null || this == null;

        public NullableEnsured(ValueType value) {
            Value = value;
        }

        public static implicit operator NullableEnsured<ValueType>(ValueType val)
            => new NullableEnsured<ValueType>(val);
        public static explicit operator ValueType(NullableEnsured<ValueType> val)
            => val.Value;
    }

    public class TrackedValue<ValueT> {

        public ValueT _value;
        public ValueT Value { 
            get => _value;
            set => Update(value);
        }

        public NullableEnsured<ValueT>[] PreviousValues;

        public TrackedValue(ValueT value, int trackingCapability) {
            _value = value;
            if (trackingCapability < 1) throw new Exception("Invalid tracker.");
            PreviousValues = new NullableEnsured<ValueT>[trackingCapability];
            for(int i = 0; i < trackingCapability; i++) {
                PreviousValues[i] = null;
            }
        }

        public TrackedValue(ValueT value, params NullableEnsured<ValueT>[] previousValues) {
            _value = value;
            if (previousValues.Length < 1) throw new Exception("Invalid tracker.");
            PreviousValues = previousValues;
        }

        public void Update(ValueT newValue) {
            int i = PreviousValues.Length - 1;
            while (i > 0) {
                PreviousValues[i] = PreviousValues[i++];
            }
            PreviousValues[0] = Value;
            Value = newValue;
        }  

    }

}

using System;

namespace Examples.ExplicitPrimitives
{
    /// <summary>
    /// The ExitCode struct enables explicit intention of return types and parameters that would have otherwise been declared as int types.  For example, the return of
    /// the method "int DoStuff(string[] args)" can't truly be inferred, but can be deduced if the method is "ExitCode DoStuff(string[] args)".
    /// </summary>
    /// <example>
    /// The following example demonstrates the behavior of the ExitCode struct with implicit and explicit conversions.
    /// <code>
    /// public enum MyEnum
    /// {
    ///     First,
    ///     Second,
    /// }
    /// public static int Main(string[] args)
    /// {
    ///     ExitCode result = 42; // implicitly converts ints
    ///     result = (ExitCode)MyEnum.First; // explicitly converts enums
    ///     return result; // implicitly converts ExitCode back to an int
    /// }
    /// </code>
    /// </example>
    public struct ExitCode
    {
        /// <summary>
        /// The ExitCode that represents success.
        /// </summary>
        public static readonly ExitCode Success = 0;

        /// <summary>
        /// The ExitCode that represents failure.
        /// </summary>
        public static readonly ExitCode Failure = 1;

        /// <summary>
        /// Gets the underlying value of this ExitCode.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Creates an ExitCode with the specified value.
        /// </summary>
        /// <param name="value"></param>
        public ExitCode(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Implicitly converts an ExitCode to an int.
        /// </summary>
        /// <param name="e"></param>
        public static implicit operator int(ExitCode e)
        {
            return e.Value;
        }

        /// <summary>
        /// Implicitly converts an int to an ExitCode.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ExitCode(int value)
        {
            return new ExitCode(value);
        }

        /// <summary>
        /// Explicitly converts an enum into an ExitCode.  The enum must use Int32 as its underlying type.
        /// </summary>
        /// <param name="e"></param>
        public static explicit operator ExitCode(Enum e)
        {
            if (e.GetTypeCode() != TypeCode.Int32)
            {
                throw new ArgumentException($"Cannot convert enum '{e.GetType()}' to ExitCode.  Only integer enums can be converted to an ExitCode.");
            }
            return new ExitCode(Convert.ToInt32(e));
        }

        /// <summary>
        /// Determines if the specified object is equal to this ExitCode's underlying value.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Value.Equals(obj);
        }

        /// <summary>
        /// Returns the hash code of this ExitCode's underlying value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Gets the string representation of this ExitCode's underlying value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

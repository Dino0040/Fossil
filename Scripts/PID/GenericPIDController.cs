using NaughtyAttributes;
namespace Fossil
{
    /// <summary>
    /// A (P)roportional, (I)ntegral, (D)erivative Controller
    /// </summary>
    /// <remarks>
    /// The controller should be able to control any process with a
    /// measureable value, a known ideal value and an input to the
    /// process that will affect the measured value.
    /// Adapted from https://github.com/ms-iot/pid-controller
    /// </remarks>
    /// <see cref="https://en.wikipedia.org/wiki/PID_controller"/>
    [System.Serializable]
    public class GenericPIDController<T, C>
    {
        System.Func<T, T, T> Add;
        System.Func<T, float, T> Scale;
        System.Func<T, C, T> ClampAbsolute;
        System.Func<T, C, C, T> ClampMinMax;

        public GenericPIDController(
            System.Func<T, T, T> Add,
            System.Func<T, float, T> Scale,
            System.Func<T, C, T> ClampAbsolute,
            System.Func<T, C, C, T> ClampMinMax)
        {
            this.Add = Add;
            this.Scale = Scale;
            this.ClampAbsolute = ClampAbsolute;
            this.ClampMinMax = ClampMinMax;
        }

        /// <summary>
        /// Calculate the controller output
        /// </summary>
        /// <param name="value">current value
        /// <param name="target">target the value should reach
        /// <param name="seconds">timespan of the elapsed time
        /// since the previous time that CalculateControlValue was called</param>
        /// <returns>Control value</returns>
        public T CalculateControlValue(T value, T target, float seconds)
        {
            T error = Add(target, Scale(value, -1));

            // integral term calculation
            IntegralTerm = Add(IntegralTerm, Scale(error, GainIntegral * seconds));

            // derivative term calculation
            T dInput = Add(value, Scale(lastValue, -1));
            T derivativeTerm = Scale(dInput, 1 / seconds * GainDerivative);

            // proportional term calcullation
            T proportionalTerm = Scale(error, GainProportional);

            T output = Add(Add(proportionalTerm, IntegralTerm), Scale(derivativeTerm, -1));

            switch (clampMode)
            {
                case ClampMode.MinMax:
                    output = ClampMinMax(output, OutputMin, OutputMax);
                    break;
                case ClampMode.Absolute:
                    output = ClampAbsolute(output, OutputMax);
                    break;
                default:
                    break;
            }

            lastValue = value;
            return output;
        }

        /// <summary>
        /// The derivative term is proportional to the rate of
        /// change of the error
        /// </summary>
        public float GainDerivative = 0;

        /// <summary>
        /// The integral term is proportional to both the magnitude
        /// of the error and the duration of the error
        /// </summary>
        public float GainIntegral = 0;

        /// <summary>
        /// The proportional term produces an output value that
        /// is proportional to the current error value
        /// </summary>
        /// <remarks>
        /// Tuning theory and industrial practice indicate that the
        /// proportional term should contribute the bulk of the output change.
        /// </remarks>
        public float GainProportional = 0;

        public ClampMode clampMode = ClampMode.None;

        bool ClampedUsingMinMax => clampMode == ClampMode.MinMax;
        bool ClampedUsingAbsolute => clampMode == ClampMode.Absolute;

        /// <summary>
        /// The max output value the control device can accept.
        /// </summary>
        [ShowIf(EConditionOperator.Or, "ClampedUsingMinMax", "ClampedUsingAbsolute")]
        [AllowNesting]
        public C OutputMax = default;

        /// <summary>
        /// The minimum ouput value the control device can accept.
        /// </summary>
        [ShowIf("ClampedUsingMinMax")]
        [AllowNesting]
        public C OutputMin = default;

        /// <summary>
        /// Adjustment made by considering the accumulated error over time
        /// </summary>
        /// <remarks>
        /// An alternative formulation of the integral action, is the
        /// proportional-summation-difference used in discrete-time systems
        /// </remarks>
        T IntegralTerm = default;

        public T lastValue { get; private set; } = default;

        public enum ClampMode
        {
            None,
            MinMax,
            Absolute
        }
    }
}
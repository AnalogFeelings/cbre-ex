namespace CBRE.Extended.Common.Easings;

public class Easing
{
    private Func<decimal, decimal> Function { get; set; }
    private EasingDirection Direction { get; set; }

    public Easing(Func<decimal, decimal> Function, EasingDirection Direction)
    {
        this.Function = Function;
        this.Direction = Direction;
    }

    public decimal Evaluate(decimal Input)
    {
        switch (Direction)
        {
            case EasingDirection.In:
                return Function(Input);
            case EasingDirection.Out:
                return 1 - Function(1 - Input);
            case EasingDirection.InOut:
                return Input < 0.5m
                    ? Function(Input * 2) / 2
                    : 1 - Function(Input * -2 + 2) / 2;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public static Easing FromType(EasingType Type, EasingDirection Direction)
    {
        return new Easing(FunctionFromType(Type), Direction);
    }

    private static Func<decimal, decimal> FunctionFromType(EasingType EasingType)
    {
        switch (EasingType)
        {
            case EasingType.Constant:
                return x => 1;
            case EasingType.Linear:
                return x => x;
            case EasingType.Quadratic:
                return x => PreciseMath.Pow(x, 2);
            case EasingType.Cubic:
                return x => PreciseMath.Pow(x, 3);
            case EasingType.Quartic:
                return x => PreciseMath.Pow(x, 4);
            case EasingType.Quintic:
                return x => PreciseMath.Pow(x, 5);
            case EasingType.Sinusoidal:
                return x => 1 - PreciseMath.Cos(x * PreciseMath.Pi / 2); // Wait... That's not Sine!
            case EasingType.Exponential:
                return x => PreciseMath.Pow(x, 5);
            case EasingType.Circular:
                return x => 1 - PreciseMath.Sqrt(1 - x * x);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
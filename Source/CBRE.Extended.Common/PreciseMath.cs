namespace CBRE.Extended.Common;

public static class PreciseMath
{
    public static readonly decimal Pi;

    static PreciseMath()
    {
        Pi = (decimal)Math.PI;
    }

    public static decimal Sqrt(decimal Value)
    {
        return (decimal)Math.Sqrt((double)Value);
    }

    public static decimal Pow(decimal Value, decimal Power)
    {
        return (decimal)Math.Pow((double)Value, (double)Power);
    }

    public static decimal Tan(decimal Angle)
    {
        return (decimal)Math.Tan((double)Angle);
    }

    public static decimal Atan(decimal Angle)
    {
        return (decimal)Math.Atan((double)Angle);
    }

    public static decimal Atan2(decimal Angle1, decimal Angle2)
    {
        return (decimal)Math.Atan2((double)Angle1, (double)Angle2);
    }

    public static decimal Cos(decimal Angle)
    {
        return (decimal)Math.Cos((double)Angle);
    }

    public static decimal Sin(decimal Angle)
    {
        return (decimal)Math.Sin((double)Angle);
    }

    public static decimal Asin(decimal Angle)
    {
        return (decimal)Math.Asin((double)Angle);
    }

    public static decimal Acos(decimal Value)
    {
        return (decimal)Math.Acos((double)Value);
    }

    public static decimal DegreesToRadians(decimal Degrees)
    {
        return Degrees * Pi / 180;
    }

    public static decimal RadiansToDegrees(decimal Radians)
    {
        return Radians * 180 / Pi;
    }

    public static decimal Abs(decimal Value)
    {
        return Value < 0 ? -Value : Value;
    }

    public static decimal Clamp(decimal Value, decimal Min, decimal Max)
    {
        return (Value < Min) ? Min : (Value > Max) ? Max : Value;
    }
}
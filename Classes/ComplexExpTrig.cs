using System;

public class ComplexExpTrig : Complex
{
    public static ComplexAlg ToComplexAlg(ComplexExpTrig value)
    {
        ComplexAlg digit = new ComplexAlg();
        digit.Re = Math.Round(value.Module * Math.Cos(value.Angle));
        digit.Im = Math.Round(value.Module * Math.Sin(value.Angle));
        return digit;
    }
    public ComplexExpTrig()
    {
        _radius = 1;
        _angle = 0;
    }
    public ComplexExpTrig(double _radius, double _angle)
    {
        this._radius = _radius;
        this._angle = _angle;
    }
    public double Module
    { get => _radius; set => _radius = value; }
    public double Angle
    { get => _angle; set => _angle = value; }
    public static string PrintTrig(ComplexExpTrig digit)
    {
        if (digit.Module != 0)
        {
            if (Math.Sin(digit.Angle) == 0) return digit.Module + "  cos (" + digit.Angle + ")";
            else if (Math.Cos(digit.Angle) == 0) return digit.Module + " i sin (" + digit.Angle + ")";
            else return digit.Module + " ( cos(" + digit.Angle + ") + i * sin(" + digit.Angle + "))";
        }
        else return "0";
    }
    public static string PrintExp(ComplexExpTrig digit)
    {
        if (digit.Module != 0)
        {
            if (digit.Angle == 0) return Convert.ToString(digit.Module);
            else if (digit.Angle > 0) return digit.Module + " exp {i * " + digit.Angle + "}";
            else return digit.Module + " exp { - i * " + Math.Abs(digit.Angle) + "}";
        }
        else return "0";
    }
}

using System;

public class ComplexAlg : Complex
{
    public static ComplexExpTrig ToComplexExpTrig(ComplexAlg value)
    {
        ComplexExpTrig digit = new ComplexExpTrig();
        digit.Module = Math.Sqrt(Math.Pow(value.Re, 2) + Math.Pow(value.Im, 2));
        if (value.Re != 0) digit.Angle = Math.Atan(value.Im / value.Re);
        return digit;
    }
    public ComplexAlg()
    {
        _realpart = _imagepart = 1;
    }
    public ComplexAlg(double _realpart, double _imagepart)
    {
        this._realpart = _realpart;
        this._imagepart = _imagepart;
    }
    public double Re { get => _realpart; set => _realpart = value; }
    public double Im { get => _imagepart; set => _imagepart = value; }
    public static string Print(ComplexAlg digit)
    {
        if (digit.Re == 0 && digit.Im != 0) return Convert.ToString(digit.Im) + " i";
        else if (digit.Re == 0) return "0";
        else if (digit.Im == 0) return Convert.ToString(digit.Re);
        else if (digit.Im > 0) return digit.Re + " + " + digit.Im + " i";
        else return digit.Re + " - " + Math.Abs(digit.Im) + " i";
    }
}

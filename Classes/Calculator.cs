using System;

public class Calculator
{
    public static ComplexAlg Summator(ComplexAlg digit_1, ComplexAlg digit_2)
    {
        ComplexAlg sum = new ComplexAlg();
        sum.Re = digit_1.Re + digit_2.Re;
        sum.Im = digit_1.Im + digit_2.Im;
        return sum;
    }
    public static ComplexAlg Subtractor(ComplexAlg digit_1, ComplexAlg digit_2)
    {
        ComplexAlg subs = new ComplexAlg();
        subs.Re = digit_1.Re - digit_2.Re;
        subs.Im = digit_1.Im - digit_2.Im;
        return subs;
    }
    public static ComplexAlg Multiplier(ComplexAlg digit1, ComplexAlg digit2)
    {
        ComplexAlg result = new ComplexAlg();
        result.Re = digit1.Re * digit2.Re - digit1.Im * digit2.Im;
        result.Im = digit1.Re * digit2.Im + digit1.Im * digit2.Re;
        return result;
    }
    public static ComplexExpTrig Multiplier(ComplexExpTrig digit_1, ComplexExpTrig digit_2)
    {
        ComplexExpTrig result = new ComplexExpTrig();
        result.Module = digit_1.Module * digit_2.Module;
        result.Angle = digit_1.Angle + digit_2.Angle;
        return result;
    }
    public static ComplexAlg Divider(ComplexAlg digit1, ComplexAlg digit2)
    {
        ComplexAlg result = new ComplexAlg();
        result.Re = (digit1.Re * digit2.Re + digit1.Im * digit2.Im) / (Math.Pow(digit2.Re, 2) + Math.Pow(digit2.Im, 2));
        result.Im = (digit2.Re * digit1.Im - digit2.Im * digit1.Re) / (Math.Pow(digit2.Re, 2) + Math.Pow(digit2.Im, 2));
        return result;
    }
    public static ComplexExpTrig Divider(ComplexExpTrig digit_1, ComplexExpTrig digit_2)
    {
        ComplexExpTrig result = new ComplexExpTrig();
        result.Module = digit_1.Module / digit_2.Module;
        result.Angle = digit_1.Angle - digit_2.Angle;
        return result;
    }
    public static ComplexExpTrig Powerer(ComplexExpTrig digit1, ComplexAlg digit2)
    {
        ComplexExpTrig result = new ComplexExpTrig();
        result.Module = Math.Pow(digit1.Module, digit2.Re);
        result.Angle = digit1.Angle * digit2.Re;
        return result;
    }
}

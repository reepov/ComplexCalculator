using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static dzpigareva.Form1;

namespace dzpigareva.Classes
{
    public class Actions
    {
        public static void Clear(ref List<ComplexAlg> complexAlgDigits, ref List<ComplexExpTrig> complexExpTrigDigits, ref Label label10, ref TextBox[] text, 
                                 ref bool [,,] operations, ref int digitsLimit, ref bool nowAlg, ref bool nowExpTrig, ref Button[] buttons)
        {
            label10.Text = "ЖУРНАЛ АКТИВНЫХ ДЕЙСТВИЙ РАБОТЫ КАЛЬКУЛЯТОРА";
            complexAlgDigits.Clear();
            complexExpTrigDigits.Clear();
            foreach (TextBox tex in text)
            {
                tex.Clear();
            }
            digitsLimit = 0;
            nowAlg = false;
            nowExpTrig = false;
            for (int i = 0; i < (int)Constants.countOperations; i++)
            {
                for (int j = 0; j < (int)Constants.countDigits; j++)
                {
                    operations[i, j, 0] = false;
                    operations[i, j, 1] = false;
                }
            }
            for (int i = 1; i < (int)Constants.countDigits + 1; i++)
            {
                buttons[i].Enabled = false;
                text[i - 1].ReadOnly = false;
                if (i == 7 || i == 8) text[i].ReadOnly = true;
            }
            buttons[0].Enabled = true;
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь очистил калькулятор.");
            }
        }
        public static void Count(int op, int i, int type, int counttype, ref List<ComplexAlg> complexAlgDigits, ref List<ComplexExpTrig> complexExpTrigDigits,
                                 ref int digitsLimit, ref TextBox[] text, ref bool[,,] operations)
        {
            switch (op)
            {
                case 4:
                    ComplexExpTrig _digit = type == 0 ? ComplexAlg.ToComplexExpTrig(complexAlgDigits[i]) : complexExpTrigDigits[i];
                    ComplexExpTrig power = Calculator.Powerer(_digit, complexAlgDigits[i + 1]);
                    complexAlgDigits[i + 1] = null;
                    if (type == 0 && (counttype == 2 || counttype == 3)) complexAlgDigits[i] = null;
                    if (type == 0 && counttype == 1) complexAlgDigits[i] = ComplexExpTrig.ToComplexAlg(power);
                    if (type == 1 || counttype == 2 || counttype == 3) complexExpTrigDigits[i] = power;
                    ComplexAlg digit = ComplexExpTrig.ToComplexAlg(power);
                    complexAlgDigits.RemoveAt(i + 1);
                    complexExpTrigDigits.RemoveAt(i + 1);
                    if (i == digitsLimit - 2)
                    {
                        if (counttype == 1) text[7].Text = text[8].Text = ComplexAlg.Print(digit);
                        if (counttype == 2) text[7].Text = text[8].Text = ComplexExpTrig.PrintTrig(power);
                        if (counttype == 3) text[7].Text = text[8].Text = ComplexExpTrig.PrintExp(power);
                        digitsLimit = 1;
                        for (int r = 0; r < (int)Constants.countOperations; r++)
                        {
                            for (int j = 0; j < (int)Constants.countDigits; j++)
                            {
                                operations[r, j, 0] = false;
                                operations[r, j, 1] = false;
                            }
                        }
                    }
                    break;
                case 0:
                    ComplexAlg summa = new ComplexAlg();
                    if (type == 0) summa = complexAlgDigits[i + 1] != null ? Calculator.Summator(complexAlgDigits[i], complexAlgDigits[i + 1])
                                         : Calculator.Summator(complexAlgDigits[i], ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[i + 1]));
                    if (type == 1) summa = complexAlgDigits[i + 1] != null ? Calculator.Summator(ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[i]), complexAlgDigits[i + 1])
                                         : Calculator.Summator(ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[i]), ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[i + 1]));
                    complexAlgDigits[i] = summa;
                    complexAlgDigits.RemoveAt(i + 1);
                    complexExpTrigDigits.RemoveAt(i + 1);
                    if (i == digitsLimit - 2)
                    {
                        if (counttype == 1) text[7].Text = text[8].Text = ComplexAlg.Print(summa);
                        if (counttype == 2) text[7].Text = text[8].Text = ComplexExpTrig.PrintTrig(ComplexAlg.ToComplexExpTrig(summa));
                        if (counttype == 3) text[7].Text = text[8].Text = ComplexExpTrig.PrintExp(ComplexAlg.ToComplexExpTrig(summa));
                        digitsLimit = 1;
                        for (int r = 0; r < (int)Constants.countOperations; r++)
                        {
                            for (int j = 0; j < (int)Constants.countDigits; j++)
                            {
                                operations[r, j, 0] = false;
                                operations[r, j, 1] = false;
                            }
                        }
                    }
                    break;
                case 2:
                    ComplexAlg multiplyAlg = new ComplexAlg();
                    ComplexExpTrig multiplyExpTrig = new ComplexExpTrig();
                    if (type == 0)
                    {
                        multiplyAlg = complexAlgDigits[i + 1] != null ? Calculator.Multiplier(complexAlgDigits[i], complexAlgDigits[i + 1])
                                : Calculator.Multiplier(complexAlgDigits[i], ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[i + 1]));
                        complexAlgDigits[i] = multiplyAlg;
                    }
                    if (type == 1)
                    {
                        multiplyExpTrig = complexAlgDigits[i + 1] == null ? Calculator.Multiplier(complexExpTrigDigits[i], complexExpTrigDigits[i + 1])
                                        : Calculator.Multiplier(complexExpTrigDigits[i], ComplexAlg.ToComplexExpTrig(complexAlgDigits[i + 1]));
                        complexExpTrigDigits[i] = multiplyExpTrig;
                    }
                    complexAlgDigits.RemoveAt(i + 1);
                    complexExpTrigDigits.RemoveAt(i + 1);
                    if (i == digitsLimit - 2)
                    {
                        if (counttype == 1) text[7].Text = text[8].Text = type == 0 ? ComplexAlg.Print(multiplyAlg) : ComplexAlg.Print(ComplexExpTrig.ToComplexAlg(multiplyExpTrig));
                        if (counttype == 2) text[7].Text = text[8].Text = type == 0 ? ComplexExpTrig.PrintTrig(ComplexAlg.ToComplexExpTrig(multiplyAlg)) : ComplexExpTrig.PrintTrig(multiplyExpTrig);
                        if (counttype == 3) text[7].Text = text[8].Text = type == 0 ? ComplexExpTrig.PrintExp(ComplexAlg.ToComplexExpTrig(multiplyAlg)) : ComplexExpTrig.PrintExp(multiplyExpTrig);
                        digitsLimit = 1;
                        for (int r = 0; r < (int)Constants.countOperations; r++)
                        {
                            for (int j = 0; j < (int)Constants.countDigits; j++)
                            {
                                operations[r, j, 0] = false;
                                operations[r, j, 1] = false;
                            }
                        }
                    }
                    break;
                case 1:
                    ComplexAlg subtract = new ComplexAlg();
                    if (type == 0) subtract = complexAlgDigits[i + 1] != null ? Calculator.Subtractor(complexAlgDigits[i], complexAlgDigits[i + 1])
                                            : Calculator.Subtractor(complexAlgDigits[i], ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[i + 1]));
                    if (type == 1) subtract = complexAlgDigits[i + 1] != null ? Calculator.Subtractor(ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[i]), complexAlgDigits[i + 1])
                                            : Calculator.Subtractor(ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[i]), ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[i + 1]));
                    complexAlgDigits[i] = subtract;
                    complexAlgDigits.RemoveAt(i + 1);
                    complexExpTrigDigits.RemoveAt(i + 1);
                    if (i == digitsLimit - 2)
                    {
                        if (counttype == 1) text[7].Text = text[8].Text = ComplexAlg.Print(subtract);
                        if (counttype == 2) text[7].Text = text[8].Text = ComplexExpTrig.PrintTrig(ComplexAlg.ToComplexExpTrig(subtract));
                        if (counttype == 3) text[7].Text = text[8].Text = ComplexExpTrig.PrintExp(ComplexAlg.ToComplexExpTrig(subtract));
                        digitsLimit = 1;
                        for (int r = 0; r < (int)Constants.countOperations; r++)
                        {
                            for (int j = 0; j < (int)Constants.countDigits; j++)
                            {
                                operations[r, j, 0] = false;
                                operations[r, j, 1] = false;
                            }
                        }
                    }
                    break;
                case 3:
                    ComplexAlg divideAlg = new ComplexAlg();
                    ComplexExpTrig divideExpTrig = new ComplexExpTrig();
                    if (type == 0)
                    {
                        divideAlg = complexAlgDigits[i + 1] != null ? Calculator.Divider(complexAlgDigits[i], complexAlgDigits[i + 1])
                                  : Calculator.Divider(complexAlgDigits[i], ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[i + 1]));
                        complexAlgDigits[i] = divideAlg;
                    }
                    if (type == 1)
                    {
                        divideExpTrig = complexAlgDigits[i + 1] == null ? Calculator.Divider(complexExpTrigDigits[i], complexExpTrigDigits[i + 1])
                                      : Calculator.Divider(complexExpTrigDigits[i], ComplexAlg.ToComplexExpTrig(complexAlgDigits[i + 1]));
                        complexExpTrigDigits[i] = divideExpTrig;
                    }
                    complexAlgDigits.RemoveAt(i + 1);
                    complexExpTrigDigits.RemoveAt(i + 1);
                    if (i == digitsLimit - 2)
                    {
                        if (counttype == 1) text[7].Text = text[8].Text = type == 0 ? ComplexAlg.Print(divideAlg) : ComplexAlg.Print(ComplexExpTrig.ToComplexAlg(divideExpTrig));
                        if (counttype == 2) text[7].Text = text[8].Text = type == 0 ? ComplexExpTrig.PrintTrig(ComplexAlg.ToComplexExpTrig(divideAlg)) : ComplexExpTrig.PrintTrig(divideExpTrig);
                        if (counttype == 3) text[7].Text = text[8].Text = type == 0 ? ComplexExpTrig.PrintExp(ComplexAlg.ToComplexExpTrig(divideAlg)) : ComplexExpTrig.PrintExp(divideExpTrig);
                        digitsLimit = 1;
                        for (int r = 0; r < (int)Constants.countOperations; r++)
                        {
                            for (int j = 0; j < (int)Constants.countDigits; j++)
                            {
                                operations[r, j, 0] = false;
                                operations[r, j, 1] = false;
                            }
                        }
                    }
                    break;
            }
        } // Метод для рассчетов
        public static void CountInAlgForm(ref int digitsLimit, ref Label label10, ref bool [,,] operations, ref TextBox[] text, 
                                          ref List<ComplexAlg> complexAlgDigits, ref List<ComplexExpTrig> complexExpTrigDigits)
        {
            if (digitsLimit < 2)
            {
                MessageBox.Show("Недостаточно чисел для проведения рассчётов");
                using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
                {
                    protocol.WriteLine("Пользователь ввёл недостаточное количество чисел для совершения вычислений");
                }
                return;
            }
            label10.Text = "ЖУРНАЛ АКТИВНЫХ ДЕЙСТВИЙ РАБОТЫ КАЛЬКУЛЯТОРА";
            for (int i = 0; i < digitsLimit - 1; i++)
            {
                if (operations[4, i, 0]) Count(4, i, 0, 1, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[4, i, 1]) Count(4, i, 1, 1, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[2, i, 0]) Count(2, i, 0, 1, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[2, i, 1]) Count(2, i, 1, 1, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[3, i, 0]) Count(3, i, 0, 1, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[3, i, 1]) Count(3, i, 1, 1, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[0, i, 0]) Count(0, i, 0, 1, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[0, i, 1]) Count(0, i, 1, 1, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[1, i, 0]) Count(1, i, 0, 1, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[1, i, 1]) Count(1, i, 1, 1, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
            }
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал расчёты в алгебраической форме");
                protocol.WriteLine("Результат: {0}", text[8].Text);
            }
        }
        public static void CountInTrigForm(ref int digitsLimit, ref Label label10, ref bool[,,] operations, ref TextBox[] text, ref List<ComplexAlg> complexAlgDigits, ref List<ComplexExpTrig> complexExpTrigDigits)
        {
            if (digitsLimit < 2)
            {
                MessageBox.Show("Недостаточно чисел для проведения рассчётов");
                return;
            }
            label10.Text = "ЖУРНАЛ АКТИВНЫХ ДЕЙСТВИЙ РАБОТЫ КАЛЬКУЛЯТОРА";
            for (int i = 0; i < digitsLimit - 1; i++)
            {
                if (operations[4, i, 0]) Count(4, i, 0, 2, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[4, i, 1]) Count(4, i, 1, 2, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[2, i, 0]) Count(2, i, 0, 2, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[2, i, 1]) Count(2, i, 1, 2, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[3, i, 0]) Count(3, i, 0, 2, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[3, i, 1]) Count(3, i, 1, 2, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[0, i, 0]) Count(0, i, 0, 2, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[0, i, 1]) Count(0, i, 1, 2, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[1, i, 0]) Count(1, i, 0, 2, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[1, i, 1]) Count(1, i, 1, 2, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
            }
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал расчёты в тригонометрической форме");
                protocol.WriteLine("Результат: {0}", text[8].Text);
            }
        }
        public static void CountInExpForm(ref int digitsLimit, ref Label label10, ref bool[,,] operations, ref TextBox[] text, ref List<ComplexAlg> complexAlgDigits, ref List<ComplexExpTrig> complexExpTrigDigits)
        {
            if (digitsLimit < 2)
            {
                MessageBox.Show("Недостаточно чисел для проведения рассчётов");
                return;
            }
            label10.Text = "ЖУРНАЛ АКТИВНЫХ ДЕЙСТВИЙ РАБОТЫ КАЛЬКУЛЯТОРА";
            for (int i = 0; i < digitsLimit - 1; i++)
            {
                if (operations[4, i, 0]) Count(4, i, 0, 3, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[4, i, 1]) Count(4, i, 1, 3, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[2, i, 0]) Count(2, i, 0, 3, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[2, i, 1]) Count(2, i, 1, 3, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[3, i, 0]) Count(3, i, 0, 3, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[3, i, 1]) Count(3, i, 1, 3, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[0, i, 0]) Count(0, i, 0, 3, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[0, i, 1]) Count(0, i, 1, 3, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[1, i, 0]) Count(1, i, 0, 3, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
                if (operations[1, i, 1]) Count(1, i, 1, 3, ref complexAlgDigits, ref complexExpTrigDigits, ref digitsLimit, ref text, ref operations);
            }
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал расчёты в экспоненциональной форме");
                protocol.WriteLine("Результат: {0}", text[8].Text);
            }
        }
        public static void ProtocolClean()
        {
            using (StreamWriter protocol = new StreamWriter("protocol.txt", false, Encoding.GetEncoding(1251)))
            {
                protocol.Write("");
            }
        }
    } 
}

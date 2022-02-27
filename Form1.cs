using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
namespace dzpigareva
{
    public partial class Form1 : Form
    {
        public enum Constants : int
        {
            countText = 9, countOperations = 5, countDigits = 8, countTypes = 2, power = 4, multiply = 2, divide = 3, summing = 0, substract = 1
        }
        int digitsLimit = 0;
        List<ComplexExpTrig> complexExpTrigDigits = new List<ComplexExpTrig>();
        List<ComplexAlg> complexAlgDigits = new List<ComplexAlg>();
        TextBox[] text = new TextBox[(int)Constants.countText];
        Button[] buttons = new Button[12];
        bool nowExpTrig = false;
        bool nowAlg = false;
        bool[,,] operations = new bool[(int)Constants.countOperations, (int)Constants.countDigits, (int)Constants.countTypes];
        class Complex
        {
            protected double _angle, _radius, _realpart, _imagepart;
        }
        class ComplexAlg : Complex
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
        class ComplexExpTrig : Complex
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
        class Calculator
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
        public Form1()
        {
            InitializeComponent();
            text[0] = textBox1; text[1] = textBox2; text[2] = textBox3; text[3] = textBox4;
            text[4] = textBox5; text[5] = textBox6; text[6] = textBox7; text[7] = textBox8; text[8] = textBox9;
            buttons[0] = button1; buttons[1] = button2; buttons[2] = button3; buttons[3] = button4; buttons[4] = button5; buttons[5] = button6;
            buttons[6] = button7; buttons[7] = button8; buttons[8] = button9; buttons[9] = button11; buttons[10] = button12; buttons[11] = button13;
            text[7].ReadOnly = true;
            text[8].ReadOnly = true;
            label10.Text = "ЖУРНАЛ АКТИВНЫХ ДЕЙСТВИЙ РАБОТЫ КАЛЬКУЛЯТОРА";
            for (int i = 1; i < 9; i++) { buttons[i].Enabled = false; }
        }
        private void button1_Click(object sender, EventArgs e) // ввести число в операцию
        {
            if (digitsLimit > 8) { MessageBox.Show("Нельзя вводить более, чем 9 чисел в операцию"); return; }
            string digit;
            bool firstdigit = text[0].Text != "" && text[1].Text != "" ? true : false;
            bool seconddigit = text[2].Text != "" && text[3].Text != "" && text[4].Text != "" ? true : false;
            bool thirddigit = text[5].Text != "" && text[6].Text != "" ? true : false;
            if (!(firstdigit || seconddigit || thirddigit)) { MessageBox.Show("Некорректно введённые данные"); return; }
            if (seconddigit && text[3].Text != text[4].Text) { MessageBox.Show("Угол фи должен быть одинаковый"); return; }
            for (int i = 0; i < 7; i++)
            {
                if ((firstdigit & i != 0 & i != 1 & text[i].Text != "") || (seconddigit & i != 2 & i != 3 & i != 4 & text[i].Text != "") || (thirddigit & i != 5 & i != 6 & text[i].Text != "")) 
                { 
                    MessageBox.Show("Введите только одно число"); 
                    return; 
                }
            }
            digit = "";
            digitsLimit++;
            if (firstdigit)
            {
                complexAlgDigits.Add(new ComplexAlg(double.Parse(text[0].Text), double.Parse(text[1].Text)));
                complexExpTrigDigits.Add(null);
                nowAlg = true; 
                digit = $"({ComplexAlg.Print(complexAlgDigits[complexAlgDigits.Count - 1])})";
                text[0].Clear(); text[1].Clear();
            }
            if (seconddigit)
            {
                complexExpTrigDigits.Add(new ComplexExpTrig(double.Parse(text[2].Text), double.Parse(text[3].Text)));
                complexAlgDigits.Add(null);
                nowExpTrig = true;
                digit = $"({ComplexExpTrig.PrintTrig(complexExpTrigDigits[complexExpTrigDigits.Count - 1])})";
                text[2].Clear(); text[3].Clear(); text[4].Clear();
            }
            if (thirddigit)
            {
                complexExpTrigDigits.Add(new ComplexExpTrig(double.Parse(text[5].Text), double.Parse(text[6].Text)));
                complexAlgDigits.Add(null);
                nowExpTrig = true;
                digit = $"({ComplexExpTrig.PrintExp(complexExpTrigDigits[complexExpTrigDigits.Count - 1])})";   
                text[5].Clear(); text[6].Clear();               
            }
            text[7].Text += digit;
            for (int i = 0; i < (int)Constants.countDigits; i++)
            {
                buttons[i + 1].Enabled = true;
                text[i].ReadOnly = true;
                label10.Text = "Нажмите кнопку Рассчитать результат для получения \n результата операции или выберите нужную операцию";
            }
            buttons[0].Enabled = false;
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Число " + digit + " введено в калькулятор");
            }
        }
        private void button2_Click(object sender, EventArgs e) // Сложение
        {
            text[7].Text += " + ";
            if (nowAlg) operations[0, digitsLimit - 1, 0] = true;
            if (nowExpTrig) operations[0, digitsLimit - 1, 1] = true;
            nowExpTrig = nowAlg = false;
            for (int i = 0; i < (int)Constants.countDigits; i++)
            {
                buttons[i + 1].Enabled = false;
                text[i].ReadOnly = false;
                label10.Text = "Введите следующее число";
                if (i == 0 || i == 1) text[i + 7].ReadOnly = true;
            }
            buttons[0].Enabled = true;
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал операцию сложения");
            }
        }
        private void button3_Click(object sender, EventArgs e) // Вычитание
        {
            text[7].Text += " - ";
            if (nowAlg) operations[1, digitsLimit - 1, 0] = true;
            if (nowExpTrig) operations[1, digitsLimit - 1, 1] = true;
            nowExpTrig = nowAlg = false;
            for (int i = 0; i < (int)Constants.countDigits; i++)
            {
                buttons[i + 1].Enabled = false;
                text[i].ReadOnly = false;
                label10.Text = "Введите следующее число";
            }
            text[7].ReadOnly = true;
            text[8].ReadOnly = true;
            buttons[0].Enabled = true;
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал операцию вычитания");
            }
        }
        private void button4_Click(object sender, EventArgs e) // Умножение
        {
            text[7].Text += " * ";
            if (nowAlg) operations[2, digitsLimit - 1, 0] = true;
            if (nowExpTrig) operations[2, digitsLimit - 1, 1] = true;
            nowExpTrig = nowAlg = false;
            for (int i = 0; i < (int)Constants.countDigits; i++)
            {
                buttons[i + 1].Enabled = false;
                text[i].ReadOnly = false;
                label10.Text = "Введите следующее число";
            }
            buttons[0].Enabled = true;
            text[7].ReadOnly = true;
            text[8].ReadOnly = true;
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал операцию умножения");
            }
        }
        private void button5_Click(object sender, EventArgs e) // Деление
        {
            text[7].Text += " / ";
            if (nowAlg) operations[3, digitsLimit - 1, 0] = true;
            if (nowExpTrig) operations[3, digitsLimit - 1, 1] = true;
            nowExpTrig = nowAlg = false;
            for (int i = 0; i < (int)Constants.countDigits; i++)
            {
                buttons[i + 1].Enabled = false;
                text[i].ReadOnly = false;
                label10.Text = "Введите следующее число";
            }
            text[7].ReadOnly = true;
            text[8].ReadOnly = true;
            buttons[0].Enabled = true;
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал операцию деления");
            }
        }
        private void button6_Click(object sender, EventArgs e) // Возведение в целую степень
        {
            text[7].Text += " ^ ";
            if (nowAlg) operations[4, digitsLimit - 1, 0] = true;
            if (nowExpTrig) operations[4, digitsLimit - 1, 1] = true;
            nowExpTrig = nowAlg = false;
            for (int i = 0; i < (int)Constants.countDigits; i++)
            {
                buttons[i + 1].Enabled = false;
                text[i].ReadOnly = false;
                label10.Text = "Введите целую степень как алгебраическое комплексное \n число с нулевой комплексной частью";
            }
            text[7].ReadOnly = true;
            text[8].ReadOnly = true;
            buttons[0].Enabled = true;
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал операцию возведения в целую степень");
            }
        }
        private void button7_Click(object sender, EventArgs e) // Преобразование в алгебраическую форму
        {
            if (digitsLimit > 1)
            {
                MessageBox.Show("Для перевода числа в другую форму требуется лишь одно введённое число");
                return;
            }
            text[7].Text = text[8].Text = complexAlgDigits[0] != null ? ComplexAlg.Print(complexAlgDigits[0]) : ComplexAlg.Print(ComplexExpTrig.ToComplexAlg(complexExpTrigDigits[0]));
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал операцию преобразования числа в алгебраическую форму");
                protocol.WriteLine("Результат: {0}", text[8].Text);
            }
        }
        private void button8_Click(object sender, EventArgs e) // Преобразование в тригонометрическую форму
        {
            if (digitsLimit > 1)
            {
                MessageBox.Show("Для перевода числа в другую форму требуется лишь одно введённое число");
                return;
            }
            text[7].Text = text[8].Text = complexAlgDigits[0] != null ? ComplexExpTrig.PrintTrig(ComplexAlg.ToComplexExpTrig(complexAlgDigits[0])) : ComplexExpTrig.PrintTrig(complexExpTrigDigits[0]);
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал операцию преобразования числа в тригонометрическую форму");
                protocol.WriteLine("Результат: {0}", text[8].Text);
            }
        }
        private void button9_Click(object sender, EventArgs e) // Преобразование в экспоненциальную форму
        {
            if (digitsLimit > 1)
            {
                MessageBox.Show("Для перевода числа в другую форму требуется лишь одно введённое число");
            }
            text[7].Text = text[8].Text = complexAlgDigits[0] != null ? ComplexExpTrig.PrintExp(ComplexAlg.ToComplexExpTrig(complexAlgDigits[0])) : ComplexExpTrig.PrintExp(complexExpTrigDigits[0]);
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал операцию преобразования числа в экспоненциальную форму");
                protocol.WriteLine("Результат: {0}", text[8].Text);
            }
        }
        private void button10_Click(object sender, EventArgs e) // Очистить калькулятор
        {
            label10.Text = "ЖУРНАЛ АКТИВНЫХ ДЕЙСТВИЙ РАБОТЫ КАЛЬКУЛЯТОРА";
            complexAlgDigits.Clear();
            complexExpTrigDigits.Clear();
            foreach (TextBox text in text)
            {
                text.Clear();
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
        void Count(int op, int i, int type, int counttype)
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
        private void button11_Click(object sender, EventArgs e) // рассчитать результат в алгебраической форме
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
                if (operations[4, i, 0]) Count(4, i, 0, 1);
                if (operations[4, i, 1]) Count(4, i, 1, 1);
                if (operations[2, i, 0]) Count(2, i, 0, 1);
                if (operations[2, i, 1]) Count(2, i, 1, 1);
                if (operations[3, i, 0]) Count(3, i, 0, 1);
                if (operations[3, i, 1]) Count(3, i, 1, 1);
                if (operations[0, i, 0]) Count(0, i, 0, 1);
                if (operations[0, i, 1]) Count(0, i, 1, 1);
                if (operations[1, i, 0]) Count(1, i, 0, 1);
                if (operations[1, i, 1]) Count(1, i, 1, 1);
            }
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал расчёты в алгебраической форме");
                protocol.WriteLine("Результат: {0}", text[8].Text);
            }
        }   
        private void button12_Click(object sender, EventArgs e) // рассчитать в тригонометрической форме
        {
            if (digitsLimit < 2)
            {
                MessageBox.Show("Недостаточно чисел для проведения рассчётов");
                return;
            }
            label10.Text = "ЖУРНАЛ АКТИВНЫХ ДЕЙСТВИЙ РАБОТЫ КАЛЬКУЛЯТОРА";
            for (int i = 0; i < digitsLimit - 1; i++)
            {
                if (operations[4, i, 0]) Count(4, i, 0, 2);
                if (operations[4, i, 1]) Count(4, i, 1, 2);
                if (operations[2, i, 0]) Count(2, i, 0, 2);
                if (operations[2, i, 1]) Count(2, i, 1, 2);
                if (operations[3, i, 0]) Count(3, i, 0, 2);
                if (operations[3, i, 1]) Count(3, i, 1, 2);
                if (operations[0, i, 0]) Count(0, i, 0, 2);
                if (operations[0, i, 1]) Count(0, i, 1, 2);
                if (operations[1, i, 0]) Count(1, i, 0, 2);
                if (operations[1, i, 1]) Count(1, i, 1, 2);
            }
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал расчёты в тригонометрической форме");
                protocol.WriteLine("Результат: {0}", text[8].Text);
            }
        }
        private void button13_Click(object sender, EventArgs e) // рассчитать в экспоненциональной форме
        {
            if (digitsLimit < 2)
            {
                MessageBox.Show("Недостаточно чисел для проведения рассчётов");
                return;
            }
            label10.Text = "ЖУРНАЛ АКТИВНЫХ ДЕЙСТВИЙ РАБОТЫ КАЛЬКУЛЯТОРА";
            for (int i = 0; i < digitsLimit - 1; i++)
            {
                if (operations[4, i, 0]) Count(4, i, 0, 3);
                if (operations[4, i, 1]) Count(4, i, 1, 3);
                if (operations[2, i, 0]) Count(2, i, 0, 3);
                if (operations[2, i, 1]) Count(2, i, 1, 3);
                if (operations[3, i, 0]) Count(3, i, 0, 3);
                if (operations[3, i, 1]) Count(3, i, 1, 3);
                if (operations[0, i, 0]) Count(0, i, 0, 3);
                if (operations[0, i, 1]) Count(0, i, 1, 3);
                if (operations[1, i, 0]) Count(1, i, 0, 3);
                if (operations[1, i, 1]) Count(1, i, 1, 3);
            }
            using (StreamWriter protocol = new StreamWriter("protocol.txt", true, Encoding.GetEncoding(1251)))
            {
                protocol.WriteLine("Пользователь выбрал расчёты в экспоненциональной форме");
                protocol.WriteLine("Результат: {0}", text[8].Text);
            }
        }
        private void label12_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void button14_Click(object sender, EventArgs e) => Process.Start("protocol.txt");
        private void button15_Click(object sender, EventArgs e)
        {
            using (StreamWriter protocol = new StreamWriter("protocol.txt", false, Encoding.GetEncoding(1251)))
            {
                protocol.Write("");
            }
        } // очистить протокол
    }
}

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
    public class Operations
    {
        public static void Summing(ref int digitsLimit, ref TextBox[] text, ref bool nowExpTrig, ref bool nowAlg, ref Button[] buttons, ref bool[,,] operations, ref Label label10)
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
        public static void Substracting(ref int digitsLimit, ref TextBox[] text, ref bool nowExpTrig, ref bool nowAlg, ref Button[] buttons, ref bool[,,] operations, ref Label label10)
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
        public static void Multiplying(ref int digitsLimit, ref TextBox[] text, ref bool nowExpTrig, ref bool nowAlg, ref Button[] buttons, ref bool[,,] operations, ref Label label10)
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
        public static void Dividing(ref int digitsLimit, ref TextBox[] text, ref bool nowExpTrig, ref bool nowAlg, ref Button[] buttons, ref bool[,,] operations, ref Label label10)
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
        public static void IntPowering(ref int digitsLimit, ref TextBox[] text, ref bool nowExpTrig, ref bool nowAlg, ref Button[] buttons, ref bool[,,] operations, ref Label label10)
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
        public static void AlgForm(ref int digitsLimit, ref TextBox[] text, ref List<ComplexAlg> complexAlgDigits, ref List<ComplexExpTrig> complexExpTrigDigits)
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
        public static void TrigForm(ref int digitsLimit, ref TextBox[] text, ref List<ComplexAlg> complexAlgDigits, ref List<ComplexExpTrig> complexExpTrigDigits)
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
        public static void ExpForm(ref int digitsLimit, ref TextBox[] text, ref List<ComplexAlg> complexAlgDigits, ref List<ComplexExpTrig> complexExpTrigDigits)
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
    }
}

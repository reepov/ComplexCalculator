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
    public class EnterDigit
    {
        public static void EnterDigitsInOperation(ref int digitsLimit, ref TextBox[] text, ref List<ComplexAlg> complexAlgDigits, ref List<ComplexExpTrig> complexExpTrigDigits, ref bool nowExpTrig, ref bool nowAlg, ref Button[] buttons, ref Label label10)
        {
            if (digitsLimit > 8) { MessageBox.Show("Нельзя вводить более, чем 9 чисел в операцию"); return; }
            string digit;
            bool firstdigit = text[0].Text != "" && text[1].Text != "";
            bool seconddigit = text[2].Text != "" && text[3].Text != "" && text[4].Text != "";
            bool thirddigit = text[5].Text != "" && text[6].Text != "";
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
    }
}

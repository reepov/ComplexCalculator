using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using dzpigareva.Classes;

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
            EnterDigit.EnterDigitsInOperation(ref digitsLimit, ref text, ref complexAlgDigits, ref complexExpTrigDigits, 
                                              ref nowExpTrig, ref nowAlg, ref buttons, ref label10);
        }
        private void button2_Click(object sender, EventArgs e) // Сложение
        {
            Operations.Summing(ref digitsLimit, ref text, ref nowExpTrig, ref nowAlg, ref buttons, ref operations, ref label10);
        }
        private void button3_Click(object sender, EventArgs e) // Вычитание
        {
            Operations.Substracting(ref digitsLimit, ref text, ref nowExpTrig, ref nowAlg, ref buttons, ref operations, ref label10);
        }
        private void button4_Click(object sender, EventArgs e) // Умножение
        {
            Operations.Multiplying(ref digitsLimit, ref text, ref nowExpTrig, ref nowAlg, ref buttons, ref operations, ref label10);
        }
        private void button5_Click(object sender, EventArgs e) // Деление
        {
            Operations.Dividing(ref digitsLimit, ref text, ref nowExpTrig, ref nowAlg, ref buttons, ref operations, ref label10);
        }
        private void button6_Click(object sender, EventArgs e) // Возведение в целую степень
        {
            Operations.IntPowering(ref digitsLimit, ref text, ref nowExpTrig, ref nowAlg, ref buttons, ref operations, ref label10);
        }
        private void button7_Click(object sender, EventArgs e) // Преобразование в алгебраическую форму
        {
            Operations.AlgForm(ref digitsLimit, ref text, ref complexAlgDigits, ref complexExpTrigDigits);
        }
        private void button8_Click(object sender, EventArgs e) // Преобразование в тригонометрическую форму
        {
            Operations.TrigForm(ref digitsLimit, ref text, ref complexAlgDigits, ref complexExpTrigDigits);
        }
        private void button9_Click(object sender, EventArgs e) // Преобразование в экспоненциальную форму
        {
            Operations.ExpForm(ref digitsLimit, ref text, ref complexAlgDigits, ref complexExpTrigDigits);
        }
        private void button10_Click(object sender, EventArgs e) // Очистить калькулятор
        {
            Actions.Clear(ref complexAlgDigits, ref complexExpTrigDigits, ref label10, ref text, 
                          ref operations, ref digitsLimit, ref nowAlg, ref nowExpTrig, ref buttons);
        }
        private void button11_Click(object sender, EventArgs e) // рассчитать результат в алгебраической форме
        {
            Actions.CountInAlgForm(ref digitsLimit, ref label10, ref operations, ref text, ref complexAlgDigits, ref complexExpTrigDigits);
        }   
        private void button12_Click(object sender, EventArgs e) // рассчитать в тригонометрической форме
        {
            Actions.CountInTrigForm(ref digitsLimit, ref label10, ref operations, ref text, ref complexAlgDigits, ref complexExpTrigDigits);
        }
        private void button13_Click(object sender, EventArgs e) // рассчитать в экспоненциональной форме
        {
            Actions.CountInExpForm(ref digitsLimit, ref label10, ref operations, ref text, ref complexAlgDigits, ref complexExpTrigDigits);
        }
        private void button14_Click(object sender, EventArgs e)
        {
            Process.Start("protocol.txt");
        }
        private void button15_Click(object sender, EventArgs e) // очистить протокол
        {
            Actions.ProtocolClean();
        }
    }
}

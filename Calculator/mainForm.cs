using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            KeyPreview = true;
        }

        private string equation = string.Empty;
        private bool wasAnsClicked = false;

        /* maximum input length before expr get out of boundaries */
        private int maxInputLength(int displayWidth)
        {
            return (int)Math.Ceiling(displayWidth / 21.5);
        }

        private bool isEndsWithOper()
        {
            return this.displayLabel.Text.EndsWith("+") ||
                   this.displayLabel.Text.EndsWith("-") ||
                   this.displayLabel.Text.EndsWith("*") ||
                   this.displayLabel.Text.EndsWith("/");
        }

        private bool isEndsWithDot()
        {
            return this.displayLabel.Text.EndsWith(".");
        }

        /* if input length is less than max length of display */
        private bool isLessThanMaxLength()
        {
            return this.displayLabel.Text.Length < 
                   maxInputLength(this.displayLabel.Width);
        }

        /* if equation is the smallest possible one with 2 operands and 1 operation */
        private bool isSmallestPossibleEquation(string equation)
        {
            return equation.Length >= 3 &&
                   Char.IsDigit(equation[0]) &&
                   Char.IsDigit(equation[equation.Length - 1]) &&
                   (equation.Contains('+') ||
                    equation.Contains('-') ||
                    equation.Contains('*') ||
                    equation.Contains('/'));
        }

        /* trims equation before calculation from trailing operation and dot */
        private string trimEquation(string rawEquation)
        {
            string eq = rawEquation;

            if (isEndsWithOper() || isEndsWithDot())
            {
                eq = rawEquation.TrimEnd('+', '-', '*', '/', '.');
            }
            eq = eq.Trim(' ');

            return eq;
        }

        /* is token is operator */
        private bool isOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }

        /* operator 2 is greater or equal precedence than token*/
        private bool isOp2GEP(char token, char op2)
        {
            if (op2 == '*' || op2 == '/')
                return true;
            if ((op2 == '+' || op2 == '-') && (token == '/' || token == '*'))
                return false;
            return true;
        }

        /* parse equantion using shunting-yard algorithm */
        private Queue<string> shuntingYard(string equation)
        {
            string[] tokens = equation.Split(' ');
            Stack<string> opers = new Stack<string>();
            Queue<string> result = new Queue<string>();

            foreach (string token in tokens)
            {
                if (isOperator(token))
                {
                    if (opers.Count != 0)
                    {
                        char op2 = Convert.ToChar(opers.Peek());
                        if (isOp2GEP(Convert.ToChar(token), op2))
                        {
                            result.Enqueue(opers.Pop());
                        }
                    }
                    opers.Push(token);
                }
                else
                {
                    result.Enqueue(token);
                }
            }

            while(opers.Count != 0)
            {
                string op = opers.Pop();
                result.Enqueue(op);
            }

            return result;
        }

        /* calculate equation using postfix evaluation algorithm */
        private double calculate(string equation)
        {
            Queue<string> postfix = shuntingYard(equation);
            Stack<double> stack = new Stack<double>();

            foreach (string token in postfix)
            {
                // if token is an operand
                if(!isOperator(token))
                {
                    stack.Push(Convert.ToDouble(token));
                }
                else
                {
                    double op1 = stack.Pop();
                    double op2 = stack.Pop();
                    double result = 0;
                    switch(token)
                    {
                        case "+": result = op2 + op1; break;
                        case "-": result = op2 - op1; break;
                        case "*": result = op2 * op1; break;
                        case "/":
                            if(op1 == 0)
                            {
                                throw new System.DivideByZeroException();
                            }
                            result = op2 / op1;
                            break;
                        default: break;
                    }
                    stack.Push(result);
                }
            }

            return stack.Pop();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            /* disable Maximize button */
            this.MaximizeBox = false;
        }

        private void onOperButtonClick(object sender, EventArgs e)
        {
            if (!isEndsWithOper() && !isEndsWithDot() && isLessThanMaxLength())
            {
                wasAnsClicked = false;
                this.displayLabel.Text += (sender as Button).Text;
                equation += ' ' + (sender as Button).Text;
            }
        }

        private void onDigButtonClick(object sender, EventArgs e)
        {
            if (!wasAnsClicked)
            {
                if (this.displayLabel.Text == "0")
                {
                    this.displayLabel.Text = "";
                    equation = "";
                }

                if (isLessThanMaxLength())
                {
                    this.displayLabel.Text += (sender as Button).Text;
                    if (equation.Length > 0 &&
                        (Char.IsDigit(equation[equation.Length - 1]) ||
                         equation[equation.Length - 1] == '.')
                       )
                    {
                        equation += (sender as Button).Text;
                    }
                    else
                    {
                        equation += ' ' + (sender as Button).Text;
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.displayLabel.Text = "0";
            equation = "";
        }

        private void answerButton_Click(object sender, EventArgs e)
        {
            wasAnsClicked = true;
            string trimedEquation = trimEquation(equation);
            this.displayLabel.Text = calculate(trimedEquation).ToString();
            equation = this.displayLabel.Text;
            Clipboard.SetText(equation);
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            if (!isEndsWithOper() && !isEndsWithDot() && isLessThanMaxLength() && !wasAnsClicked)
            {
                this.displayLabel.Text += (sender as Button).Text;
                equation += (sender as Button).Text;
            }
        }

        /* keys press handling */
        private void onKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    btn0.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    btn1.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    btn2.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    btn3.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    btn4.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    btn5.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    btn6.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    btn7.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    btn8.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    btn9.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.Back:
                case Keys.Delete:
                    btnClear.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.Decimal:
                case Keys.OemPeriod:
                    btnDot.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.Add:
                case Keys.Oemplus:
                    btnPlus.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.Subtract:
                case Keys.OemMinus:
                    btnMinus.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.Multiply:
                    btnMul.PerformClick();
                    e.Handled = true;
                    break;
                case Keys.Divide:
                case Keys.OemQuestion:
                    btnDiv.PerformClick();
                    e.Handled = true;
                    break;
            }
        }
    }
}

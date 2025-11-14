using System;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private double storedValue = 0;
        private string operation = "";
        private bool operationPressed = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string number = button.Content.ToString();

            if (txtDisplay.Text == "0" || operationPressed)
            {
                txtDisplay.Text = number;
                operationPressed = false;
            }
            else
            {
                txtDisplay.Text += number;
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string op = button.Content.ToString();

            // Convert display symbols to internal operators
            switch (op)
            {
                case "÷":
                    operation = "/";
                    break;
                case "×":
                    operation = "*";
                    break;
                case "−":
                    operation = "-";
                    break;
                default:
                    operation = op;
                    break;
            }

            if (double.TryParse(txtDisplay.Text, out double result))
            {
                storedValue = result;
                operationPressed = true;
            }
        }

        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            if (operation != "" && double.TryParse(txtDisplay.Text, out double currentValue))
            {
                double result = 0;

                try
                {
                    switch (operation)
                    {
                        case "+":
                            result = storedValue + currentValue;
                            break;
                        case "-":
                            result = storedValue - currentValue;
                            break;
                        case "*":
                            result = storedValue * currentValue;
                            break;
                        case "/":
                            if (currentValue != 0)
                            {
                                result = storedValue / currentValue;
                            }
                            else
                            {
                                MessageBox.Show("Cannot divide by zero!", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                                txtDisplay.Text = "0";
                                operation = "";
                                storedValue = 0;
                                return;
                            }
                            break;
                    }

                    // Format the result to avoid scientific notation and excessive decimals
                    if (Math.Abs(result) < 1e-10)
                    {
                        txtDisplay.Text = "0";
                    }
                    else if (result % 1 == 0 && Math.Abs(result) < 1e10)
                    {
                        txtDisplay.Text = result.ToString("0");
                    }
                    else
                    {
                        txtDisplay.Text = result.ToString("G10");
                    }

                    operation = "";
                    operationPressed = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Calculation error: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    txtDisplay.Text = "0";
                    operation = "";
                    storedValue = 0;
                }
            }
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (operationPressed)
            {
                txtDisplay.Text = "0.";
                operationPressed = false;
            }
            else if (!txtDisplay.Text.Contains("."))
            {
                txtDisplay.Text += ".";
            }
        }

        private void Function_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string function = button.Content.ToString();

            switch (function)
            {
                case "C": // Clear all
                    txtDisplay.Text = "0";
                    storedValue = 0;
                    operation = "";
                    operationPressed = false;
                    break;

                case "CE": // Clear entry
                    txtDisplay.Text = "0";
                    break;

                case "←": // Backspace
                    if (txtDisplay.Text.Length > 1)
                    {
                        txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);
                    }
                    else
                    {
                        txtDisplay.Text = "0";
                    }
                    break;

                case "±": // Toggle sign
                    if (txtDisplay.Text != "0" && double.TryParse(txtDisplay.Text, out double value))
                    {
                        txtDisplay.Text = (-value).ToString();
                    }
                    break;
            }
        }
    }
}
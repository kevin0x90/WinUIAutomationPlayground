using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using WinUIAutomationPlayground.AutomationExtensions;

namespace WinUIAutomationPlayground.Calculator
{
  public sealed class Calculator : IDisposable
  {
    private readonly Process _calculatorProcess;

    private readonly AutomationElement _calculatorAutomationElement;

    private readonly AutomationElement _calculatorResultElement;

    public Calculator()
    {
      Process.Start("Calc.exe");

      Thread.Sleep(500);

      _calculatorProcess = Process.GetProcessesByName("calculator")[0];
      _calculatorAutomationElement = ElementFindExtensions.FindMainAppElement("Calculator");
      _calculatorResultElement = _calculatorAutomationElement.FindByAutomationId("CalculatorResults") ?? throw new Exception();
    }

    public int ReadResult() => Convert.ToInt32(_calculatorResultElement.GetValue());

    public void InvokeDigitButton(int number)
    {
      if (number < 0 || number > 9)
      {
        throw new InvalidOperationException("number must be a digit 0-9");
      }

      AutomationElement buttonElement = _calculatorAutomationElement.FindByName(GetDigitName(number))
        ?? throw new InvalidOperationException($"Could not find button corresponding to digit {number}");

      GetInvokePattern(buttonElement).Invoke();

      static string GetDigitName(int number) => number switch
      {
        0 => "Zero",
        1 => "One",
        2 => "Two",
        3 => "Three",
        4 => "Four",
        5 => "Five",
        6 => "Six",
        7 => "Seven",
        8 => "Eight",
        9 => "Nine",
        _ => throw new Exception("Invalid digit")
      };
    }

    public void InvokeMultiply() => InvokeFunction(Functions.Multiply);

    public void InvokeDivide() => InvokeFunction(Functions.Divide);

    public void InvokeMinus() => InvokeFunction(Functions.Minus);

    public void InvokePlus() => InvokeFunction(Functions.Plus);

    public void InvokeEvaluate() => InvokeFunction(Functions.Equals);

    public void InvokeClear() => InvokeFunction(Functions.Clear);

    private void InvokeFunction(string functionName)
    {
      GetInvokePattern(GetFunctionButton(functionName)).Invoke();
    }

    private AutomationElement GetFunctionButton(string functionName)
    {
      return _calculatorAutomationElement.FindByName(functionName)
        ?? throw new InvalidOperationException($"No function button found with name: {functionName}");
    }

    private static InvokePattern GetInvokePattern(AutomationElement element)
    {
      var pattern = element.GetCurrentPattern(InvokePattern.Pattern);

      return (InvokePattern)pattern;
    }

    public void Dispose()
    {
      _calculatorProcess.Kill();
      _calculatorProcess.Dispose();
    }
  }
}

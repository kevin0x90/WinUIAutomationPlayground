using WinUIAutomationPlayground.Calculator;

namespace CalculatorTest
{
  [TestClass]
  public class BasicCalculatorTest
  {
    [TestMethod]
    public void BasicAddition()
    {
      // Arrange
      using var calculator = new Calculator();

      // Act
      calculator.InvokeClear();
      calculator.InvokeDigitButton(1);
      calculator.InvokePlus();
      calculator.InvokeDigitButton(2);
      calculator.InvokeEvaluate();

      // Assert
      Assert.AreEqual(3, calculator.ReadResult());
    }
  }
}
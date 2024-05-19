using System;
using System.Threading;
using System.Windows.Automation;

namespace WinUIAutomationPlayground.AutomationExtensions
{
  internal static class ElementFindExtensions
  {
    public static AutomationElement FindMainAppElement(ReadOnlySpan<char> name)
    {
      TimeSpan waitTime = TimeSpan.FromMilliseconds(100);
      const uint MaxTryCount = 50u;

      AutomationElement appElement;
      uint tryCount = 0;

      do
      {
        appElement = AutomationElement.RootElement.FindFirst(TreeScope.Children,
          new PropertyCondition(AutomationElement.NameProperty, name.ToString()));

        ++tryCount;
        Thread.Sleep(waitTime);
      }
      while (appElement == null && tryCount < MaxTryCount);


      if (appElement == null)
      {
        throw new InvalidOperationException($"{name} not found application must be running");
      }

      return appElement;
    }

    public static AutomationElement? FindByAutomationId(this AutomationElement element, ReadOnlySpan<char> automationId)
    {
      return element.FindFirst(TreeScope.Descendants,
        new PropertyCondition(AutomationElement.AutomationIdProperty, automationId.ToString()));
    }

    public static AutomationElement? FindByName(this AutomationElement element, ReadOnlySpan<char> name)
    {
      return element.FindFirst(TreeScope.Descendants,
        new PropertyCondition(AutomationElement.NameProperty, name.ToString()));
    }

    public static string GetValue(this AutomationElement element)
    {
      ReadOnlySpan<char> prefix = ['D', 'i', 's', 'p', 'l', 'a', 'y', ' ', 'i', 's', ' '];

      return element.Current.Name[prefix.Length..];
    }
  }
}

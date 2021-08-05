using Core.Interfaces;
using System;
using System.Diagnostics;

namespace Core
{
    public class Notifier1 : INotifier
    {
        public bool CanRun()
        {
            throw new NotImplementedException();
        }

        public void Notify()
        {
            Debug.WriteLine("Debugging from Notifier 1");
        }
    }
}

using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core
{
    public class NotifierMediatorService : INotifierMediatorService
    {
        private readonly IEnumerable<INotifier> _notifiers;

        public NotifierMediatorService(IEnumerable<INotifier> notifiers)
        {
            _notifiers = notifiers;
        }

        public void Notify()
        {
            _notifiers.ToList().ForEach(x => x.Notify());
        }
    }

}

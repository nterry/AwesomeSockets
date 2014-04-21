using AwesomeSockets.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwesomeSockets.Domain.SocketModifiers
{
    public abstract class AbstractSocketModifier
    {
        public dynamic[] Conflicts { get; private set; }

        public AbstractSocketModifier(List<dynamic> type)
        {
            Conflicts = type.ToArray();
            type.ForEach(x => CheckConflicts(type));
        }

        private bool CheckConflicts<T>(T type)
        {
            if (Conflicts.Contains(typeof(T))) 
                throw new SocketModifierException(string.Format("Modifier {0} conflicts with modifier {1} already applied!", typeof(T), this.GetType()));
            return true;
        }
    }
}

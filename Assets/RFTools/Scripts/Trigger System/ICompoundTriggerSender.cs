using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger {
    public interface ICompoundTriggerSender
    {
        IEnumerable<TriggerSend> GetCompoundSends();
    }
}
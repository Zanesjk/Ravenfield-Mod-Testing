using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Trigger
{
    [System.Serializable]
    public struct TriggerSend
    {
        public List<TriggerReceiver> destinations;

		public struct DocInfo
		{
			public SignalDocAttribute signalDoc;
			public string name;

			public DocInfo(string name, SignalDocAttribute signalDocAttribute) : this() {

				if (!string.IsNullOrEmpty(signalDocAttribute.overrideName)) {
					this.name = signalDocAttribute.overrideName;
				}
				else {
					this.name = FormatName(name);
				}

				this.signalDoc = signalDocAttribute;
			}

			string FormatName(string fieldName) {
				var chars = new List<char>(fieldName);
				chars[0] = char.ToUpperInvariant(chars[0]);

				for (int i = 1; i < chars.Count; i++) {
					if (chars[i - 1] != ' ' && char.IsUpper(chars[i]) && !char.IsUpper(chars[i + 1])) {
						chars.Insert(i, ' ');
						i++;
					}
				}

				return new string(chars.ToArray());
			}
		}
	}
}

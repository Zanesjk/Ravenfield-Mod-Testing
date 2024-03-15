using System;

namespace Ravenfield.Trigger
{
	public class SignalDocAttribute : Attribute
	{
		public SignalDocAttribute(string doc) {
			this.doc = doc;
		}

		public string doc, actor, vehicle, squad, weapon, overrideName;
	}
}
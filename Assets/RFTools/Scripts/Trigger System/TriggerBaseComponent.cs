using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

namespace Ravenfield.Trigger
{
    public abstract class TriggerBaseComponent : MonoBehaviour
    {
        static Dictionary<System.Type, IEnumerable<FieldInfo>> sendFieldCache = new Dictionary<System.Type, IEnumerable<FieldInfo>>();

        static IEnumerable<FieldInfo> GetReflectionSendFields(System.Type type) {
            if(!sendFieldCache.ContainsKey(type)) {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(field => field.FieldType == typeof(TriggerSend));
                sendFieldCache.Add(type, fields);
            }
            return sendFieldCache[type];
		}

		public IEnumerable<TriggerSend.DocInfo> GetSendInfos() {
			var sendFields = GetReflectionSendFields(GetType());

			foreach (var fieldInfo in sendFields) {

				var matches = fieldInfo.GetCustomAttributes(typeof(SignalDocAttribute), false);
				if (matches.Length > 0) {
					yield return new TriggerSend.DocInfo(fieldInfo.Name, (SignalDocAttribute)matches[0]);
				}
			}
		}

		public IEnumerable<TriggerSend> GetSends() {
            var sendFields = GetReflectionSendFields(GetType());

            foreach(var fieldInfo in sendFields) {
                yield return (TriggerSend) fieldInfo.GetValue(this);
			}

            var compoundSender = this as ICompoundTriggerSender;
            if(compoundSender != null) {
                foreach(var send in compoundSender.GetCompoundSends()) {
                    yield return send;
				}
            }
		}

		public override string ToString() {
            return $"{this.gameObject.name} ({GetType().Name})";
		}
	}
}

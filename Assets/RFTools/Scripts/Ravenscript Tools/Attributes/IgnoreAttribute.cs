using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lua
{
	[System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = false)]
	public sealed class IgnoreAttribute : System.Attribute
	{
		public bool ignoreBase { get; set; }
	}
}

namespace Ravenfield.Trigger
{
	[System.Serializable]
	public struct WeaponState
	{
		public bool overrideDefault;
		[ConditionalField("overrideDefault", true)] public int loadedAmmo;
		[ConditionalField("overrideDefault", true)] public int spareAmmo;
	}
}

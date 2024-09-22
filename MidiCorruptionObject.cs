using System;
using UnityEngine;

// Token: 0x020001F5 RID: 501
[CreateAssetMenu(fileName = "Midi Corruption Object", menuName = "Custom Assets/Midi Corruption Object", order = 12)]
public class MidiCorruptionObject : ScriptableObject
{
	// Token: 0x04000D70 RID: 3440
	public MidiChannelCorruptionData[] corruptions = new MidiChannelCorruptionData[16];
}

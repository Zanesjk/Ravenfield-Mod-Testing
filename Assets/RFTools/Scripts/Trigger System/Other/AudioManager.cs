using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public enum AudioSnapshot
    {
        Default,
        Enclosed,
        Deaf,
        GameEnd,
        Zero,
        ReducedIngame,
    }
}

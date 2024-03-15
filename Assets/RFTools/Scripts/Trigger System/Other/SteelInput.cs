using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Linq;
using Steamworks;

[System.Serializable]
public class SteelInput {
    public enum KeyBinds {
        Horizontal, Vertical,
        Fire, Aim,
        Lean,
        Reload, Use,
        Crouch, Sprint, Jump,
        Weapon1, Weapon2, Weapon3, Weapon4, Weapon5, NextWeapon,
        OpenLoadout, Map,
        AimX, AimY,
        Kick, Slowmotion,
        CarSteer, CarThrottle,
        HeliPitch, HeliYaw, HeliRoll, HeliThrottle, 
        PlanePitch, PlaneYaw, PlaneRoll, PlaneThrottle,
        PreviousWeapon,
        Call, SquadLeaderKit,
        Goggles,
        ThirdPersonToggle, Countermeasures,
        Scoreboard,
        Prone,
        FireMode, NextScope, PreviousScope, ScopeModifier,
        Console, ReloadScripts,
        AutoHover,

        TogglePauseMenu, PeekScoreboard,

        Seat1, Seat2, Seat3, Seat4, Seat5, Seat6, Seat7
    };
}

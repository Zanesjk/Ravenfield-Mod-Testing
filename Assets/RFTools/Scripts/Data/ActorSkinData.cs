using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ravenfield.Data
{
    [CreateAssetMenu(menuName = "Actor Skin Data", fileName = "NewActorSkin")]
    public class ActorSkinData : ScriptableObject
    {
        public ActorSkin actorSkin;
    }
}
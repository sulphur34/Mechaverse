using UnityEngine;
using Sprite = UnityEngine.ProBuilder.Shapes.Sprite;

namespace EntityActors
{
    public class LevelActor
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
    }
}
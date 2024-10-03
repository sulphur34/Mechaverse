using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Utilitiy
{
    public class OffsetKeeper : MonoBehaviour
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private Vector3 _offset = Vector3.right;

        private void Update()
        {
            transform.position = _parent.position + _offset;
        }
    }
}
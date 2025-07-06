using System;
using UnityEngine;

namespace Descensus
{
    public class FollowPlayer : MonoBehaviour
    {
        private Transform _follow;

        public void SetFollow(Transform follow)
        {
            _follow = follow;    
        }
        
        public void UpdatePosition()
        {
            if (!_follow) return;
            
            // Just updating y position so camera will move only down
            transform.position = new Vector3(0, _follow.position.y, _follow.position.z);
        }
    }
}

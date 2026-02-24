using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class TeamComponent : MonoBehaviour
    {
        public TeamType TeamType => _teamType;
        
        [SerializeField]
        private TeamType _teamType;

        public bool IsFriendly(TeamComponent teamComponent)
        {
            if (teamComponent == null)
                throw new ArgumentNullException(nameof(teamComponent));

            return teamComponent.TeamType == _teamType;
        }
    }
}
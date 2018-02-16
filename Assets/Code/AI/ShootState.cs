using System;
using System.Collections;
using System.Collections.Generic;
using TankGame.Systems;
using UnityEngine;

namespace TankGame.AI
{
    public class ShootState : AIStateBase
    {
        public ShootState( EnemyUnit owner )
          : base( owner, AIStateType.Shoot )
        {
            AddTransition(AIStateType.FollowTarget);
        }

        public override void StateActivated()
        {
            base.StateActivated();
        }

        public override void Update()
        {
            if (!ChangeState())
            {
                Owner.Weapon.Shoot();
            }
        }

        private bool ChangeState()
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int mask = Flags.CreateMask(playerLayer);

            Collider[] players = Physics.OverlapSphere(Owner.transform.position,
                Owner.SqrShootingDistance, mask);

            if (players.Length < 1)
            {
                return Owner.PerformTransition(AIStateType.FollowTarget);
            }

            return false;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TankGame.Systems;
using UnityEngine;

namespace TankGame.AI
{
    public class FollowTargetState : AIStateBase
    {

        public FollowTargetState( EnemyUnit owner )
          : base( owner, AIStateType.FollowTarget )
        {
            AddTransition( AIStateType.Patrol );
            AddTransition( AIStateType.Shoot );
        }

        public override void StateActivated()
        {
            base.StateActivated();
        }

        public override void Update()
        {
            if(!ChangeState())
            {
                Owner.Mover.Move(Owner.transform.forward);
                Owner.Mover.Turn(Owner.Target.transform.position);
            }
        }

        private bool ChangeState()
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int mask = Flags.CreateMask(playerLayer);

            Collider[] players = Physics.OverlapSphere(Owner.transform.position,
                Owner.DetectEnemyDistance, mask);

            if (players.Length < 1)
            {
                Owner.Target = null;
                return Owner.PerformTransition(AIStateType.Patrol);
            }

            Vector3 playerVector = Owner.Target.transform.position - Owner.transform.position;

            Debug.Log(playerVector);
            if(playerVector == Owner.transform.forward)

            players = Physics.OverlapSphere(Owner.transform.position,
                Owner.SqrShootingDistance, mask);

            if (players.Length > 0)
            {
                return Owner.PerformTransition(AIStateType.Shoot);
            }

            return false;
        }
    }
}

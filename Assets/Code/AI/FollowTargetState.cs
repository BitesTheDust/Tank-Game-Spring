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
            // 1. Are we at the shooting range?
            // If yes, go to shoot state.
            float sqrDistanceToTarget = Owner.ToTargetVector.Value.sqrMagnitude;
            if (sqrDistanceToTarget < Owner.SqrShootingDistance)
            {
                return Owner.PerformTransition(AIStateType.Shoot);
            }

            // 2. Did the player get away?
            // If yes, go to patrol state.
            if (sqrDistanceToTarget > Owner.SqrDetectEnemyDistance) 
            {
                Owner.Target = null;
                return Owner.PerformTransition(AIStateType.Patrol);
            }

            return false;
        }
    }
}

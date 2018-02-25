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

        /// <summary>
        /// Checks state change every frame.
        /// </summary>
        public override void Update()
        {
            if(!ChangeState())
            {
                Owner.Mover.Move(Owner.transform.forward);
                Owner.Mover.Turn(Owner.Target.transform.position);
            }
        }
        
        /// <summary>
		/// Attempts to change state from Follow Target to either Shoot or Patrol.
		/// </summary>
		/// <returns>True, if the state change is successful.
        /// False otherwise.</returns>
        private bool ChangeState()
        {
            // Get layermask for player unit.
            int playerLayer = LayerMask.NameToLayer("Player");
            int mask = Flags.CreateMask(playerLayer);

            // A ray from owner to their forward vector.
            Ray toPlayerRay = new Ray( Owner.transform.position, Owner.transform.forward );
            RaycastHit hit;

            // Raycast in front of owner the length of ShootingDistance on player layer.
            // If player was hit ...
            if ( Physics.Raycast( toPlayerRay, out hit, Owner.ShootingDistance, mask ) ) 
            {
                // Go to Shoot state.
                return Owner.PerformTransition(AIStateType.Shoot);
            }

            // Get squared distance to target
            float sqrDistanceToTarget = Owner.ToTargetVector.Value.sqrMagnitude;
            
            // If squared distance to target is greater than squared enemy detection distance ...
            if (sqrDistanceToTarget > Owner.SqrDetectEnemyDistance) 
            {
                // Forget target & go to Patrol state.
                Owner.Target = null;
                return Owner.PerformTransition(AIStateType.Patrol);
            }

            return false;
        }
    }
}

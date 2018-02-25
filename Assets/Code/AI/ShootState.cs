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
            // Add transition back to Patrol and Follow Target.
            AddTransition(AIStateType.FollowTarget);
            AddTransition(AIStateType.Patrol);
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
            // If state is not changed ...
            if (!ChangeState())
            {
                // Shoot with weapon.
                Owner.Weapon.Shoot();
            }
        }

        /// <summary>
		/// Attempts to change state from Shoot to Patrol or Follow Target.
		/// </summary>
		/// <returns>True, if the state change is successful.
        /// False otherwise.</returns>
        private bool ChangeState()
        {
            // If target is dead ...
            if ( Owner.Target.Health.CurrentHealth <= 0 ) 
            {
                // Forget target & change state to Patrol.
                Owner.Target = null;
                return Owner.PerformTransition(AIStateType.Patrol);
            }

            // Bool CanShoot is publicly available.
            // If weapon can't shoot yet ...
            if ( !Owner.Weapon.CanShoot ) 
            {
                // Change state to Follow Target.
                return Owner.PerformTransition(AIStateType.FollowTarget);
            }

            return false;
        }
    }
}

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
            // Add transition back to Follow Target
            AddTransition(AIStateType.FollowTarget);
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
                // Shoot with weapon
                Owner.Weapon.Shoot();
            }
        }

        /// <summary>
		/// Attempts to change state from Shoot to Follow Target.
		/// </summary>
		/// <returns>True, if the state change is successful.
        /// False otherwise.</returns>
        private bool ChangeState()
        {
            // Bool CanShoot is publicly available
            // If weapon can't shoot yet ...
            if ( !Owner.Weapon.CanShoot ) 
            {
                // Change state to Follow Target
                return Owner.PerformTransition(AIStateType.FollowTarget);
            }

            return false;
        }
    }
}

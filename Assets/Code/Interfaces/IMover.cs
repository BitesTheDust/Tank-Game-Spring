using UnityEngine;

namespace TankGame
{
	public interface IMover
	{
		/// <summary>
		/// Initializes the Mover
		/// </summary>
		/// <param name="moveSpeed">Move speed (units/s)</param>
		/// <param name="turnSpeed">Turn speed (degrees/s)</param>
		void Init( float moveSpeed, float turnSpeed );

		void Move( float amount );
        void Move( Vector3 direction);
		void Turn( float amount );
        void Turn(Vector3 target);
	}
}

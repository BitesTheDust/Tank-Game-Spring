using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TankGame.WaypointSystem;
using TankGame.Systems;
using UnityEngine;

namespace TankGame.AI
{
	public class PatrolState : AIStateBase
	{
		private Path _path;
		private Direction _direction;
		private float _arriveDistance;

		public Waypoint CurrentWaypoint { get; private set; }

		public PatrolState( EnemyUnit owner, Path path, Direction direction, float arriveDistance )
			: base()
		{
			State = AIStateType.Patrol;
			Owner = owner;
			AddTransition( AIStateType.FollowTarget );
			_path = path;
			_direction = direction;
			_arriveDistance = arriveDistance;
		}

		public override void StateActivated()
		{
			base.StateActivated();
			CurrentWaypoint = _path.GetClosestWaypoint( Owner.transform.position );
		}

		public override void Update()
		{
			if(!ChangeState())
            {
                CurrentWaypoint = GetWaypoint();

                Owner.Mover.Move(Owner.transform.forward);
                Owner.Mover.Turn(CurrentWaypoint.Position);
            }
		}

        private bool ChangeState()
        {
            //int mask = LayerMask.GetMask("Player");
            int playerLayer = LayerMask.NameToLayer("Player");
            int mask = Flags.CreateMask(playerLayer);

            Collider[] players = Physics.OverlapSphere(Owner.transform.position,
                Owner.DetectEnemyDistance, mask);

            if(players.Length > 0)
            {
                PlayerUnit player = players[0].gameObject.GetComponentInHierarchy<PlayerUnit>();

                if( player != null)
                {
                    Owner.Target = player;

                    var sqrDistanceToPlayer = Owner.ToTargetVector.Value.sqrMagnitude;
                    
                    if( sqrDistanceToPlayer < Owner.SqrDetectEnemyDistance)
                        return Owner.PerformTransition(AIStateType.FollowTarget);

                    Owner.Target = null;
                }


            }

            return false;
        }

        private Waypoint GetWaypoint()
        {
            Waypoint result = CurrentWaypoint;
            Vector3 toWaypointVector = CurrentWaypoint.Position - Owner.transform.position;
            float toWaypointSqr = toWaypointVector.sqrMagnitude;
            float sqrArriveDistance = _arriveDistance * _arriveDistance;

            if(toWaypointSqr <= sqrArriveDistance)
            {
                result = _path.GetNextWaypoint(CurrentWaypoint, ref _direction);
            }

            return result;
        }
	}
}

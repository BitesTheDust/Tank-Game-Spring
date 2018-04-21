using System;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame
{
	public class Projectile : MonoBehaviour
	{
		[SerializeField, Range( 0, 100 )] private int _damage;
		[SerializeField] private float _shootingForce;
		[SerializeField] private float _explosionForce;
		[SerializeField] private float _explosionRadius;
        [SerializeField, HideInInspector] private int _hitMask;
		
		private Rigidbody _rigidbody;
		private System.Action<Projectile> _collisionCallback;

		[SerializeField] private ProjectileType _type;
		
		public enum ProjectileType 
		{
			None = 0,
			Player = 1,
			Enemy = 1 << 1,
			Neutral = 1 << 2,
		}

		// Self initializing property. Gets the reference to the Rigidbody component when
		// used the first time.
		public Rigidbody Rigidbody
		{
			get
			{
				if ( _rigidbody == null )
				{
					_rigidbody = gameObject.GetOrAddComponent< Rigidbody >();
				}
				return _rigidbody;
			}
		}

		public void Init( System.Action< Projectile > collisionCallback )
		{
			_collisionCallback = collisionCallback;
		}

		public void Launch( Vector3 direction )
		{
			// TODO: Add particle effects.
			Rigidbody.AddForce( direction.normalized * _shootingForce, ForceMode.Impulse );
		}

		protected void OnCollisionEnter( Collision collision )
		{
			Debug.Log(collision.collider);
            // TODO: Add particle effects.
            // TODO: Apply damage to enemies.
            ApplyDamage();
			Rigidbody.velocity = Vector3.zero;
			_collisionCallback( this );
		}

        private void ApplyDamage()
        {
			List<IDamageReceiver> alreadyDamaged = new List< IDamageReceiver >();
            Collider[] damageReceivers = Physics.OverlapSphere(transform.position,
                _explosionRadius, _hitMask);

				Debug.Log( damageReceivers.Length );
            for(int i = 0; i < damageReceivers.Length; i++)
            {
                IDamageReceiver damageReceiver = damageReceivers[i].GetComponentInParent<IDamageReceiver>();

                if(damageReceiver != null && !alreadyDamaged.Contains( damageReceiver ))
                {
					Debug.Log("We damaged " + damageReceiver);
                    damageReceiver.TakeDamage( _damage );
                    // TODO: Apply explosion force
					alreadyDamaged.Add( damageReceiver );
                }
            }
        }
	}
}

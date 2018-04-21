using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TankGame.Persistence 
{
	public class SaveSystem
	{
		private IPersistence _persistence;

		public SaveSystem( IPersistence persistence ) 
		{
			_persistence = persistence;
		}

		public void Save( GameData data ) 
		{
			_persistence.Save( data );
		}

		public GameData Load() 
		{
			return _persistence.Load<GameData>();
		}
	}
}

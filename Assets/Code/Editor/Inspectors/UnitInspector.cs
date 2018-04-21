using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TankGame.Editor 
{
	[CustomEditor(typeof(Unit), true)]
	public class UnitInspector : UnityEditor.Editor
	{
		protected virtual void OnEnable() 
		{
			Unit unit = target as Unit;

			if( unit != null && unit.Id < 0 ) 
			{
				Undo.RecordObject( unit, "Set id for the unit." );
				unit.RequestId();
			}
		}
	}
}

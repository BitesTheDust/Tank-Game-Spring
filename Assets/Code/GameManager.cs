﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TankGame.Persistence;
using TankGame.Messaging;
using TankGame.Localization;
using L10n = TankGame.Localization.Localization;

namespace TankGame 
{
	public class GameManager : MonoBehaviour 
	{
		#region Statics
		private static GameManager _instance;

		public static GameManager Instance 
		{
			get 
			{
				if ( _instance == null && !IsClosing ) 
				{
					GameObject gameManagerObject = new GameObject(typeof(GameManager).Name);
					_instance = gameManagerObject.AddComponent<GameManager>();
				}

				return _instance;
			}
		}

		public static bool IsClosing { get; private set; }

		#endregion Statics

		private List<Unit> _enemyUnits = new List<Unit>();
		private Unit _playerUnit = null;
		private SaveSystem _saveSystem;
		private const string LanguageKey = "Language";

		public string SavePath { get { return Path.Combine( Application.persistentDataPath, "save" ); } }

		public MessageBus MessageBus { get; private set; }
		protected void Awake() 
		{
			if ( _instance == null ) 
			{
				_instance = this;
			}
			else if( _instance != this) 
			{
				Destroy( gameObject );
				return;
			}

			Init();
		}

		private void OnApplicationQuit() 
		{
			IsClosing = true;
		}

		private void OnDestroy() 
		{
			L10n.LanguageLoaded -= OnLanguageLoaded;
		}

		private void Init() 
		{
			InitLocalization();
			IsClosing = false;
			MessageBus = new MessageBus();

			var UI = FindObjectOfType< UI.UI >();
			UI.Init();

			Unit[] allUnits = FindObjectsOfType<Unit>();

			foreach( Unit unit in allUnits ) 
			{
				AddUnit(unit);
			}

			_saveSystem = new SaveSystem( new JSONPersistence( SavePath ) );
		}

		private void InitLocalization() 
		{
			LangCode currentLang = (LangCode) PlayerPrefs.GetInt( LanguageKey, (int) LangCode.EN );
			L10n.LoadLanguage( currentLang );
			L10n.LanguageLoaded += OnLanguageLoaded;
		}

		private void OnLanguageLoaded( LangCode currentLanguage )
		{
			PlayerPrefs.SetInt( LanguageKey, (int) currentLanguage );
		}

		public void AddUnit(Unit unit) 
		{
			unit.Init();

			if( unit is EnemyUnit ) 
			{
				_enemyUnits.Add(unit);
			}
			else if( unit is PlayerUnit && _playerUnit == null) 
			{
				_playerUnit = unit;
			}

			// Add unit's health to the UI.
			UI.UI.Current.HealthUI.AddUnit( unit );
		}

		public void Save() 
		{
			GameData data = new GameData();
			foreach( Unit unit in _enemyUnits) 
			{
				data.EnemyData.Add( unit.GetUnitData() );
			}
			data.PlayerData = _playerUnit.GetUnitData();

			_saveSystem.Save( data );
		}

		public void Load() 
		{
			GameData data = _saveSystem.Load();
			foreach( UnitData enemyData in data.EnemyData ) 
			{
				Unit enemy = _enemyUnits.FirstOrDefault( unit => unit.Id == enemyData.Id );
				if( enemy != null ) 
				{
					enemy.SetUnitData( enemyData );
				}
			}

			_playerUnit.SetUnitData( data.PlayerData );
		}

		private void Update() 
		{
			bool save = Input.GetKeyDown( KeyCode.F5 );
			bool load = Input.GetKeyDown( KeyCode.F4 );

			if( save && !load ) 
			{
				Save();
			}

			if( !save && load ) 
			{
				Load();
			}
		}
	}
}

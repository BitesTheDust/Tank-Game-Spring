namespace TankGame.Persistence 
{
	public interface IPersistence
	{
		// File extension of the save file.
		string Extension { get; }

		// Path to the save file on disk.
		string FilePath { get; }

		// Generic method. Serializes the game data and stores it to the disk.
		void Save<T> ( T data );
		
		// Generic method. Loads game data from disk and deserializes it. 
		T Load<T> ();
	}
}

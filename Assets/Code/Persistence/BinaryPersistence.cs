using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace TankGame.Persistence 
{
    public class BinaryPersistence : IPersistence
    {
        public string Extension
        {
            get
            {
                return ".bin";
            }
        }

        public string FilePath { get; private set; }

		// Initializes the BinaryPersistence object.
		public BinaryPersistence( string path ) 
		{
			FilePath = path + Extension;
		}
        public void Save<T>(T data)
        {
			using ( FileStream stream = File.OpenWrite( FilePath ) ) 
			{
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize( stream, data );

				// Called automatically inside using statement, so this is moot
				// stream.Close();
			}
        }

        public T Load<T>()
        {
			T data = default(T);
			FileStream stream = File.OpenRead( FilePath );

			if( File.Exists( FilePath )) 
			{
				try
				{
					BinaryFormatter bf = new BinaryFormatter();
					data = (T) bf.Deserialize( stream );
				}
				catch ( Exception e)
				{
					Debug.LogException( e );
				}
				finally 
				{
					stream.Close();
				}
			}
			
			return data;
        }
    }
}

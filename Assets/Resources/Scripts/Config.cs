using UnityEngine;
using System.Collections;

using System.IO;
using System.Xml.Serialization;


public class Config {
	#region dont change
	static string path = "Assets/Resources/Config/xml.xml";
	
	public static void SetConfigs(){
		var serializer = new XmlSerializer(typeof(Config));
		var stream = new FileStream(path, FileMode.Create);
		serializer.Serialize(stream, new Config());
		stream.Close();
	}
	
	public static Config GetConfigs(){
		var serializer = new XmlSerializer(typeof(Config));
		var stream = new FileStream(path, FileMode.Open);
		var container = serializer.Deserialize(stream) as Config;
		stream.Close();
		return container;
	}

	#endregion

	public PrefabPaths prefabPaths = new PrefabPaths();
	public class PrefabPaths{
		public string cell = "Prefabs/Cell";
		public string uiHealthbar = "Prefabs/UI/HealthBar";
	}
}

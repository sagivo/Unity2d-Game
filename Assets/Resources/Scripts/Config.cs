using UnityEngine;
using System.Collections;

using System.IO;
using System.Xml.Serialization;


public class Config {
	#region dont change
	static string path = "Config/xml";
	
	public static void SetConfigs(){
		var serializer = new XmlSerializer(typeof(Config));
		TextAsset t = Resources.Load(path) as TextAsset;
		Stream s = new MemoryStream(t.bytes);
		serializer.Serialize(s, new Config());
	}
	
	public static Config GetConfigs(){
		var serializer = new XmlSerializer(typeof(Config));
		TextAsset t = Resources.Load(path) as TextAsset;
		Stream s = new MemoryStream(t.bytes);
		var container = serializer.Deserialize(s) as Config;
		s.Close();
		return container;
	}

	#endregion

	public PrefabPaths prefabPaths = new PrefabPaths();
	public class PrefabPaths{
		public string cell = "Prefabs/Cell";
		public string uiHealthbar = "Prefabs/UI/HealthBar";
	}
}

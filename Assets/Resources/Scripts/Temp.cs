using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

using System.IO;
using System.Xml.Serialization;

public class Temp {

	public MyClass cllll = new MyClass();

	public class MyClass
	{
		public MyObject MyObjectProperty = new MyObject();
	}
	public class MyObject
	{
		public string ObjectName = "asdasd";
		public int[] ints = new int[]{1,2,3};
	}

}


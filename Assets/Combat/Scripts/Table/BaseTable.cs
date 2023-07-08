using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BaseTable : ScriptableObject
{
	// [SerializeField] private List<T> dataList = new List<T>();
	// public List<T> DataList => dataList;

	public virtual void Init()
	{
		
	}
}

public class BaseTable<T> : BaseTable where T : struct
{
	[SerializeField] private List<T> dataList;
	
	private Dictionary<string, T> _dataDictionary = new Dictionary<string, T>();

	public override void Init()
	{
		foreach (var datum in dataList)
		{
			var type = datum.GetType();
			var field = type.GetProperty("Key");
			if (field == null) 
			{
				continue;
			}
			var keyValue = (string)field.GetValue(datum);
			_dataDictionary.Add(keyValue, datum);
		}
	}
	
	public T GetData(string key)
	{
		if (_dataDictionary.TryGetValue(key, out var value))
		{
			return value;
		}
		return default;
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TableContainer", menuName = "Scriptable Object/TableContainer", order = 0)]
public class TableContainer : ScriptableObject
{
	[SerializeField] private List<BaseTable> tables;

	private Dictionary<Type, BaseTable> _tableDictionary = new Dictionary<Type, BaseTable>();

	public void Init()
	{
		_tableDictionary.Clear();
		foreach (var table in tables) {
			table.Init();
			_tableDictionary.Add(table.GetType(), table);
		}
	}

	public T GetTable<T>() where T:BaseTable
	{
		var key = typeof(T);
		if (_tableDictionary.TryGetValue(key, out BaseTable value))
		{
			return value as T;
		}
		return null;
	}
}

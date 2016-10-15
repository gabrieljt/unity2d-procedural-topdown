﻿using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(Colliders))]
public class CollidersInspector : ALevelComponentInspector
{
}

#endif

[ExecuteInEditMode]
[RequireComponent(
	typeof(Map)
)]
public class Colliders : ALevelComponent
{
	[SerializeField]
	private Map map;

	public Map Map { get { return map; } }

	private void Awake()
	{
		map = GetComponent<Map>();
	}

	public override void Build()
	{
		BuildColliders();

		Built(GetType());
	}

	private void BuildColliders()
	{
		for (int x = 0; x < map.width; x++)
		{
			for (int y = 0; y < map.height; y++)
			{
				if (map.tiles[x, y].Type == TileType.Wall)
				{
					var collider = gameObject.AddComponent<BoxCollider2D>();
					collider.offset = new Vector2(x, y) + Vector2.one * 0.5f - map.Center;
					collider.size = Vector2.one;
				}
			}
		}
	}

	public void DestroyColliders()
	{
		var colliders = GetComponents<BoxCollider2D>();

		for (int i = 0; i < colliders.Length; i++)
		{
#if UNITY_EDITOR
			if (Application.isPlaying)
			{
				Destroy(colliders[i]);
			}
			else
			{
				DestroyImmediate(colliders[i]);
			}
#else
				Destroy(colliders[i]);
#endif
		}
	}

	public override void Dispose()
	{
		DestroyColliders();
	}
}
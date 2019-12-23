using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class Rail : MonoBehaviour
{

	// 閉じる
	public bool isClose = false;

	// レール点のリスト
	public List<GameObject> railPoints = new List<GameObject>();

	//----
	// アクセサ
	//----

	// レール点の数を取得する
	public int getRailPointNum()
	{
		return railPoints.Count;
	}

	// レール点を取得する
	public GameObject getRailPoint(int i)
	{
		Debug.Assert(railPoints.Count > 0);
		return railPoints[i];
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	void OnDrawGizmos()
	{
		for (int i = 0, n = railPoints.Count; i < n; ++i)
		{
			if (i == n - 1 && !isClose)
			{
				break;
			}

			GameObject from = railPoints[i];
			GameObject to = railPoints[(i + 1) % n];
			Gizmos.color = Color.green;
			Gizmos.DrawLine(from.transform.position, to.transform.position);
		}
	}

	// レールのカスタムエディタ
	[CustomEditor(typeof(Rail))]
	public class RailEditor : Editor
	{
		ReorderableList railPointsReorderableList;

		private Rail rail
		{
			get
			{
				return target as Rail;
			}
		}

		void OnEnable()
		{
			railPointsReorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("railPoints"));

			// コールバック設定
			railPointsReorderableList.onAddCallback += AddRailPoint;
			railPointsReorderableList.onRemoveCallback += RemoveRailPoint;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			serializedObject.Update();

			railPointsReorderableList.DoLayoutList();

			serializedObject.ApplyModifiedProperties();
		}

		// レール点を追加する
		private void AddRailPoint(ReorderableList list)
		{
			// レール点の生成位置
			Vector3 position;
			{
				// 最初のレール点 ⇒ レールの位置
				if (rail.railPoints.Count == 0)
				{
					position = rail.transform.position;
				}
				// 既にレール点がある ⇒ 最後のレール点の位置
				else
				{
					GameObject railPoint_Last = rail.railPoints[rail.railPoints.Count - 1];
					position = railPoint_Last.transform.position;
				}
			}

			// レール点生成
			GameObject prefab = (GameObject)Resources.Load("Prefabs/RailPoint");
			GameObject railPoint_New = Instantiate(prefab, position, Quaternion.identity);

			// リストに追加
			rail.railPoints.Add(railPoint_New);

			// レールの子にする
			railPoint_New.transform.parent = rail.transform;
		}

		// レール点を削除する
		private void RemoveRailPoint(ReorderableList list)
		{
			GameObject railPoint = rail.railPoints[list.index];
			rail.railPoints.Remove(railPoint);
			DestroyImmediate(railPoint);
		}
	}
}
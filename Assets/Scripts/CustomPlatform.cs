using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (PolygonCollider2D))]
[ExecuteInEditMode]
public class CustomPlatform : MonoBehaviour {
	private MeshRenderer mshRenderer;
	private MeshFilter mshFilter;
	private PolygonCollider2D polyCollider;
	private Triangulator triangulator;
	// Use this for initialization
	void Start () {
		mshRenderer = GetComponent<MeshRenderer>();
		mshFilter = GetComponent<MeshFilter>();
		polyCollider = GetComponent<PolygonCollider2D>();

		triangulator = new Triangulator ();
		if(polyCollider.points.Length == 0) CreateDefaultPlatform ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!Application.isPlaying) {
			triangulator = new Triangulator ();
			Vector2[] vertex = polyCollider.points;
			triangulator.UpdatePoints (vertex);
			int[] index = triangulator.Triangulate ();

			Vector3[] vertex3D = new Vector3[vertex.Length];
			for (int i = 0; i < vertex.Length; i++) {
				vertex3D [i] = vertex [i];
			}
			Vector2[] uvs = new Vector2[vertex.Length];
		
			for (int i=0; i < uvs.Length; i++) {
				uvs [i] = new Vector2 (vertex [i].x, vertex [i].y);
			}


			Mesh msh = new Mesh ();
			msh.vertices = vertex3D;
			msh.triangles = index;
			msh.uv = uvs;
			msh.RecalculateBounds ();
			msh.RecalculateNormals ();

			mshFilter.mesh = msh;
		}
	}

	void CreateDefaultPlatform(){
		List<Vector2> vertex = new List<Vector2> ();
		vertex.Add (new Vector2 (-1,1));
		vertex.Add (new Vector2 (1,1));
		vertex.Add (new Vector2 (1,-1));
		vertex.Add (new Vector2 (-1,-1));

		polyCollider.points = vertex.ToArray ();
	}
}

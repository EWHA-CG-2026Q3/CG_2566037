  using UnityEngine;

  [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
  public class MeshNew : MonoBehaviour
  {
      void Start()
      {
        // TODO 1: 원하는 다각형의 정점 좌표를 채우세요 (최소 4개)
        Vector3[] vertices = new Vector3[]
        {
              // 예: new Vector3(0f, 1f, 0f),
              new Vector3(0f, 0f, 0f), //0
              new Vector3(1f, 0f, 0f), //1
              new Vector3(0f, 1f, 0f), //2
              new Vector3(1f, 1f, 0f), //3
              new Vector3(0f, -1f, 0f), //4
              new Vector3(2f, 1f, 0f), //5
              new Vector3(1f, -1f, 0f), //6
              new Vector3(-1f, 0f, 0f), //7
              new Vector3(-1f,-1f, 0f), //8
              new Vector3(-2f, 0f, 0f), //9
              new Vector3(0f, -2f, 0f), //10
              new Vector3(-2f, 1f, 0f), //11
              new Vector3(-1f, 2f, 0f), //12
              new Vector3(1f, 2f, 0f), //13
              new Vector3(2f, 0f, 0f), //14
              new Vector3(-1f, 1f, 0f), //15
        };

          // TODO 2: 정점 3개씩 묶어 삼각형들을 구성하세요
          int[] triangles = new int[]
          {
              11, 12, 15,
              12, 2, 15,
              2, 13, 3,
              13, 5, 3,
              11, 15, 9,
              9, 15, 7,
              7, 15, 2,
              7, 2, 0,
              0, 2, 3,
              0, 3, 1,
              1, 3, 5,
              1, 5, 14,
              9, 7, 8,
              8, 7, 0,
              8, 0, 4,
              4, 0, 1,
              4, 1, 6,
              6, 1, 14,
              8, 4, 10,
              10, 4, 6
          };

          Mesh mesh = new Mesh();
          mesh.vertices = vertices;
          mesh.triangles = triangles;
          mesh.RecalculateNormals();

          GetComponent<MeshFilter>().mesh = mesh;
          GetComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
      }
  }
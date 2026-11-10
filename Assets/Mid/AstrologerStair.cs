using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AstrologerStair : MonoBehaviour
{
    void Start()
    {
        Mesh myMesh = new Mesh();
        myMesh.name = "TexturedMagicStaircase";
        GetComponent<MeshFilter>().mesh = myMesh;

        int stepCount = 90; // <-- [수정] 두 바퀴를 돌기 위해 계단 총 개수를 60개로 늘립니다! (한 바퀴당 30개)

        Vector3[] vertices = new Vector3[stepCount * 6];
        Vector2[] uv = new Vector2[stepCount * 6];
        int[] triangles = new int[stepCount * 24];

        for (int i = 0; i < stepCount; i++)
        {
            // [수정] 360도가 아니라 720도(두 바퀴) 기준로 각도를 나누어 계산합니다.
            float angle1 = (720f / stepCount) * i * Mathf.Deg2Rad;
            float angle2 = (720f / stepCount) * (i + 1) * Mathf.Deg2Rad;

            float topHeight = i * 0.2f;            // <-- [조정] 계단이 너무 가파르지 않게 높이 증가 폭을 0.2로 조절했습니다.
            float bottomHeight = topHeight - 0.2f;
            float radius = 5f;

            int v = i * 6;

            // [1] 윗면 점 3개 (0, 1, 2)
            vertices[v + 0] = new Vector3(0, topHeight, 0);
            vertices[v + 1] = new Vector3(Mathf.Cos(angle1) * radius, topHeight, Mathf.Sin(angle1) * radius);
            vertices[v + 2] = new Vector3(Mathf.Cos(angle2) * radius, topHeight, Mathf.Sin(angle2) * radius);

            // [2] 아랫면 점 3개 (3, 4, 5)
            vertices[v + 3] = new Vector3(0, bottomHeight, 0);
            vertices[v + 4] = new Vector3(Mathf.Cos(angle1) * radius, bottomHeight, Mathf.Sin(angle1) * radius);
            vertices[v + 5] = new Vector3(Mathf.Cos(angle2) * radius, bottomHeight, Mathf.Sin(angle2) * radius);

            // [3] UV 좌표(텍스처 매핑 좌표 0~1) 부여하기
            uv[v + 0] = new Vector2(0.5f, 0.5f);
            uv[v + 1] = new Vector2(0f, 0f);
            uv[v + 2] = new Vector2(1f, 0f);

            uv[v + 3] = new Vector2(0.5f, 0.5f);
            uv[v + 4] = new Vector2(0f, 1f);
            uv[v + 5] = new Vector2(1f, 1f);

            int t = i * 24;

            // 삼각형 8개 엮기
            triangles[t + 0] = v + 0; triangles[t + 1] = v + 2; triangles[t + 2] = v + 1;
            triangles[t + 3] = v + 3; triangles[t + 4] = v + 4; triangles[t + 5] = v + 5;

            triangles[t + 6] = v + 1; triangles[t + 7] = v + 2; triangles[t + 8] = v + 5;
            triangles[t + 9] = v + 1; triangles[t + 10] = v + 5; triangles[t + 11] = v + 4;

            triangles[t + 12] = v + 0; triangles[t + 13] = v + 1; triangles[t + 14] = v + 4;
            triangles[t + 15] = v + 0; triangles[t + 16] = v + 4; triangles[t + 17] = v + 3;

            triangles[t + 18] = v + 2; triangles[t + 19] = v + 0; triangles[t + 20] = v + 3;
            triangles[t + 21] = v + 2; triangles[t + 22] = v + 3; triangles[t + 23] = v + 5;
        }

        myMesh.vertices = vertices;
        myMesh.uv = uv;
        myMesh.triangles = triangles;

        myMesh.RecalculateNormals();

        // [여기가 추가된 핵심 코드입니다!]
        // 계단 메쉬와 완전히 똑같은 모양의 물리 충돌체(MeshCollider)를 생성하고 부착합니다.
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        meshCollider.sharedMesh = myMesh;
    }
}
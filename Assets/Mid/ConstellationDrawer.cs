using UnityEngine;
using System.Collections.Generic;

public class ConstellationDrawer : MonoBehaviour
{
    private Material lineMaterial;
    private List<GameObject> starObjects = new List<GameObject>();

    // 계단 주변 밤하늘을 가득 채우는 총 30개 별자리의 꼭짓점 좌표들
    private readonly Vector3[] starPositions = new Vector3[]
    {
        // --- [기존 10개 별자리] ---
        new Vector3(-8f, 16f, 8f), new Vector3(-6f, 17.5f, 9.5f),
        new Vector3(-4f, 17f, 11f), new Vector3(-2f, 19f, 12f),
        new Vector3(0f, 21f, 13f), new Vector3(2f, 20f, 11.5f), new Vector3(0f, 18f, 10f),

        new Vector3(7f, 18f, 8f), new Vector3(9f, 20.5f, 9.5f),
        new Vector3(11f, 19f, 11f), new Vector3(13f, 22f, 12f), new Vector3(15f, 20f, 13f),

        new Vector3(0f, 24f, -2f), new Vector3(0f, 20.5f, -2f),
        new Vector3(-2f, 22f, -2f), new Vector3(2f, 22f, -2f),

        new Vector3(-4f, 14f, -9f), new Vector3(-2f, 17f, -9f),
        new Vector3(4f, 14f, -9f), new Vector3(2f, 17f, -9f),
        new Vector3(-1f, 15.5f, -9f), new Vector3(0f, 15.5f, -9f), new Vector3(1f, 15.5f, -9f),

        new Vector3(-11f, 20f, -4f), new Vector3(-7f, 20f, -4f),
        new Vector3(-7f, 23.5f, -4f), new Vector3(-11f, 23.5f, -4f),

        new Vector3(9f, 15f, -7f), new Vector3(11f, 18f, -7f),
        new Vector3(13f, 17f, -7f), new Vector3(12f, 15f, -7f),

        new Vector3(4f, 17f, 7f), new Vector3(6f, 16f, 8.5f), new Vector3(8f, 17f, 10f),

        new Vector3(-7f, 15f, 4f), new Vector3(-5f, 16.5f, 5.5f),
        new Vector3(-6f, 18.5f, 5.5f), new Vector3(-8f, 17f, 4f),

        new Vector3(11f, 25f, -1f), new Vector3(14f, 24f, 0f), new Vector3(13f, 22f, 1f),

        new Vector3(-13f, 14f, 9f), new Vector3(-11f, 16f, 11f), new Vector3(-9f, 15f, 13f),


        // --- [새로 추가된 20개 별자리] ---
        new Vector3(2f, 26f, 5f), new Vector3(4f, 27f, 6f), new Vector3(5f, 29f, 7f),
        new Vector3(-14f, 18f, -8f), new Vector3(-12f, 16.5f, -7f), new Vector3(-10f, 18.5f, -8f),
        new Vector3(-6f, 23f, -10f), new Vector3(-4f, 26f, -10f),
        new Vector3(-3f, 23f, -11f), new Vector3(-1f, 26f, -11f),
        new Vector3(5f, 14f, -13f), new Vector3(7f, 16f, -12f), new Vector3(8f, 14f, -11f),
        new Vector3(13f, 13f, -5f), new Vector3(15f, 15.5f, -3f), new Vector3(17f, 14f, -1f),
        new Vector3(14f, 10f, 3f), new Vector3(16f, 11f, 5f), new Vector3(15f, 13f, 4f),
        new Vector3(10f, 9f, 12f), new Vector3(12f, 11f, 13f), new Vector3(13f, 9f, 15f), new Vector3(11f, 7f, 16f),
        new Vector3(5f, 8f, 15f), new Vector3(7f, 10f, 16f), new Vector3(8f, 8f, 18f),
        new Vector3(-3f, 9f, 16f), new Vector3(-1f, 11f, 17f), new Vector3(1f, 9.5f, 16f),
        new Vector3(-10f, 11f, 14f), new Vector3(-8f, 13f, 15f), new Vector3(-6f, 12f, 16f),
        new Vector3(-16f, 17f, 2f), new Vector3(-14f, 19f, 3f), new Vector3(-12f, 18f, 4f),
        new Vector3(-6f, 25f, -3f), new Vector3(-4f, 26.5f, -1f), new Vector3(-2f, 28f, 1f),
        new Vector3(3f, 27f, -4f), new Vector3(5f, 28.5f, -2f), new Vector3(7f, 27f, 0f),
        new Vector3(6f, 23f, -7f), new Vector3(8f, 25f, -6f), new Vector3(10f, 23.5f, -7f),
        new Vector3(15f, 27f, 4f), new Vector3(17f, 29f, 5f),
        new Vector3(13f, 24f, 8f), new Vector3(15f, 23f, 9f),
        new Vector3(-3f, 24f, -4f), new Vector3(3f, 24f, -4f),
        new Vector3(3f, 20f, 4f), new Vector3(5f, 21f, 5f),
        new Vector3(6f, 19f, 4f), new Vector3(8f, 19.5f, 5f),
        new Vector3(9f, 16f, 6f), new Vector3(11f, 17f, 7f), new Vector3(10f, 18f, 8f)
    };

    void Start()
    {
        foreach (Vector3 pos in starPositions)
        {
            GameObject star = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            star.name = "GlowingStar";
            star.transform.parent = this.transform;
            star.transform.localPosition = pos;
            star.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

            Renderer rend = star.GetComponent<Renderer>();
            Material starMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));

            Color starColor = new Color(0.7f, 0.9f, 1.0f, 1.0f);
            starMat.color = starColor;

            if (starMat.HasProperty("_BaseColor"))
                starMat.SetColor("_BaseColor", starColor);

            rend.material = starMat;
            Destroy(star.GetComponent<Collider>());

            // --- [핵심 추가] 각 별마다 작고 은은한 Point Light 자동 생성 ---
            Light starLight = star.AddComponent<Light>();
            starLight.type = LightType.Point;
            starLight.color = starColor;       // 별 색상과 똑같은 빛
            starLight.range = 6.0f;            // 빛이 닿는 범위 (적당히 아담하게)
            starLight.intensity = 4.0f;        // 빛의 밝기
            starLight.shadows = LightShadows.None; // 렉 방지를 위해 그림자는 안 생기게 설정!

            starObjects.Add(star);
        }
    }

    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    void OnRenderObject()
    {
        CreateLineMaterial();
        if (!lineMaterial) return;

        lineMaterial.SetPass(0);

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        GL.Begin(GL.LINES);
        GL.Color(new Color(0.7f, 0.9f, 1.0f, 0.8f));

        // --- 1~10번 기존 별자리 선 잇기 ---
        DrawLine(0, 1); DrawLine(1, 2); DrawLine(2, 3); DrawLine(3, 4); DrawLine(4, 5); DrawLine(5, 6);
        DrawLine(7, 8); DrawLine(8, 9); DrawLine(9, 10); DrawLine(10, 11);
        DrawLine(12, 13); DrawLine(14, 15);
        DrawLine(16, 17); DrawLine(18, 19); DrawLine(20, 21); DrawLine(21, 22);
        DrawLine(23, 24); DrawLine(24, 25); DrawLine(25, 26); DrawLine(26, 23);
        DrawLine(27, 28); DrawLine(28, 29); DrawLine(29, 30);
        DrawLine(31, 32); DrawLine(32, 33);
        DrawLine(34, 35); DrawLine(35, 36); DrawLine(36, 37); DrawLine(37, 34);
        DrawLine(38, 39); DrawLine(39, 40);
        DrawLine(41, 42); DrawLine(42, 43);

        // --- 11~30번 새로 추가된 별자리 선 잇기 ---
        DrawLine(44, 45); DrawLine(45, 46);
        DrawLine(47, 48); DrawLine(48, 49);
        DrawLine(50, 51); DrawLine(52, 53);
        DrawLine(54, 55); DrawLine(55, 56);
        DrawLine(57, 58); DrawLine(58, 59);
        DrawLine(60, 61); DrawLine(61, 62);
        DrawLine(63, 64); DrawLine(64, 65); DrawLine(65, 66);
        DrawLine(67, 68); DrawLine(68, 69);
        DrawLine(70, 71); DrawLine(71, 72);
        DrawLine(73, 74); DrawLine(74, 75);
        DrawLine(76, 77); DrawLine(77, 78);
        DrawLine(79, 80); DrawLine(80, 81);
        RankDrawLine(82, 83); RankDrawLine(83, 84); // (오타 방지용 보조 함수 명칭 유지)
        DrawLine(85, 86); DrawLine(86, 87);
        DrawLine(88, 89);
        DrawLine(90, 91);
        DrawLine(92, 93);
        DrawLine(94, 95);
        DrawLine(96, 97);
        DrawLine(98, 99); DrawLine(99, 100);

        GL.End();
        GL.PopMatrix();
    }

    void DrawLine(int indexA, int indexB)
    {
        GL.Vertex(starPositions[indexA]);
        GL.Vertex(starPositions[indexB]);
    }

    void RankDrawLine(int indexA, int indexB)
    {
        GL.Vertex(starPositions[indexA]);
        GL.Vertex(starPositions[indexB]);
    }
}
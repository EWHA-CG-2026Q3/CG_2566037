using UnityEngine;

public class StarCandleSpawner : MonoBehaviour
{
    public float circleRadius = 9.0f;     // 원의 크기
    public int totalCandles = 36;         // 촛불 개수
    public Material flameMaterial;        // 커스텀 불 Material (선택사항)

    void Start()
    {
        SpawnCircularCandles();
    }

    void SpawnCircularCandles()
    {
        for (int i = 0; i < totalCandles; i++)
        {
            float angle = (i / (float)totalCandles) * Mathf.PI * 2f;

            float x = Mathf.Cos(angle) * circleRadius;
            float z = Mathf.Sin(angle) * circleRadius;

            Vector3 spawnPos = new Vector3(x, 0f, z);
            CreateCandle(spawnPos);
        }
    }

    void CreateCandle(Vector3 pos)
    {
        // 1. 촛대 (몸체) 생성
        GameObject candle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        candle.name = "MagicCircleCandle";
        candle.transform.parent = this.transform;

        float candleHeight = 0.4f;
        float candleRadius = 0.2f;
        candle.transform.localScale = new Vector3(candleRadius, candleHeight / 2f, candleRadius);

        pos.y = candleHeight / 2f;
        candle.transform.position = pos;

        // 촛대 왁스 재질
        Renderer candleRend = candle.GetComponent<Renderer>();
        Material candleMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        candleMat.color = new Color(0.85f, 0.75f, 0.55f);
        candleRend.material = candleMat;

        Destroy(candle.GetComponent<Collider>());

        // 2. 불꽃 크기를 훨씬 아담하게 줄이고, 촛대 꼭대기에 자연스럽게 걸쳐지도록 조정
        GameObject flame = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        flame.name = "FlameEffect";
        flame.transform.parent = candle.transform;

        // 촛대 꼭대기 부근에 아담하게 위치시킴
        flame.transform.localPosition = new Vector3(0f, 0.85f, 0f);
        // 크기를 작고 얄렵한 물방울/타원체 느낌으로 대폭 축소
        flame.transform.localScale = new Vector3(0.6f, 0.9f, 0.6f);

        // 3. 불 재질(Material) 적용
        Renderer flameRend = flame.GetComponent<Renderer>();
        if (flameMaterial != null)
        {
            flameRend.material = flameMaterial;
        }
        else
        {
            Material defaultFlameMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            Color flameColor = new Color(1.0f, 0.65f, 0.15f);
            defaultFlameMat.color = flameColor;
            if (defaultFlameMat.HasProperty("_BaseColor"))
                defaultFlameMat.SetColor("_BaseColor", flameColor);

            flameRend.material = defaultFlameMat;
        }

        Destroy(flame.GetComponent<Collider>());

        // 4. 빛(Point Light)의 위치를 불꽃 오브젝트의 중앙에 정확히 일치시킴
        GameObject lightObj = new GameObject("FlameLight");
        lightObj.transform.parent = flame.transform;
        lightObj.transform.localPosition = Vector3.zero; // 불꽃 중심에 정확히 위치

        Light flameLight = lightObj.AddComponent<Light>();
        flameLight.type = LightType.Point;
        flameLight.color = new Color(1.0f, 0.55f, 0.15f);
        flameLight.range = 1.8f;
        flameLight.intensity = 1.0f;
        flameLight.shadows = LightShadows.None;
    }
}
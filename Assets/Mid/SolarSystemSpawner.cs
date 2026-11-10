using UnityEngine;

public class SpiralSolarSystem : MonoBehaviour
{
    [System.Serializable]
    public class PlanetData
    {
        public string name;
        public float orbitRadius;
        public float orbitSpeed;
        public float size;

        [Header("행성 디자인 설정")]
        public Material planetMaterial;
        public Color lightColor;

        [Header("오르내리기 설정")]
        public float verticalSpeed;
        public float minHeight;
        public float maxHeight;

        [Header("위성(달) 설정")]
        public bool hasMoon = true;
        public float moonOrbitRadius = 2.0f;
        public float moonOrbitSpeed = 120f;

        // [추가] 위성의 궤도를 기울어지게 만드는 각도 (X, Y, Z축 회전)
        public Vector3 moonTiltAngle;

        public float moonSize = 0.3f;
        public Material moonMaterial;
        public Color moonColor = Color.white;

        [HideInInspector] public GameObject obj;
        [HideInInspector] public GameObject orbitLine;
        [HideInInspector] public float currentAngle;
        [HideInInspector] public float verticalTime;

        [HideInInspector] public GameObject moonObj;
        [HideInInspector] public GameObject moonOrbitLine;
        [HideInInspector] public float moonCurrentAngle;
    }

    // 각 위성마다 궤도가 다르게 틀어지도록 moonTiltAngle 값을 개별적으로 주었습니다.
    public PlanetData[] planets = new PlanetData[]
    {
        new PlanetData { name = "Planet_Red", orbitRadius = 7f, orbitSpeed = 50f, size = 1.0f, lightColor = new Color(1f, 0.3f, 0.2f), verticalSpeed = 1.2f, minHeight = 2f, maxHeight = 16f, moonOrbitRadius = 1.8f, moonOrbitSpeed = 150f, moonColor = new Color(1f, 0.8f, 0.8f),
                         moonTiltAngle = new Vector3(15f, 0f, 30f) }, // 약간 비스듬하게 틀어짐
        new PlanetData { name = "Planet_Blue", orbitRadius = 10f, orbitSpeed = -40f, size = 1.2f, lightColor = new Color(0.2f, 0.6f, 1f), verticalSpeed = 0.8f, minHeight = 4f, maxHeight = 14f, moonOrbitRadius = 2.2f, moonOrbitSpeed = -120f, moonColor = new Color(0.8f, 0.9f, 1f),
                         moonTiltAngle = new Vector3(-45f, 0f, 10f) }, // 반대쪽으로 크게 틀어짐
        new PlanetData { name = "Planet_Yellow", orbitRadius = 13f, orbitSpeed = 30f, size = 1.5f, lightColor = new Color(1f, 0.8f, 0.3f), verticalSpeed = 1.0f, minHeight = 1f, maxHeight = 18f, moonOrbitRadius = 2.5f, moonOrbitSpeed = 100f, moonColor = new Color(1f, 1f, 0.8f),
                         moonTiltAngle = new Vector3(60f, 0f, -20f) }  // 거의 세워진 형태로 틀어짐
    };

    void Start()
    {
        foreach (var p in planets)
        {
            // 1. 부모 행성 생성
            p.obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            p.obj.name = p.name;
            p.obj.transform.localScale = Vector3.one * p.size;
            Destroy(p.obj.GetComponent<Collider>());

            Renderer rend = p.obj.GetComponent<Renderer>();
            if (p.planetMaterial != null) rend.material = p.planetMaterial;
            else
            {
                Material defaultMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                defaultMat.color = p.lightColor;
                rend.material = defaultMat;
            }

            Light ptLight = p.obj.AddComponent<Light>();
            ptLight.type = LightType.Point;
            ptLight.color = p.lightColor;
            ptLight.range = p.size * 8f;
            ptLight.intensity = 2.5f;
            ptLight.shadows = LightShadows.None;

            p.orbitLine = new GameObject(p.name + "_OrbitLine");
            LineRenderer lr = p.orbitLine.AddComponent<LineRenderer>();
            lr.useWorldSpace = true; lr.loop = true;
            lr.startWidth = 0.08f; lr.endWidth = 0.08f;
            Material lineMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            lineMat.color = new Color(p.lightColor.r, p.lightColor.g, p.lightColor.b, 0.5f);
            lr.material = lineMat;
            lr.positionCount = 60;

            // 2. 위성(달) 생성
            if (p.hasMoon)
            {
                p.moonObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                p.moonObj.name = p.name + "_Moon";
                p.moonObj.transform.localScale = Vector3.one * p.moonSize;
                Destroy(p.moonObj.GetComponent<Collider>());

                Renderer moonRend = p.moonObj.GetComponent<Renderer>();
                if (p.moonMaterial != null) moonRend.material = p.moonMaterial;
                else
                {
                    Material defaultMoonMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                    defaultMoonMat.color = p.moonColor;
                    moonRend.material = defaultMoonMat;
                }

                Light moonLight = p.moonObj.AddComponent<Light>();
                moonLight.type = LightType.Point;
                moonLight.color = p.moonColor;
                moonLight.range = p.moonSize * 6f;
                moonLight.intensity = 1.5f;
                moonLight.shadows = LightShadows.None;

                p.moonOrbitLine = new GameObject(p.name + "_MoonOrbitLine");
                LineRenderer moonLr = p.moonOrbitLine.AddComponent<LineRenderer>();
                moonLr.useWorldSpace = true; moonLr.loop = true;
                moonLr.startWidth = 0.03f; moonLr.endWidth = 0.03f;
                Material moonLineMat = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
                moonLineMat.color = new Color(p.moonColor.r, p.moonColor.g, p.moonColor.b, 0.4f);
                moonLr.material = moonLineMat;
                moonLr.positionCount = 60;
            }
        }
    }

    void Update()
    {
        foreach (var p in planets)
        {
            p.currentAngle += p.orbitSpeed * Time.deltaTime;
            p.verticalTime += p.verticalSpeed * Time.deltaTime;

            float currentY = Mathf.Lerp(p.minHeight, p.maxHeight, (Mathf.Sin(p.verticalTime) + 1f) / 2f);

            // [부모 행성의 행렬 계산]
            Matrix4x4 m_elevate = Matrix4x4.Translate(new Vector3(0, currentY, 0));
            Matrix4x4 m_spin = Matrix4x4.Rotate(Quaternion.Euler(0, p.currentAngle, 0));
            Matrix4x4 m_offset = Matrix4x4.Translate(new Vector3(p.orbitRadius, 0, 0));

            Matrix4x4 m_planetFinal = m_elevate * m_spin * m_offset;
            p.obj.transform.position = m_planetFinal.MultiplyPoint(Vector3.zero);

            LineRenderer lr = p.orbitLine.GetComponent<LineRenderer>();
            for (int i = 0; i < 60; i++)
            {
                float angle = (i / 60f) * 360f;
                Matrix4x4 m_circleSpin = Matrix4x4.Rotate(Quaternion.Euler(0, angle, 0));
                Matrix4x4 m_lineFinal = m_elevate * m_circleSpin * m_offset;
                lr.SetPosition(i, m_lineFinal.MultiplyPoint(Vector3.zero));
            }

            // [자식 위성의 행렬 계산]
            if (p.hasMoon)
            {
                p.moonCurrentAngle += p.moonOrbitSpeed * Time.deltaTime;

                // [핵심 추가] 위성의 공전 궤도를 통째로 기울여주는 회전 행렬
                Matrix4x4 m_moonTilt = Matrix4x4.Rotate(Quaternion.Euler(p.moonTiltAngle));

                Matrix4x4 m_moonSpin = Matrix4x4.Rotate(Quaternion.Euler(0, p.moonCurrentAngle, 0));
                Matrix4x4 m_moonOffset = Matrix4x4.Translate(new Vector3(p.moonOrbitRadius, 0, 0));

                // 적용 순서: 위성 띄우기(Offset) -> 위성 돌리기(Spin) -> 궤도 기울이기(Tilt) -> 부모 위치(PlanetFinal)
                Matrix4x4 m_moonFinal = m_planetFinal * m_moonTilt * m_moonSpin * m_moonOffset;
                p.moonObj.transform.position = m_moonFinal.MultiplyPoint(Vector3.zero);

                // 기울어진 위성 궤도선 업데이트
                LineRenderer moonLr = p.moonOrbitLine.GetComponent<LineRenderer>();
                for (int i = 0; i < 60; i++)
                {
                    float angle = (i / 60f) * 360f;
                    Matrix4x4 m_moonCircleSpin = Matrix4x4.Rotate(Quaternion.Euler(0, angle, 0));

                    // 궤도선 좌표에도 똑같이 Tilt 행렬을 적용해줍니다.
                    Matrix4x4 m_moonLineFinal = m_planetFinal * m_moonTilt * m_moonCircleSpin * m_moonOffset;
                    moonLr.SetPosition(i, m_moonLineFinal.MultiplyPoint(Vector3.zero));
                }
            }
        }
    }
}
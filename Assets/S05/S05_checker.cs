using UnityEngine;
using UnityEngine.UI;
public class S05_checker: MonoBehaviour
{
    [SerializeField] private int canvasWidth = 256;
    [SerializeField] private int canvasHeight = 256;
    [SerializeField] private Color backgroundColor = new Color(0f, 0f, 0f, 1f);
    // 검정
    [Header("무늬 실습 (줄무늬·체스판 공용)")]
    [SerializeField] private int patternSize = 16;
    [SerializeField] private Color colorA = new Color(1f, 1f, 1f, 1f); // 흰색
    [SerializeField] private Color colorB = new Color(0.3f, 0.5f, 0.8f, 1f); // 하늘색
    private Texture2D canvasTexture;
    private RawImage targetImage;
    void Start()
    {
        targetImage = GetComponent<RawImage>(); //rawimage component가 있어야 target 인지 가능

        // 1. 빈 텍스처(Texture2D) 생성 — 아직 아무 색도 채워지지 않은 상태
        canvasTexture = new Texture2D(canvasWidth, canvasHeight);

        // 2. 픽셀 경계를 흐리지 않게(확대해도 네모난 픽셀 그대로 보이도록)
        canvasTexture.filterMode = FilterMode.Point; //FilterMode.Point는 우리가 정하는 것이 아닌 unity가 제공

        // 3. 픽셀 채우기 (실습 단계에 따라 아래 호출을 교체)
        FillCheckerboard(patternSize, colorA, colorB);

        // 4. 지금까지의 SetPixel 변경 사항을 실제로 텍스처에 반영
        canvasTexture.Apply();

        // 5. 완성된 텍스처를 화면의 RawImage에 연결
        targetImage.texture = canvasTexture;
    }
    // 참고 예시 ① — 캔버스 전체를 한 가지 색으로 채움 (모든 픽셀이 같은 값)
    /*
    private void FillBackground(Color color)
    {
        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                canvasTexture.SetPixel(x, y, color); //특정 색

                Color randomColor = new Color(Random.value, Random.value, Random.value, 1f);
                canvasTexture.SetPixel(x, y, randomColor); //랜덤

            }
        }
    } 
    private void FillVerticalStripes(int width, Color colorA, Color colorB)
    {
        for (int x = 0; x < canvasWidth; x++)
        {
            // TODO: x를 width로 나눈 몫이 짝수면 colorA, 홀수면 colorB가 되도록
            // isColorA를 올바른 조건식으로 바꾸세요.
            // 힌트: (x / width) % 2 == 0
            bool isColorA = (x / width) % 2 == 0;
            Color stripeColor = isColorA ? colorA : colorB;
            for (int y = 0; y < canvasHeight; y++)
                canvasTexture.SetPixel(x, y, stripeColor);
        }
    } //줄무늬 만들기
    */
    private void FillCheckerboard(int size, Color colorA, Color colorB)
    {
        for (int x = 0; x < canvasWidth; x++)
        {
            for (int y = 0; y < canvasHeight; y++)
            {
                bool isColorA = ((x / size) + (y / size)) % 2 == 0;
                Color checkerboardColor = isColorA ? colorA : colorB;
                canvasTexture.SetPixel(x, y, checkerboardColor);
                // TODO: 줄무늬는 x만 봤지만, 체스판은 x와 y를 함께 고려해야 합니다.
                // 힌트: (x / size) + (y / size) 의 결과를 활용해보세요.
                // 여기에 SetPixel 호출까지 직접 작성하세요.
            }
        }
    }
}
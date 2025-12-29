using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform neckBone;

    private Camera mainCamera;

    private Vector3 targetPoint;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 마우스 화면 좌표에서 3D 공간으로 Ray(광선) 발사
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Y축을 법선으로 하는 가상의 바닥 평면 생성 (높이 0)
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        // Ray가 바닥 평면과 교차하면 그 지점을 타겟으로 저장
        if (groundPlane.Raycast(ray, out float distance))
        {
            // Ray의 시작점에서 distance만큼 떨어진 지점 = 바닥과의 교차점
            targetPoint = ray.GetPoint(distance);
        }
    }

    void LateUpdate()
    {
        if (neckBone == null) return;

        // 목에서 타겟까지의 방향 벡터 계산
        Vector3 direction = targetPoint - neckBone.position;

        // ========== Mathf.Atan2 설명 ==========
        // Atan2(y, x)는 2D 좌표 (x, y)가 원점에서 어느 방향에 있는지 각도를 반환하는 함수
        //
        // [수학적 원리]
        // - 일반 Atan(y/x)는 -90° ~ 90° 범위만 반환하고, 0으로 나누기 문제 있음
        // - Atan2(y, x)는 -180° ~ 180° 전체 범위를 반환하며, x=0일 때도 안전하게 처리
        //
        // [Unity에서의 사용]
        // - 3D 공간에서 Y축 회전(수평 회전)을 구할 때는 X와 Z 좌표 사용
        // - Atan2(x, z): Z축(앞방향) 기준으로 X축(오른쪽) 방향의 각도 반환
        // - 결과: Z+ 방향 = 0°, X+ 방향 = 90°, Z- 방향 = ±180°, X- 방향 = -90°
        //
        // [Mathf.Rad2Deg]
        // - Atan2는 라디안(radian) 단위로 반환 (π = 180°)
        // - Rad2Deg = 180/π ≈ 57.2958
        // - 라디안 * Rad2Deg = 도(degree)로 변환
        // =======================================
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // neckBone에서 타겟까지의 방향을 빨간색 선으로 표시 (Scene 뷰에서 확인)
        Debug.DrawLine(neckBone.position, targetPoint, Color.red);

        // 현재 로컬 회전값 가져오기
        Vector3 currentRotation = neckBone.localEulerAngles;

        // Y축만 새 각도로 변경, X와 Z는 기존 값 유지
        neckBone.localEulerAngles = new Vector3(currentRotation.x, angle, currentRotation.z + 90);
    }
}
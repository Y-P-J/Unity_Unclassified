using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

//게임루프 : 게임은 기본적으로 반복된다.
//Init -> Input -> Update -> Render -> Input -> ...

//Init      : 초기값을 세팅하는 단계
//Input     : 유저의 입력을 받는 단계
//Update    : 값을 처리하는 단계
//Render    : 값을 화면에 그리는 단계

public class Movement : MonoBehaviour
{
    //SerializeField : private이나 유니티의 컴포넌트에는 노출된다.
    [SerializeField] float moveSpeed;
    [SerializeField] float movePower;

    //유니티 이벤트 함수 : 사용자가 아닌 유니티가 부르는 함수

    //게임 시작 최초에 1회 호출되어 실행됨
    void Start()
    {
        //내가 붙어있는 오브젝트의 transform 컴포넌트에 접근한다
        //내부에있는 position 값을 1, 0으로 대입한다.
        transform.position = new Vector3(0, 0);
    }

    //프레임마다 호출되어 실햄됨
    void Update()
    {
        /* Input.GetKeyDown
         * 특정 키보드 입력을 판단하며 키보드를 입력한 순간 True를 반환한다.
         */
        /* Input.GetKeyUp
         * 특정 키보드 입력을 판단하며 키보드를 입력한 이후 입력을 해제했을 시 True를 반환한다.
         */
        /* Input.GetKey
         * 특정 키보드 입력을 판단하며 키보드를 입력하고 있는 동안 True를 반환한다.
         */

        if (Input.GetKeyDown(KeyCode.F))
            transform.position += Vector3.right * -movePower;
        else if (Input.GetKeyDown(KeyCode.H))
            transform.position += Vector3.right * movePower;
        else if (Input.GetKeyDown(KeyCode.T))
            transform.position += Vector3.up * movePower;
        else if (Input.GetKeyDown(KeyCode.G))
            transform.position += Vector3.up * -movePower;

        /*
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position += Vector3.right * movespeed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.LeftArrow))
            transform.position += Vector3.left * movespeed * Time.deltaTime;
        */

        /* Time.deltaTime
         * 이전 프레임에서 현재프레임 까지 걸린시간
         * 프레임 차이에 따른 이동량을 보정하기 위해 사용한다.
         */

        // 단위벡터 : 움직인 대각선의 값의 한 쪽 방향의 수만큼 나눴을때 1당 움직인 값

        /* 움직이는 방향의 값은 고정이어야 되기에
         * 단위벡터 * 크기(scalar) * 델타타임 = 이동량(movement)
         * 방식으로 사용한다
         */

        //Input.GetAxisRaw : Input Moudle에 작성된 값에 따라 -1, 0, 1의 값을 반환한다.
        //Horizontal의 경우 왼쪽,a는 -1 / 오른쪽,d는 1 / 미입력시 0을 반환한다
        Vector3 hor = Input.GetAxisRaw("Horizontal") * Vector3.right;
        Vector3 ver = Input.GetAxisRaw("Vertical") * Vector3.up;
        
        //normalized를 사용해 대각선 벡터의 값을 x와y값 만큼의 단위벡터로 만들어 원 모양의 이동값을 나타낸다.
        Vector3 dir = (ver + hor).normalized;

        transform.position += dir * moveSpeed * Time.deltaTime;
    }
}

//게임에서 사용하는 수학
/* 1. 공간을 다루는 수학 : 벡터 > 행렬
 * 공간을 다루는 수학은 가상 공간을 구축하고 분석하는데 사용한다.
 * 가상공간은 벡터로 이루어져 있고 행렬을 이용해 원하는대로 변환한다.
 */
/* 2. 물체를 다루는 수학 : 점 > 삼각형
 * 물체를 다루는 수학은 물체의 외형을 설정하고 화면에 표시(Render)한다.
 */
/* 3. 회전을 다루는 수학 : 삼각함수 > 사원수
 * 
 */

/* 수와 집합
 * 자연수 : 물건을 세거나 순서를 지정하기 위해 사용하는 수의 집합(N)
 * 정수 : 자연수와 음수, 0을 포함하는 수의 집합(Z)
 * 유리수 : 분모가 0이 아닌 두 정수의 비율, 또는 분수로 나타낼 수 있는 수의 집합(Q)
 * 무리수 : 두 정수의 비율, 또는 분수로 나타낼 수 없는 수의 집합(I)
 * 실수 : 유리수와 무리수를 포함하는 수의 집합(R)
 * 복소수 : 실수와 제곱하면 -1이 되는 허수 단위 i를 조합해 a + bi 형태로 표현하는 수의 집합(C)
 * 사원수 : 실수와 제곱하면 -1이 되는 세 허수 단위 i,j,k를 조합해 a + bi + cj + dk 형태로 표현하는 수의 집합(H)
 */

/* 연산과 수의 구조
 * 수집합의 고유한 특징은 원소를 이용해 연산을 한다.
 */

/* 이항연산
 * 대표적인 사칙연산을 예로, 두 원소를 이용해 새로운 원소를 만드는 것을 말한다.
 */
/* 이항연산의 특징 및 성질
 * 1. 닫혀있다 : 같은 집합에 속한 두 수를 투입한 이항연산의 결과가 항상 투입한 집합에 속한다.
 * 2. 교환법칙 : 임의의 두 수를 연산할 때 순서에 상관없이 결과가 동일하다.
 * 3. 결합법칙 : 연산이 두 번 이상 연속될 때 동일한 연산자일 시,
 *              무엇을 먼저 연산하건 결과가 동일하다.
 * 4. 분배법칙 : 서로 다른 2가지 연산에 대해 아래와 같은 규칙이 성립
 *              a(b + c) = ab + ac / (b + c)a = ab + ac
 *              연산자에 따라 같은 방식으로 좌분배, 우분배법칙의 값이 같아야 성립한다.
 * 5. 항등원 : 임의의 수와 연산 결과를 항상 동일한 수로 만들어주는 수
 *            a + 0 = a / a * 1 = a
 * 6. 역원 : 임의의 수와 연산 결과를 항등원으로 만들어주는 수
 * 6-1. 반대수 : a + (-a) = 0
 * 6-2. 역수 : a * (1/a) = 1 
 * 7. 두 번째 연산에 대해 닫혀있다.
 * 8. 두 번째 연산에 대해 결합법칙이 성립한다.
 * 9. 첫 번째 연산과 두 번째 연산에 대해 분배 법칙이 성립한다.
 * 10. 두 번째 연산에 대해 교환법칙이 성립힌다.
 * 
 * = 덧셈과 곱셈은 이항연산의 조건을 만족한다.
 * = 체의 구조 : 위의 모든 조건을 만족하는 수의 집합
 *              유리수, 실수가 해당되며, 예외 없이 덧셈과 곱셈을 자유롭게 사용한다.
 *              예를 들어, 체의 구조를 사용한 이항연산에 만족하기 위해
 *              a - b 대신 a + (-b), a / b 대신 a * (1/b)와 같은 방식으로 사용한다.
 */

/* 벡터 공간(Vector Space)
 * 두 개 이상의 실수를 곱집합으로 묶어 형상된 집합.
 * 해당 공간에 있는 원소를 벡터(Vector)라고 함
 */
/* 벡터의 합
 * v1(x1, y1) + v2(x1, y2) = (x1 + x2, y1 + y2)
 */
/* 스칼라 곱
 * a * v(x, y) = (ax, ay)
 */


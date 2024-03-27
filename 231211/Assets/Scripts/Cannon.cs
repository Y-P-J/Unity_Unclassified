using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cannon : MonoBehaviour
{
    //1. 위, 아래 방향키로 각도를 조절할 수 있다.
    //2. space를 누르면 해당 방향으로 날아간다.

    [SerializeField] Bullet bulletprefab;
    [SerializeField] float angle;
    [SerializeField] float speed;
    [SerializeField] Vector3 windSpeed;
    [SerializeField] Transform target;
    [SerializeField] float power = 0.0f;


    //실제 중력가속도는 9.81이 맞으나, 게임상에서는 조절해 사용한다.
    const float GRAVITY = -9.81f * 0.5f;

    const float MIN_POWER = 1.0f;
    const float MAX_POWER = 10.0f;
    const float POWER = 10.0f;
    const float CHARGE_TIME = 2.0f;

    float time;

    Vector3 velocity;

    void Start()
    {
        /* Quaternion.Euler(vector)
         * 오일러 각도를 사원수로 변환시켜 반환한다.
         * 
         * transform.rotation = Quaternion.Euler(vector)
         * 절대 값을 기준으로 회전한다.
         * 
         * transform.localRotation = Quaternion.Euler(vector) | transform.Rotate
         * 상대 값을 기준으로 회전한다.
         * 
         * Quaternion.LookRotation(direction)
         * 특정 방향으로 향하는 회전값을 반환한다.
         * 
         * transform.LookAt(target)
         * 해당 위치를 바라보는 회전 값을 반환한다.
         */
    }

    void Update()
    {
        float movement = speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            power = MIN_POWER;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            float amount = (MAX_POWER - MIN_POWER) / CHARGE_TIME * Time.deltaTime;
            power = Mathf.Clamp(power + amount, MIN_POWER, MAX_POWER);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            float angle = transform.rotation.eulerAngles.z;
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * power;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * power;

            Bullet newBullet = Instantiate(bulletprefab, transform.position, Quaternion.identity);
            newBullet.AddForce(new Vector3(x, y));
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, angle * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, -angle * Time.deltaTime));
        }

        //transform.rotation = Quaternion.Euler(30, 30, 30);        //필드값 30,30,30을 반환한다.
        //transform.Rotate(30, 30, 30);                             //상대값 30,30,30을 넘기기에 계속해서 회전하게 된다.
        //transform.rotation = Quaternion.LookRotation(dir);        //target의 포지션을 넘겨받은 dir을 이용해 해당 방향을 반환한다.
        //transform.LookAt(target);                                 //target의 포지션을 직접 넘겨받아 해당 방향을 바라본다.
    }
}

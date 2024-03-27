using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cannon : MonoBehaviour
{
    //1. ��, �Ʒ� ����Ű�� ������ ������ �� �ִ�.
    //2. space�� ������ �ش� �������� ���ư���.

    [SerializeField] Bullet bulletprefab;
    [SerializeField] float angle;
    [SerializeField] float speed;
    [SerializeField] Vector3 windSpeed;
    [SerializeField] Transform target;
    [SerializeField] float power = 0.0f;


    //���� �߷°��ӵ��� 9.81�� ������, ���ӻ󿡼��� ������ ����Ѵ�.
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
         * ���Ϸ� ������ ������� ��ȯ���� ��ȯ�Ѵ�.
         * 
         * transform.rotation = Quaternion.Euler(vector)
         * ���� ���� �������� ȸ���Ѵ�.
         * 
         * transform.localRotation = Quaternion.Euler(vector) | transform.Rotate
         * ��� ���� �������� ȸ���Ѵ�.
         * 
         * Quaternion.LookRotation(direction)
         * Ư�� �������� ���ϴ� ȸ������ ��ȯ�Ѵ�.
         * 
         * transform.LookAt(target)
         * �ش� ��ġ�� �ٶ󺸴� ȸ�� ���� ��ȯ�Ѵ�.
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

        //transform.rotation = Quaternion.Euler(30, 30, 30);        //�ʵ尪 30,30,30�� ��ȯ�Ѵ�.
        //transform.Rotate(30, 30, 30);                             //��밪 30,30,30�� �ѱ�⿡ ����ؼ� ȸ���ϰ� �ȴ�.
        //transform.rotation = Quaternion.LookRotation(dir);        //target�� �������� �Ѱܹ��� dir�� �̿��� �ش� ������ ��ȯ�Ѵ�.
        //transform.LookAt(target);                                 //target�� �������� ���� �Ѱܹ޾� �ش� ������ �ٶ󺻴�.
    }
}

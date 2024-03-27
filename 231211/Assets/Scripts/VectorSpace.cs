using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VectorSpace : MonoBehaviour
{
    [SerializeField] Transform target;
    //[SerializeField] float speed = 3.0f;

    void Start()
    {
        #region ������ǥ, �����ǥ
        /* ����(����)��ǥ | transform.position
         * ��ǥ�� ��ġ�� ��ȯ���� �ʴ� �������� ��ǥ��
         * 
         * ����(���)��ǥ | transform.localPosition
         * Ư�� ��ġ�� �������� ������ ��ǥ��
         * 
         */

        /* ���� ���� ����
         * Vector3.right
         * ������ǥ �������� ������ ���ϴ� ��������
         * 
         * transform.right
         * �����ǥ �������� ������ ���ϴ� ��������
         */

        //transform.position = new Vector3(3, 3);           //������ǥ �������� 3, 3���� �ű��.
        //transform.localPosition = new Vector3(3, 3);      //�����ǥ �������� 3, 3���� �ű��.

        //transform.position += Vector3.right * 3;          //������ǥ �������� 3��ŭ �̵��ض�
        //transform.position += transform.right * 3;        //�����ǥ �������� 3��ŭ �̵��ض�

        //transform.Translate(Vector3.right);               //�����ǥ ���� �̵�����ŭ ��������.
        //transform.Translate(Vector3.right, Space.World);  //������ǥ ���� �̵�����ŭ ��������.
        //transform.Translate(Vector3.right, target);       //�ش� ������Ʈ�� �����ǥ ���� �̵�����ŭ ��������.

        //Debug.Log(transform.root.name);                   //���������� �ֻ��� ��ü�� �̸��� ��ȯ�Ѵ�.
        //Debug.Log(transform.parent.name);                 //���������� �θ� ��ü�� �̸��� ��ȯ�Ѵ�. 
        #endregion

        #region ����/�����ǥ ȸ��
        /* ������ǥ ���� ȸ����
         * transform.rotation
         * 
         * �����ǥ ���� ȸ����
         * transform.localRotation
         */

        //transform.rotation = Quaternion.Euler(0, 0, 30);              //������ǥ �������� 30�� ȸ���Ѵ�.
        //transform.localRotation = Quaternion.Euler(0, 0, 30);         //�����ǥ �������� 30�� ȸ���Ѵ�.

        //transform.Rotate(Vector3.forward * 30f);                      //�����ǥ �������� 30�� ȸ���Ѵ�.

        /* �ءء� �������� start�� �Ű����� update���� ���� ��� ȸ���ԡءء� */
        //transform.RotateAround(target.position, Vector3.forward, 1f); //�ش� ������Ʈ�� ������ ȸ���Ѵ�. 
        #endregion
    }

    void Update()
    {
        #region Ÿ���� ���� �̵� �� ���� ����
        /* Vector3.Distance
         * A�� B������ �Ÿ����� ��ȯ�Ѵ�.
         * 
         * A�� B�� �Ÿ��� ����� �̵��Ÿ����� �����ٸ� �� �Ÿ���ŭ�� �̵��Ѵ�.
         * �׿ܿ� �⺻���� �Ÿ���ŭ�� �����δ�.
         */

        //float distance = Vector3.Distance(transform.position, target.position);
        //float movement = speed * Time.deltaTime;

        //Vector3 dir = (target.position - transform.position).normalized;

        //if (distance < movement)
        //    transform.position += dir * distance;
        //else
        //    transform.position += dir * movement;



        /* Vector3.MoveTowards
         * A�� B��ġ�� C��ŭ ������������ ��ġ���� ��ȯ�ϸ�, ��ӿ�Ͽ� ������ �ӵ��� �̵��Ѵ�(�� �ڵ�� �Ȱ���)
         * 
         * Vector3.Lerp
         * �Ÿ��� ���� �ӵ��� ��ȭ�� ���� �ε巴�� �����δ�
         */

        //transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime); 
        //transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
        #endregion
    }
}

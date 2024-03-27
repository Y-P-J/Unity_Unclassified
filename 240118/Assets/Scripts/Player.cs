using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Movement2D movement;       //�̵����� Ŭ����

    [SerializeField] HpGroupUI hpGroupUI;       //ü��UI Ŭ����
    [SerializeField] int maxHp;                 //�ִ� ü��
    public int curHp { get; private set; }      //���� ü��

    float hitThrow;                             //�ǰݽ� �������� ��
    Vector2 currentInput;                       //���� �Է°�
    float godModeTime = 2.0f;                   //�����ð�
    Dictionary<ITEM, int> itemCount;            //�� �����ۺ� ����
    bool isGameClear;                           //Ŭ��������

    Animator anim;                              //�ִϸ�����
    SpriteRenderer spriteRenderer;              //��������Ʈ ������

    public int MaxHp => maxHp;
    public Movement2D Movement => movement;
    public float HitThrow => hitThrow;

    void Start()
    {
        curHp = maxHp;

        hpGroupUI.Setup(this);

        hitThrow = 10.0f;
        currentInput = Vector2.zero;//0 �Ҵ�
        anim = GetComponent<Animator>();//�ڽ��� Inspector�� �ִ� Animator �Ҵ�
        spriteRenderer = GetComponent<SpriteRenderer>();//�ڽ��� Inspector�� �ִ� SpriteRenderer �Ҵ�
        itemCount = new Dictionary<ITEM, int>();//�ڽ��� ��ųʸ��� �Ҵ�

        if (CheckPoint.checkPosition != null)
            transform.position = (Vector3)CheckPoint.checkPosition;
    }

    void Update()
    {
        if (isGameClear)//Ŭ���������� Ȱ��ȭ�Ǹ�
            return;//update�� ��ȿȭ�Ѵ�.

        /// <summary>
        /// �÷��̾� �¿��̵� ����
        /// </summary>
        currentInput.x = Input.GetAxisRaw("Horizontal");//���ʹ���Ű -1, ���Է� 0, �����ʹ���Ű 1
        currentInput.y = Input.GetAxisRaw("Vertical");//�Ʒ��ʹ���Ű -1, ���Է� 0, ���ʹ���Ű 1

        movement.Movement(currentInput);

        if (currentInput.x != 0)//���Է� ���°� �ƴҶ�
            spriteRenderer.flipX = currentInput.x < 0;//�����̵� �Էµɶ� ����ȴ�.

        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("onJump");//onJump �ִϸ��̼�Ʈ���� Ȱ��ȭ
            movement.Jump();
        }//zŰ �Է½� Jump() ���
    }

    void LateUpdate()
    {
        anim.SetBool("isRun", currentInput.x != 0.0f);//�¿� �Է��� ������ üũ
        anim.SetBool("isGround", movement.IsGrounded);//�ٴڿ� ���������� üũ
        anim.SetBool("isCrouch", currentInput.y == -1.0f);//�Ʒ� �Է������� üũ
        anim.SetFloat("velocityY", Mathf.Round(movement.Velocity.y));//���� �̵��ӵ� �Է�

        currentInput = Vector2.zero;//���� �¿��Է°��� �ʱ�ȭ��
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Clear")//Clear�±׿� �浹�ҽ�
        {
            movement.Movement(Vector2.zero);//�̵����� ������Ű��
            isGameClear = true;//Ŭ���������� Ȱ��ȭ�Ѵ�
        }
    }

    /// <summary>
    /// �ǰ� ����
    /// </summary>
    public void Hit()
    {
        if(movement.Throw(hitThrow))
        {
            curHp--;
            hpGroupUI.UpdateUI();

            anim.SetTrigger("onHurt");//�ִϸ��̼� onHurt Ʈ���� Ȱ��ȭ
            StartCoroutine(IEGodMode());//����带 �ڷ�ƾ ��Ų��
        }
    }

    /// <summary>
    /// �÷��̾� �����
    /// </summary>
    void Dead()
    {

    }

    /// <summary>
    /// ������ ȹ��
    /// </summary>
    public void GetItem(ITEM id, int count)
    {
        if (!itemCount.ContainsKey(id))//������ Ű���� ���ٸ�
            itemCount.Add(id, 0);//�ش� Ű���� ����

        itemCount[id] += count;//ī��Ʈ����ŭ �ش� Ű���� ���� �߰�

        Debug.Log($"{id} increase {count} / current = {itemCount[id]}");
    }

    /// <summary>
    /// �����ð� �� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator IEGodMode()
    {
        float godModeTime = this.godModeTime;//�����ð� �޾ƿ���
        float offset = godModeTime / 10.0f;//�����ð������� 10�� �������� ����������
        float time = offset;//�帣�� �����½ð�
        int prevLayer = gameObject.layer;//���� ���̾� ����

        gameObject.layer = LayerMask.NameToLayer("GodMode");//���̾� GodMode�� ����

        spriteRenderer.ChangeAlpha(0.9f);

        while((godModeTime -= Time.deltaTime) >= 0.0f)//�����ð����� ����
        {
            if ((time -= Time.deltaTime) <= 0.0f)//�����½ð� �����
            {
                time = offset;//�ٽ� �����ϰ�

                spriteRenderer.enabled = !spriteRenderer.enabled;//������ ����Ű��
            }
            yield return null;//������ �ѱ��
        }

        spriteRenderer.enabled = true;//������ �ѱ�
        spriteRenderer.ChangeAlpha(1.0f);
        
        gameObject.layer = prevLayer;//���� ���̾�� �ٽ� ����
    }

    /// <summary>
    /// editor�� �׸���
    /// </summary>
    void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.red;//������� ������ ���������� ����
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, movement.GroundRadius);//ī�޶� ���� �ٶ󺸴� ������� ����� �׸���.
    }

    /* RigidBody�� ���� �������
     * 1. ���� : �⺻���Դ� 1�̴�.
     * 2. ���� : ��������, ȸ������
     * 3. ������ : �浹ü����
     */

    /* Physics2D.OverlapCircle(�߽���ǥ, ��������, [���̾��ũ])
         * �߽���ǥ�� �������� ����������ŭ ���� �׷� ���̾��ũ�� ������ �ݶ��̴��� ���� �浹�� �����Ѵ�.
         */

}

/// <summary>
/// �޼ҵ� �߰�
/// </summary>
public static class Method
{
    /// <summary>
    /// SpriteRenderer���� �޼ҵ��߰�, ���İ� ����
    /// </summary>
    public static void ChangeAlpha(this SpriteRenderer renderer, float alpha)
    {
        Color color = renderer.color;//�������� ����ü ����
        color.a = alpha;//���İ�����
        renderer.color = color;//����
    }
}
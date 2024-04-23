using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [Tooltip("ä�� �Է� �ʵ�")]
    [SerializeField] TMP_Text chatText;
    [SerializeField] TMP_InputField chatInputField;

    Queue<string> messageQueue = new Queue<string>();

    void Start()
    {
        chatText.text = string.Empty;//ä�� �ؽ�Ʈ �ʱ�ȭ
        chatInputField.text = string.Empty;//ä�� �Է� �ʵ� �ʱ�ȭ
        PhotonNetwork.IsMessageQueueRunning = true;//�޽��� ť ���� Ȱ��ȭ
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            chatInputField.ActivateInputField();

            if (string.IsNullOrEmpty(chatInputField.text))
                chatInputField.DeactivateInputField();//�Է� �ʵ� ��Ȱ��ȭ
            else
            {
                photonView.RPC("ReceiveMessage", RpcTarget.All, $"{PhotonNetwork.LocalPlayer.NickName} : {chatInputField.text}");//��� Ŭ���̾�Ʈ���� �޽��� ����

                chatInputField.text = string.Empty;//�Է� �ʵ� �ʱ�ȭ
                chatInputField.ActivateInputField();//�Է� �ʵ� Ȱ��ȭ
            }
        }
    }

    [PunRPC]
    public void ReceiveMessage(string message)
    {
        messageQueue.Enqueue(message);//�޽��� ť�� �޽��� �߰�

        if(messageQueue.Count > 50)//�޽��� ť�� ũ�Ⱑ 10�� �Ѿ��
            messageQueue.Dequeue();//���� ������ �޽��� ����

        chatText.text = string.Join('\n', messageQueue.ToArray());//�޽��� ť�� ��� �޽����� �Է� �ʵ忡 ǥ��
    }
}

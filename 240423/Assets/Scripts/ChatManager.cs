using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviourPunCallbacks
{
    [Tooltip("채팅 입력 필드")]
    [SerializeField] TMP_Text chatText;
    [SerializeField] TMP_InputField chatInputField;

    Queue<string> messageQueue = new Queue<string>();

    void Start()
    {
        chatText.text = string.Empty;//채팅 텍스트 초기화
        chatInputField.text = string.Empty;//채팅 입력 필드 초기화
        PhotonNetwork.IsMessageQueueRunning = true;//메시지 큐 수신 활성화
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            chatInputField.ActivateInputField();

            if (string.IsNullOrEmpty(chatInputField.text))
                chatInputField.DeactivateInputField();//입력 필드 비활성화
            else
            {
                photonView.RPC("ReceiveMessage", RpcTarget.All, $"{PhotonNetwork.LocalPlayer.NickName} : {chatInputField.text}");//모든 클라이언트에게 메시지 전송

                chatInputField.text = string.Empty;//입력 필드 초기화
                chatInputField.ActivateInputField();//입력 필드 활성화
            }
        }
    }

    [PunRPC]
    public void ReceiveMessage(string message)
    {
        messageQueue.Enqueue(message);//메시지 큐에 메시지 추가

        if(messageQueue.Count > 50)//메시지 큐의 크기가 10을 넘어가면
            messageQueue.Dequeue();//가장 오래된 메시지 제거

        chatText.text = string.Join('\n', messageQueue.ToArray());//메시지 큐의 모든 메시지를 입력 필드에 표시
    }
}

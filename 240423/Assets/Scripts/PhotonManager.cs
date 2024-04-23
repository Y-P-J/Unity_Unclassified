using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [Tooltip("닉네임 입력 필드")]
    [SerializeField] TMP_InputField nameInputField;
    [Tooltip("방 입장 버튼")]
    [SerializeField] Button connectButton;
    [Tooltip("네트워크 상태 텍스트")]
    [SerializeField] TMP_Text statusText;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();//포톤 서버에 접속(외부 기본세팅 사용)

        connectButton.onClick.AddListener(connectToRoom);//방 입장 버튼에 이벤트 추가
    }

    /// <summary>
    /// 닉네임 확인 후 방 입장
    /// </summary>
    public void connectToRoom()
    {
        if (string.IsNullOrEmpty(nameInputField.text))//닉네임이 비어있는 경우
        {
            Debug.Log("닉네임 비어있음.");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = nameInputField.text;
        PhotonNetwork.JoinRandomRoom();//랜덤 방 입장 시도
    }

    /// <summary>
    /// 랜덤 방 입장 실패시 호출되는 함수
    /// </summary>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room. Creating a room...");

        PhotonNetwork.CreateRoom("Room", new Photon.Realtime.RoomOptions { MaxPlayers = 10 });//방 생성(방을 생성하면서 본인도 입장)
    }

    /// <summary>
    /// 방 입장 성공시 호출되는 함수
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room.");

        PhotonNetwork.IsMessageQueueRunning = true;//메시지 큐 수신 활성화
        PhotonNetwork.LoadLevel("Chat");//채팅 씬으로 이동(동기화를 위해 LoadLevel 사용)
    }

    void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();//네트워크 상태 표시

        if (!PhotonNetwork.IsConnected)
            return;


        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            connectToRoom();
    }
}

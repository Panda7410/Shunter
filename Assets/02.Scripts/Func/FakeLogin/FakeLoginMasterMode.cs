using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace FakeLogin
{

    public class FakeLoginMasterMode : MonoBehaviour
    {
        FakeLogin fakeLogin;
        [Header("드롭다운")]
        public TMP_Dropdown dropdown;
        [Header("비밀번호 입력칸")]

        public TMP_InputField PasswordField;
        [Header("현재 비번")]

        public TextMeshProUGUI NowPass;
        [Header("세이브버튼클릭이벤트(성공)")]
        public UnityEvent OnSaveBtnClickEve;
        [Header("세이브 실패")]
        public UnityEvent<string> OnSaveFailEvent;

        // Start is called before the first frame update
        void Start()
        {
            fakeLogin = FindObjectOfType<FakeLogin>();
            dropdown.options.Clear();
            dropdown.captionText.text = fakeLogin.LoginDatas[0].DisplayName;
            NowPass.text = fakeLogin.LoginDatas[0].NowPassword;

                //표기명으로 드롭다운리스트 작성. 
            for (int i = 0; i < fakeLogin.LoginDatas.Count; i++)
            {
                TMP_Dropdown.OptionData data = new TMP_Dropdown.OptionData();
                data.text = fakeLogin.LoginDatas[i].DisplayName;
                dropdown.options.Add(data);
            }
            dropdown.onValueChanged.AddListener((t) => 
            {
                //표기명.
                string Selectex = dropdown.options[t].text;
                //표기 명이 일치하는 로그인데이터를 가져온다. 
                FakeLogin.LogInData editData = fakeLogin.LoginDatas.Find(f => f.DisplayName == Selectex);
                if(editData == null)
                {
                    Debug.LogError("드롭다운 구성에러");
                    return;
                }
                NowPass.text = editData.NowPassword;
            });


        }



        public void SaveBtnClick()
        {
            if(PasswordField.text.Length == 0)
            {
                Debug.LogError("비밀번호길이는 0자가 될 수 없습니다.");
                OnSaveFailEvent?.Invoke("비밀번호길이는 0자가 될 수 없습니다.");
                return;    
            }
            FakeLogin.LogInData oldData = fakeLogin.LoginDatas.Find(f => f.NowPassword == PasswordField.text);
            if (oldData != null)
            {
                Debug.LogError("비밀번호 설정이 같은 옵션이 존재합니다. ");
                OnSaveFailEvent?.Invoke("비밀번호 설정이 중복되는 옵션이 존재합니다.");
                return;
            }
            //표기명. 현재값을 가져온다
            string Selectex = dropdown.options[dropdown.value].text;
            //표기 명이 일치하는 로그인데이터를 가져온다. 
            FakeLogin.LogInData editData = fakeLogin.LoginDatas.Find(f => f.DisplayName == Selectex);
            if (editData == null)
            {
                Debug.LogError("드롭다운 구성에러");
                return;
            }

            fakeLogin.SavePassword(editData.SceneName, PasswordField.text);

            OnSaveBtnClickEve?.Invoke();
        }
    }
}

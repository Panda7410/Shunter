using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace FakeLogin
{
    public class FakeLogin : MonoBehaviour
    {
        public const string MasterPassword = "MASTER";
        const string Passwordcalibrate = "PASS";

        //로그인 성공시 동작 . 변수는 씬이름
        [Header("로그인 성공시 동작")]
        public UnityEvent<string> OnLoginSuccessEvent = new UnityEvent<string>();
        //로그인 실패시 동작
        [Header("로그인 실패시 동작")]
        public UnityEvent OnLoginFailEvent = new UnityEvent();
        [Header("마스터 모드 이벤트")]
        public UnityEvent OnMasterMode = new UnityEvent();

        [Header("로그인 데이터")]
        public List<LogInData> LoginDatas = new List<LogInData>();

        #region 바깥에서 바로 로그인시도.
        //직접입력용.
        string NowInputPass;
        public void InputNowPass(string NowInputPass)
        => this.NowInputPass = NowInputPass;
        public void LoginDirect()
        => Login(NowInputPass);
        #endregion


        private void Start()
        {
            Init();
        }



        public void Login(string password)
        {
            if (password.Length == 0)
                return;
            if (password == MasterPassword)
            {
                //마스터 패스워드를 입력함. 변경 모드로 이행.
                Debug.Log("마스터 모드");
                OnMasterMode?.Invoke();
                return;
            }
            //입력된 패스 워드의 존재여부. 
            LogInData editData = LoginDatas.Find(t => t.NowPassword == password);
            //패스워드가 일치하는 로그인데이터 존재하지 않음.
            if (editData == null)
            {
                OnLoginFailEvent?.Invoke();
            }
            else //일치 데이터 발견.
            {
                OnLoginSuccessEvent?.Invoke(editData.SceneName);
            }
        }

        public void SavePassword(string SceneName, string Password) // 패스워드 저장. 
        {
            LogInData editData = GetLogInData(SceneName);
            //키값에러. 이경우 내가 잘못만듬. 
            if (editData == null)
            {
                Debug.LogError("구성오류가 존재합니다.");
                return;
            }

            //비밀번호 저장.
            PlayerPrefs.SetString(GetPasscalibrate(editData.SceneName), Password);
            ////저장이 완료되면 다시 비밀번호를 불러온다. 
            Init();
        }

        #region 기본 내부 기능

        public List<string> GetKeys() // 외부에서 불러올때도 쓰자..!
        {
            //로그인데이터 리스트의 키(씬)이름만을를 반환.
            List<string> keys = new List<string>();
            for (int i = 0; i < LoginDatas.Count; i++)
            {
                keys.Add(LoginDatas[i].SceneName);
            }
            return keys;
        }

        void Init() //초기화
        {
            //키 리스트 불러오기
            List<string> Keys = GetKeys();
            //패스워드 확인. + 최신화.
            for (int i = 0; i < Keys.Count; i++)
            {
                LogInData editData = GetLogInData(Keys[i]);
                //키값에러. 이경우 내가 잘못만듬. 
                if (editData == null)
                {
                    Debug.LogError("구성오류가 존재합니다.");
                    continue;
                }
                //저장된 키값이 없는경우. 다음루프
                if (!PlayerPrefs.HasKey(GetPasscalibrate(editData.SceneName)))
                {
                    //현재 패스워드는 기본 패스워드와 동일.
                    editData.NowPassword = editData.DefPassword;
                    continue;
                }
                //키값이 존재.
                Debug.Log($"{editData.SceneName} 저장된 비밀번호를 불러옵니다. ");
                editData.NowPassword = PlayerPrefs.GetString(GetPasscalibrate(editData.SceneName));
            }
        }


        LogInData GetLogInData(string SceneName)
        {
            LogInData editData = LoginDatas.Find(t => t.SceneName == SceneName);
            if (editData == null)
            {
                //키값에러. 이경우 내가 잘못만듬. 
                Debug.LogError("구성오류가 존재합니다.");
                return null;
            }
            return editData;
        }

        string GetPasscalibrate(string Key) //키 보정값.
            => $"{Key}{Passwordcalibrate}";


        public void TestLog()
        {
            Debug.Log("로그인 실패다..!");
        }
        public void TestLog(string Scene)
        {
            Debug.Log($"로그인 성공..! 선택한 씬의 이름은 {Scene}");
        }

        #endregion





        [System.Serializable]
        public class LogInData
        {
            [Header("표기될 이름")]
            public string DisplayName;
            [Header("씬이름분류명")]
            public string SceneName;
            [Header("기본패스워드")]
            public string DefPassword;
            [Header("현재패스워드")]
            //현재의 패스워드
            public string NowPassword;
        }
    }
}

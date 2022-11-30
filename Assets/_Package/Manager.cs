using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NameScene
{
    Home,
    BallSort
}

public class Manager : MonoSingletonGlobal<Manager>
{
    protected override void Awake()
    {
        base.Awake();
        ShowLoading();
        RuntimeStorageData.ReadData();
    }

    public bool IsOpen = true;
    private Transform LoadingGlobal;

    public void ShowLoading()
    {
        //if (LoadingGlobal == null)
        //    LoadingGlobal = this.transform.FindChildByRecursion("GlobalLoading");

        //LoadingGlobal.SetActive(true);
    }

    public void HideLoading()
    {
        //if (LoadingGlobal == null)
        //    LoadingGlobal = this.transform.FindChildByRecursion("GlobalLoading");

        //LoadingGlobal.SetActive(false);
    }

    private void Log(string _string)
    {
        var _text = this.transform.FindChildByRecursion("Text (Legacy)").GetComponent<UnityEngine.UI.Text>();
        _text.text = _string;
    }

    private IEnumerator Start()
    {
#if FIREBASE_ENABLE
        yield return FirebaseManager.Instance.InitializedFirebase();
        yield return IronSourceManager.Instance.InitializedIronsource();
        yield return FirebaseManager.Instance.InitializedFirebaseMessaging();

        FirebaseManager.Instance.OpenApplication();
#endif
#if APPLOVIN_ENABLE

#endif
        yield return null;
        yield return new WaitUntil(() => RuntimeStorageData.IsReady);
        HideLoading();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            RuntimeStorageData.SaveAllData();
    }

    private void OnApplicationQuit()
    {
        RuntimeStorageData.SaveAllData();
    }

    public NameScene GetNameScene()
    {
        int indexScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        return (NameScene)indexScene;
    }
}

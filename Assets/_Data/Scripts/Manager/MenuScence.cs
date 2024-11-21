using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScene : MonoBehaviour
{

    [SerializeField] private LoadScene loadScene;
    [SerializeField] private Button newGame;
    [SerializeField] private Button continueGame;


    private void Start()
    {
        newGame.onClick.AddListener(NewGame);
        continueGame.onClick.AddListener(ContinueGame);

        if (!File.Exists(DataPath.InventoryData))
        {
            continueGame.gameObject.SetActive(false);
        }
    }

    [NaughtyAttributes.Button]
    public void ResetInventoryData()
    {
        if (File.Exists(DataPath.InventoryData))
        {
            File.Delete(DataPath.InventoryData);
        }
    }

    [NaughtyAttributes.Button]
    public void ResetWorldData()
    {
        if (File.Exists(DataPath.WorldData))
        {
            File.Delete(DataPath.WorldData);
        }
    }

    [NaughtyAttributes.Button]
    public void ResetGold()
    {
        PlayerPrefs.SetInt("Coins", 500);
    }

    [NaughtyAttributes.Button]
    public void LoadSceneGameplay()
    {
        SceneManager.LoadScene(1);
    }


    private void NewGame()
    {
        ResetInventoryData();
        ResetWorldData();
        ResetGold();
        loadScene.LoadScenes();
    }

    private void ContinueGame()
    {
        loadScene.LoadScenes();
    }

}

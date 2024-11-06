using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
public class DataPersistenceManager : MonoBehaviour
{
   
   [Header("File Storage Config")]
   [SerializeField] private string fileName;
   [SerializeField] private bool useEncryption;
   public static DataPersistenceManager instance { get; private set; }
   
   private GameData _gameData;
   private List<IDataPersistence> _dataPersistenceObjects;
   private FileDataHandler dataHandler;

   private void Awake()
   {
      if (instance != null)
      {
         Debug.LogError("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
      }

      instance = this;
      DontDestroyOnLoad(this.gameObject);
      this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
   }
   
   
   private void OnEnable() 
   {
      SceneManager.sceneLoaded += OnSceneLoaded;
      SceneManager.sceneUnloaded += OnSceneUnloaded;
   }

   private void OnDisable() 
   {
      SceneManager.sceneLoaded -= OnSceneLoaded;
      SceneManager.sceneUnloaded -= OnSceneUnloaded;
      
   }

   public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
   {
      this._dataPersistenceObjects = FindAllDataPersistenceObjects();
      LoadGame();
   }

   public void OnSceneUnloaded(Scene scene)
   {
      SaveGame();
   }
   public void NewGame() 
   {
      this._gameData = new GameData();
   }

   public void SaveGame()
   {
      // if we don't have any data to save, log a warning here
      if (this._gameData == null) 
      {
         Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
         return;
      }
      
      // pass the data to other scripts so they can update it
      foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
      {
         dataPersistenceObj.SaveData(_gameData);
      }
      Debug.Log("Saved coins = " + _gameData!.money);
      
      dataHandler.Save(_gameData);
   }

   public void LoadGame()
   {
      // load any saved data from a file using the data handler
      this._gameData = dataHandler.Load();
      
      // if no data can be loaded, don't continue
      if (this._gameData == null) 
      {
         Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
         return;
      }
      
      // push the loaded data to all other scripts that need it
      foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects) 
      {
         dataPersistenceObj.LoadData(_gameData);
      }
       Debug.Log("Loaded coins = " + _gameData!.money);
   }

   private void OnApplicationQuit()
   {
      SaveGame();
   }
   
   private List<IDataPersistence> FindAllDataPersistenceObjects() 
   {
      // FindObjectsofType takes in an optional boolean to include inactive gameobjects
      IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
         .OfType<IDataPersistence>();

      return new List<IDataPersistence>(dataPersistenceObjects);
   }
   
   public bool HasGameData() 
   {
      return _gameData != null;
   }
}

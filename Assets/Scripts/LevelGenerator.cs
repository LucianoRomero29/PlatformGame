using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using Unity.Services.Analytics;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator sharedInstance;

    //Esta lista almacena todos los disponibles, osea todos los creados como prefabs
    [SerializeField] private List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
    [SerializeField] private Transform levelStartPoint;

    //Esta lista se rellena dinamicamente
    [SerializeField] private List<LevelBlock> currentBlocks = new List<LevelBlock>();
    [SerializeField] private LevelBlock firstBlock;
    [SerializeField] private LevelUpPopup lvlUpPopup;
    private int distanceToNextLevel = 150, addDistanceToNewLevel = 10;
    [Header("Levels")]
    [SerializeField] private List<GameLevel> gameLevels;
    
    private bool allLevelsPassed = false;
    private int levelIndex;

    private void Awake() {
        sharedInstance = this;
    }

    private void Start() {
        GenerateInitialBlocks();
    }

    private void Update() {
        LevelUpByDistanceTraveled();
    }

    private void LevelUpByDistanceTraveled(){
        if(PlayerController.sharedInstance.GetDistance() > distanceToNextLevel + (addDistanceToNewLevel * GameManager.sharedInstance.levelIndex)){
            //Reseteo esta variable porque es la distancia que recorro por nivel, y si superó eso quiere decir que paso el nivel
            distanceToNextLevel += distanceToNextLevel * GameManager.sharedInstance.levelIndex;
            addDistanceToNewLevel += 10;
            GameManager.sharedInstance.levelIndex++;
   
            int levelIndex = GameManager.sharedInstance.levelIndex;
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"level_up", "Level " + levelIndex}
            };
            AnalyticsService.Instance.CustomData("levelComplete", parameters);

            Dictionary<string, object> parametersTwo = new Dictionary<string, object>()
            {
                {"level_index", levelIndex}
            };
            AnalyticsService.Instance.CustomData("levelStart", parametersTwo);


            lvlUpPopup.ShowPopup(GameManager.sharedInstance.levelIndex);
            //TODO: Falta un sonido de checkpoint

            if(GameManager.sharedInstance.levelIndex > gameLevels.Count){
                allLevelsPassed = true;
            }
        }
    }

    public void AddLevelBlock(bool restartGame = false){
        levelIndex = restartGame == true ? levelIndex = 1 : GameManager.sharedInstance.levelIndex;

        //Elige un bloque random
        int randomIndex = !allLevelsPassed ? Random.Range(0, gameLevels[levelIndex - 1].GetComponent<GameLevel>().level.Count) : Random.Range(0, allTheLevelBlocks.Count);

        //Instancia ese bloque elegido y lo hace hijo de este levelgenerator
        LevelBlock currentBlock; 

        //Si recien arranca el juego uso vector 0, sino el start position del bloque siguiente
        Vector3 spawnPosition = Vector3.zero;
        if(currentBlocks.Count == 0){
            //Si es el primero que genero, quiero que sea el bloque que yo elijo
            currentBlock = (LevelBlock)Instantiate(firstBlock);
            currentBlock.transform.SetParent(this.transform, false);
            spawnPosition = levelStartPoint.position;
        }else{
            currentBlock = !allLevelsPassed ? (LevelBlock)Instantiate(gameLevels[levelIndex - 1].GetComponent<GameLevel>().level[randomIndex]) : (LevelBlock)Instantiate(allTheLevelBlocks[randomIndex]);
            currentBlock.transform.SetParent(this.transform, false);
            spawnPosition = currentBlocks[currentBlocks.Count - 1].exitPoint.position;
        }

        //Este es el vector que me corrige los bloques de escena para que queden alineados correctamente
        Vector3 correction = new Vector3(spawnPosition.x - currentBlock.startPoint.position.x, spawnPosition.y - currentBlock.startPoint.position.y , 0f);

        //Lo pone en el mundo y lo agrega a la lista dinamica
        currentBlock.transform.position = correction;
        currentBlocks.Add(currentBlock);
    }

    public void RemoveOldestLevelBlock(){
        LevelBlock oldestBlock = currentBlocks[0];
        currentBlocks.Remove(oldestBlock);
        Destroy(oldestBlock.gameObject);
    }

    public void RemoveAllTheBlocks(){
        while(currentBlocks.Count > 0){
            RemoveOldestLevelBlock();
        }
    }

    public void GenerateInitialBlocks(bool restartLevel = false){
        for (var i = 0; i < 2; i++)
        {
            AddLevelBlock(restartLevel);
        }
    }
}

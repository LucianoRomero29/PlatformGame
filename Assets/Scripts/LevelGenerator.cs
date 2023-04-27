using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator sharedInstance;

    //Esta lista almacena todos los disponibles, osea todos los creados como prefabs
    [SerializeField] private List<LevelBlock> allTheLevelBlocks = new List<LevelBlock>();
    [SerializeField] private Transform levelStartPoint;

    //Esta lista se rellena dinamicamente
    [SerializeField] private List<LevelBlock> currentBlocks = new List<LevelBlock>();
    [SerializeField] private LevelBlock firstBlock;
    private int distanceToNextLevel = 500, addDistanceToNewLevel = 50, distanceTraveledByLevel = 0;
    
    [Header("Levels")]
    //Esta es una lista que tiene todos los niveles que iría a reemplazar a AllTheLevelBlocks
    //TODO: Sino me deja hacer una lista de listas, hacer tanta cantidad de listas como niveles tenga
    [SerializedField] private List<List<LevelBlock>> gameLevels = new List<List<LevelBlock>>();
    private bool allLevelsPassed = false;

    private void Awake() {
        sharedInstance = this;
    }

    private void Start() {
        GenerateInitialBlocks();
    }

    private void FixedUpdate() {
        LevelUpByDistanceTraveled();
    }

    private void LevelUpByDistanceTraveled(){
        distanceTraveledByLevel = PlayerController.sharedInstance.GetDistance();
        if(distanceTraveledByLevel > distanceToNextLevel + addDistanceToNewLevel * GameManager.sharedInstance.levelIndex){
            //Reseteo esta variable porque es la distancia que recorro por nivel, y si superó eso quiere decir que paso el nivel
            distanceTraveledByLevel = 0;
            addDistanceToNewLevel += 50;
            GameManager.sharedInstance.levelIndex++;
            Debug.Log("Subí al nivel: " + GameManager.sharedInstance.levelIndex);
            //TODO: Si esto llega a funcionar la idea es que cuando supere la cantidad de niveles que hay
            //utilizar ahora si la variable AllTheLevelBlocks la cual en esa guardaría todos los niveles
            //Y una vez superados (por ahora tenemos 5) los niveles, utilizar esta variable
            if(GameManager.sharedInstance.levelIndex > gameLevels.Count){
                allLevelsPassed = true;
            }
        }
    }

    public void AddLevelBlock(){
        //TODO: Acá reemplace allTheLevelBlocks en la linea 50 y 63 por gameLevel[levelIndex] 
        levelIndex = GameManager.sharedInstance.levelIndex;

        //Elige un bloque random
        int randomIndex = !allLevelsPassed ? Random.Range(0, gameLevels[levelIndex].Count) : Random.Range(0, allTheLevelBlocks.Count);

        //Instancia ese bloque elegido y lo hace hijo de este levelgenerator
        LevelBlock currentBlock; 

        //Si recien arranca el juego uso vector 0, sino el start position del bloque siguiente
        Vector3 spawnPosition = Vector3.zero;
        if(currentBlocks.Count == 0){
            //Si es el primero que genero, quiero que sea el bloque que yo elijo
            //TODO: Ver como hacer esto de FIRST BLOCK, creo que solo entra a este IF una vez al inicio
            currentBlock = (LevelBlock)Instantiate(firstBlock);
            currentBlock.transform.SetParent(this.transform, false);
            spawnPosition = levelStartPoint.position;
        }else{
            currentBlock = !allLevelsPassed ? (LevelBlock)Instantiate(gameLevels[levelIndex][randomIndex]) : (LevelBlock)Instantiate(allTheLevelBlocks[randomIndex]);
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

    public void GenerateInitialBlocks(){
        for (var i = 0; i < 2; i++)
        {
            AddLevelBlock();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Pontos de spawn para os inimigos
    public Transform[] spawnPoint;

    // Dados de spawn com informações sobre o inimigo a ser spawnado
    public SpawnData[] spawnData;

    // Tempo total para cada nível
    public float levelTime;
    
    // Nível atual e temporizador
    int level;
    float timer;

    // Lista para controlar quais spawn points foram usados recentemente
    List<int> usedSpawnPoints = new List<int>();

    void Awake()
    {
        // Coleta todos os pontos de spawn disponíveis
        spawnPoint = GetComponentsInChildren<Transform>();

        // Calcula o tempo para cada nível com base no tempo máximo do jogo
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {
        // Se o jogo não estiver ativo, não faz nada
        if (!GameManager.instance.isLive)
            return;

        // Conta o tempo para determinar o nível atual
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

        // Verifica se é o momento de spawnar inimigos para o nível atual
        if (timer > spawnData[level].spawnTime)
        {
            timer = 0;
            Spawn(); // Realiza o spawn de inimigos
        }
    }

    // Método para spawnar inimigos
    void Spawn()
    {
        int enemiesToSpawn = 1; // Ajusta a quantidade de inimigos a serem spawnados conforme necessário

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            GameObject enemy = GameManager.instance.pool.Get(0); // Obtém um inimigo da pool

            // Escolhe um spawn point que não foi usado recentemente
            int randomSpawnPointIndex = GetUnusedSpawnPointIndex();

            // Se não houver spawn points disponíveis (todos foram usados recentemente), reinicia a lista
            if (randomSpawnPointIndex == -1)
            {
                usedSpawnPoints.Clear();
                randomSpawnPointIndex = Random.Range(1, spawnPoint.Length);
            }

            // Guarda o spawn point utilizado
            usedSpawnPoints.Add(randomSpawnPointIndex);

            // Posiciona o inimigo no spawn point escolhido e inicializa com os dados do nível atual
            enemy.transform.position = spawnPoint[randomSpawnPointIndex].position;
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        }

        // Reduz o tempo de spawn até um valor mínimo
        float minSpawnTime = 0.2f; // Define o tempo mínimo de spawn desejado
        if (spawnData[level].spawnTime > minSpawnTime)
        {
            float decreasedSpawnTime = spawnData[level].spawnTime * 0.99f; // Reduz 1% do tempo
            spawnData[level].spawnTime = Mathf.Max(decreasedSpawnTime, minSpawnTime);
        }
    }

    // Retorna um índice de um spawn point que não foi usado recentemente
    int GetUnusedSpawnPointIndex()
    {
        List<int> availableSpawnPoints = new List<int>();

        for (int i = 1; i < spawnPoint.Length; i++)
        {
            if (!usedSpawnPoints.Contains(i))
            {
                availableSpawnPoints.Add(i);
            }
        }

        if (availableSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            return availableSpawnPoints[randomIndex];
        }
        else
        {
            return -1; // Retorna -1 se todos os spawn points foram usados recentemente
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public float spawnTime;     // Tempo para o próximo spawn
    public int spriteType;      // Tipo de sprite do inimigo
    public int health;          // Vida do inimigo
    public float speed;         // Velocidade do inimigo
    public float enemyDamage;   // Dano causado pelo inimigo
}

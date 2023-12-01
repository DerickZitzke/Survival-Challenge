using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instância estática do GameManager
    public static GameManager instance;

    [Header("# Controle do Jogo")]
    public bool isLive; // Controla se o jogo está em andamento ou não
    public float gameTime; // Tempo de jogo atual
    public float maxGameTime = 2 * 10f; // Tempo máximo de jogo
    
    [Header("# Informações do Jogador")]
    public int playerId; // ID do jogador
    public float health; // Vida do jogador
    public float maxHealth = 100; // Vida máxima do jogador
    public int level; // Nível do jogador
    public float fireSpeed; // Velocidade de disparo
    public float rotationSpeed; // Velocidade de rotação
    public int kill; // Quantidade de inimigos derrotados
    public int exp; // Experiência do jogador
    public float playerDamage; // Dano do jogador
    public int playerCount; // Contagem do jogador
    public int[] nextExp = { 2, 3, 6, 100, 150, 210, 280, 360, 450, 600}; // Valores necessários para o próximo nível
    
    [Header("# Game Object")]
    // Referências a vários objetos do jogo
    public PoolManager pool; // Gerenciador de Pool
    public Player player; // Jogador
    public LevelUp uiLevelUp; // Interface de nível
    public Result uiResult; // Interface de resultado
    public Transform uiJoy; // Interface de controle
    public GameObject enemyCleaner; // Objeto limpador de inimigos

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
        AudioManager.instance.PlayMbgm(true); // Inicia a música de fundo principal
    }

    // Inicia o jogo com um jogador específico
    public void GameStart(int id)
    {
        // Configura o jogador com valores iniciais
        playerId = id;
        health = maxHealth;

        // Ativa o jogador e alguns elementos da interface do usuário
        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);
        Resume(); // Continua o jogo
        AudioManager.instance.PlayBgm(true); // Inicia a música de fundo do jogo
        AudioManager.instance.PlayMbgm(false); // Interrompe a música de fundo principal
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); // Reproduz um efeito sonoro de seleção
    }

    // Função chamada quando o jogo acaba
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    // Rotina para o jogo acabar
    IEnumerator GameOverRoutine()
    {
        isLive = false; // Define o estado do jogo para "não vivo"
        yield return new WaitForSeconds(0.5f); // Espera por um curto período de tempo
        uiResult.gameObject.SetActive(true); // Ativa o painel de resultado
        uiResult.Lose(); // Mostra a mensagem de derrota no painel de resultado
        Stop(); // Para o jogo
        AudioManager.instance.PlayBgm(false); // Interrompe a música de fundo do jogo
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose); // Reproduz um efeito sonoro de derrota
    }

    // Função chamada quando o jogador vence o jogo
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    // Rotina para a vitória no jogo
    IEnumerator GameVictoryRoutine()
    {
        isLive = false; // Define o estado do jogo para "não vivo"
        DestroyAllEnemies(); // Destroi todos os inimigos
        enemyCleaner.SetActive(true); // Ativa um objeto limpador de inimigos
        yield return new WaitForSeconds(0.5f); // Espera por um curto período de tempo
        uiResult.gameObject.SetActive(true); // Ativa o painel de resultado
        uiResult.Win(); // Mostra a mensagem de vitória no painel de resultado
        Stop(); // Para o jogo
        AudioManager.instance.PlayBgm(false); // Interrompe a música de fundo do jogo
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win); // Reproduz um efeito sonoro de vitória
    }

    // Função para recomeçar o jogo
    public void GameRetry()
    {
        SceneManager.LoadScene(0); // Recarrega a cena do jogo
    }

    // Limpa todas as preferências do jogador
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared!");
    }

    // Sai do jogo
    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        // Verifica se o jogo está em andamento
        if (!isLive)
            return;

        gameTime += Time.deltaTime;

        // Verifica se o tempo de jogo atingiu o tempo máximo
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }
    }

    // Função para obter experiência
    public void GetExp()
    {
        if (!isLive)
            return;
        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length-1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    // Para o jogo
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }

    // Continua o jogo
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }

    // Destroi todos os inimigos
    public void DestroyAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (Enemy enemy in enemies)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}

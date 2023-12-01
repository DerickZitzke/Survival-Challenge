using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchiveManager : MonoBehaviour
{
    // Declaração de variáveis públicas para armazenar referências a objetos no Editor Unity
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    // Declaração de um enum chamado 'Achive' para representar diferentes tipos de conquistas
    enum Achive { UnlockAntonio, UnlockValentina }
    Achive[] achives; // Array de enumerações Achive para armazenar todas as conquistas disponíveis
    WaitForSecondsRealtime wait; // Objeto de espera usado para atrasos de tempo

    void Awake()
    {
        // Inicializa o array 'achives' com todos os valores do enum Achive
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5); // Define o objeto de espera com um atraso de 5 segundos

        // Verifica se não há dados salvos para inicializar as conquistas
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init(); // Inicializa os valores das conquistas no PlayerPrefs
        }
    }

    void Init()
    {
        // Define o valor de "MyData" como 1, indicando que há dados salvos
        PlayerPrefs.SetInt("MyData", 1);

        // Define todas as conquistas como não desbloqueadas (com valor 0 no PlayerPrefs)
        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    void Start()
    {
        UnlockCharacter(); // Inicializa o desbloqueio de personagens
    }

    void UnlockCharacter()
    {
        // Percorre os personagens bloqueados e desbloqueados com base nas conquistas alcançadas
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;

            // Define a visibilidade dos personagens com base no estado de desbloqueio
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        // Verifica todas as conquistas no final de cada quadro de atualização
        foreach (Achive achive in achives)
        {
            CheckAchive(achive); // Verifica se cada conquista foi alcançada
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;

        // Verifica as condições para cada tipo de conquista
        switch (achive)
        {
            case Achive.UnlockAntonio:
                isAchive = GameManager.instance.kill >= 10; // Verifica se o número de mortes é maior ou igual a 10
                break;

            case Achive.UnlockValentina:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime; // Verifica se o tempo de jogo é igual ao tempo máximo
                break;
        }

        // Se a conquista foi alcançada e ainda não foi desbloqueada, realiza a ação
        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1); // Marca a conquista como desbloqueada no PlayerPrefs

            // Ativa um aviso visual para a conquista desbloqueada
            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)achive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine()); // Inicia uma rotina para exibir o aviso por um tempo
        }
    }

    // Rotina para mostrar o aviso da conquista por um tempo determinado
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true); // Ativa o aviso visual
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp); // Toca um efeito sonoro

        yield return wait; // Espera o tempo definido

        uiNotice.SetActive(false); // Desativa o aviso visual
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Enumeração para tipos de informações exibidas no HUD
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type; // Tipo de informação

    Text myText; // Referência ao componente de texto
    Slider mySlider; // Referência ao componente slider

    void Awake()
    {
        // Obtém referências aos componentes Text e Slider
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        // Estrutura de seleção com base no tipo de informação
        switch (type)
        {
            case InfoType.Exp:
                // Obtém a experiência atual e máxima do GameManager
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                // Calcula e define o valor do Slider baseado na proporção da experiência atual em relação à experiência máxima
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                // Define o texto exibido como o nível atual do GameManager
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                // Define o texto exibido como a quantidade de inimigos derrotados
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                // Calcula o tempo restante do jogo e exibe no formato MM:SS
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                // Obtém a saúde atual e máxima do GameManager e define o valor do Slider com base na proporção
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    // Array de GameObjects para os títulos de vitória e derrota
    public GameObject[] titles;

    // Método chamado para mostrar a tela de derrota
    public void Lose()
    {
        // Ativa o GameObject do título de derrota
        titles[0].SetActive(true);
    }

    // Método chamado para mostrar a tela de vitória
    public void Win()
    {
        // Ativa o GameObject do título de vitória
        titles[1].SetActive(true);
    }
}

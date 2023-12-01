using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Variáveis para armazenar informações sobre o projétil
    public float damage; // Dano causado pelo projétil
    public int per; // Número de impactos que o projétil pode ter

    Rigidbody2D rigid; // Componente Rigidbody2D do projétil

    void Awake()
    {
        // Obtém o componente Rigidbody2D associado ao objeto
        rigid = GetComponent<Rigidbody2D>();
    }

    // Método para inicializar os parâmetros do projétil
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage; // Define o dano do projétil
        this.per = per; // Define o número de impactos que o projétil pode ter

        // Se o número de impactos for maior ou igual a zero, define a velocidade do projétil na direção 'dir'
        if (per >= 0)
        {
            rigid.velocity = dir * 15f; // Define a velocidade do projétil
        }
    }

    // Método chamado quando o projétil colide com outro Collider2D
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se a colisão não é com um inimigo ou se 'per' é igual a -100
        if (!collision.CompareTag("Enemy") || per == -100)
            return; // Retorna se não for um inimigo ou se 'per' for igual a -100

        per--; // Decrementa o contador de impactos

        // Se o número de impactos for menor que zero, para o projétil e desativa o objeto
        if (per < 0)
        {
            rigid.velocity = Vector2.zero; // Define a velocidade do projétil como zero
            gameObject.SetActive(false); // Desativa o objeto do projétil
        }    
    }

    // Método chamado quando o projétil sai de outro Collider2D
    void OnTriggerExit2D(Collider2D collision)
    {
        // Verifica se a saída não é de uma "área" ou se 'per' é igual a -100
        if (!collision.CompareTag("Area") || per == -100)
            return; // Retorna se não for uma "área" ou se 'per' for igual a -100

        gameObject.SetActive(false); // Desativa o objeto do projétil
    }
}

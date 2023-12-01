using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Variáveis de controle do jogador
    public Vector2 inputVec;    
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;
    public Enemy enemy;
    float baseSpeed;

    // Referências aos componentes do jogador
    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake() 
    {
        // Atribuição dos componentes necessários
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); // Busca pelos componentes 'Hand' nos filhos do jogador
        baseSpeed = speed; // Salva o valor original da velocidade do jogador
    }

    void OnEnable()
    {
        // Ajusta a velocidade do jogador com base em um valor global 'Character.Speed'
        speed *= Character.Speed;
        // Define o controlador de animação do jogador com base no ID do jogador
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
    }

    // Método chamado quando há movimento de entrada (Input) no jogador
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>(); // Obtém o valor do movimento do input
    }

    void Update()
    {
        // Se o jogo não estiver ativo, sai da função
        if (!GameManager.instance.isLive)
            return;
    }

    void FixedUpdate() 
    {
        // Se o jogo não estiver ativo, sai da função
        if (!GameManager.instance.isLive)
            return;

        // Calcula a próxima posição do jogador com base na velocidade e entrada do input
        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        // Se o jogo não estiver ativo, sai da função
        if (!GameManager.instance.isLive)
            return;

        // Define o parâmetro de animação 'Speed' com base na magnitude do input
        anim.SetFloat("Speed", inputVec.magnitude);

        // Vira o sprite do jogador de acordo com a direção do input
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Se o jogo não estiver ativo, sai da função
        if (!GameManager.instance.isLive)
            return;
        
        // Verifica se a colisão ocorreu com um inimigo (ou outra tag adequada)
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Acessa o script Enemy do objeto colidido
            Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();

            // Verifica se o script Enemy foi encontrado
            if (enemyScript != null)
            {
                // Reduz a saúde do jogador com base no dano do inimigo e no tempo
                GameManager.instance.health -= Time.deltaTime * enemyScript.enemyDamage;

                // Verifica se a saúde do jogador chegou a zero
                if (GameManager.instance.health < 0)
                {
                    // Desativa alguns elementos do jogador e executa a animação de morte
                    for (int index = 2; index < transform.childCount; index++)
                    {
                        transform.GetChild(index).gameObject.SetActive(false);                
                    }

                    anim.SetTrigger("Dead"); // Ativa o gatilho 'Dead' na animação do jogador
                    GameManager.instance.GameOver(); // Chama a função de Game Over do GameManager
                }
            }
        }
    }
}

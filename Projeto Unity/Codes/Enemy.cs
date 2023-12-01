using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Variáveis públicas que armazenam dados do inimigo, como velocidade, vida, dano, entre outros
    public float speed;
    public float health;
    public float maxHealth;
    public float enemyDamage;
    public RuntimeAnimatorController[] animCon;

    // Variáveis de controle e referência para componentes do inimigo
    public Rigidbody2D target;
    bool isLive;
    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        // Inicializa os componentes do inimigo
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        // Verifica se o jogo está ativo antes de executar as ações do inimigo
        if (!GameManager.instance.isLive)
            return;

        // Verifica se o inimigo está vivo e não está sendo atingido
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        // Move o inimigo em direção ao alvo (geralmente o jogador)
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
    
    void LateUpdate()
    {
        // Verifica se o jogo está ativo antes de executar as ações do inimigo
        if (!GameManager.instance.isLive)
            return;

        // Verifica se o inimigo está vivo e atualiza a orientação do sprite em direção ao alvo
        if (!isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // Ativado quando o inimigo é reativado
    void OnEnable()
    {
        // Obtém a referência do jogador e reinicializa os valores do inimigo
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    // Inicializa os parâmetros do inimigo com base nos dados de spawn
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
        enemyDamage = data.enemyDamage;
    }

    // Chamado quando o inimigo colide com uma bala
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se a colisão é com uma bala e se o inimigo está vivo
        if(!collision.CompareTag("Bullet") || !isLive)
            return;

        // Reduz a vida do inimigo com base no dano da bala e executa a lógica de impacto
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            // Se a vida for maior que zero, executa animação de acerto
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            // Se a vida for zero ou menor, o inimigo morre
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }        
    }

    // Corrotina que aplica um impulso de recuo no inimigo ao ser atingido
    IEnumerator KnockBack ()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    // Desativa o inimigo
    void Dead()
    {
        gameObject.SetActive(false);
    }
}

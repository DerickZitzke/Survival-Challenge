using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // ID e informações da arma
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;
    float timer;
    Player player;

    private void Awake() {
        // Atribui o jogador do GameManager à variável player
        player = GameManager.instance.player;
    }

    void Update()
    {
        // Se o jogo não está ativo, não faz nada
        if (!GameManager.instance.isLive)
            return;

        // Verifica o tipo de arma para execução de diferentes comportamentos
        switch (id)
        {
            case 0:
                // Rotação da arma
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                // Contagem de tempo para disparar a arma
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    Fire(); // Dispara a arma
                }

                break;  
        }

    }
    
    public void LevelUp(float damage, int count)
    {
        // Aumenta o dano e a contagem da arma de acordo com os parâmetros recebidos
        this.damage = damage * Character.Damage;
        this.count += count;

        if (id == 0){
            Batch(); // Realiza o agrupamento de balas para armas rotativas
            player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); // Aplica o equipamento ao jogador
        }

    }


    public void Init(ItemData data)
    {
        // Configurações básicas da arma
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Configuração das propriedades da arma com base nos dados do item
        id = data.itemId;
        damage = data.baseDamage * Character.Damage + GameManager.instance.playerDamage;
        count = data.baseCount + Character.Count + GameManager.instance.playerCount;

        // Encontra o prefab correspondente ao projetil na pool
        for (int index=0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        // Define a velocidade com base no tipo de arma
        switch (id)
        {
            case 0:
                speed = GameManager.instance.rotationSpeed * Character.WeaponSpeed;
                Batch(); // Agrupa as balas para armas rotativas
                break;
            default:
                speed = GameManager.instance.fireSpeed * Character.WeaponRate;
                break;  
        }

        // Configuração visual da mão da arma
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // Aplica o equipamento ao jogador
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        // Agrupa balas para armas rotativas
        for (int index=0; index < count; index++)
        {
            Transform bullet;
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else 
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            // Posiciona e inicializa as balas
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 é Penetração infinita
        }
    }

    void Fire ()
    {
        // Dispara a arma na direção do alvo mais próximo detectado pelo scanner
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position; 
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        // Obtém uma bala da pool e a dispara na direção do alvo
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform; 
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        // Reproduz o som do disparo da arma
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
        
    }

}

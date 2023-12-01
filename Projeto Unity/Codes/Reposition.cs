using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; // Define 'Random' como referência para a classe UnityEngine.Random

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>(); // Obtém o componente Collider2D do objeto atual
    }

    // Chamado quando um objeto deixa a colisão
    void OnTriggerExit2D(Collider2D collision)
    {
        // Se o objeto que colidiu não possuir a tag "Area", retorna
        if (!collision.CompareTag("Area"))
            return;
        
        // Obtém a posição do jogador e a posição do objeto atual
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        
        // Verifica a tag do objeto atual
        switch (transform.tag)
        {
            case "Ground":
                // Calcula a diferença de posição entre o jogador e o objeto atual
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;        
                float dirX = diffX < 0 ? -1 : 1; // Direção X com base na diferença
                float dirY = diffY < 0 ? -1 : 1; // Direção Y com base na diferença
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                // Se a diferença em X for maior que em Y, move na direção X
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 60); // Move o objeto na direção X
                }
                // Se a diferença em Y for maior que em X, move na direção Y
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 60); // Move o objeto na direção Y
                }
                break;
            case "Enemy":
                // Se o colisor estiver ativado
                if (coll.enabled)
                {
                    // Calcula a distância entre o jogador e o objeto atual
                    Vector3 dist = playerPos - myPos;
                    // Gera uma posição aleatória dentro de um intervalo
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    // Move o objeto para uma posição aleatória baseada na distância multiplicada por 2
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}

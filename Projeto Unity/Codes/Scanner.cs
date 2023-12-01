using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    // Raio de alcance do scanner
    public float scanRange;
    
    // Camada de alvos para o scanner detectar
    public LayerMask targetLayer;
    
    // Array de Raycasts para armazenar alvos detectados
    public RaycastHit2D[] targets;
    
    // Transform do alvo mais próximo
    public Transform nearestTarget;

    // Método chamado a cada atualização fixa do jogo
    void FixedUpdate()
    {
        // Detecta todos os objetos na área de alcance usando um círculo
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        
        // Encontra o alvo mais próximo
        nearestTarget = GetNearest();
    }

    // Método para encontrar o alvo mais próximo
    Transform GetNearest()
    {
        // Inicialização do resultado como nulo
        Transform result = null;
        
        // Distância inicial considerada grande para comparação
        float diff = 100;

        // Iteração por todos os alvos detectados
        foreach (RaycastHit2D target in targets)
        {
            // Posição do scanner
            Vector3 myPos = transform.position;
            
            // Posição do alvo atual
            Vector3 targetPos = target.transform.position;
            
            // Calcula a distância entre o scanner e o alvo atual
            float curDiff = Vector3.Distance(myPos, targetPos);

            // Verifica se a distância atual é menor que a distância anterior
            if (curDiff < diff)
            {
                // Atualiza a distância e define o alvo como o mais próximo
                diff = curDiff;
                result = target.transform;
            }
        }

        // Retorna o alvo mais próximo encontrado
        return result;
    }    
}

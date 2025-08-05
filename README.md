# 🎮 Survival Challenge

Um **jogo 2D** desenvolvido na **Unity**, **multiplataforma** (PC, Android e iOS), onde o jogador deve **sobreviver pelo tempo limite** enquanto enfrenta ondas de inimigos, sobe de nível e escolhe habilidades ou itens para aumentar suas chances de vitória.

---

## 🕹 Sobre o jogo

No **Survival Challenge**, o objetivo é simples: **sobreviver**.  
Você começa cercado por monstros que aparecem em diferentes pontos do mapa.  
Cada inimigo derrotado concede **experiência** e, ao subir de nível, você pode escolher entre uma **pool de habilidades e equipamentos** para personalizar sua jogabilidade.

O desafio aumenta progressivamente — quanto mais tempo você sobrevive, mais **rápidos, resistentes e perigosos** os inimigos se tornam.

---

## 🚀 Principais funcionalidades

- ⚔ **Sistema de combate** com armas corpo a corpo e de longo alcance.
- 🧠 **IA de inimigos** com perseguição e recuo ao serem atingidos.
- 🌀 **Sistema de spawn inteligente** com aumento gradual de dificuldade.
- 💾 **Object Pooling** para otimizar performance.
- 🎯 **Sistema de experiência e evolução** com escolha de habilidades/itens.
- 📦 **Itens e equipamentos** que afetam atributos como dano, velocidade e taxa de ataque.
- 🌍 **Cenário infinito** com reposicionamento dinâmico do terreno.
- 📱 **Compatível com múltiplas plataformas**: PC, Android e iOS.

---

## 🎨 Capturas de tela

**Menu inicial**
<img width="1179" height="663" alt="image" src="https://github.com/user-attachments/assets/68347f12-c9db-4671-b5e5-cd1298eec1e2" />

**Jogo**
<img width="1182" height="666" alt="image" src="https://github.com/user-attachments/assets/4bd922f0-9fc4-47ff-8772-90e5fb760bcd" />


---

## 🛠 Tecnologias utilizadas

- **Unity** (versão recomendada: `2021.x` ou superior)
- **C#** para scripts
- **Sistema de Object Pooling**
- **Animator Controller** para animações dos inimigos
- **UI Unity** para HUD e menus

---

## 📂 Estrutura simplificada do código

📦 Projeto Unity
┣ 📂 Codes
┃ ┣ 📜 PoolManager.cs # Gerencia instâncias reutilizáveis de objetos
┃ ┣ 📜 Spawner.cs # Spawn inteligente de inimigos
┃ ┣ 📜 Reposition.cs # Reposiciona terreno e inimigos fora da área
┃ ┣ 📜 Weapon.cs # Controle e disparo das armas
┃ ┣ 📜 Character.cs # Atributos estáticos do jogador
┃ ┣ 📜 Enemy.cs # IA e comportamento dos inimigos
┃ ┗ 📜 Item.cs # Sistema de itens, equipamentos e upgrades

---

## ▶️ Como jogar

1. **Movimente-se** para evitar os inimigos.
2. **Derrote monstros** para ganhar experiência.
3. **Suba de nível** e escolha habilidades ou itens.
4. **Sobreviva** até o tempo acabar para vencer!

---

## 📦 Como executar o projeto

### **No PC**
1. Baixe o projeto ou clone o repositório:
   ```bash
   git clone https://github.com/DerickZitzke/Survival-Challenge.git
Abra na Unity (versão compatível).

Compile para PC usando File > Build Settings.

No Android / iOS
Configure o módulo de exportação na Unity (Android Build Support / iOS Build Support).

Compile para a plataforma desejada.

Instale no dispositivo.

📜 Licença
Este projeto está sob a licença MIT — sinta-se livre para usar, modificar e distribuir.

💡 Feito com dedicação, código e muitas ondas de inimigos para enfrentar!

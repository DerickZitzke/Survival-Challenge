using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Declaração de variável estática que permite acessar a instância do AudioManager de qualquer lugar do código
    public static AudioManager instance;

    // Variáveis relacionadas aos sons de música de fundo (BGM)
    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    // Variáveis relacionadas aos sons de música de fundo múltipla (MBGM)
    [Header("#MBGM")]
    public AudioClip mbgmClip;
    public float mbgmVolume;
    AudioSource mbgmPlayer;
    AudioHighPassFilter mbgmEffect;

    // Variáveis relacionadas aos efeitos sonoros (SFX)
    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    // Enumeração que representa diferentes tipos de efeitos sonoros
    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }

    void Awake()
    {
        instance = this; // Atribui a instância atual do AudioManager à variável estática 'instance'
        Init(); // Inicializa o AudioManager
    }

    void Init()
    {
        // Cria um objeto vazio para reprodução do som de música de fundo (BGM) e define suas propriedades
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>(); // Obtém o componente AudioHighPassFilter da câmera principal

        // Cria um objeto vazio para reprodução do som de música de fundo múltipla (MBGM) e define suas propriedades
        GameObject mbgmObject = new GameObject("MbgmPlayer");
        mbgmObject.transform.parent = transform;
        mbgmPlayer = mbgmObject.AddComponent<AudioSource>();
        mbgmPlayer.playOnAwake = false;
        mbgmPlayer.loop = true;
        mbgmPlayer.volume = mbgmVolume;
        mbgmPlayer.clip = mbgmClip;
        mbgmEffect = Camera.main.GetComponent<AudioHighPassFilter>(); // Obtém o componente AudioHighPassFilter da câmera principal

        // Cria um objeto vazio para reprodução de efeitos sonoros (SFX) e define suas propriedades
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels]; // Inicializa um array de 'channels' elementos do tipo AudioSource

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            // Adiciona um AudioSource para cada canal e define algumas de suas propriedades
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    // Método para reproduzir música de fundo (BGM)
    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play(); // Inicia a reprodução do som de música de fundo
            mbgmPlayer.Play(); // Inicia a reprodução do som de música de fundo múltipla
        }
        else
        {
            bgmPlayer.Stop(); // Para a reprodução do som de música de fundo
            mbgmPlayer.Stop(); // Para a reprodução do som de música de fundo múltipla
        }
    }

    // Método para reproduzir som de música de fundo múltipla (MBGM)
    public void PlayMbgm(bool isPlay)
    {
        if (isPlay)
        {
            mbgmPlayer.Play(); // Inicia a reprodução do som de música de fundo múltipla
        }
        else
        {
            mbgmPlayer.Stop(); // Para a reprodução do som de música de fundo múltipla
        }
    }

    // Método para ativar/desativar efeitos de áudio de fundo (BGM)
    public void EffectBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmEffect.enabled = isPlay; // Ativa/desativa o efeito de áudio de fundo
            mbgmEffect.enabled = isPlay; // Ativa/desativa o efeito de áudio de fundo múltipla
        }
    }

    // Método para reproduzir efeitos sonoros (SFX)
    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length; // Calcula o índice do canal atual

            if (sfxPlayers[loopIndex].isPlaying)
                continue; // Se o canal estiver reproduzindo, passa para o próximo canal

            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2); // Gera um índice aleatório entre 0 e 1
            }

            channelIndex = loopIndex; // Atualiza o índice do canal atual
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex]; // Define o clipe de áudio a ser reproduzido
            sfxPlayers[loopIndex].Play(); // Inicia a reprodução do efeito sonoro
            break; // Sai do loop para tocar apenas um som por vez
        }
    }
}

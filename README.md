# 💎 Mini Arcade 2D - Time Attack Gem Collector

Un videogioco 2D arcade sviluppato in Unity. Il giocatore deve raccogliere il maggior numero di gemme possibili in un'arena prima che il timer scada.

Il progetto include un'architettura completa disaccoppiata basata su Eventi C#, gestione dinamica del flusso di gioco tramite `GameManager` persistente, asset grafici in Pixel Art e traccia audio retro 8-bit.

---

## 🎮 Game Loop e Dinamica di Gioco

1. **Menu Principale:** L'utente visualizza l'High Score globale salvato e avvia la partita.
2. **Game Scene (Time Attack):** 
   * Viene avviato un timer di **30 secondi**.
   * Il Player si muove liberamente in 8 direzioni (WASD / Frecce) con sprite animati dinamici.
   * Raccogliendo la gemma, il punteggio aumenta di **+1**, viene riprodotto un effetto sonoro dedicato e la gemma si riposiziona in un punto casuale della mappa.
3. **Pausa:** In qualsiasi momento è possibile mettere in pausa il gioco premendo `ESC` (il tempo e la fisica si congelano).
4. **Schermata Risultati:** Allo scadere del tempo si carica la scena finale, mostrando il punteggio ottenuto e l'eventuale badge **"New High Score"**.

---

## 🛠️ Architettura e Soluzioni Tecniche

* **Architecture Event-Driven:** Comunicazione pulita tramite `System.Action` (`OnGameStart`, `OnGameEnd`, `OnGemCollected`) senza accoppiamento rigido tra i componenti.
* **Persistent Manager (Singleton):** `GameManager` gestisce lo stato di gioco, il tempo, l'instanziazione differita tramite `SceneManager.sceneLoaded` e persiste tra le scene con `DontDestroyOnLoad`.
* **Zero Animator Controller:** Gestione delle `AnimationClip` direzionali (Walk/Idle per Front, Back e Side) direttamente da codice C# sfruttando le **Playables API** (`AnimationPlayableUtilities.PlayClip`) e `flipX`.
* **Multi-Canvas UI Layering:**
  * `BackgroundCanvas` (`Screen Space - Camera`): Sfondo erboso renderizzato sotto il mondo 2D.
  * `HUDCanvas` (`Screen Space - Overlay`): Interfaccia utente (Score, Timer, Bottoni) sempre renderizzata sopra al Player.

---

## 📁 Struttura del Progetto

```text
Assets/
 ├── Audio/
 │    ├── arcade_bgm_loop.wav     # Musica di sottofondo 8-bit (120 BPM)
 │    └── gem_collect.wav         # Effetto sonoro al pick-up della gemma
 ├── Animations/                  # AnimationClips per Walk/Idle (Front, Back, Side)
 ├── Prefabs/
 │    ├── [GameManager].prefab    # Controller persistente tra scene
 │    ├── Player.prefab           # Personaggio giocabile 8-bit
 │    └── Gem.prefab              # Gemma collezionabile
 ├── Scenes/
 │    ├── MenuScene.unity         # Menu iniziale e High Score
 │    ├── GameScene.unity         # Arena di gioco
 │    └── ResultsScene.unity      # Schermata finale di riepilogo
 ├── Scripts/
 │    ├── GameManager.cs          # Gestore dello stato di gioco, scene e timer
 │    ├── PlayerController.cs     # Input System, movimento 2D e Playables Animation
 │    ├── Gem.cs                  # Trigger collisioni, coroutine di respawn ed eventi
 │    └── GameUIManager.cs        # Controllo UI, Pausa (Time.timeScale) e Canvas
 └── Sprites/
      ├── player_spritesheet.png  # Sprite Sheet 32x32 8-bit (Pixel Art)
      └── smooth_grass_bg.png     # Sfondo mappa fluido senza griglia (1920x1080)
```

---

## ⚙️ Requisiti e Setup

### Requisiti di Sistema
* **Unity Version:** 2022.3 LTS o successiva (compatibile anche con Unity 2023 / 6).
* **Render Pipeline:** 2D / Universal Render Pipeline (URP) o Built-in 2D.
* **Packages:** `Input System` (nuovo o legacy Keyboard), `TextMeshPro`.

### Istruzioni per la Configurazione
1. Clona il repository:
   ```bash
   git clone https://github.com/tuo-username/mini-arcade-2d.git
   ```
2. Apri il progetto con **Unity Hub**.
3. Assicurati che le scene siano aggiunte in **Build Settings** (`File -> Build Settings`):
   * `Index 0:` `Assets/Scenes/MenuScene.unity`
   * `Index 1:` `Assets/Scenes/GameScene.unity`
   * `Index 2:` `Assets/Scenes/ResultsScene.unity`
4. Apri la scena `MenuScene` e premi **Play**.

https://markof87.itch.io/runeheadstechnicaltest

---

## 🕹️ Comandi di Gioco

| Tasto | Azione |
| :--- | :--- |
| **W A S D** / **Frecce Direzionali** | Movimento del Player |
| **ESC** | Pausa / Ripristino Gioco |
| **Mouse Left Click** | Interazione con Bottoni UI |

---
## 📝 Postmortem & Key Learnings
# 💎 Gem Collector Prototype

Un prototipo di videogioco 2D arcade sviluppato in Unity. Il giocatore deve raccogliere il maggior numero di gemme possibili in un'arena prima che il timer scada.

---

## 🎮 Game Loop e Dinamica di Gioco

1. **Menu Principale:** L'utente visualizza l'High Score globale salvato e avvia la partita.
2. **Game Scene (Time Attack):** 
   * Viene avviato un timer di **30 secondi**.
   * Il Player si muove liberamente in 8 direzioni (WASD / Frecce) con sprite animati dinamici.
   * Raccogliendo la gemma, il punteggio aumenta di **+1**, viene riprodotto un effetto sonoro dedicato e la gemma si riposiziona in un punto casuale della mappa.
3. **Schermata Risultati:** Allo scadere del tempo si può andare alla schermata del risultato, mostrando il punteggio ottenuto e l'eventuale badge **"New Record"** qualora fosse il più alto.

---

## 🛠️ Architettura e Soluzioni Tecniche

* **Architecture Event-Driven (Observer):** Comunicazione pulita tramite `System.Action` senza accoppiamento rigido tra i componenti.
* **Persistent Manager (Singleton):** `GameManager` gestisce lo stato di gioco, il tempo, e persiste tra le scene con il pattern Singleton (`DontDestroyOnLoad`).
* **ScoreData (ScriptableObject):** ScriptableObject contenente l'ultimo risultato, il più alto ed un flag `isRecord` per verificare che sia un nuovo record.
* **Build  (WebGL):** Build del gioco caricata su pagina web usando le librerie WebGL.

---

## ⚙️ Requisiti e Setup

### Requisiti di Sistema
* **Unity Version:** 6000.0.62f1 o successiva (compatibile anche con versioni precedenti).
* **Packages:** `New Input System` per il controllo del Player, `TextMeshPro` per la resa grafica delle stringhe di testo.

### Istruzioni per la Configurazione
1. Clona il repository:
   ```bash
   git clone https://github.com/Markof87/RuneHeadsTechnicalTest.git
   ```
2. Apri il progetto con **Unity Hub**.
3. Assicurati che le scene siano aggiunte in **Build Settings** (`File -> Build Settings`):
   * `Index 0:` `Assets/Scenes/Menu.unity`
   * `Index 1:` `Assets/Scenes/Game.unity`
   * `Index 2:` `Assets/Scenes/Results.unity`
4. Apri la scena `Menu` e premi **Play**.

### Assets del gioco

Le sprites del Player e del Gem provengono da pack di assets acquistati a titolo personale.
Il font utilizzato per le stringhe (Fredoka) è stato prelevato dalle librerie di Google Fonts.
I file audio e l'immagine di backgroud sono generati da intelligenza artificiale (Gemini).
N.B: in questo progetto l'intelligenza artificiale è stata utilizzata anche per generare file più "verbosi" (`.gitignore`, la prima versione del `README.md`...), ma nella scrittura del codice sono state utilizzate solo alcune funzionalità di Github Copilot per una maggiore velocità di scrittura.
Nessun "vibe coding" per gli script.

### Build e link del gioco

* **Test del gioco (itch.io):**

https://markof87.itch.io/runeheadstechnicaltest

* **Archivio della build (esportabile su altre piattaforme):**

https://github.com/Markof87/RuneHeadsTechnicalTest/blob/main/Build.zip

---

## 🕹️ Comandi di Gioco

| Tasto | Azione |
| :--- | :--- |
| **W A S D** / **Frecce Direzionali** | Movimento del Player |
| **Mouse Left Click** | Interazione con Bottoni UI |

---

## 📝 Postmortem & Key Learnings

### Scelte rivedibili e possibili estensioni

* Con lo sviluppo del gioco, la scelta dello `ScriptableObject` per il salvataggio del punteggio è apparsa un po' ridondante. Tuttavia, per un'eventuale estensione del prototipo, si può utilizzare questo strumento per estendere le meccaniche di game design (power-up, weapons ecc...)
* Fin dall'inizio si è scelto di far apparire una gemma alla volta nell'area di gioco per semplificare alcune meccaniche. Si poteva estendere la frequenza di spawn su un numero impostabile da Inspector.
* Il game loop di base è piuttosto scarno, ma con diverse giornate di sviluppo si può creare un set di gemme dalle diverse proprietà (estensione del timer, power-up...), ostacoli e nemici (semplici o anche boss) che possono far terminare il gioco anticipatamente con la morte del Player, ed anche diversi "character" (ognuno con le proprie caratteristiche) sbloccabili con determinati traguardi, oltre a diverse mappe e livelli bonus.

### Key Learnings

Oltre alla pura finalità del prototipo, lo sviluppo di un progetto semplice, ma con diverse scelte di game design lasciate volutamente libere, è stato particolarmente utile per cercare soluzioni efficaci ma allo stesso tempo contenute nelle ore di sviluppo.
Anche se non richieste, l'utilizzo di asset grafici e qualche funzionalità più avanzata di Unity (audio, animations...) mi ha permesso di rivedere e di risolvere alcune problematiche spesso sottovalutate.
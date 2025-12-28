<div align="center">
    <h1>GalEngine</h1>
    <strong>Visual Novel Engine</strong>
    <p>🚧 Currently under active development 🚧</p>
    <p><a href="README.md">简体中文</a> | English</p>
    <hr>
</div>

Recently, *GalGames* have become a frequently discussed topic on [bilibili](https://www.bilibili.com). As a Unity beginner, I thought: *"Why not try making a GalGame myself?"* (Definitely not just to ride the trend 🤨).

> My English isn't very good, so I used AI to create this README. If you find any errors, thank you for pointing them out.

## 1. Introduction

### 1.1. What is a *GalGame*?

*GalGame* is essentially a game genre originating from Japan, short for "Gyaru Game" (from the Japanese word **ギャル**, meaning "gal" or "stylish girl"). It is also commonly referred to as a **visual novel**.

The core of a GalGame revolves around narrative—typically romance-focused—and it belongs to the broader category of story-driven games. Crucially, for a game to be classified as a GalGame, **the plot must be the primary gameplay element**. Games with significant non-narrative mechanics (e.g., action, puzzles, etc.) do not qualify as GalGames, even if they include romantic storylines.

So, GalGame is actually a legitimate and well-defined game genre—though on bilibili, it has unfortunately become something of an internet meme due to certain contextual reasons.

### 1.2. About GalEngine

As the name suggests, **GalEngine** is an engine designed specifically for creating GalGames.

I’ve built a foundational framework that allows you to write plot content in structured JSON files. Once formatted correctly, these files can be played directly within GalEngine.

You simply place your completed plot package into a designated directory (`{Application.persistentDataPath}/Packs`). GalEngine will automatically scan this folder, list all available packages, and let users click to start playing any of them.

> ⚠️ **Note**: This is my **first Unity project** and also my **first C# project**. The earliest code (specifically the plot playback logic in the `DisplayTalkingInformation` class) was written just **2 weeks after learning C#** and **4 days after starting with Unity**. As of writing this (2025-12-27), I’ve only been using Unity for about a month. Therefore, some logic may be unclear or implemented suboptimally. Your understanding is appreciated—I still have much to learn!

### 1.3. Open Source

#### 1.3.1. Code
All source code is released under the [MIT LICENSE](https://mit-license.org/), which means you are free to:
- Use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the software.

However, you **must**:
- Include **attribution**: credit me by name and include a copy of this license in your project.

#### 1.3.2. Assets
All UI assets are released under the [CC0 1.0 Universal Public Domain Dedication](https://creativecommons.org/publicdomain/zero/1.0/legalcode.txt), meaning you may:
- Use the assets in any way, for any purpose, **without attribution**.

> 💡 This license applies **only** to UI assets—specifically all `.ai` files located in `/Resources/Image/Illustration`.

---

## 2. Creation

### 2.1. Plot Package Creation

In the future, I plan to develop **GalCraft**—a GUI-based tool that will allow you to create plot packages without manually editing JSON files.

For now, however, you’ll need to write JSON by hand. Below is a guide to the required format and standard structure.

#### Configuration File (`config.json`)
Every plot package must include a `config.json` file at its root directory, formatted like this:

```json
{
  "title": "Your Pack Title",           // Title of your package
  "author": "Your_name",                // Your name
  "link": "https://your.social.link",   // Optional: your social link
  "id": "your_pack_id",                 // Unique identifier for your package
  "version": "1.0.0",                   // Optional: package version
  "description": "Your pack description", // Optional: brief description
  "license": "MIT",                     // Optional: license of your package
  "cover": "cover.png",                 // Optional: cover image (aspect ratio 778:591 or 1556×1182; other ratios will be stretched)
  "index": "plot/index.json"            // Entry point: path to your main plot JSON file
}
```

#### Plot File (Core Content)
This is the heart of your package—it contains the entire plot. Currently, two node types are supported: `node` (standard dialogue) and `branch` (player choices).

> 🔹 The entire JSON file must be wrapped in a top-level **array**.

```json
[
  // Example of a simple node
  {
    "node": 0,                            // Node ID (matches array index, starting from 0)
    "type": "node",                       // Node type: "node" or "branch"
    "character": "character_name",        // Speaker's name (leave empty for no name)
    "description": "the character description", // Optional descriptor shown in small font next to the name (e.g., "Mysterious girl", "Café clerk")
    "value": "character talk",            // Dialogue text displayed in the speech box
    "background": "image/background/background.png", // Background image (relative path); if omitted, previous background is reused (experimental)
    "voice": "sounds/vocals/voice.ogg",   // Voice line (under development)
    "sound": "sounds/effects/sound.ogg",  // Sound effect (under development)
    "character_illustration": "image/char/character.png", // Character illustration (leave empty for none; under development)
    "facial_differential": "image/diff/character_name.feel.png" // Facial expression variant; reuses last if empty, defaults if none exists (under development)
  },

  // Example of a branch node
  {
    "node": 1,
    "type": "branch",
    "choice": [
      {
        "ordinal": 0,                     // Choice ID (matches array index, starting from 0)
        "value": "Choice1",               // Text shown to the player
        "branch": [                       // Nested array of nodes executed if this choice is selected
          // ... more nodes here
        ]
      },
      {
        "ordinal": 1,
        "value": "Choice2",
        "branch": [
          // ... more nodes here
        ]
      }
    ]
  }
]
```

> All path fields use **relative paths** (relative to the package root).

#### Recommended Directory Structure
```bash
.
├── config.json                 # Configuration file
├── cover.png                   # Cover image
├── LICENSE.txt                 # License file (optional but recommended)
├── image/
│   ├── background/             # Background images
│   ├── char/                   # Character illustrations
│   └── diff/                   # Facial expression variants
├── plot/
│   └── index.json              # Entry plot file
└── sounds/
    ├── effects/                # Sound effects
    ├── music/                  # Background music
    └── vocals/                 # Voice lines
```

### 2.2. Other Customizations

Currently, only **page background customization** is supported. More UI customization options may be added later.

#### Custom Page Background
The current page background is generated using [`Stable Diffusion`](https://stability.ai/), and you can choose your favorite image to replace it.
You can customize the background of the package selection screen by placing files in `{Application.persistentDataPath}`.

On Windows, this is typically:  
`C:\Users\<Your_name>\AppData\LocalLow\MillenTec\GalEngine`

Steps:
1. Create a folder named `UI`.
2. Inside it, create a subfolder named `pageBackground`.
3. Place your background image and a `config.json` inside:

```json
{
  "image": "background.png"     // Relative path to your background image
}
```

---

## 3. Development Environment

- **Unity**: Unity 2022.3.62f1c1 LTS
- **IDE**: JetBrains Rider 2025.3.1 (Non-commercial license)
- **.NET Framework**: v4.7.1
- **C# Language Version**: 9.0
- **OS**: Windows 11 Pro 25H2
- **UI Design**: Adobe Illustrator 2024

---

## 4. Quick Start

1. Clone the repository:
   ```bash
   git clone https://github.com/MillenTec/GalEngine.git
   ```
2. Open the folder in **Unity Hub** using the specified Unity version.
3. Click **Play** to run in the Unity Editor,  
   **or** go to `File > Build Settings`, select your target platform, click **Build**, and choose an output directory to generate an `.exe` (or other platform executable).

---

## 5. Third-Party Dependencies

- **Newtonsoft.Json**
    - License: [MIT LICENSE](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)
    - GitHub: https://github.com/JamesNK/Newtonsoft.Json

- **HarmonyOS Sans Font**
    - Website: [Han Yi Fonts](https://www.hanyi.com.cn/custom-font-case-7)
    - License: [LICENSE.HarmonyOS-Sans](third_party/LICENSE.HarmonyOS-Sans)

---

## 6. About Me

Hi! I’m **MillenTec**, a Chinese student currently learning C#, and a self-proclaimed anime-loving tech geek. You can call me **“Millen”**—though that’s not my real name 😄

**Contact me:**
- GitHub: [MillenTec](https://github.com/MillenTec)
- bilibili: [MillenTec](https://space.bilibili.com/3546591566760474)
- Email: MillenTec@outlook.com
- Community: [Microsoft Teams](https://teams.live.com/l/community/FBAPJVv6nA7sOvEzgI)
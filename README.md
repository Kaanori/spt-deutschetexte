# DeutscheTexte (German texts)

Adds **12,257 German texts**: 9,143 for items and quests, and since 1.1.0 **3,114 dialogue lines** of the Storyline Backport (VisitAPI).

## How this mod was made (AI disclosure)

This mod was **programmed by Claude**, an AI assistant made by Anthropic.
**Kaan was the tester**: Kaan has no programming background, played with the mod
on SPT 4.1.6, reported what went wrong, and Claude fixed it. No human programmer
has reviewed the code. It is open source so anyone can check it — if you find a
bug, please open an [issue](https://github.com/Kaanori/spt-deutschetexte/issues).

## Requirements

- SPT 4.1.6 (server).

## Installation

1. Download `DeutscheTexte-1.1.0.zip` from the [Releases](https://github.com/Kaanori/spt-deutschetexte/releases) page.
2. Extract it **into your SPT game folder** (the folder that contains `EscapeFromTarkov.exe`).
   The ZIP already has the right layout; the mod ends up in `SPT_Runtime/user/mods/DeutscheTexte/`.
   On Windows, make sure "Extract All" does not add an extra subfolder.
3. Restart the SPT server.

## Uninstall

Delete the mod's files from `SPT_Runtime/user/mods/DeutscheTexte/` (settings: `BepInEx/config/` for client mods) and restart the SPT server.

## Why a mod and not directly in the database

The files under `SPT_Data/database` are protected by checksums (`checks.dat`);
if you change them, the server will no longer start.

In addition, SPT loads the language table **lazily**. The mod therefore hooks
in with a transformer: it only runs once SPT actually reads the German
language file; before that there would be nothing to add to.

## Origin of the texts

The texts are **self-made**: translated into German from the English names and
descriptions with custom Python scripts and then merged into one file.

**Nothing** was taken from the game's German language file and nothing was
copied from another translation mod. The scripts themselves no longer exist;
their result, `texte-ge.json`, has been kept.

For classification see LICENSES.md, section 2.3.

## Storyline dialogues (1.1.0)

Since 1.1.0 the mod also contains German translations of all **3,114 dialogue lines** of the *Storyline Backport* pack for **VisitAPI** (trader conversations, narrations, jokes). The English originals belong to their authors – all credit for the story, characters and writing goes to them; this mod only adds the German translation.

- They only show up if VisitAPI with the Storyline pack is installed; without it the extra entries are simply unused.
- VisitAPI only fills in texts that are still missing, while this mod sets them, so the German version wins regardless of load order. The Storyline files themselves are not changed.
- Names of people, maps, factions and weapons stay in English, as in the German version of the game. The translation was done with the help of an AI assistant (Claude).

## File layout

`texte-ge.json` is a flat mapping from key to text. The keys are the ones SPT
uses, i.e. template ID plus field name:

```json
{
  "544fb25a4bdc2dfb738b4567 ShortName": "Verband",
  "5580239d4bdc2de7118b4583 Description": "Eine schwere Mündungsbremse …"
}
```

Common field names are `Name`, `ShortName` and `Description`.

You can add your own entries directly to the installed file at
`SPT_Runtime/user/mods/DeutscheTexte/texte-ge.json`. Rebuilding with
`--install` does **not** overwrite it if it already exists.

## Building from source

Needs the .NET 10 SDK and an SPT 4.1.6 installation to build against.

```bash
./build.sh             # Linux: build only
./build.sh --install   # build and copy into the game folder
```
```powershell
.\build.ps1            # Windows
.\build.ps1 -Install
```

The game folder is set in `Directory.Build.props` (Windows default `C:/SPT`)
or via the environment variable `SPT_DIR`.

## Support

Bugs and questions: [GitHub issues](https://github.com/Kaanori/spt-deutschetexte/issues).

## License

MIT — see [LICENSE](LICENSE).

using System.Reflection;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Spt.Tables;
using SPTarkov.Common.Models.Logging;

namespace DeutscheTexte;

/// <summary>
/// Traegt deutsche Texte fuer Gegenstaende und Aufgaben nach.
///
/// Warum als Mod und nicht direkt in der Datenbank? Die Dateien unter
/// SPT_Data/database sind pruefsummengesichert (checks.dat) - wer sie aendert,
/// bekommt den Server nicht mehr gestartet. Die Sprachtabelle wird ausserdem
/// verzoegert geladen, deshalb haengt sich diese Mod mit einem Transformer ein:
/// Er laeuft erst, wenn SPT die deutsche Sprachdatei tatsaechlich einliest.
/// </summary>
public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "com.kaan.deutschetexte";
    public string Name { get; init; } = "Deutsche Texte";
    public string Author { get; init; } = "Kaan";
    public List<string>? Contributors { get; init; }
    public bool HasPrepatcher { get; init; } = false;
    public SemanticVersioning.Version Version { get; init; } = new("1.0.0");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public string? Url { get; init; }
    public bool? IsBundleMod { get; init; } = false;
    public string? License { get; init; } = "MIT";
}

[Injectable]
public class DeutscheTexte(
    ISptLogger<DeutscheTexte> logger,
    ModHelper modHelper,
    LocaleTable localeTable) : IOnLoad
{
    private const string Sprache = "ge";
    private const string Datei = "texte-ge.json";

    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        var ordner = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        var texte = modHelper.GetJsonDataFromFile<Dictionary<string, string>>(ordner, Datei);

        if (texte is null || texte.Count == 0)
        {
            logger.Warning($"[Deutsche Texte] {Datei} ist leer - es wird nichts geaendert.");
            return Task.CompletedTask;
        }

        if (!localeTable.Global.TryGetValue(Sprache, out var sprachdatei))
        {
            logger.Warning($"[Deutsche Texte] Sprache '{Sprache}' gibt es nicht - uebersprungen.");
            return Task.CompletedTask;
        }

        sprachdatei.AddTransformer(geladen =>
        {
            if (geladen is null)
            {
                return geladen;
            }

            var woerterbuch = (Dictionary<string, string>)(object)geladen;
            foreach (var (schluessel, text) in texte)
            {
                woerterbuch[schluessel] = text;
            }

            return geladen;
        });

        logger.Success($"[Deutsche Texte] {texte.Count} Texte werden auf Deutsch gesetzt");
        return Task.CompletedTask;
    }
}

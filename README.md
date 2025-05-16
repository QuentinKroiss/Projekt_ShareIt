## ShareIt - Blazor WebApp für Artikelverwaltung

ShareIt ist eine Webanwendung zum Teilen und Verwalten von Artikeln 
wie Lebensmittel, Getränke oder Gegenstände.  
Die Anwendung wurde entwickelt mit:
- BlazorWebassembly
- ASP.NET Core
- Entity Framework Core (SQLite)
- Identity (Zur Nutzerverwaltung)

## Features
--> Registrierung und Login mit JWT  
--> Upload von Artikeln mit Bild, Beschreibung und MHD  
> **Hinweis:**  
> Die Eingabe eines MHD wird nur für Getränke und Lebensmittel angezeigt.

--> Bildersuche über API, auf Grundlage eines Artikeltitels.  
> **Hinweis:**  
> In dem Projekt wurde ein privater API-Key von Pexels genutzt.  
> Um den vollen Umfang der Bildersuche nutzen zu können, müssen Sie in  
> `ShareIt\Client\Service\PexelsImageService.cs` in **Zeile 14** Ihren API-Key hinterlegen.  
> Andernfalls können Artikel nur mit einem grauen Standard-Icon hochgeladen werden.  
> Das Projekt wurde für eine optimale Anwendung mit der Nutzung eines API-Keys optimiert.  
> Aus Lizenzgründen ist dieser jedoch im GIT-Repository nicht enthalten.

--> Userspezifische Anzeige
> **Hinweis:**  
> Ein eingeloggter User sieht im Home-Bereich alle Artikel, welche andere User hochgeladen haben,
> Artikel die ein User selbst hochgeladen hat, sieht er im Bereich `Meine Verläufe`

--> Artikelanzeige kategorisiert in Lebensmittel, Getränke, Objekte
--> Responsives Design mit der Erkennung eines Login-Status
--> Löschen von Usern
> **Hinweis:**  
> Ein User kann sein Konto nur löschen, wenn sein Passwort 2x korrekt eingegeben wird.

--> Ansicht von Aufrufen durch andere User für einen Artikel

## Testzugänge
**Die Anwendung enthält 2 vordefinierte Demo-User mit jeweils zu Beginn 3 hinterlegten Artikeln**
| Benutzername       | Passwort     |
|--------------------|--------------|
| `Deko@gmx.de`      | `Abc123!`     |
| `Test@gmx.de`      | `Test123!`    |  

Für die Nutzung der Datenbank wurde eine SQLite DB implementiert, welche durch hinterlegen von  
`context.Database.EnsureCreated();` direkt beim ersten Starten der Anwendung eine automatische  
Migration durchführt. Es ist demnach keine manuelle Migration einer DB mehr notwendig. Dies wurde  
für eine besser Darstellung dieser Demo-Version umgesetzt.  

## TechStack
- **Frontend**: Blazor WebAssembly
- **Backend**: ASP.NET Core Web API
- **Datenbank**: SQLite (automatisch erzeugt über `EnsureCreated()`)
- **Auth**: ASP.NET Core Identity mit JWT
- **API**: Pexels (Bildvorschläge)
- **Tooling**: Visual Studio, Swagger (API-Dokumentation)

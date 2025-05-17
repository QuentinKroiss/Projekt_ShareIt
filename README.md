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
für eine bessere Darstellung dieser Demo-Version umgesetzt.  

## TechStack
- **Frontend**: Blazor WebAssembly
- **Backend**: ASP.NET Core Web API
- **Datenbank**: SQLite (automatisch erzeugt über `EnsureCreated()`)
- **Auth**: ASP.NET Core Identity mit JWT
- **API**: Pexels (Bildvorschläge)
- **Tooling**: Visual Studio, Swagger (API-Dokumentation) -- Swagger kann erreicht werden über
  das ergänzen von `/swagger` am Ende der URL. Bsp. https://localhost:7016/swagger/index.html.

## Aufbau als Screenshots
**Startseite - Nicht eingeloggter Zustand:**  
Im Nicht eingeloggten Zustand gibt es im NavMenu zunächst nur den Home Button zu sehen.  
Hier kann man sich entweder mit einem neuen Account registrieren, mit einer E-Mail und  
Passwort oder sich mit einem der bereits angelegten Demo-Accounts direkt einloggen. 

![Home_Unauthorized](https://github.com/user-attachments/assets/4a9f69d9-81f8-454c-b0d1-40ce05961904)

![Login](https://github.com/user-attachments/assets/cd996481-598f-487b-b93d-d38016c6d2a1)

**Startseite - eingeloggter Zustand:**  
Ist man mit einem User eingeloggt, so ändert sich die Ansicht. Im NavMenu hat man hier  
neben dem Home Button nun die Möglichkeit, weitere Artikel hochzuladen oder über den  
Button `Meine Verläufe` alle bereits hochgeladenen Artikel des aktuell eingeloggten  
Users einzusehen. Eine Besonderheit hier ist, das der eingeloggte User im Home-Bereich  
nur Artikel anderer User sieht. Unter `Meine Verläufe` hat er darüber hinaus noch eine  
Ansicht, wie viele User seine jeweils hochgeladenen Artikel angeschaut haben.  

![LoginHome](https://github.com/user-attachments/assets/da6355a7-deb6-49fb-9993-fb667ce54349)

![Meine Verläufe](https://github.com/user-attachments/assets/2d02d513-ea2c-4183-9283-b3494ddb34b7)

**Artikel hochladen:**  
Möchte der User einen Artikel hochladen, so müssen alle Felder einer Kategorie ausgefüllt werden.  
In dieser Screenshot Dokumentation ist der API-Key von Pexels im `PexelsImageService` eingefügt.  
Nach der Eingabe eines Artikelnamens erscheint ein Button, welcher über die API eine Auswahl  
verschiedener Bilder zu dem eingegebenen Namen ermöglicht. 

![ArtikelUpload](https://github.com/user-attachments/assets/6876b3e3-8198-4e8e-8e1e-9f0dc1ca56fc)

![Bildersuche_Mit API-Key](https://github.com/user-attachments/assets/7caaf287-5cc0-41f4-b206-82fcd7d55552)

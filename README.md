# LearninChem - VR Laboratorium Training

Dit project is ontwikkeld als applicatie voor een bachelorproef Multimedia en Creatieve Technology. LearninChem is ontworpen voor de Meta Quest 2 en 3 en biedt een veilige virtuele omgeving om alledaagse laboratoriumprocedures te oefenen. De simulatie wijst zichzelf grotendeels uit dankzij intuïtieve in-gamebegeleiding en interactieve dashboards.

## 🧪 Trainingsmodules

De applicatie bevat momenteel drie interactieve modules:

- **Module 1: Persoonlijke Beschermingsmiddelen (PBM's)**
  Leer welke veiligheidsuitrusting (visie, hand en lichaamsbescherming) vereist is in een lab en waartegen deze beschermt.
- **Module 2: Titratieprocedure**
  Oefentitratie met een buret aan de blauwe werktafel. Voeg op tijd een indicator toe met de pipet en sluit de kraan nauwkeurig bij de roze kleuromslag om overtitratie te voorkomen.
- **Module 3: Kalk & Geleidbaarheid**
  Aan de gele werktafel leer je afwegen en verdunnen. Plaats de maatbeker, gebruik de tarreknop, voeg kalkpoeder en water toe en voer de gemeten geleidbaarheid (µS/cm) in via het numerieke UI toetsenbord.

## 🎮 Besturing (Meta Quest)

De interactie voelt natuurlijk aan met behulp van de standaard VR controllers:

- **Vastpakken (Grabbables)**: Gebruik de gripknop aan de zijkant om objecten zoals de pipet en maatbeker vast te pakken.
- **Knoppen aanraken**: Gebruik de klikknop (bovenaan de controller) om UI knoppen in de virtuele wereld te selecteren.
- **Bewegen**: Gebruik de joysticks voor voortbeweging en camerapositie.

---

## ⚙️ Technische Architectuur

LearninChem is een standalone lokaal systeem (Client Only) zonder externe SQL of NoSQL database. De applicatie maakt gebruik van een efficiënte MVC achtige structuur:

- **Data**: `ScriptableObjects` fungeren als in memory database voor stappenplannen en instructies.
- **View**: Ontwikkeld met Unity UI Toolkit (UXML/USS) voor scherpe en schaalbare interfaces.
- **Logic**: Aangestuurd door centrale C# managers, waarbij de sessiestatus en waarschuwingen lokaal in het werkgeheugen worden opgeslagen en gevalideerd.

## 🛠️ Installatie & Ontwikkeling

**Link APK bestand LearninChem**
https://drive.google.com/drive/folders/1ZzgFMJY7YAKWz_sSj5s8wtKuAwdSIrNf?usp=sharing

**Systeemvereisten:**

- Game Engine: **Unity 6** (versie 6000.3).
- IDE: **Visual Studio 2022** of VS Code (.NET ondersteuning).
- Vereiste Unity modules: _Android Build Support_, _OpenJDK_, _Android SDK & NDK Tools_.

**Projectconfiguratie (Editor Settings):**

- **XR Plug in Management**: Zorg dat _Oculus_ is aangevinkt (Android) en _OpenXR/Oculus_ voor PC testen.
- **Player Settings (Android)**: Color Space op _Linear_, Minimum API Level op _Android 12.0 (API Level 32)_, en Texture Compression Format op _ASTC_.

### ⚠️ UI Toolkit Restricties

Bij het aanpassen van UXML bestanden voor nieuwe dashboards gelden strikte regels om de gevreesde _Trying to read value of type Color..._ Unity error te voorkomen:

1. Gebruik **uitsluitend HEX kleurcodes** (bijv. `#FFD500` of transparant `#f1c40f33`). Vermijd het gebruik van `rgb()` of `rgba()`.
2. Vermijd CSS shorthands voor randen (gebruik altijd losse properties zoals `border-left-width`, `border-left-color`).

## 📂 Kernscripts Overzicht

- `UniversalProcedureManager.cs`: Beheert de algehele logica, leest de ScriptableObjects uit voor de UI, toont pop ups (foutmeldingen) en valideert de modulestatussen.
- `MainMenuController.cs`: Beheert de interacties in het hoofdmenu en de opstartsequentie van de modules.
- `WorkstationDashboard.cs`: Handelt de lokale UI updates af op de fysieke schermen bij de werktafels.
- `DashboardInputValidator.cs`: Valideert de numerieke invoer van de speler.
- `TitrationController.cs` & `BuretValve.cs`: Verantwoordelijk voor visuele timers, kleurovergangen (Lerp) en fail states van Module 2.
- `BeakerVisuals.cs` & `LiquidTriggerZone.cs`: Beheert visuele statussen (Module 3) en detecteert fysieke collisies met anti spam cooldowns (`Time.time`).

<div align="center">
  <img src="StressNull/Resources/AppIcon/appicon.svg" alt="StressNull Logo" width="150"/>
  <h1>StressNull</h1>
  <p><b>Your Digital Stress-Ball in a Fast-Paced World.</b></p>
</div>

---

## 🛑 About StressNull

**StressNull** is a multi-platform mobile application conceptualized and designed to help you relieve stress, defeat burnout, and take a mental break through a series of chaotic, lighthearted mini-games and interactive visual novelties. 

When everything is too much, StressNull serves as your digital stress-ball—providing instant access to satisfying, humorous, and engaging activities to improve your mood and offer a quick escape from daily pressures.

## 🧠 Features & "Therapy" Modules

- 🐈 **Popcat**: A tap-based mini-game featuring the iconic "Popcat" internet meme with accompanying satisfying audio effects. How high can you score?
- 🎭 **Brainrot**: A humorous, meme-centric page tailored for quick, absurd amusement when your brain just needs to shut off.
- 🐼 **Panda**: An interactive visual toy featuring multiple stages of a stress-relief panda.
- 📜 **Baroque**: A sophisticated venting module that responds to your frustrations with dramatic classical instrumentation and archaic, validating affirmations.
- 🗄️ **Bureaucracy**: A satirical module mimicking complex red tape. Suffer through fake form-submissions, fax delays, bureaucratic hurdles, and get a concluding virtual stamp of denial!
- 🤬 **Scream Log**: A simulated decibel meter allowing you to digitally "scream" to register your stress intensity and receive humorous feedback.
- 📓 **Wreck-It Journal**: An ephemeral daily journal that automatically clears its contents each day, providing a secure, temporary space to vent without a trace.
- 🔮 **Unsolicited Advice**: A comedic feature offering random, satirical, or slightly absurd life advice.
- 🏆 **Global Leaderboards**: Log in, save your scores, and compete to see who is the most "brainrotted" among all players globally!

## 🛠️ Tech Stack
- **Frontend**: Built entirely with cross-platform **.NET MAUI** (C#, XAML). Supports Android, iOS, Windows, and macOS.
- **Backend**: A custom secure **ASP.NET Core REST API** handling user authentication (JWT) and high-score leaderboards.
- **Database**: Entity Framework Core with PostgreSQL.
- **Media**: Scalable Vector Graphics (SVGs) and interactive Audio for a polished multimedia experience.

## 🚀 Installation (For Developers)

1. Clone the repository.
2. Ensure you have the `.NET 9 SDK` and the `MAUI` workload installed.
3. Start the API locally:
   ```bash
   cd StressNull.Api
   dotnet run --launch-profile http
   ```
4. Update `MauiProgram.cs` to point to your local API IP address.
5. Run the mobile application via Visual Studio / Rider / CLI:
   ```bash
   cd StressNull
   dotnet build -t:Run -f net9.0-android
   ```

---
*Created in partial fulfillment of the requirements for Mobile Development, University of San Carlos (May 2026).*

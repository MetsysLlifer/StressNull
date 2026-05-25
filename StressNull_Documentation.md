# StressNull Documentation

*(Note: When porting to Word, please set paper size to 8.5” x 11”, font to Arial 12-point, 1.5 line spacing, Left margin 1.5", Top/Bottom/Right margins 1", and add page numbers to the bottom right.)*

<div style="text-align: center; margin-top: 50px; margin-bottom: 100px;">
    <h2>STRESSNULL</h2>
    <br><br><br>
    <p>A Mobile Development Project</p>
    <p>Presented to the Faculty of the</p>
    <p>Department of Computer, Information Sciences and Mathematics</p>
    <p>University of San Carlos</p>
    <br><br><br>
    <p>In Partial Fulfillment</p>
    <p>of the Requirements for the</p>
    <p>Mobile Development</p>
    <br><br><br>
    <p>By</p>
    <p>[NAME 1]</p>
    <p>[NAME 2]</p>
    <p>[NAME 3]</p>
    <br><br><br>
    <p>CHRIS RAY B. BELARMINO</p>
    <p>Instructor</p>
    <br><br><br>
    <p>May 2026</p>
</div>

<div style="page-break-before: always;"></div>

## RATIONALE
StressNull is a mobile application conceptualized and designed to help users relieve stress and take short, entertaining mental breaks through a series of lighthearted mini-games and interactive visual novelties. In a fast-paced world, individuals frequently experience burnout. StressNull serves as a digital stress-ball, providing instant access to satisfying, humorous, and engaging activities to improve mood and offer a quick escape from daily pressures.

## SYSTEM DESCRIPTION
The StressNull system is a cross-platform mobile application utilizing a modern user interface, backed by a custom cloud API. It features a main menu allowing users to navigate between various interactive modules, such as:
- **Brainrot Page**: A humorous, meme-centric page tailored for quick amusement.
- **Panda Page**: An interactive visual featuring multiple stages of a stress-relief panda.
- **Popcat Page**: A tap-based mini-game featuring the popular "Popcat" internet meme with accompanying audio effects.
- **Play Page**: Additional mini-games focused on simple, satisfying mechanics.
- **Baroque Page**: A sophisticated venting module that responds to user frustrations with dramatic classical instrumentation and archaic, validating affirmations.
- **Bureaucracy Page**: A satirical module mimicking complex red tape, featuring a fake form-submission process with fax delays, bureaucratic hurdles, and a concluding virtual stamp.
- **Scream Log Page**: A simulated decibel meter allowing users to digitally "scream" to register their stress intensity and receive humorous feedback.
- **Unsolicited Advice Page**: A comedic feature offering random, satirical, or slightly absurd life advice.
- **Wreck-It Journal Page**: An ephemeral daily journal that automatically clears its contents each day, providing a secure, temporary space to vent.
- **Leaderboard Page (Upcoming)**: A competitive module allowing users to view and submit their scores from mini-games to see who is the most "brainrotted" among all players globally.
- **Authentication System (Upcoming)**: Sign-up and Login pages allowing users to securely create accounts and save their brainrot scores to the global database.
- **Settings Page**: A configurable interface for users to toggle features like sound effects, volume, and visual themes.
- **About Page**: Provides basic application information and credits.

The app relies on scalable vector graphics (SVGs), custom fonts, and audio playback to deliver a polished multimedia experience on both Android and iOS devices. The backend is powered by a secure RESTful API that handles user authentication and leaderboard data storage.

## TOOLS USED
- **.NET MAUI (Multi-platform App UI)**: The primary framework used for cross-platform mobile application development.
- **ASP.NET Core Web API (.NET 9)**: The framework used to build the custom backend RESTful API for authentication and the leaderboard.
- **PostgreSQL**: The relational database used to securely store user accounts and high scores.
- **Entity Framework Core**: The Object-Relational Mapper (ORM) used to interact with the PostgreSQL database.
- **JSON Web Tokens (JWT) & BCrypt**: Used for secure user authentication, session management, and password hashing.
- **C#**: The backend programming language utilized for application logic and event handling.
- **XAML (Extensible Application Markup Language)**: Used for designing the user interface and page layouts.
- **Rider / Visual Studio**: The Integrated Development Environment (IDE) used for coding, building, and debugging the application.
- **Android SDK & iOS SDK**: For building and packaging the application for respective mobile platforms.
- **SVG Graphics**: Utilized for resolution-independent images and icons.

## SYSTEM SCREENSHOTS
*(Please insert screenshots of the Main Menu, Brainrot Page, Panda Page, Popcat Page, Play Page, Baroque Page, Bureaucracy Page, Scream Log Page, Unsolicited Advice Page, Wreck-It Journal Page, Leaderboard Page, Login Page, Register Page, Settings Page, and About Page here. Ensure figure captions are single-spaced.)*

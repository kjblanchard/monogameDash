
# Supergoon Dash - Monogame

Wanted to start back on some 2d monogame projects as they seem to be the most fun for me, so to update my engine code, going to make a geometry dash clone with monogame

WASD to move
Space to jump
Once you start moving, you won't be able to stop.  Coins will increase your max speed.

![Picture didn't load idiot](https://github.com/kjblanchard/monogameDash/blob/master/img/gif.gif?raw=true)

Most of the actual code is in the SupergoonEngine project.  Then the actual game specific logic (very slim currently) is in the supergoondashcrossplatform project.  The Desktop project has no actual code really, but it will hold some of the build specific dependencies.

Tiled And FMOD build into the desktop project, and that is copied over (does not use the Monogame Content pipeline)
M1 support in monogame in 3.8 is not ready, supposed to be released with monogame 3.9 (for development and building)

- SupergoonEngine - Classes that could possibly be reused between Monogame developed games, this is a shared project (not really supported anymore in C#) so I've installed the libraries in Cross platform, this should be moved in the future. <br>
- SupergoonDashCrossPlatform - Most of the game logic, this can be used as a resource for a specific Platform <br>
- SupergoonDashDesktop - Desktop build, can run in Windows, MacOS(intel) and Linux


### [Trello Board](https://trello.com/b/mirFjXRE/geometry-dash-board)

## Screenshots

**Uses Annotations to create ImGui debug windows dynamically**

![Picture didn't load idiot](https://github.com/kjblanchard/monogameDash/blob/master/img/debugIde.png?raw=true)
![Picture didn't load idiot](https://github.com/kjblanchard/monogameDash/blob/master/img/debugGame.png?raw=true)


**Uses Tiled to load box colliders, and all tiles / actors**
![Picture didn't load idiot](https://github.com/kjblanchard/monogameDash/blob/master/img/tiled.png?raw=true)

**Uses Aseprite data to load animations and frametime from animation tags**
![Picture didn't load idiot](https://github.com/kjblanchard/monogameDash/blob/master/img/aseprite.png?raw=true)
## Tech Stack

**Frameworks:** Monogame (XNA)

**Libraries:** FMOD - Sound, TiledCS - Loading tiledmaps, Dear ImGUI - Dev Debugging, Monogame.Aseprite - load aseprite file in content pipeline





## License

Assets 
Kings and Pigs Tileset
[Creative Commons License](https://pixelfrog-assets.itch.io/kings-and-pigs)

Platformer Tileset
[Creative Commons License](https://erayzesen.itch.io/pixel-platformer)

Coin Sound
[Creative Commons License](https://freesound.org/people/bradwesson/sounds/135936/)

Jump Sound
[Creative Commons License](https://freesound.org/people/se2001/sounds/528568/)

Death Sound
[Creative Commons License](https://freesound.org/people/ProjectsU012/sounds/334266/)

## Releases
 - [v0.1.0](tba) June 11 - 2022 - 45 hours of code time.
   ![Picture didn't load idiot](https://github.com/kjblanchard/monogameDash/releases/tag/v0.1.0)

## Authors

- [@Kevin Blanchard](https://www.github.com/kjblanchard)

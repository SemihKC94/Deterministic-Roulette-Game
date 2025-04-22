<h1 align="center" id="title">Deterministic Roulette</h1>

<p id="description">This is a deterministic version of the classic roulette game. In this implementation players have the ability to choose the winning number prior to initiating the spin.</p>

<h2>Project Screenshots:</h2>

<img src="https://gcdnb.pbrd.co/images/nVSO4ZdTsIHB.png?o=1" alt="project-screenshot" width="360" height="640/">

<img src="https://gcdnb.pbrd.co/images/8nSTGiUvDd9w.png?o=1" alt="project-screenshot" width="360" height="640/">

<img src="https://gcdnb.pbrd.co/images/e5rCoULw8q4Z.png?o=1" alt="project-screenshot" width="360" height="640/">

<img src="https://gcdnb.pbrd.co/images/vMN4a0jtA5fA.png?o=1" alt="project-screenshot" width="360" height="640/">

<img src="https://gcdnb.pbrd.co/images/nx7r87LD05MP.png?o=1" alt="project-screenshot" width="360" height="640/">

<h2>Project Video:</h2>

[![Watch the GamePlay](https://gcdnb.pbrd.co/images/nVSO4ZdTsIHB.png?o=1)](https://www.youtube.com/watch?v=T6Nrr4R5Lyw)

<h2>🧐 About Game</h2>

This project is a comprehensive implementation of a classic roulette game, incorporating all standard features and betting options. Notably, it includes a deterministic mode, allowing players to pre-select the winning number. The game meticulously tracks player statistics, including spin count, wins, losses, total winnings, and total losses, all of which are accessible via a dedicated statistics interface.  Furthermore, player progress is persistently stored using a custom-designed save system.

For development and testing purposes, a cheat menu is integrated, providing the ability to set the winning number and adjust in-game currency.  A utility is also provided to delete the current save data, accessible through the SKC/Delete Save option in the application's menu.

In the interest of efficiency and control, the project minimizes the use of third-party tools, avoiding libraries such as Dotween. While I acknowledge that the game's visual presentation could be enhanced, my primary focus as a game developer has been on core functionality and architecture.  Investing additional time in visual polish would have significantly extended the development timeline.  I believe that a game developer's time is a valuable resource that must be carefully managed alongside other project constraints.
  
  
<h2>💻 About Code</h2>

About OOP and SOLID:

*   In developing the game's architecture I prioritized object-oriented principles to achieve modularity and maintainability. For managing persistent data I employed encapsulation by grouping save/load operations and the data structure itself. This approach allows for clear separation of concerns hiding the data access details and providing a controlled interface. Furthermore the use of generics in the object pooling system significantly enhanced code reusability. This design allows for pooling of various object types without code duplication effectively adhering to the Open/Closed Principle.
*   In summary while OOP principles like encapsulation and abstraction were applied there's a need for stronger adherence to SOLID principles. Specifically refactoring the event management and utility function systems with a focus on Single Responsibility Open/Closed and Dependency Inversion would result in a more robust and adaptable game architecture.

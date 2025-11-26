Devin Puckett

CST-250 Programming in C#2

Grand Canyon University

11/11/2025

Milestone 4

[https://github.com/devinpuckett4/CST-250-Milestone-4/blob/main/Milestone4.md]

https://www.loom.com/share/12290469f75044d9a0e4603a30cd6b4c














FLOW CHART

  
Figure 1: Flow chart of Activity 1

This flowchart shows the path my console app takes from start to finish. First, I make a small board and set the difficulty, then I place the bombs and count the numbers around each square, and I print the board with B, numbers, and dots plus labels and borders. I repeat the same steps for a larger board to prove it scales. After the boards print, I ran a couple of simple tests that check the neighbor counting and that the bomb percentage looks reasonable. Then the program ends.






UML Class Diagram
  
Figure 2: UML Class Diagram

This diagram shows how I split my project into simple parts. BoardModel and CellModel just hold the data for the game, and BoardModel owns the grid of cells. GameState is a small list that says if the game is in progress, won, or lost. The game rules live in BoardService, and it follows the IBoardOperations interface so I can call it the same way from my app. The Program class is the console app that creates a board, calls the interface to set it up, and then prints the results.








Low Fidelity

  
 
Figure 3: Screenshot of Build Success

This screenshot shows that my Minesweeper solution builds successfully with all layers working together. The Models project compiles first, confirming that my core data structures are set up correctly. The BLL project succeeds next, which means my game logic and rules are recognized and error free. Both the ConsoleApp and WinForms projects build without issues, so I can run and demonstrate the game from either interface. Finally, the Tests project also succeeds, showing that my unit tests are in place and helping verify that the different parts of the application behave the way they are supposed to.












High Fidelity
 

Figure 4: High Fidelity

 
This screenshot shows my Minesweeper projects building successfully and all unit tests passing. The Models, BLL, and Tests projects compile without errors, confirming that the core game logic and data structures are wired up correctly. xUnit automatically discovers the Minesweeper.Tests project, runs the tests, and reports that each one finishes successfully. The test summary shows four tests total with zero failures or skips, which backs up that my logic behaves the way I intended. Overall, this output is proof that the solution is stable, testable, and ready to demonstrate.



 
Figure 5: Screenshot of Console Running

 
This screenshot shows the console-based version of my Minesweeper game displaying the “Answer Key” cheat view. It prints the full 10x10 board with numbers, bombs marked with B, and dots for empty spaces so I can verify that bombs and counts are placed correctly. The grid is labeled with row and column indexes across the top and left side, which makes it easier to match positions during testing. This view is only for debugging and validation, not for normal players, but it helped me confirm my logic before building the full gameplay. At the bottom, the prompt to press Enter to start gameplay shows that once I’m done checking the board, the program continues into the regular Minesweeper experience.
Figure 6: Screenshot of Setup Form

 
This screenshot shows my Minesweeper setup form where the player chooses how the game will start. The top slider controls the board size and is currently set to a 10 x 10 grid, which is shown clearly on the right so the player knows exactly what they’re picking. The second slider sets the difficulty level, starting at 1 for an easier game and scaling up for more bombs as the player wants more of a challenge. The layout is simple and clean so it’s easy to adjust both settings without confusion. Once the player is happy with their choices, they click Start Game to generate the board and jump straight into gameplay.









Figure 7: Screenshot of Main Form Running

 

This screenshot shows the main Minesweeper game board after the player starts a 10x10 game. Every cell begins as a gray button with a question mark, clearly signaling that it’s hidden and waiting to be revealed. The message at the top reminds the player of the simple controls: left-click to reveal a cell and right-click to place a flag on suspected bombs. At the bottom, there’s a Peeks counter showing how many safe “peek” uses are available, along with Use and Close buttons to control that feature. Overall, the layout is clean, readable, and focused on giving the player clear instructions and a fair starting point.



 Figure 8: Screenshot of In Visit and Fill

 
This screenshot shows my Minesweeper game in action after I’ve started revealing cells. The gray buttons with question marks are still hidden tiles, while the white tiles with numbers show how many bombs are touching that spot. You can see how the flood fill opened up a safe section automatically, which makes it easier to plan the next moves instead of guessing blindly. The instructions at the top keep it simple, reminding players to left-click to reveal and right-click to flag suspected bombs. At the bottom, the Peeks counter and buttons are ready so the player can use their limited peek ability if they get stuck. 





Figure 9: Screenshot of Loss

 
This screenshot shows the lose state of my Minesweeper game after I clicked on a bomb. The top message clearly says “Boom! You lost” and displays my final time in seconds so I can see how long the round lasted. All of the bombs are revealed with red tiles and bomb icons to make it obvious where they were hiding and what went wrong. The remaining tiles stay visible but can’t be interacted with anymore, which prevents accidental clicks after the game is over. At the bottom, the peek button is disabled and only the Close button is available, guiding the player to end the round and start fresh if they want to try again.



Figure 10:  Display Smaller Board and Flagged Cell
 

This screenshot shows my Minesweeper game during normal gameplay with a mix of revealed numbers, hidden tiles, and a flagged bomb. The yellow flag marker makes it easy to keep track of a suspected mine without accidentally clicking it. Around the flag, the numbered tiles help me read the board and make smarter choices instead of guessing. The bottom of the screen still shows I have one peek available, along with the Use and Close buttons so I can decide whether to spend that bonus or keep it for later. Overall, it captures how the game balances strategy, visual clarity, and a little bit of tension as I work toward clearing the board safely.





Figyure 11: Display Special Hint Rewardc
 

This screenshot shows the special reward feature that I added to my Minesweeper game. During gameplay, hitting a certain safe cell triggers a pop-up message titled “Reward Found!” so the player knows they unlocked a bonus. The dialog clearly explains that this reward grants a safe hint, allowing the player to peek at one hidden cell without risking a bomb. This mechanic adds a fun twist to classic Minesweeper by giving players a small safety net and encouraging them to keep exploring the board. It also highlights that my game isn’t just functional, but includes extra features that support strategy and player engagement.





Figure 12 : Display Win Time
  
This screenshot shows my Minesweeper game mid-round with the timer displayed at the top so I can see how quickly I’m playing. A small section of the board has been revealed with numbers and a few flags, which confirms I’m using logic instead of randomly guessing. Having the time visible adds pressure but also makes it easier to compare different runs for scoring and improvement. The Peeks counter shows I still have two safe hints available, and the Use button is disabled here to avoid accidental clicks. The simple layout keeps everything focused on the board, the timer, and smart decision-making.
Figure 13: Display Win Score 
 

This screenshot shows the win screen from my Minesweeper game after clearing a 6x6 board successfully. The message at the top, “You won! Score: 3885,” gives clear feedback and celebrates the result so it feels rewarding, not just finished. All bombs are correctly marked with flags, which confirms my decisions were accurate and the game logic recognized a true win. The Peeks display shows I finished with two hints still available, which ties into the scoring and encourages smart, efficient play. The Use button is disabled and only Close is active, guiding the player to wrap up the round and either exit or start another game.


---
Use Case Scenario
---

 
This diagram maps the main interactions in my Minesweeper app across three actors: User, System, and Administrator. On the User Lane, the flow goes from starting the game to viewing the board, entering moves (left-click reveal, right-click flag), and then feeding into Determine_Gamestate, which leads to a Win or Lose screen with the score. The System Lane shows the engine work, visiting cells, flagging, and triggering the peek reward when it’s earned. The Administrator Lane captures configuration and quality control: setting the board size, setting the difficulty percentage, and running xUnit tests to keep everything reliable. Together, it shows who does what and how each action rolls up to the final game result.





ADD ON
Programming Conventions

Keep files grouped by what they do. Models hold the data, BLL handles the game rules, and the WinForms app shows the board and reads input.

Name things clearly and add small comments so future me knows what a method is for.

The BLL is the brain. The models are storage. The UI only displays and reads from the player.

Keep methods focused on one job. RevealCell now handles regular visits and calls flood fill when needed. Other methods include SetupBombs, CountBombsNearby, ToggleFlag, UseRewardPeek, and DetermineGameState.

Check for out-of-bounds or bad input first, then always guide the player with a friendly message.

Computer Specs
• Windows 10 or Windows 11
• Visual Studio 2022
• .NET SDK installed
• 8 GB RAM or more
• Git and GitHub account

Work Log Milestone 4
Wednesday
• 4:00–5:15 PM - Started GUI setup with FormStart and trackbars for size/difficulty
Total: 1h 15m

Wednesday - Discussion Day
• Posted about WinForms GUI integration and event handling for clicks
Total: 40m

Friday - Discussion Day
• Posted about challenges with dynamic button grids in WinForms
Total: 40m

Friday
• 6:15–7:30 PM - Added FormStart for size/difficulty input and FormGame for button grid
• 7:40–8:30 PM - Implemented left-click reveal, right-click flag, and refresh logic
Total: 1h 45m

Saturday
• 10:00–11:15 AM - Updated UML and flowchart to show GUI forms and connections
• 11:30–12:40 PM - Added score calculation on win and disabled grid
Total: 2h 25m

Saturday - Discussion Day
• 1:00–1:35 PM - Discussion replies about GUI vs console for user experience
Total: 35m

Sunday
• 5:20–6:45 PM - Cleaned comments, retook screenshots, fixed alignment in console
• 7:00–7:40 PM - Reviewed build and final test run with passes
Total: 2h 05m

Grand Total: 9h 20m

What I added
• FormStart for choosing size and difficulty with trackbars
• FormGame with dynamic button grid, click events for reveal/flag, flood fill on zeros
• Score display on win, grid disable, timer stop
• Updated UML/flowchart to include GUI forms

Research

Facebook

Candy Crush Saga

Match-3 Puzzle

Fun but repetitive; easy at first, hard later.

Bright and colorful, attractive.

Use matching mechanics for a future puzzle mode in Minesweeper.

OOP principles I used
• Abstraction - The interface describes what the board service can do
• Encapsulation - Data stays in the board and cell classes and the BLL changes it
• Polymorphism - Event handlers use the same click method for different actions
• Inheritance - Forms inherit from base Form class for UI

Tests
• Three tests passed: GUI reveal, flood fill expansion, and score calculation

Bug Report
• Buttons cutoff on large boards, fixed by scaling cell size
• Win not disabling clicks, added DisableAllCells method
• Timer kept running on win, fixed by stopping it in win block

Follow-Up Questions

What was tough? Getting the button grid to scale right for big boards without cutoff.

What did I learn? WinForms event handling makes mouse interactions smooth, and dynamic sizing keeps the UI clean.

How would I improve it? Add more difficulty levels or random reward placement.

How does this help at work? GUI skills make apps more user-friendly, and layering code helps team collaboration.

# Account Manager

This project aims to create a Chess Game program that features chess gameplay as well as account registration and stats tracking. The GUI will be created in WPF, the database will be stored using SQL. The game engine for the chess gameplay will be created in a separate repository and migrated over when ready. The main language for this project is C#, with some XAML for the GUI. The database will be queried using LINQ to SQL. Persistent save files will be stored using XML and queried using LINQ to XML.

Chess Game Engine (CGE) repository is found [here](https://github.com/jkchuong/LetsTryMakeAChessGame).

Kanban Board progress of each sprints can be found in the images folder [here](https://github.com/jkchuong/AccountManager/tree/main/Images).

**Definition of Done:** This project is considered to be complete when a user can register an account, log in, and play a full game of chess with ability to track user stats, moves, colour themes, and computer playstyle. If the user can again log in with the same details, this project will be marked as done.

## Sprint Progress

#### <u>Sprint 1</u>

The first sprint is spent creating the 3-layer model, comprising of a WPF GUI, an SQL database, and a business layer in between to help the two communicate. Additionally, construction on the CGE began.

- Created SQL database.
- Created simple log in, registration in WPF.
- Created business layer.
- Created necessary classes to define a chessboard and its piece.
- Created static rulebook class to store chess rules.

<u>Retrospect</u>

What went well: Creating Login/Registration system was simple, planning of the GUI helped progression.

What didn't went well: Lack of planning for the CGE meant the start was slow.

What to change next time: Ensure I have a clearer idea of how the software will proceed before writing code. 

#### <u>Sprint 2</u>

- Added dynamically generated game area with colour scheme drawn from database
- Completed ability to move pieces in CGE

<u>Retrospect</u>

What went well: CGE progress fast after planning was done. 

What didn't go well: Not keeping within the scope of the Sprint

What to change next time: Work only on tasks inside Sprint Log.

#### <u>Sprint 3</u>

- Completed CGE included AI moves
- CGE connected to application
- User wins and losses stored and shown
- User move history tracked and shown

<u>Retrospect</u>

What went well: Constructing CGE outside of solution was a good idea as it didn't interfere with the other parts of the program and features segregated.

What didn't go well: Not knowing how to store custom datatypes in SQL table

What to change next time: Lean serialisation better.

#### <u>Sprint 4</u>

- Added toggle for random AI moves, and aggressive AI moves.
- Added save files and temp files.
- Added images in place of letters.
- Added more themes and users.
- Optimised and implemented reusable code..
- Added King in Check detection.
- Added Pawn Promotion function.

<u>Retrospect</u>

What went well: CGE was constructed with scalability in mind, simple to implement additional features using previous existing methods.

What didn't go well: Finding out how to store persistent save files using LINQ to XML.

What to change next time: Take more breaks because I am very tired.



## **How To Use**

This program is very simple to use and fairly intuitive. Below the directions on how to use the Chess application is outlined.

#### <u>Login Page</u>

![Login Page](https://github.com/jkchuong/AccountManager/blob/main/Images/Login.png)

The log in page has two spaces for you to enter your username and password. Click on "LOGIN" to log in, if your details are incorrect, an error message will show. The "REGISTER" button takes you to the registration page.

#### <u>Registration Page</u>

![Registration Page](https://github.com/jkchuong/AccountManager/blob/main/Images/Registration.png)

The registration page allows you to register an account with the program.

- Name is where your name goes and will be how you are identified within the game.
- Username is what you will use to log in to the game and is your unique identifier. This cannot be changed later.
- Password and Repeat is where you type in your password; if the two are not the same, the registration will not work and an error message will be shown.
- "Back" takes you back to the login page.

#### <u>Game Page</u>

![Game Page](https://github.com/jkchuong/AccountManager/blob/main/Images/Game.png)

Given that you successfully register an account and log in with the correct details, you will be directed to the main game page. This is where you can play chess against the computer.

The largest section is the game section. This is where you will make your chess moves. You will always play as the white side in this program, so you will always have the first move. Clicking on one of the white pieces shows which moves you can make, as highlighted in yellow. In the above example, the "Knight" is able to move in an L shape.

The left section shows the moves made by you (White Moves) and by the computer (Black Moves) in algebraic notation, for example "Qxf5" means that a white queen has taken a piece that was located in square "f5". As the list extends, it can be scrolled up to view earlier moves if you wish to.

The right section shows the rankings of all the players that currently exist within the program, arranged by total number of wins. 

The bottom section contains four buttons and two text boxes. The upper text box contains your name, wins, and losses. This will update each time you complete a game. The lower text box contains important information such as if a king is in check or whether you have saved your game. The four buttons on the left have important functionality. If you forget the rules, the "Rules" button will toggle the rules so you may refresh your memory on how Chess works. "Save" will save the current state of your chess game, and it is safe to logout afterwards. "Logout" will return you to the log in page. "Settings" will take you to the settings page.

#### <u>Settings Page</u>

![Settings Page](https://github.com/jkchuong/AccountManager/blob/main/Images/Settings.png)

The final available page is the settings page. Here, you can update your information, password, and gameplay. Your name and password can be changed here, however your username is unique and cannot be changed.

- "Theme" provides a selection of colour schemes if you wish to jazz up your chessboard.
- "Aggressive AI" has a toggle that changes the computer's playstyle from doing random moves, to capturing your pieces every chance it gets. Be careful! Any piece you move, it *will* take.

Once you are happy with the changes, you can click on "Save Changes", or you can click on "Back" to navigate back to the game without changing your settings. Finally, the bright red button lets you delete your account. Keep in mind that deleting your account is permanent, if you wish to play again, you will be required to create another account.

## License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT). This license is conducive to free, open-source software.

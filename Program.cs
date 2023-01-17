// Connect 4 | Jordan A

#nullable disable

// VARIABLES

List<Tiles> gameTiles = new List<Tiles>{};
List<Tiles> rowTiles;

bool main = true;
bool gameRunning;

int curTile;
int curRow;
int curCol;

int width = 7;
int height = 6;

int[,] gameBoard;

int tileType;
bool redTurn;

int place;
int dropCol;

// MAIN LOOP
while (main)
{
    // Creates the game board
    Initialize(); 
    // Prompt a screen to start the game
    LaunchScreen(); 

    while (gameRunning)
    {
        // Draw the game board
        Render();
        // Update variables based on users inputs
        HandleInput();
        // Update tile information
        Update();
        // Check board for 4 in a row OR full board
        ContinueGame();
    }
}


// FUNCTIONS

void Initialize()
{
    redTurn = true;
    gameRunning = true;
    place = 0;
    tileType = 2;
    gameBoard = new int[height, width];
    dropCol = width/2;
    
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++) 
        {
            gameBoard[i, j] = 0;
        }
    }
    gameBoard[height - 1, width/2] = 3;
}

void LaunchScreen()
{
    Console.Clear();
    Console.WriteLine("WELCOME TO CONNECT 4");
    Console.WriteLine();
    Console.WriteLine("Get 4 in a row to win!");
    Console.WriteLine();
    Console.WriteLine("Press [enter] to start...");
    InputContinue();
}

void Render()
{
    Console.Clear();
    Console.BackgroundColor = ConsoleColor.Blue;
    Console.WriteLine("               ");
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++) 
        {
            if (gameBoard[i, j] == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;  
            }
            else if (gameBoard[i, j] == 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;  
            }
            else if (gameBoard[i, j] == 2)
            {
                Console.ForegroundColor = ConsoleColor.Red;  
            }
            else if (gameBoard[i, j] == 3)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (gameBoard[i, j] == 4)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else if (gameBoard[i, j] == 5)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            Console.Write(" ■", Console.ForegroundColor);
        }
        Console.Write(" ");
        Console.WriteLine();
    }
    Console.WriteLine("               ");
    if (redTurn)
    {
        Console.BackgroundColor = ConsoleColor.Red;
    }
    else
    {
        Console.BackgroundColor = ConsoleColor.Yellow;
    }
    Console.WriteLine("               ");
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.Gray;
    Console.WriteLine();
    Console.WriteLine("Use Arrow Keys to Move Drop");
    Console.WriteLine("Press [enter] to place");
}

void HandleInput()
{
GetInput:
    ConsoleKey key = Console.ReadKey(true).Key;
    switch (key)
    {
        case ConsoleKey.LeftArrow:
                if (dropCol > 0)
                {
                    dropCol -= 1;
                    place = 0;
                }
                break;
        case ConsoleKey.RightArrow:
                if (dropCol < 6)
                {
                    dropCol += 1;
                    place = 0;
                }
                break;
        case ConsoleKey.Enter:
                place = 1;
                break;
        case ConsoleKey.Escape:
                gameRunning = false;
                break;
        default: goto GetInput;
    }
}
    
void TileChanger()
{
    if (place == 1)
    {
        if (redTurn)
        {
            redTurn = false;
            tileType = 2;
        }
        else
        {
            redTurn = true;
            tileType = 1;
        }
    }
    else
    {
        tileType = 3;
    }
}

void Update()
{
    tileType = 3;
   for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++) 
        {
            if (gameBoard[i, j] == 3)
            {
                gameBoard[i, j] = 0; 
            }
        }
    }
    curTile = gameTiles.Count;
    gameTiles.Add(new Tiles(tileType, dropCol, 0));
    gameTiles[curTile].setRow(gameBoard);
    curRow = gameTiles[curTile].row;
    curCol = gameTiles[curTile].col;
    if (place == 1 && gameTiles[curTile].placeable == true)
    {
        TileChanger();
        gameBoard[curRow, curCol] = tileType;
        place = 0;
        try
        {
            gameBoard[curRow-1, curCol] = 3;
        }
        catch (IndexOutOfRangeException)
        {   
        }
    }
    else if (tileType == 3)
    {
        if (gameBoard[curRow, curCol] == 0)
        {
            gameBoard[curRow, curCol] = tileType;
        }
    }
    else
    {
        return;
    }
}

void ContinueGame()
{
    rowTiles = new List<Tiles>{};

    // HORIZONTAL
    for (int row = 0; row < 6; row++)
    {
        for (int col = 0; col < 4; col++)
        {
            if (gameBoard[row, col] == tileType && gameBoard[row, col+1] == tileType && gameBoard[row, col+2] == tileType && gameBoard[row,col+3] == tileType)
            {
                rowTiles.Add(new Tiles(tileType, col, row));
                rowTiles.Add(new Tiles(tileType, col+1, row));
                rowTiles.Add(new Tiles(tileType, col+2, row));
                rowTiles.Add(new Tiles(tileType, col+3, row));
                EndScreen(row, col, rowTiles);
                return;
            } 
        }
    }

    // VERTICAL
    for (int col = 0; col < 7; col++)
    {
        for (int row = 0; row < 3; row++)
        {
            if (gameBoard[row, col] == tileType && gameBoard[row+1, col]  == tileType && gameBoard[row+2, col] == tileType && gameBoard[row+3,col] == tileType)
            {
                rowTiles.Add(new Tiles(tileType, col, row));
                rowTiles.Add(new Tiles(tileType, col, row+1));
                rowTiles.Add(new Tiles(tileType, col, row+2));
                rowTiles.Add(new Tiles(tileType, col, row+3));
                EndScreen(row, col, rowTiles);
                return;
            }
        }
    }

    // ASCENDING DIAGONAL

    for (int row = 5; row > 2; row--)
    {
        for (int col = 0; col < 4; col++)
        {   
            if (gameBoard[row, col] == tileType && gameBoard[row-1, col+1] == tileType && gameBoard[row-2, col+2] == tileType && gameBoard[row-3,col+3] == tileType)
            {
                rowTiles.Add(new Tiles(tileType, col, row));
                rowTiles.Add(new Tiles(tileType, col+1, row-1));
                rowTiles.Add(new Tiles(tileType, col+2, row-2));
                rowTiles.Add(new Tiles(tileType, col+3, row-3));
                EndScreen(row, col, rowTiles);
                return;
            }
        }
    }

    // DESCENDING DIAGONAL
    for (int row = 0; row < 3; row++)
    {
        for (int col = 0; col < 4; col++)
        {   
            if (gameBoard[row, col] == tileType && gameBoard[row+1, col+1] == tileType && gameBoard[row+2, col+2] == tileType && gameBoard[row+3,col+3] == tileType)
            {
                rowTiles.Add(new Tiles(tileType, col, row));
                rowTiles.Add(new Tiles(tileType, col+1, row+1));
                rowTiles.Add(new Tiles(tileType, col+2, row+2));
                rowTiles.Add(new Tiles(tileType, col+3, row+3));
                EndScreen(row, col, rowTiles);
                return;
            }
        }
    }

    // FULL BOARD
    int fullRow = 0;
    for (int col = 0; col < 7; col++)
    {
        if (gameBoard[0, col] == 1 || gameBoard[0, col] == 2)
        {
            fullRow++;
        }
        else
        {
            fullRow = 0;
        }
    }
    
    if (fullRow == 7)
    {
        Console.Clear();
        Render();
        Console.WriteLine("DRAW");
        gameRunning = false;
        main = false;
    }
}

    

void EndScreen(int row, int col, List<Tiles> tiles)
{

    // Darken all tiles and remove the place marker
    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++) 
        {
            if (gameBoard[i, j] == 3)
            {
                gameBoard[i, j] = 0; 
            }
            if (gameBoard[i, j] == 2)
            {
                gameBoard[i, j] = 5;
            }
            if (gameBoard[i, j] == 1)
            {
                gameBoard[i, j] = 4;
            }
        }
    }

    for (int p = 0; p < 4; p++)
    {
        gameBoard[tiles[p].row, tiles[p].col] = tiles[p].type;
    }

    if (gameBoard[row, col] == 1)
    {
        Console.Clear();
        redTurn = false;
        Render();
        Console.WriteLine("Yellow WIN");
        gameRunning = false;  
        main = false; 
        
    }
    else if (gameBoard[row, col] == 2)
    {
        Console.Clear();
        redTurn = true;
        Render();
        Console.WriteLine("Red WIN");
        gameRunning = false; 
        main = false;
    }           
}


void InputContinue()
{
GetInput:
	ConsoleKey key = Console.ReadKey(true).Key;
	switch (key)
	{
		case ConsoleKey.Enter:
			break;
		case ConsoleKey.Escape:
			gameRunning = false;
            main = false;
			break;
		default: goto GetInput;
	}
}

// CLASSES
public class Tiles
{
    public int type;
    public int col;
    public int row;
    public bool placeable;

    public Tiles(int type, int col, int row)
    {
        this.type = type;
        this.col = col;
        this.row = row;
        this.placeable = false;
    }
    public void setRow(int[,] gameBoard)
    {
        for (int row = 5; row >= 0; row--)
        {
            if (gameBoard[row, col] == 0 || gameBoard[row, col] == 3)
            {
                this.row = row;
                this.placeable = true;
                break;
            }
        }

    }
}

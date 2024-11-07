namespace LiveGameAPI.Models
{
    public class GameState
    {
        public int[,] Board { get; set; } = new int[,]
        {   //for test
            { 0, 0, 0, 0, 0 },
            { 1, 0, 1, 0, 0 },
            { 0, 1, 0, 0, 1 },
            { 0, 0, 1, 0, 0 },
            { 0, 0, 0, 0, 1 }
        };

        //initialize 5 for test
        public int Rows { get; set; } = 5;
        //initialize 5 for test
        public int Columns { get; set; } = 5;
        public int CurrentTurn { get; set; } = 0;

        //flag for error
        public bool isError = false;

        public void initBoard(int[] board, int rows, int columns)
        {
            //initialize grid board
            //convert from one-dimentional array to two-dimentional
            Rows = rows;
            Columns = columns;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int index = i * columns + j; // Calculate the index in the one-dimensional array
                    if (index < board.Length)
                    {
                        Board[i, j] = board[index];
                    }
                }
            }
        }

        public GameState()
        {            
            
        }

        public int[] convertState()
        {
            //convert grid board to one-dimentional array for result
            int totalElements = Rows * Columns;
            int[] boardForResult = new int[totalElements];
            int index = 0;
            for (int i = 0; i < Rows; i++) // Loop through rows
            {
                for (int j = 0; j < Columns; j++) // Loop through columns
                {
                    boardForResult[index] = Board[i, j];
                    index++;
                }
            }
            return boardForResult;
        }

        public GameState nextGrid(int nTime = 1)
        {
            //calculate nTime number of states away for grid board
            int nNewRound = CurrentTurn + nTime;
            var nextGeneration = new int[Rows, Columns];
            while(CurrentTurn < nNewRound)
            {
                bool isChanged = false;
                // Loop through every cell 
                for (var row = 1; row < Rows - 1; row++)
                    for (var column = 1; column < Columns - 1; column++)
                    {
                        // find your alive neighbors
                        var aliveNeighbors = 0;
                        for (var i = -1; i <= 1; i++)
                        {
                            for (var j = -1; j <= 1; j++)
                            {
                                aliveNeighbors += Board[row + i, column + j] == 1 ? 1 : 0;
                            }
                        }

                        var currentCell = Board[row, column];

                        // The cell needs to be subtracted from its neighbors as it was counted before 
                        aliveNeighbors -= currentCell == 1 ? 1 : 0;

                        // Implementing the Rules of Life 

                        // Cell is lonely and dies 
                        if (currentCell == 1 && aliveNeighbors < 2)
                        {
                            nextGeneration[row, column] = 0;
                            isChanged = true;
                        }

                        // Cell dies due to over population 
                        else if (currentCell == 1 && aliveNeighbors > 3)
                        {
                            nextGeneration[row, column] = 0;
                            isChanged = true;
                        }

                        // A new cell is born 
                        else if (currentCell == 0 && aliveNeighbors == 3)
                        {
                            nextGeneration[row, column] = 1;
                            isChanged = true;
                        }
                        // stays the same
                        else
                        {
                            nextGeneration[row, column] = currentCell;
                        }
                    }
                //if grid board state is "block" - "final state"
                if (!isChanged)
                {
                    isError = true;
                    return this;
                }
                //udpate board state and initialize template array
                for(int i = 0; i < Rows - 1; i++)
                {
                    for(int j = 0; j < Columns - 1; j++)
                    {
                        Board[i, j] = nextGeneration[i, j];
                        nextGeneration[i, j] = 0;
                    }
                }
                CurrentTurn++;
            }
            isError = false;
            return this;
        }
    }
}
